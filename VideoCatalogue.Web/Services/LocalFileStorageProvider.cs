using Microsoft.Extensions.Options;
using VideoCatalogue.Web.Configuration;

namespace VideoCatalogue.Web.Services.Storage;

/// <summary>
/// Local file system storage implementation
/// </summary>
public class LocalFileStorageProvider : IStorageProvider
{
  private readonly string _basePath;
  private readonly string _baseUrl;

  public LocalFileStorageProvider(
      IWebHostEnvironment webHostEnvironment,
      IOptions<StorageOptions> options)
  {
    _basePath = Path.Combine(webHostEnvironment.ContentRootPath, options.Value.LocalPath);
    _baseUrl = options.Value.BaseUrl;

    // Ensure directory exists
    Directory.CreateDirectory(_basePath);
  }

  public async Task SaveFileAsync(string fileName, Stream fileStream)
  {
    // Sanitize the file name to prevent path traversal attacks
    var safeFileName = Path.GetFileName(fileName);
    var filePath = Path.Combine(_basePath, safeFileName);

    using (var outputStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
    {
      await fileStream.CopyToAsync(outputStream);
    }
  }

  public Task<List<StorageFileInfo>> GetFilesAsync(string pattern)
  {
    if (!Directory.Exists(_basePath))
    {
      return Task.FromResult(new List<StorageFileInfo>());
    }

    var files = Directory.GetFiles(_basePath, pattern, SearchOption.TopDirectoryOnly);
    var fileInfos = files.Select(filePath =>
    {
      var fileInfo = new System.IO.FileInfo(filePath);
      return new StorageFileInfo
      {
        FileName = fileInfo.Name,
        SizeInBytes = fileInfo.Length,
        LastModified = fileInfo.LastWriteTime
      };
    }).ToList();

    return Task.FromResult(fileInfos);
  }

  public string GetFileUrl(string fileName)
  {
    return $"{_baseUrl}/{Path.GetFileName(fileName)}";
  }
}

