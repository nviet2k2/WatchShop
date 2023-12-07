using Database;
using Database.Models;
using Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBrandRepository : IBaseRepository<BrandModel>
    {

    }
    public class BrandRepository : BaseRepository<BrandModel>, IBrandRepository
    {
        public BrandRepository(WebApiContext context) : base(context)
        {

        }
    }
}
