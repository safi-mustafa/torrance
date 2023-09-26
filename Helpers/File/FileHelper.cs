using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Helpers.File
{
    public interface IFileHelper
    {
        string Save(IFileModel model, string existingUrl = "", string baseFolder = "", string namePrefix = "");
        bool Delete(string url);

        void GetFilePath(string extension, string _baseFolder, string prefix, out string subPath, out string uniqueFileName, out string filePath, out string basePath);
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
        public string Save(IFileModel model, string existingUrl = "", string baseFolder = "", string namePrefix = "")
        {
            string imageUrl = "";
            try
            {
                if (model.File != null)
                {

                    string _baseFolder = string.IsNullOrEmpty(baseFolder) ? model.GetBaseFolder() : baseFolder;
                    var extension = Path.GetExtension(model.File.FileName);
                    _logger.LogDebug("File Save method, extension trimmed", extension);
                    string subPath, filePath, uniqueFileName, basePathStorage;
                    GetFilePath(extension, _baseFolder, namePrefix, out subPath, out uniqueFileName, out filePath, out basePathStorage);
                    if (!string.IsNullOrEmpty(existingUrl))
                    {
                        Delete(existingUrl);
                    }
                    _logger.LogDebug("File Save method, created filepath", filePath);
                    var directoryPath = _configuration.GetValue<string>("DirectoryPath");
                    //var absolutePath = //_env.ContentRootPath;
                    var savedDirectory = Path.Combine(directoryPath, filePath);
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

        public void GetFilePath(string extension, string _baseFolder, string prefix, out string subPath, out string uniqueFileName, out string filePath, out string basePath)
        {
            string basePathStorage = $"{_configuration.GetValue<string>("UploadBaseStoragePath")}";
            basePath = _configuration.GetValue<string>("UploadBasePath");
            subPath = _configuration.GetValue<string>("UploadSubPath");
            string uploadsFolder = Path.Combine(basePathStorage, subPath, _baseFolder);
            uniqueFileName = DateTime.UtcNow.Ticks.ToString() + extension;
            if (!string.IsNullOrEmpty(prefix))
            {
                uniqueFileName = prefix + "-" + uniqueFileName;
            }
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
