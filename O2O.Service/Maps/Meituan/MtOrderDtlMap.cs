using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Service
{
    class MtOrderDtlMap : EntityTypeConfiguration<MtOrderDtlEntity>
    {
        public MtOrderDtlMap()
        {
            ToTable("Mt_OrderDtl").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.SkuId).HasMaxLength(30).IsRequired().IsUnicode(false);
            Property(a => a.FoodName).HasMaxLength(30).IsRequired().IsUnicode(false);
            Property(a => a.Quantity).IsRequired();
            Property(a => a.BoxNum).IsRequired();
            Property(a => a.BoxPrice).IsRequired();
            Property(a => a.Unit).HasMaxLength(10).IsUnicode(false);
            //Property(a => a.FoodDiscount);
            Property(a => a.Spec).HasMaxLength(30).IsUnicode(false);
            Property(a => a.FoodPreperty).HasMaxLength(30).IsUnicode(false);
        }
    }
}
