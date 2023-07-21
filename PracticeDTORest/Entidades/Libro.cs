using System.ComponentModel.DataAnnotations;

namespace PracticeDTORest.Entidades
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:250)]
        public string Titulo { get; set; }

        //public int AutorId { get; set; }    
        //public Autor AutorCreador { get; set; }

        public List<Comentario> Comentarios { get; set; }

        public List<AutorLibro> AutoresLibros { get; set; }

    }
}
