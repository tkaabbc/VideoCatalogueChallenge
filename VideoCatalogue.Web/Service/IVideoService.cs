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
    /// Uploads and validates the list of files.
    /// </summary>
    /// <param name="files">The list of files provided by the HTTP request.</param>
    Task UploadFilesAndValidateAsync(List<IFormFile> files);
  }
}