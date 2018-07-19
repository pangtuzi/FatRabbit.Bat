using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FatRabbit.Bat;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddConsulServices();//注册蝙蝠
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            //服务注册
            Console.WriteLine("我们将用" + Configuration["ip"] + "" + Configuration["port"] + "连接");

            app.AddDiscoverClient(Configuration["FatRabbit:ClientAddress"], Configuration["FatRabbit:ClientDatacenter"]).
            AddDiscoverService(Configuration["ip"], Configuration["port"]);
            app.RegisterService();//用蝙蝠插件进行服务注册
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                app.DeRegisterService(() => { Console.WriteLine("蝙蝠插件注销服务"); });
            });


        }
    }
}
