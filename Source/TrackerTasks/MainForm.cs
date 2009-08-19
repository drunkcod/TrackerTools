using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using TrackerTools;

namespace TrackerTasks
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void NewToken_Click(object sender, EventArgs e)
        {
            var tracker = new TrackerApi(ApiToken.Text);
            Projects.DataSource = tracker.GetProjects().Projects;
            Projects.DisplayMember = "Name";
            Projects.ValueMember = "Id";
            Projects.SelectedValueChanged += Projects_SelectedValueChanged;
        }

        private void Projects_SelectedValueChanged(object sender, EventArgs e)
        {
            var tracker = new TrackerApi(ApiToken.Text);
            Stories.DataSource = tracker.GetStories((int)Projects.SelectedValue).Stories;
            Stories.DisplayMember = "Name";
            Stories.ValueMember = "Id";
        }
    }
}
