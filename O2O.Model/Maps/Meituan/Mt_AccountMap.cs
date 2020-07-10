using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps.Eleme
{
    public class Mt_AccountMap : EntityTypeConfiguration<Mt_AccountEntity>
    {
        public Mt_AccountMap()
        {
            ToTable("Mt_Account").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.AccountNo).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.AccountName).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.Description).HasMaxLength(200).IsUnicode(false);

            Property(a => a.WaimaiAppId).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.WaimaiAppSecret).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.TuangouAppKey).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.TuangouAppSecret).HasMaxLength(50).IsRequired().IsUnicode(false);

            //
            HasRequired(a => a.User).WithMany(a => a.Mt_Accounts).HasForeignKey(a => a.UserId);
        }
    }
}
