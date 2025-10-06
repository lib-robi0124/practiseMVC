using AutoMapper;
using Lamazon.DataAccess.Interfaces;
using Lamazon.Services.Interfaces;
using Lamazon.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamazon.Services.Implementations
{
    public class ProductService : IProductService
    {   // Dependency Injection of the repository and mapper
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public List<ProductViewModel> GetAllFeaturedProducts()
        {
           var featureProducts = _productRepository.GetAllFeaturedProducts(); // Get featured products from repository by listing

            var mappedProducts = _mapper.Map<List<ProductViewModel>>(featureProducts); // Map to ViewModel

            return mappedProducts;
        }

        public List<ProductViewModel> GetAllProducts()
        {
            var products = _productRepository.GetAll();

            var mappedProducts = _mapper.Map<List<ProductViewModel>>(products);

            return mappedProducts;
        }

        public ProductViewModel GetProductById(int id)
        {
            var product = _productRepository.GetById(id);   

            return _mapper.Map<ProductViewModel>(product);
        }
    }
}
