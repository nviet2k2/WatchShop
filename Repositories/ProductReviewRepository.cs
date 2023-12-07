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
    public interface IProductReviewRepository : IBaseRepository<ProductReviewModel>
    {

    }
    public class ProductReviewRepository : BaseRepository<ProductReviewModel>, IProductReviewRepository
    {
        public ProductReviewRepository(WebApiContext context) : base(context)
        {

        }
    }
}
