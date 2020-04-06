using Microsoft.EntityFrameworkCore;

namespace Loqr.Models
{
    public class LoqrContext : DbContext
    {
        public DbSet<LoqrItem> LoqrItems { get; set; }

        public LoqrContext(DbContextOptions<LoqrContext> options) : base(options)
        { }
    }
}
