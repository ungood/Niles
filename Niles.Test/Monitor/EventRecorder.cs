#region License
// Copyright 2012 Jason Walker
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

using System.Collections;
using System.Collections.Generic;
using Niles.Monitor;

namespace Niles.Test.Monitor
{
    public class EventRecorder : IEnumerable<string>
    {
        private readonly IList<string> recordedEvents = new List<string>();

        public EventRecorder(JenkinsMonitor monitor)
        {
            monitor.PollingError += (sender, e) => recordedEvents.Add(e.Exception.Message);
            monitor.FoundJob += (sender, e) => recordedEvents.Add(e.Job.Name + " found");
            monitor.BuildStarted += (sender, e) => recordedEvents.Add(e.Job.Name + " started");
            monitor.BuildFinished += (sender, e) => recordedEvents.Add(e.Job.Name + " finished");
            monitor.BuildFailed += (sender, e) => AddStatusMessage("fail", e);
            monitor.BuildAborted += (sender, e) =>  AddStatusMessage("abort", e);
            monitor.BuildSucceeded += (sender, e) =>  AddStatusMessage("succeed", e);
            monitor.BuildUnstable += (sender, e) =>  AddStatusMessage("fail", e);
        }

        private void AddStatusMessage(string verb, BuildEventArgs e)
        {
            var message = e.StatusChanged ? verb + "ed" : verb + "ing";
            recordedEvents.Add(e.Job.Name + " " + message);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return recordedEvents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(", ", recordedEvents);
        }

        public void Clear()
        {
            recordedEvents.Clear();
        }
    }
}
