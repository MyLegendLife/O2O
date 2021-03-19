using O2O.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps
{
    public class OrderMap : EntityTypeConfiguration<OrderEntity>
    {
        public OrderMap()
        {
            ToTable("T_Order").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.TakeType).IsRequired();
            Property(a => a.OrderId).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.TtlPrice).IsRequired();
            Property(a => a.Consume).IsRequired();

            Property(a => a.UserName).HasMaxLength(100).IsUnicode(false);
            Property(a => a.UserMobile).HasMaxLength(100).IsUnicode(false);
            Property(a => a.DeliverAddress).HasMaxLength(500).IsUnicode(false);
            Property(a => a.CancelCode).HasMaxLength(50).IsUnicode(false);
            Property(a => a.CancelReason).HasMaxLength(200).IsUnicode(false);
            Property(a => a.RefundCode).HasMaxLength(50).IsUnicode(false);
            Property(a => a.RefundReason).HasMaxLength(200).IsUnicode(false);
            Property(a => a.UserId).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.RefundPartAmt).IsRequired();
            Property(a => a.DispatcherName).HasMaxLength(40).IsUnicode(false);
            Property(a => a.DispatcherMobile).HasMaxLength(40).IsUnicode(false);
            Property(a => a.Greeting).HasMaxLength(500).IsUnicode(false);
        }
    }
}
