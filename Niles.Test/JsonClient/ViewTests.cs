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

namespace Niles.Test.JsonClient
{
    [TestFixture]
    public class ViewTests : JsonClientFixture
    {
        [Test]
        public void Can_Get_Valid_View_Resource()
        {
            Expect("ValidView.json", "ValidView");
            var view = Client.GetResource<View>(new Uri("mock://test/ValidView/"));

            Assert.IsNotNull(view);
            Assert.AreEqual("operations", view.Name);
            Assert.AreEqual(new Uri("mock://test:8080/view/operations/"), view.Url);
            Assert.AreEqual("Operations make things operate.", view.Description);
            Assert.AreEqual(3, view.Jobs.Count);
            Assert.AreEqual("blue", view.Jobs[0].Color);
            Assert.AreEqual(new Uri("mock://test:8080/job/operations.alfred.deploy/"), view.Jobs[1].Url);
            Assert.AreEqual("operations.alfred.qa", view.Jobs[2].Name);
        }
    }
}
