using Microsoft.AspNetCore.Mvc;
using VideoCatalogue.Web.Common;
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
    public async Task<ActionResult> Upload([FromForm] VideoUploadRequest request)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      await _videoService.UploadFilesAsync(request.Files);
      return Ok();
    }

    [HttpGet("catalogue")]
    public ActionResult<List<VideoItem>> GetCatalogue()
    {
      var catalogue = _videoService.GetVideoList();
      return catalogue;
    }
  }
}