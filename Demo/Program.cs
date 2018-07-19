using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
          //  CreateWebHostBuilder(args).Build().Run();


            string[] myArgs = new string[] { "--ip", "192.168.1.4", "--port", "8004" };


            if (args.Length == 0)
            {
                BuildWebHost(myArgs).Run();

            }
            else
            {
                BuildWebHost(args).Run();
            }

        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            String ip = config["ip"];
            String port = config["port"];

            Console.WriteLine($"已经启动服务ip:{ip}port:{port}");
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://{ip}:{port}")
                .Build();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
