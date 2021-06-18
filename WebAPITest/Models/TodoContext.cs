using Microsoft.EntityFrameworkCore;

namespace WebAPITest.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          

            modelBuilder.Entity<Gateway>().HasMany<Device>(d => d.PeripheralDevices).WithOne().OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);


        }

        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Device> Devices { get; set; }
    }
}