using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps.Meituan
{
    class UserMap : EntityTypeConfiguration<UserEntity>
    {
        public UserMap()
        {
            ToTable("T_User").HasKey(a => a.Id);

            Property(a => a.UserName).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.LoginName).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.PasswordHash).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.PasswordSalt).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.ConnString).HasMaxLength(500).IsRequired().IsUnicode(false);
            Property(a => a.Ket).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.Description).HasMaxLength(100).IsUnicode(false);
            Property(a => a.SetBuyPara).HasMaxLength(100).IsUnicode(false);
        }
    }
}
