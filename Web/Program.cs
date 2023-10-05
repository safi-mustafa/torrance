using Microsoft.Identity.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Web.Extensions;
using DataLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Configuration;
using CorrelationId;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllAuthorized", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});
builder.Services.ConfigureDependencies();
Microsoft.Extensions.Configuration.ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
using var db = scope.ServiceProvider.GetService<ToranceContext>();
await db.Database.MigrateAsync();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

var directoryPath = configuration.GetValue<string>("DirectoryPath");
var uploadBasePath = configuration.GetValue<string>("UploadBasePath");
//app.UseHttpsRedirection(); //ENABLE IN PRODUCTION
app.UseStaticFiles(); // For the wwwroot folder  
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(directoryPath, uploadBasePath)),
    RequestPath = "/Storage"
});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//                    Path.Combine(builder.Environment.ContentRootPath, "Storage")),
//    RequestPath = "/Storage"
//});

//app.UseDirectoryBrowser(new DirectoryBrowserOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        System.IO.Path.Combine(builder.Environment.ContentRootPath, "Storage")
//        ),
//    RequestPath = "/Storage"
//});
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, "Storage")),
//    RequestPath = "/Storage"
//});
app.UseCorrelationId();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
