using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.validaciones
{
    //validateatribute es para que pueas crar validaciones
    public class PrimeraLetraMatusculaAttribute:ValidationAttribute
    {
        //(value=valor del atributo, validationContext=tengo acceso a valores como las entidades)
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //creamos mi logica de validacion
            if (value==null || string.IsNullOrEmpty(value.ToString()))
            {
                // si el valor es nullo ponemos que es correcto
                // /por que validams  que sea mayuscua la primera no que exista o no

                return ValidationResult.Success;
            }

            //retorna la primera letra
            var primeraLetra= value.ToString()[0].ToString();

            if (primeraLetra !=primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayuscula");            
            }
            else
            {
                return ValidationResult.Success;
            }


        }
    }
}
