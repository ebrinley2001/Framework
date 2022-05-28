using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EthanFramework.Sdk.ServiceInterfaces
{
    public interface IBaseApiService<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetCollectionAsync();
        Task<TModel> GetAsync(string Id);
        Task<int> PostAsync(TModel body);
        Task<int> DeleteAsync(string Id);
        Task<int> UpdateAsync(string id, TModel body);
        Task<HttpResponseMessage> CallClientWithBody(Func<string, HttpContent, Task<HttpResponseMessage>> callMethod, string body, string? id = null);
        Task<HttpResponseMessage> CallClientWithReturnValue(Func<string, Task<HttpResponseMessage>> callMethod, string? Id = null, Dictionary<string, string>? QueryParameters = null);
    }
}
