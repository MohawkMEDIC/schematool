using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MohawkCollege.EHR.Util.SimpleXSD;

namespace MohawkCollege.EHR.Util.XsdDiff
{
    public class ChoiceDiffNode : DiffNode<XmlSchemaChoice>
    {
        /// <summary>
        /// Override display name
        /// </summary>
        public override string FriendlyName
        {
            get
            {
                return "<Choice>";
            }
        }

        /// <summary>
        /// Calculate if any of the children have changed
        /// </summary>
        public override void CalculateDiff()
        {
            base.CalculateDiff();
            if (Type == DiffType.None && Original != null && Compared != null)
            {
                int nOriginal = Original.Content == null ? 0 : Original.Content.Count,
                    nCompared = Compared.Content == null ? 0 : Compared.Content.Count;
                if (nOriginal != nCompared) // No match, children have changed
                    Type = DiffType.ChildDifferent;
                else if(Children != null)// are each of the child elements the same
                {
                    var differentChildren = from child in Children
                                            where child.Type != DiffType.None
                                            select child;
                    if (differentChildren.Count() > 0) Type = DiffType.ChildDifferent;
                }

            }
        }
    }
}
