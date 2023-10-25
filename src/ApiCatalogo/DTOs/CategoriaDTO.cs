
namespace ApiCatalogo.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<ProdutoDTO>? Produtos { get; set; }
    }
}