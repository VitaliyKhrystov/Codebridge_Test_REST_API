using Codebridge_Test_REST_API.Domain.Enteties;
using Codebridge_Test_REST_API.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Codebridge_Test_REST_API.Domain.Repositories
{
    public class DogRepositoryEF : IDogRepository, IDisposable
    {
        private readonly AppDbContext context;

        public DogRepositoryEF(AppDbContext context)
        {
            this.context = context;
        }
        public async Task CreateDogAsync(Dog dog)
        {
            await context.Dogs.AddAsync(dog);
            await context.SaveChangesAsync();
        }
        public async Task<Dog> GetDogByNameAsync(string name)
        {
            return await context.Dogs.FirstOrDefaultAsync(d => d.Name == name, default);
        }

        public async Task<IEnumerable<Dog>> GetAllDogsAsync()
        {
            return await context.Dogs.ToListAsync();
        }

        public async Task UpdateDogAsync(Dog dog)
        {
            if (await context.Dogs.AnyAsync(d => d.Name == dog.Name))
            {
                context.Dogs.Update(dog);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteDogAsync(string name)
        {
            var dog = await GetDogByNameAsync(name);
            if (dog != null) 
            { 
                context.Dogs.Remove(dog);
                await context.SaveChangesAsync();
            };
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
