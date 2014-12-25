using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Activities.DurableInstancing;
using System.Activities;
using System.Data.SqlClient;
using System.IO;
using WorkflowActivities;

namespace WindowsFormsApplication1
{
    public partial class WorkflowHost : Form
    {
        WorkflowHostHelper host;
        List<Guid> listPersisted;
        public Guid WorkflowInstanceId
        {
            get
            {
                return InstanceId.SelectedIndex == -1 ? Guid.Empty : (Guid)InstanceId.SelectedItem;
            }
        }

        public WorkflowHost()
        {
            InitializeComponent();
            host = new WorkflowHostHelper();
            listPersisted = host.ListPersitedWorkflow();
            foreach (var temp in listPersisted)
            {
                InstanceId.Items.Add(temp);
            }
        }

        private void NewGame_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            host = new WorkflowHostHelper();
            Guid id = host.StartWizard();
        }

        private void InstanceId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InstanceId.SelectedIndex == -1)
            {
                return;
            }

            // Clear the status window.
            WorkflowStatus.Clear();

            // Get the workflow version and display it.
            // If the workflow is just starting then this info will not
            // be available in the persistence store so do not try and retrieve it.
            if (!host.workflowStarting)
            {
                WorkflowApplicationInstance instance = WorkflowApplication.GetInstance(this.WorkflowInstanceId, host.store);

                //WorkflowVersion.Text = WorkflowVersionMap.GetIdentityDescription(instance.DefinitionIdentity);

                // Unload the instance.
                instance.Abandon();
            }
        }
    }
}
