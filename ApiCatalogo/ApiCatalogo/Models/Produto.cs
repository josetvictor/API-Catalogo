using ApiCatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo_essencial.Net6.Models;

[Table("Produtos")]
public class Produto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }
    
    [Required]
    [Column(TypeName="decimal(10,2)")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(80)]
    public string? ImagemUrl { get; set; }

    public float Estoque { get; set; }

    public DateTime dtCadastro { get; set; }

    public int CategoriaId { get; set; }

    // JSONIgnore é usado para descartar a propriedade na serialização do objeto
    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    // Validação a nivel de model
    public IEnumerable<ValidationResult> Validade(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {
            var primeiraLetra = this.Nome.ToString()[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult("A primeira letra do produto deve ser maiúscula", new[] { nameof(this.Nome) });
            }
        }

        if (this.Estoque <= 0)
        {
            yield return new ValidationResult("O estoque deve ser maior que zero", new[] { nameof(this.Estoque) });
        }
    }
}
