namespace MiCroserviceDemo
{
    public class CreateOrderRequest
    {
        public Guid UserId { get; set; }
        public string OrderDetails { get; set; }
    }
}
