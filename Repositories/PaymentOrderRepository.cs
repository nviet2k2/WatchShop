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
    
    public interface IPaymentOrderRepository : IBaseRepository<OrderPaymentModel>
    {

    }
    public class PaymentOrderRepository : BaseRepository<OrderPaymentModel>, IPaymentOrderRepository
    {
        public PaymentOrderRepository(WebApiContext context) : base(context)
        {

        }
    }
}
