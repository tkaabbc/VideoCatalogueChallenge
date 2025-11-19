namespace VideoCatalogue.Web.Services.Storage;

/// <summary>
/// Abstract interface for file storage operations
/// Supports multiple storage implementations: Local File System, AWS S3, Azure Blob, etc.
/// </summary>
public interface IStorageProvider
{
  /// <summary>
  /// Saves a file to storage
  /// </summary>
  /// <param name="fileName">The name of the file</param>
  /// <param name="fileStream">The file stream to save</param>
  /// <returns>A Task representing the asynchronous save operation</returns>
  Task SaveFileAsync(string fileName, Stream fileStream);

  /// <summary>
  /// Gets the list of all files matching the specified pattern
  /// </summary>
  /// <param name="pattern">File pattern (e.g., "*.mp4")</param>
  /// <returns>List of file information</returns>
  Task<List<StorageFileInfo>> GetFilesAsync(string pattern);

  /// <summary>
  /// Gets the URL/path to access a file
  /// </summary>
  /// <param name="fileName">The name of the file</param>
  /// <returns>The URL or path to access the file</returns>
  string GetFileUrl(string fileName);
}

/// <summary>
/// File information returned by storage provider
/// </summary>
public class StorageFileInfo
{
  public string FileName { get; set; } = string.Empty;
  public long SizeInBytes { get; set; }
  public DateTime LastModified { get; set; }
}
