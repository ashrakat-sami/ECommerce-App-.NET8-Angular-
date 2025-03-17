using ECommerce.Core.Interfaces;
using ECommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
   public class UnitofWork: IUnitofWork
    {
        private readonly AppDbContext context;
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        public UnitofWork(AppDbContext context)
        {
            this.context = context;
            ProductRepository = new ProductRepository(context);
            CategoryRepository = new CategoryRepository(context);
            PhotoRepository = new PhotoRepository(context);

        }
    
    
    }
}
