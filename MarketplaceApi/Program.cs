using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MarketplaceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Data data = new Data();
            
            var host = CreateHostBuilder(args).Build();
            
            CreateHostBuilder(args).Build().Run();

            using (var db = new MarketplaceContext())
            {
                int[] userId = data.Id;
                
                for (int i = 0; i < userId.Length; i++)
                {
                    //db.User.Add(new User(userId));
                }
                db.SaveChanges();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}