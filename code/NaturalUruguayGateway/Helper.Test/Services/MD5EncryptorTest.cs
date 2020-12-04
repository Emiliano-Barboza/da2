using System.Threading.Tasks;
using NaturalUruguayGateway.Helper.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NaturalUruguayGateway.Helper.Test
{
    [TestClass]
    public class MD5EncryptorTest
    {
        private const string Key = "somethiing to encrypt";
        private string expectedEncryption = "";
        private const string KeyMD5 = "c96efec30c4895f8b84054f458806950";
        private MD5Encryptor encryptor = null;
        
        [TestInitialize]
        public void BeforeEach()
        {
            encryptor = new MD5Encryptor();
        }
        
        [TestMethod]
        public async Task EncryptAsync_Key_ReturnMD5Ok()
        {
            // Arrange
            expectedEncryption = KeyMD5;
            
            // Act
            var actualKey = await encryptor.EncryptAsync(Key);
            
            
            // Assert
            Assert.AreEqual(expectedEncryption, actualKey);
        }
        
        [TestMethod]
        public async Task ValidateAsync_KeyAndKeyMD5_ReturnValidOk()
        {
            // Arrange
            var key = Key;
            var keyMD5 = KeyMD5;
            
            // Act
            var actualIsValid = await encryptor.ValidateAsync(key, keyMD5);
            
            // Assert
            Assert.IsTrue(actualIsValid);
        }
        
        [TestMethod]
        public async Task ValidateAsync_KeyAndKey_ReturnNotValidOk()
        {
            // Arrange
            var key = Key;
            var keyMD5 = Key;
            
            // Act
            var actualIsValid = await encryptor.ValidateAsync(key, keyMD5);
            
            // Assert
            Assert.IsFalse(actualIsValid);
        }
    }
}