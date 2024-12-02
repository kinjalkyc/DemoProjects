using Microsoft.AspNetCore.Mvc;

namespace MiCroserviceDemo.UserService
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateUser([FromBody] string name)
        {
            var user = new User { Id = Guid.NewGuid(), Name = name };
            return Ok(user);
        }
    }
}
