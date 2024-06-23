using System;
using System.Linq;
using System.Collections.Generic;

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
}
