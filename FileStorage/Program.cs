using System;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new UserDbContext())
        {
            context.Database.EnsureCreated();
        }

        while (true)
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Upload File");
            Console.WriteLine("4. Download File");
            Console.WriteLine("5. View Files");
            Console.WriteLine("6. Exit");

            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Register();
                    break;
                case 2:
                    Login();
                    break;
                case 3:
                    UploadFile();
                    break;
                case 4:
                    DownloadFile();
                    break;
                case 5:
                    ViewFiles();
                    break;
                case 6:
                    return;
            }
        }
    }

    static void Register()
    {
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();
        UserManager.RegisterUser(username, password);
    }

    static void Login()
    {
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();
        var user = UserManager.AuthenticateUser(username, password);
        if (user != null)
        {
            Console.WriteLine("Login successful.");
        }
        else
        {
            Console.WriteLine("Login failed.");
        }
    }

    static void UploadFile()
    {
        var currentUser = UserManager.GetCurrentUser();
        if (currentUser == null)
        {
            Console.WriteLine("You must be logged in to upload a file.");
            return;
        }

        Console.WriteLine("Enter file path to upload:");
        string filePath = Console.ReadLine();

        try
        {
            string encryptedFilePath = FileSecurity.EncryptFile(filePath);
            currentUser.AddFile(encryptedFilePath);

            using (var context = new UserDbContext())
            {
                context.Users.Update(currentUser);
                context.SaveChanges();
            }

            Console.WriteLine($"File encrypted and saved to: {encryptedFilePath}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File not found: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    static void DownloadFile()
    {
        var currentUser = UserManager.GetCurrentUser();
        if (currentUser == null)
        {
            Console.WriteLine("You must be logged in to download a file.");
            return;
        }

        Console.WriteLine("Enter the encrypted file path to download:");
        string encryptedFilePath = Console.ReadLine();

        if (!currentUser.Files.Contains(encryptedFilePath))
        {
            Console.WriteLine("You do not have access to this file.");
            return;
        }

        try
        {
            string decryptedFilePath = FileSecurity.DecryptFile(encryptedFilePath);
            Console.WriteLine($"File decrypted and saved to: {decryptedFilePath}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File not found: {encryptedFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    static void ViewFiles()
    {
        var currentUser = UserManager.GetCurrentUser();
        if (currentUser == null)
        {
            Console.WriteLine("You must be logged in to view files.");
            return;
        }

        Console.WriteLine("Your files:");
        foreach (var file in currentUser.Files)
        {
            Console.WriteLine(file);
        }
    }
}
