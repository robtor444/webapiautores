using Microsoft.AspNetCore.Identity;

namespace WebApiAutores.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
       public string Contenido { get; set;}

        //a que id de libro le pertenece
        public int LibroId { get; set; }

        //propiedad de navegacion
        public Libro Libro { get; set; }


        //relacion con el usuario
        public string UsuarioId { get; set; }
        public IdentityUser Usuario{ get; set; }

    }
}
