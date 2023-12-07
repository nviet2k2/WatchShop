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

    public interface IRoleRepository : IBaseRepository<RoleModel>
    {

    }
    public class RoleRepository : BaseRepository<RoleModel>, IRoleRepository
    {
        public RoleRepository(WebApiContext context) : base(context)
        {

        }
    }
}
