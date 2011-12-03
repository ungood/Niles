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

namespace Niles.Model
{
    public class JobReference : IReference<Job>
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string Color { get; set; }
    }

    public class Job
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Buildable { get; set; }
        public IList<BuildReference> Builds { get; set; }
        [JsonProperty("healthReport")]
        public IList<HealthReport> HealthReports { get; set; }
        public bool InQueue { get; set; }
        public bool KeepDependencies { get; set; }

        public BuildReference LastBuild { get; set; }
        public BuildReference LastCompletedBuild { get; set; }
        public BuildReference LastFailedBuild { get; set; }
        public BuildReference LastStableBuild { get; set; }
        public BuildReference LastSuccessfulBuild { get; set; }
        public BuildReference LastUnstableBuild { get; set; }
        public BuildReference LastUnsuccessfulBuild { get; set; }

        public int NextBuildNumber { get; set; }
        public bool ConcurrentBuild { get; set; }
        public IList<JobReference> DownstreamProjects { get; set; }
        public IList<JobReference> UpstreamProjects { get; set; }

        // Items I'm not sure about yet
        // actions
        // property
        // queueItem
        // scm

        public Job()
        {
            Builds = new List<BuildReference>();
            DownstreamProjects = new List<JobReference>();
            UpstreamProjects = new List<JobReference>();
            HealthReports = new List<HealthReport>();
        }
    }

    public class HealthReport
    {
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int Score { get; set; }
    }
}
