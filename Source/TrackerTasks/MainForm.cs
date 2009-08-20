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
            Projects.SelectedValueChanged -= Projects_SelectedValueChanged;
            Projects.DataSource = tracker.GetProjects().Projects;
            Projects.DisplayMember = "Name";
            Projects.ValueMember = "Id";
            Projects.SelectedValueChanged += Projects_SelectedValueChanged;
        }

        private void Projects_SelectedValueChanged(object sender, EventArgs e)
        {
            var tracker = new TrackerApi(ApiToken.Text);
            Stories.SelectedValueChanged -= Stories_SelectedValueChanged;
            Stories.DataSource = tracker.GetStories((int)Projects.SelectedValue).Items;
            Stories.DisplayMember = "Name";
            Stories.ValueMember = "Id";
            Stories.SelectedValueChanged += Stories_SelectedValueChanged;
        }

        private void Stories_SelectedValueChanged(object sender, EventArgs e)
        {
            var tracker = new TrackerApi(ApiToken.Text);
            Tasks.DataSource = tracker.GetTasks((int)Projects.SelectedValue, (int)Stories.SelectedValue).Tasks;
            Tasks.AutoGenerateColumns = true;
        }
    }
}
