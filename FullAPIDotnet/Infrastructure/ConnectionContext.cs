using FullAPIDotnet.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FullAPIDotnet.Infrastructure;

// dbContext é responsavel pela conexão com o banco
public class ConnectionContext : DbContext
{
    // dbSet relaciona a classe com uma tabela
    public DbSet<Employee> Employee { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Server=localhost;" +
            "Port=5432;Database=employee;" +
            "User Id=postgres;" +
            "Password=1973"
        );
        base.OnConfiguring(optionsBuilder);
    }
}