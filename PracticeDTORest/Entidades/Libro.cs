using System.ComponentModel.DataAnnotations;

namespace PracticeDTORest.Entidades
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:250)]
        public string Titulo { get; set; }


        public List<Comentario> Comentarios { get; set; }



    }
}
