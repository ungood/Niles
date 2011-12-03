﻿using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Niles
{
    public class JenkinsSerializer
    {
        private readonly JsonSerializer serializer;

        public JenkinsSerializer()
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

        public void WriteObject<T>(Stream stream, T value)
        {
            using(var streamWriter = new StreamWriter(stream))
            {
                serializer.Serialize(streamWriter, value);
            }
        }
    }
}
