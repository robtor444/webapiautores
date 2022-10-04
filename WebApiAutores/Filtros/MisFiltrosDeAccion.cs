using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filtros
{
    //aki traiga la importacion
    //luego implemente la interfaz
    public class MisFiltrosDeAccion : IActionFilter
    {

        //debo registrar la inyeccion de dependencias el filtro es decir 
        // en Startup ConfigureServices
        private readonly ILogger<MisFiltrosDeAccion> logger;

        public MisFiltrosDeAccion(ILogger<MisFiltrosDeAccion> logger)
        {
            this.logger = logger;
        }

        //se ejecuta cuando termina la accion de ejecutarse
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Despues de ejecutar la accion");
        }

        // se ejecuta antes de ejecutar la accion
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de ejecutar la accion");
        }
    }
}
