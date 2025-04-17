namespace LetShare.Models
{
    // Modelo usado para receber o refresh_token no endpoint de renovação
    public class RefreshRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
