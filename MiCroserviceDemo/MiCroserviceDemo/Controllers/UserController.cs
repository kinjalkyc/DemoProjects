using Microsoft.AspNetCore.Mvc;

namespace MiCroserviceDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateUser([FromBody] string name)
        {
            var user = new User { Id = Guid.NewGuid(), Name = name };

            // Simulate saving to a database 

            return Ok(user);
        }

    }
}
