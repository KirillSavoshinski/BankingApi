using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Data;
using BankingApi.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BankingApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            
            try
            {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<Customer>>(); 
                await context.Database.MigrateAsync();
                await SeedUsers.Seed(userManager);
            }
            catch(Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}