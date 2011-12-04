using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Niles.Monitor
{
    /// <summary>
    /// Stores job state in memory, without persisting it to disk.
    /// Simple, fast, and good enough for most monitoring uses.
    /// </summary>
    public class MemoryJobStateStore : IJobStateStore
    {
        private ConcurrentDictionary<Uri, JobState> dictionary
            = new ConcurrentDictionary<Uri, JobState>(); 

        public JobState Load(Uri url)
        {
            JobState value;
            dictionary.TryGetValue(url, out value);
            return value;
        }

        public void Store(JobState state)
        {
            dictionary.AddOrUpdate(state.Url, u => state, (u, s) => state);
        }
    }
}
