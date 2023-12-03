using alura_webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace alura_webapi.Data
{
  public class EFContext : DbContext
  {
    public DbSet<Video> Videos { get; set; }

    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<User> Users { get; set; }
    
    public EFContext(DbContextOptions<EFContext> opts) : base (opts)
    {
    }
  }
}