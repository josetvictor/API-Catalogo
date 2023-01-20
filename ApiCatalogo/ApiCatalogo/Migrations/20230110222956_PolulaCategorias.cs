using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo_essencial.Net6.Migrations
{
    public partial class PolulaCategorias : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Categorias(Nome, ImagemURL) VALUES('Bebida', 'bebidas.jpg')");
            mb.Sql("INSERT INTO Categorias(Nome, ImagemURL) VALUES('Lanches', 'lanches.jpg')");
            mb.Sql("INSERT INTO Categorias(Nome, ImagemURL) VALUES('Sobremesas', 'sobremesas.jpg')");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Categorias");
        }
    }
}
