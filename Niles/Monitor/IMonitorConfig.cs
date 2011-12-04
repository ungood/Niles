using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Niles.Model;

namespace Niles.Monitor
{
    public interface IMonitorConfig
    {
        Uri BaseUri { get; }
        IEnumerable<IFilterConfig> Include { get; }
        IEnumerable<IFilterConfig> Exclude { get; } 
    }

    public interface IFilterConfig
    {
        string Pattern { get; }
    }
}
