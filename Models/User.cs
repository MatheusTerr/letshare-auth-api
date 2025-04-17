using System.ComponentModel.DataAnnotations.Schema;

namespace LetShare.Models
{
    // Mapeia a tabela 'tbl_user' do banco de dados
    [Table("tbl_user")]
    public class User
    {
        [Column("id")]
        public string Id { get; set; } = string.Empty;  // ID do usuário (UUID no banco)

        [Column("email")]
        public string Email { get; set; } = string.Empty;  // Email usado como login

        [Column("username")]
        public string? Username { get; set; }  // <-- Corrigido: agora aceita NULL

        [Column("password")]
        public string PasswordHash { get; set; } = string.Empty;  // Hash da senha

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("middle_name")]
        public string? MiddleName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("tenant_id")]
        public string? TenantId { get; set; }

        [Column("role")]
        public string? Role { get; set; }

        [Column("language_id")]
        public string? LanguageId { get; set; }

        // Campo auxiliar para exibir nome completo (não mapeado no banco) cd D:\Projeto\LetShare taskkill /F /IM LetShare.exe
        [NotMapped]
        public string? FullName => $"{FirstName} {MiddleName} {LastName}".Trim();
    }
}
