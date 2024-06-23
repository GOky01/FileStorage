using System.Security.Cryptography;
using System.Text;

namespace FileStorage.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void ValidatePassword_ShouldReturnTrueForCorrectPassword()
        {
            // Arrange
            string username = "testuser";
            string password = "password123";
            User user = new User(username, password);

            // Act
            bool result = user.ValidatePassword(password);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidatePassword_ShouldReturnFalseForIncorrectPassword()
        {
            // Arrange
            string username = "testuser";
            string password = "password123";
            User user = new User(username, password);

            // Act
            bool result = user.ValidatePassword("wrongpassword");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddFile_ShouldAddFileToUserFiles()
        {
            // Arrange
            string username = "testuser";
            string password = "password123";
            User user = new User(username, password);
            string filePath = "testfile.enc";

            // Act
            user.AddFile(filePath);

            // Assert
            Assert.Contains(filePath, user.Files);
        }

        private string GetHashedPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                string saltedPassword = password + "your-salt-string";
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
