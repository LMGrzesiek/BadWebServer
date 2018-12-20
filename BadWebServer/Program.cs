using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using System;
using System.IO;
using System.Threading.Tasks;

namespace November2018.BadWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.ToString())
                .UseStartup<Startup>()
                .UseUrls("http://*:5000", "https://*:5001")
                .Build()
                .Run();
        }
    }
}