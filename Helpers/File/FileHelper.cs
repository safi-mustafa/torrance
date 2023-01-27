using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Helpers.File
{
    public interface IFileHelper
    {
        string Save(IFileModel model, string existingUrl = "");
        bool Delete(string url);

        void GetFilePath(string extension, string _baseFolder, out string subPath, out string uniqueFileName, out string filePath, out string basePath);
    }
    public class FileHelper : IFileHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileHelper> _logger;
        private readonly IHostingEnvironment _env;

        public FileHelper(IConfiguration configuration, ILogger<FileHelper> logger, IHostingEnvironment env)
        {
            _configuration = configuration;
            _logger = logger;
            _env = env;
        }
        public string Save(IFileModel model, string existingUrl = "")
        {
            string imageUrl = "";
            try
            {
                if (model.File != null)
                {
                    string _baseFolder = model.GetBaseFolder();
                    var extension = Path.GetExtension(model.File.FileName);
                    _logger.LogDebug("File Save method, extension trimmed", extension);
                    string subPath, uniqueFileName, filePath, basePathStorage;
                    GetFilePath(extension, _baseFolder, out subPath, out uniqueFileName, out filePath, out basePathStorage);
                    if (!string.IsNullOrEmpty(existingUrl))
                    {
                        Delete(existingUrl);
                    }
                    _logger.LogDebug("File Save method, created filepath", filePath);

                    var absolutePath = _env.ContentRootPath;
                    var savedDirectory = Path.Combine(absolutePath, filePath);
                    FileInfo file = new FileInfo(savedDirectory);
                    file.Directory.Create(); // If the directory already exists, this method does nothing.
                    _logger.LogDebug("File Save method, creating file directory");
                    using (var fileStream = new FileStream(savedDirectory, FileMode.Create))
                    {
                        _logger.LogDebug("File Save method, copying file");

                        model.File.CopyTo(fileStream);
                    }
                    _logger.LogDebug("File Save method, file saved");

                    imageUrl = $"/{basePathStorage}/{subPath}/{_baseFolder}/{uniqueFileName}";
                    _logger.LogDebug("File Save method, url created", imageUrl);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FileHelper.SaveImage");
            }
            return imageUrl;
        }

        public void GetFilePath(string extension, string _baseFolder, out string subPath, out string uniqueFileName, out string filePath, out string basePath)
        {
            string basePathStorage = $"wwwroot/{_configuration.GetValue<string>("UploadBaseStoragePath")}";
            basePath = _configuration.GetValue<string>("UploadBasePath");
            subPath = _configuration.GetValue<string>("UploadSubPath");
            string uploadsFolder = Path.Combine(basePathStorage, subPath, _baseFolder);
            uniqueFileName = DateTime.UtcNow.Ticks.ToString() + extension;
            filePath = Path.Combine(uploadsFolder, uniqueFileName);
        }

        public bool Delete(string url)
        {
            try
            {
                string basePath = _configuration.GetValue<string>("UploadBasePath");
                if (!string.IsNullOrEmpty(url))
                {
                    string tempFilePath = basePath + url.Replace("/", "\\");
                    if (System.IO.File.Exists(tempFilePath))
                    {
                        System.IO.File.Delete(tempFilePath);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserManagementAPI.DeleteImage");
            }
            return false;
        }
    }
}
