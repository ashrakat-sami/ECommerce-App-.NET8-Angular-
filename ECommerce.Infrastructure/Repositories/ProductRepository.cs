using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Entities.Product;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Core.Sharing;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
   public class ProductRepository :GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }
        public async Task<IEnumerable<ProductDto>> GetAllAsync(ProductParam productParam)
        {
            var query = context.Products
                .Include(c => c.Category)
                .Include(p => p.Photos)
                .AsNoTracking();



            //filtering by word
            if (!string.IsNullOrEmpty(productParam.Search))
            {


                //OLD IMPLEMENTATION
                //query = query.Where(m => m.Name.ToLower().Contains(productParam.Search.ToLower()) 
                //||
                //m.Description.ToLower().Contains(productParam.Search.ToLower()));

                //NEW IMPLEMENTATION
                var searchWords = productParam.Search.Split(' ');

                query = query.Where(m => searchWords.All(word =>
                   m.Name.ToLower().Contains(word.ToLower()) 
                   || m.Description.ToLower().Contains(word.ToLower())

                ));
              
            }

            if (productParam.CategoryId.HasValue)
            
                query = query.Where(m => m.CategoryId == productParam.CategoryId);
            
                if (!string.IsNullOrEmpty(productParam.Sort))
                {
                    query = productParam.Sort switch
                    {
                        "PriceAsc" => query.OrderBy(m => m.NewPrice),
                        "PriceDesc" => query.OrderByDescending(m => m.NewPrice),
                        _ => query.OrderBy(m => m.Name),
                    };
                }


            query = query.Skip((productParam.PageNumber - 1) * productParam.PageSize)
                .Take(productParam.PageSize);


            var result=mapper.Map<List<ProductDto>>( query);
            return result;
        }

        public async Task<bool> AddAsync(AddProductDto productDto)
        {
            if (productDto == null) return false;

            var product=mapper.Map<Product>(productDto);

           await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var imagePath = await imageManagementService.AddImageAsync(productDto.Photo, productDto.Name);

            var photo = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId=product.Id,


            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductDto productDto)
        {
           if(productDto is null)
            {
                return false;
            }

            var product = await context.Products.Include(m => m.Category)
                 .Include(m => m.Photos)
                 .FirstOrDefaultAsync(m => m.Id == productDto.Id);

            if (product is null)
                return false;

            mapper.Map(productDto,product);

            var photoes = await context.Photos.Where(m => m.ProductId == productDto.Id).ToListAsync();
            foreach(var item in photoes)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Photos.RemoveRange(photoes);

            var imagePath = await imageManagementService.AddImageAsync(productDto.Photo, productDto.Name);

            var photo = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = productDto.Id,


            }).ToList();
            await context.Photos.AddRangeAsync(photo);

            await context.SaveChangesAsync();
            return true;
        }


        public async Task DeleteAsync(Product product)
        {
            var photoes = await context.Photos.Where(m => m.ProductId == product.Id).ToListAsync();

            foreach (var item in photoes)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();

        }
    }
}
