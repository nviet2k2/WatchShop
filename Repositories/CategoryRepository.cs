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
    public interface ICategoryRepository : IBaseRepository<CategoryModel>
    {

    }
    public class CategoryRepository : BaseRepository<CategoryModel>, ICategoryRepository
    {
        public CategoryRepository(WebApiContext context) : base(context)
        {

        }
    }
}
