using Lamazon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamazon.DataAccess.EntitiesConfig
{
    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.HasOne(x => x.ProductCategoryStatus)
                .WithMany(x => x.ProductCategories)
                .HasForeignKey(x => x.ProductCategoryStatusId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise parent category, da ne se izbriseat i subcategory-ite
                .HasConstraintName("FK_ProductCategory_ProductCategoryStatus");
        }
    }
}
