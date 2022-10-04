using System.ComponentModel.DataAnnotations;
using WebApiAutores.DTO;
using WebApiAutores.validaciones;

namespace WebApiAutores.Dto
{
    public class LibroDTO
    {
        public int Id { get; set; }
        [PrimeraLetraMatuscula]
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }

        public List<ComentarioDTO> Comentarios { get; set; }

        //paa el mucho a muchos
       // public List<AutorDTO> Autores{ get; set; }
    }
}
