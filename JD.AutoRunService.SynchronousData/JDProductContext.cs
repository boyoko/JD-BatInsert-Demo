using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.AutoRunService.SynchronousData
{
    public class JDProductContext : DbContext
    {

        static JDProductContext()
        {
            Database.SetInitializer<JDProductContext>(null);
        }

        public JDProductContext() : base("JDProductContext")
        {
        }
        public virtual DbSet<ProductPool> ProductPool { get; set; }
        public virtual DbSet<ProductSku> ProductSku { get; set; }
        public virtual DbSet<ProductDetail> ProductDetail { get; set; }
        public virtual DbSet<ProductSkuBase> ProductSkuBase { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductPool>().ToTable("ProductPool");
            modelBuilder.Entity<ProductSku>().ToTable("ProductSku");
            modelBuilder.Entity<ProductDetail>().ToTable("ProductDetail");
            modelBuilder.Entity<ProductSkuBase>().ToTable("ProductSkuBase");
            base.OnModelCreating(modelBuilder);
        }
    }

    public class ProductPool
    {
        [Key]
        public string ProductPoolId { get; set; }
        public string Name { get; set; }
        public int PageNum { get; set; }
    }

    public class ProductSku
    {
        [Key]
        public string ProductSkuId { get; set; }
        public string ProductPoolId { get; set; }
        public int PageNum { get; set; }
        public long SkuId { get; set; }
    }

    public class ProductDetail
    {
        [Key]
        public string ProductId { get; set; }
        /// <summary>
        /// 销售单位
        /// </summary>
        public string SaleUnit { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// 产地
        /// </summary>
        public string ProductArea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WareQD { get; set; }
        /// <summary>
        /// 主图地址
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// 规格参数
        /// </summary>
        public string Param { get; set; }
        /// <summary>
        /// 上下架状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int Sku { get; set; }
        /// <summary>
        /// 售后
        /// </summary>
        public string Shouhou { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public string Upc { get; set; }
        /// <summary>
        /// 手机端详细介绍
        /// </summary>
        public string Appintroduce { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 详细介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 京东自营礼品卡， 只有当sku为京东自营实物礼品卡的时候才有该字段
        /// </summary>
        public int EleGift { get; set; }
    }


    public class ProductSkuBase
    {
        [Key]
        public string ProductSkuId { get; set; }
        public string ProductPoolId { get; set; }
        public int PageNum { get; set; }
        public string SkuIds { get; set; }
    }
}
