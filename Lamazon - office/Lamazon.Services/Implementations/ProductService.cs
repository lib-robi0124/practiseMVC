using AutoMapper;
using Lamazon.DataAccess.Interfaces;
using Lamazon.Services.Interfaces;
using Lamazon.ViewModels.Models;

namespace Lamazon.Services.Implementations
{
    public class ProductService : IProductService
    {
        //inject repository and automapper

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public List<ProductViewModel> GetAllFeaturedProducts()
        {
            var featuredProducts = _productRepository.GetAllFeaturedProducts();
            var mappedProducts = _mapper.Map<List<ProductViewModel>>(featuredProducts);
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
