using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using MohawkCollege.EHR.Util.SimpleXSD;
using SchemaTool;

namespace Everest.Workshop.SchemaTool.Forms
{
    public partial class frmSchemaDiff : DockContent
    {

        /// <summary>
        /// Selection has changed
        /// </summary>
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
        /// <summary>
        /// Status has changed
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Original schema
        /// </summary>
        public string OriginalSchema { get; set; }
        /// <summary>
        /// Compared schema
        /// </summary>
        public string ComparedSchema { get; set; }

        public frmSchemaDiff()
        {
            InitializeComponent();
            this.DockAreas = DockAreas.Document;
        }

        private void reloadSchemasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Compare();
        }

        // Show the form
        public void Compare()
        {
            if (String.IsNullOrEmpty(OriginalSchema) ||
                String.IsNullOrEmpty(ComparedSchema))
            {
                frmNewCompare fnc = new frmNewCompare();
                fnc.OldFile = OriginalSchema;
                fnc.NewFile = ComparedSchema;

                // Show the compare dialog
                if (fnc.ShowDialog() == DialogResult.OK)
                {
                    OriginalSchema = fnc.OldFile;
                    ComparedSchema = fnc.NewFile;
                    schemaDiffData.StatusChanged += new EventHandler<StatusChangedEventArgs>(schemaDiffData_StatusChanged);

                    this.Text = String.Format("Compare {0}<=>{1}", Path.GetFileName(OriginalSchema), Path.GetFileName(ComparedSchema));
                    this.TabText = this.Text;
                    try
                    {
                        schemaDiffData.Compare(OriginalSchema, ComparedSchema);

                    }
                    catch
                    {
                        this.Close();
                    }
                }
                else
                    this.Close();

            }
        }

        /// <summary>
        /// Status has changed
        /// </summary>
        void schemaDiffData_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            // TODO: Context.RequestStatusChange(this, e);
            if (StatusChanged != null)
                StatusChanged(this, e);
        }


        #region IEnvironmentChild Members

        /// <summary>
        /// The active context
        /// </summary>
        //public ActiveEnvironmentContext Context { get; set; }

        #endregion

        /// <summary>
        ///  Small structure for displaying diff data in the property window
        /// </summary>
        [Description("Comparison Node")]
        private struct DiffData
        {
            
            [TypeConverter(typeof(ExpandableObjectConverter))]
            [ReadOnly(true)]
            public XmlSchemaObject Original { get; set; }
            [TypeConverter(typeof(ExpandableObjectConverter))]
            [ReadOnly(true)]
            public XmlSchemaObject Compared { get; set; }
        }

        private void schemaDiffData_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (schemaDiffData.SelectedObject.GetComparedNode() == null && SelectionChanged != null)
                SelectionChanged(this, new SelectionChangedEventArgs() { SelectedObject = schemaDiffData.SelectedObject.GetComparedNode() });
            else if (SelectionChanged != null)
            {
                SelectionChanged(this, new SelectionChangedEventArgs() { SelectedObject = new DiffData()
                {
                    Original = schemaDiffData.SelectedObject.GetOriginalNode(),
                    Compared = schemaDiffData.SelectedObject.GetComparedNode()
                }
                }
                );
            }
        }

        private void frmSchemaDiff_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.GC.Collect();
        }

      
    }
}
