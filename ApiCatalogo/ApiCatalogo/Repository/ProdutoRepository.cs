using APICatalogo_essencial.Net6.Context;
using APICatalogo_essencial.Net6.Models;

namespace ApiCatalogo.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    protected AppCatalogoContext _context;

    public ProdutoRepository(AppCatalogoContext context) : base(context)
    {
    }
    
    public IEnumerable<Produto> GetProdutosPorPreco()
    {
        return Get().OrderBy(c => c.Preco).ToList();
    }
}
