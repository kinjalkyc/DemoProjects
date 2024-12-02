namespace MiCroserviceDemo.Class
{
    public class OrderService
    {
        public void CreateOrder(Guid userId, string orderDetails)
        {
            // Simulate saving to a database
            Console.WriteLine($"Order for User ID {userId} created with details: {orderDetails}");
        }
    }
}
