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
    public partial class frmErrorDialog : Form
    {

        /// <summary>
        /// Details
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        public frmErrorDialog()
        {
            InitializeComponent();
        }

       
        private void frmErrorDialog_Load(object sender, EventArgs e)
        {
            lblError.Text = Message;
            txtDetails.Text = Details;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
