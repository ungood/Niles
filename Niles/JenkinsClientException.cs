using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Niles
{
    [Serializable]
    public class JenkinsClientException : Exception
    {
        public JenkinsClientException()
        {
        }

        public JenkinsClientException(string message) : base(message)
        {
        }

        public JenkinsClientException(string message, Exception inner) : base(message, inner)
        {
        }

        protected JenkinsClientException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
