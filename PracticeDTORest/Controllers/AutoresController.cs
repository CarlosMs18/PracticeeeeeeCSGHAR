using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeDTORest.DTOs;
using PracticeDTORest.Entidades;

namespace PracticeDTORest.Controllers
{

    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(
            ApplicationDbContext context,
            IMapper mapper,
            IConfiguration configuration
            )
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet("configuraciones")]
        public ActionResult<string> ObtenerConfiguracion()
        {
            //return configuration["ConnectionStrings:defaultConnection"];
            return configuration["apellido"];
        }

        [HttpGet]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>( autores );
        }


        [HttpGet("{id:int}", Name = "ObtenerPorAutor")]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            var autor = await context.Autores
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Libro)
                //.ThenInclude(autorLibroDB => autorLibroDB.Libro)
                 .FirstOrDefaultAsync(autorDB => autorDB.Id == id); 
            if(autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOConLibros>(autor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);
            await context.SaveChangesAsync();
            //return Ok();

            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("ObtenerPorAutor", new {id = autor.Id}, autorDTO);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            var autor = await context.Autores.AnyAsync(autorDB => autorDB.Id == id);
            if(!autor)
            {
                return NotFound();
            }

            var autorActualizacion = mapper.Map<Autor>(autorCreacionDTO);
            autorActualizacion.Id = id;
            context.Update(autorActualizacion);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var autor = await context.Autores.AnyAsync(autorDB => autorDB.Id == id);
            if (!autor)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();

        }
    }
}
