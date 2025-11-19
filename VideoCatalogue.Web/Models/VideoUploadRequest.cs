using Microsoft.AspNetCore.Mvc;

namespace VideoCatalogue.Web.Models;

/// <summary>
/// Request model for video upload
/// </summary>
public class VideoUploadRequest
{
  /// <summary>
  /// List of video files to upload
  /// </summary>
  public List<IFormFile> Files { get; set; } = new();
}

