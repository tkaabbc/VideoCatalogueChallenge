namespace VideoCatalogue.Web.Models
{
  public class VideoItem
  {
    /// <summary>
    /// The original file name of the video.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// The raw size of the file in bytes.
    /// </summary>
    public long SizeInBytes { get; set; }

    /// <summary>
    /// The URL of the video.
    /// </summary>
    public string Url { get; set; } = string.Empty;
  }
}