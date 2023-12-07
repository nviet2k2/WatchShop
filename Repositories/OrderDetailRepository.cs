using Database.Models;
using Database;
using Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IOrderDetailRepository : IBaseRepository<OrderDetailModel>
    {

    }
    public class OrderDetailRepository : BaseRepository<OrderDetailModel>, IOrderDetailRepository
    {
        public OrderDetailRepository(WebApiContext context) : base(context)
        {

        }
    }
}
