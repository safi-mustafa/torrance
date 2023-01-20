using Microsoft.Identity.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataLibrary;
using Models;
using IdentityProvider.Seed;
using System.Diagnostics;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Repositories.Services.CommonServices.ContractorService;
using Repositories.Services.CommonServices.UnitService;
using Repositories.Services.CommonServices.DepartmentService;
using Repositories.Services.WeldRodRecordServices.LocationService;
using Repositories.Services.WeldRodRecordServices.WeldMethodService;
using Repositories.Services.WeldRodRecordServices.RodTypeService;
using Repositories.Services.TimeOnToolServices.PermitTypeService;
using Repositories.Services.TimeOnToolServices.ReworkService;
using Repositories.Services.TimeOnToolServices.ShiftService;
using Repositories.Services.TimeOnToolServices.ShiftDelayService;
using Repositories.Services.TimeOnToolServices.SOWService;
using Repositories.Services.WeldRodRecordServices.WRRLogService;
using Repositories.Services.WeldRodRecordServices.EmployeeService;
using Repositories.Services.TimeOnToolServices.TOTLogService;
using Repositories.Services.TimeOnToolServices.UserService;
using Repositories.Shared.AuthenticationService;
using Repositories.Services.FolderService;
using Helpers.File;
using Centangle.Common.ResponseHelpers.Models;
using Microsoft.Extensions.FileProviders;
using System;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});
builder.Services.ConfigureDependencies();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                    System.IO.Path.Combine(Directory.GetParent(builder.Environment.ContentRootPath).FullName, "Storage")
                    ),
    RequestPath = "/Storage"
});

//app.UseDirectoryBrowser(new DirectoryBrowserOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        System.IO.Path.Combine(Directory.GetParent(environment.ContentRootPath).FullName, "Storage")
//        ),
//    RequestPath = "/Storage"
//});
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, "Storage")),
//    RequestPath = "/Storage"
//});
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
