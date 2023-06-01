using Codebridge_Test_REST_API.Domain.Enteties;

namespace Codebridge_Test_REST_API.Models
{
    public static class DogExtension
    {
        public static Dog FromDTO(this Dog dog, DogModel dogModel)
        {
            return new Dog()
            {
                Name = dogModel.Name,
                Color = dogModel.Color,
                Tail_Length = dogModel.Tail_Length,
                Weight = dogModel.Weight
            };
        }
    }
}
