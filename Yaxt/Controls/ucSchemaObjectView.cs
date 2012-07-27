using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MohawkCollege.EHR.Util.SimpleXSD;
using MohawkCollege.EHR.Util.XsdDiff;

namespace Everest.Workshop.SchemaTool
{
    public partial class ucSchemaObjectView : UserControl
    {
        private XmlSchemaObject schemaObject;

        /// <summary>
        /// Show an XPath 
        /// </summary>
        public bool ShowXpath
        {
            get { return txtXPath.Visible; }
            set { txtXPath.Visible = value; }
        }

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
        /// True if viewing should be done deeply
        /// </summary>
        public bool DeepView { get; set; }
        /// <summary>
        /// Title override
        /// </summary>
        public string Title { get { return lblTitle.Text; } set { lblTitle.Text = value; lblTitle.Visible = !String.IsNullOrEmpty(value); } }
        /// <summary>
        /// The schema object to show
        /// </summary>
        public XmlSchemaObject SchemaObject {
            get
            {
                return schemaObject;
            }
            set
            {
                schemaObject = value;
                ProcessSchemaObject();
            }
        }
        /// <summary>
        /// Schema set view
        /// </summary>
        public XmlSchemaSet Schema
        {
            set
            {
                ProcessSchema(value);
            }
        }

        /// <summary>
        /// Process schema
        /// </summary>
        private void ProcessSchema(XmlSchemaSet value)
        {
            lblError.Visible = value == null;

                // Elements
                DiffBuilder builder = new DiffBuilder();
                builder.TreatSequenceAsNode = true;

                List<IDiffNode> diffNodes = new List<IDiffNode>();

                if (value.Elements == null || value.Elements.Count == 0)
                    throw new InvalidOperationException("This view requires that a schema have at least one root element!");

                foreach (XmlSchemaElement xse in value.Elements)
                    diffNodes.Add(builder.BuildTree(xse));

                trvObjectView.Visible = false;
                trvObjectView.Nodes.Clear();
                ShowDiffTreeNodes(diffNodes, null, trvObjectView);
                trvObjectView.Nodes[0].Expand();
                trvObjectView.Visible = true;

        }

        /// <summary>
        /// Process the schema object
        /// </summary>
        private void ProcessSchemaObject()
        {
            if (schemaObject == null)
            {
                lblError.Visible = true;
                lblError.Text = "Object not available";
                return;
            }
            else
                lblError.Visible = false;

            DiffBuilder builder = new DiffBuilder();
            IDiffNode diffNode = builder.BuildTree(SchemaObject);
            trvObjectView.Visible = false;
            trvObjectView.Nodes.Clear();
            ShowDiffTreeNodes(new List<IDiffNode>() { diffNode }, null, trvObjectView);
            if(trvObjectView.Nodes.Count > 0)
                trvObjectView.Nodes[0].Expand();
            trvObjectView.Visible = true;
        }

        /// <summary>
        /// Display difference tree nodes
        /// </summary>
        private void ShowDiffTreeNodes(List<IDiffNode> list, TreeNode node, TreeView trv)
        {
            if (list == null) return;

            foreach (IDiffNode nd in list)
            {
                if (nd == null) continue;

                TreeNode childNode = null;
                if (trv == null && node != null)
                    childNode = node.Nodes.Add(nd.FriendlyName, nd.DisplayName);
                else if (trv != null)
                    childNode = trv.Nodes.Add(nd.FriendlyName, nd.DisplayName);
                else
                    return;

                // Get the name of the type
                childNode.ImageKey = nd.TypeName;
                childNode.SelectedImageKey = childNode.ImageKey;
                childNode.Tag = nd;
                childNode.ContextMenuStrip = contextMenuStrip1;
                if (nd.Children != null && trv != null)
                    ShowDiffTreeNodes(nd.Children, childNode,null);
            }
        }

        public ucSchemaObjectView()
        {
            InitializeComponent();
        }

        private void trvObjectView_AfterSelect(object sender, TreeViewEventArgs e)
        {


            // Get the diff node 
            IDiffNode idn = e.Node.Tag as IDiffNode;
            if (idn == null) return;

            // Set the selected object
            this.SelectedObject = idn;
            if (!ShowXpath) return;
            // Show the XPath
            txtXPath.Text = BuildXpath(e.Node);
            txtXPath.SelectionStart = txtXPath.Text.Length;

        }

        /// <summary>
        /// Build an xpath by prepending the data from lastnode
        /// </summary>
        private string BuildXpath(TreeNode tn)
        {
            IDiffNode lastNode = tn.Tag as IDiffNode;
            if (lastNode.TypeName == "XmlSchemaAttribute" &&
                String.IsNullOrEmpty(lastNode.NodeNamespace))
                return string.Format("{0}/@{1}",
                    BuildXpath(tn.Parent), lastNode.FriendlyName);
            else if (lastNode.TypeName == "XmlSchemaAttribute")
                return string.Format("{0}/*@[namespace-uri() = '{1}' and local-name() = '{2}']",
                    BuildXpath(tn.Parent), lastNode.NodeNamespace, lastNode.FriendlyName);
            else if (lastNode.Parent != null && lastNode.TypeName == "XmlSchemaElement")
                return string.Format("{0}/*[namespace-uri() = '{1}' and local-name() = '{2}']",
                    BuildXpath(tn.Parent), lastNode.NodeNamespace, lastNode.FriendlyName);
            else if (lastNode.TypeName == "XmlSchemaElement")
                return string.Format("/*[namespace-uri() = '{0}' and local-name() = '{1}']",
                    lastNode.NodeNamespace, lastNode.FriendlyName);
            else if (lastNode.Parent != null)
                return BuildXpath(tn.Parent);
            else return "";
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (trvObjectView.SelectedNode == null) return;

            // Get the diff node 
            IDiffNode idn = trvObjectView.SelectedNode.Tag as IDiffNode;
            if (idn == null) return;

            Clipboard.SetText(BuildXpath(trvObjectView.SelectedNode));
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            Point location = trvObjectView.PointToClient(contextMenuStrip1.Location);
            trvObjectView.SelectedNode = trvObjectView.GetNodeAt(location.X, location.Y);
        }

        private void trvObjectView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            // Add the nodes
            if (e.Node == null || !DeepView) return;
            foreach (TreeNode child in e.Node.Nodes)
            {
                ShowDiffTreeNodes((child.Tag as IDiffNode).Children, child, null);
            }

        }

        private void trvObjectView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            // Add the nodes
            if (e.Node == null || !DeepView) return;
            foreach (TreeNode child in e.Node.Nodes)
                child.Nodes.Clear();
        }


 


      
    }
}
