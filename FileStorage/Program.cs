class Program
{
    static void Main(string[] args)
    {
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
        string encryptedFilePath = FileSecurity.EncryptFile(filePath);
        currentUser.AddFile(encryptedFilePath);
        Console.WriteLine($"File encrypted and saved to: {encryptedFilePath}");
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

        string decryptedFilePath = FileSecurity.DecryptFile(encryptedFilePath);
        Console.WriteLine($"File decrypted and saved to: {decryptedFilePath}");
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
