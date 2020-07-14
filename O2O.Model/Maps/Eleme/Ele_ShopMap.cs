using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps.Eleme
{
    class Ele_ShopMap : EntityTypeConfiguration<Ele_ShopEntity>
    {
        public Ele_ShopMap()
        {
            ToTable("Ele_Shop").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.ShopNo).HasMaxLength(10).IsRequired().IsUnicode(false);

            HasRequired(a => a.Account).WithMany(a => a.Shops).HasForeignKey(a => a.AccountId).WillCascadeOnDelete(false);
        }
    }
}
