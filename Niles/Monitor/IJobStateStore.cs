using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Niles.Monitor
{
    public interface IJobStateStore
    {
        JobState Load(Uri url);
        void Store(JobState state);
    }
}
