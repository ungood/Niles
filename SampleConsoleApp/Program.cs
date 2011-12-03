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
using Niles;

namespace SampleConsoleApp
{
    class Program
    {
        public static readonly Uri BaseUri = new Uri("http://ci:8081/");

        static void Main(string[] args)
        {
            WorkaroundTrailingPeriodBug();

            var client = new JenkinsClient(BaseUri);

            var node = client.GetNode();
            Console.WriteLine(node.Description);

            foreach(var jobRef in node.Jobs)
            {
                var job = client.GetResource(jobRef);
                Console.WriteLine("  Job: " + job.Name);
            }
        }

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
