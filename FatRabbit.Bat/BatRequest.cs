using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FatRabbit.Bat
{
    /// <summary>
    /// 胖兔蝙蝠系列http请求封装，直接请求伪服务名称路径，并响应成实体类型返回给调用者，注意只用于服务之间的通讯，不涉及浏览器向服务请求
    /// </summary>
    public class BatRequest
    {
        private HttpClient _httpClient;
        private IServiceDiscover _serviceDiscover;

        /// <summary>
        /// 构造函数，可以的通过di把实例化后的IServiceDiscover传给私有变量_serviceDiscover，这里我们不必关心它是consul还是别的服务发现
        /// </summary>
        /// <param name="serviceDiscover">IServiceDiscover类型的实例</param>
        public BatRequest(IServiceDiscover serviceDiscover)
        {
            _httpClient = new HttpClient();
            _serviceDiscover = serviceDiscover;
        }

      
        public async Task<ResponseEntity<T>> PostResponseEntityAsync<T>(String url,object body=null, HttpRequestHeaders requestHeaders = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            using (HttpRequestMessage hrm = await GetHttpRequestMessageAsync(url,HttpMethod.Post,body,requestHeaders))
            {
                ResponseEntity<T> resp = await SendRequestAsync<T>(hrm);
                return resp;
            }
        }



        /// <summary>
        /// 异步GET请求获取数据
        /// </summary>
        /// <typeparam name="T">响应正文需要反序列化成的类型</typeparam>
        /// <param name="url">请求的伪URL</param>
        /// <param name="requestHeaders">请求报文头</param>
        /// <returns>异步返回ResponseEntity<T>类型</returns>
        public async Task<ResponseEntity<T>> GetResponseEntityAsync<T>(String url, HttpRequestHeaders requestHeaders = null)
        {
            if(string.IsNullOrEmpty(url))
            {
                return null;
            }
            using (HttpRequestMessage hrm = await GetHttpRequestMessageAsync(url, HttpMethod.Get, null, requestHeaders))
            {
                ResponseEntity<T> resp = await SendRequestAsync<T>(hrm);
                return resp;
            }
           

        }


        /// <summary>
        /// 基方法：拼接请求报问题
        /// </summary>
        /// <param name="url">http请求的伪地址</param>
        /// <param name="method">请求方法</param>
        /// <param name="body">请求正文</param>
        /// <param name="requestHeaders">请求报文头</param>
        /// <returns>异步返回HttpRequestMessage</returns>
        private async Task<HttpRequestMessage> GetHttpRequestMessageAsync(String url, HttpMethod method, object body, HttpRequestHeaders requestHeaders = null)
        {
            HttpRequestMessage hrm = new HttpRequestMessage();
            if (requestHeaders != null)
            {
                foreach (var item in requestHeaders)
                {
                    hrm.Headers.Add(item.Key, item.Value);
                }
            }
           
            hrm.Method = method;//请求方法
                                //判断如果是post请求是有正文的
            if (method == HttpMethod.Post)
            {
                hrm.Content = new StringContent(JsonConvert.SerializeObject(body));
                hrm.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            //解析url
            var realUrl = await _serviceDiscover.ParsingToApiUrl(url);
            hrm.RequestUri = new Uri(realUrl);
            return hrm;
        }



        /// <summary>
        /// http基方法：请求数据
        /// </summary>
        /// <typeparam name="T">希望响应正文被解析成合众数据类型</typeparam>
        /// <param name="requestMsg">请求的数据报文体</param>
        /// <returns>异步返回ResponseEntity<T>类型</returns>
        private async Task<ResponseEntity<T>> SendRequestAsync<T>(HttpRequestMessage requestMsg)
        {

            ResponseEntity<T> resp = new ResponseEntity<T>();
            HttpResponseMessage result = await _httpClient.SendAsync(requestMsg);//获取响应报文
            resp.StatusCode = result.StatusCode;
            resp.Headers = result.Headers;
            if(resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string body = await result.Content.ReadAsStringAsync();
                //反序列化
                T t = JsonConvert.DeserializeObject<T>(body);
                resp.Body = t;
            }
            return resp;
        }

    }
}
