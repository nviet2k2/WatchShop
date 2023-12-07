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
    public interface ICustomerRepository : IBaseRepository<CustomerModel>
    {
        Task<int> GetTotalCustomersAsync();
    }
    public class CustomerRepository : BaseRepository<CustomerModel>, ICustomerRepository
    {
        private readonly WebApiContext _context;
        public CustomerRepository(WebApiContext context) : base(context)
        {
            _context = context;
        }
        public async Task<int> GetTotalCustomersAsync()
        {
            return await _context.Set<CustomerModel>().CountAsync();
        }
    }

}

