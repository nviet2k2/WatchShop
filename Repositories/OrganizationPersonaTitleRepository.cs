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
   
    public interface IOrganizationPersonaTitleRepository : IBaseRepository<OrganizationPersonaTitleModel>
    {

    }
    public class OrganizationPersonaTitleRepository : BaseRepository<OrganizationPersonaTitleModel>, IOrganizationPersonaTitleRepository
    {
        public OrganizationPersonaTitleRepository(WebApiContext context) : base(context)
        {

        }
    }
}
