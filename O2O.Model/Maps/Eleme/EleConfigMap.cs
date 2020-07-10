using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Model
{
    public class EleConfigMap : EntityTypeConfiguration<EleConfigEntity>
    {
        public EleConfigMap()
        {
            ToTable("Ele_Config").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.AccessToken).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.RefreshToken).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.ExpiresDate).IsRequired();
            Property(a => a.Sign).HasMaxLength(20).IsRequired().IsUnicode(false);
        }
    }
}
