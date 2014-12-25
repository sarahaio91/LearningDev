using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

namespace WorkflowActivities
{
    public class WorkflowHostHelper
    {
        AutoResetEvent syncEvent = new AutoResetEvent(false);
        WorkflowApplication wfApp;
        bool _isCompleted = false;
        public SqlWorkflowInstanceStore store
        {
            get; set;
        }
        string InstanceStoreConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["InstanceStoreConnectionString"];
            }
        }

        public WorkflowHostHelper()
        {
            wfApp = new WorkflowApplication(new Activity1());
            store = new SqlWorkflowInstanceStore(InstanceStoreConnectionString);
            wfApp.InstanceStore = store;
        }

        public Guid StartWizard(Dictionary<string, object> inputs)
        {
            wfApp = new WorkflowApplication(new Activity1(), inputs);
            wfApp.InstanceStore = store;

            wfApp.PersistableIdle = (e) =>
            {
                return PersistableIdleAction.Unload;
            };
            wfApp.Completed = (e) =>
            {
                _isCompleted = true;
                syncEvent.Set();
            };
            wfApp.Idle = (e) =>
            {
                syncEvent.Set();
            };
            wfApp.Run();
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
            wfApp.Load(id);
            string bookmarkName = "final";
            System.Collections.ObjectModel.ReadOnlyCollection<System.Activities.Hosting.BookmarkInfo> bookmarks = wfApp.GetBookmarks();
            if (bookmarks != null && bookmarks.Count > 0)
            {
                bookmarkName = bookmarks[0].BookmarkName;
            }

            return bookmarkName;
        }

        public string RunWorkflow(string Command)
        {
            string bookmarkName = wfApp.GetBookmarks()[0].BookmarkName;
            wfApp.ResumeBookmark(bookmarkName, Command);
            syncEvent.WaitOne();

            if (!_isCompleted)
            {
                bookmarkName = wfApp.GetBookmarks()[0].BookmarkName;
                return bookmarkName;
            }
            else
            {
                return "final";
            }
        }
        public List<Guid> ListPersitedWorkflow()
        {
            
            using (SqlConnection localCon = new SqlConnection(InstanceStoreConnectionString))
            {
                
                string localCmd =
                    "Select [InstanceId] from [System.Activities.DurableInstancing].[Instances] Order By [CreationTime]";

                SqlCommand cmd = localCon.CreateCommand();
                cmd.CommandText = localCmd;
                localCon.Open();
                
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    List<Guid> temp = new List<Guid>();
                    while (reader.Read())
                    {
                        // Get the InstanceId of the persisted Workflow
                        Guid id = Guid.Parse(reader[0].ToString());
                        temp.Add(id);
                    }
                    return temp;
                }
                
            } 
        }
        public void resumeWorkflow(Guid id)
        {
            wfApp.Load(id);

            string bookmarkName = "final";
            var bookmarks = wfApp.GetBookmarks();
            if (bookmarks != null && bookmarks.Count > 0)
            {
                bookmarkName = bookmarks[0].BookmarkName;
            }
        }
    }
}
