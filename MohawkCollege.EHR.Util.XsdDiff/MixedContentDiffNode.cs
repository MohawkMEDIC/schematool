using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MohawkCollege.EHR.Util.SimpleXSD;

namespace MohawkCollege.EHR.Util.XsdDiff
{
    /// <summary>
    /// Mixed content node
    /// </summary>
    public class MixedContentDiffNode : DiffNode<XmlSchemaObject>
    {
        /// <summary>
        /// Mixed content
        /// </summary>
        public override string FriendlyName
        {
            get
            {
                return "<MixedContent>";
            }
        }
        /// <summary>
        /// Full name
        /// </summary>
        public override string FullName
        {
            get
            {
                return "<MixedContent>";
            }
        }
        /// <summary>
        /// Mixed content
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return "<MixedContent>";
            }
        }

    }
}
