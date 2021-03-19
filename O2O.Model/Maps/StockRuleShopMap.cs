using O2O.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps
{
    class StockRuleShopMap : EntityTypeConfiguration<StockRuleShopEntity>
    {
        public StockRuleShopMap()
        {
            ToTable("T_StockRuleShop").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.ShopNo).HasMaxLength(50).IsRequired().IsUnicode(false);

            HasRequired(a => a.StockRule).WithMany(b => b.StockRuleShops).HasForeignKey(a => a.StockRuleId);
        }
    }
}
