using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APICatalogo_essencial.Net6.Context;

namespace ApiCatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {   
        private ProdutoRepository _produtoRepo;
        private CategoriasRepository _categoriaRepo;
        public AppCatalogoContext _context;

        public UnitOfWork(AppCatalogoContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepo = _categoriaRepo ?? new CategoriasRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}