using AutoMapper;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Services;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories.Service;
using Microsoft.AspNetCore.Identity;
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
        private readonly IConnectionMultiplexer redis;
        // User registeration
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        // User Login
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken token;

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        public ICustomerCartRepository CustomerCart { get;}
        public IAuth Auth { get; }

        public UnitofWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService,
            IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken token)

        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            this.redis = redis;
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.token = token;
            CategoryRepository = new CategoryRepository(_context);
            PhotoRepository = new PhotoRepository(_context);
            ProductRepository = new ProductRepository(_context, _mapper, _imageManagementService);
            CustomerCart = new CustomerCartRepository(redis);
            Auth = new AuthRepository(userManager, emailService, signInManager, token);
        }
    }
}
