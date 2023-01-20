using Microsoft.EntityFrameworkCore;
using MinApiCatalogo.Models;

namespace MinApiCatalogo.Context;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

	public DbSet<Produto>? Produtos { get; set; }
	public DbSet<Categoria>? Categorias { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) 
	{
		// Vale lembrar que no produto de Catalogo Rest foi usado os data anotations para a configuração dessas entidade
		// Configurando a classe de categoria
		modelBuilder.Entity<Categoria>().HasKey(c => c.CategoriaId);
		modelBuilder.Entity<Categoria>().Property(c => c.Nome)
										.HasMaxLength(100)
										.IsRequired();
		modelBuilder.Entity<Categoria>().Property(c => c.Descricao)
										.HasMaxLength(150)
										.IsRequired();

		// Configurando a classe de Produto
		modelBuilder.Entity<Produto>().HasKey(p => p.ProdutoId);
		modelBuilder.Entity<Produto>().Property(p => p.Nome).HasMaxLength(100).IsRequired();
		modelBuilder.Entity<Produto>().Property(p => p.Descricao).HasMaxLength(150).IsRequired();
		modelBuilder.Entity<Produto>().Property(p => p.Imagem).HasMaxLength(100).IsRequired();

		modelBuilder.Entity<Produto>().Property(p => p.Preco).HasPrecision(14, 2);

		// Relacionamento das entidades
		modelBuilder.Entity<Produto>()
					.HasOne<Categoria>(p => p.Categoria)
					.WithMany(p => p.Produtos)
						.HasForeignKey(p => p.CategoriaId);
	}
}
