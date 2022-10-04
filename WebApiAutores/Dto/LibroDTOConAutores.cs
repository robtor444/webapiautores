using WebApiAutores.DTO;

namespace WebApiAutores.Dto
{
    public class LibroDTOConAutores:LibroDTO
    {
        public new List<AutorDTO> Autores { get; set; }
    }
}
