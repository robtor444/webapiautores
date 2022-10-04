using WebApiAutores.DTO;

namespace WebApiAutores.Dto
{
    public class AutorDTOConLibros:AutorDTO
    {
        public  List<LibroDTO> Libros { get; set; }
    }
}
