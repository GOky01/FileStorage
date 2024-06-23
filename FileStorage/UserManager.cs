
public static class UserManager
{
    private static List<User> users = new List<User>();
    private static User currentUser;

    public static void RegisterUser(string username, string password)
    {
        if (users.Any(u => u.Username == username))
        {
            Console.WriteLine("User already exists.");
            return;
        }
        users.Add(new User(username, password));
        Console.WriteLine("User registered successfully.");
    }

    public static User AuthenticateUser(string username, string password)
    {
        User user = users.FirstOrDefault(u => u.Username == username);
        if (user != null && user.ValidatePassword(password))
        {
            currentUser = user;
            Console.WriteLine("User authenticated successfully.");
            return user;
        }
        Console.WriteLine("Invalid username or password.");
        return null;
    }

    public static User GetCurrentUser()
    {
        return currentUser;
    }
}
