using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeDTORest.DTOs;
using PracticeDTORest.Entidades;
using System.Linq;

namespace PracticeDTORest.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<LibroDTO>>> Get()
        {
            var libro = await context.Libros.Include(libroDB => libroDB.Comentarios).ToListAsync();
            return mapper.Map<List<LibroDTO>>(libro);
        }
 
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            var libro = await context.Libros.Include(libroDB => libroDB.Comentarios).FirstOrDefaultAsync(libroDb => libroDb.Id == id);
            if(libro  == null)
            {
                return NotFound();
            }
            return mapper.Map<LibroDTO>(libro);
        }


        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            if(libroCreacionDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }


            var autores = await context.Autores.Where(autorDB
                => libroCreacionDTO.AutoresIds.Contains(autorDB.Id)).Select(x => x.Id).ToListAsync();

            if(libroCreacionDTO.AutoresIds.Count != autores.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroCreacionDTO);

            if(libro.AutoresLibros != null)
            {
                for(int i = 0; i< libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
