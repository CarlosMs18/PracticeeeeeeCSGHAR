using Microsoft.EntityFrameworkCore;
using PracticeDTORest.Entidades;

namespace PracticeDTORest
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Autor> Autores { get; set; }
     
    }
}
