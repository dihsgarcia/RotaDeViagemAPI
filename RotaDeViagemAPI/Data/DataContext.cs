using Microsoft.EntityFrameworkCore;

namespace RotaDeViagemAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<RotaDeViagem> RotaDeViagem { get; set; }
    }
}
