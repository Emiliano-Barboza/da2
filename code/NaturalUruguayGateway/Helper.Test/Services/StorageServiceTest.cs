using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using NaturalUruguayGateway.Domain.Models;
using NaturalUruguayGateway.Helper.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NaturalUruguayGateway.HelperInterface.Configuration;
using NaturalUruguayGateway.HelperInterface.Services;

namespace NaturalUruguayGateway.Helper.Test
{
    [TestClass]
    public class StorageServiceTest
    {
        private IFixture fixture = null;
        private Mock<IConfigurationManager> moqConfigManager = null;
        private List<IFormFile> formFiles = null;
        private FileModel fileModel = null;
        private int lodgmentId = 0;
        private string expectedUrl = null;
        private List<string> expectedUrls = null;
        private string expectedLodgmentsFolder = null;
        private string expectedResourcesFolder = null;
        private string expectedBaseUrlFolder = null;
        private IStorageService storageService = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            moqConfigManager = new Mock<IConfigurationManager>(MockBehavior.Strict);
            formFiles = fixture.CreateMany<IFormFile>().ToList();
            fixture.Customize<FileModel>(c => c
                .With(x => x.Files, formFiles));
            fileModel = fixture.Create<FileModel>();
            lodgmentId = fixture.Create<int>();
            expectedUrls = new List<string>();
            expectedLodgmentsFolder = fixture.Create<string>();
            expectedResourcesFolder = fixture.Create<string>();
            expectedBaseUrlFolder = fixture.Create<string>();
            storageService = new StorageService(moqConfigManager.Object);
        }
        
        [TestMethod]
        public async Task AddLodgmentImageAsync_LodgmentFolderExists_ReturnsUrl()
        {
            // Arrange
            foreach (var file in fileModel.Files)
            {
                var fileName = fileModel.Name + (expectedUrls.Count + 1);
                expectedUrl = string.Format("{0}/{1}/{2}/{3}/{4}", expectedBaseUrlFolder, expectedLodgmentsFolder, lodgmentId, "images", fileName);
                expectedUrls.Add(expectedUrl);
            }
            
            moqConfigManager.Setup(x => x.LodgmentsFolder).Returns(expectedLodgmentsFolder);
            moqConfigManager.Setup(x => x.ResourcesFolder).Returns(expectedResourcesFolder);
            moqConfigManager.Setup(x => x.BaseUrl).Returns(expectedBaseUrlFolder);
            
            // Act
            var actualUrls = await storageService.AddLodgmentImageAsync(lodgmentId, fileModel);
            
            // Assert
            moqConfigManager.VerifyAll();
            CollectionAssert.AreEqual(expectedUrls, actualUrls);
        }
    }
}