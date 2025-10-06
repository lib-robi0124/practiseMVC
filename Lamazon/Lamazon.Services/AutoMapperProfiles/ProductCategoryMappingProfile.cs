using AutoMapper;
using Lamazon.Domain.Entities;
using Lamazon.Entities.Models;
using Lamazon.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamazon.Services.AutoMapperProfiles
{           //in Services project - dependency for ViewModels project
    public class ProductCategoryMappingProfile : Profile
    {
        public ProductCategoryMappingProfile() //constructor for mapping profile
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>()
                   .ForMember(x => x.ProductCategoryStatus, opt => opt.Ignore()) //ignore this property in one direction
                   .ForMember(x => x.ProductCategoryStatus,
                                   opt => opt.MapFrom(s => s.ProductCategoryStatusId)) //map from int to enum as per our convention
                   .ReverseMap()
                   .ForMember(x => x.ProductCategoryStatus, opt => opt.Ignore())
                   .ForMember(x => x.ProductCategoryStatusId,
                                   opt => opt.MapFrom(s => s.ProductCategoryStatus));

            CreateMap<PageResultModel<ProductCategory>, PagedResultViewModel<ProductCategoryViewModel>>();

        }
    }
}
