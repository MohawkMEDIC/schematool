using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MohawkCollege.EHR.Util.SimpleXSD;
using System.IO;
using SchemaTool;

namespace Everest.Workshop.SchemaTool.Forms
{
    public partial class frmSchemaView : DockContent
    {
        public frmSchemaView()
        {
            InitializeComponent();
            DockAreas = DockAreas.Document;
        }

        /// <summary>
        /// Status has changed
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;
        /// <summary>
        /// The selected node has changed
        /// </summary>
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        public void LoadSchema()
        {
            this.Cursor = Cursors.AppStarting;
            XmlSchemaSet xss = new XmlSchemaSet();
            try
            {
                xss.Add(FileName);
                xss.Loading += new XmlSchemaLoadingHandler(xss_Loading);
                xss.Load();
                schemaObjectView.Schema = xss;
                this.Text = Path.GetFileName(FileName);
                this.TabText = Path.GetFileName(FileName);
                // TODO: Context.RequestStatusChange(this, new StatusChangedEventArgs() { Progress = -1 });
            }
            catch (Exception e)
            {
                frmErrorDialog dlg = new frmErrorDialog();
                dlg.Message = "Could not complete comparison";
                dlg.Details = e.ToString();
                dlg.ShowDialog();
                // TODO: Context.RequestStatusChange(this, new StatusChangedEventArgs() { Text = "Error Occured", Progress = -1 });
                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                if(StatusChanged != null)
                    StatusChanged(this, new StatusChangedEventArgs() 
                    { Progress = 0, Text = "Idle" });
                xss = null;
            }
        }

        void xss_Loading(object Sender, float perc)
        {
            if(StatusChanged != null)
                StatusChanged(this, new StatusChangedEventArgs()
                {
                    Progress = (int)(perc * 100),
                    Text = "Compiling Schema..."
                });
        }


        public string FileName { get; set; }

        // The selected object has changed
        private void schemaObjectView_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, new SelectionChangedEventArgs()
                {
                    SelectedObject = schemaObjectView.SelectedObject.GetOriginalNode()
                });
        }

        private void frmSchemaView_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.GC.Collect();
        }

        /// <summary>
        /// Show the find node form
        /// </summary>
        internal void ShowFindNode()
        {
            throw new NotImplementedException();
        }

        #region IEditorWindow Members

               /// <summary>
        /// Not implemented
        /// </summary>
        public void ScrollTo(int line, int column)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
