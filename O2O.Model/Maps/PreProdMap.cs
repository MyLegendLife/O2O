using O2O.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps.Meituan
{
    class PreProdMap : EntityTypeConfiguration<PreProdEntity>
    {
        public PreProdMap()
        {
            ToTable("T_PreProd").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.UserId).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.ProdNo).HasMaxLength(20).IsRequired().IsUnicode(false);
        }
    }
}
