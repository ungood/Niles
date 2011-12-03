using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Niles.Test.MockWebRequest
{
    public class MockWebRequestFactory : IWebRequestCreate
    {
        public string Prefix { get; private set; }

        public MockWebRequestFactory(string prefix)
        {
            Prefix = prefix;
            WebRequest.RegisterPrefix(Prefix, this);
        }

        private readonly IDictionary<Uri, MockWebResponse> responses
            = new Dictionary<Uri, MockWebResponse>();

        public void Expect(Uri uri, MockWebResponse response)
        {
            responses[uri] = response;
        }

        public void Expect(Uri uri, string response)
        {
            Expect(uri, new MockWebResponse(response));
        }

        public void Expect(Uri uri, Stream responseStream)
        {
            using(var streamReader = new StreamReader(responseStream))
            {
                Expect(uri, streamReader.ReadToEnd());
            }
        }

        public WebRequest Create(Uri uri)
        {
            MockWebResponse response;

            responses.TryGetValue(uri, out response);
            return new MockWebRequest(uri, response);
        }
    }
}
