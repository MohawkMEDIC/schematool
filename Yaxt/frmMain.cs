using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using SchemaTool.Forms;
using System.Diagnostics;
using System.IO;
using Everest.Workshop.SchemaTool;
using Everest.Workshop.SchemaTool.Forms;

namespace SchemaTool
{
    public partial class frmMain : Form
    {

        public ActiveEnvironmentContext EnvironmentContext { get; set; }
        private frmPropertyView propertyView = new frmPropertyView();

        public frmMain()
        {
            InitializeComponent();
            
            // Create the environment context
            EnvironmentContext = new ActiveEnvironmentContext();
            EnvironmentContext.StatusChanged += new EventHandler<StatusChangedEventArgs>(ChildFormStatusChanged);

            //propertyView.MdiParent = this;
            propertyView.Context = EnvironmentContext;
            propertyView.Show(dpMain, DockState.DockRight);
        }

        private void xmlSchemaCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCopySchemas fcs = new frmCopySchemas();
            fcs.ShowDialog();
        }

        private void closeProjecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Child form status has changed
        /// </summary>
        private void ChildFormStatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (e.Progress >= 0 && e.Progress <= 100)
            {
                pgStatus.Visible = true;
                pgStatus.Style = ProgressBarStyle.Blocks;
                pgStatus.Value = e.Progress;
            }
            else if (e.Progress < 0)
                pgStatus.Visible = false;
            else if (e.Progress == Int32.MaxValue)
            {
                pgStatus.Visible = true;
                pgStatus.Style = ProgressBarStyle.Marquee;
            }

            txtStatus.Text = e.Text ?? "Idle";
            Application.DoEvents();
        }

        /// <summary>
        /// Show a child form
        /// </summary>
        /// <param name="child"></param>
        private void ShowChildMDI(DockContent child)
        {
            child.MdiParent = this;

            // Subscribe to events
            if (child is IEnvironmentChild)
                SubscribeEvents(child as IEnvironmentChild);

            child.Show(dpMain, DockState.Document);
        }

        /// <summary>
        /// Subscribe events
        /// </summary>
        private void SubscribeEvents(IEnvironmentChild child)
        {
            child.Context = this.EnvironmentContext;
        }

        private void xmlSchemaDiffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSchemaDiff scd = new frmSchemaDiff();
            scd.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(delegate(object snd, SelectionChangedEventArgs ev)
            {
                EnvironmentContext.ActiveObject = ev.SelectedObject;
            });
            scd.StatusChanged += new EventHandler<StatusChangedEventArgs>(scd_StatusChanged);
            ShowChildMDI(scd);
            scd.Compare();
        }

        /// <summary>
        /// Status has changed
        /// </summary>
        void scd_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            txtStatus.Text = e.Text;
            pgStatus.Visible = e.Progress != 0;

            if (e.Progress < 0 && pgStatus.Style != ProgressBarStyle.Marquee)
                pgStatus.Style = ProgressBarStyle.Marquee;
            else if (e.Progress >= 0)
            {
                pgStatus.Style = ProgressBarStyle.Continuous;
                pgStatus.Value = e.Progress;
            }
            Application.DoEvents();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new frmAbout()).ShowDialog();
        }

        private void xmlSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                frmSchemaView scv = new frmSchemaView();
                scv.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(delegate(object snd, SelectionChangedEventArgs ev)
                    {
                        EnvironmentContext.ActiveObject = ev.SelectedObject;
                    });
                scv.StatusChanged += new EventHandler<StatusChangedEventArgs>(scd_StatusChanged);
                scv.FileName = dlgOpenFile.FileName;
                ShowChildMDI(scv);
                scv.LoadSchema();
            }
        }

        private void topicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Application.StartupPath, "yaxt.chm"));
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem.Checked = !propertiesToolStripMenuItem.Checked;
            propertyView.Visible = propertiesToolStripMenuItem.Checked;
        }

    }
}
