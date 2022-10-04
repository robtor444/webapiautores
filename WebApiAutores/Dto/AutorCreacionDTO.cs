using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.validaciones;

namespace WebApiAutores.Dto
{
    public class AutorCreacionDTO
    {
        //todo lo que necesito para crear autor
        [Required(ErrorMessage = "El campo nombre es Requerido")]
        //logitud 
        [StringLength(maximumLength: 60, ErrorMessage = "El campo {0} no puede tener menos de {1} caracters")]
        [PrimeraLetraMatuscula]
        public string Nombre { get; set; }

        [NotMapped]
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FechaInicial { get; set; }
        
        [NotMapped]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaFinal { get; set; }
    }
}
