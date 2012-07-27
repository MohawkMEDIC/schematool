namespace Everest.Workshop.SchemaTool
{
    partial class frmNewCompare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewCompare));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOldSchema = new System.Windows.Forms.TextBox();
            this.txtNewSchema = new System.Windows.Forms.TextBox();
            this.btnBrowseOld = new System.Windows.Forms.Button();
            this.btnBrowseNew = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dlgOpenSchema = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Old Schema";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "New Schema";
            // 
            // txtOldSchema
            // 
            this.txtOldSchema.Location = new System.Drawing.Point(94, 19);
            this.txtOldSchema.Name = "txtOldSchema";
            this.txtOldSchema.ReadOnly = true;
            this.txtOldSchema.Size = new System.Drawing.Size(312, 20);
            this.txtOldSchema.TabIndex = 2;
            // 
            // txtNewSchema
            // 
            this.txtNewSchema.Location = new System.Drawing.Point(94, 50);
            this.txtNewSchema.Name = "txtNewSchema";
            this.txtNewSchema.ReadOnly = true;
            this.txtNewSchema.Size = new System.Drawing.Size(312, 20);
            this.txtNewSchema.TabIndex = 3;
            // 
            // btnBrowseOld
            // 
            this.btnBrowseOld.Location = new System.Drawing.Point(410, 19);
            this.btnBrowseOld.Name = "btnBrowseOld";
            this.btnBrowseOld.Size = new System.Drawing.Size(30, 20);
            this.btnBrowseOld.TabIndex = 4;
            this.btnBrowseOld.Text = "...";
            this.btnBrowseOld.UseVisualStyleBackColor = true;
            this.btnBrowseOld.Click += new System.EventHandler(this.btnBrowseOld_Click);
            // 
            // btnBrowseNew
            // 
            this.btnBrowseNew.Location = new System.Drawing.Point(410, 50);
            this.btnBrowseNew.Name = "btnBrowseNew";
            this.btnBrowseNew.Size = new System.Drawing.Size(30, 20);
            this.btnBrowseNew.TabIndex = 5;
            this.btnBrowseNew.Text = "...";
            this.btnBrowseNew.UseVisualStyleBackColor = true;
            this.btnBrowseNew.Click += new System.EventHandler(this.btnBrowseNew_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(284, 77);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Compare";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(365, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dlgOpenSchema
            // 
            this.dlgOpenSchema.Filter = "XML Schemas (*.xsd)|*.xsd";
            // 
            // frmNewCompare
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(454, 112);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnBrowseNew);
            this.Controls.Add(this.btnBrowseOld);
            this.Controls.Add(this.txtNewSchema);
            this.Controls.Add(this.txtOldSchema);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNewCompare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Comparison";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOldSchema;
        private System.Windows.Forms.TextBox txtNewSchema;
        private System.Windows.Forms.Button btnBrowseOld;
        private System.Windows.Forms.Button btnBrowseNew;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog dlgOpenSchema;
    }
}