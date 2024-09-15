using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZadElealam.Apis.Models;
using ZadElealam.Core.Models;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private List<Item> items = new List<Item>()
        {
            new Item { Id = 1, Name = "Item 1", Description = "Description 1" },
            new Item { Id = 2, Name = "Item 2", Description = "Description 2" },
            new Item { Id = 3, Name = "Item 3", Description = "Description 3" },
        };

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public IActionResult Get()
        {
            return Ok(items);
        }
    }
}
