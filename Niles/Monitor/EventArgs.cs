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
using Niles.Client;
using Niles.Model;

namespace Niles.Monitor
{
    public class PollingErrorEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public PollingErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }

    public class JobFoundEventArgs : EventArgs
    {
        public Job Job { get; private set; }
        
        public JobFoundEventArgs(Job job)
        {
            Job = job;
        }
    }

    public class BuildEventArgs : EventArgs
    {
        public Job Job { get; private set; }
        public Build Build { get; private set; }
        public bool StatusChanged { get; private set; }

        public BuildEventArgs(Job job, Build build, bool isFirstTime = false)
        {
            Build = build;
            Job = job;
            StatusChanged = isFirstTime;
        }
    }
}
