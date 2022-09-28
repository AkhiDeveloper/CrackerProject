using CrackerProject.API.Interfaces;
using CrackerProject.API.Model;

namespace CrackerProject.API.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IMongoContext context) : base(context)
        {
        }
    }
}
