using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowActivities;

namespace WorkflowHostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkflowInvoker.Invoke(new Germany());

            WorkflowHostHelper wizardHostHelper = new WorkflowHostHelper();
            Guid id = wizardHostHelper.StartWizard();

            wizardHostHelper.ResumeWizard(Guid.Parse("{79951da3-dae6-4934-b663-f1405d719ef5}"));
            string bookmarkName1 = wizardHostHelper.RunWorkflow("Next");
            string bookmarkName2 = wizardHostHelper.RunWorkflow("Next");
            //string bookmarkName3 = wizardHostHelper.RunWorkflow("Next");
            //string bookmarkName4 = wizardHostHelper.RunWorkflow("Next");
        }
    }
}
