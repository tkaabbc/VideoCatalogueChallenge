using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VideoCatalogue.Tests.Utils;
using VideoCatalogue.Web.Controllers;
using VideoCatalogue.Web.Models;
using VideoCatalogue.Web.Services;

namespace VideoCatalogue.Tests.Controllers;

public class VideoApiControllerTests
{
  [Fact]
  public async Task Upload_WhenModelStateIsInvalid_ReturnsBadRequest()
  {
    // Arrange
    var serviceMock = new Mock<IVideoService>();
    var controller = new VideoApiController(serviceMock.Object);
    controller.ModelState.AddModelError("files", "Files are required");

    var request = new VideoUploadRequest();

    // Act
    var result = await controller.Upload(request);

    // Assert
    Assert.IsType<BadRequestObjectResult>(result);
    serviceMock.Verify(s => s.UploadFilesAsync(It.IsAny<List<IFormFile>>()), Times.Never);
  }

  [Fact]
  public async Task Upload_WithValidRequest_CallsServiceAndReturnsOk()
  {
    // Arrange
    var serviceMock = new Mock<IVideoService>();
    serviceMock
        .Setup(s => s.UploadFilesAsync(It.IsAny<List<IFormFile>>()))
        .Returns(Task.CompletedTask);

    var controller = new VideoApiController(serviceMock.Object);
    var request = new VideoUploadRequest
    {
      Files = new List<IFormFile> { Helper.CreateFormFile("video.mp4") }
    };

    // Act
    var result = await controller.Upload(request);

    // Assert
    Assert.IsType<OkResult>(result);
    serviceMock.Verify(s => s.UploadFilesAsync(request.Files), Times.Once);
  }

  [Fact]
  public async Task GetCatalogue_ReturnsServiceResult()
  {
    // Arrange
    var expected = new List<VideoItem>
        {
            new() { FileName = "a.mp4", SizeInBytes = 10, Url = "/video/a.mp4" },
            new() { FileName = "b.mp4", SizeInBytes = 20, Url = "/video/b.mp4" },
        };

    var serviceMock = new Mock<IVideoService>();
    serviceMock
        .Setup(s => s.GetVideoListAsync())
        .ReturnsAsync(expected);

    var controller = new VideoApiController(serviceMock.Object);

    // Act
    var result = await controller.GetCatalogue();

    // Assert
    var okResult = Assert.IsType<ActionResult<List<VideoItem>>>(result);
    var value = Assert.IsType<List<VideoItem>>(okResult.Value);
    Assert.Equal(expected, value);
  }

}

