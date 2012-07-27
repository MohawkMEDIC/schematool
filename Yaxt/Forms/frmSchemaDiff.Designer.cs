namespace Everest.Workshop.SchemaTool.Forms
{
    partial class frmSchemaDiff
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSchemaDiff));
            this.mnuDiff = new System.Windows.Forms.MenuStrip();
            this.comparisonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadSchemasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schemaDiffData = new ucSchemaDiff();
            this.mnuDiff.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuDiff
            // 
            this.mnuDiff.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.comparisonToolStripMenuItem});
            this.mnuDiff.Location = new System.Drawing.Point(0, 0);
            this.mnuDiff.Name = "mnuDiff";
            this.mnuDiff.Size = new System.Drawing.Size(463, 24);
            this.mnuDiff.TabIndex = 1;
            this.mnuDiff.Text = "menuStrip1";
            this.mnuDiff.Visible = false;
            // 
            // comparisonToolStripMenuItem
            // 
            this.comparisonToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadSchemasToolStripMenuItem});
            this.comparisonToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.comparisonToolStripMenuItem.MergeIndex = 1;
            this.comparisonToolStripMenuItem.Name = "comparisonToolStripMenuItem";
            this.comparisonToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.comparisonToolStripMenuItem.Text = "&Comparison";
            // 
            // reloadSchemasToolStripMenuItem
            // 
            this.reloadSchemasToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("reloadSchemasToolStripMenuItem.Image")));
            this.reloadSchemasToolStripMenuItem.Name = "reloadSchemasToolStripMenuItem";
            this.reloadSchemasToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.reloadSchemasToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.reloadSchemasToolStripMenuItem.Text = "&Refresh";
            this.reloadSchemasToolStripMenuItem.Click += new System.EventHandler(this.reloadSchemasToolStripMenuItem_Click);
            // 
            // schemaDiffData
            // 
            this.schemaDiffData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaDiffData.Location = new System.Drawing.Point(0, 0);
            this.schemaDiffData.Name = "schemaDiffData";
            this.schemaDiffData.Schemas = null;
            this.schemaDiffData.SelectedObject = null;
            this.schemaDiffData.Size = new System.Drawing.Size(463, 356);
            this.schemaDiffData.TabIndex = 0;
            this.schemaDiffData.SelectedObjectChanged += new System.EventHandler(this.schemaDiffData_SelectedObjectChanged);
            // 
            // frmSchemaDiff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 356);
            this.Controls.Add(this.schemaDiffData);
            this.Controls.Add(this.mnuDiff);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuDiff;
            this.Name = "frmSchemaDiff";
            this.Text = "Schema Diff";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSchemaDiff_FormClosed);
            this.mnuDiff.ResumeLayout(false);
            this.mnuDiff.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ucSchemaDiff schemaDiffData;
        private System.Windows.Forms.MenuStrip mnuDiff;
        private System.Windows.Forms.ToolStripMenuItem comparisonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadSchemasToolStripMenuItem;
    }
}