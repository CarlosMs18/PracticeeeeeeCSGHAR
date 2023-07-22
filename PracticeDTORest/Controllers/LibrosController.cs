using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
            var libro = await context.Libros
                                .ToListAsync();
            return mapper.Map<List<LibroDTO>>(libro);
        }
 
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            var libro = await context.Libros
               
                .Include(libroDb => libroDb.AutoresLibros)
                .ThenInclude(autorLibroDb => autorLibroDb.Autor)
                .FirstOrDefaultAsync(libroDb => libroDb.Id == id);


            
            
            if(libro  == null)
            {
                return NotFound();
            }

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();
            return mapper.Map<LibroDTOConAutores>(libro);
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

            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }



        [HttpPut("{id:int}")] //al actualizar libro es especial, ya que al crear un libro aparte de crear un libro
                //creamos a los autores que perteneceran a este
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
        {
            var libroDB = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(libroDB == null ) {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacionDTO, libroDB);

            AsignarOrdenAutores(libroDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if(libroDB == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB); //llenado el libropatchDTO con la informacion del libro de la bbdd

            patchDocument.ApplyTo(libroDTO, ModelState); //luego al libro le estamos aplicacion los cambios provenitneteds del patch document,
                                                    //si indicamos en el patchdocument que cambiaremos el titulo aca es donde se realizara la operacion

            var esValido = TryValidateModel(libroDTO); //validamos si todas las reglas de validcion se estan cumpliendo
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroDB);//mapeamos ahora de libroPatch a libro
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var autor = await context.Libros.AnyAsync(libroDB => libroDB.Id == id);
            if (!autor)
            {
                return NotFound();
            }

            context.Remove(new Libro() { Id = id });
            await context.SaveChangesAsync();
            return Ok();

        }
    }
}
