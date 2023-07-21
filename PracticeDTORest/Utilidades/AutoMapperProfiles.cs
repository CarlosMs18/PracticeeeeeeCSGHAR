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

            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
                
                

            //Comentario

            CreateMap<ComentarioCreacionDTO , Comentario>();

            CreateMap<Comentario, ComentarioDTO>();

        }


        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();
            if (libroCreacionDTO == null)
            {
                return resultado;
            }

            foreach(var autorId in libroCreacionDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro { AutorId = autorId });    
            }

            return resultado;
        }


    }
}
