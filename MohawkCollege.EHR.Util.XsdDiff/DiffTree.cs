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
    /// Contains a tree of differenced items from a schema
    /// </summary>
    public class DiffTree
    {
        /// <summary>
        /// Gets or sets the original schema
        /// </summary>
        public XmlSchemaSet Original { get; set; }
        /// <summary>
        /// Gets or sets the compared schema
        /// </summary>
        public XmlSchemaSet Compared { get; set; }
        /// <summary>
        /// Nodes at the root level of the diff tree
        /// </summary>
        public List<IDiffNode> Nodes { get; private set;  }

        // ctor
        public DiffTree() { Nodes = new List<IDiffNode>(); }

        /// <summary>
        /// Determine if there are no differences in this tree
        /// </summary>
        public int CountDifferences()
        {
            int retVal = 0;
            foreach (var child in Nodes)
                retVal += child.CountDifferences();
            return retVal;
        }

        /// <summary>
        /// Calculate differences
        /// </summary>
        internal void CalculateDiff()
        {
            foreach (var child in Nodes)
                child.CalculateDiff();
        }
    }
}
