using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Service
{
    class MtOrderMap : EntityTypeConfiguration<MtOrderEntity>
    {
        public MtOrderMap()
        {
            ToTable("Mt_Order").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.OrderId).HasMaxLength(30).IsRequired().IsUnicode(false);
            Property(a => a.OrderIdView).HasMaxLength(30).IsRequired().IsUnicode(false);
            Property(a => a.Caution).HasMaxLength(200).IsUnicode(false);
            Property(a => a.CTime).IsRequired();
            Property(a => a.UTime).IsRequired();
            Property(a => a.DaySeq).IsRequired();
            //Property(a => a.DeliveryTime).IsRequired();
            Property(a => a.OriginalPrice).IsRequired();
            Property(a => a.RecipientAddress).HasMaxLength(200).IsUnicode(false);
            Property(a => a.RecipientName).HasMaxLength(20).IsUnicode(false);
            Property(a => a.RecipientPhone).HasMaxLength(20).IsUnicode(false);
            Property(a => a.ShippingPhone).HasMaxLength(20).IsUnicode(false);
            //Property(a => a.ShippingFee).IsRequired();
            //Property(a => a.Status).IsRequired();
            //Property(a => a.Total).IsRequired();
            //Property(a => a.IsSaled).IsRequired();
        }
    }
}
