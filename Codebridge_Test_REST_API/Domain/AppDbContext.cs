using Codebridge_Test_REST_API.Domain.Enteties;
using Microsoft.EntityFrameworkCore;

namespace Codebridge_Test_REST_API.Domain
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<Dog> Dogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var firstDog = new Dog()
            {
                Name = "Neo",
                Color = "red & amber",
                Tail_Length = 22,
                Weight = 32
            };
            var secondDog = new Dog()
            {
                Name = "Jessy",
                Color = "black & white",
                Tail_Length = 7,
                Weight = 14
            };

            modelBuilder.Entity<Dog>().HasData(new List<Dog>() { firstDog, secondDog });
        }

    }
}
