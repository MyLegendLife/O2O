using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class EleShopMap : EntityTypeConfiguration<EleShopEntity>
    {
        public EleShopMap()
        {
            ToTable("Ele_Shop").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.ShopId).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.ShopName).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.ShopNo).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.Sign).HasMaxLength(10).IsRequired().IsUnicode(false);
        }
    }
}
