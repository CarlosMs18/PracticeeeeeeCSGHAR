using System.ComponentModel.DataAnnotations;

namespace PracticeDTORest.DTOs
{
    public class LibroPatchDTO
    {

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 250)]

        public string Titulo { get; set; }

        public DateTime FechaPublicacion { get; set; }
    }
}
