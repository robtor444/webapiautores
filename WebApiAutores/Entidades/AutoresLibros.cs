namespace WebApiAutores.Entidades
{
    public class AutoresLibros
    {
        // tabla media entre muchos a muchos
        public int LibroId { get; set; }
        public int AutorId { get; set; }

        //orden de autores
        public int Orden { get; set; }

        public Libro Libro{ get; set; }
        public Autor Autor{ get; set; }
    }
}
