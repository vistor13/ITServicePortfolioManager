namespace ITServicePortfolioManager.BLL.Models.Dto.Auth;

public class AuthOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
}