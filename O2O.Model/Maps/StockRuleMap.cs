using O2O.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps
{
    public class StockRuleMap : EntityTypeConfiguration<StockRuleEntity>
    {
        public StockRuleMap()
        {
            ToTable("T_StockRule").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.UserId).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.RuleName).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.Description).HasMaxLength(200).IsUnicode(false);
        }
    }
}
