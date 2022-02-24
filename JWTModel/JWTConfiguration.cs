namespace JWTGenerator.JWTModel;
public class JWTConfiguration
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public TimeSpan AccessTokenExpiration { get; set; } = TimeSpan.FromDays(14);
    public TimeSpan RefreshTokenExpiration { get; set; } = TimeSpan.FromDays(90);
    public bool ClearCliamTypeMap { get; set; }
}

