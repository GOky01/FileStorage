using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class User
{
    [Key]
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public List<string> Files { get; set; } = new List<string>();

    public User() { }

    public User(string username, string password)
    {
        while (!IsPasswordStrong(password))
        {
            Console.WriteLine("Password does not meet strength requirements. Please enter a stronger password:");
            password = Console.ReadLine();
        }
        Username = username;
        PasswordHash = HashPassword(password);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + "your-salt-string";
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public bool ValidatePassword(string password)
    {
        return PasswordHash == HashPassword(password);
    }

    private bool IsPasswordStrong(string password)
    {
        var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
        return regex.IsMatch(password);
    }

    public void AddFile(string filePath)
    {
        Files.Add(filePath);
    }
}
