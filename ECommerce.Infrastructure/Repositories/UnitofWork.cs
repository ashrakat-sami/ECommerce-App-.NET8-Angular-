using AutoMapper;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
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
        private readonly IMapper mapper;
        private IImageManagementService imageManagementService;

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        public UnitofWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
            CategoryRepository = new CategoryRepository(context);
            PhotoRepository = new PhotoRepository(context);


            ProductRepository = new ProductRepository(context, mapper, imageManagementService);
        }


    }
}
