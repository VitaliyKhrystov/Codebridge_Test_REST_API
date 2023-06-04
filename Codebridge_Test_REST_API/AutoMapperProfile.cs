using AutoMapper;
using Codebridge_Test_REST_API.Domain.Enteties;
using Codebridge_Test_REST_API.Models;

namespace Codebridge_Test_REST_API
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Dog, DogDTO>();
            CreateMap<DogDTO, Dog>();
        }
    }
}
