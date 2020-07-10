using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps.Eleme
{
    public class Ele_AccountMap : EntityTypeConfiguration<Ele_AccountEntity>
    {
        public Ele_AccountMap()
        {
            ToTable("Ele_Account").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.AccountNo).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.AccountName).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.AccessToken).HasMaxLength(40).IsRequired().IsUnicode(false);
            Property(a => a.TokenType).HasMaxLength(10).IsUnicode(false);
            Property(a => a.RefreshToken).HasMaxLength(40).IsRequired().IsUnicode(false);
            Property(a => a.Scope).HasMaxLength(10).IsUnicode(false);

            HasRequired(a => a.User).WithMany(a => a.Ele_Accounts).HasForeignKey(a => a.UserId);
        }
    }
}
