using AdminDashBoard.Models;
using AutoMapper;
using Talabt.Core.Entities;


namespace AdminDashBoard.Helper
{
    public class MapsProfile:Profile
    {
        public MapsProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
