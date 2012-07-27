using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Everest.Workshop.SchemaTool
{
    public partial class frmNewCompare : Form
    {

        /// <summary>
        /// Old file
        /// </summary>
        public string OldFile { get { return txtOldSchema.Text; } set { txtNewSchema.Text = value; } }
        /// <summary>
        /// New file
        /// </summary>
        public string NewFile { get { return txtNewSchema.Text; } set { txtNewSchema.Text = value; } }

        public frmNewCompare()
        {
            InitializeComponent();
        }

        private void btnBrowseOld_Click(object sender, EventArgs e)
        {
            dlgOpenSchema.FileName = "";
            if (dlgOpenSchema.ShowDialog() == DialogResult.OK)
                txtOldSchema.Text = dlgOpenSchema.FileName;
        }

        private void btnBrowseNew_Click(object sender, EventArgs e)
        {
            dlgOpenSchema.FileName = "";
            if (dlgOpenSchema.ShowDialog() == DialogResult.OK)
                txtNewSchema.Text = dlgOpenSchema.FileName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtNewSchema.Text) || String.IsNullOrEmpty(txtOldSchema.Text))
            {
                MessageBox.Show("Comparison must be done between two schemas!", "Input Error");
                return;
            }
            else if (txtNewSchema.Text.Equals(txtOldSchema.Text) &&
                MessageBox.Show("It appears the two schemas you have selected the same file. Do you still want to compare them?", "Same File", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
