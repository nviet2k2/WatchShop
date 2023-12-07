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
   
    public interface IVoucherRepository : IBaseRepository<VoucherModel>
    {

    }
    public class VoucherRepository : BaseRepository<VoucherModel>, IVoucherRepository
    {
        public VoucherRepository(WebApiContext context) : base(context)
        {

        }
    }
}
