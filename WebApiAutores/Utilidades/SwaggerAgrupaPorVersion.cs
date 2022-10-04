using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebApiAutores.Utilidades
{

    //***************lo que son las versiones 
    //ctrl . para implementar la interfas
    public class SwaggerAgrupaPorVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            //esto me dara el namespace
            var namespaceControllador = controller.ControllerType.Namespace; //controller.V1

            //es decir de lo de arriba cogera el ultimo elemento separado por el . es decir V1
            var versionAPI= namespaceControllador.Split(".").Last().ToLower(); //v1

            controller.ApiExplorer.GroupName = versionAPI;

            // luego anda a startup hacia addController
        }   
    }
}
