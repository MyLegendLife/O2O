using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Model
{
    public class EleOrderMap : EntityTypeConfiguration<EleOrderEntity>
    {
        public EleOrderMap()
        {
            ToTable("Ele_Order").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.AA).HasMaxLength(20).IsRequired().IsUnicode(false);
        }
    }
}
