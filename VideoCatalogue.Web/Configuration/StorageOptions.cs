namespace VideoCatalogue.Web.Configuration;

/// <summary>
/// Configuration options for storage provider
/// </summary>
public class StorageOptions
{
  public const string SectionName = "Storage";

  /// <summary>
  /// Storage type: "Local" (currently only "Local" is supported)
  /// </summary>
  public string Type { get; set; } = "Local";

  /// <summary>
  /// Local file system path (for Local storage)
  /// </summary>
  public string LocalPath { get; set; } = "wwwroot/video";

  /// <summary>
  /// Base URL for accessing files
  /// </summary>
  public string BaseUrl { get; set; } = "/video";
}

