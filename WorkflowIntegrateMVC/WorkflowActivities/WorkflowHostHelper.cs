using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace WorkflowActivities
{
    public class WorkflowHostHelper
    {
        WorkflowApplication wfApp;
        public AutoResetEvent syncEvent { get; set; }
        public AutoResetEvent persistEvent { get; set; }
        bool _isCompleted = false;

        string InstanceStoreConnectionString { get { return System.Configuration.ConfigurationManager.AppSettings["InstanceStoreConnectionString"]; } }
        SqlWorkflowInstanceStore store;

        public WorkflowHostHelper()
        {
            Debug.WriteLine(InstanceStoreConnectionString);
            store = new SqlWorkflowInstanceStore(InstanceStoreConnectionString);
            syncEvent = new AutoResetEvent(false);
            persistEvent = new AutoResetEvent(false);
            wfApp = new WorkflowApplication(new Germany());
            wfApp.InstanceStore = store;

        }

        public Guid StartWizard()
        {
            wfApp.PersistableIdle = (e) =>
            {
                Console.WriteLine("Persistable Idle");
                return PersistableIdleAction.Unload;
            };
            wfApp.Idle = (e) =>
            {
                Console.WriteLine("Idle");
                syncEvent.Set();
            };
            wfApp.Completed = (e) =>
            {
                Console.WriteLine("Completed workflow!");
                _isCompleted = true;
                syncEvent.Set();
            };
            wfApp.Aborted = (e) =>
            {
                Console.WriteLine("Aborted");
                Console.WriteLine(e.Reason);
                syncEvent.Set();
            };
            wfApp.OnUnhandledException = (e) =>
            {
                Console.WriteLine("Unhandled Exception");
                Console.WriteLine(e.UnhandledException.ToString());
                return UnhandledExceptionAction.Terminate;
            };
            wfApp.Run();
            syncEvent.WaitOne();
            return wfApp.Id;
        }

        public void Unload()
        {
            if (wfApp != null)
            {
                wfApp.Unload();
            }
        }

        public string ResumeWizard(Guid id)
        {
            wfApp.PersistableIdle = (e) =>
            {
                Console.WriteLine("Persistable Idle");
                return PersistableIdleAction.Persist;
            };
            wfApp.Idle = (e) =>
            {
                Console.WriteLine("Idle");
                syncEvent.Set();
            };
            wfApp.Completed = (e) =>
            {
                Console.WriteLine("Completed workflow!");
                _isCompleted = true;
                syncEvent.Set();
            };
            wfApp.Aborted = (e) =>
            {
                Console.WriteLine("Aborted");
                Console.WriteLine(e.Reason);
                syncEvent.Set();
            };
            wfApp.OnUnhandledException = (e) =>
            {
                Console.WriteLine("Unhandled Exception");
                Console.WriteLine(e.UnhandledException.ToString());
                return UnhandledExceptionAction.Terminate;
            };
            wfApp.Load(id);
            string bookmarkName = "final";
            System.Collections.ObjectModel.ReadOnlyCollection<System.Activities.Hosting.BookmarkInfo> bookmarks = wfApp.GetBookmarks();
            if (bookmarks != null && bookmarks.Count > 0)
            {
                bookmarkName = bookmarks[0].BookmarkName;
            }

            return bookmarkName;
        }

        public string CurrentStep()
        {
            return wfApp.GetBookmarks()[0].BookmarkName;
        }

        public string RunWorkflow(string Command)
        {
            string bookmarkName = wfApp.GetBookmarks()[0].BookmarkName;


            //WaitHandle[] handles = new WaitHandle[] { syncEvent, idleEvent };
            wfApp.ResumeBookmark(bookmarkName, Command);
            syncEvent.WaitOne();

            if (!_isCompleted)
            {
                var das = wfApp.GetBookmarks();
                var dfdsa = wfApp.GetBookmarks();
                bookmarkName = wfApp.GetBookmarks()[0].BookmarkName;
                return bookmarkName;
            }
            else
            {
                return "final";
            }
        }
    }
}
