using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace FatRabbit.Bat
{
    public class BaseResponse
    {
        /// <summary>
        /// 响应状态码
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 响应报文头
        /// </summary>
        public HttpResponseHeaders Headers { get; set; }


    }
}
