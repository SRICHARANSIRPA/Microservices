using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration _configuration { get; set; }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionString"));
            var inserted = await connection.ExecuteAsync(
                "INSERT INTO public.coupon(productname, description, amount) VALUES(@productname, @description, @amount)",
                new { productName = coupon.productName, Description = coupon.Description, Amount = coupon.Amount });
            if (inserted == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionString"));
            var deleted = await connection.ExecuteAsync(
                "DELETE FROM  Coupon WHERE productName=@productName",
                new { productName = productName });
            if (deleted == 0)
                return false;
            return true;
        }

        public async Task<Coupon> GetDiscount(string ProductName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM COUPON WHERE ProductName=@ProductName", new { ProductName = ProductName });
            if (coupon == null)
                return new Coupon { productName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            return coupon;

        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                    ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                            new { ProductName = coupon.productName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }

    }
}
