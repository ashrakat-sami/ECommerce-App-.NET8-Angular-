using AutoMapper;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Infrastructure.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
   public class UnitofWork: IUnitofWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private IImageManagementService _imageManagementService;

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }

        public ICustomerCartRepository CustomerCart { get;}

        public UnitofWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService,
            IConnectionMultiplexer redis)
            
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
             
            CategoryRepository = new CategoryRepository(_context);
            PhotoRepository = new PhotoRepository(_context);
            ProductRepository = new ProductRepository(_context, _mapper, _imageManagementService);
            CustomerCart = new CustomerCartRepository(redis);
        }
    }
}
