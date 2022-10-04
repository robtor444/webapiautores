using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dto
{
    public class ComentarioCreacion
    {
        public int LibroId { get; set; }

        [Required]
        [MinLength(1,ErrorMessage ="Almenos debe tener 4 caracteres")]
        public string Contenido { get; set; }
    }
}
