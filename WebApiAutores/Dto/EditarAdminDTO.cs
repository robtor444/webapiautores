using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dto
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
