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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Niles.Json;

namespace Niles.Client
{
    /// <summary>
    /// Provides a simple, low-level client for navigating the Jenkins JSON API.
    /// </summary>
    public class JsonJenkinsClient : JenkinsClientBase
    {
        private readonly JenkinsJsonSerializer serializer;

        public JsonJenkinsClient()
        {
            serializer = new JenkinsJsonSerializer();
        }

        protected override string ApiSuffix
        {
            get { return "api/json"; }
        }

        protected async override Task<T> GetResourceInternal<T>(Uri absoluteUri)
        {
            var request = WebRequest.Create(absoluteUri);
            try
            {
                var response = await request.GetResponseAsync();
                
                using(var responseStream = response.GetResponseStream())
                {
                   return serializer.ReadObject<T>(responseStream);
                }
                
            }
            catch (WebException ex)
            {
                throw new ClientException("Could not access resource at: " + absoluteUri, ex);
            }
            catch (JsonReaderException ex)
            {
                throw new ClientException("Resource at " + absoluteUri + " is not valid JSON", ex);
            }
        }
    }
}
