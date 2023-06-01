using Codebridge_Test_REST_API.Domain.Enteties;
using Codebridge_Test_REST_API.Domain.Repositories.Abstract;
using Codebridge_Test_REST_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Codebridge_Test_REST_API.Controllers
{
    [Route("/")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IDogRepository dogRepository;
        private readonly ILogger<BaseController> logger;

        public BaseController(IDogRepository dogRepository, ILogger<BaseController> logger)
        {
            this.dogRepository = dogRepository;
            this.logger = logger;
        }

        // curl -X GET https://localhost:44336/ping   
        [HttpGet ("ping")]
        public IActionResult Ping() 
        {
            return Ok("Dogs house service. Version 1.0.1");
        }

        // curl -X GET https://localhost:44336/dogs
        // curl -X GET http://localhost:44336/dogs?attribute=weight&order=desc
        // curl -X GET http://localhost:44336/dogs?pageNumber=1&pageSize=2
        // curl -X GET https://localhost:44336/dogs?attribute=name&order=asc&pageNumber=1&pageSize=3
        [HttpGet("dogs")]
        public async Task<ActionResult<IEnumerable<Dog>>> GetGogs(string? attribute, string? order, int? pageNumber, int? pageSize)
        {
            try
            {
                var dogs = await dogRepository.GetAllDogsAsync();

                if (dogs == null)
                    return Ok("The list is empty!");

                dogs = Sorting(dogs, attribute, order);
                dogs = Paging(dogs, pageNumber, pageSize);

                return Ok(dogs.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest();
            }
            
        }

        //curl -X POST https://localhost:44336/dog -H 'Content-Type: application/json' -d '{"Name": "Don", "Color": "Brown", "Tail_Length": 17, "Weight": 10}'
        [HttpPost ("dog")]
        public async Task<IActionResult> CreateDog(DogModel dogModel)
        {
            if (await dogRepository.GetDogByNameAsync(dogModel.Name) != null)
            {
                ModelState.AddModelError("", "The dog with the same name already exists in DB!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var dog = new Dog().FromDTO(dogModel);
                    await dogRepository.CreateDogAsync(dog);

                    return Ok("The dog has been created!");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return BadRequest();
                }
            }
            else
            {
                var list = new List<ModelError>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        list.Add(error);
                    }
                }
                return BadRequest(list);
            }
        }

        protected IEnumerable<Dog> Sorting(IEnumerable<Dog> list, string? attribute, string? order)
        {
            if (attribute != null && order != null)
            {
                switch (attribute.ToLower())
                {
                    case "name":
                        if (order.ToLower() == "asc")
                        {
                            return list.OrderBy(x => x.Name);
                        }
                        else if (order.ToLower() == "desc")
                        {
                            return list.OrderByDescending(x => x.Name);
                        }
                        break;
                    case "color":
                        if (order.ToLower() == "asc")
                        {
                           return list.OrderBy(x => x.Color);
                        }
                        else if (order.ToLower() == "desc")
                        {
                            return list.OrderByDescending(x => x.Color);
                        }
                        break;
                    case "tail_length":
                        if (order.ToLower() == "asc")
                        {
                            return list.OrderBy(x => x.Tail_Length);
                        }
                        else if (order.ToLower() == "desc")
                        {
                            return list.OrderByDescending(x => x.Tail_Length);
                        }
                        break;
                    case "weight":
                        if (order.ToLower() == "asc")
                        {
                            return list.OrderBy(x => x.Weight);
                        }
                        else if (order.ToLower() == "desc")
                        {
                            return list.OrderByDescending(x => x.Weight);
                        }
                        break;
                }
            }
            return list;
        }

        protected IEnumerable<Dog> Paging(IEnumerable<Dog> list, int? pageNumber, int? pageSize)
        {
            if (pageNumber == null || pageSize == null)
                return list;

            var pageCount = list.Count();
            var pageSizeCount = pageSize > 0 ? pageSize : 1;
            var pageNumb = pageNumber > 0 && pageNumber <= (Math.Ceiling((double)pageCount / (double)pageSizeCount)) ? pageNumber : 1;

            return list.Skip((int)(pageNumb - 1) * (int)pageSizeCount).Take((int)pageSizeCount);

        }

    }
}
