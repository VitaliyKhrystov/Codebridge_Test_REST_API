using Codebridge_Test_REST_API.Controllers;
using Codebridge_Test_REST_API.Domain.Enteties;
using Codebridge_Test_REST_API.Domain.Repositories.Abstract;
using Codebridge_Test_REST_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Xunit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Codebridge_Test_REST_API.Tests
{
    public class BaseControllerTest
    {
        private readonly Mock<IDogRepository> repository;
        private readonly Mock<Microsoft.Extensions.Logging.ILogger<BaseController>> logger;
        private readonly Mock<IMapper> mapper;
        public BaseControllerTest()
        {
            repository = new Mock<IDogRepository>();
            logger = new Mock<ILogger<BaseController>>();
            mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Ping_CheckStatus_ShouldReturnOk()
        {
            //Arrange     
            BaseController baseController = new BaseController(repository.Object, logger.Object, mapper.Object);

            //Act
            ObjectResult result = baseController.Ping() as ObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Ping_CheckMessage_ShouldReturn_DogsHouseServiceVersion_1_0_1()
        {
            //Arrange
            var message = "Dogs house service. Version 1.0.1";
            BaseController baseController = new BaseController(repository.Object, logger.Object, mapper.Object);

            //Act
            ObjectResult result = baseController.Ping() as ObjectResult;

            //Assert
            Assert.Equal(message, result?.Value);
        }

        [Fact]
        public async Task GetDogsAsync_CheckStatusGetDogs_ShouldReturnOk_AllDogs()
        {
            //Arrange
            repository.Setup(repository => repository.GetAllDogsAsync()).Returns(GetDogsTestAsync());
            BaseController baseController = new BaseController(repository.Object, logger.Object, mapper.Object);

            //Act
            var dogs = await baseController.GetDogsAsync(null, null, null, null);
            var result = dogs.Result as ObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            dogs.Value?.Should().BeEquivalentTo(GetDogsTestAsync().Result,option => option.ComparingByMembers<Dog>());
        }

        [Fact]
        public async Task GetDogsAsync_CheckSortingByWeigthDesc_ShouldReturnOkAndSortedList()
        {
            //Arrange
            var list = await GetDogsTestAsync();
            var dogWithSmallestWeightTest = list.OrderByDescending(d => d.Weight).First();
            repository.Setup(repository => repository.GetAllDogsAsync()).Returns(GetDogsTestAsync());
            BaseController baseController = new BaseController(repository.Object, logger.Object, mapper.Object);

            //Act
            var dogs = await baseController.GetDogsAsync("Weight", "desc", null, null);
            var result = dogs.Result as ObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            var dogList = result.Value as List<Dog>;
            dogList?.First().Should().BeEquivalentTo(dogWithSmallestWeightTest, options => options.ComparingByMembers<Dog>());
        }

    
        [Fact]
        public async Task CreateDogAsync_CheckStatusCreateItem_ShouldReturnStatusOk_GetMessageCreatedItem()
        {
            //Arrange
            var message = "The dog has been created!";
            var dog = new DogDTO() { Name = "Lucy", Color = "white", Tail_Length = 12, Weight = 15 };
            repository.Setup(repository => repository.GetDogByNameAsync(dog.Name)).Returns(GetDogByNameTestAsync(dog.Name));
            BaseController baseController = new BaseController(repository.Object, logger.Object, mapper.Object);

            //Act
            var result = await baseController.CreateDogAsync(dog);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(message, (result as ObjectResult).Value);
        }

        [Fact]
        public async Task CreateDogAsync_SendModelWithExistingDogNameInDBCheckStatus_ShouldReturnStatusBadRequest_GetErrorMessage()
        {
            //Arrange
            var dog = new DogDTO() { Name = "Jonny", Color = "white", Tail_Length = 12, Weight = 15 };
            repository.Setup(repository => repository.GetDogByNameAsync(dog.Name)).Returns(GetDogByNameTestAsync(dog.Name));
            BaseController baseController = new BaseController(repository.Object, logger.Object, mapper.Object);
            var message = $"The dog named {dog.Name} already exists in DB!";

            //Act
            var result = await baseController.CreateDogAsync(dog);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = (((result as ObjectResult)?.Value) as List<ModelError>).First().ErrorMessage;
            Assert.Equal(message, errorMessage);
        }


        private async Task<IEnumerable<Dog>> GetDogsTestAsync()
        {
            await Task.Delay(1);
            var users = new List<Dog>
            {
                new Dog(){Name = "Fonny", Color = "white", Tail_Length = 12, Weight = 15},
                new Dog(){Name = "Donny", Color = "red", Tail_Length = 1, Weight = 3},
                new Dog(){Name = "Jonny", Color = "black", Tail_Length = 50, Weight = 14},
                new Dog(){Name = "Lex", Color = "brown", Tail_Length = 13, Weight = 23},
                new Dog(){Name = "Nick", Color = "white", Tail_Length = 34, Weight = 8},
            };
            return users;
        }

        private async Task<Dog> GetDogByNameTestAsync(string name)
        {
            var dogs = await GetDogsTestAsync();
            return dogs.FirstOrDefault(d => d.Name == name, default);
        }
    }
}