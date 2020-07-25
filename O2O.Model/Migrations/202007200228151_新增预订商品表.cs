namespace O2O.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 新增预订商品表 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_PreProd",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 20, unicode: false),
                        ProdNo = c.String(nullable: false, maxLength: 20, unicode: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.T_PreProd");
        }
    }
}
