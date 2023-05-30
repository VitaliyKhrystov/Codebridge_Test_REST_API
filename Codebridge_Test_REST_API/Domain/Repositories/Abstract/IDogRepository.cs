using Codebridge_Test_REST_API.Domain.Enteties;

namespace Codebridge_Test_REST_API.Domain.Repositories.Abstract
{
    public interface IDogRepository
    {
        Task CreateDogAsync(Dog dog);
        Task UpdateDogAsync(Dog dog);
        Task<Dog> GetDogByNameAsync(string name);
        Task<IEnumerable<Dog>> GetAllDogsAsync();
        Task DeleteDogAsync(string name);
    }
}
