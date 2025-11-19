using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VideoCatalogue.Web.Models;

namespace VideoCatalogue.Web.Validators;

public class VideoUploadRequestValidator : AbstractValidator<VideoUploadRequest>
{
  private static readonly string[] ValidMimeTypes =
  {
    "video/mp4",
  };

  public VideoUploadRequestValidator()
  {
    RuleFor(x => x.Files)
        .NotNull()
        .WithMessage("Files list cannot be null.")
        .NotEmpty()
        .WithMessage("No files selected for upload.");

    RuleForEach(x => x.Files)
        .Must(file => file != null)
        .WithMessage("File cannot be null.")
        .Must(file => file!.Length > 0)
        .WithMessage((request, file) => $"File '{file!.FileName}' is empty.")
        .Must(file => IsValidMimeType(file!))
        .WithMessage((request, file) => $"File '{file!.FileName}' is not a valid MP4 video file. Only MP4 format is allowed.");
  }

  private static bool IsValidMimeType(IFormFile file)
  {
    return ValidMimeTypes.Contains(file.ContentType.ToLowerInvariant());
  }
}

