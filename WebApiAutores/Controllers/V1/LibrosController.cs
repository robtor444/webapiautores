using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiAutores.Dto;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet(Name = "ObtenerLibros")]
        public async Task<ActionResult<List<LibroDTO>>> Get()
        {
            var buscado = await context.Libros.ToListAsync();
            if (buscado is null)
            {
                return NotFound("Libros no encontrados");
            }

            var resultado = mapper.Map<List<LibroDTO>>(buscado);

            return resultado;
        }
        [HttpGet("{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            var buscado = await context.Libros
                .Include(libro => libro.Comentarios)
                .Include(libroDb => libroDb.AutoresLibros)
                .ThenInclude(autorLibroDb => autorLibroDb.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (buscado is null)
            {
                return NotFound("Libro no encontrado");
            }

            //para el orden
            buscado.AutoresLibros = buscado.AutoresLibros.OrderBy(x => x.Orden).ToList();

            var resultado = mapper.Map<LibroDTOConAutores>(buscado);

            return resultado;
        }

        [HttpPost(Name = "CrearLibros")]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            //Se compara la tabla autor con un id de autor si hay 
            //var existeAutor = await context.Autores.AnyAsync(aut => aut.Id == libro.AutorId);

            //if (!existeAutor)
            //{
            //    return BadRequest($"No se encontro un autor para el libro {libro.Titulo}");
            //}
            if (libroCreacionDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            try
            {

                //ver que exista el autor 
                var autoresIds = await context.Autores
                    //anda a la tabla autores y hay que exista el id que mando en la tabla
                    .Where(autorBD => libroCreacionDTO.AutoresIds.Contains(autorBD.Id))
                    //y solo traeme el id del autor osea solo quiero ver si existe
                    .Select(x => x.Id).ToListAsync();

                if (libroCreacionDTO.AutoresIds.Count != autoresIds.Count)
                {
                    return BadRequest("No existe uno de los autores enviados");
                }

                var libroChange = mapper.Map<Libro>(libroCreacionDTO);

                //para el orden
                if (libroChange.AutoresLibros != null)
                {

                    for (int i = 0; i < libroChange.AutoresLibros.Count; i++)
                    {
                        //ordenar
                        libroChange.AutoresLibros[i].Orden = i;
                    }
                }

                context.Add(libroChange);

                var libroDto = mapper.Map<LibroDTO>(libroChange);

                await context.SaveChangesAsync();
                return CreatedAtRoute("ObtenerLibro", new { id = libroChange.Id }, libroDto);
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }

        [HttpPut("{id:int}", Name = "ActualizarLibros")]

        public async Task<ActionResult> PutLibro(int id, LibroCreacionDTO libroCreacionDTO)
        {
            //traigo el libro que coresponde
            var libroDb = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libroDb == null)
            {
                return NotFound("No existe el libro");
            }

            //usar autormapper para llevar las propiedades de libroCreacionDto, hacia libroDb
            libroDb = mapper.Map(libroCreacionDTO, libroDb);

            //funcin para ordenar 
            AsignarOrdenAutores(libroDb);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id:int}", Name = "ActializarCampoLibros")]
        //AspNetCore.Mvc.NewtonsoftJson
        public async Task<ActionResult> PatchLibro(int id, JsonPatchDocument<LibrosPatchDTO> patchDocument)
        {
            if (patchDocument==null)
            {
                return BadRequest("error de formato");
            }

            //traigo el libro que coresponde
            var libroDb = await context.Libros           
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libroDb == null)
            {
                return NotFound("No existe el libro");
            }

            //usar autormapper para llevar las propiedades de libroCreacionDto, hacia libroDb
            var libroDto = mapper.Map<LibrosPatchDTO>( libroDb);

            //aplico a libro dto los cambiso que vinieron en el patch document
            //si existe eun error se coloque en el model state
            patchDocument.ApplyTo(libroDto, ModelState);

            var esValido = TryValidateModel(libroDto);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(libroDto, libroDb);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "BorrarLibros")]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Libros.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound("El id de autor no existe en la bbd");
            }
            context.Remove(new Libro() { Id = id });

            await context.SaveChangesAsync();
            return Ok("Eliminado correctamente");
        }

        private void AsignarOrdenAutores(Libro libroChange)
        {

            //para el orden
            if (libroChange.AutoresLibros != null)
            {

                for (int i = 0; i < libroChange.AutoresLibros.Count; i++)
                {
                    //ordenar
                    libroChange.AutoresLibros[i].Orden = i;
                }
            }
        }
    }
}
