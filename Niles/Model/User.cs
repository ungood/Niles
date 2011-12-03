using System;
using Newtonsoft.Json;

namespace Niles.Model
{
    public class UserReference : IReference<User>
    {
        [JsonProperty("absoluteUrl")]
        public Uri Url { get; set; }

        public string FullName { get; set; }
    }

    public class User
    {
        [JsonProperty("absoluteUrl")]
        public Uri Url { get; set; }

        public string FullName { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        // property
    }
}