using Lamazon.ViewModels.Models;

namespace Lamazon.Services.Interfaces
{
    public interface IProductCategoriesService
    {
        void CreateProductCategory(ProductCategoryViewModel productCategoryViewModel);
        void UpdateProductCategory(ProductCategoryViewModel productCategoryViewModel);
        void DeleteProductCategory(int id);
        ProductCategoryViewModel GetProductCategoryById(int id);
        PageResultViewModel<ProductCategoryViewModel> GetPageResultViewModel(DatatableRequesViewModel datatableRequesViewModel);
    }
}
