using VideoCatalogue.Web.Common;
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

    public async Task UploadFilesAsync(List<IFormFile> files)
    {
      foreach (var file in files)
      {
        await SaveFileAsync(file);
      }
    }

    public List<VideoItem> GetVideoList()
    {
      if (!Directory.Exists(_mediaPath))
      {
        return new List<VideoItem>();
      }

      var files = Directory.GetFiles(_mediaPath, $"*{FileExtensions.Mp4}", SearchOption.TopDirectoryOnly);

      return files.Select(filePath =>
      {
        var fileInfo = new FileInfo(filePath);
        return new VideoItem
        {
          FileName = fileInfo.Name,
          SizeInBytes = fileInfo.Length,
        };
      })
      .OrderBy(v => v.FileName)
      .ToList();
    }
  }
}