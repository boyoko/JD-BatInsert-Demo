using JD.SDK;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JD.AutoRunService.SynchronousData
{
    public sealed class RefreshTokenJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RefreshTokenJob));

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("######刷新Token开始######");
                SynchronousDataService service = new SynchronousDataService();

                service.RefreshToken().GetAwaiter().GetResult();

                _logger.Info("######刷新Token结束######");
            }
            catch(Exception ex)
            {
                _logger.Error("刷新Token出现错误，错误信息："+ex.Message);
            }
        }
    }
}
