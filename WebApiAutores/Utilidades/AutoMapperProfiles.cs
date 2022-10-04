using AutoMapper;
using WebApiAutores.Dto;
using WebApiAutores.DTO;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles:Profile
    {

        public AutoMapperProfiles()
        {
            //aki va los mapper
            //<fuente(de donde voy amapiar ),destino(hacia donde)>
            CreateMap<AutorCreacionDTO, Autor>();

            //X QUE RECIBIMOS UN AUTOR Y LO QUE REMOS HACER AUTOR DTO

            CreateMap<Autor, AutorDTO>();


            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autor=>autor.Libros,opciones=>opciones.MapFrom(MapAutorDTOLibros));

            //LA MISMA LOGICA QUE ARRIGA
            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));

            //para el get
            CreateMap<Libro, LibroDTO>();

            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(libroDTO => libroDTO.Autores, opciones => opciones.MapFrom(MapLibroDtoAutores));


            CreateMap<ComentarioCreacion,Comentario >();
            CreateMap< Comentario,ComentarioDTO>();

            CreateMap<LibrosPatchDTO, Libro>().ReverseMap();


            //configuracion 
        }

        private List<AutoresLibros> MapAutoresLibros(LibroCreacionDTO libroCreacionDto,Libro libro)
        {
            //retornar el alistado de autor libro
            var resultado=new List<AutoresLibros>();

            if (libroCreacionDto.AutoresIds==null)
            {
                return resultado;
            }

            foreach ( var autorId in libroCreacionDto.AutoresIds)
            {
                resultado.Add(new AutoresLibros() { AutorId = autorId });
            }
            return resultado;
        }

        private List<AutorDTO> MapLibroDtoAutores(Libro libro,LibroDTOConAutores libroDTO)
        {
            var resultado= new List<AutorDTO>();

            if (libro.AutoresLibros == null) { return resultado; }

            foreach (var autorlibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO()
                { 
                    Id = autorlibro.AutorId,
                    //ingresamos a entidad autor y luego a nombre
                    Nombre=autorlibro.Autor.Nombre
                });
            }
            return resultado;

        }


        private List<LibroDTO> MapAutorDTOLibros(Autor autor,AutorDTOConLibros autorDTO)
        {
            var resultado = new List<LibroDTO>();

            if (autor.AutoresLibros == null) { return resultado; }

            foreach (var autorLibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO()
                {
                    Id=autorLibro.LibroId,
                    //ingresamos a entidad autor y luego a nombre
                    Titulo = autorLibro.Libro.Titulo
                });
            }
            return resultado;

        }
    }
}
