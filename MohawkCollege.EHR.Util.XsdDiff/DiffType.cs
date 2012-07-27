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
