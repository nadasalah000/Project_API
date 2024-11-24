namespace Talabat.API.DTOs
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethod { get; set; }
        public AddressDto ShippingAddress { get; set; }
    }
}
