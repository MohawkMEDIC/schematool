using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MohawkCollege.EHR.Util.SimpleXSD;

namespace MohawkCollege.EHR.Util.XsdDiff
{
    /// <summary>
    /// Sequence differencing node
    /// </summary>
    public class SequenceDiffNode : DiffNode<XmlSchemaSequence>
    {
        public override string FriendlyName
        {
            get
            {
                return "<Sequence>";
            }
        }
    }
}
