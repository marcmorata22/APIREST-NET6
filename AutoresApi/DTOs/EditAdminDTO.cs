using System.ComponentModel.DataAnnotations;

namespace AutoresApi.DTOs
{
    public class EditAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
