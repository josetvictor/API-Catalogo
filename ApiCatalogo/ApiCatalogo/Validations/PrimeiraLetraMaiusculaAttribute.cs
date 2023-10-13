using System.ComponentModel.DataAnnotations;

namespace APICatalogo_essencial.Net6.Validations;
public class PrimeiraLetraMaiusculaAtttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if(value == null || string.IsNullOrEmpty(value.ToString())){
            return ValidationResult.Success;
        }

        var primeiraLetra = value.ToString()[0].ToString();
        if(primeiraLetra != primeiraLetra.ToUpper()){
            return new ValidationResult("A primeiraLetra do nome do produto deve ser maiúscula");
        }

        return ValidationResult.Success;
    }
}
