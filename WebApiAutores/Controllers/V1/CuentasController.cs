using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAutores.Dto;

namespace WebApiAutores.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IDataProtector dataProtector;

        //para crear el controlador crearemos un tabla y dto

        //para que funcione el identyti
        public CuentasController(
            //servicio que me permite registrar un usuario
            UserManager<IdentityUser> userManager
            , IConfiguration configuration, 
            //sirve para hacer logion al usuario
            SignInManager<IdentityUser> signInManager
            , IDataProtectionProvider dataProtectionProvider )
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;

            //para el dataproteccionprovider(string de proposito=parate de la llave de algo rit de encriptacion)
            //edvuelve un IDataProtector
           dataProtector= dataProtectionProvider.CreateProtector("valor_unico_secreto");
        }


        // este es un ejemplo ara encriptar
        [HttpGet("encriptar",Name ="EncriptarEjemplo")]
        public ActionResult Encriptar()
        {
            var textoPlano = "Sebastian";
            //encripta
            var textoCifrado = dataProtector.Protect(textoPlano);
            //desencriptar texto
            var textoDecencriptado = dataProtector.Unprotect(textoCifrado);

            return Ok(new
            {
                textoPlano = textoPlano,
                textoCifrado = textoCifrado,
                textoDecencriptado = textoDecencriptado
            });

        }



        [HttpGet("encriptarxtiempo", Name = "EncriptarPorTiempoEjemplo")]
        public ActionResult EncriptarPorTiempo()
        {

            //porctor l¿pi tiempo
            var protectorPorTiempo = dataProtector.ToTimeLimitedDataProtector();

            var textoPlano = "Sebastian";
            var textoCifrado = protectorPorTiempo.Protect(textoPlano,lifetime:TimeSpan.FromSeconds(2));
            //desencriptar texto
            //se duerme 6 seg la aplicacion
            Thread.Sleep(3000);

            var textoDecencriptado = protectorPorTiempo.Unprotect(textoCifrado);

            return Ok(new
            {
                textoPlano = textoPlano,
                textoCifrado = textoCifrado,
                textoDecencriptado = textoDecencriptado
            });

        }


        [HttpPost("registrar", Name = "Registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
        {
            //creamos el usuario con email y usuario que es lo que necesita para autenticarse o registrarse 
            var usuario = new IdentityUser { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };

            //create asyn crear un usuario
            //pasamos el usuario y password
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Password);
            // jsjshjkasfhjhsj
            //si es exitosos
            if (resultado.Succeeded)
            {
                //..aki retornamos el json web token

                //return ConstruirToken(credencialesUsuario);

                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                // sino se puso crear el usuario
                return BadRequest(resultado.Errors);
            }

        }

        [HttpPost("login", Name = "Login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Loguear(CredencialesUsuario credencialesUsuario)
        {


            //pasamos el usuario y password para el login
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email
                // isPersistent:es para cokkie si usamos,lockoutOnFailure: en caso de los intentos de logueo se bloquee al usuario 
                , credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: false);



            //si es exitosos
            if (resultado.Succeeded)
            {
                //..aki retornamos el json web token
                //return ConstruirToken(credencialesUsuario);
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }

        }

        //funcion para que el toke se renueve automaticamente
        [HttpGet("RenovarToken", Name = "RenovarToken")]
        //necesita para renvar que tengas un token valido
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public ActionResult<RespuestaAutenticacion> Renovar()
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            //vamos acceder a los clieims
            //necesito configurar algo primero en Startup en el constructor de Startup.cs
            //accedemos al email
            //para obtener el claims el usuario primero de be autenticarse con usuario y password
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var credencialesUsuario = new CredencialesUsuario()
            {
                Email = email
            };
            //return ConstruirToken(credencialesUsuario);
            return await ConstruirToken(credencialesUsuario);
        }

        [HttpGet("Ver")]
        //necesita para renvar que tengas un token valido
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public ActionResult<RespuestaAutenticacion> Renovar()
        public async Task<ActionResult<RespuestaAutenticacion>> Ver()
        {
            //vamos acceder a los clieims
            //necesito configurar algo primero en Startup en el constructor de Startup.cs
            //accedemos al email
            //para obtener el claims el usuario primero de be autenticarse con usuario y password
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var credencialesUsuario = new CredencialesUsuario()
            {
                Email = email
            };
            //return ConstruirToken(credencialesUsuario);
            return await ConstruirToken(credencialesUsuario);
        }

        
        
        
        
        
        
        
        
        
        
        
        //=============================================================================================
        //=============================================================================================
     
        //funcion para construir Toke
        //private  RespuestaAutenticacion ConstruirToken(CredencialesUsuario credencialesUsuario)

        //Por las funciens del claim se iso asyncrino
        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            //cleim es una informacion acerca del usuario en la cual podemos confiar 
            //el usuario puede leer el claim por eso no se puede poner tarjetas de credito passwords 
            //en el claims
            var claims = new List<Claim>()
            {
                //new Claim("llave","valor")
                new Claim("email",credencialesUsuario.Email),
                new Claim("lo que desee","valo que deseeee no importa")
            };


            //LOS CLAIMS administrador o vendedor ==============================================
            var usuario = await userManager.FindByEmailAsync(credencialesUsuario.Email);

            //trae todo los claims
            var claimsDb=await userManager.GetClaimsAsync(usuario);

            //fucionamos los claim+ claimsDb
            claims.AddRange(claimsDb);

            //==========================================================


            //contruimos el jwt
            //primero vamos al appsettings a cread una llavejwt
            //Symetric... Representa la clase base abstracta para todas las claves que se generan mediante algoritmos simétricos.
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            //cstruimos toke
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion,
            };
        }

        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);

            //hacerlo admin
            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));

            return Ok("Hecho");
        }

        [HttpPost("RemoverAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);

            //hacerlo admin
            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));

            return Ok("Hecho");
        }



    }
}
