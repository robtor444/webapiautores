using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiAutores.Dto;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers.V1
{
    [Route("api/v1/libros/{libroId:int}/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(ApplicationDbContext context, IMapper mapper,
            //necesitamos algo para a partir del email traiga el id de usuario
            UserManager<IdentityUser> userManager
            )
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet("/comentarios", Name = "ObtenerComentarios")]
        public async Task<ActionResult<List<ComentarioDTO>>> GetComentariosTodos(int libroId)
        {
            //existe el libro
            

            var comentarios = await context.Comentarios.ToListAsync();

            var comentarioDto = mapper.Map<List<ComentarioDTO>>(comentarios);

            return Ok(comentarioDto);
        }


        [HttpGet(Name = "ObtenerComentarioPorLibro")]
        public async Task<ActionResult<List<ComentarioDTO>>> GetComentarios(int libroId)
        {
            //existe el libro
            var existeLibro = await context.Libros.AnyAsync(libroDb => libroDb.Id == libroId);

            if (existeLibro == false)
            {
                return NotFound("Libro que comenta No Existe");
            }

            var comentarios= await context.Comentarios
                .Where(comentarios=>comentarios.LibroId==libroId)
                .ToListAsync();

            var comentarioDto=mapper.Map<List<ComentarioDTO>>(comentarios);

            return Ok(comentarioDto);
        }

        [HttpGet("{id:int}",Name ="ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetComentario(int id)
        {
            //existe el libro
            var existeComentario = await context.Libros.FirstOrDefaultAsync(comentario => comentario.Id == id);
            if (existeComentario == null)
            {
                return NotFound("No existe el libro");
            }
            var comentarioDto = mapper.Map<ComentarioDTO>(existeComentario);

            return Ok(comentarioDto);
        }

        [HttpPost(Name = "CrearComentarios")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId,ComentarioCreacion comentarioDTO )
        {
            //vamos acceder a los clieims
            //necesito configurar algo primero en Startup en el constructor de Startup.cs
            //accedemos al email
            //para obtener el claims el usuario primero de be autenticarse con usuario y password
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email")
            .FirstOrDefault();
;           var email = emailClaim.Value;
            // encontrar el usuario a partir del email
            var usuario =await  userManager.FindByEmailAsync(email);
            var usuarioId=usuario.Id;
            //existe el libro
            var existeLibro = await context.Libros.AnyAsync(libroDb=>libroDb.Id==libroId);

            if (existeLibro==false)
            {
                return NotFound("Libro que comenta No Existe");
            }

            var comentario = mapper.Map<Comentario>(comentarioDTO);
            //relacion con unsuari
            comentario.UsuarioId = usuarioId;
            await context.AddAsync(comentario);


           
            await context.SaveChangesAsync();
            var comentariossDto = mapper.Map<ComentarioDTO>(comentario);
          

            return CreatedAtRoute("ObtenerComentario", new {id=comentario.Id, libroId =libroId},comentariossDto);

            //return Ok();

        }

        [HttpPut("{id:int}", Name = "ActualizarComentarios")]

        public async Task<ActionResult<ComentarioDTO>> UpdateComentario(int libroId,int id,ComentarioDTO comentarioDTOUpdate)
        {

            var existeLibro = await context.Libros.AnyAsync(libroDb => libroDb.Id == libroId);
            if (! existeLibro)
            {
                return NotFound("No se encontro libro");
            }

            var existeComentario = await context.Comentarios.AnyAsync(comentarioDb => comentarioDb.Id == id);
            if (!existeLibro)
            {
                return NotFound("No se encontro libro");
            }

            var comentario = mapper.Map<Comentario>(comentarioDTOUpdate);
            comentario.Id = id;

            context.Comentarios.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();

        }

    }
}
