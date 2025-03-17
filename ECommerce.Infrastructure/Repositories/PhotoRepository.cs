using ECommerce.Core.Entities.Product;
using ECommerce.Core.Interfaces;
using ECommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class PhotoRepository :GenericRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(AppDbContext context):base(context)
        {
        }

    }
}
