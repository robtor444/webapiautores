using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiAutores.Controllers.V1;
using WebApiAutoresTest.PruebasUnitarias.Mocks;

namespace WebApiAutoresTest.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTest
    {
        [TestMethod]
        public async Task SiusuarioEsAdmin_Obtenemos4Links()
        {
            //preparacion
            var authorizationService = new AuthorizationServiceSuccessMock();
            //admin
            authorizationService.Resultado = AuthorizationResult.Success();

            var rootController = new RootController(authorizationService);

            rootController.Url = new URLHelperMock();



            //ejecucion
            var resultado = await rootController.Get();


            //verificacion
            //espero un 5 son 5 link que devuelve el get
            Assert.AreEqual(5,resultado.Value.Count());

        }

        [TestMethod]
        public async Task SiusuarioNOEsAdmin_Obtenemos4Links()
        {
            //preparacion
            var authorizationService = new AuthorizationServiceSuccessMock();

            //no admin
            authorizationService.Resultado = AuthorizationResult.Failed();

            var rootController = new RootController(authorizationService);

            rootController.Url = new URLHelperMock();



            //ejecucion
            var resultado = await rootController.Get();


            //verificacion
            //espero un 5 son 5 link que devuelve el get
            Assert.AreEqual(3, resultado.Value.Count());

        }


        [TestMethod]
        public async Task SiusuarioNOEsAdmin_Obtenemos4Links_UsandoMoq()
        {
            //preparacion
            //new Moq<La interfaz que queremos suplantar>
            var mockAuthorizationService = new Mock<IAuthorizationService>();

            mockAuthorizationService.Setup(x=>x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorizationService.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(),
               It.IsAny<object>(),
               It.IsAny<string>()
               )).Returns(Task.FromResult(AuthorizationResult.Failed()));



            var mockUrlHelper = new Mock<IUrlHelper>();

            mockUrlHelper.Setup(x => 
            x.Link(It.IsAny<string>(),
                It.IsAny<object>())
                ).Returns(string.Empty);

            var rootController = new RootController(mockAuthorizationService.Object);

            rootController.Url = mockUrlHelper.Object;



            //ejecucion
            var resultado = await rootController.Get();


            //verificacion
            //espero un 5 son 5 link que devuelve el get
            Assert.AreEqual(3, resultado.Value.Count());

        }

    }
}
