using BC = BCrypt.Net.BCrypt;

namespace BSC.Utilities.Static;
public class PasswordHasher
{
    // The WorkFactor adjusts the computational complexity of the hashing process.
    // A higher value increases security but also increases processing time.
    private const int WorkFactor = 10;

    public static string HashPassword(string password)
    {
        return BC.HashPassword(password, WorkFactor);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BC.Verify(password, hashedPassword);
    }
}