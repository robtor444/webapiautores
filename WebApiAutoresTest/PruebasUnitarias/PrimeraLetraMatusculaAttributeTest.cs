using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.validaciones;

namespace WebApiAutoresTest.PruebasUnitarias
{
    [TestClass]
    public class PrimeraLetraMatusculaAttributeTest
    {
        [TestMethod]
        public void PrimeraLetraMinusculaDevuelveError()
        {
            //Preparacion

            //para instanciar debemos crear una referencia al proyecto padre
            //click derecho en la webApitest
            //agregar _> referenciaAl Proyecto
            //seleccionar el checkbox el proyecto 
            var primeraletraMayuscula = new PrimeraLetraMatusculaAttribute();

            var valor = "felipe";

            var valContext = new ValidationContext(new { Nombre = valor });


            //ejecucion o prueba
            var resultado = primeraletraMayuscula.GetValidationResult(valor, valContext);

            //verificacion
            //assert=clase que me permite hacer verificaciones y si ver no es satisfactoria da un error 
            //(lo esperado, lo obtenido )
            Assert.AreEqual("La primera letra debe ser mayuscula", resultado.ErrorMessage);

        }


        [TestMethod]
        public void ValorNullo_NoDevuelveError()
        {
            //Preparacion

            //para instanciar debemos crear una referencia al proyecto padre
            //click derecho en la webApitest
            //agregar _> referenciaAl Proyecto
            //seleccionar el checkbox el proyecto 
            var primeraletraMayuscula = new PrimeraLetraMatusculaAttribute();

            string valor = null;

            var valContext = new ValidationContext(new { Nombre = valor });


            //ejecucion o prueba
            var resultado = primeraletraMayuscula.GetValidationResult(valor, valContext);

            //verificacion
            //assert=clase que me permite hacer verificaciones y si ver no es satisfactoria da un error 
            //(lo esperado, lo obtenido )
            Assert.IsNull(valor);

        }


        [TestMethod]
        public void ValorConLetraMayuscula_NoDevuelveError()
        {
            //Preparacion

            //para instanciar debemos crear una referencia al proyecto padre
            //click derecho en la webApitest
            //agregar _> referenciaAl Proyecto
            //seleccionar el checkbox el proyecto 
            var primeraletraMayuscula = new PrimeraLetraMatusculaAttribute();

            string valor ="Felipe";

            var valContext = new ValidationContext(new { Nombre = valor });


            //ejecucion o prueba
            var resultado = primeraletraMayuscula.GetValidationResult(valor, valContext);

            //verificacion
            //assert=clase que me permite hacer verificaciones y si ver no es satisfactoria da un error 
            //(lo esperado, lo obtenido )

            //es igual a nullo??
            //es deciq que no hay erropr
            Assert.IsNull(resultado);

        }
    }
}