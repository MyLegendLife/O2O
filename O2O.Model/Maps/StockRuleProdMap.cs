using System.ComponentModel.DataAnnotations.Schema;
using O2O.Model.Entities;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps
{
    public class StockRuleProdMap : EntityTypeConfiguration<StockRuleProdEntity>
    {
        public StockRuleProdMap()
        {
            ToTable("T_StockRuleProd").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.ProdNo).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.ProdName).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.EleStock).IsRequired();
            Property(a => a.MtStock).IsRequired();

            HasRequired(a => a.StockRule).WithMany(a => a.StockRuleProds).HasForeignKey(a => a.StockRuleId);
        }
    }
}
