using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabt.Core.Entities;

namespace Talabt.Core.Order_Aggregate
{
    public class Order:BaseEntity
    {
        public Order(string buyerEmail, Address shippingAddress, int deliveryMethodId, ICollection<OrderItem> items, decimal subTotal,string paymentIntendId)
        { 
          BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethodId = deliveryMethodId;
            Items = items;
            SubTotal = subTotal;
            PaymentIntendId= paymentIntendId;
        }
        public Order() { }
        public string  BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.pending;
        public Address ShippingAddress { get; set; }
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntendId { get; set; }

    }
}
