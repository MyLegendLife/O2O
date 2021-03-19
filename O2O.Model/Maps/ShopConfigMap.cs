using O2O.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps
{
    public class ShopConfigMap : EntityTypeConfiguration<ShopConfigEntity>
    {
        public ShopConfigMap()
        {
            ToTable("T_ShopConfig").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.UserId).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.ShopNo).HasMaxLength(20).IsRequired().IsUnicode(false);
        }
    }
}
