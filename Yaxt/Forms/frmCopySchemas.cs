/**
 * Copyright 2009-2015 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Schema;
using System.IO;

namespace Everest.Workshop.SchemaTool
{
    public partial class frmCopySchemas : Form
    {
        public frmCopySchemas()
        {
            InitializeComponent();
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = "";
                foreach (var s in dlgOpenFile.FileNames)
                    txtSource.Text += string.Format("\"{0}\" ", s);
            }
        }

        private void btnBrowseDest_Click(object sender, EventArgs e)
        {
            if (dlgBrowseFolder.ShowDialog() == DialogResult.OK)
                txtDest.Text = dlgBrowseFolder.SelectedPath;
        }

        private void CopySchema(string source, string destDir, List<String> files)
        {
            if (files.Contains(source))
                return;
            files.Add(source);

            // Copy to output
            lblStatus.Text = String.Format("Copy '{0}'->'{1}\\{0}'", Path.GetFileName(source), destDir);
            Application.DoEvents();
            File.Copy(source, Path.Combine(destDir, Path.GetFileName(source)), true);
            
            // Copy dependencies
            Stream fs = null;
            try
            {
                fs = File.OpenRead(source);
                XmlSchema xsd = XmlSchema.Read(fs, new ValidationEventHandler(delegate(object o, ValidationEventArgs e1)
                {
                    System.Diagnostics.Trace.WriteLine(e1.Message);
                }));
                fs.Close();

                // Process includes
                foreach (object xsi in xsd.Includes)
                    if(xsi is XmlSchemaInclude)
                        CopySchema(Path.Combine(Path.GetDirectoryName(source), (xsi as XmlSchemaInclude).SchemaLocation), destDir, files);
                    else if(xsi is XmlSchemaImport &&
                        (xsi as XmlSchemaImport).SchemaLocation != null)
                        CopySchema(Path.Combine(Path.GetDirectoryName(source), (xsi as XmlSchemaImport).SchemaLocation), destDir, files);
            }
            catch (Exception ex)
            {
                frmErrorDialog dlg = new frmErrorDialog();
                dlg.Message = ex.Message;
                dlg.Details = ex.ToString();
                dlg.ShowDialog();
                return;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {

            // Find out if any files will be overwritten

            if (Directory.GetFiles(txtDest.Text).Count() > 0 && MessageBox.Show(
                String.Format("The destination directory '{0}' is not empty. Files in this directory will be overwritten if their name matches. Are you sure you want to continue with the copy?", txtDest.Text), "Non-Empty Directory Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            btnCopy.Enabled = false;
            btnClose.Enabled = false;

            if (dlgOpenFile.FileNames.Count() == 0 || String.IsNullOrEmpty(txtDest.Text))
            {
                MessageBox.Show("Please select files and a destination!");
                return;
            }

            List<String> files = new List<string>();
            // Copy the files
            int i =0;
            foreach (var s in dlgOpenFile.FileNames)
            {
                CopySchema(s, txtDest.Text, files);
                i++;
                pgStatus.Value = (int)(((float)i / dlgOpenFile.FileNames.Count()) * 100);
            }
            lblStatus.Text = "Complete";
            btnCopy.Enabled = true;
            btnClose.Enabled = true;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
