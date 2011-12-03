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
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Niles.Test.MockWebRequest;
using TechTalk.SpecFlow;

namespace Niles.Test.Steps
{
    [Binding]
    public class JenkinsClientSteps
    {
        public static JenkinsClient Client
        {
            get { return ScenarioContext.Current.GetOrCreate("Client", SetupClient); }
            set { ScenarioContext.Current["Client"] = value; }
        }

        private static string ToJson<T>(T obj)
        {
            var serializer = new DataContractJsonSerializer(typeof (T));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Flush();

            return Encoding.Default.GetString(stream.ToArray());
        }

        private static JenkinsClient SetupClient()
        {
            var context = new MockWebRequestFactory("mock");
            var baseUri = new Uri("mock://test/");

            context.Expect(new Uri(baseUri, "api/json"), ToJson(NodeSteps.Node));
            
            //foreach(var job in JobSteps.Jobs)
                //context.Expect()

            return new JenkinsClient();
        }

        [When(@"I get the hudson element")]
        public void WhenIGetTheHudsonElement()
        {
            //Client.GetRoot();
        }

    }
}
