using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class O2OContext : DbContext
    {
        private static ILog log = LogManager.GetLogger(typeof(O2OContext));

        public O2OContext() : base("name=connStr")
        {
            //关闭自动创建表的功能
            //Database.SetInitializer<O2OContext>(null);

            Database.Log = sql =>
            {
                log.DebugFormat("EF执行SQL:{0}",sql);
            };
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //加载实体类的配置信息，即继承EntityTypeConfiguration<T>的Map类
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<UserEntity> User { get; set; }

        public DbSet<MtConfigEntity> MtConfig { get; set; }
        public DbSet<MtShopEntity> MtShop { get; set; }
        public DbSet<MtOrderEntity> MtOrder { get; set; }
        public DbSet<MtOrderDtlEntity> MtOrderDtl { get; set; }

        public DbSet<EleConfigEntity> EleConfig { get; set; }
        public DbSet<EleShopEntity> EleShop { get; set; }
        public DbSet<EleOrderEntity> EleOrder { get; set; }
    }
}
