using VideoCatalogue.Web.Common;
using VideoCatalogue.Web.Exceptions;
using VideoCatalogue.Web.Models;

namespace VideoCatalogue.Web.Services
{
  public class VideoService : IVideoService
  {
    private readonly string _mediaPath;

    public VideoService(IWebHostEnvironment webHostEnvironment)
    {
      _mediaPath = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot/video");
    }

    public async Task SaveFileAsync(IFormFile file)
    {
      // Sanitize the file name to prevent path traversal attacks
      var safeFileName = Path.GetFileName(file.FileName);
      var filePath = Path.Combine(_mediaPath, safeFileName);

      using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
      {
        // FileMode.Create ensures that if the file exists, it is overwritten
        await file.CopyToAsync(stream);
      }
    }

    public async Task UploadFilesAndValidateAsync(List<IFormFile> files)
    {
      var errors = new List<string>();

      foreach (var file in files)
      {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension != FileExtensions.Mp4)
        {
          errors.Add(string.Format(ErrorMessages.InvalidVideoFormat, file.FileName));
          continue;
        }
      }

      if (errors.Any())
      {
        throw new VideoValidationException(string.Join("\n", errors));
      }

      foreach (var file in files)
      {
        await SaveFileAsync(file);
      }
    }
  }
}