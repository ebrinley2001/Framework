using System.Collections.Generic;
using System.Threading.Tasks;

namespace EthanFramework.BC
{
    public interface IBaseEfBc<TModel>
        where TModel : class
    {
        public Task<List<TModel>> GetCollectionAsync();
        public Task<TModel> GetByIdAsync(int id);
        public Task<int> DeleteByIdAsync(TModel entity);
        public Task<int> CreateAsync(TModel entity);
        public Task<int> UpdateAsync(TModel entity);
    }
}
