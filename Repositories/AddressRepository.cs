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

    public interface IAddressRepository : IBaseRepository<AddressModel>
    {

    }
    public class AddressRepository : BaseRepository<AddressModel>, IAddressRepository
    {
        public AddressRepository(WebApiContext context) : base(context)
        {

        }
    }
}
