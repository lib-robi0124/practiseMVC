using Lamazon.Domain.Entities;

namespace Lamazon.Services.Implementations
{
    internal interface IProductCategoryRepository
    {
        int Insert(ProductCategory productCategory);
    }
}