using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkflowActivities;

namespace WorkflowHostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //WorkflowInvoker.Invoke(new Germany());

            WorkflowHostHelper wizardHostHelper = new WorkflowHostHelper();
            Guid id = wizardHostHelper.StartWizard();

            //runWork("47486BAC-A3EC-4488-BC85-F5C17086158D");
        }

        static void runWork(string id)
        {
            WorkflowHostHelper wizardHostHelper = new WorkflowHostHelper();
            wizardHostHelper.ResumeWizard(Guid.Parse(id));

            string command;
            while (true)
            {
                Console.WriteLine("Enter command n/b: ");
                command = Console.ReadLine();
                wizardHostHelper.RunWorkflow(command.Equals("n") ? "Next" : "Back");
            }
        }
    }
}
