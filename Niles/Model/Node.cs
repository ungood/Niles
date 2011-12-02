using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Niles.Model
{
    [DataContract]
    public class Node
    {
        [DataMember(Name="nodeName")]
        public string Name { get; set; }

        [DataMember(Name = "nodeDescription")]
        public string Description { get; set; }

        [DataMember(Name="jobs")]
        public IList<Job> Jobs { get; set; }

        [DataMember(Name = "views")]
        public IList<View> Views { get; set; }

        public Node()
        {
            Jobs = new List<Job>();
            Views = new List<View>();
        }
    }
}
