using System.ComponentModel.DataAnnotations;
using WebApiAutores.validaciones;

namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [PrimeraLetraMatuscula]
        [StringLength(maximumLength:250)]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }


        //propiedad de navegacion
        public List<Comentario> Comentarios { get; set; }

        //para el muchos a muchos d autor libro
        public List<AutoresLibros> AutoresLibros { get; set; }


        //  public int Id { get; set; }

        // [Required(ErrorMessage ="El campo es Requerido")]
        // [MaxLength(10,ErrorMessage ="solo son 10 letras")]
        // public string? Nombre { get; set; }

        // [Required(ErrorMessage = "El campo es Requerido")]
        // [StringLength(maximumLength:10,ErrorMessage ="Solo 10 caracteres")]
        // public string? Descripcion { get; set; }

      
        // [DefaultValue(false)]
        // public Boolean Realizado { get; set; }   


    }
}
