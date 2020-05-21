using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Service
{
    class MtConfigMap : EntityTypeConfiguration<MtConfigEntity>
    {
        public MtConfigMap()
        {
            ToTable("Mt_Config").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.WaimaiAppId).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.WaimaiAppSecret).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.TuangouAppKey).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.TuangouAppSecret).HasMaxLength(50).IsRequired().IsUnicode(false);
        }
    }
}
