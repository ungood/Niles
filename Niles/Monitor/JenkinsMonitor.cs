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
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Niles.Client;
using Niles.Model;

namespace Niles.Monitor
{
    /// <summary>
    /// Polls a jenkins node and raises events when jobs or builds change states.
    /// </summary>
    public class JenkinsMonitor
    {
        private readonly ConcurrentDictionary<Uri, Build> lastSeenBuilds
            = new ConcurrentDictionary<Uri, Build>();
        private readonly IJenkinsClient client;

        public Uri BaseUri { get; private set; }

        public JenkinsMonitor(Uri baseUri, IJenkinsClient client = null)
        {
            if(baseUri == null)
                throw new ArgumentNullException("baseUri");

            BaseUri = baseUri;
            this.client = client ?? new JsonJenkinsClient();
        }

        public event EventHandler<PollingErrorEventArgs> PollingError = delegate { };
        public event EventHandler<JobFoundEventArgs> FoundJob = delegate { };
        public event EventHandler<BuildEventArgs> BuildStarted = delegate { };
        public event EventHandler<BuildEventArgs> BuildFinished = delegate { };
        public event EventHandler<BuildEventArgs> BuildFailed = delegate { };
        public event EventHandler<BuildEventArgs> BuildSucceeded = delegate { };
        public event EventHandler<BuildEventArgs> BuildAborted = delegate { };
        public event EventHandler<BuildEventArgs> BuildUnstable = delegate { };
        
        private const string TreeParameter = "jobs[name,displayName,url,lastBuild[url,number,building,result]]";

        public void Poll(int timeout = Timeout.Infinite)
        {
            var tasks = new Task[0];

            try
            {
                var node = client.GetResource<Node>(BaseUri, TreeParameter);
                tasks = node.Jobs.Select(UpdateJobAsync).ToArray();
            }
            catch(Exception ex)
            {
                PollingError(this, new PollingErrorEventArgs(ex));
            }

            Task.WaitAll(tasks);
        }

        private Task UpdateJobAsync(Job jobInfo)
        {
            return Task.Factory.StartNew(() => UpdateJob(jobInfo));
        }

        private void UpdateJob(Job jobInfo)
        {
            try
            {
                Build lastSeenBuild;
                lastSeenBuilds.TryGetValue(jobInfo.Url, out lastSeenBuild);

                if(lastSeenBuild == null)
                {
                    var state = GetJobState(jobInfo, "");
                    FoundJob(this, new JobFoundEventArgs(state.Job));
                    return;
                }

                if(lastSeenBuild.Number != jobInfo.LastBuild.Number)
                {
                    var state = GetJobState(jobInfo, lastSeenBuild.Result);
                    BuildStarted(this, new BuildEventArgs(state.Job, state.CurrentBuild));
                    if(!state.Job.LastBuild.Building)
                        OnBuildFinished(state, state.CurrentBuild.Result != lastSeenBuild.Result);
                    return;
                }

                if(lastSeenBuild.Building != jobInfo.LastBuild.Building)
                {
                    var state = GetJobState(jobInfo, lastSeenBuild.Result);
                    if(!state.Job.LastBuild.Building)
                        OnBuildFinished(state, state.CurrentBuild.Result != lastSeenBuild.Result);
                }
            }
            catch(Exception ex)
            {
                PollingError(this, new PollingErrorEventArgs(ex));
            }
        }

        private void OnBuildFinished(JobBuildTuple buildTuple, bool isStatusChange)
        {
            var e = new BuildEventArgs(buildTuple.Job, buildTuple.CurrentBuild, isStatusChange);
            BuildFinished(this, e);
            switch(buildTuple.CurrentBuild.Result.ToUpperInvariant())
            {
                case "SUCCESS":
                    BuildSucceeded(this, e);
                    break;
                case "FAILURE":
                    BuildFailed(this, e);
                    break;
                case "ABORTED":
                    BuildAborted(this, e);
                    break;
                case "UNSTABLE":
                    BuildUnstable(this, e);
                    break;
            }
        }

        private JobBuildTuple GetJobState(Job job, string previousResult)
        {
            job = client.Expand(job);
            var currentBuild = job.LastBuild == null
                ? new Build {Building = false, Number = 0, Result = ""}
                : client.Expand(job.LastBuild);
            currentBuild.Result = currentBuild.Result ?? previousResult;
            lastSeenBuilds[job.Url] = currentBuild;
            
            return new JobBuildTuple {
                Job = job,
                CurrentBuild = currentBuild,
            };
        }

        private struct JobBuildTuple
        {
            public Job Job { get; set; }
            public Build CurrentBuild { get; set; }
        }
    }
}
