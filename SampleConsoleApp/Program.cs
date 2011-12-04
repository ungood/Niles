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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Niles;
using Niles.Model;
using Niles.Monitor;

namespace SampleConsoleApp
{
    class Program
    {
        public static readonly Uri BaseUri = new Uri("http://ci:8081/");

        private class SimpleConfig : IMonitorConfig
        {
            public Uri BaseUri { get; set; }
            public IEnumerable<IFilterConfig> Include { get; set; }
            public IEnumerable<IFilterConfig> Exclude { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            //var monitor = new JenkinsMonitor();
            //var config = new SimpleConfig {
            //    BaseUri = BaseUri
            //};

            //monitor.Configure(config);
            
            //monitor.PollingError += HandlePollingError;
            //monitor.FoundJob += HandleFoundJob;

            //while(true)
            //{
            //    monitor.Poll();
            //    Thread.Sleep(5000);
            //}
        }

        private static void HandleFoundJob(object sender, JobEventArgs e)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Found new job: " + e.Job);
        }

        static void HandlePollingError(object sender, PollingErrorEventArgs e)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            throw e.Exception;
        }

        
    }
}
