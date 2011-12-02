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
    public class JobSteps
    {
        public static IEnumerable<Job> Jobs
        {
            get { return ScenarioContext.Current.GetOrCreate<IEnumerable<Job>>("Jobs"); }
            set { ScenarioContext.Current["Jobs"] = value; }
        }

        [Given(@"I have the following jobs:")]
        public void GivenIHaveTheFollowingJobs(Table table)
        {
            Jobs = table.CreateSet<Job>();
        }

    }
}
