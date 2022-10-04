using System.ComponentModel.DataAnnotations;
using WebApiAutores.validaciones;

namespace WebApiAutores.Dto
{
    public class LibroCreacionDTO
    {
        [PrimeraLetraMatuscula]
        [StringLength(maximumLength: 250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }

        //como traerr data de autpres si hay id media
        public List<int> AutoresIds { get; set; }
    }
}
