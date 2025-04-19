using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Entities.Product;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
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
