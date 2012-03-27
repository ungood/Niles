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
using System.Reflection;
using System.Threading.Tasks;
using Niles.Model;

namespace Niles.Client
{
    public abstract class JenkinsClientBase : IJenkinsClient
    {
        protected abstract string ApiSuffix { get; }
        protected abstract T GetResourceInternal<T>(Uri absoluteUri);

        protected JenkinsClientBase()
        {
            WorkaroundTrailingPeriodBug();
        }

        /// <summary>
        /// Retrieves a jenkins resource given it's URI and optional tree parameter.
        /// </summary>
        /// <param name="resourceUri">The absolute URI of that resource (not including the api suffix).</param>
        /// <param name="tree">
        /// A tree parameter, which will filter what properties are selected. See the Jenkins API documentation for details.
        /// </param>
        public T GetResource<T>(Uri resourceUri, string tree = null)
        {
            var absoluteUri = GetAbsoluteUri(resourceUri, tree);
            return GetResourceInternal<T>(absoluteUri);
        }

        /// <summary>
        /// Retrieves a jenkins resource given it's URI and optional tree parameter.
        /// </summary>
        /// <param name="resourceUri">The absolute URI of that resource (not including the api suffix).</param>
        /// <param name="depth">
        /// The number of levels to select.  See the Jenkins API documentation for details.
        /// </param>
        public T GetResource<T>(Uri resourceUri, int depth)
        {
            var absoluteUri = GetAbsoluteUri(resourceUri, depth);
            return GetResourceInternal<T>(absoluteUri);
        }

        /// <summary>
        /// Expands a partially retrieved resource, with an optional tree parameter.
        /// </summary>
        /// <remarks>
        /// It's important to notice this method returns a new instance of the resource, and doesn't
        /// change the passed in instance.
        /// </remarks>
        /// <param name="resource">A previously retrieved instance of this resource.</param>
        /// <param name="tree">
        /// A tree parameter, which will filter what properties are selected. See the Jenkins API documentation for details.
        /// </param>
        public T Expand<T>(T resource, string tree = null) where T : class, IResource
        {
            if(resource == null)
                return null;
            return GetResource<T>(resource.Url, tree);
        }

        /// <summary>
        /// Expands a partially retrieved resource, with an optional tree parameter.
        /// </summary>
        /// <remarks>
        /// It's important to notice this method returns a new instance of the resource, and doesn't
        /// change the passed in instance.
        /// </remarks>
        /// <param name="resource">A previously retrieved instance of this resource.</param>
        /// <param name="depth">
        /// The number of levels to select.  See the Jenkins API documentation for details.
        /// </param>
        public T Expand<T>(T resource, int depth) where T : class, IResource
        {
            if(resource == null)
                return null;
            return GetResource<T>(resource.Url, depth);
        }

        private Uri GetAbsoluteUri(Uri resourceUri, string tree)
        {
            var relativeUri = ApiSuffix;
            if(!string.IsNullOrWhiteSpace(tree))
                relativeUri += "?tree=" + tree;
            
            var absoluteUri = new Uri(resourceUri, relativeUri);
            return absoluteUri;
        }

        private Uri GetAbsoluteUri(Uri resourceUri, int depth)
        {
            var relativeUri = ApiSuffix;
            if(depth > 0)
                relativeUri += "?depth=" + depth;

            var absoluteUri = new Uri(resourceUri, relativeUri);
            return absoluteUri;
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
