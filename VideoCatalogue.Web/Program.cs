using FluentValidation;
using FluentValidation.AspNetCore;
using VideoCatalogue.Web.Services;
using VideoCatalogue.Web.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IVideoService, VideoService>();

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<VideoUploadRequestValidator>();

// Enable FluentValidation auto-validation for API controllers
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "wwwroot/video"));

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
