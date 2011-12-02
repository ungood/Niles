using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Niles.Model;
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

            return new JenkinsClient(new Uri("mock://test/"));
        }

        [When(@"I get the hudson element")]
        public void WhenIGetTheHudsonElement()
        {
            Client.GetRoot();
        }

    }
}
