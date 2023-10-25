using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ApiCatalogo.Repository;

using APICatalogo_essencial.Net6.Context;
using APICatalogo_essencial.Net6.Models;
using AutoMapper;
using ApiCatalogo.DTOs;

namespace APICatalogo_essencial.Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _context.ProdutoRepository.Get().ToList();
            if (produtos is null)
                return NotFound("Nenhum produto encontrado");

            return Ok(_mapper.Map<List<ProdutoDTO>>(produtos));
        }

        // api/produtos/1
        [HttpGet("{id:int:min(1)}", Name ="ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _context.ProdutoRepository.GetById(p => p.Id == id);

            if (produto is null)
                return NotFound("Produto não encontrado");

            return Ok(_mapper.Map<ProdutoDTO>(produto));
        }

        [HttpPost]
        public ActionResult Post([FromBody]ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            _context.ProdutoRepository.Add(produto);
            _context.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produtoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            if(id != produtoDto.Id)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);

            _context.ProdutoRepository.Update(produto); 
            _context.Commit();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _context.ProdutoRepository.GetById(p => p.Id == id);

            if (produto is null)
                return NotFound("Produto não localizado...");

            _context.ProdutoRepository.Delete(produto);
            _context.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
        }
    }
}
