using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.Dto;

namespace WebApiAutores.Controllers.V1
{
    //parte de los hateas
    [Route("api/v1")]
    [ApiController]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(
            //deseo ver si esta logueado o no para ver que puedo mostrar
            IAuthorizationService authorizationService
            )
        {
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name ="ObtenerRoot")]
        [AllowAnonymous]
        //public ActionResult<IEnumerable<DatoHateOAS>> Get()
        public async Task<ActionResult<IEnumerable<DatoHateOAS>>> Get()
        {
            var datosHateoas = new List<DatoHateOAS>();

            //("el usuario ", y el nomnre de la politica)
            var esAdmin = await authorizationService.AuthorizeAsync(User, "EsAdmin");

            //aki vamos agregando al datoHateoas
            //urlLink(apuntamos al nombre de la ruta, y pongamos valores en mi caso un objeto anonimo)
            // descricion(lugar donde se encuentra el usuario)
            datosHateoas.Add(new DatoHateOAS(enlace: Url.Link("ObtenerRoot",new {}), descripcion: "self", metodo: "GET"));


            datosHateoas.Add(new DatoHateOAS(enlace: Url.Link("ObtenerAutores", new { }), descripcion: "autores"
                , metodo: "GET"));

            datosHateoas.Add(new DatoHateOAS(enlace: Url.Link("CrearAutores", new { }), descripcion: "autores"
                , metodo: "POST"));

            if (esAdmin.Succeeded)
            {
                datosHateoas.Add(new DatoHateOAS(enlace: Url.Link("CrearLibros", new { }), descripcion: "libros"
              , metodo: "POST"));

                datosHateoas.Add(new DatoHateOAS(enlace: Url.Link("ObtenerLibros", new { }), descripcion: "libros"
                   , metodo: "GET"));
            }
           



            return datosHateoas;
        }
    }
}
