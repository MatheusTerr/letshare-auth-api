namespace LetShare.Models
{
    public class LoginRequest
    {
        public required string GrantType { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string Username { get; set; } // Continua sendo 'Username' aqui
        public required string Password { get; set; }
    }
}
