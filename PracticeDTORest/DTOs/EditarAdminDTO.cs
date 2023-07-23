using System.ComponentModel.DataAnnotations;

namespace PracticeDTORest.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }   
    }
}
