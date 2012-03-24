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
using NUnit.Framework;
using Niles.Model;
using Nito.AsyncEx;

namespace Niles.Test.JsonClient
{
    [TestFixture]
    public class JobTests : JsonClientFixture
    {
        [Test]
        public void Can_Get_Valid_Job_Resource()
        {
            AsyncContext.Run(async () => {
                Expect("ValidJob.json", "ValidJob");
                var job = await Client.GetResourceAsync<Job>(new Uri("mock://test/ValidJob/"));
                AssertJobIsCorrect(job);
            });
        }

        [Test]
        public void Can_Expand_Valid_Job_Resource()
        {
            AsyncContext.Run(async () => {
                Expect("ValidJob.json", "ValidJob");
                var job = new Job {
                    Name = "alfred.automation",
                    Url = new Uri("mock://test/ValidJob/"),
                    Color = "red"
                };

                job = await Client.ExpandAsync(job);
                AssertJobIsCorrect(job);
            });
        }

        private void AssertJobIsCorrect(Job job)
        {
            Assert.IsNotNull(job);
            Assert.AreEqual("Build the Alfred application in the automation environment. ", job.Description);
            Assert.AreEqual("alfred.automation", job.DisplayName);
            Assert.AreEqual("alfred.automation", job.Name);
            Assert.AreEqual(new Uri("mock://test/job/alfred.automation/"), job.Url);
            Assert.AreEqual(true, job.Buildable);

            Assert.AreEqual(4, job.Builds.Count);
            Assert.AreEqual(26, job.Builds[3].Number);
            Assert.AreEqual(new Uri("mock://test:8080/job/alfred.automation/29/"), job.Builds[0].Url);
            Assert.AreEqual("red", job.Color);
            Assert.AreEqual(26, job.FirstBuild.Number);

            Assert.AreEqual(1, job.HealthReports.Count);
            Assert.AreEqual("Build stability: 3 out of the last 4 builds failed.", job.HealthReports[0].Description);
            Assert.AreEqual("health-20to39.png", job.HealthReports[0].IconUrl);
            Assert.AreEqual(25, job.HealthReports[0].Score);

            Assert.AreEqual(false, job.InQueue);
            Assert.AreEqual(false, job.KeepDependencies);
            Assert.AreEqual(29, job.LastBuild.Number);
            Assert.AreEqual(29, job.LastCompletedBuild.Number);
            Assert.AreEqual(29, job.LastFailedBuild.Number);
            Assert.AreEqual(26, job.LastStableBuild.Number);
            Assert.AreEqual(26, job.LastSuccessfulBuild.Number);
            Assert.IsNull(job.LastUnstableBuild);
            Assert.AreEqual(29, job.LastUnsuccessfulBuild.Number);
            Assert.AreEqual(30, job.NextBuildNumber);
            Assert.AreEqual(false, job.ConcurrentBuild);
            Assert.AreEqual(0, job.DownstreamProjects.Count);
            Assert.AreEqual(0, job.UpstreamProjects.Count);
        }

        [Test]
        public void Can_Get_Job_Resource_With_Tree()
        {
            AsyncContext.Run(async () => {
                Expect("ValidJobTree.json", "ValidJob", "?tree=name,color,builds[number,timestamp]");
                var job = await Client.GetResourceAsync<Job>(new Uri("mock://test/ValidJob/"), "name,color,builds[number,timestamp]");

                Assert.IsNotNull(job);
                Assert.AreEqual("alfred.automation", job.Name);
                Assert.AreEqual(4, job.Builds.Count);
                Assert.AreEqual(new DateTime(2011, 11, 29, 06, 03, 01, 433, DateTimeKind.Utc), job.Builds[3].Timestamp);
                Assert.AreEqual(29, job.Builds[0].Number);
                Assert.AreEqual("red", job.Color);
            });
        }
    }
}
