using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Repository;
using APICatalogo_essencial.Net6.Context;
using APICatalogo_essencial.Net6.Controllers;
using APICatalogo_essencial.Net6.Models;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApiCatalogoXUnitTest
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnitOfWork repository;
        private AppCatalogoContext context;

        public static DbContextOptions<AppCatalogoContext> dbContextOptions { get; }
        public static string connectionString = "Server=localhost;Port=3308;DataBase=CatalogoDB;Uid=root;Pwd=root";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppCatalogoContext>()
                .UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString)).Options;
        }

        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(ctg =>
            {
                ctg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            context = new AppCatalogoContext(dbContextOptions);

            repository = new UnitOfWork(context);
        }

        // testes unitarios
        // GET - OKResult
        [Fact]
        public void GetCategorias_return_OkResult()
        {
            // Arrange
            var controller = new CategoriasController(context);

            // Act
            var data = controller.Get();

            // Assert
            Assert.IsType<List<Categoria>>(data.Value.ToList());
        }

        // GET - BadRequest
        [Fact]
        public void GetCategorias_return_BadRequest()
        {
            // Arrange
            var controller = new CategoriasController(context);

            // Act
            var data = controller.Get();

            // Assert
            Assert.IsType<BadRequestResult>(data.Result);
        }

        // GET - retorna uma lista de objetos Categoria
        [Fact]
        public void GetCategorias_matchResult()
        {
            // Arrage
            var controller = new CategoriasController(context);

            //Act
            var data = controller.Get();

            //Assert
            Assert.IsType<List<Categoria>>(data.Result);
            var cat = data.Value.Should().BeAssignableTo<List<Categoria>>().Subject;

            Assert.Equal("Bebida alterada", cat[0].Nome);
            Assert.Equal("Bebida21.jpg", cat[0].ImagemURL);

            Assert.Equal("Sobremesa", cat[2].Nome);
            Assert.Equal("sobremesa.jpg", cat[2].ImagemURL);
        }
    }
}