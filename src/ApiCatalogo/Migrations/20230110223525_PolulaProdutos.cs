using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo_essencial.Net6.Migrations
{
    public partial class PolulaProdutos : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, dtCadastro, CategoriaId) VALUES('Coca-Cola Diet', 'Refrigerante de Cola 350 ml',5.45 ,'cocacola.jpg', 50, now(), 1)");
            mb.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, dtCadastro, CategoriaId) VALUES('Blend', 'Hamburguer gourmet',8.45 ,'blend.jpg', 20, now(), 2)");
            mb.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, dtCadastro, CategoriaId) VALUES('Pudim', 'Nem gelatina, nem bolo',9.45 ,'pudim.jpg', 35, now(), 3)");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Produtos");
        }
    }
}
