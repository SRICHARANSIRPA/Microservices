using Discount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string ProductName);

        public Task<bool> CreateDiscount(Coupon coupon);

        public Task<bool> UpdateDiscount(Coupon coupon);

        public Task<bool> DeleteDiscount(string productName);

    }
}
