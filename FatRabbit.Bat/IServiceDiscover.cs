using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FatRabbit.Bat
{
    public interface IServiceDiscover
    {

         ServiceConfig ServiceConfig { get; set; } //服务配置
         ClientConfig ClientConfig { get; set; } //客户端连接配置


        /// <summary>
        /// 注册服务
        /// </summary>
        void RegisterService();
        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="action">回调方法</param>
        void DeRegisterService(Action action);
        /// <summary>
        /// 根据传入的伪url路径解析成实际的URL请求地址https://192.168.1.2:8080/api/role
        /// </summary>
        /// <param name="urlWithHttp">例如https://msgservice:8080/api/role</param>
        /// <returns>返回真实的完整的网路路径</returns>
        Task<string> ParsingToApiUrl(string urlWithHttp);
    }
}
