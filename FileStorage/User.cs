using System.Security.Cryptography;
using System.Text;

public class User
{
    public string Username { get; set; }
    private string PasswordHash { get; set; }
    public List<string> Files { get; private set; } = new List<string>();
    private static readonly string Salt = "your-salt-string"; // Сіль для хешування паролів

    public User(string username, string password)
    {
        Username = username;
        PasswordHash = HashPassword(password);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + Salt;
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public bool ValidatePassword(string password)
    {
        return PasswordHash == HashPassword(password);
    }

    public void AddFile(string filePath)
    {
        Files.Add(filePath);
    }
}
