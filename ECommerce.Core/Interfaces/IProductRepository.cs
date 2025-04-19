using ECommerce.Core.DTOs;
using ECommerce.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<bool> AddAsync(AddProductDto productDto);
        Task<bool> UpdateAsync(UpdateProductDto productDto);
        Task DeleteAsync(Product product);

    }
}
