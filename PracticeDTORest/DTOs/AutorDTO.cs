﻿using PracticeDTORest.Entidades;

namespace PracticeDTORest.DTOs
{
    public class AutorDTO : Recurso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        /*public List<LibroDTO> Libros { get; set; }*/
    }
}
