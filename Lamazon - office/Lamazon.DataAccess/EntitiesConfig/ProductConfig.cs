using Lamazon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamazon.DataAccess.EntitiesConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).IsRequired().HasPrecision(10, 2);
            builder.Property(x => x.ImageUrl).IsRequired();
            builder.Property(x => x.IsFeatured).HasDefaultValue(false);
            builder.Property(x => x.DiscountPercentage).HasDefaultValue(0);
            //foreign key to Category
            builder.HasOne(x => x.ProductStatus)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductStatusId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise category, da ne se izbriseat i product-ite
                .HasConstraintName("FK_Product_Category");
            //foreign key to ProductCategory
            builder.HasOne(x => x.ProductCategory)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductCategoryId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise category, da ne se izbriseat i product-ite
                .HasConstraintName("FK_Product_ProductCategory");
        }

    }
}
