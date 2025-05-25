namespace ITServicePortfolioManager.BLL.Services.Common;

public static class PasswordHashProvider
{
    public static string GenerateHashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    
    public static bool Verify(string password, string hashedPassword)=>
        BCrypt.Net.BCrypt.EnhancedVerify(password,hashedPassword);
}