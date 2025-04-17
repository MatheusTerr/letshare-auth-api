using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using LetShare.Data;
using LetShare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LetShare.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _jwtSecret;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _jwtSecret = _configuration["JwtSettings:SecretKey"]!;
        }

        // Valida credenciais e gera tokens
        public async Task<(string accessToken, string refreshToken)?> AuthenticateAsync(string username, string password)
        {
            Console.WriteLine($"Tentando autenticar o usuário (email): {username}");

            // Buscar o usuário pelo email (username no Postman é o email)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == username.ToLower());

            if (user == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                return null;
            }

            Console.WriteLine($"Usuário encontrado: {user.Email}");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                Console.WriteLine("Senha incorreta.");
                return null;
            }

            var accessToken = GenerateToken(user, expiresInMinutes: 6);
            var refreshToken = GenerateToken(user, expiresInMinutes: 60);

            Console.WriteLine("Autenticação bem-sucedida.");
            return (accessToken, refreshToken);
        }

        // Gera novos tokens a partir de um refresh token válido
        public async Task<(string accessToken, string refreshToken)?> RefreshTokensAsync(string refreshToken)
        {
            var principal = GetPrincipalFromToken(refreshToken);
            if (principal == null)
            {
                Console.WriteLine("Refresh token inválido.");
                return null;
            }

            var email = principal.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email não encontrado no refresh token.");
                return null;
            }

            // Buscar usuário pelo email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                Console.WriteLine("Usuário não encontrado no banco de dados.");
                return null;
            }

            var newAccessToken = GenerateToken(user, expiresInMinutes: 6);
            var newRefreshToken = GenerateToken(user, expiresInMinutes: 60);

            Console.WriteLine("Tokens renovados com sucesso.");
            return (newAccessToken, newRefreshToken);
        }

        // Gera o token JWT com as claims do usuário
        private string GenerateToken(User user, int expiresInMinutes)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email), // Email como principal identificador
                new Claim("userId", user.Id),
                // Removido: new Claim("username", user.Username), pois pode estar null no banco
            };

            if (!string.IsNullOrEmpty(user.TenantId))
                claims.Add(new Claim("tenantId", user.TenantId));

            if (!string.IsNullOrEmpty(user.Role))
                claims.Add(new Claim("role", user.Role));

            if (!string.IsNullOrEmpty(user.LanguageId))
                claims.Add(new Claim("languageId", user.LanguageId));

            if (!string.IsNullOrEmpty(user.FirstName))
                claims.Add(new Claim("firstName", user.FirstName));

            if (!string.IsNullOrEmpty(user.MiddleName))
                claims.Add(new Claim("middleName", user.MiddleName));

            if (!string.IsNullOrEmpty(user.LastName))
                claims.Add(new Claim("lastName", user.LastName));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Extrai as informações de um token já emitido
        private ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
                    ValidateLifetime = false // Tokens expirados ainda são válidos para refresh cd D:\Projeto\LetShare taskkill /F /IM LetShare.exe
                }, out var validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
