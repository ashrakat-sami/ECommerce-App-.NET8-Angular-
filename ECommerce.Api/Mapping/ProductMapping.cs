using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Entities.Product;

namespace ECommerce.Api.Mapping
{
    public class ProductMapping:Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                 .ReverseMap();

            CreateMap<Photo, PhotoDto>().ReverseMap();
        }
    }
}
