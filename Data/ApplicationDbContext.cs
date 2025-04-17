using LetShare.Models;
using Microsoft.EntityFrameworkCore;

namespace LetShare.Data
{
    // Classe responsável por interagir com o banco de dados PostgreSQL
    public class ApplicationDbContext : DbContext
    {
        // Construtor que recebe as opções de configuração do banco
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet que representa a tabela 'tbl_user'
        public DbSet<User> Users { get; set; }
    }
}
