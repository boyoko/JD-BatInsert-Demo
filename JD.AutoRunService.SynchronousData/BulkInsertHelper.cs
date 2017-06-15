using DapperExtensions;
using DapperExtensions.Sql;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.AutoRunService.SynchronousData
{
    public class BulkInsertHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BulkInsertHelper));

        /// <summary>
        /// 批量插入功能
        /// </summary>
        public static async Task InsertBatch<T>(IDbConnection conn, IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class
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
                using (var bulkCopy = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.KeepIdentity, tran))
                {
                    if (transaction == null)
                    {
                        string sqlText = string.Format("delete  FROM {0}", tblName);
                        SqlCommand cmd = new SqlCommand(sqlText, conn as SqlConnection, tran);
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                    }
                    
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
                        if (itemm == null)
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
                    if (transaction == null && tran != null)
                    {
                        tran.Commit();
                    }
                    watch.Stop();
                    _logger.InfoFormat("执行WriteToServerAsync耗时：{0}毫秒。", watch.ElapsedMilliseconds);

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
