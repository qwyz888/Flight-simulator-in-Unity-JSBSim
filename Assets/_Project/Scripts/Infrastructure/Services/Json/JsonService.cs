using Infrastructure.Services.Json.Core;
using Newtonsoft.Json;

namespace Infrastructure.Services.Json
{
    public class JsonService : IJsonService
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };

        public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, settings: _settings);

        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, settings: _settings);
    }
}