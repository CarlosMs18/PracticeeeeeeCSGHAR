using System.ComponentModel.DataAnnotations;

namespace PracticeDTORest.DTOs
{
    public class LibroCreacionDTO
    {


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }


        public List<int> AutoresIds { get; set; }
        
    }
}
