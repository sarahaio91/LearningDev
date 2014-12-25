//namespace WindowsFormsApplication1
//{
//    partial class WorkflowHost
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.WorkflowVersion = new System.Windows.Forms.Label();
//            this.groupBox1 = new System.Windows.Forms.GroupBox();
//            this.comboBox3 = new System.Windows.Forms.ComboBox();
//            this.comboBox2 = new System.Windows.Forms.ComboBox();
//            this.label2 = new System.Windows.Forms.Label();
//            this.label1 = new System.Windows.Forms.Label();
//            this.comboBox1 = new System.Windows.Forms.ComboBox();
//            this.WorkflowStatus = new System.Windows.Forms.TextBox();
//            this.QuitGame = new System.Windows.Forms.Button();
//            this.EnterGuess = new System.Windows.Forms.Button();
//            this.label4 = new System.Windows.Forms.Label();
//            this.InstanceId = new System.Windows.Forms.ComboBox();
//            this.label3 = new System.Windows.Forms.Label();
//            this.label5 = new System.Windows.Forms.Label();
//            this.button1 = new System.Windows.Forms.Button();
//            this.groupBox1.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // WorkflowVersion
//            // 
//            this.WorkflowVersion.AutoSize = true;
//            this.WorkflowVersion.Location = new System.Drawing.Point(13, 362);
//            this.WorkflowVersion.Name = "WorkflowVersion";
//            this.WorkflowVersion.Size = new System.Drawing.Size(89, 13);
//            this.WorkflowVersion.TabIndex = 5;
//            this.WorkflowVersion.Text = "Workflow version";
//            // 
//            // groupBox1
//            // 
//            this.groupBox1.Controls.Add(this.comboBox3);
//            this.groupBox1.Controls.Add(this.comboBox2);
//            this.groupBox1.Controls.Add(this.label2);
//            this.groupBox1.Controls.Add(this.label1);
//            this.groupBox1.Controls.Add(this.comboBox1);
//            this.groupBox1.Controls.Add(this.WorkflowStatus);
//            this.groupBox1.Controls.Add(this.QuitGame);
//            this.groupBox1.Controls.Add(this.EnterGuess);
//            this.groupBox1.Controls.Add(this.label4);
//            this.groupBox1.Controls.Add(this.InstanceId);
//            this.groupBox1.Controls.Add(this.label3);
//            this.groupBox1.Location = new System.Drawing.Point(13, 62);
//            this.groupBox1.Name = "groupBox1";
//            this.groupBox1.Size = new System.Drawing.Size(358, 292);
//            this.groupBox1.TabIndex = 6;
//            this.groupBox1.TabStop = false;
//            this.groupBox1.Text = "Selection";
//            // 
//            // comboBox3
//            // 
//            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.comboBox3.FormattingEnabled = true;
//            this.comboBox3.Items.AddRange(new object[] {
//            "On",
//            "Off"});
//            this.comboBox3.Location = new System.Drawing.Point(80, 117);
//            this.comboBox3.Name = "comboBox3";
//            this.comboBox3.Size = new System.Drawing.Size(65, 21);
//            this.comboBox3.TabIndex = 11;
//            // 
//            // comboBox2
//            // 
//            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.comboBox2.FormattingEnabled = true;
//            this.comboBox2.Items.AddRange(new object[] {
//            "On",
//            "Off"});
//            this.comboBox2.Location = new System.Drawing.Point(80, 86);
//            this.comboBox2.Name = "comboBox2";
//            this.comboBox2.Size = new System.Drawing.Size(65, 21);
//            this.comboBox2.TabIndex = 10;
//            // 
//            // label2
//            // 
//            this.label2.AutoSize = true;
//            this.label2.Location = new System.Drawing.Point(11, 120);
//            this.label2.Name = "label2";
//            this.label2.Size = new System.Drawing.Size(35, 13);
//            this.label2.TabIndex = 9;
//            this.label2.Text = "About";
//            // 
//            // label1
//            // 
//            this.label1.AutoSize = true;
//            this.label1.Location = new System.Drawing.Point(11, 86);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(44, 13);
//            this.label1.TabIndex = 8;
//            this.label1.Text = "Contact";
//            // 
//            // comboBox1
//            // 
//            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.comboBox1.FormattingEnabled = true;
//            this.comboBox1.Items.AddRange(new object[] {
//            "On",
//            "Off"});
//            this.comboBox1.Location = new System.Drawing.Point(80, 56);
//            this.comboBox1.Name = "comboBox1";
//            this.comboBox1.Size = new System.Drawing.Size(65, 21);
//            this.comboBox1.TabIndex = 7;
//            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
//            // 
//            // WorkflowStatus
//            // 
//            this.WorkflowStatus.Location = new System.Drawing.Point(10, 182);
//            this.WorkflowStatus.Multiline = true;
//            this.WorkflowStatus.Name = "WorkflowStatus";
//            this.WorkflowStatus.ReadOnly = true;
//            this.WorkflowStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
//            this.WorkflowStatus.Size = new System.Drawing.Size(338, 104);
//            this.WorkflowStatus.TabIndex = 6;
//            // 
//            // QuitGame
//            // 
//            this.QuitGame.Location = new System.Drawing.Point(273, 153);
//            this.QuitGame.Name = "QuitGame";
//            this.QuitGame.Size = new System.Drawing.Size(75, 23);
//            this.QuitGame.TabIndex = 5;
//            this.QuitGame.Text = "Quit";
//            this.QuitGame.UseVisualStyleBackColor = true;
//            // 
//            // EnterGuess
//            // 
//            this.EnterGuess.Location = new System.Drawing.Point(179, 153);
//            this.EnterGuess.Name = "EnterGuess";
//            this.EnterGuess.Size = new System.Drawing.Size(75, 23);
//            this.EnterGuess.TabIndex = 4;
//            this.EnterGuess.Text = "Confirm";
//            this.EnterGuess.UseVisualStyleBackColor = true;
//            // 
//            // label4
//            // 
//            this.label4.AutoSize = true;
//            this.label4.Location = new System.Drawing.Point(11, 59);
//            this.label4.Name = "label4";
//            this.label4.Size = new System.Drawing.Size(33, 13);
//            this.label4.TabIndex = 2;
//            this.label4.Text = "Index";
//            // 
//            // InstanceId
//            // 
//            this.InstanceId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.InstanceId.FormattingEnabled = true;
//            this.InstanceId.Location = new System.Drawing.Point(121, 24);
//            this.InstanceId.Name = "InstanceId";
//            this.InstanceId.Size = new System.Drawing.Size(227, 21);
//            this.InstanceId.TabIndex = 1;
//            this.InstanceId.SelectedIndexChanged += new System.EventHandler(this.InstanceId_SelectedIndexChanged);
//            // 
//            // label3
//            // 
//            this.label3.AutoSize = true;
//            this.label3.Location = new System.Drawing.Point(11, 27);
//            this.label3.Name = "label3";
//            this.label3.Size = new System.Drawing.Size(48, 13);
//            this.label3.TabIndex = 0;
//            this.label3.Text = "Instance";
//            this.label3.Click += new System.EventHandler(this.label3_Click);
//            // 
//            // label5
//            // 
//            this.label5.AutoSize = true;
//            this.label5.Location = new System.Drawing.Point(24, 26);
//            this.label5.Name = "label5";
//            this.label5.Size = new System.Drawing.Size(73, 13);
//            this.label5.TabIndex = 8;
//            this.label5.Text = "New Instance";
//            this.label5.Click += new System.EventHandler(this.label5_Click);
//            // 
//            // button1
//            // 
//            this.button1.Location = new System.Drawing.Point(286, 21);
//            this.button1.Name = "button1";
//            this.button1.Size = new System.Drawing.Size(75, 23);
//            this.button1.TabIndex = 12;
//            this.button1.Text = "New";
//            this.button1.UseVisualStyleBackColor = true;
//            this.button1.Click += new System.EventHandler(this.button1_Click);
//            // 
//            // WorkflowHost
//            // 
//            this.AcceptButton = this.EnterGuess;
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(384, 382);
//            this.Controls.Add(this.button1);
//            this.Controls.Add(this.label5);
//            this.Controls.Add(this.groupBox1);
//            this.Controls.Add(this.WorkflowVersion);
//            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
//            this.MaximizeBox = false;
//            this.Name = "WorkflowHost";
//            this.Text = "WorkflowHostForm";
//            this.groupBox1.ResumeLayout(false);
//            this.groupBox1.PerformLayout();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private System.Windows.Forms.Label WorkflowVersion;
//        private System.Windows.Forms.GroupBox groupBox1;
//        private System.Windows.Forms.TextBox WorkflowStatus;
//        private System.Windows.Forms.Button QuitGame;
//        private System.Windows.Forms.Button EnterGuess;
//        private System.Windows.Forms.Label label4;
//        private System.Windows.Forms.ComboBox comboBox1;
//        private System.Windows.Forms.ComboBox InstanceId;
//        private System.Windows.Forms.Label label3;
//        private System.Windows.Forms.Label label2;
//        private System.Windows.Forms.Label label1;
//        private System.Windows.Forms.ComboBox comboBox3;
//        private System.Windows.Forms.ComboBox comboBox2;
//        private System.Windows.Forms.Label label5;
//        private System.Windows.Forms.Button button1;
//    }
//}

