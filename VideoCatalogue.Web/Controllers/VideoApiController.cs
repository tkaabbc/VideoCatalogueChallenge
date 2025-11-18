using Microsoft.AspNetCore.Mvc;
using VideoCatalogue.Web.Common;
using VideoCatalogue.Web.Exceptions;
using VideoCatalogue.Web.Models;
using VideoCatalogue.Web.Services;

namespace VideoCatalogue.Web.Controllers
{
  [Route("api/videos")]
  [ApiController]
  public class VideoApiController : ControllerBase
  {
    private readonly IVideoService _videoService;
    private const long MaxFileSize = 200 * Size.MB;

    public VideoApiController(IVideoService videoService)
    {
      _videoService = videoService;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(MaxFileSize)]
    public async Task<ActionResult> Upload([FromForm] List<IFormFile> files)
    {
      if (files == null || !files.Any())
      {
        return BadRequest(ErrorMessages.NoFilesSelected);
      }

      try
      {
        await _videoService.UploadFilesAndValidateAsync(files);
        return Ok();
      }
      catch (VideoValidationException e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}