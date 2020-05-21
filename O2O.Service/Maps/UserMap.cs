using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Service
{
    class UserMap : EntityTypeConfiguration<UserEntity>
    {
        public UserMap()
        {
            ToTable("T_User").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.UserNo).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.UserName).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.ConnString).HasMaxLength(100).IsRequired().IsUnicode(false);
            Property(a => a.Ket).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(a => a.Description).HasMaxLength(100).IsUnicode(false);

            //用户与美团配置关系：单向一对一
            HasRequired(a => a.MtConfig).WithMany().HasForeignKey(a => a.MtConfigId);

            //用户与饿了么配置：一对多
            HasMany(a => a.EleConfigs).WithRequired(a => a.User).HasForeignKey(a => a.UserId);

            //用户与美团门店：一对多
            HasMany(a => a.MtShops).WithRequired(a=> a.User).HasForeignKey(a => a.UserId);

            //用户与饿了么门店：一对多
            HasMany(a => a.EleShops).WithRequired(a => a.User).HasForeignKey(a => a.UserId);
        }
    }
}
