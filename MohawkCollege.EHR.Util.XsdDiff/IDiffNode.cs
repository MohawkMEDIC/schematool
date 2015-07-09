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
using System.Linq;
using System.Text;
using MohawkCollege.EHR.Util.SimpleXSD;

namespace MohawkCollege.EHR.Util.XsdDiff
{
    /// <summary>
    /// Interface defining a diff node
    /// </summary>
    public interface IDiffNode
    {
        /// <summary>
        /// Gets the namespace the node belongs to
        /// </summary>
        string NodeNamespace { get; }
        /// <summary>
        /// Number of differences
        /// </summary>
        int CountDifferences();
        /// <summary>
        /// Get the full name of the node
        /// </summary>
        string FullName { get; }
        /// <summary>
        /// Change details
        /// </summary>
        string[] ChangeDetails { get; }
        /// <summary>
        /// The name to display
        /// </summary>
        string DisplayName { get; }
        /// <summary>
        /// Get the type of difference
        /// </summary>
        DiffType Type { get; }
        /// <summary>
        /// Get the friendly name of the difference
        /// </summary>
        String FriendlyName { get; }
        /// <summary>
        /// Get the parent of the different node
        /// </summary>
        IDiffNode Parent { get; set; }
        /// <summary>
        /// Get the children of the difference node
        /// </summary>
        List<IDiffNode> Children { get; set; }
        /// <summary>
        /// Add a child to this difference node
        /// </summary>
        void AddChild(IDiffNode child);
        /// <summary>
        /// Set the data for the difference
        /// </summary>
        void SetData(XmlSchemaObject original, XmlSchemaObject compared);
        /// <summary>
        /// Get the original node
        /// </summary>
        XmlSchemaObject GetOriginalNode();
        /// <summary>
        /// Get the compared node
        /// </summary>
        XmlSchemaObject GetComparedNode();
        /// <summary>
        /// Get the type name of the contained compared objects
        /// </summary>
        string TypeName { get; }
        /// <summary>
        /// Calculate the diff
        /// </summary>
        void CalculateDiff();
        /// <summary>
        /// ADd the child to this node and preserve the sequence order
        /// </summary>
        void AddChildPreserveOrder(IDiffNode child);

        IDiffNode Clone();
    }
}
