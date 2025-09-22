using Lamazon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamazon.DataAccess.EntitiesConfig
{
    public class OrderLineItemConfig : IEntityTypeConfiguration<OrderLineItem>
    {
        public void Configure(EntityTypeBuilder<OrderLineItem> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.ProductPrice).IsRequired().HasPrecision(10, 2);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.TotalPrice).IsRequired().HasPrecision(10, 2);
            //foreign key to Order
            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderLineItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise order, da se ne izbriseat i line item-ite
                .HasConstraintName("FK_OrderLineItem_Order");
            //foreign key to Product
            builder.HasOne(x => x.Product)
                .WithMany(x => x.OrderLineItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise product, da ne se izbriseat i line item-ite
                .HasConstraintName("FK_OrderLineItem_Product");
        }
    }
}
