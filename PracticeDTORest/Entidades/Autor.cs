﻿using System.ComponentModel.DataAnnotations;

namespace PracticeDTORest.Entidades
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]   
        public string Nombre { get; set; }  

        public List<Libro> Libros { get; set; } 

        public List<AutorLibro> AutoresLibros { get; set; }   
       
    }
}
