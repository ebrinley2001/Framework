using EthanFramework.Common.Helpers;
using EthanFramework.Sdk.ServiceInterfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EthanFramework.Sdk.Proxy
{
    public abstract class BaseApiProxy<TModel> : IBaseApiService<TModel>
        where TModel : class
    {
        private readonly IConfiguration _config;
        private string Uri { get; set; }
        private string Endpoint { get; set; }

        private HttpClient? client;

        public BaseApiProxy(IConfiguration config, IHttpClientFactory httpClientFactory, string ControllerName, string EndpointName)
        {
            _config = config;

            Uri = _config.GetValue<string>("ProxyConfig:" + ControllerName);
            Endpoint = EndpointName;

            client = CreateHttpClient(httpClientFactory);
        }

        public BaseApiProxy(IConfiguration config, string ControllerName, string EndpointName)
        {
            _config = config;

            Uri = _config.GetValue<string>("ProxyConfig:" + ControllerName);
            Endpoint = EndpointName;
        }

        public async Task<IEnumerable<TModel>> GetCollectionAsync()
        {
            HttpResponseMessage data;

            if (client != null)
            {
                data = await CallClientWithReturnValue(client.GetAsync);
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    data = await CallClientWithReturnValue(client.GetAsync);
                }
            }
            return TypeConverter.JsonToClass<IEnumerable<TModel>>(await data.Content.ReadAsStringAsync());
        }

        public async Task<TModel> GetAsync(string Id)
        {
            HttpResponseMessage data;

            if (client != null)
            {
                data = await CallClientWithReturnValue(client.GetAsync, Id);
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    data = await CallClientWithReturnValue(client.GetAsync, Id);
                }
            }
            return TypeConverter.JsonToClass<TModel>(await data.Content.ReadAsStringAsync());
        }

        public async Task<int> PostAsync(TModel body)
        {
            string json = TypeConverter.ClassToJson<TModel>(body);
            HttpResponseMessage result;

            if (client != null)
            {
                result = await CallClientWithBody(client.PostAsync, json);
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    result = await CallClientWithBody(client.PostAsync, json);
                }
            }
            return (int)result.StatusCode;
        }

        public async Task<int> DeleteAsync(string Id)
        {
            HttpResponseMessage result;

            if (client != null)
            {
                result = await CallClientWithReturnValue(client.DeleteAsync, Id);
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    result = await CallClientWithReturnValue(client.DeleteAsync, Id);
                }
            }
            return (int)result.StatusCode;
        }

        public async Task<int> UpdateAsync(string id, TModel body)
        {
            string json = TypeConverter.ClassToJson<TModel>(body);
            HttpResponseMessage result;

            if (client != null)
            {
                result = await CallClientWithBody(client.PutAsync, json, id);
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    result = await CallClientWithBody(client.PutAsync, json, id);
                }
            }
            return (int)result.StatusCode;
        }

        public Task<HttpResponseMessage> CallClientWithBody(Func<string, HttpContent, Task<HttpResponseMessage>> callMethod, string body, string? id = null)
        {
            string route = BuildRoute(Uri, Endpoint, id);
            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");

            return callMethod(route, httpContent);
        }

        public Task<HttpResponseMessage> CallClientWithReturnValue(Func<string, Task<HttpResponseMessage>> callMethod, string? Id = null, Dictionary<string, string>? QueryParameters = null)
        {
            string route;
            route = BuildRoute(Uri, Endpoint, Id, QueryParameters);

            return callMethod(route);
        }

        private HttpClient CreateHttpClient(IHttpClientFactory httpClientFactory)
        {
            var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        private string BuildRoute(string Uri, string Endpoint, string? Id = null, Dictionary<string, string>? QueryParameters = null)
        {
            string route = $"{Uri}/api/{Endpoint}";

            if (QueryParameters != null)
            {
                route += "?";

                foreach (var param in QueryParameters)
                {
                    route += $"{param.Key}={param.Value}&";
                }
            }
            else if (Id != null)
            {
                route += $"/{Id}";
            }
            return route;
        }
    }
}
