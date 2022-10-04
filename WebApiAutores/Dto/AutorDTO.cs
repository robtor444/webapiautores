using WebApiAutores.Dto;

namespace WebApiAutores.DTO
{
    //public class AutorDTO
    public class AutorDTO:Recurso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }


        //libros listado
       // public List<LibroDTO> Libros{ get; set; }
    }
}
