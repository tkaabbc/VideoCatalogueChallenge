using VideoCatalogue.Web.Common;
using VideoCatalogue.Web.Models;
using VideoCatalogue.Web.Services.Storage;

namespace VideoCatalogue.Web.Services
{
  public class VideoService : IVideoService
  {
    private readonly IStorageProvider _storageProvider;

    public VideoService(IStorageProvider storageProvider)
    {
      _storageProvider = storageProvider;
    }

    public async Task SaveFileAsync(IFormFile file)
    {
      var safeFileName = Path.GetFileName(file.FileName);

      using (var stream = file.OpenReadStream())
      {
        await _storageProvider.SaveFileAsync(safeFileName, stream);
      }
    }

    public async Task UploadFilesAsync(List<IFormFile> files)
    {
      foreach (var file in files)
      {
        await SaveFileAsync(file);
      }
    }

    public async Task<List<VideoItem>> GetVideoListAsync()
    {
      var files = await _storageProvider.GetFilesAsync($"*{FileExtensions.Mp4}");

      return files.Select(fileInfo => new VideoItem
      {
        FileName = fileInfo.FileName,
        SizeInBytes = fileInfo.SizeInBytes,
        Url = _storageProvider.GetFileUrl(fileInfo.FileName),
      })
      .OrderBy(v => v.FileName)
      .ToList();
    }
  }
}