using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Niles.Model;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Niles.Test.Steps
{
    [Binding]
    public class NodeSteps
    {
        public static Node Node
        {
            get { return ScenarioContext.Current.GetOrCreate<Node>("Node"); }
            set { ScenarioContext.Current["Node"] = value; }
        }

        [Given(@"a node named (.*)")]
        public void GivenANodeNamedTest(string name)
        {
            Node.Name = name;
        }
    }
}
