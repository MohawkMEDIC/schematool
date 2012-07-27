using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using MohawkCollege.EHR.Util.SimpleXSD;
using System.Diagnostics;

namespace MohawkCollege.EHR.Util.XsdDiff
{
    /// <summary>
    /// A diff-node represents differencing data for one node set
    /// </summary>
    public class DiffNode<T> : IDiffNode, IDisposable
        where T : XmlSchemaObject
    {

        /// <summary>
        /// Get the namespace the current node belongs to
        /// </summary>
        public string NodeNamespace
        {
            get
            {
                if (Original != null)
                    return Original.Namespace;
                else
                    return Compared.Namespace;
            }
        }

        /// <summary>
        /// Change details
        /// </summary>
        public string[] ChangeDetails { get; protected set; }
        /// <summary>
        /// Display name
        /// </summary>
        public virtual string DisplayName
        {
            get
            {
                return FriendlyName;
            }
        }
        /// <summary>
        /// A reference to the node in the original schema
        /// </summary>
        public T Original { get; set; }

        /// <summary>
        /// A reference to the node in the compared schema
        /// </summary>
        public T Compared { get; set; }

        /// <summary>
        /// The type of change that occurred
        /// </summary>
        public DiffType Type
        {
            get;
            protected set;
        }
        /// <summary>
        /// Get the name of the type this diff node compares
        /// </summary>
        public string TypeName
        {
            get { return typeof(T).Name; }
        }
        /// <summary>
        /// The name to display in the diff tree
        /// </summary>
        public virtual string FullName
        {
            get
            {
                // Get name property
                if (Original != null) return String.Format("{0}#{1}", Original.Namespace, Original.Name);
                else return String.Format("{0}#{1}", Compared.Namespace, Compared.Name);
            }
        }
        /// <summary>
        /// The friendly name
        /// </summary>
        public virtual string FriendlyName
        {
            get
            {

                // Get name property
                if (Original != null) return String.Format("{0}", Original.Name);
                else return String.Format("{0}", Compared.Name);
            }
        }
        /// <summary>
        /// Gets the number of differences
        /// </summary>
        public int CountDifferences()
        {
            int retVal = Type == DiffType.None ? 0 : 1;
            foreach (var child in Children ?? new List<IDiffNode>())
                retVal += child.CountDifferences();
            return retVal;
        }

        #region IDiffNode Members

        /// <summary>
        /// Get or sets the parent node
        /// </summary>
        public IDiffNode Parent
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the children of this node
        /// </summary>
        public List<IDiffNode> Children { get; set; }

        /// <summary>
        /// Add a child to this node
        /// </summary>
        public void AddChild(IDiffNode child)
        {
            if (Children == null) Children = new List<IDiffNode>();
            child.Parent = this;
            Children.Add(child);
        }

        /// <summary>
        /// Set the data of this node
        /// </summary>
        public void SetData(XmlSchemaObject original, XmlSchemaObject compared)
        {
            if (!((original is T || original == null) && (compared is T || compared == null)))
                throw new InvalidOperationException(String.Format("Parameters must match the generic parameter of '{0'}!", typeof(T).FullName));


            if (original == null && compared == null)
                System.Diagnostics.Debugger.Break();

            Original = original as T;
            Compared = compared as T;
        }

        /// <summary>
        /// Get the original node
        /// </summary>
        public XmlSchemaObject GetOriginalNode()
        {
            return Original;
        }

        /// <summary>
        /// Get the compared node
        /// </summary>
        public XmlSchemaObject GetComparedNode()
        {
            return Compared;
        }

        #endregion

        /// <summary>
        /// Parse method sets the differencing type
        /// </summary>
        public virtual void CalculateDiff()
        {
            if(Children != null)
                foreach (var child in Children)
                    child.CalculateDiff();

            if (Original != null && Compared == null)
                Type = DiffType.Removed;
            else if (Original == null && Compared != null)
                Type = DiffType.Added;
            else if (Original.Namespace != Compared.Namespace)
            {
                Type = DiffType.Namespace;
                ChangeDetails = new string[] {
                    Original.Namespace,
                    Compared.Namespace
                };
            }
            else if (Children != null && Children.Find(o => o.Type != DiffType.None) != null)
                Type = DiffType.ChildDifferent;
            else
                Type = DiffType.None;
        }

        public override string ToString()
        {
            return FriendlyName;
        }

        /// <summary>
        /// Clone this object
        /// </summary>
        /// <returns></returns>
        public IDiffNode Clone()
        {
            IDiffNode retVal = MemberwiseClone() as IDiffNode;

            if (Children != null)
            {
                retVal.Children = new List<IDiffNode>();
                foreach (var child in Children)
                    retVal.AddChild(child.Clone());
            }

            return retVal;
        }

        #region IDiffNode Members

        /// <summary>
        /// Adds a child to this node and preserves its sequence order
        /// </summary>
        public void AddChildPreserveOrder(IDiffNode child)
        {
            if (Children == null) Children = new List<IDiffNode>();

            // Now time to preserve!
            if (child.Parent == null)
                Children.Add(child);
            else
            {
                // Find the node that appeard prior to this child in the original tree
                int idx = child.Parent.Children.IndexOf(child);
                if (idx > 0)
                {
                    IDiffNode dn = null;
                    int fi = -1;
                    while (fi == -1 && idx > 0)
                    {
                        idx--;
                        dn = child.Parent.Children[idx];
                        fi = Children.FindIndex(o => o.FullName == dn.FullName); // find the index of the same node in this file ..
                    }
                    
                    // Could we find a reference?
                    if (fi == -1) // nope, so add
                        Children.Add(child);
                    else
                        Children.Insert(fi+1, child);
                }
                else  // Appears first
                    Children.Insert(0, child);
            }
        }

        #endregion

        #region IDisposable Members

        // True if the object has been disposed
        bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                Children = null;
                Original = null;
                Compared = null;
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}
