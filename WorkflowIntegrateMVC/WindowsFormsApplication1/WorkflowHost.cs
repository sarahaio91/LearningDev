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
        bool workflowStarting;
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
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
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
            var inputs = new Dictionary<string, object>();
            inputs.Add("Index", comboBox1.SelectedItem.Equals("On")?"True":"False");
            inputs.Add("Contact", comboBox2.SelectedItem.Equals("On")?"True":"False");
            inputs.Add("About", comboBox3.SelectedItem.Equals("On")?"True":"False");
            
            Guid id = host.StartWizard(inputs);
            //// Add the workflow to the list and display the version information.
            workflowStarting = true;
            InstanceId.SelectedIndex = InstanceId.Items.Add(id);
            workflowStarting = false;
            
        }

        private delegate void UpdateStatusDelegate(string msg);

        public void UpdateStatus(string msg)
        {
            // We may be on a different thread so we need to
            // make this call using BeginInvoke.
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateStatusDelegate(UpdateStatus), msg);
            }
            else
            {
                if (!msg.EndsWith("\r\n"))
                {
                    msg += "\r\n";
                }
                WorkflowStatus.AppendText(msg);

                WorkflowStatus.SelectionStart = WorkflowStatus.Text.Length;
                WorkflowStatus.ScrollToCaret();
            }
        }

        private void InstanceId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InstanceId.SelectedIndex == -1)
            {
                return;
            }

            // Clear the status window.
            WorkflowStatus.Clear();

            // If there is tracking data for this workflow, display it
            // in the status window.
            //if (File.Exists(WorkflowInstanceId.ToString()))
            //{
            //    string status = File.ReadAllText(WorkflowInstanceId.ToString());
            //    UpdateStatus(status);
            //}

            // Get the workflow version and display it.
            // If the workflow is just starting then this info will not
            // be available in the persistence store so do not try and retrieve it.
            if (!workflowStarting)
            {
                WorkflowApplicationInstance instance = WorkflowApplication.GetInstance(this.WorkflowInstanceId, host.store);
                
                //WorkflowVersion.Text = WorkflowVersionMap.GetIdentityDescription(instance.DefinitionIdentity);

                // Unload the instance.
                instance.Abandon();

                // Get bookmark
                host.resumeWorkflow(this.WorkflowInstanceId);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
