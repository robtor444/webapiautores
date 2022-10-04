using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dto
{
    public class CredencialesUsuario
    {

        //lo que necesita para autenticar
        //se

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
