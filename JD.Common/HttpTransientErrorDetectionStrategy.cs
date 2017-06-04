using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.TransientFaultHandling;

namespace JD.Common
{
    public class HttpTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            if (ex != null)
            {
                HttpRequestExceptionWithStatus httpException;

                if ((httpException = ex as HttpRequestExceptionWithStatus) != null)
                {
                    if (httpException.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }
    }
}
