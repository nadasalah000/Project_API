using AutoMapper;
using Talabat.API.DTOs;
using Talabat.API.Helpers;
using Talabt.Core.Entities;
using Talabt.Core.Order_Aggregate;

namespace Talabat.API.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Product, ProductToReturnDto>()
                    .ForMember(d => d.ProductType, o => o.MapFrom(S => S.ProductType.Name))
                    .ForMember(d => d.ProductBrand, o => o.MapFrom(S => S.ProductBrand.Name))
                    .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());
            CreateMap<Talabt.Core.Order_Aggregate.Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto,Talabt.Core.Order_Aggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
               .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
               .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));
             CreateMap<OrderItem, OrderItemDto>()
                 .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                 .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                 .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                 .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolve>());
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
        }
    }
}
