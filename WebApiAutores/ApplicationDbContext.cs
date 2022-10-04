using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores
{

    //public class ApplicationDbContext : DbContext

    //para tener esto debemos descargar el paquete de Microsoft.AspNetCore.Identity.EntityFrameworkCore
   //luego toca ir al paage mannager console y agregamos una migracion add-migration nombre
   //y updeteamos la bbd update-database
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        //elDbContextOptions podemos pasar el conexion string 
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //para relacion muchos a muchos=============================
        //hacemos un override de onmodel Creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //importante del identyty si esta sobreescrito
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutoresLibros>()
                //tienellave(estoy creando una llave primaria compuesta entre autorid y libroid)
                .HasKey(al => new { al.AutorId, al.LibroId });
        }
        //=================================================

        //aki confuiguramos las tablas que generara el entity framework
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros{ get; set; }

        public DbSet<Comentario> Comentarios { get; set; }

        public DbSet<AutoresLibros> AutoresLibros{ get; set; }
       
    }
}
