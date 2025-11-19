using Microsoft.AspNetCore.Http;
using Moq;
using VideoCatalogue.Tests.Utils;
using VideoCatalogue.Web.Common;
using VideoCatalogue.Web.Models;
using VideoCatalogue.Web.Services;
using VideoCatalogue.Web.Services.Storage;

namespace VideoCatalogue.Tests.Services;

public class VideoServiceTests
{
  [Fact]
  public async Task SaveFileAsync_WhenFileNameContainsPath_TrimsAndDelegatesToStorage()
  {
    // Arrange
    var storageMock = new Mock<IStorageProvider>();
    string? capturedFileName = null;
    Stream? capturedStream = null;
    storageMock
        .Setup(s => s.SaveFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
        .Callback<string, Stream>((fileName, stream) =>
        {
          capturedFileName = fileName;
          capturedStream = stream;
        })
        .Returns(Task.CompletedTask);

    var service = new VideoService(storageMock.Object);
    var file = Helper.CreateFormFile("../evil/path/video.mp4", "test-content");

    // Act
    await service.SaveFileAsync(file);

    // Assert
    Assert.Equal("video.mp4", capturedFileName);
    Assert.NotNull(capturedStream);
    storageMock.Verify(
        s => s.SaveFileAsync(It.IsAny<string>(), It.IsAny<Stream>()),
        Times.Once);
  }

  [Fact]
  public async Task UploadFilesAsync_WithMultipleFiles_CallsSaveForEach()
  {
    // Arrange
    var storageMock = new Mock<IStorageProvider>();
    storageMock
        .Setup(s => s.SaveFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
        .Returns(Task.CompletedTask);

    var service = new VideoService(storageMock.Object);
    var files = new List<IFormFile>
        {
            Helper.CreateFormFile("first.mp4", "first"),
            Helper.CreateFormFile("second.mp4", "second")
        };

    // Act
    await service.UploadFilesAsync(files);

    // Assert
    storageMock.Verify(
        s => s.SaveFileAsync(It.IsAny<string>(), It.IsAny<Stream>()),
        Times.Exactly(files.Count));
  }

  [Fact]
  public async Task GetVideoListAsync_WhenStorageHasFiles_ReturnsOrderedItemsWithUrls()
  {
    // Arrange
    var storageMock = new Mock<IStorageProvider>();
    var storageFiles = new List<StorageFileInfo>
        {
            new() { FileName = "b_video.mp4", SizeInBytes = 2, LastModified = DateTime.UtcNow },
            new() { FileName = "a_video.mp4", SizeInBytes = 1, LastModified = DateTime.UtcNow },
        };

    storageMock
        .Setup(s => s.GetFilesAsync($"*{FileExtensions.Mp4}"))
        .ReturnsAsync(storageFiles);

    storageMock
        .Setup(s => s.GetFileUrl(It.IsAny<string>()))
        .Returns((string name) => $"/video/{name}");

    var service = new VideoService(storageMock.Object);

    // Act
    var result = await service.GetVideoListAsync();

    // Assert
    Assert.Collection(
        result,
        item =>
        {
          Assert.Equal("a_video.mp4", item.FileName);
          Assert.Equal(1, item.SizeInBytes);
          Assert.Equal("/video/a_video.mp4", item.Url);
        },
        item =>
        {
          Assert.Equal("b_video.mp4", item.FileName);
          Assert.Equal(2, item.SizeInBytes);
          Assert.Equal("/video/b_video.mp4", item.Url);
        });
  }

}

