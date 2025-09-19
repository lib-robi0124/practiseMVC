using Lamazon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamazon.DataAccess.EntitiesConfig
{
    public class InvoiceStatusConfig : IEntityTypeConfiguration<InvoiceStatus>
    {
        public void Configure(EntityTypeBuilder<InvoiceStatus> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            //builder.HasMany(x => x.Invoices)
            //    .WithOne(x => x.InvoiceStatus)
            //    .HasForeignKey(x => x.InvoiceStatusId)
            //    .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise status, da ne se izbriseat i invoice-ite
            //    .HasConstraintName("FK_Invoice_InvoiceStatus");
        }
    }
}
