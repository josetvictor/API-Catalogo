using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ApiCatalogo.Repository;

using APICatalogo_essencial.Net6.Context;
using APICatalogo_essencial.Net6.Models;

namespace APICatalogo_essencial.Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _context;

        public ProdutosController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.ProdutoRepository.Get().ToList();
            if (produtos is null)
                return NotFound("Nenhum produto encontrado");

            return Ok(produtos);
        }

        // api/produtos/1
        [HttpGet("{id:int:min(1)}", Name ="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.ProdutoRepository.GetById(p => p.Id == id);

            if (produto is null)
                return NotFound("Produto não encontrado");

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            _context.ProdutoRepository.Add(produto);
            _context.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.Id)
                return BadRequest();

            _context.ProdutoRepository.Update(produto); 
            _context.Commit();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.ProdutoRepository.GetById(p => p.Id == id);

            if (produto is null)
                return NotFound("Produto não localizado...");

            _context.ProdutoRepository.Delete(produto);
            _context.Commit();

            return Ok(produto);
        }
    }
}
