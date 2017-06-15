using Dapper;
using JD.SDK;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.AutoRunService.SynchronousData
{
    public sealed class InsertToProductSkuDetailJob : IJob
    {
        private static readonly object obj = new object();
        private readonly ILog _logger = LogManager.GetLogger(typeof(InsertToProductSkuDetailJob));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("######同步商品详情ProductSkuDetail开始######");

                InsertToProductSkuDetail().GetAwaiter().GetResult();

                _logger.Info("######同步商品详情ProductSkuDetail结束######");
            }
            catch (Exception ex)
            {
                _logger.Error("同步ProductSkuDetail出现错误，错误信息：" + ex.Message);
            }
        }
        private  List<ProductSku> GetProductSkuList()
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var db = new JDProductContext())
            {
                var x = db.ProductSku.OrderBy(a => a.PageNum).ToList();
                sw.Stop();
                _logger.InfoFormat("执行GetProductSkuList耗时：{0}毫秒。", sw.ElapsedMilliseconds);
                return x;
            }
        }
        private  async Task<bool> InsertToProductSkuDetail()
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                SynchronousDataService service = new SynchronousDataService();
                var token = await service.AccessToken();
                IDbConnection conn = null;
                IDbTransaction transaction = null;
                List<ProductSku> list = GetProductSkuList();
                var len = Math.Ceiling(list.Count() * 0.0001);
                using (var db = new JDProductContext())
                {
                    for (var i = 0; i < len; i++)
                    {
                        List<ProductDetail> insertList = new List<ProductDetail>();
                        ParallelOptions po = new ParallelOptions();
                        po.MaxDegreeOfParallelism = Environment.ProcessorCount;
                        Parallel.ForEach(list.Skip(i * 10000).Take(10000), po, (sku) => {
                            var x = service.GetDetail(token, (sku.SkuId.ToString()), true).GetAwaiter().GetResult();
                            if (x == null)
                            {
                                _logger.ErrorFormat("######################SKUId={0}返回值为null#######################", sku.SkuId);
                                return;
                            }

                            ProductDetail productDetail = new ProductDetail
                            {
                                ProductId = Guid.NewGuid().ToString(),
                                Appintroduce = x.appintroduce,
                                BrandName = x.brandName,
                                Category = x.category,
                                EleGift = x.eleGift,
                                ImagePath = x.imagePath,
                                Introduction = x.introduction,
                                Name = x.name,
                                Param = x.param,
                                ProductArea = x.productArea,
                                SaleUnit = x.saleUnit,
                                Shouhou = x.shouhou,
                                Sku = x.sku,
                                State = x.state,
                                Upc = x.upc,
                                WareQD = x.wareQD,
                                Weight = x.weight,
                                CreateTime = DateTime.Now
                            };
                            lock (obj)
                            {
                                insertList.Add(productDetail);
                            }
                            //Console.WriteLine("Insert {0} Success!", x.sku);
                        });

                        sw.Stop();
                        _logger.InfoFormat("调用{0}次商品明细接口耗时{1}毫秒", insertList.Count(), sw.ElapsedMilliseconds);
                        insertList.RemoveAll(item => item == null);

                        if (conn == null)
                        {
                            conn = db.Database.Connection;
                        }
                            
                        if(conn.State!= System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        if(transaction==null)
                            transaction = conn.BeginTransaction();

                        if (i == 0 && transaction!=null)
                        {
                            conn.Execute("delete from [dbo].[ProductDetail]",null, transaction);
                        }
                        BulkInsertHelper.InsertBatch<ProductDetail>(conn, insertList, transaction).GetAwaiter().GetResult();
                        if (i == len - 1)
                        {
                            //提交事务
                            transaction.Commit();
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
