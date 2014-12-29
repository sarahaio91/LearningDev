using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkflowActivities;

namespace WebApplication1.Models
{
    public class WorkflowModel
    {
        public interface IWorkflowService
        {
            Guid StartWorkflow();
            string ResumeWorkflow(Guid id);
            string Next();
            string Back();
            string Current();
            void Unload();
        }

        public class WorkflowService : IWorkflowService
        {
            WorkflowHostHelper host;

            public WorkflowService()
            {
                host = new WorkflowHostHelper();
            }

            public Guid StartWorkflow()
            {
                return host.StartWizard();
            }

            public string ResumeWorkflow(Guid id)
            {
                string bookmarkName = "final";
                try
                {
                    bookmarkName = host.ResumeWizard(id);
                }
                catch (Exception ex)
                {
                    // TODO get complete exception
                }
                return bookmarkName;
            }

            public string Next()
            {
                return host.RunWorkflow("Next");
            }

            public string Back()
            {
                return host.RunWorkflow("Back");
            }
            public string Current()
            {
                return host.CurrentStep();
            }

            public void Unload()
            {
                try
                {
                    host.Unload();
                }
                catch
                {
                    // TODO get complete exception
                }
            }
        }
    }
}