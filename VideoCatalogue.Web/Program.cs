using FluentValidation;
using FluentValidation.AspNetCore;
using VideoCatalogue.Web.Configuration;
using VideoCatalogue.Web.Services;
using VideoCatalogue.Web.Services.Storage;
using VideoCatalogue.Web.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.SectionName));
builder.Services.AddSingleton<IStorageProvider, LocalFileStorageProvider>();

builder.Services.AddSingleton<IVideoService, VideoService>();

builder.Services.AddValidatorsFromAssemblyContaining<VideoUploadRequestValidator>();
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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
