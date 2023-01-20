using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo_essencial.Net6.Models;

[Table("Categorias")]
public class Categoria
{
    public Categoria()
    {
        // É uma boa pratica inicializar a coleção na classe que contem a 
        // coleção de um outra classe
        Produtos = new Collection<Produto>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(80)]
    public string? Nome { get; set; }

    [Required]
    [MaxLength(300)]
    public string? ImagemURL { get; set; }

    public ICollection<Produto>? Produtos { get; set; }
}
