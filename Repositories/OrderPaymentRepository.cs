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
    
    public interface IOrderPaymentRepository : IBaseRepository<OrderPaymentModel>
    {

    }
    public class OrderPaymentRepository : BaseRepository<OrderPaymentModel>, IOrderPaymentRepository
    {
        public OrderPaymentRepository(WebApiContext context) : base(context)
        {

        }
    }
}
