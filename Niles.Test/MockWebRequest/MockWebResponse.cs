using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Niles.Test.MockWebRequest
{
    public class MockWebResponse : WebResponse
    {
        private readonly MemoryStream responseStream;

        public MockWebResponse(string response)
        {
            responseStream = new MemoryStream(Encoding.UTF8.GetBytes(response));
        }

        public override Stream GetResponseStream()
        {
            return responseStream;
        }
    }
}
