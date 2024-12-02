using Microsoft.AspNetCore.Mvc;

namespace MiCroserviceDemo.OrderService
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public OrdersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            // Fetch user information from UserService 

            var userResponse = await _httpClient.GetAsync($"http://userservice/api/users/{request.UserId}");

            if (!userResponse.IsSuccessStatusCode)
            {
                return BadRequest("User not found");
            }

            // Simulate saving order to a database 

            return Ok($"Order for User ID {request.UserId} created with details: {request.OrderDetails}");
        }
    }
}
