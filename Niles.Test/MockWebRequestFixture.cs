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
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Niles.Test.MockWebRequest;

namespace Niles.Test
{
    public class MockWebRequestFixture
    {
        protected static readonly string TestFilesPrefix = "Niles.Test.TestFiles.";
        protected static MockWebRequestFactory WebRequestFactory { get; private set; }

        static MockWebRequestFixture()
        {
            WebRequestFactory = new MockWebRequestFactory("mock");
        }

        protected void Expect(string resourceName, string relativeUri, string query = "")
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullResourceName = TestFilesPrefix + resourceName;

            var fullUrl = "mock://test/" + relativeUri + "/api/json";
            if(!string.IsNullOrWhiteSpace(query))
                fullUrl += query;

            using(var stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                WebRequestFactory.Expect(new Uri(fullUrl), stream);
            }
        }
    }
}
