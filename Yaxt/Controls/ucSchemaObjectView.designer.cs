using System.Windows.Forms;
using System;
namespace Everest.Workshop.SchemaTool
{
    partial class ucSchemaObjectView
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
                DisposeDiffNodeTag(trvObjectView.Nodes);
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DisposeDiffNodeTag(System.Windows.Forms.TreeNodeCollection treeNodeCollection)
        {
            foreach (var c in treeNodeCollection)
            {
                if ((c as TreeNode).Tag is IDisposable)
                    ((c as TreeNode).Tag as IDisposable).Dispose();
                DisposeDiffNodeTag((c as TreeNode).Nodes);
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSchemaObjectView));
            this.imlTrvDiff = new System.Windows.Forms.ImageList(this.components);
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.txtXPath = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trvObjectView = new DoubleBufferedTreeView();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.imlTrvDiff.Images.SetKeyName(5, "XmlSchemaSimpleType");
            this.imlTrvDiff.Images.SetKeyName(6, "XmlSchemaAttributeGroupRef");
            this.imlTrvDiff.Images.SetKeyName(7, "XmlSchemaSequence");
            this.imlTrvDiff.Images.SetKeyName(8, "XmlSchemaElementGroupRef");
            this.imlTrvDiff.Images.SetKeyName(9, "XmlSchemaInclude");
            this.imlTrvDiff.Images.SetKeyName(10, "root");
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 13);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Title";
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblError.ForeColor = System.Drawing.Color.Maroon;
            this.lblError.Location = new System.Drawing.Point(0, 33);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(150, 117);
            this.lblError.TabIndex = 4;
            this.lblError.Text = "ERROR";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            // 
            // txtXPath
            // 
            this.txtXPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtXPath.Location = new System.Drawing.Point(0, 13);
            this.txtXPath.Name = "txtXPath";
            this.txtXPath.ReadOnly = true;
            this.txtXPath.Size = new System.Drawing.Size(150, 20);
            this.txtXPath.TabIndex = 5;
            this.txtXPath.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboardToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 26);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToClipboardToolStripMenuItem.Image")));
            this.copyToClipboardToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy Xpath";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // trvObjectView
            // 
            this.trvObjectView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvObjectView.ImageIndex = 0;
            this.trvObjectView.ImageList = this.imlTrvDiff;
            this.trvObjectView.Location = new System.Drawing.Point(0, 33);
            this.trvObjectView.Name = "trvObjectView";
            this.trvObjectView.SelectedImageIndex = 0;
            this.trvObjectView.Size = new System.Drawing.Size(150, 117);
            this.trvObjectView.StateImageList = this.imlTrvDiff;
            this.trvObjectView.TabIndex = 3;
            this.trvObjectView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.trvObjectView_AfterCollapse);
            this.trvObjectView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvObjectView_AfterSelect);
            this.trvObjectView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.trvObjectView_AfterExpand);
            // 
            // ucSchemaObjectView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.trvObjectView);
            this.Controls.Add(this.txtXPath);
            this.Controls.Add(this.lblTitle);
            this.Name = "ucSchemaObjectView";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedTreeView trvObjectView;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ImageList imlTrvDiff;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.TextBox txtXPath;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
    }
}
