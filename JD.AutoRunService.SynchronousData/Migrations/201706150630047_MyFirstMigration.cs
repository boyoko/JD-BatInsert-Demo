namespace JD.AutoRunService.SynchronousData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyFirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductDetail",
                c => new
                    {
                        ProductId = c.String(nullable: false, maxLength: 128),
                        SaleUnit = c.String(),
                        Weight = c.String(),
                        ProductArea = c.String(),
                        WareQD = c.String(),
                        ImagePath = c.String(),
                        Param = c.String(),
                        State = c.Int(nullable: false),
                        Sku = c.Int(nullable: false),
                        Shouhou = c.String(),
                        BrandName = c.String(),
                        Upc = c.String(),
                        Appintroduce = c.String(),
                        Category = c.String(),
                        Name = c.String(),
                        Introduction = c.String(),
                        EleGift = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductPool",
                c => new
                    {
                        ProductPoolId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        PageNum = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProductPoolId);
            
            CreateTable(
                "dbo.ProductSku",
                c => new
                    {
                        ProductSkuId = c.String(nullable: false, maxLength: 128),
                        ProductPoolId = c.String(),
                        PageNum = c.Int(nullable: false),
                        SkuId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProductSkuId);
            
            CreateTable(
                "dbo.ProductSkuBase",
                c => new
                    {
                        ProductSkuId = c.String(nullable: false, maxLength: 128),
                        ProductPoolId = c.String(),
                        PageNum = c.Int(nullable: false),
                        SkuIds = c.String(),
                    })
                .PrimaryKey(t => t.ProductSkuId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductSkuBase");
            DropTable("dbo.ProductSku");
            DropTable("dbo.ProductPool");
            DropTable("dbo.ProductDetail");
        }
    }
}
