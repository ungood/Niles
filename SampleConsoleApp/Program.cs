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
using System.Threading;
using Niles.Monitor;

namespace SampleConsoleApp
{
    class Program
    {
        public static readonly Uri BaseUri = new Uri("http://localhost:8080/");

        static void Main(string[] args)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            var monitor = new JenkinsMonitor(BaseUri);

            monitor.PollingError += HandlePollingError;
            monitor.BuildStarted += (sender, e) => Log("Build Started", e);
            monitor.BuildFinished += (sender, e) => Log("Build Finished", e);
            monitor.BuildFailed += (sender, e) => Log("Build Failed", e);
            monitor.BuildSucceeded += (sender, e) => Log("Build Succeeded", e);
            monitor.FoundJob += HandleFoundJob;

            
           while(true)
           {
               monitor.Poll();
               Thread.Sleep(10000);
           }
        }

        private static void Log(string eventName, BuildEventArgs e)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(eventName + ":" + e.Job + " " + e.Build + " " + e.StatusChanged);
        }

        private static void HandleFoundJob(object sender, JobFoundEventArgs e)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Found new job: " + e.Job);
        }

        static void HandlePollingError(object sender, PollingErrorEventArgs e)
        {
            Console.WriteLine("Polling Error: " + e.Exception.Message);
        }
    }
}
