using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeDTORest.DTOs;
using PracticeDTORest.Entidades;

namespace PracticeDTORest.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController :ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(
            ApplicationDbContext context,
             IMapper mapper
            )
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var comentario = await context.Comentarios
                .Where(comentarioDb => comentarioDb.LibroId == libroId).ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(comentario);
        }



        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);
            if(!existeLibro)
            {
                return NotFound($"No existe un libro por el identificador {libroId}");
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            context.Add(comentario);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);
            if (!existeLibro)
            {
                return NotFound($"No existe un libro por el identificador {libroId}");
            }

            var existeComentario = await context
                .Comentarios.AnyAsync(comentarioDB => comentarioDB.Id == id);

            if (!existeComentario)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.Id = id;
            comentario.LibroId = libroId;
            context.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();
        }

       

    }
}
