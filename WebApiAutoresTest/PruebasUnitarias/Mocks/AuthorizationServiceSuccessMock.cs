using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApiAutoresTest.PruebasUnitarias.Mocks
{

    //los mocks reemplazan las dependeicias para hacer las prebas unitarias

    //esta clase devolvera que el usuario siempre tiene permiso de administrador
    public class AuthorizationServiceSuccessMock : IAuthorizationService
    {

        public AuthorizationResult Resultado { get; set; }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            //No me importa lo que manda en los 
            //return Task.FromResult(AuthorizationResult.Success());
            return Task.FromResult(Resultado);
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            return Task.FromResult(Resultado);
        }
    }
}
