using APICatalogo_essencial.Net6.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo_essencial.Net6.Context;

public class AppCatalogoContext : DbContext
{
    public AppCatalogoContext(DbContextOptions<AppCatalogoContext> options) : base(options)
    {

    }

    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }
}
