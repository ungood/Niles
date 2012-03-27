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
using NSubstitute;
using NUnit.Framework;
using Niles.Client;
using Niles.Model;
using Niles.Monitor;

namespace Niles.Test.Monitor
{
    public class MonitorTestFixture
    {
        public IJenkinsClient MockClient { get; private set; }
        public JenkinsMonitor Monitor { get; private set; }
        private EventRecorder recorder;

        [SetUp]
        public void Setup()
        {
            MockClient = Substitute.For<IJenkinsClient>();
            Monitor = new JenkinsMonitor(new Uri("urn://localhost"), MockClient);
            recorder = new EventRecorder(Monitor);
        }

        protected Job SetupMockJob(string name, int buildNumber = 0, bool isBuilding = false, string result = "")
        {
            var job = new Job {
                Name = name,
                Url = new Uri("urn:" + name),
                LastBuild = new Build {
                    Number = buildNumber,
                    Building = isBuilding,
                    Result = result,
                    Url = new Uri("urn:" + name + "-lastbuild")
                }
            };
            SetupMockExpand(job);
            SetupMockExpand(job.LastBuild);
            return job;
        }

        protected Node SetupMockNode(params Job[] jobs)
        {
            var node = new Node {
                Jobs = jobs
            };
            SetupMockResource(node);
            return node;
        }

        protected void SetupMockResource<T>(T result)
        {
            MockClient.GetResourceAsync<T>(null)
                .ReturnsForAnyArgsAsync(result);
        }

        protected void SetupMockExpand<T>(T objectToExpand)
            where T : class, IResource
        {
            MockClient.ExpandAsync(objectToExpand)
                .ReturnsAsync(objectToExpand);
        }

        protected void AssertEventsFired(params string[] expected)
        {
            CollectionAssert.AreEqual(expected, recorder);
            recorder.Clear();
        }
    }
}