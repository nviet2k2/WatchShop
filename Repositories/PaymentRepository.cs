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
    
    public interface IPaymentRepository : IBaseRepository<PaymentModel>
    {

    }
    public class PaymentRepository : BaseRepository<PaymentModel>, IPaymentRepository
    {
        public PaymentRepository(WebApiContext context) : base(context)
        {

        }
    }
}
