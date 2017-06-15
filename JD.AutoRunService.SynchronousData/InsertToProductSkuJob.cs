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
    public sealed class InsertToProductSkuJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(InsertToProductSkuJob));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("######同步ProductSku开始######");
                SynchronousDataService service = new SynchronousDataService();
                var token = service.AccessToken().GetAwaiter().GetResult();
                //同步商品池
                var x = service.GetPageNum(token).GetAwaiter().GetResult();
                if (InsertToProductPool(x))
                {
                    Console.WriteLine("同步商品池数据成功");
                }
                else
                {
                    Console.WriteLine("同步商品池数据失败！");
                }
                //同步每个商品池中的SkuId
                InsertToProductSku();

                _logger.Info("######同步ProductSku结束######");
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("同步ProductSku出现错误，错误信息：{0}", ex.Message);
            }
        }

        private List<ProductPool> GetProductPoolList()
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var db = new JDProductContext())
            {
                var x = db.ProductPool.ToList();
                sw.Stop();
                _logger.InfoFormat("执行GetProductPoolList耗时：{0}毫秒。", sw.ElapsedMilliseconds);
                return x;
            }
        }

        private bool InsertToProductSku()
        {
            var sw = new Stopwatch();
            sw.Start();

            SynchronousDataService service = new SynchronousDataService();
            var token = service.AccessToken().GetAwaiter().GetResult();
            using (var db = new JDProductContext())
            {
                try
                {
                    List<ProductPool> list = GetProductPoolList();
                    List<ProductSku> skus = new List<ProductSku>();
                    Parallel.ForEach(list, (num) =>
                    {
                        _logger.InfoFormat("商品池编号:{0},商品池名称:{1}", num.PageNum, num.Name);
                        var skustringArray = service.GetSku(token, num.PageNum.ToString()).GetAwaiter().GetResult();
                        if (string.IsNullOrWhiteSpace(skustringArray))
                        {
                            _logger.InfoFormat("#######string.IsNullOrWhiteSpace(skustringArray)###########");
                            return;
                        }
                        if (skustringArray.Contains(','))
                        {
                            var skuList = skustringArray.Split(',');
                            //Console.WriteLine("商品池编号:{0}--商品池名称:{1}--有{2}个Sku", num.PageNum, num.Name, skuList.Count());
                            long x = 0;

                            if (skuList == null || !skuList.Any())
                            {
                                Console.WriteLine("*******************************************");
                                Debug.Assert(true);
                            }

                            var tmp = (from c in skuList
                                       select new ProductSku
                                       {
                                           ProductSkuId = Guid.NewGuid().ToString(),
                                           ProductPoolId = num.ProductPoolId,
                                           PageNum = num.PageNum,
                                           SkuId = long.TryParse(c, out x) ? x : 0,
                                           CreateTime = DateTime.Now
                                       }).ToList();

                            if (tmp == null || !tmp.Any())
                            {
                                Console.WriteLine("----------------------------------------------------");
                                Debug.Assert(true);
                            }

                            //var jsonStr = JsonConvert.SerializeObject(tmp);
                            //Console.WriteLine("商品池编号:{0}--商品池名称:{1}--有{2}个Sku,序列化结果：{3}", num.PageNum, num.Name, skuList.Count(),jsonStr);
                            //log.Info(jsonStr);
                            skus.AddRange(tmp);

                        }
                        else
                        {
                            long x = 0;
                            var flag = long.TryParse(skustringArray, out x);
                            var tmp = new ProductSku
                            {
                                ProductSkuId = Guid.NewGuid().ToString(),
                                ProductPoolId = num.ProductPoolId,
                                PageNum = num.PageNum,
                                SkuId = x,
                                CreateTime = DateTime.Now
                            };

                            if (tmp == null)
                            {
                                Console.WriteLine("=====================================================");
                                Debug.Assert(true);
                            }
                            skus.Add(tmp);

                            //var jsonStr = JsonConvert.SerializeObject(tmp);
                            //Console.WriteLine("商品池编号:{0}--商品池名称:{1},序列化结果：{2}", num.PageNum, num.Name, jsonStr);
                            //log.Info(jsonStr);
                        }

                    });

                    sw.Stop();
                    _logger.InfoFormat("执行InsertToProductSku-All耗时：{0}毫秒。", sw.ElapsedMilliseconds);

                    var conn = db.Database.Connection;

                    skus.RemoveAll(item => item == null);
                    BulkInsertHelper.InsertBatch<ProductSku>(conn, skus).GetAwaiter().GetResult();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return true;

            }

        }


        private bool InsertToProductPool(List<PageNumResult> list)
        {
            using (var db = new JDProductContext())
            {
                try
                {
                    var tmp = (from c in list
                               select new ProductPool
                               {
                                   ProductPoolId = Guid.NewGuid().ToString(),
                                   Name = c.name,
                                   PageNum = Convert.ToInt32(c.page_num),
                                   CreateTime = DateTime.Now
                               }).ToList();

                    var conn = db.Database.Connection;
                    tmp.RemoveAll(item => item == null);

                    BulkInsertHelper.InsertBatch<ProductPool>(conn, tmp).GetAwaiter().GetResult();
                    return true;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                
            }
        }

    }
}
