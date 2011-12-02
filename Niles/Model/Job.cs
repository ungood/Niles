using System.Runtime.Serialization;

namespace Niles.Model
{
    [DataContract]
    public class Job : Resource
    {
        [DataMember(Name="color")]
        public string Color { get; set; }
    }
}
