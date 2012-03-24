using NUnit.Framework;
using Niles.Client;

namespace Niles.Test.JsonClient
{
    public class JsonClientFixture : MockWebRequestFixture
    {
        protected JsonJenkinsClient Client { get; private set; }

        [SetUp]
        public void Setup()
        {
            Client = new JsonJenkinsClient();
        }
    }
}
