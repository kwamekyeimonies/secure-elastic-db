namespace AcceralytDevTest.utils;

public interface IPasswordHandler
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string password);
}