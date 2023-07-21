using PracticeDTORest.Entidades;

namespace PracticeDTORest.DTOs
{
    public class AutorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<Libro> Libros { get; set; }
    }
}
