using VideoCatalogue.Web.Models;

namespace VideoCatalogue.Web.Services
{
  public interface IVideoService
  {
    /// <summary>
    /// Saves the uploaded file stream to the server media folder.
    /// Overwrites the file if a file with the same name exists.
    /// </summary>
    /// <param name="file">The file provided by the HTTP request.</param>
    /// <returns>A Task representing the asynchronous save operation.</returns>
    Task SaveFileAsync(IFormFile file);

    /// <summary>
    /// Uploads the list of files (validation should be done at controller level).
    /// </summary>
    /// <param name="files">The list of validated files from the HTTP request.</param>
    Task UploadFilesAsync(List<IFormFile> files);

    /// <summary>
    /// Retrieves the list of all valid MP4 videos from the server media folder.
    /// </summary>
    /// <returns>A list of VideoItem objects.</returns>
    List<VideoItem> GetVideoList();
  }
}