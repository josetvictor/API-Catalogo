using APICatalogo_essencial.Net6.Models;

namespace ApiCatalogo.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorPreco();
}
