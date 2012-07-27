namespace Everest.Workshop.SchemaTool.Forms
{
    partial class frmSchemaView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSchemaView));
            this.schemaObjectView = new ucSchemaObjectView();
            this.SuspendLayout();
            // 
            // schemaObjectView
            // 
            this.schemaObjectView.DeepView = true;
            this.schemaObjectView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaObjectView.Location = new System.Drawing.Point(0, 0);
            this.schemaObjectView.Name = "schemaObjectView";
            this.schemaObjectView.SchemaObject = null;
            this.schemaObjectView.SelectedObject = null;
            this.schemaObjectView.ShowXpath = true;
            this.schemaObjectView.Size = new System.Drawing.Size(284, 262);
            this.schemaObjectView.TabIndex = 0;
            this.schemaObjectView.Title = "";
            this.schemaObjectView.SelectedObjectChanged += new System.EventHandler(this.schemaObjectView_SelectedObjectChanged);
            // 
            // frmSchemaView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.schemaObjectView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSchemaView";
            this.Text = "Schema View";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSchemaView_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private ucSchemaObjectView schemaObjectView;
    }
}