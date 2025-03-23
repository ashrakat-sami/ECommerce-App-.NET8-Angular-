using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Entities.Product;

namespace ECommerce.Api.Mapping
{
    public class CategoryMapping: Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
        }
    }
}
