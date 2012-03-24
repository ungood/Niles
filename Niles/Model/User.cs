using System;
using Newtonsoft.Json;

namespace Niles.Model
{
    public class User : IResource
    {
        [JsonProperty("absoluteUrl")]
        public Uri Url { get; set; }

        public string FullName { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        // property
    }
}