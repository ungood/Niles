#region License
// Copyright 2011 Jason Walker
// ungood@onetrue.name
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Niles.Model
{
    public class Node
    {
        public string NodeName { get; set; }
        public string NodeDescription { get; set; }
        public string Description { get; set; }

        public IList<JobReference> Jobs { get; set; }
        public IList<ViewReference> Views { get; set; }
        public string Mode { get; set; }
        public int NumExecutors { get; set; }
        public ViewReference PrimaryView { get; set; }

        public bool QuietingDown { get; set; }
        public int SlaveAgentPort { get; set; }
        public bool UseCrumbs { get; set; }
        public bool UseSecurity { get; set; }

        // Items I'm not sure about
        // assignedLabels
        // overallLoad

        public Node()
        {
            Jobs = new List<JobReference>();
            Views = new List<ViewReference>();
        }
    }
}
