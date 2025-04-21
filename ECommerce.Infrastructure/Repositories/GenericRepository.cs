using ECommerce.Core.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
       private readonly AppDbContext context;

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }
       
        public async Task AddAsync(T entity)
        {
          await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        =>await context.Set<T>().CountAsync();

        public async Task DeleteAsync(int id)
        {
            var entity=await GetByIdAsync(id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>  
        /// Asynchronously retrieves all entities of type T from the database,   
        /// including specified related entities based on the provided includes.  
        /// </summary>  
        /// <param name="includes">An optional array of expressions that specify   
        /// the related entities to include in the result.</param>  
        /// <returns>A read-only list of entities of type T.</returns> 
        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
           var query = context.Set<T>().AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return  await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity= await context.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var entity= await query.FirstOrDefaultAsync(x=>EF.Property<int>(x,"Id")==id);
            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }

       
    }
}
