using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class CustomerCartRepository : ICustomerCartRepository
    {
        // to treat with db in redis
        private readonly IDatabase _database;
        public CustomerCartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }            // seed data for redis
        public Task<bool> DeleteCartAsync(string id)
        {
            return _database.KeyDeleteAsync(id);
        }
        public async Task<CustomerCart> GetCartAsync(string id)
        {
            var result = await _database.StringGetAsync(id);
            if (!string.IsNullOrEmpty(result));
            {
                // result is a json string so i want to deserialize it to CustomerCart object
                return JsonSerializer.Deserialize<CustomerCart>(result);
            }
            return null;
        }
        public async Task<CustomerCart> UpdateCartAsync(CustomerCart cart)
        {
            var _cart = await _database.StringSetAsync
                (cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(3));
            // Validation
            if(_cart)
            {
                return await GetCartAsync(cart.Id);
            }
            return null;
        }
    }
}
