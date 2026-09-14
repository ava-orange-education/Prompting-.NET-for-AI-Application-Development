using Microsoft.AspNetCore.Mvc;
using Ecomerce.API.Repositories;

namespace Ecomerce.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var products = ProductRepository.GetAll();
            return Ok(products);
        }
    }
}
