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
using System.Text.RegularExpressions;
using Niles.Model;

namespace Niles.Monitor
{
    public enum JobStatus
    {
        Unknown = 0,
        Pending,
        Stable,
        Failed,
        Unstable,
        Aborted,
        Disabled
    }

    public class JobState
    {
        public Uri Url { get; set; }
        public JobStatus Status { get; set; }
        public bool IsRunning { get; set; }

        private const string ColorPattern = "(red|yellow|blue|disabled|aborted|grey)(_anime)?";
        private static readonly Regex ColorRegex = new Regex(ColorPattern);

        private static void ParseColor(string color, out JobStatus status, out bool isRunning)
        {
            var match = ColorRegex.Match(color);
            if(!match.Success)
            {
                status = JobStatus.Unknown;
                isRunning = false;
                return;
            }

            isRunning = !string.IsNullOrEmpty(match.Groups[2].Value);

            switch(match.Groups[1].Value)
            {
                case "red":
                    status = JobStatus.Failed;
                    return;
                case "blue":
                    status = JobStatus.Stable;
                    return;
                case "disabled":
                    status = JobStatus.Disabled;
                    return;
                case "aborted":
                    status = JobStatus.Aborted;
                    return;
                case "yellow":
                    status = JobStatus.Unstable;
                    return;
                case "grey":
                    status = JobStatus.Pending;
                    return;
                default:
                    status = JobStatus.Unknown;
                    return;
            }
        }

        public static JobState CreateFromJob(Job job)
        {
            JobStatus status;
            bool isRunning;
            ParseColor(job.Color, out status, out isRunning);
            return new JobState {
                Url = job.Url,
                IsRunning = isRunning,
                Status = status
            };
        }
    }
}
