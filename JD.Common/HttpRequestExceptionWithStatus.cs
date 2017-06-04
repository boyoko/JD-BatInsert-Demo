using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JD.Common
{
    public class HttpRequestExceptionWithStatus : HttpRequestException
    {
        public HttpRequestExceptionWithStatus() : base() { }

        public HttpRequestExceptionWithStatus(string message) : base(message) { }

        public HttpRequestExceptionWithStatus(string message, Exception inner) : base(message, inner) { }

        public HttpStatusCode StatusCode { get; set; }

        public int CurrentRetryCount { get; set; }
    }
}
