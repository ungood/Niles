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
using Niles.Client;
using Niles.Model;

namespace Niles.Test.JsonClient
{
    [TestFixture]
    public class NodeTests : JsonClientFixture
    {
        [Test]
        public void Can_Get_Valid_Node_Resource()
        {
        Expect("ValidNode.json", "ValidNode");
            var uri = new Uri("mock://test/ValidNode/");
            var node = Client.GetResource<Node>(uri);

            Assert.IsNotNull(node);
            Assert.AreEqual("NORMAL", node.Mode);
            Assert.AreEqual("the master Jenkins node", node.NodeDescription);
            Assert.AreEqual(6, node.NumExecutors);
            Assert.AreEqual("Example Build System", node.Description);

            Assert.AreEqual(5, node.Jobs.Count);
            Assert.AreEqual("blue", node.Jobs[3].Color);
            Assert.AreEqual("alfred.automation", node.Jobs[0].Name);
            Assert.AreEqual(new Uri("mock://test:8080/job/alfred.binarypromotion.branch/"), node.Jobs[1].Url);

            Assert.AreEqual("All", node.PrimaryView.Name);
            Assert.AreEqual(new Uri("mock://test:8080/"), node.PrimaryView.Url);

            Assert.AreEqual(false, node.QuietingDown);
            Assert.AreEqual(0, node.SlaveAgentPort);
            Assert.AreEqual(false, node.UseCrumbs);
            Assert.AreEqual(true, node.UseSecurity);

            Assert.AreEqual(3, node.Views.Count);
            Assert.AreEqual("badabing", node.Views[1].Name);
            Assert.AreEqual(new Uri("mock://test:8080/view/operations/"), node.Views[2].Url);
        }

        [Test]
        public void Throws_JenkinsClientException_When_Accessing_NonExistingResource()
        {
            var resourceUri = new Uri("mock://doesnotexist/");
            Assert.Throws<ClientException>(() => Client.GetResource<Node>(resourceUri));
        }

        [Test]
        public void Throws_JenkinsClientException_When_Accessing_InvalidResource()
        {
            var resourceUri = new Uri("mock://test/InvalidNode/");
            Expect("InvalidNode.json", "InvalidNode");
            Assert.Throws<ClientException>(() => Client.GetResource<Node>(resourceUri));
        }
    }
}
