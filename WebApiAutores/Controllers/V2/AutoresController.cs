using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiAutores.Dto;
using WebApiAutores.DTO;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;
using WebApiAutores.Utilidades;

namespace WebApiAutores.Controllers.V2
{
    [ApiController]
    [Route("api/v2/autores")]

    //[Route("api/autores")]
    [CabeceraEstaPresenteAtribute("x-version","2")]
    // filtro esto protege la autorizacion para este controlador
    //[Authorize]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Policy ="EsAdmin")]
    public class AutoresController:ControllerBase
    {
       private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        //private readonly IServicio servicio;
        //private readonly ServicioTransient servicioTransient;
        //private readonly ServicioSingleton servicioSingleton;
        //private readonly ServicioScope servicioScope;
        //private readonly ILogger<AutoresController> logger;

        //aki es sin api con datos estaticos========================
        //private List<Autor> listaAutor = new List<Autor>() {
        //        new Autor(){Id = 1,Nombre="Felipe"},
        //          new Autor(){Id = 2,Nombre="Carlos"},
        //    };


        //[HttpGet]
        //public ActionResult<List<Autor>> Get()
        //{


        //    return listaAutor;
        //}
        //[HttpGet("{id}")]
        //public ActionResult<Autor> Get(int id)
        //{


        //    var autor= listaAutor.Find(i=>i.Id==id);
        //    return autor;
        //}



        // [HttpPost]
        // public ActionResult<List<Cliente>> PostCliente(Cliente cliente)
        // {
        //     var clienteBuscado = listaCliente.Find(clientebuscado => clientebuscado.IdCliente == cliente.IdCliente);
        //     if (clienteBuscado != null)
        //     {
        //         return NotFound("ya existe el cliente");
        //     }
        //     listaCliente.Add(cliente);

        //     return listaCliente;
        // }

        //aki fin  es sin api con datos estaticos========================



        public AutoresController(ApplicationDbContext context,
            IMapper mapper ,
            // el que es de configuration extension no el de automapper
            IConfiguration configuration
            //, IServicio servicio,
            //ServicioTransient servicioTransient,
            //ServicioSingleton servicioSingleton,
            //ServicioScope servicioScope,
            //ILogger<AutoresController> logger
            )
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            //this.servicio = servicio;
            //this.servicioTransient = servicioTransient;
            //this.servicioSingleton = servicioSingleton;
            //this.servicioScope = servicioScope;
            //this.logger = logger;
        }


        //Ejemplos de servicoos==================

        //[HttpGet("GUID")]
        //filtro de cahe
        //quiere decir que la respuesta de esto guardara en cche y si hay otra solicitud en los 10 prox segunt
        //se servidra de la misma respuesta
        //[ResponseCache(Duration =10)]

        // filtro esto protege la autorizacion para esta accion
        //[Authorize]
        //public ActionResult ObtenerGuids()
        //{
        //    return Ok(new 
        //    {
        //        AutoresControllerTransient=servicioTransient.guid,
        //        ServicioA_Transient = servicio.ObtenerTransient(),
        //        AutoresControllerScoped =servicioScope.guid,
        //        ServicioA_Scope = servicio.ObtenerScope(),
        //        AutoresControllerSingleton = servicioSingleton.guid,   
        //        ServicioA_Singleton= servicio.ObtenerSingleton(),

        //    });
        //}

        //==================

        [HttpGet("configuraciones")]

        //permitir anonymos es decir que permite cuando esta protegido todo el controller ingresar sin token
        [AllowAnonymous]
        public ActionResult<string> ObtenerConfiguracion()
        {
            //configuration el configuration puedes acceder 
            //    a variables appjson, usuarios secretos y variables de ambientes 
            return configuration["apellido1"];
           // return configuration["connectionStrings:defaultConnection"];

        }



        [HttpGet(Name ="ObtenerAutoresv2")]
        //autenticatioscheme=CookieBuilder estamos utilizando el addIdentity en la clase Startup.cs

        //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        //filtro creado uno mismo
        [ServiceFilter(typeof(MisFiltrosDeAccion))]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            //controlar el filtro global
            //throw new NotImplementedException();

            //logger ter permite tener mensajes en la consola 
            //logger.LogInformation("Estamos obteniendo autores");
            //logger puede hacer tambien en bbd 

            //return await context.Autores.Include(l=>l.Libros).ToListAsync();

            var autores= await context.Autores.ToListAsync();

            //*****************************************Diferencia de la version v1 con esta v2****************

            //cada autor pondre el nombre en mayusculas
            autores.ForEach(autor => autor.Nombre = autor.Nombre.ToUpper());

            //*******************************************************************************************

            return mapper.Map<List<AutorDTO>>(autores);
        }

        //en caso que no quiera que sea obligatoria
        //[HttpGet("{id:int}/{param2=persona}")]

        ////model building Ejemplos [FromRoute], [FromBody]

        //public async Task<ActionResult<Autor>> GetById([FromRoute]int id,[FromRoute]string param2)
        //{
        //    var buscado = await context.Autores.FirstOrDefaultAsync(x=>x.Id==id);

        //    if (buscado == null) 
        //        return NotFound("No Encontrado");

        //    return Ok(buscado);
        //}

        [HttpGet("{id:int}",Name ="ObtenerAutorv2")]
        public async Task<ActionResult<AutorDTOConLibros>> GetById([FromRoute] int id )
        {
            var buscadoAutor = await context.Autores
                .Include(autorDb=>autorDb.AutoresLibros)
                .ThenInclude(autorLibroDb=> autorLibroDb.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);

            var buscado = mapper.Map<AutorDTOConLibros>(buscadoAutor);

            if (buscado == null)
                return NotFound("No Encontrado");

            return Ok(buscado);
        }

        //no existe una validacionstring
        [HttpGet("{name}", Name = "ObtenerAutoresPorNombrev2")]
        public async Task<ActionResult<List<AutorDTO>>> GetByName(string name)
        {
            //aki solo me trae el primero
            //var buscado = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Nombre.Contains(name));

            //aki me va a traer todos los que tengan ese nombre
            var buscado = await context.Autores.Where(autoresBd => autoresBd.Nombre.Contains(name)).ToListAsync();
            if (buscado == null)
                return NotFound("No Encontrado");

            var resultado = mapper.Map<AutorDTO>(buscado);
            return Ok(resultado);
        }

        [HttpGet("primero")]
        //FromHeader es desde la cabecera el valor
        //[FromQuery] es decir en la ruta api/ruta/?nombre
        public async Task<ActionResult<Autor>> GetAutor([FromHeader] int miValor,[FromQuery] string nombre)
        {
            var buscado = await context.Autores.FirstOrDefaultAsync(x => x.Id==miValor);
            var buscado2 = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (buscado == null || buscado2==null)
                return NotFound("No Encontrado");

            Autor[] respuesta = new Autor[] { buscado, buscado2 }; 

            return Ok( respuesta);
        }


        [HttpPost(Name = "CrearAutoresv2")]
       public async Task<ActionResult> Post([FromBody]AutorCreacionDTO autorCreacion)
       {
            //validat por contrlador
            //validacion de autor no exista un autor con el mismo nombre creado
            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre.ToLower() == autorCreacion.Nombre.ToLower());
            if (existeAutorConElMismoNombre==true)
            {
                return BadRequest("El autor ya esta registrado");
            }

            //sin automapper
            //var autor = new Autor
            //{
            //    Nombre = autorCreacion.Nombre
            //};


            //con automapper 
            //<conversionDestion>(de donde proviene)
            var autor = mapper.Map<Autor>(autorCreacion);


            context.Add(autor);

            await context.SaveChangesAsync();

            //recuerda que el autor que mandas es de tipo autor y no autor DTO
            var autorDto = mapper.Map<AutorDTO>(autor);

            //createAtRoute(nombre de ruta,valor de ruta que quirto mandar ejemplo id, devolver el objeto creado en la bbd)
            return CreatedAtRoute("ObtenerAutorv2",new { id=autor.Id},autorDto);
       }

        [HttpPut("{id:int}", Name = "ActualizarAutoresv2")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDto,int id)
        {
            var existeAutor = await context.Autores.AnyAsync(autorDb => autorDb.Id == id);
            if (!existeAutor)
            {
                return NotFound("No se encontro el autor");
            }

            var autor = mapper.Map<Autor>(autorCreacionDto);
            autor.Id = id;

            context.Update(autor);

            await context.SaveChangesAsync();
           
            
            
            //asi se llena en swagger===================
            

            //"path": "/titulo",
            //"op": "replace",
    
            //"value": "Titulo desde patch"
            //======================================
  
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "BorrarAutoresv2")]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound("El id de autor no existe en la bbd");
            }
            context.Remove(new Autor() { Id=id});

            await context.SaveChangesAsync();
            return Ok("Eliminado correctamente");
        }
    }
}
