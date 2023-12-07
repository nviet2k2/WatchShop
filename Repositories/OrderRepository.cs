using Database.Models;
using Database;
using Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IOrderRepository : IBaseRepository<OrderModel>
    {
        Task<int> GetTotalOrders();
        Task<double> GetTotalRevenue();
        Task<List<ProductModel>> GetTopSellingProducts(int topCount);
    }
    public class OrderRepository : BaseRepository<OrderModel>, IOrderRepository
    {
        private readonly WebApiContext _context;
        public OrderRepository(WebApiContext context) : base(context)
        {
            _context = context;

        }
        public async Task<int> GetTotalOrders()
        {


            int totalOrders = _context.Set<OrderModel>().Count();
            return totalOrders;

        }
        public async Task<double> GetTotalRevenue()
        {
            double totalRevenue = await _context.Set<OrderModel>().SumAsync(o => o.TotalPrice);
            return totalRevenue;
        }
        public async Task<List<ProductModel>> GetTopSellingProducts(int topCount)
        {
            var topProducts = await _context.Set<OrderDetailModel>()
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .Take(topCount)
                .ToListAsync();

            var productIds = topProducts.Select(p => p.ProductId).ToList();

            var topSellingProducts = await _context.Set<ProductModel>()
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            return topSellingProducts;
        }


    }
}
