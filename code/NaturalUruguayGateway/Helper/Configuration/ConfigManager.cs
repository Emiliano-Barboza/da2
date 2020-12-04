using System.IO;
using NaturalUruguayGateway.HelperInterface.Configuration;
using Microsoft.Extensions.Configuration;

namespace NaturalUruguayGateway.Helper.Configuration
{
    public class ConfigManager : IConfigurationManager
    {
        private readonly IConfigurationRoot configuration = null;
        private readonly string appSettingsKey = null;
        private readonly string dataBaseKey = null;
        private readonly string logoutUrlKey = null;
        private readonly string defaultPasswordKey = null;
        private readonly string authenticationSchema = null;
        private readonly string lodgmentsFolder = null;
        private readonly string spotsFolder = null;
        private readonly string resourcesFolder = null;
        private readonly string importThirdPartyAssembliesFolder = null;
        private readonly string baseUrl = null;
        private readonly string appSettingsJsonKey = null;
        public ConfigManager()
        {
            appSettingsKey = "AppSettings";
            dataBaseKey = "NaturalUruguayDB";
            logoutUrlKey = "LogoutRedirectUrl";
            defaultPasswordKey = "DefaultPassword";
            authenticationSchema = "AuthenticationSchema";
            lodgmentsFolder = "LodgmentsFolder";
            spotsFolder = "SpotsFolder";
            resourcesFolder = "ResourcesFolder";
            importThirdPartyAssembliesFolder = "ImportThirdPartyAssembliesFolder";
            baseUrl = "BaseUrl";
            appSettingsJsonKey = "appsettings.json";
            var directory = Directory.GetCurrentDirectory();
            configuration = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile(appSettingsJsonKey)
                .Build();
        }

        public string ConnectionString => configuration.GetConnectionString(dataBaseKey);

        public string LogoutRedirectUrl {
            get
            {
                var defaultLogoutUrl = GetValueInAppSettings(logoutUrlKey);
                return defaultLogoutUrl;
            }
        }
        public string DefaultPassword {
            get
            {
                var defaultPassword = GetValueInAppSettings(defaultPasswordKey);
                return defaultPassword;
            }
        }
        public string AuthenticationSchema {
            get
            {
                var defaultAuthenticationSchema = GetValueInAppSettings(authenticationSchema);
                return defaultAuthenticationSchema;
            }
        }
        
        public string LodgmentsFolder {
            get
            {
                var defaultAuthenticationSchema = GetValueInAppSettings(lodgmentsFolder);
                return defaultAuthenticationSchema;
            }
        }
        
        public string SpotsFolder {
            get
            {
                var defaultAuthenticationSchema = GetValueInAppSettings(spotsFolder);
                return defaultAuthenticationSchema;
            }
        }
        
        public string ResourcesFolder {
            get
            {
                var defaultAuthenticationSchema = GetValueInAppSettings(resourcesFolder);
                return defaultAuthenticationSchema;
            }
        }
        
        public string ImportThirdPartyAssembliesFolder {
            get
            {
                var defaultAuthenticationSchema = GetValueInAppSettings(importThirdPartyAssembliesFolder);
                return defaultAuthenticationSchema;
            }
        }

        public string BaseUrl {
            get
            {
                var defaultAuthenticationSchema = GetValueInAppSettings(baseUrl);
                return defaultAuthenticationSchema;
            }
        }

        private IConfigurationSection GetSection(string section)
        {
            return configuration.GetSection(section);
        }
        private string GetValueInAppSettings(string section)
        {
            var value = string.Empty;
            var appSettingsSection = GetSection(appSettingsKey);
            var sectionInAppSettings = appSettingsSection?.GetSection(section);
            if (sectionInAppSettings != null)
            {
                value = sectionInAppSettings.Value;
            }
            return value;
        }
    }
}