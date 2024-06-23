using NUnit.Framework;
using System.IO;

namespace FileStorage.Tests
{
    [TestFixture]
    public class FileSecurityTests
    {
        private string inputFile;
        private string encryptedFile;
        private string decryptedFile;

        [SetUp]
        public void Setup()
        {
            // Ensure directories exist for tests
            Directory.CreateDirectory("EncryptedFiles");
            Directory.CreateDirectory("DecryptedFiles");

            // Create a test file
            inputFile = "test.txt";
            encryptedFile = Path.Combine("EncryptedFiles", "test.txt.enc");
            decryptedFile = Path.Combine("DecryptedFiles", "test.txt");
            File.WriteAllText(inputFile, "This is a test file.");
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup files after tests
            if (File.Exists(inputFile)) File.Delete(inputFile);
            if (File.Exists(encryptedFile)) File.Delete(encryptedFile);
            if (File.Exists(decryptedFile)) File.Delete(decryptedFile);
        }

        [Test]
        public void EncryptFile_ShouldCreateEncryptedFile()
        {
            // Act
            string resultFilePath = FileSecurity.EncryptFile(inputFile);

            // Assert
            Assert.AreEqual(encryptedFile, resultFilePath);
            Assert.IsTrue(File.Exists(encryptedFile));
        }

        [Test]
        public void DecryptFile_ShouldCreateDecryptedFileWithSameContent()
        {
            // Arrange
            FileSecurity.EncryptFile(inputFile);

            // Act
            string resultFilePath = FileSecurity.DecryptFile(encryptedFile);
            string decryptedContent = File.ReadAllText(resultFilePath);

            // Assert
            Assert.AreEqual(decryptedFile, resultFilePath);
            Assert.AreEqual("This is a test file.", decryptedContent);
        }
    }
}
