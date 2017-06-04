using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using DapperExtensions.Sql;
using JD.SDK;

namespace JD.AutoRunService.SynchronousData
{
    class Program
    {
        static void Main(string[] args)
        {
            //同步每个商品池中的SkuId
            try
            {
                if (InsertToProductSku())
                {
                    Console.WriteLine("同步商品池中的SkuId成功");
                }
                else
                {
                    Console.WriteLine("同步商品池中的SkuId失败！");
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            Console.Read();

        }



        private static List<ProductPool> GetProductPoolList()
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var db = new JDProductContext())
            {
                var x = db.ProductPool.ToList();
                sw.Stop();
                Console.WriteLine("执行GetProductPoolList耗时：{0}毫秒。", sw.ElapsedMilliseconds);
                return x;
            }
        }

        private static bool InsertToProductSku()
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
                        Console.WriteLine("商品池编号:{0},商品池名称:{1}", num.PageNum, num.Name);
                        var skustringArray = service.GetSku(token, num.PageNum.ToString()).GetAwaiter().GetResult();
                        if (string.IsNullOrWhiteSpace(skustringArray))
                        {
                            Console.WriteLine("#######string.IsNullOrWhiteSpace(skustringArray)###########");
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
                                           SkuId = long.TryParse(c, out x) ? x : 0
                                       }).ToList();

                            if (tmp==null || !tmp.Any())
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
                    Console.WriteLine("执行InsertToProductSku-All耗时：{0}毫秒。", sw.ElapsedMilliseconds);

                    var conn = db.Database.Connection;

                    skus.RemoveAll(item=>item==null);
                    InsertBatch<ProductSku>(conn, skus).GetAwaiter().GetResult();

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return true;

            }

        }


        /// <summary>
        /// 批量插入功能
        /// </summary>
        private static async Task InsertBatch(IDbConnection conn, IEnumerable<ProductSku> entityList, IDbTransaction transaction = null)
        {
            var watch = new System.Diagnostics.Stopwatch();
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var tblName = string.Format("dbo.{0}", typeof(ProductSku).Name);

                SqlTransaction tran = null;
                if (transaction == null)
                {
                    tran = (SqlTransaction)conn.BeginTransaction();
                }
                else
                {
                    tran = (SqlTransaction)transaction;
                }
                //using (var bulkCopy = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.TableLock, tran))
                using (var bulkCopy = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.KeepIdentity, tran))
                {
                    string sqlText = "delete  FROM [dbo].[ProductSku]";
                    SqlCommand cmd = new SqlCommand(sqlText, conn as SqlConnection, tran);
                    cmd.CommandTimeout = 300;
                    cmd.ExecuteNonQuery();
                    bulkCopy.BatchSize = entityList.Count();
                    bulkCopy.DestinationTableName = tblName;
                    DapperExtensions.Sql.ISqlGenerator sqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration());
                    var classMap = sqlGenerator.Configuration.GetMap<ProductSku>();
                    var props = classMap.Properties.Where(x => x.Ignored == false).ToArray();
                    foreach (var propertyInfo in props)
                    {
                        bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    }

                    watch.Start();
                    bulkCopy.BulkCopyTimeout = 300;

                    await bulkCopy.WriteToServerAsync(new DataTable());
                    tran.Commit();
                    watch.Stop();
                    Console.WriteLine("执行WriteToServerAsync耗时：{0}毫秒。", watch.ElapsedMilliseconds);

                }
            }
            catch (AggregateException e)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        /// <summary>
        /// 批量插入功能
        /// </summary>
        private static async Task InsertBatch<T>(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class
        {
            var watch = new System.Diagnostics.Stopwatch();
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var tblName = string.Format("dbo.{0}", typeof(T).Name);
                SqlTransaction tran = null;
                if (transaction == null)
                {
                    tran = (SqlTransaction)conn.BeginTransaction();
                }
                else
                {
                    tran = (SqlTransaction)transaction;
                }
                //using (var bulkCopy = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.TableLock, tran))
                using (var bulkCopy = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.KeepIdentity, tran))
                {
                    string sqlText =string.Format("delete  FROM [dbo].{0}", tblName);
                    SqlCommand cmd = new SqlCommand(sqlText, conn as SqlConnection, tran);
                    cmd.CommandTimeout = 300;
                    cmd.ExecuteNonQuery();
                    bulkCopy.BatchSize = entityList.Count();
                    bulkCopy.DestinationTableName = tblName;
                    var table = new DataTable();
                    DapperExtensions.Sql.ISqlGenerator sqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration());
                    var classMap = sqlGenerator.Configuration.GetMap<T>();
                    var props = classMap.Properties.Where(x => x.Ignored == false).ToArray();
                    foreach (var propertyInfo in props)
                    {
                        bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                        table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyInfo.PropertyType) ?? propertyInfo.PropertyInfo.PropertyType);
                    }
                    var values = new object[props.Count()];
                    watch.Start();

                    foreach (var itemm in entityList)
                    {
                        if(itemm == null)
                        {
                            continue;
                        }
                        for (var i = 0; i < values.Length; i++)
                        {
                            try
                            {
                                values[i] = props[i].PropertyInfo.GetValue(itemm, null);
                            }
                            catch
                            {
                                Debug.Assert(true);
                            }
                        }

                        try
                        {
                            table.Rows.Add(values);
                        }
                        catch
                        {
                            Debug.Assert(true);
                        }


                    }
                    
                    bulkCopy.BulkCopyTimeout = 300;
                    await bulkCopy.WriteToServerAsync(table);
                    tran.Commit();
                    watch.Stop();
                    Console.WriteLine("执行WriteToServerAsync耗时：{0}毫秒。", watch.ElapsedMilliseconds);

                }
            }
            catch (AggregateException e)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


    }
}
