using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace O2O.Model.Maps.Meituan
{
    class OrderDtlMap : EntityTypeConfiguration<OrderDtlEntity>
    {
        public OrderDtlMap()
        {
            ToTable("T_OrderDtl").HasKey(a => a.Id);
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.ProdNo).HasMaxLength(200).IsRequired().IsUnicode(false);
            Property(a => a.ProdName).HasMaxLength(50).IsRequired().IsUnicode(false);
            Property(a => a.ProdUnit).HasMaxLength(10).IsRequired().IsUnicode(false);

            HasRequired(a => a.Order).WithMany(a => a.OrderDtls).HasForeignKey(a=> a.OrderId);
        }
    }
}
