using System.Security.Cryptography;
using System.Text;

public class ValueHasher
{
    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var HashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(HashBytes);
        }
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var hashedInputPassword = HashPassword(password);
        return hashedInputPassword == hashedPassword;
    }
}