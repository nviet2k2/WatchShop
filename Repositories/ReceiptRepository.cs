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
    public interface IReceiptRepository : IBaseRepository<ReceiptModel>
    {

    }
    public class ReceiptRepository : BaseRepository<ReceiptModel>, IReceiptRepository
    {
        public ReceiptRepository(WebApiContext context) : base(context)
        {

        }
    }
    
}
