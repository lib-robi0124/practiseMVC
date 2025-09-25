using AutoMapper;
using Lamazon.Domain.Entities;
using Lamazon.ViewModels.Models;

namespace Lamazon.Services.AutoMapperProfiles
{
    public class ProductCategorMappingProfile : Profile
    {
        public ProductCategorMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>()
                .ForMember(x => x.ProductCategoryStatus, opt => opt.Ignore()) //member ProductCateoryStatus what we don't want to map is ignored
                .ForMember(x => x.ProductCategoryStatus, opt => opt.MapFrom(s => s.ProductCategoryStatusId))
                .ReverseMap()
                .ForMember(x => x.ProductCategoryStatus, opt => opt.Ignore())
                .ForMember(x => x.ProductCategoryStatusId, opt => opt.MapFrom(s => s.ProductCategoryStatus));
        }
    }
}
