using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.validaciones;

namespace WebApiAutores.Entidades
{
    //para validaciones
        // public class Autor:IValidatableObject
    public class Autor
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="El campo nombre es Requerido")]
        //logitud 
        [StringLength(maximumLength:60,ErrorMessage ="El campo {0} no puede tener menos de {1} caracters")]
        [PrimeraLetraMatuscula]
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage ="Solo letras en el campo")]
        public string Nombre { get; set; }

        [NotMapped]
        
        [DataType(DataType.Date)]
        
        public DateTime FechaInicial { get; set; }
       

        [NotMapped]
        [DataType(DataType.Date)]
        
        public DateTime FechaFinal { get; set; }= DateTime.Now;

        // rango de numeros int
        //[Range(18,120,ErrorMessage ="El campo {0} esta en rango de 18 a 120")]
        ////esta en el modelo pero no lo mapea en la bbd
        //[NotMapped]
        //public int Edad { get; set; }

        ////valida tarjeta de credito
        //[CreditCard]
        //[NotMapped]
        //public string TarjetaDeCredito { get; set; }

        //[Url]
        //[NotMapped]
        //public string URL { get; set; }


        //quiero vaidar que el numero menos sea menos siempre al mayot
        //[NotMapped]
        //public int Menor { get; set; }
        //[NotMapped]
        //public int Mayor { get; set; }

        //public List<Libro> Libros{ get; set; }

        //para el muchis a muchs 
        public List<AutoresLibros> AutoresLibros { get; set; }






        //Validacion por modelo
        //Comparison validaciones que se crean en el prpio modelo
        //1 debemnos instanciar al la entidad con IValidatableObject
        //2 se aplica la interface

        //validate aki ponemos todas las validaciones
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (!string.IsNullOrEmpty(Nombre))
        //    {
        //        // asi coge el primer caracter de nombre
        //        var primeraLetra = Nombre[0].ToString();

        //        if (primeraLetra!= primeraLetra.ToUpper())
        //        {
        //            //(mesaje, campo donde quiere vlidar)
        //            yield return new ValidationResult("La primera letra debe ser mayuscula en campo {0}",new string[] { nameof(Nombre) });
        //        }
        //    }

        //    if (Menor > Mayor)
        //    {
        //        yield return new ValidationResult("El numero menor debe ser siempre menor al mayor",new string[]{nameof(Menor) });
        //    }
        //}

        //problema con este tipo de validaciones que primero debe atrobar todas las que son de atributo
        //una vez aprobado estas se hacen validaciones de modelo
    }
}
