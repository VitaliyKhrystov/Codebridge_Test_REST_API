using Codebridge_Test_REST_API.Domain.Enteties;
using Codebridge_Test_REST_API.Domain.Repositories;
using Codebridge_Test_REST_API.Domain.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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

        [HttpGet ("ping")]
        public IActionResult Ping() 
        {
            return Ok("Dogs house service. Version 1.0.1");
        }

        [HttpGet("dogs")]
        public async Task<ActionResult<IEnumerable<Dog>>> Get(string? attribute, string? order, int? pageNumber, int? pageSize)
        {
            var dogs = await dogRepository.GetAllDogsAsync();

            if (dogs == null)
                return Ok("The list of dogs is empty!");

            dogs = Sorting(dogs, attribute, order);
            dogs = Paging(dogs, pageNumber, pageSize);

            return Ok(dogs);
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



        //var isAttributeName = dogs.First().GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        //                               .Any(d => d.Name.Substring(d.Name.IndexOf('<') + 1, d.Name.IndexOf('>') - 1).ToLower() == attribute.ToLower());

        //var attributeName = isAttributeName ? attribute : null;
        //var orderBy = order.IsNullOrEmpty() || order.ToLower() == "desc" ? order : "asc";
    }
}
