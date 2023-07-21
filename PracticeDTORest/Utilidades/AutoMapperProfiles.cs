using AutoMapper;
using PracticeDTORest.DTOs;
using PracticeDTORest.Entidades;

namespace PracticeDTORest.Utilidades
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();

            CreateMap<Autor, AutorDTO>();


            //LIBRO
            CreateMap<Libro, LibroDTO>();

            CreateMap<LibroCreacionDTO, Libro>();

            //Comentario

            CreateMap<ComentarioCreacionDTO , Comentario>();

            CreateMap<Comentario, ComentarioDTO>();

        }
    }
}
