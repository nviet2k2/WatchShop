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
    public interface IReceiptDetailRepository : IBaseRepository<ReceiptDetailModel>
    {

    }
    public class ReceiptDetailRepository : BaseRepository<ReceiptDetailModel>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(WebApiContext context) : base(context)
        {

        }
    }
}
