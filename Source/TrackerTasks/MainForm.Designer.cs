namespace TrackerTasks
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.ApiToken = new System.Windows.Forms.TextBox();
            this.Projects = new System.Windows.Forms.ComboBox();
            this.Stories = new System.Windows.Forms.ComboBox();
            this.Tasks = new System.Windows.Forms.DataGridView();
            this.NewToken = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Tasks)).BeginInit();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tracker API Token";
            //
            // ApiToken
            //
            this.ApiToken.Location = new System.Drawing.Point(13, 30);
            this.ApiToken.Name = "ApiToken";
            this.ApiToken.Size = new System.Drawing.Size(224, 20);
            this.ApiToken.TabIndex = 1;
            //
            // Projects
            //
            this.Projects.FormattingEnabled = true;
            this.Projects.Location = new System.Drawing.Point(13, 57);
            this.Projects.Name = "Projects";
            this.Projects.Size = new System.Drawing.Size(267, 21);
            this.Projects.TabIndex = 2;
            //
            // Stories
            //
            this.Stories.FormattingEnabled = true;
            this.Stories.Location = new System.Drawing.Point(16, 85);
            this.Stories.Name = "Stories";
            this.Stories.Size = new System.Drawing.Size(264, 21);
            this.Stories.TabIndex = 3;
            //
            // Tasks
            //
            this.Tasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tasks.Location = new System.Drawing.Point(16, 112);
            this.Tasks.Name = "Tasks";
            this.Tasks.Size = new System.Drawing.Size(264, 267);
            this.Tasks.TabIndex = 4;
            //
            // NewToken
            //
            this.NewToken.Location = new System.Drawing.Point(243, 30);
            this.NewToken.Name = "NewToken";
            this.NewToken.Size = new System.Drawing.Size(36, 23);
            this.NewToken.TabIndex = 5;
            this.NewToken.Text = ">>";
            this.NewToken.UseVisualStyleBackColor = true;
            this.NewToken.Click += new System.EventHandler(this.NewToken_Click);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 388);
            this.Controls.Add(this.NewToken);
            this.Controls.Add(this.Tasks);
            this.Controls.Add(this.Stories);
            this.Controls.Add(this.Projects);
            this.Controls.Add(this.ApiToken);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "TrackerTasks";
            ((System.ComponentModel.ISupportInitialize)(this.Tasks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ApiToken;
        private System.Windows.Forms.ComboBox Projects;
        private System.Windows.Forms.ComboBox Stories;
        private System.Windows.Forms.DataGridView Tasks;
        private System.Windows.Forms.Button NewToken;
    }
}

