namespace FileStorage.Tests
{
    [TestFixture]
    public class UserManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset the UserManager state before each test
            typeof(UserManager).GetField("users", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                               .SetValue(null, new List<User>());
            typeof(UserManager).GetField("currentUser", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                               .SetValue(null, null);
        }

        [Test]
        public void RegisterUser_ShouldAddNewUser()
        {
            // Arrange
            string username = "newuser";
            string password = "password123";

            // Act
            UserManager.RegisterUser(username, password);

            // Assert
            User user = UserManager.AuthenticateUser(username, password);
            Assert.IsNotNull(user);
        }

        [Test]
        public void AuthenticateUser_ShouldReturnNullForInvalidCredentials()
        {
            // Arrange
            string username = "invaliduser";
            string password = "password123";

            // Act
            User user = UserManager.AuthenticateUser(username, password);

            // Assert
            Assert.IsNull(user);
        }

        [Test]
        public void AuthenticateUser_ShouldSetCurrentUser()
        {
            // Arrange
            string username = "newuser";
            string password = "password123";
            UserManager.RegisterUser(username, password);

            // Act
            UserManager.AuthenticateUser(username, password);
            User currentUser = UserManager.GetCurrentUser();

            // Assert
            Assert.IsNotNull(currentUser);
            Assert.AreEqual(username, currentUser.Username);
        }

        [Test]
        public void GetCurrentUser_ShouldReturnNullWhenNotAuthenticated()
        {
            // Act
            User currentUser = UserManager.GetCurrentUser();

            // Assert
            Assert.IsNull(currentUser);
        }
    }
}
