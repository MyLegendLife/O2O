using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Service
{
    class MtShopMap : EntityTypeConfiguration<MtShopEntity>
    {
        public MtShopMap()
        {
            ToTable("Mt_Shop").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.PoiName).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.AppAuthToken).HasMaxLength(100).IsRequired().IsUnicode(false);
            Property(a => a.BusinessType).IsRequired();
            Property(a => a.PoiId).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.ShopNo).HasMaxLength(10).IsRequired().IsUnicode(false);

            HasRequired(a => a.User).WithMany(a => a.MtShops).HasForeignKey(a => a.UserId);
        }
    }
}
