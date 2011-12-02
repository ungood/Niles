using System;
using System.IO;
using System.Net;

namespace Niles.Test.MockWebRequest
{
    public class MockWebRequest : WebRequest
    {
        private readonly Uri requestUri;
        private readonly WebResponse response;
        private readonly MemoryStream requestStream = new MemoryStream();
        
        public override string Method { get; set; }
        public override string ContentType { get; set; }
        public override long ContentLength { get; set; }
        public override Uri RequestUri
        {
            get { return requestUri; }
        }
        
        public MockWebRequest(Uri requestUri, WebResponse response)
        {
            this.requestUri = requestUri;
            this.response = response;
        }

        public override Stream GetRequestStream()
        {
            return requestStream;
        }

        public override WebResponse GetResponse()
        {
            if(response == null)
                throw new WebException("Mock URI not setup: " + requestUri);
            return response;
        }
    }
}
