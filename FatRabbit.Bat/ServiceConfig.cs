using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.PlatformAbstractions;
namespace FatRabbit.Bat
{

    public class ServiceConfig
    {

        public string Ip { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 服务id
        /// </summary>
        public string ServiceId { get; set; }
        /// <summary>
        /// 故障多长时间后注销
        /// </summary>
        public int DeregisterCriticalServiceAfterSecond { set; get; }= 10;
        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string HealthUrlWithoutRootUrl { set; get; } = "Api/Health";
        /// <summary>
        /// 健康检查间隔时间
        /// </summary>
        public int IntervalSecond { set; get; } = 10;
        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeoutSecond { set; get; } = 10;

        public ServiceConfig()
        {
            Microsoft.Extensions.PlatformAbstractions.ApplicationEnvironment env = PlatformServices.Default.Application;
            ServiceName = env.ApplicationName;
            ServiceId = ServiceName + Guid.NewGuid();

        }

        public ServiceConfig(string ip, string port) : this()
        {

            Ip = ip; //configuration["ip"];
            Port = Convert.ToInt32(port); //Convert.ToInt32(configuration["port"]);


        }


    }

}
