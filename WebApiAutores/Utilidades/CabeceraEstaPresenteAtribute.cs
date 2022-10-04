using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace WebApiAutores.Utilidades
{

    //*****************Versionamiento por cabecera*********************
    //IActionConstraint=vamos a poder decir esta caracteristicas
    //ctl mas . para implementar interfaz
    public class CabeceraEstaPresenteAtribute : Attribute, IActionConstraint
    {
        private readonly string cabecera;
        private readonly string valor;

        public CabeceraEstaPresenteAtribute(string cabecera,string valor)
        {
            this.cabecera = cabecera;
            this.valor = valor;
        }

        public int Order => throw new NotImplementedException();

        public bool Accept(ActionConstraintContext context)
        {
            //depende a la cabecera
            var cabeceras = context.RouteContext.HttpContext.Request.Headers;

            if (!cabeceras.ContainsKey(cabecera))
            {
                return false;
            }

            return string.Equals(cabeceras[cabecera], valor, StringComparison.OrdinalIgnoreCase);
        }
    }
}
