using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;
using Niles.Model;

namespace Niles
{
    public class JenkinsClient
    {
        private readonly Uri baseUri;
        private readonly ILog log;

        public JenkinsClient(Uri baseUri, ILog log)
        {
            this.baseUri = baseUri;
            this.log = log ?? new NoOpLogger();
        }

        public JenkinsClient(Uri baseUri) : this(baseUri, null)
        {}

        private T ReadObject<T>(Uri baseUri)
        {
            var uri = new Uri(baseUri, "/api/json");
            log.Debug("Polling Jenkins at " + uri);

            var request = WebRequest.Create(uri);
            try
            {
                var response = request.GetResponse();

                using(var stream = response.GetResponseStream())
                {
                    var serializer = new DataContractJsonSerializer(typeof (T));
                    return (T)serializer.ReadObject(stream);
                }
            }
            catch (WebException ex)
            {
                log.Warn("Could not access " + uri, ex);
                return default(T);
            }
        }

        public Node GetRoot()
        {
            return ReadObject<Node>(baseUri);
        }
    }
}
