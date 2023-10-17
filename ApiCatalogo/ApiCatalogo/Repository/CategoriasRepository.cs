using APICatalogo_essencial.Net6.Context;
using APICatalogo_essencial.Net6.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repository;

public class CategoriasRepository : Repository<Categoria>, ICategoriaRepository
{
    protected AppCatalogoContext _context;

    public CategoriasRepository(AppCatalogoContext context) : base(context)
    {
    }
    
    public IEnumerable<Categoria> GetCategoriasProdutos()
    {
        return Get().Include(c => c.Produtos);
    }
}
