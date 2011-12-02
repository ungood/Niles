using System;
using System.Runtime.Serialization;

namespace Niles.Model
{
    [DataContract]
    public abstract class Resource
    {
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name = "url")]
        public Uri Url { get; set; }
    }
}
