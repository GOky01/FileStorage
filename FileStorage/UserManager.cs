public static class UserManager
{
    private static User currentUser;

    public static void RegisterUser(string username, string password)
    {
        using (var context = new UserDbContext())
        {
            if (context.Users.Any(u => u.Username == username))
            {
                Console.WriteLine("User already exists.");
                return;
            }
            var user = new User(username, password);
            context.Users.Add(user);
            context.SaveChanges();
            currentUser = user; 
            Console.WriteLine("User registered successfully.");
        }
    }

    public static User AuthenticateUser(string username, string password)
    {
        using (var context = new UserDbContext())
        {
            User user = context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null && user.ValidatePassword(password))
            {
                currentUser = user;
                Console.WriteLine("User authenticated successfully.");
                return user;
            }
            Console.WriteLine("Invalid username or password.");
            return null;
        }
    }

    public static User GetCurrentUser()
    {
        return currentUser;
    }

    public static void SetCurrentUser(User user)
    {
        currentUser = user;
    }

    public static void AddFileToCurrentUser(string filePath)
    {
        if (currentUser == null)
        {
            throw new InvalidOperationException("No user is currently logged in.");
        }

        currentUser.AddFile(filePath);
        using (var context = new UserDbContext())
        {
            context.Users.Update(currentUser);
            context.SaveChanges();
        }
    }

    public static void ViewAllUsers()
    {
        using (var context = new UserDbContext())
        {
            var users = context.Users.ToList();
            foreach (var user in users)
            {
                Console.WriteLine($"Username: {user.Username}, PasswordHash: {user.PasswordHash}");
            }
        }
    }
}
