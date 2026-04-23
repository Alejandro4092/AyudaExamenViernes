using Microsoft.EntityFrameworkCore;

namespace AyudaExamenViernes.Data
{
    public class ComicContext : DbContext
    {
        public ComicContext(DbContextOptions<ComicContext> options) : base(options)
        {
        }
        public DbSet<Models.Comic> Comics { get; set; }
    }
}
