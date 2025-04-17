using LetShare.Models;
using LetShare.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // Endpoint POST: /api/auth/token (compatível com o Postman)
        [HttpPost("token")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            // Log para verificar o conteúdo da requisição recebida
            Console.WriteLine($"Requisição de Login recebida: {loginRequest.Username}");

            // Valida se as credenciais do cliente estão corretas
            if (loginRequest.GrantType != "password" ||
                loginRequest.ClientId != "web" ||
                loginRequest.ClientSecret != "webpass1")
            {
                // Log de falha na validação de credenciais do cliente
                Console.WriteLine("Falha na validação de credenciais do cliente.");
                return Unauthorized(new { message = "Credenciais do cliente inválidas." });
            }

            // Valida usuário e senha usando o AuthService
            var result = await _authService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            if (result == null)
            {
                // Log de falha na autenticação
                Console.WriteLine("Falha na autenticação: usuário ou senha inválidos.");
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            // Log de sucesso na autenticação
            Console.WriteLine($"Usuário {loginRequest.Username} autenticado com sucesso.");

            // Retorna os tokens (access e refresh)
            return Ok(new
            {
                access_token = result.Value.accessToken,
                refresh_token = result.Value.refreshToken
            });
        }

        // Endpoint POST: /api/auth/refresh - Gera novos tokens a partir do refresh token 
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            // Log para verificar o conteúdo do refresh token recebido
            Console.WriteLine($"Requisição de Refresh token recebida: {refreshRequest.RefreshToken}");

            var result = await _authService.RefreshTokensAsync(refreshRequest.RefreshToken);

            if (result == null)
            {
                // Log de falha no refresh do token
                Console.WriteLine("Falha na renovação do refresh token.");
                return Unauthorized(new { message = "Refresh token inválido ou expirado" });
            }

            // Log de sucesso no refresh do token
            Console.WriteLine("Refresh token gerado com sucesso.");

            // Retorna os novos tokens (access e refresh)
            return Ok(new
            {
                access_token = result.Value.accessToken,
                refresh_token = result.Value.refreshToken
            });
        }
    }
}
