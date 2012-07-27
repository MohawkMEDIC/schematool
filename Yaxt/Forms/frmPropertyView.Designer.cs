namespace SchemaTool.Forms
{
    partial class frmPropertyView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPropertyView));
            this.pgMain = new System.Windows.Forms.PropertyGrid();
            this.lblObjectType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pgMain
            // 
            this.pgMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgMain.HelpVisible = false;
            this.pgMain.Location = new System.Drawing.Point(0, 22);
            this.pgMain.Name = "pgMain";
            this.pgMain.Size = new System.Drawing.Size(214, 237);
            this.pgMain.TabIndex = 0;
            this.pgMain.SelectedObjectsChanged += new System.EventHandler(this.pgMain_SelectedObjectsChanged);
            // 
            // lblObjectType
            // 
            this.lblObjectType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblObjectType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblObjectType.Location = new System.Drawing.Point(0, 0);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(214, 22);
            this.lblObjectType.TabIndex = 1;
            this.lblObjectType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmPropertyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 259);
            this.Controls.Add(this.pgMain);
            this.Controls.Add(this.lblObjectType);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPropertyView";
            this.Text = "Properties";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgMain;
        private System.Windows.Forms.Label lblObjectType;
    }
}