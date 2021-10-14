using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async  Task SeedAsync(OrderContext orderContext,ILogger<OrderContextSeed> logger)
        {
            if(!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(getPreConfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database Associated with Context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> getPreConfiguredOrders()
        {
            return new List<Order>
            {
                new Order(){ UserName = "swn", FirstName="SriCharan" , LastName="Sirpa" , EmailAddress="charanchery9989@gmail.com" , AddressLine = "Shivaji nagar" , Country="India",TotalPrice=350 }

            };
        }
    }
}
