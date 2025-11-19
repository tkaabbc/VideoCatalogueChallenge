using System.Text;
using Microsoft.AspNetCore.Http;
using Moq;

namespace VideoCatalogue.Tests.Utils;

public static class Helper
{
  public static IFormFile CreateFormFile(string fileName, string content = "test-content")
  {
    var bytes = Encoding.UTF8.GetBytes(content);
    var fileMock = new Mock<IFormFile>();

    fileMock.Setup(f => f.FileName).Returns(fileName);
    fileMock.Setup(f => f.Length).Returns(bytes.Length);
    fileMock
        .Setup(f => f.OpenReadStream())
        .Returns(() => new MemoryStream(bytes, writable: false));

    return fileMock.Object;
  }
}

