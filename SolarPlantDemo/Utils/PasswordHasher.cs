using System.Security.Cryptography;
using System.Text;

namespace SolarPlantDemo.Utils;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

public class Sha256PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = SHA256.HashData(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var computedHash = HashPassword(password);
        return computedHash == hashedPassword;
    }
}