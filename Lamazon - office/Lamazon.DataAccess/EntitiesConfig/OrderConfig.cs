using Lamazon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamazon.DataAccess.EntitiesConfig
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(50);
            builder.Property(x => x.CountryCode).HasMaxLength(5);
            builder.Property(x => x.IpAddress).HasMaxLength(50);
            builder.Property(x => x.TotalAmount).IsRequired().HasPrecision(10, 2);
            //foreign key to OrderStatus
            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise user, da ne se izbriseat i order-ite
                .HasConstraintName("FK_Order_User");
            //foreign key to OrderStatus
            builder.HasOne(x => x.OrderStatus)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.OrderStatusId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise status, da ne se izbriseat i order-ite
                .HasConstraintName("FK_Order_OrderStatus");
        }
    }
}
