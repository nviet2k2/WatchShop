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
    
    public interface ISupplierRepository : IBaseRepository<SupplierModel>
    {

    }
    public class SupplierRepository : BaseRepository<SupplierModel>, ISupplierRepository
    {
        public SupplierRepository(WebApiContext context) : base(context)
        {

        }
    }
}
