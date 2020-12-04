using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.HelperInterface.Configuration;
using NaturalUruguayGateway.HelperInterface.Services;

namespace NaturalUruguayGateway.Helper.Services
{
    public class StorageService : IStorageService
    {
        private readonly IConfigurationManager configurationManager;
        private readonly string imagesFolderName = "images";
        

        public StorageService(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        private string GetAbsoluteFolderPath(int id, string folderName)
        {
            var relativeFolderPath = Path.Combine(folderName, id.ToString());
            var path = Path.Combine(Directory.GetCurrentDirectory(), configurationManager.ResourcesFolder, relativeFolderPath);
            return path;
        }

        private string GetFilePath(int id, string folderName, string fileName)
        {
            var folderPath = GetAbsoluteFolderPath(id, folderName);
            string filePath = string.Empty;

            if (Directory.Exists(folderPath))
            {
                var checkedFilePath = Path.Combine(folderPath, imagesFolderName, fileName);
                if (File.Exists(checkedFilePath))
                {
                    filePath = checkedFilePath;
                }
            }

            return filePath;
        }

        private async Task<Stream> GetFileStream(int id, string folderName, string fileName)
        {
            await Task.Yield();
            FileStream stream = null;
            var filePath = GetFilePath(id, folderName, fileName);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                stream = File.OpenRead(filePath);
            }

            return stream;
        }
        
        private void UpsertFolderPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        
        private string BuildUrlFolderPath(int id, string folderName)
        {
            var relativeFolderPath = Path.Combine(folderName, id.ToString());
            var path = Path.Combine(configurationManager.BaseUrl, relativeFolderPath, imagesFolderName);

            return path;
        }
        
        private string BuildAbsoluteFolderPath(int id, string folderName)
        {
            var relativeFolderPath = Path.Combine(folderName, id.ToString());
            var path = Path.Combine(Directory.GetCurrentDirectory(), configurationManager.ResourcesFolder, relativeFolderPath);

            UpsertFolderPath(path);
            
            path = Path.Combine(path, imagesFolderName);
            
            UpsertFolderPath(path);
            
            return path;
        }

        public async Task<List<string>> AddLodgmentImageAsync(int id, FileModel fileModel)
        {
            var filesPath = new List<string>();
            string fileName;
            var urlFolderPath = BuildUrlFolderPath(id, configurationManager.LodgmentsFolder);
            var absoluteFolderPath = BuildAbsoluteFolderPath(id, configurationManager.LodgmentsFolder);

            foreach (var file in fileModel.Files)
            {
                fileName = string.IsNullOrEmpty(file.FileName) ? fileModel.Name + (filesPath.Count + 1) : file.FileName ;
                var absoluteFilePath = Path.Combine(absoluteFolderPath, fileName);
                var urlFilePath = Path.Combine(urlFolderPath, fileName);
                filesPath.Add(urlFilePath);
                await using (var stream = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return filesPath;
        }

        public async Task<Stream> GetLodgmentImageAsync(int id, string fileName)
        {
            var stream = await GetFileStream(id, configurationManager.LodgmentsFolder, fileName);

            return stream;
        }
        
        public async Task<string> AddSpotImageAsync(int id, FileModel fileModel)
        {
            var urlFolderPath = BuildUrlFolderPath(id, configurationManager.SpotsFolder);
            var absoluteFolderPath = BuildAbsoluteFolderPath(id, configurationManager.SpotsFolder);
            var file = fileModel.Files[0];
            string fileName = string.IsNullOrEmpty(file.FileName) ? fileModel.Name: file.FileName ;
            var absoluteFilePath = Path.Combine(absoluteFolderPath, fileName);
            var filePath = Path.Combine(urlFolderPath, fileName);
            
            await using (var stream = new FileStream(absoluteFilePath, FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<Stream> GetSpotImageAsync(int id, string fileName)
        {
            var stream = await GetFileStream(id, configurationManager.SpotsFolder, fileName);

            return stream;
        }
    }
}