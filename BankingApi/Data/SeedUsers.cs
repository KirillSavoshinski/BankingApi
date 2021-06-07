using System.Threading.Tasks;
using BankingApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Data
{
    public class SeedUsers
    {
        public static async Task Seed(UserManager<Customer> userManager)
        {
            if (await userManager.Users.AnyAsync()) return; 
            await userManager.CreateAsync(new Customer {UserName = "Arisha Barron"}, "123"); 
        }
    }
}