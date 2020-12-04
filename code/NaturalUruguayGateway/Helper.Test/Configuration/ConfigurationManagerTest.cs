using System.Threading.Tasks;
using NaturalUruguayGateway.Helper.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NaturalUruguayGateway.Helper.Test
{
    [TestClass]
    public class ConfigurationManagerTest
    {
        private string emptyString = "";
        private ConfigManager configManager = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            configManager = new ConfigManager();
        }
        
        [TestMethod]
        public async Task ConnectionString_HasValue_Success()
        {
            // Arrange
            
            // Act
            var actualResponse = configManager.ConnectionString;

            // Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreNotEqual(emptyString, actualResponse);
        }
        
        [TestMethod]
        public async Task LogoutRedirectUrl_HasValue_Success()
        {
            // Arrange
            
            // Act
            var actualResponse = configManager.LogoutRedirectUrl;

            // Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreNotEqual(emptyString, actualResponse);
        }
        
        [TestMethod]
        public async Task DefaultPassword_HasValue_Success()
        {
            // Arrange
            
            // Act
            var actualResponse = configManager.DefaultPassword;

            // Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreNotEqual(emptyString, actualResponse);
        }
        
        [TestMethod]
        public async Task AuthenticationSchema_HasValue_Success()
        {
            // Arrange
            
            // Act
            var actualResponse = configManager.AuthenticationSchema;

            // Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreNotEqual(emptyString, actualResponse);
        }
        
        [TestMethod]
        public async Task LodgmentImagesFolder_HasValue_Success()
        {
            // Arrange
            
            // Act
            var actualResponse = configManager.LodgmentsFolder;

            // Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreNotEqual(emptyString, actualResponse);
        }
        
        [TestMethod]
        public async Task ResourcesFolder_HasValue_Success()
        {
            // Arrange
            
            // Act
            var actualResponse = configManager.ResourcesFolder;

            // Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreNotEqual(emptyString, actualResponse);
        }
        
        [TestMethod]
        public async Task BaseUrl_HasValue_Success()
        {
            // Arrange
            
            // Act
            var actualResponse = configManager.BaseUrl;

            // Assert
            Assert.IsNotNull(actualResponse);
            Assert.AreNotEqual(emptyString, actualResponse);
        }
    }
}