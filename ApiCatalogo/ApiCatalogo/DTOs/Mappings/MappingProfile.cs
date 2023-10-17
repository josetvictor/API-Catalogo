using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo_essencial.Net6.Models;
using AutoMapper;

namespace ApiCatalogo.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
}