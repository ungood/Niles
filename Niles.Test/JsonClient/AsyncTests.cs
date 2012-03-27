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
using System.Threading.Tasks;
using NUnit.Framework;
using Niles.Model;

namespace Niles.Test.JsonClient
{
    [TestFixture]
    public class AsyncTests : JsonClientFixture
    {
        //[Test]
        //public void Can_Get_Valid_Job_Resource_Async()
        //{
        //    Expect("ValidJob.json", "ValidJob");
        //    var task = Client.GetResourceAsync<Job>(new Uri("mock://test/ValidJob/"));
        //    task.ContinueWith(t => JobTests.AssertJobIsCorrect(t.Result),
        //        TaskContinuationOptions.ExecuteSynchronously);
        //}

        //[Test]
        //public void Can_Expand_Valid_Job_Resource_Async()
        //{
        //    Expect("ValidJob.json", "ValidJob");
        //    var job = new Job {
        //        Name = "alfred.automation",
        //        Url = new Uri("mock://test/ValidJob/"),
        //        Color = "red"
        //    };

        //    var task = Client.ExpandAsync(job);
        //    task.ContinueWith(t => JobTests.AssertJobIsCorrect(t.Result),
        //        TaskContinuationOptions.ExecuteSynchronously);
        //}
    }
}
