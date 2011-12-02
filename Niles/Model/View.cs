using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Niles.Model
{
    [DataContract]
    public class View : Resource
    {
        [DataMember(Name="description")]
        public string Description { get; set; }

        [DataMember(Name="jobs")]
        public IList<Job> Jobs { get; set; }

        public View()
        {
            Jobs = new List<Job>();
        }
    }
}
