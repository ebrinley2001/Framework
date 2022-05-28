using Newtonsoft.Json;

namespace EthanFramework.Common.Helpers
{
    public static class TypeConverter
        
    {
        public static string ClassToJson<TModel>(TModel body) where TModel : class
        {
            return JsonConvert.SerializeObject(body);
        }

        public static TModel JsonToClass<TModel>(string json) where TModel : class
        {
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            return JsonConvert.DeserializeObject<TModel>(json);
        }
    }
}
