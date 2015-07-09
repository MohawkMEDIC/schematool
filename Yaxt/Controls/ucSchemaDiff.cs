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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using MohawkCollege.EHR.Util.XsdDiff;
using System.Xml;
using MohawkCollege.EHR.Util.SimpleXSD;
using System.Xml.Schema;
using SchemaTool;

namespace Everest.Workshop.SchemaTool
{
    public partial class ucSchemaDiff : UserControl
    {

        private string schema1, schema2;
        private MohawkCollege.EHR.Util.SimpleXSD.XmlSchemaSet schemaSet1, schemaSet2;

        private IDiffNode selectedObject;

        /// <summary>
        /// The selected object
        /// </summary>
        public IDiffNode SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                if (SelectedObjectChanged != null)
                    SelectedObjectChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires when a new object is selected
        /// </summary>
        public event EventHandler SelectedObjectChanged;

        /// <summary>
        /// No differences exception
        /// </summary>
        public class NoDifferenceException : Exception
        {
            public NoDifferenceException() : base("Schemas are identical!"){ }
        }

        /// <summary>
        /// The schemas being compared
        /// </summary>
        public List<XmlSchema> Schemas { get; set; }

        /// <summary>
        /// Current status
        /// </summary>
        public KeyValuePair<String, Int32> Status
        {
            set
            {
                if (StatusChanged != null)
                    StatusChanged(this, new StatusChangedEventArgs() { Progress = value.Value, Text = value.Key });
            }
        }

        /// <summary>
        /// Occurs when the status is changed
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        // ctor
        public ucSchemaDiff()
        {
            InitializeComponent();
            schemaObjectA.SelectedObjectChanged += new EventHandler(schemaObjectA_SelectedObjectChanged);
            schemaObjectB.SelectedObjectChanged += new EventHandler(schemaObjectA_SelectedObjectChanged);
        }

        /// <summary>
        /// Selected object of the schema view has changed
        /// </summary>
        void schemaObjectA_SelectedObjectChanged(object sender, EventArgs e)
        {
            SelectedObject = (sender as ucSchemaObjectView).SelectedObject;
        }

        /// <summary>
        /// Compare the two schemas
        /// </summary>
        /// <param name="schema1"></param>
        /// <param name="schema2"></param>
        public void Compare(string schema1, string schema2)
        {
            this.schema1 = schema1;
            this.schema2 = schema2;
            
            Compare();
        }

        public void Compare()
        {
            this.Cursor = Cursors.AppStarting;

            try
            {
                Status = new KeyValuePair<string, int>("Comparing schemas...", 0);

                // The main diff tree
                DiffBuilder builder = new DiffBuilder();
                builder.Comparing += new XmlSchemaLoadingHandler(delegate(object sender, float perc)
                    {
                        if (StatusChanged != null)
                            StatusChanged(this, new StatusChangedEventArgs() { Text = "Comparing schemas...", Progress = (int)(perc * 100) });
                    });
                schemaSet1 = LoadSchemaSet(schema1);
                schemaSet2 = LoadSchemaSet(schema2);
                DiffTree differenceTree = builder.BuildTree(schemaSet1, schemaSet2);

                // SEt the initial schema elemnts
                schemaObjectA.SchemaObject = schemaSet1.Elements[0];
                schemaObjectB.SchemaObject = schemaSet2.Elements[0];
                schemaObjectA.Title = schema1;
                schemaObjectB.Title = schema2;

                // Draw the difference tree
                trvOverview.Nodes.Clear();

                // Add the nodes
                TreeNode node = trvOverview.Nodes.Add("root", "Comparison Summary", "root", "root");
                node.Tag = differenceTree;
                ShowDiffTreeNodes(differenceTree.Nodes, node, "/");
                //trvOverview.Nodes[0].Expand();

                Status = new KeyValuePair<string, int>("Comparison Complete...", 100);

                if (differenceTree.CountDifferences() == 0)
                    throw new NoDifferenceException();

            }
            catch (NoDifferenceException e)
            {
                Status = new KeyValuePair<string, int>("Identical schemas encountered.", 0);

                if (MessageBox.Show(
                    String.Format("The schemas '{0}' and '{1}' are identical in structure. Do you still want to see the summary?", schema1, schema2),
                    "Identical Schemas", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    throw;
            }
            catch (Exception e)
            {
                frmErrorDialog dlg = new frmErrorDialog();
                dlg.Message = "Could not complete comparison";
                dlg.Details = e.ToString();
                dlg.ShowDialog();
                Status = new KeyValuePair<string, int>("Error occured", 0);
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Display difference tree nodes
        /// </summary>
        private void ShowDiffTreeNodes(List<IDiffNode> list, TreeNode node, string xPath)
        {
            if (list == null) return;

            foreach (IDiffNode nd in list)
            {
                
                string key = String.Format("{0}/{1}", xPath, nd.FriendlyName);
                TreeNode childNode = node.Nodes.Add(nd.FriendlyName, nd.DisplayName);

                if (nd.Type != DiffType.None)
                    childNode.BackColor = Color.FromArgb((int)nd.Type);
                // Get the name of the type
                childNode.ImageKey = nd.TypeName;
                childNode.SelectedImageKey = childNode.ImageKey;
                childNode.Tag = nd;

                if (nd.Children != null && xPath != null)
                    ShowDiffTreeNodes(nd.Children, childNode, null);
            }
        }

        /// <summary>
        /// Load an XmlSchemaSet object
        /// </summary>
        private MohawkCollege.EHR.Util.SimpleXSD.XmlSchemaSet LoadSchemaSet(string fileName)
        {
            List<KeyValuePair<String, XmlSeverityType>> parseErrors = new List<KeyValuePair<String, XmlSeverityType>>();

            MohawkCollege.EHR.Util.SimpleXSD.XmlSchemaSet retVal = new MohawkCollege.EHR.Util.SimpleXSD.XmlSchemaSet();
            retVal.Loading += new XmlSchemaLoadingHandler(retVal_Loading);
            // Read
            retVal.Add(fileName);
            retVal.Load();

            // Any parsing errors or warnings?
            if (parseErrors.Count > 0)
            {
                frmErrorDialog dlg = new frmErrorDialog();
                dlg.Message = String.Format(CultureInfo.CurrentCulture, "Encountered '{0}' warnings and errors processing schema '{1}'...", parseErrors.Count, Path.GetFileName(fileName));
                foreach (KeyValuePair<String, XmlSeverityType> kv in parseErrors)
                    dlg.Details += String.Format("{0}: {1}\r\n", kv.Value, kv.Key);
                dlg.ShowDialog();
            }

            return retVal;
        }

        /// <summary>
        /// Loading handler for XML Schema
        /// </summary>
        void retVal_Loading(object Sender, float perc)
        {
            Status = new KeyValuePair<string, int>("Loading XSD...", (int)(perc * 100));
        }

        /// <summary>
        /// Item has been selected
        /// </summary>
        private void trvOverview_AfterSelect(object sender, TreeViewEventArgs e)
        {

            // Display what has changed
            if (!(e.Node.Tag is IDiffNode)) return;

            IDiffNode difference = e.Node.Tag as IDiffNode;
            switch (difference.Type)
            {
                case DiffType.Added:
                    lblDifference.Text = String.Format("'{0}' appears in new schema, but not in old schema", difference.FriendlyName);
                    break;
                case DiffType.Removed:
                    lblDifference.Text = string.Format("'{0}' appears in old schema, but not in new schema", difference.FriendlyName);
                    break;
                case DiffType.ChildDifferent:
                    lblDifference.Text = String.Format("At this level, '{0}' is the same in both schemas, however a child node has been changed", difference.FriendlyName);
                    break;
                case DiffType.TypeChanged:
                    lblDifference.Text = String.Format("'{0}' shares the same name in both schemas, but the data type has been changed from '{1}' to '{2}' (child differences noted)", difference.FriendlyName, difference.ChangeDetails[0], difference.ChangeDetails[1]);
                    break;
                case DiffType.FixedChanged:
                    lblDifference.Text = string.Format("The fixed value of '{0}' has changed from '{1}' to '{2}'", difference.FriendlyName, difference.ChangeDetails[0], difference.ChangeDetails[1]);
                    break;
                case DiffType.Namespace:
                    lblDifference.Text = string.Format("'{0}' is the same in both schemas but the namespace changes from '{1}' to '{2}'", difference.FriendlyName, difference.ChangeDetails[0], difference.ChangeDetails[1]);
                    break;
                case DiffType.None:
                    lblDifference.Text = "Node is identical in both schemas";
                    break;
            }

            // Set the grid view
            schemaObjectA.SchemaObject = difference.GetOriginalNode();
            schemaObjectB.SchemaObject = difference.GetComparedNode();

            SelectedObject = difference;
        }

        private void trvOverview_AfterExpand(object sender, TreeViewEventArgs e)
        {
            // Add the nodes
            if (e.Node == null) return;
            foreach (TreeNode child in e.Node.Nodes)
            {
                if(child.Nodes.Count == 0)
                    ShowDiffTreeNodes((child.Tag as IDiffNode).Children, child, null);
            }
        }

        private void trvObjectView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            // Add the nodes
            if (e.Node == null) return;
            foreach (TreeNode child in e.Node.Nodes)
                child.Nodes.Clear();
        }
    }
}
