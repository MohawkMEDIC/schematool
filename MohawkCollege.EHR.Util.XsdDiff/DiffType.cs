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

namespace MohawkCollege.EHR.Util.XsdDiff
{
    public enum DiffType : uint
    {
        /// <summary>
        /// Item appears in B but not A
        /// </summary>
        Added = 0x99ff99,
        /// <summary>
        /// Item appears in A but not B
        /// </summary>
        Removed = 0xff9999,
        /// <summary>
        /// A child of this node has a difference
        /// </summary>
        ChildDifferent = 0xffff99,
        /// <summary>
        /// The element name is the same, but the data-type is different
        /// </summary>
        TypeChanged = 0x9999ff,
        /// <summary>
        /// Fixed value has changed
        /// </summary>
        FixedChanged = 0x99ffff,
        /// <summary>
        /// Namespace change
        /// </summary>
        Namespace = 0xff99ff,
        /// <summary>
        /// No change
        /// </summary>
        None = 0xffffff
    }
}
