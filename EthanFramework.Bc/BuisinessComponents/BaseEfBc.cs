using EthanFramework.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EthanFramework.BC.BuisinessComponents
{
    public abstract class BaseEfBc<TModel, TBaseEfRepository>
        where TModel : class
        where TBaseEfRepository : IBaseEfRepository<TModel>
    {
        private readonly IBaseEfRepository<TModel> _repo;
        public BaseEfBc(IBaseEfRepository<TModel> repo)
        {
            _repo = repo;
        }

        public Task<List<TModel>> GetCollectionAsync()
        {
            return _repo.GetCollectionAsync();
        }

        public Task<TModel> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }
        public Task<int> DeleteByIdAsync(TModel entity)
        {
            return _repo.DeleteByIdAsync(entity);
        }

        public Task<int> CreateAsync(TModel entity)
        {
            return _repo.CreateAsync(entity);
        }

        public Task<int> UpdateAsync(TModel entity)
        {
            return _repo.UpdateAsync(entity);
        }
    }
}
