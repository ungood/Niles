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

using System;
using NUnit.Framework;
using Niles.Model;
using Niles.Monitor;

namespace Niles.Test.Monitor
{
    [TestFixture]
    public class ExceptionTests : MonitorTestFixture
    {
        [Test]
        public void Creating_Monitor_With_Null_BaseUri_Should_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => new JenkinsMonitor(null));
        }

        [Test]
        public void Exception_While_Retrieving_Node_Should_Raise_PollingError()
        {
            MockClient.GetResource<Node>(null)
                .ThrowsForAnyArgs(new Exception("Error"));
            
            Monitor.Poll();

            AssertEventsFired("Error");
        }

        [Test]
        public void Exception_While_Expanding_Job_Should_Raise_PollingError()
        {
            var mockJob = new Job {
                Url = new Uri("urn://mockurl"),
                LastBuild = new Build(),
            };

            SetupMockNode(mockJob);

            MockClient.Expand(mockJob)
                .Throws(new Exception("Error"));

            Monitor.Poll();

            AssertEventsFired("Error");
        }

        [Test]
        public void Exception_While_Expanding_LastBuild_Should_Raise_PollingError()
        {
            var mockJob = new Job {
                Url = new Uri("urn://mockurl"),
                LastBuild = new Build(),
            };

            SetupMockNode(mockJob);
            
            SetupMockExpand(mockJob);

            MockClient.Expand(mockJob.LastBuild)
                .Throws(new Exception("Error"));

            Monitor.Poll();

            AssertEventsFired("Error");
        }
    }
}
