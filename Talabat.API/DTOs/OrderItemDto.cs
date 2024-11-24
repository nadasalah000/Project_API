using Talabt.Core.Order_Aggregate;

namespace Talabat.API.DTOs
{
    public class OrderItemDto
    {
         //public ProductItemOrder Product {  get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
