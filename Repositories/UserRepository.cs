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
    public interface IUserRepository : IBaseRepository<UserModel>
    {
       
    }
    public class UserRepository : BaseRepository<UserModel>, IUserRepository { 
        public UserRepository(WebApiContext context) : base(context)
        {

        }
    }
}
