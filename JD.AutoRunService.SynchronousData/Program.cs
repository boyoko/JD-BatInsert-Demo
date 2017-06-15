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
using JD.Common;
using Topshelf;
using System.IO;
using Topshelf.Quartz;
using Quartz;

namespace JD.AutoRunService.SynchronousData
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
                
                HostFactory.Run(c =>
                {
                    c.UseLog4Net();

                    c.Service<ServiceRunner>(s =>
                    {
                        s.ConstructUsing(name => new ServiceRunner());
                        s.WhenStarted((service, control) => service.Start(null));
                        s.WhenStopped((service, control) => service.Stop(null));

                        //同步商品池和商品sku
                        s.ScheduleQuartzJob<ServiceRunner>(q =>
                            q.WithJob(() =>
                                JobBuilder.Create<InsertToProductSkuDetailJob>().Build())
                            .AddTrigger(() =>
                                TriggerBuilder.Create()
                                    .WithSimpleSchedule(builder => builder
                                        .WithIntervalInHours(5)
                                        .RepeatForever())
                                    .Build())
                            );

                    });

                });


                //var a = service.Get(token).GetAwaiter().GetResult();

                //var b = service.Del(token,1525817600).GetAwaiter().GetResult();

                //var request = new SubmitOrderRequestDto
                //{
                //    token = token,
                //    thirdOrder = "ABC0001",
                //    sku = new List<SkuObject>
                //    {
                //        new SkuObject
                //        {
                //            skuId = 3153365,
                //            num = 1,
                //            bNeedAnnex = true,
                //            bNeedGift = false,
                //            //yanbao = new List<long> { 1618482 }
                //        },
                //    },

                //    name = "hahah",
                //    province = 1,  //北京
                //    city = 2810, // 大兴区
                //    county = 6501, //五环至六环之间
                //    town = 0,
                //    address = "中航技广场B座3层",
                //    mobile = "13811931624",
                //    email = "519364325@qq.com",
                //    invoiceState = 2,
                //    invoiceType = 1,
                //    selectedInvoiceTitle = 5,
                //    companyName = "门财科技",
                //    invoiceContent = 22,
                //    paymentType = 4,
                //    isUseBalance = 1,
                //    submitState = 1,
                //    doOrderPriceMode = 1,
                //    orderPriceSnap = new List<OrderPriceSnap>
                //    {
                //        new OrderPriceSnap
                //        {
                //            skuId = 3153365,
                //            price = 7699.00M
                //        }
                //    },



                //};

                //var jsonRequest = StringHelper.ToJson(request);

                //var result = service.SubmitOrder(request).GetAwaiter().GetResult();

            }
            catch (Exception e)
            {
                throw e;
            }


            Console.Read();

        }



        

        


    }
}
