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

using NUnit.Framework;

namespace Niles.Test.Monitor
{
    [TestFixture]
    public class StatusTests : MonitorTestFixture
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            SetupMockNode(
                SetupMockJob("job", 0, false));

            Monitor.Poll();
            AssertEventsFired("job found");
        }

        private void StartJob(int jobNumber)
        {
            SetupMockNode(
                SetupMockJob("job", jobNumber, true));
            Monitor.Poll();
        }

        private void FinishJob(int jobNumber, string result)
        {
            SetupMockNode(
                SetupMockJob("job", jobNumber, false, result));
            Monitor.Poll();
        }

        [Test]
        public void Job_Started_Between_Polls()
        {
            StartJob(1);
            AssertEventsFired("job started");
        }

        [Test]
        public void Job_Started_And_Finished_Between_Polls()
        {
            FinishJob(1, "");
            AssertEventsFired("job started", "job finished");
        }

        [Test]
        public void Job_Started_And_Finihsed_Between_Consecutive_Polls()
        {
            StartJob(1);
            AssertEventsFired("job started");

            FinishJob(1, "");
            AssertEventsFired("job finished");
        }

        [Test]
        public void Job_Succeeding()
        {
            FinishJob(1, "SUCCESS");
            AssertEventsFired("job started", "job finished", "job succeeded");

            FinishJob(2, "SUCCESS");
            AssertEventsFired("job started", "job finished", "job succeeding");
        }

        [Test]
        public void Job_Failing()
        {
            FinishJob(1, "FAILURE");
            AssertEventsFired("job started", "job finished", "job failed");

            FinishJob(2, "FAILURE");
            AssertEventsFired("job started", "job finished", "job failing");
        }

        [Test]
        public void Job_Aborting()
        {
            FinishJob(1, "ABORTED");
            AssertEventsFired("job started", "job finished", "job aborted");

            FinishJob(2, "ABORTED");
            AssertEventsFired("job started", "job finished", "job aborting");
        }

        [Test]
        public void Job_Unstabling()
        {
            FinishJob(1, "UNSTABLE");
            AssertEventsFired("job started", "job finished", "job failed");

            FinishJob(2, "UNSTABLE");
            AssertEventsFired("job started", "job finished", "job failing");
        }

        [Test]
        public void Job_Changing_Status()
        {
            FinishJob(1, "SUCCESS");
            AssertEventsFired("job started", "job finished", "job succeeded");

            StartJob(2);
            AssertEventsFired("job started");

            FinishJob(2, "FAILURE");
            AssertEventsFired("job finished", "job failed");

            FinishJob(3, "FAILURE");
            AssertEventsFired("job started", "job finished", "job failing");

            FinishJob(4, "SUCCESS");
            AssertEventsFired("job started", "job finished", "job succeeded");
        }
    }
}
