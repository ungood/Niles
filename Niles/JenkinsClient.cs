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
using System.Reflection;
using Newtonsoft.Json;
using Niles.Model;

namespace Niles
{
    /// <summary>
    /// Provides a simple, low-level client for navigating the Jenkins API.
    /// </summary>
    public class JenkinsClient
    {
        public const string ApiSuffix = "api/json";

        private readonly JenkinsSerializer serializer;

        public JenkinsClient()
        {
            serializer = new JenkinsSerializer();
            WorkaroundTrailingPeriodBug();
        }

        public TResource GetResource<TResource>(Uri resourceUri, string tree = null)
        {
            var relativeUri = ApiSuffix;
            if(!string.IsNullOrWhiteSpace(tree))
                relativeUri += "?tree=" + tree;

            var absoluteUri = new Uri(resourceUri, relativeUri);
            return GetResourceInternal<TResource>(absoluteUri);
        }

        public TResource GetResource<TResource>(Uri resourceUri, int depth)
        {
            var relativeUri = ApiSuffix;
            if(depth > 0)
                relativeUri += "?depth=" + depth;

            var absoluteUri = new Uri(resourceUri, relativeUri);
            return GetResourceInternal<TResource>(absoluteUri);
        }

        public TResource Expand<TResource>(TResource resource, string tree = null)
            where TResource : IResource
        {
            return GetResource<TResource>(resource.Url, tree);
        }

        public TResource Expand<TResource>(TResource resource, int depth)
            where TResource : IResource
        {
            return GetResource<TResource>(resource.Url, depth);
        }

        private T GetResourceInternal<T>(Uri absoluteUri)
        {
            var request = WebRequest.Create(absoluteUri);
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
                throw new JenkinsClientException("Could not access resource at: " + absoluteUri, ex);
            }
            catch (JsonSerializationException ex)
            {
                throw new JenkinsClientException("Resource at " + absoluteUri + " could not be deserialized", ex);
            }
        }

        /// <summary>
        /// There is a bug in .NET versions 4.0 and lower that does not correctly 
        /// parse URIs with a trailing period in a path.  Jenkins will sometimes
        /// generate URIs with a trailing period (when a job or build name is
        /// written like a sentence, for example).  This workaround resolves
        /// the issue.
        /// 
        /// See: https://connect.microsoft.com/VisualStudio/feedback/details/386695/system-uri-incorrectly-strips-trailing-dots#tabs
        /// </summary>
        private static void WorkaroundTrailingPeriodBug()
        {
            var getSyntax = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            var flagsField = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
            if (getSyntax != null && flagsField != null)
            {
                foreach (var scheme in new[] { "http", "https" })
                {
                    var parser = (UriParser)getSyntax.Invoke(null, new object[] { scheme });
                    if (parser != null)
                    {
                        var flagsValue = (int) flagsField.GetValue(parser);
                        // Clear the CanonicalizeAsFilePath attribute
                        if ((flagsValue & 0x1000000) != 0)
                            flagsField.SetValue(parser, flagsValue & ~0x1000000);
                    }
                }
            }
        }
    }
}
