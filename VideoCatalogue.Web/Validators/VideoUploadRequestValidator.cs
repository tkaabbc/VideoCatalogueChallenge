using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VideoCatalogue.Web.Common;
using VideoCatalogue.Web.Models;

namespace VideoCatalogue.Web.Validators;

public class VideoUploadRequestValidator : AbstractValidator<VideoUploadRequest>
{
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
        .Must(file => IsValidMP4Extension(file!))
        .WithMessage((request, file) => $"File '{file!.FileName}' is not a valid MP4 video file. Only .mp4 format is allowed.");
  }

  private static bool IsValidMP4Extension(IFormFile file)
  {
    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
    return extension == FileExtensions.Mp4;
  }
}

