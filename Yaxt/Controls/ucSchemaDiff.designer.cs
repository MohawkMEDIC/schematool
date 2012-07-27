namespace Everest.Workshop.SchemaTool
{
    partial class ucSchemaDiff
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSchemaDiff));
            this.spDetails = new System.Windows.Forms.SplitContainer();
            this.trvOverview = new DoubleBufferedTreeView();
            this.imlTrvDiff = new System.Windows.Forms.ImageList(this.components);
            this.lblDifference = new System.Windows.Forms.Label();
            this.scSchemaTool = new System.Windows.Forms.SplitContainer();
            this.schemaObjectA = new ucSchemaObjectView();
            this.schemaObjectB = new ucSchemaObjectView();
            this.spDetails.Panel1.SuspendLayout();
            this.spDetails.Panel2.SuspendLayout();
            this.spDetails.SuspendLayout();
            this.scSchemaTool.Panel1.SuspendLayout();
            this.scSchemaTool.Panel2.SuspendLayout();
            this.scSchemaTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // spDetails
            // 
            this.spDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spDetails.Location = new System.Drawing.Point(0, 0);
            this.spDetails.Name = "spDetails";
            this.spDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spDetails.Panel1
            // 
            this.spDetails.Panel1.Controls.Add(this.trvOverview);
            this.spDetails.Panel1.Controls.Add(this.lblDifference);
            // 
            // spDetails.Panel2
            // 
            this.spDetails.Panel2.Controls.Add(this.scSchemaTool);
            this.spDetails.Size = new System.Drawing.Size(599, 545);
            this.spDetails.SplitterDistance = 295;
            this.spDetails.TabIndex = 0;
            // 
            // trvOverview
            // 
            this.trvOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvOverview.ImageIndex = 0;
            this.trvOverview.ImageList = this.imlTrvDiff;
            this.trvOverview.Location = new System.Drawing.Point(0, 0);
            this.trvOverview.Name = "trvOverview";
            this.trvOverview.SelectedImageIndex = 0;
            this.trvOverview.Size = new System.Drawing.Size(599, 282);
            this.trvOverview.StateImageList = this.imlTrvDiff;
            this.trvOverview.TabIndex = 0;
            this.trvOverview.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.trvObjectView_AfterCollapse);
            this.trvOverview.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvOverview_AfterSelect);
            this.trvOverview.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.trvOverview_AfterExpand);
            // 
            // imlTrvDiff
            // 
            this.imlTrvDiff.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlTrvDiff.ImageStream")));
            this.imlTrvDiff.TransparentColor = System.Drawing.Color.Magenta;
            this.imlTrvDiff.Images.SetKeyName(0, "XmlSchemaComplexType");
            this.imlTrvDiff.Images.SetKeyName(1, "XmlSchemaAttribute");
            this.imlTrvDiff.Images.SetKeyName(2, "XmlSchemaChoice");
            this.imlTrvDiff.Images.SetKeyName(3, "XmlSchemaElement");
            this.imlTrvDiff.Images.SetKeyName(4, "XmlSchemaUnion");
            this.imlTrvDiff.Images.SetKeyName(5, "XmlSchemaAny");
            this.imlTrvDiff.Images.SetKeyName(6, "XmlSchemaAttributeGroupRef");
            this.imlTrvDiff.Images.SetKeyName(7, "XmlSchemaSequence");
            this.imlTrvDiff.Images.SetKeyName(8, "XmlSchemaElementGroupRef");
            this.imlTrvDiff.Images.SetKeyName(9, "XmlSchemaInclude");
            this.imlTrvDiff.Images.SetKeyName(10, "root");
            // 
            // lblDifference
            // 
            this.lblDifference.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDifference.Location = new System.Drawing.Point(0, 282);
            this.lblDifference.Name = "lblDifference";
            this.lblDifference.Size = new System.Drawing.Size(599, 13);
            this.lblDifference.TabIndex = 1;
            this.lblDifference.Text = "(Select Node)";
            // 
            // scSchemaTool
            // 
            this.scSchemaTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSchemaTool.Location = new System.Drawing.Point(0, 0);
            this.scSchemaTool.Name = "scSchemaTool";
            // 
            // scSchemaTool.Panel1
            // 
            this.scSchemaTool.Panel1.Controls.Add(this.schemaObjectA);
            // 
            // scSchemaTool.Panel2
            // 
            this.scSchemaTool.Panel2.Controls.Add(this.schemaObjectB);
            this.scSchemaTool.Size = new System.Drawing.Size(599, 246);
            this.scSchemaTool.SplitterDistance = 300;
            this.scSchemaTool.TabIndex = 0;
            // 
            // schemaObjectA
            // 
            this.schemaObjectA.DeepView = false;
            this.schemaObjectA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaObjectA.Location = new System.Drawing.Point(0, 0);
            this.schemaObjectA.Name = "schemaObjectA";
            this.schemaObjectA.SchemaObject = null;
            this.schemaObjectA.SelectedObject = null;
            this.schemaObjectA.ShowXpath = false;
            this.schemaObjectA.Size = new System.Drawing.Size(300, 246);
            this.schemaObjectA.TabIndex = 0;
            this.schemaObjectA.Title = "Title";
            // 
            // schemaObjectB
            // 
            this.schemaObjectB.DeepView = false;
            this.schemaObjectB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaObjectB.Location = new System.Drawing.Point(0, 0);
            this.schemaObjectB.Name = "schemaObjectB";
            this.schemaObjectB.SchemaObject = null;
            this.schemaObjectB.SelectedObject = null;
            this.schemaObjectB.ShowXpath = false;
            this.schemaObjectB.Size = new System.Drawing.Size(295, 246);
            this.schemaObjectB.TabIndex = 0;
            this.schemaObjectB.Title = "Title";
            // 
            // ucSchemaDiff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spDetails);
            this.Name = "ucSchemaDiff";
            this.Size = new System.Drawing.Size(599, 545);
            this.spDetails.Panel1.ResumeLayout(false);
            this.spDetails.Panel2.ResumeLayout(false);
            this.spDetails.ResumeLayout(false);
            this.scSchemaTool.Panel1.ResumeLayout(false);
            this.scSchemaTool.Panel2.ResumeLayout(false);
            this.scSchemaTool.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spDetails;
        private System.Windows.Forms.SplitContainer scSchemaTool;
        private DoubleBufferedTreeView trvOverview;
        private System.Windows.Forms.ImageList imlTrvDiff;
        private System.Windows.Forms.Label lblDifference;
        private ucSchemaObjectView schemaObjectA;
        private ucSchemaObjectView schemaObjectB;
    }
}
