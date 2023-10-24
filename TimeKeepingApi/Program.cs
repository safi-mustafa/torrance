using Web.Extensions;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Formatting.Json;
using CorrelationId;

var builder = WebApplication.CreateBuilder(args);


ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;


builder.Host.UseSerilog((context, configuration) => configuration
.ReadFrom.Configuration(context.Configuration)
);


builder.Services.AddRazorPages();
builder.Services.ConfigureServices(configuration);

//Dependance Injections for custom service configured
builder.Services.ConfigureDependencies();

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseSerilogRequestLogging();
//app.UseHttpsRedirection(); //ENABLE IN PRODUCTION
var directoryPath = configuration.GetValue<string>("DirectoryPath");
var uploadBasePath = configuration.GetValue<string>("UploadBasePath");
//app.UseHttpsRedirection(); //ENABLE IN PRODUCTION
app.UseStaticFiles(); // For the wwwroot folder  
app.UseStaticFiles(new StaticFileOptions
{

    FileProvider = new PhysicalFileProvider(Path.Combine(directoryPath, uploadBasePath)),
    RequestPath = "/Storage"
});


app.UseCorrelationId();
app.UseRouting();
//app.UseCors(
//    options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
//);

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TorranceApi v1"));

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
       name: "default",
       pattern: "{controller=Account}/{action=Login}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();

