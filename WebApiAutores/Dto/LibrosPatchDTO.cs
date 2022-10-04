using System.ComponentModel.DataAnnotations;
using WebApiAutores.validaciones;

namespace WebApiAutores.Dto
{
    public class LibrosPatchDTO
    {
        [PrimeraLetraMatuscula]
        [StringLength(maximumLength: 250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
    }
}
