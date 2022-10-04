using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiAutoresTest.PruebasUnitarias.Mocks
{
    public class URLHelperMock : IUrlHelper
    {
        public ActionContext ActionContext => throw new NotImplementedException();

        public string? Action(UrlActionContext actionContext)
        {
            throw new NotImplementedException();
        }

        [return: NotNullIfNotNull("contentPath")]
        public string? Content(string? contentPath)
        {
            throw new NotImplementedException();
        }

        public bool IsLocalUrl([NotNullWhen(true)] string? url)
        {
            throw new NotImplementedException();
        }


        /// este es el metodo que usamos y que debo concentrarme
        public string? Link(string? routeName, object? values)
        {
            //lo que me interasa que no bote un erro asi que devlvere un string vacio
            return "";
        }

        public string? RouteUrl(UrlRouteContext routeContext)
        {
            throw new NotImplementedException();
        }
    }
}
