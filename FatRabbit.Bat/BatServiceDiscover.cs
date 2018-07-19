using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatRabbit.Bat
{
    public class BatServiceDiscover : IServiceDiscover
    {

        public ServiceConfig ServiceConfig { get; set; } //服务配置
        public ClientConfig ClientConfig { get; set; } //客户端连接配置

     
        //public BatServiceDiscover(ClientConfig clientConfig, ServiceConfig serviceConfig=null)
        //{
        //    ClientConfig = clientConfig;
        //    ServiceConfig = serviceConfig;
        //}


        /// <summary>
        /// 把形如《http://msgService:8080/api/role》伪路径转换成真实路径:《http://192.168.1.1:8080/api/role》
        /// </summary>
        /// <param name="urlWithHttp">伪路径地址：形如：《http://msgService:8080/api/role》</param>
        /// <returns>返回真实路径:《http://192.168.1.1:8080/api/role》</returns>
        public async Task<string> ParsingToApiUrl(string urlWithHttp)
        {
            Uri uri = new Uri(urlWithHttp);
            String serviceName = uri.Host;//获取主机地址
            String rootUrl = await GetAddressByServiceNameAsync(serviceName);//获取根路径+端口号                                                   
            return uri.Scheme + "://" + rootUrl + uri.PathAndQuery;//返回真实完整路径
        }


        /// <summary>
        /// 注册服务
        /// </summary>

        public void RegisterService()
        {
            if (ServiceConfig == null || ClientConfig == null)
            {
                throw new NullReferenceException("请先配置服务或者客户端连接");
            }
            try
            {
                using (var consulClient = new ConsulClient(ConsulConfig))
                {
                    var asr = ConsulAgentServiceRegistration();
                    consulClient.Agent.ServiceRegister(asr).Wait();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="action">回调方法Action</param>
        public void DeRegisterService(Action action)
        {
            try
            {
                using (var consulClient = new ConsulClient(ConsulConfig))
                {
                    action();
                    consulClient.Agent.ServiceDeregister(ServiceConfig.ServiceName).Wait();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 配置服务的各项参数
        /// </summary>
        /// <returns></returns>
        private AgentServiceRegistration ConsulAgentServiceRegistration()
        {
            var asr = new AgentServiceRegistration();
            asr.Address = ServiceConfig.Ip;
            asr.Port = ServiceConfig.Port;
            asr.ID = ServiceConfig.ServiceId;
            asr.Name = ServiceConfig.ServiceName;
            asr.Check = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(ServiceConfig.DeregisterCriticalServiceAfterSecond),
                HTTP = $"http://{ServiceConfig.Ip}:{ServiceConfig.Port}/{ServiceConfig.HealthUrlWithoutRootUrl}",
                Interval = TimeSpan.FromSeconds(ServiceConfig.IntervalSecond),
                Timeout = TimeSpan.FromSeconds(ServiceConfig.TimeoutSecond)
            };

            return asr;
        }

        /// <summary>
        /// 根据服务名称获取服务的《地址:端口》
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回形如：《地址:端口》</returns>
        private async Task<string> GetAddressByServiceNameAsync(string serviceName)
        {
            if (ClientConfig == null)
            {
                throw new ArgumentException("请配置客户端连接");
            }

            using (var consulClient = new ConsulClient(p => p.Address = new Uri(ClientConfig.ClientAddress)))
            {
                var allServices = await consulClient.Agent.Services();
                var services = allServices.Response.Values.Where(p => p.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
                if (!services.Any())
                {
                    throw new ArgumentException($"找不到名称为{serviceName }的任何实例");
                }
                //简易负载均衡策略
                AgentService service = services.ElementAt(Environment.TickCount % services.Count());
                string address = $"{ service.Address}:{service.Port}";
                return address;

            }


        }


        /// <summary>
        /// 配置服务发现客户端的地址和数据中心名称
        /// </summary>
        /// <param name="config"></param>
        private void ConsulConfig(ConsulClientConfiguration config)
        {
            // Console.WriteLine("我看看我什么时候被调用了");
            config.Address = new Uri(ClientConfig.ClientAddress);
            config.Datacenter = ClientConfig.ClientDatacenter;
        }





    }
}
