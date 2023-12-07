using Database;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.BaseRepository;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IProductRepository : IBaseRepository<ProductModel>
    {
        Task<string> GetProductNameAsync(int? productId);
    }

    public class ProductRepository : BaseRepository<ProductModel>, IProductRepository
    {
        private readonly WebApiContext _context;

        public ProductRepository(WebApiContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string> GetProductNameAsync(int? productId)
        {
            
            var product = await _context.Set<ProductModel>().FindAsync(productId);
            return product?.ProductName;
        }
    }
}
