using System.Security.Cryptography;
using System.Text;

public static class FileSecurity
{
    private static readonly string EncryptionKey = "AAECAwQFBgcICQoLDA0ODw==";
    private static readonly string EncryptedFolder = "EncryptedFiles";
    private static readonly string DecryptedFolder = "DecryptedFiles";

    static FileSecurity()
    {
        // Ensure directories exist
        Directory.CreateDirectory(EncryptedFolder);
        Directory.CreateDirectory(DecryptedFolder);
    }

    public static string EncryptFile(string inputFile)
    {
        string fileName = Path.GetFileName(inputFile);
        string encryptedFilePath = Path.Combine(EncryptedFolder, fileName + ".enc");

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.IV = new byte[16];

            using (FileStream fsOutput = new FileStream(encryptedFilePath, FileMode.Create))
            using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
            {
                fsInput.CopyTo(cs);
            }
        }
        return encryptedFilePath;
    }

    public static string DecryptFile(string inputFile)
    {
        string fileName = Path.GetFileNameWithoutExtension(inputFile);
        string decryptedFilePath = Path.Combine(DecryptedFolder, fileName);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.IV = new byte[16];

            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
            using (CryptoStream cs = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (FileStream fsOutput = new FileStream(decryptedFilePath, FileMode.Create))
            {
                cs.CopyTo(fsOutput);
            }
        }
        return decryptedFilePath;
    }
}
