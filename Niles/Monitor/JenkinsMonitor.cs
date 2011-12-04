//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Niles.Client;
//using Niles.Model;

//namespace Niles.Monitor
//{
//    /// <summary>
//    /// Polls a jenkins node and raises events when jobs or builds change states.
//    /// </summary>
//    public class JenkinsMonitor
//    {
//        private readonly SynchronizationContext eventContext;
//        private readonly IJenkinsClient client;
//        private readonly IJobStateStore store;
//        private readonly TaskScheduler scheduler;

//        private readonly object pollLock = new object();
        
//        public Uri BaseUri { get; private set; }

//        public JenkinsMonitor(
//            IJobStateStore store = null,
//            IJenkinsClient client = null,
//            TaskScheduler scheduler = null,
//            SynchronizationContext eventContext = null)
//        {
//            this.store = store ?? new MemoryJobStateStore();
//            this.client = client ?? new JsonJenkinsClient();
//            this.scheduler = scheduler ?? TaskScheduler.Default;
//            this.eventContext = eventContext ?? SynchronizationContext.Current;

//            //async
//        }

//        public void Configure(IMonitorConfig config)
//        {
//            if(config == null)
//                throw new ArgumentNullException("config");
//            if(config.BaseUri == null)
//                throw new InvalidOperationException("Configure must include a valid URI to poll.");

//            BaseUri = config.BaseUri;
//        }

//        #region Events

        
//        // Events I care about:
//        // * Polling error
//        // * New job
//        // * Build started
//        // * Build finished
//        // *   Previous State
//        // *   Latest state

//        public event EventHandler<PollingErrorEventArgs> PollingError;
//        protected virtual void OnPollingError(PollingErrorEventArgs e)
//        {
//            eventContext.Post(state => {
//                var handlers = PollingError;
//                if (handlers != null)
//                    handlers(this, e);
//            }, null);
//        }

//        public event EventHandler<JobEventArgs> FoundJob;
//        protected virtual void OnFoundJob(JobEventArgs e)
//        {
//            eventContext.Post(state => {
//                var handlers = FoundJob;
//                if (handlers != null)
//                    handlers(this, e);
//            }, null);
//        }
 

//        #endregion

//        private static string CreateBuildTree(string buildId)
//        {
//            return buildId + "[number,url]";
//        }

//        private static string CreateJobTree()
//        {
//            return "jobs[name,displayName,url,color]";
//            // + CreateBuildTree("lastBuild") + "]";
//            //+ CreateBuildTree("lastCompletedBuild")
//            //+ CreateBuildTree("lastFailedBuild")
//            //+ CreateBuildTree("lastStableBuild")
//            //+ CreateBuildTree("lastSuccessfulBuild")
//            //+ CreateBuildTree("lastUnstableBuild")
//            //+ CreateBuildTree("lastUnsuccessfulBuild");
//        }

//        private static readonly string TreeParameter = CreateJobTree();
        
//        private Task StartTask(Action action, bool isLongRunning = false)
//        {
//            var creationOptions = isLongRunning ? TaskCreationOptions.LongRunning : TaskCreationOptions.None;
//            var task = new Task(action, creationOptions);
//            task.Start(scheduler);
//            return task;
//        }

//        /// <summary>
//        /// Polls the monitored node, and fires events based on changes in job state.
//        /// </summary>
//        public Task Poll()
//        {
//            if(BaseUri == null)
//                throw new InvalidOperationException("Cannot poll an unconfigured monitor.");

//            return StartTask(PollTask, true);
//        }

//        private void PollTask()
//        {
//            try
//            {
//                // Only one polling task can be executing at a time
//                // If another polling task is started, just exit.
//                if(!System.Threading.Monitor.TryEnter(pollLock))
//                    return;
                
//                var node = client.GetResourceAsync<Node>(BaseUri, TreeParameter);

//                foreach(var job in node.Jobs)
//                    UpdateJob(job);
//            }
//            catch (ClientException ex)
//            {
//                OnPollingError(new PollingErrorEventArgs(ex));
//            }
//            finally
//            {
//                System.Threading.Monitor.Exit(pollLock);
//            }
//        }

//        private void UpdateJob(Job job)
//        {
//            var previousState = store.Load(job.Url);
//            var currentState = JobState.CreateFromJob(job);

//            if(previousState == null)
//            {
//                job = client.ExpandAsync(job);
//                OnFoundJob(new JobEventArgs(job));
//                store.Store(currentState);
//                return;
//            }

//            store.Store(currentState);
//        }
//    }
//}
