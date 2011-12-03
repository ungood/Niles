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
using System.Net;
using Common.Logging;
using Common.Logging.Simple;
using Niles.Model;

namespace Niles
{
    /// <summary>
    /// Provides a simple, low-level client for navigating the Jenkins API.
    /// </summary>
    public class JenkinsClient
    {
        private readonly Uri baseUri;
        private readonly ILog log;
        private readonly JenkinsSerializer serializer;

        public JenkinsClient(Uri baseUri, ILog log)
        {
            this.baseUri = baseUri;
            this.log = log ?? new NoOpLogger();
            serializer = new JenkinsSerializer();
        }

        public JenkinsClient(Uri baseUri) : this(baseUri, null)
        {
        }

        private T ReadObject<T>(Uri resourceUri)
        {
            var uri = new Uri(resourceUri, "api/json");
            log.Debug("Polling Jenkins at " + uri);

            var request = WebRequest.Create(uri);
            try
            {
                var response = request.GetResponse();
                
                using(var responseStream = response.GetResponseStream())
                {
                    return serializer.ReadObject<T>(responseStream);
                }
                
            }
            catch (WebException ex)
            {
                log.Warn("Could not access " + uri, ex);
                throw;
            }
        }

        public Node GetNode()
        {
            return ReadObject<Node>(baseUri);
        }

        public T GetResource<T>(IReference<T> reference)
        {
            return ReadObject<T>(reference.Url);
        }
    }
}
