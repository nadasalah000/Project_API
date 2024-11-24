using AutoMapper;
using Talabat.API.DTOs;
using Talabt.Core.Order_Aggregate;

namespace Talabat.API.Helpers
{
    public class OrderItemPictureUrlResolve : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolve(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            return string.Empty;
        }
    }
}
