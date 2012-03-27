using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Niles.Json
{
    public class JenkinsJsonSerializer
    {
        private readonly JsonSerializer serializer;

        public JenkinsJsonSerializer()
        {
            serializer = new JsonSerializer {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public T ReadObject<T>(Stream stream)
        {
            using(var streamReader = new StreamReader(stream))
            using(var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}
