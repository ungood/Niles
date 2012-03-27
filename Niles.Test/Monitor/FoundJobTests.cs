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
    public class FoundJobTests : MonitorTestFixture
    {
        [Test]
        public void New_Job_Should_Fire_FoundJob_Event()
        {
            SetupMockNode();
            Monitor.Poll();

            var newJob = SetupMockJob("newjob");
            SetupMockNode(newJob);
            Monitor.Poll();

            AssertEventsFired("newjob found");
        }

        [Test]
        public void Job_Should_Be_Found_Only_Once()
        {
            var job = SetupMockJob("newjob");

            SetupMockNode(job);
            Monitor.Poll();
            Monitor.Poll();

            AssertEventsFired("newjob found");
        }
    }
}
