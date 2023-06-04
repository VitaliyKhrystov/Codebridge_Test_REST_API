using Codebridge_Test_REST_API;
using Codebridge_Test_REST_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web.Http;
using Xunit;

namespace Codebridge_Test_REST_API_IntegrationTests
{
    public class BaseControllerIntegrationTest
    {
        private WebApplicationFactory<Program> factory;

        public BaseControllerIntegrationTest()
        {
            factory= new WebApplicationFactory<Program>();
        }

        [Fact]
        public async Task CreateDogAsync_SendEmptyNameForModelDogDTO_ShouldReturnStatusBadRequest_ErrorMessage()
        {
            // Arrange
            var client = factory.CreateClient();
            var dog = new DogDTO() { Name = null, Color = "white", Tail_Length = 5, Weight = 15 };
            var message = "Enter the name of the dog!";

            // Act
            var response = await client.PostAsync("/dog", new StringContent(JsonConvert.SerializeObject(dog), Encoding.UTF8, "application/json"));

            // Assert
            var json = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<ErrorDetails>(json).Errors.Values.First();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(message, errorMessage.First());
            
        }

        [Fact]
        public async Task CreateDogAsync_SendNotValidNameForModelDogDTO_ShouldReturnStatusBadRequest_ErrorMessage()
        {
            // Arrange
            var client = factory.CreateClient();
            var dog = new DogDTO() { Name = "A", Color = "white", Tail_Length = 5, Weight = 15 };
            var message = "The field Name must be a string or array type with a minimum length of '2'.";

            // Act
            var response = await client.PostAsync("/dog", new StringContent(JsonConvert.SerializeObject(dog), Encoding.UTF8, "application/json"));

            // Assert
            var json = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<ErrorDetails>(json).Errors.Values.First();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(message, errorMessage.First());
        }


        [Fact]
        public async Task CreateDogAsync_SendEmptyColorForModelDogDTO_ShouldReturnStatusBadRequest_ErrorMessage()
        {
            // Arrange
            var client = factory.CreateClient();
            var dog = new DogDTO() { Name = "Rocky", Color = null, Tail_Length = 5, Weight = 15 };
            var message = "Enter the color of the dog!";

            // Act
            var response = await client.PostAsync("/dog", new StringContent(JsonConvert.SerializeObject(dog), Encoding.UTF8, "application/json"));

            // Assert
            var json = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<ErrorDetails>(json).Errors.Values.First();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(message, errorMessage.First());

        }

        [Fact]
        public async Task CreateDogAsync_SendNotValidTailLengthForModelDogDTO_ShouldReturnStatusBadRequest_ErrorMessage()
        {
            // Arrange
            var client = factory.CreateClient();
            var dog = new DogDTO() { Name = "Rocky", Color = "black", Tail_Length = -1, Weight = 15 };
            var message = "Specify the length of the tail in the range from 0 to 70 cm!";

            // Act
            var response = await client.PostAsync("/dog", new StringContent(JsonConvert.SerializeObject(dog), Encoding.UTF8, "application/json"));

            // Assert
            var json = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<ErrorDetails>(json).Errors.Values.First();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(message, errorMessage.First());

        }

        [Fact]
        public async Task CreateDogAsync_SendNotValidWeightForModelDogDTO_ShouldReturnStatusBadRequest_ErrorMessage()
        {
            // Arrange
            var client = factory.CreateClient();
            var dog = new DogDTO() { Name = "Rocky", Color = "black", Tail_Length = 5, Weight = 160 };
            var message = "Specify the weight of the dog in the range from 1 to 150 kg!";

            // Act
            var response = await client.PostAsync("/dog", new StringContent(JsonConvert.SerializeObject(dog), Encoding.UTF8, "application/json"));

            // Assert
            var json = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<ErrorDetails>(json).Errors.Values.First();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(message, errorMessage.First());

        }
    }
}