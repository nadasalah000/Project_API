using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabt.Core.Entities;
using Talabt.Core.Order_Aggregate;
using Talabt.Core.Repositories;
using Talabt.Core.Services;
using Talabt.Core.Specifications.OrderSpec;
using Address = Talabt.Core.Order_Aggregate.Address;

namespace Talabt.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            var OrdersItems = new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrderd = new ProductItemOrder(Product.Id, Product.Name, Product.PictureUrl);
                    var OrdersItem = new OrderItem(ProductItemOrderd, Product.Price,item.Quantity);
                    OrdersItems.Add(OrdersItem);
                }
            }
            var SubTotal = OrdersItems.Sum(item => item.Price * item.Quantity);
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            var Spec = new OrderWithPaymentIntentIdSpec(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
           
            /*if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            }*/

            var Order = new Order(buyerEmail, ShippingAddress, DeliveryMethodId, OrdersItems, SubTotal,Basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().Add(Order);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethosd = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethosd;
        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var Spec = new OrderSpecification(buyerEmail, orderId);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            return Order;
        }

        public Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var Spec = new OrderSpecification(buyerEmail);
            var Orders = _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return Orders;
        }
    }
}
