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
    public interface IAccessoryRepository : IBaseRepository<AccessoryModel>
    {

    }
    public class AccessoryRepository : BaseRepository<AccessoryModel>, IAccessoryRepository
    {
        public AccessoryRepository(WebApiContext context) : base(context)
        {

        }
    }
}
