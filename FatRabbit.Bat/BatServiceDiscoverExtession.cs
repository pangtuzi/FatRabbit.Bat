using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace FatRabbit.Bat
{
    public static class BatServiceDiscoverExtession
    {
        private static IServiceDiscover _serviceDiscover;
  
        public static void AddConsulServices(this IServiceCollection services)
        {
            services.AddSingleton<IServiceDiscover, BatServiceDiscover>();
            services.AddTransient<BatRequest>();//蝙蝠请求返回实体类型
        }


        /// <summary>
        /// 配置服务发现客户端地址和名称
        /// </summary>
        /// <param name="app"></param>
        /// <param name="clientAddress">服务发现客户端地址</param>
        /// <param name="clientDatacenter">服务发现客户端名称</param>
        public static IApplicationBuilder AddDiscoverClient(this IApplicationBuilder app, string clientAddress,string clientDatacenter)
        {
            ClientConfig cc = new ClientConfig()
            {
                ClientAddress = clientAddress,//
                ClientDatacenter = clientDatacenter
            };
            _serviceDiscover = app.ApplicationServices.GetService<IServiceDiscover>();
            _serviceDiscover.ClientConfig = cc;
            return app;
        }
        //配置服务发现客户端
        public static IApplicationBuilder AddDiscoverClient(this IApplicationBuilder app, Action<ClientConfig> clientConfigOptions)
        {
            ClientConfig cc = new ClientConfig();
            clientConfigOptions(cc);
             _serviceDiscover = app.ApplicationServices.GetService<IServiceDiscover>();
            _serviceDiscover.ClientConfig = cc;
            return app;
        }





        //配置服务端
        public static IApplicationBuilder AddDiscoverService(this IApplicationBuilder app, string ip, string port)
        {
            ServiceConfig sc = new ServiceConfig(ip, port);
            _serviceDiscover = app.ApplicationServices.GetService<IServiceDiscover>();
            _serviceDiscover.ServiceConfig = sc;
            return app;
        }
        //配置服务端
        public static IApplicationBuilder AddDiscoverService(this IApplicationBuilder app,Action<ServiceConfig> discoverServiceConfigOptions)
        {
            var sc = new ServiceConfig();
            discoverServiceConfigOptions(sc);
            _serviceDiscover = app.ApplicationServices.GetService<IServiceDiscover>();
            _serviceDiscover.ServiceConfig = sc;
            return app;
        }




        public static void RegisterService(this IApplicationBuilder app)
        {
            _serviceDiscover.RegisterService();
        }
        public static void DeRegisterService(this IApplicationBuilder app,Action action)
        {
            _serviceDiscover.DeRegisterService(action);
        }

    }
}
