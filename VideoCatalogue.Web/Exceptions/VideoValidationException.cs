namespace VideoCatalogue.Web.Exceptions;

public class VideoValidationException : Exception
{
  public VideoValidationException(string message) : base(message) { }
}