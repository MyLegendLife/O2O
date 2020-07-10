using log4net;
using System.Data.Entity;
using System.Reflection;

namespace O2O.Model
{
    public class O2OContext : DbContext
    {
        private static ILog _log = LogManager.GetLogger(typeof(O2OContext));

        public O2OContext() : base("name=connStr")
        {
            //关闭自动创建表的功能
            //Database.SetInitializer<O2OContext>(null);

            //记录sql
            //Database.Log = sql =>
            //{
            //    _log.DebugFormat("EF执行SQL:{0}", sql);
            //};
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //加载实体类的配置信息，即继承EntityTypeConfiguration<T>的Map类
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<OrderEntity> Order { get; set; }
        public DbSet<OrderDtlEntity> OrderDtl { get; set; }
        
        public DbSet<Mt_AccountEntity> Mt_Account { get; set; }

        public DbSet<Ele_AccountEntity> Ele_Account { get; set; }
        public DbSet<Ele_ShopEntity> Ele_Shop { get; set; }
    }
}
