using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Niles.Client;

namespace Niles.Monitor
{
    /// <summary>
    /// Polls a jenkins node and raises events when jobs or builds change states.
    /// </summary>
    public class JenkinsMonitor
    {
        public JenkinsMonitor(IJenkinsClient client, IJobStateStore store) 
        {
            
        }

        public event EventHandler<PollingErrorEventArgs> OnPollingError;
        // Events I care about:
        // * Polling error
        // * New job
        // * Build started
        // * Build finished
        // *   Previous State
        // *   Latest state
    }
}
