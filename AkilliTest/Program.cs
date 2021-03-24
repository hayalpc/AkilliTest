using AkilliTest.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.WindowsServices;

namespace AkilliTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            var host = CreateHostBuilder(args).Build();

            if (isWindows && isService)
                host.RunAsService();
            else
                host.Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var appSettingJson = environment == Environments.Development ? $"appsettings.json" : "appsettings.json";

            var config = new ConfigurationBuilder().AddJsonFile(appSettingJson).Build();

            var webSettings = new WebHostSettingHelper();
            config.GetSection("WebHostSettings").Bind(webSettings);

            var appConfig = new AppConfigHelper();
            config.GetSection("AppConfig").Bind(appConfig);

            var dbConfig = new DBConfigHelper();
            config.GetSection("DBConfig").Bind(dbConfig);

            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.DnsRefreshTimeout = 60 * 10 * 1000;

   
            AppConfigHelper.ApiUrl = webSettings.Urls;

            var host = new WebHostBuilder()
            .UseKestrel(
            o =>
            {
                o.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(1);
                o.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
                o.Limits.MaxConcurrentConnections = webSettings.MaxConcurrentConnections;
                o.AllowSynchronousIO = true;
            })
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);

                config.AddJsonFile(appSettingJson);
                config.AddEnvironmentVariables(prefix: "ASPNETCORE_");
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole(c =>
                {
                    c.TimestampFormat = "[HH:mm:ss.fff] ";
                });
            })
            .UseStartup<Startup>()
            .UseIIS()
            .UseIISIntegration()
            .UseUrls(webSettings.Urls);

            return host;
        }

        public static List<string> GetLocalIPv4()
        {
            List<string> output = new List<string>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output.Add(ip.Address.ToString());
                        }
                    }
                }
            }
            return output;
        }
    }
}
