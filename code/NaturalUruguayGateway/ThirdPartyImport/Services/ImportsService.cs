using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NaturalUruguayGateway.ImportInterface.Services;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.HelperInterface.Configuration;
using NaturalUruguayGateway.NaturalUruguayEngineInterface.Services;
using NaturalUruguayGateway.ThridPartyImportInterface.Services;

namespace NaturalUruguayGateway.ThirdPartyImport.Services
{
    public class ImportsService : IImportsService
    {
        private readonly ILodgmentsService lodgmentsService;
        private readonly ISpotsService spotsService;
        private readonly IConfigurationManager configurationManager;
        private readonly DirectoryInfo directory;
        private const string AssemblyExtension = "*.dll";
        
        
        public ImportsService(ILodgmentsService lodgmentsService, ISpotsService spotsService,
            IConfigurationManager configurationManager)
        {
            this.lodgmentsService = lodgmentsService;
            this.spotsService = spotsService;
            this.configurationManager = configurationManager;
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), configurationManager.ResourcesFolder,
                configurationManager.ImportThirdPartyAssembliesFolder);
            this.directory = new DirectoryInfo(rootPath);
        }
        
        public async Task<List<ImportModel>> GetImportsAsync()
        {
            await Task.Yield();
            var imports = new List<ImportModel>();
            FileInfo[] files = this.directory.GetFiles(AssemblyExtension);
            foreach (var file in files)
            {
                Assembly assemblyLoaded = Assembly.LoadFile(file.FullName);
                var loadedImplementations = assemblyLoaded.GetTypes().Where(t => typeof(IImport).IsAssignableFrom(t) && t.IsClass);

                if (loadedImplementations.Any())
                {
                    var import = new ImportModel()
                    {
                        Name = Path.GetFileNameWithoutExtension(file.Name)
                    };
                    imports.Add(import);
                }
            }

            return imports;
        }
    }
}