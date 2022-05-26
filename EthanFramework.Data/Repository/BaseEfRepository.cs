using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EthanFramework.Data.Repository
{
    public abstract class BaseEfRepository<TModel, TDbContext>
        where TModel : class
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        public BaseEfRepository(TDbContext context)
        {
            _context = context;
        }

        public Task<List<TModel>> GetCollectionAsync()
        {
            return _context.Set<TModel>().ToListAsync();
        }

        public Task<TModel> GetByIdAsync(int id)
        {
            return _context.Set<TModel>().FindAsync(id).AsTask();
        }

        public async Task<int> DeleteByIdAsync(TModel entity)
        {
            _context.Set<TModel>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public Task<int> CreateAsync(TModel entity)
        {
            _context.Set<TModel>().AddAsync(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(TModel entity)
        {
            _context.Set<TModel>().Update(entity);
            return _context.SaveChangesAsync();
        }
    }
}
