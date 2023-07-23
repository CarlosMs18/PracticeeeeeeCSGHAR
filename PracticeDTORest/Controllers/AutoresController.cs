using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeDTORest.DTOs;
using PracticeDTORest.Entidades;
using Microsoft.AspNetCore.Authentication;

namespace PracticeDTORest.Controllers
{

    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(
            ApplicationDbContext context,
            IMapper mapper,
            IConfiguration configuration,
            IAuthorizationService authorizationService
            )
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
        }

        //[HttpGet("configuraciones")]
        //public ActionResult<string> ObtenerConfiguracion()
        //{
        //    //return configuration["ConnectionStrings:defaultConnection"];
        //    return configuration["apellido"];
        //}

        [HttpGet(Name = "obtenerAutores")]
        [AllowAnonymous]
        //public async Task<ActionResult<List<AutorDTO>>> Get()
        public async Task<ColeccionDeRecursos<AutorDTO>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            //return mapper.Map<List<AutorDTO>>( autores );

            var dtos =  mapper.Map<List<AutorDTO>>(autores);
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            dtos.ForEach(dto => GenerarEnlace(dto, esAdmin.Succeeded));

            var resultado = new ColeccionDeRecursos<AutorDTO> { Valores = dtos };
            resultado.Enlaces.Add(new DatoHATEOAS(
                enlace: Url.Link("obtenerAutores", new { }), descripcion: "self", metodo: "GET"));


            if (esAdmin.Succeeded)
            {
                resultado.Enlaces.Add(new DatoHATEOAS(
                enlace: Url.Link("obtenerAutores", new { }), descripcion: "crear-autor", metodo: "POST"));

            }

            
            //return dtos;
            return resultado;
        }


       


        [HttpGet("{id:int}", Name = "ObtenerAutor")]
        [AllowAnonymous]
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

            /*return mapper.Map<AutorDTOConLibros>(autor)*/;
            var dto = mapper.Map<AutorDTOConLibros>(autor);
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
            GenerarEnlace(dto, esAdmin.Succeeded);
            return dto;
        }


        private void GenerarEnlace(AutorDTO autorDTO, bool esAdmin)
        {
            autorDTO.Enlaces.Add(new DatoHATEOAS(
                enlace: Url.Link("obtenerAutor", new { id = autorDTO.Id }),
                descripcion: "self", metodo: "GET"));

            if (esAdmin)
            {
                autorDTO.Enlaces.Add(new DatoHATEOAS(
               enlace: Url.Link("actualizarAutor", new { id = autorDTO.Id }),
               descripcion: "autor-actualizar", metodo: "PUT"));


                autorDTO.Enlaces.Add(new DatoHATEOAS(
                   enlace: Url.Link("borrarAutor", new { id = autorDTO.Id }),
                   descripcion: "self", metodo: "DELETE"));
            }

            
        }









        [HttpPost(Name ="crearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);
            await context.SaveChangesAsync();
            //return Ok();

            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("ObtenerPorAutor", new {id = autor.Id}, autorDTO);

        }

        [HttpPut("{id:int}",Name = "actualizarAutor")]
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

        [HttpDelete("{id:int}", Name = "borrarAutor")]
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
