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

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Niles.Model
{
    public class BuildReference : IReference<Build>
    {
        public int Number { get; set; }
        public Uri Url { get; set; }
    }

    public class Build
    {
        public int Number { get; set; }
        public Uri Url { get; set; }

        // TODO:
        // action -> parameter, causes, build by branch name...

        public bool Building { get; set; }
        public int Duration { get; set; }
        public string FullDisplayName { get; set; }
        public string Id { get; set; }
        public bool KeepLog { get; set; }
        public string Result { get; set; }
        public string BuiltOn { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Timestamp { get; set; }

        public IList<Artifact> Artifacts { get; set; }

        public ChangeSet ChangeSet { get; set; }
        public IList<UserReference> Culprits { get; set; }

        public Build()
        {
            Culprits = new List<UserReference>();
        }
    }

    public class Artifact
    {
        public string DisplayPath { get; set; }
        public string FileName { get; set; }
        public string RelativePath { get; set; }
    }

    public class ChangeSet
    {
        public IList<ChangeSetItem> Items { get; set; }
        // TODO: kind

        public ChangeSet()
        {
            Items = new List<ChangeSetItem>();
        }
    }

    public class ChangeSetItem
    {
        public UserReference Author { get; set; }
        public string Comment { get; set; }
        public string Id { get; set; }
        
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        public IList<ChangeSetPath> Paths { get; set; }

        public ChangeSetItem()
        {
            Paths = new List<ChangeSetPath>();
        }
    }

    public class ChangeSetPath
    {
        public string EditType { get; set; }
        public string File { get; set; }
    }
}