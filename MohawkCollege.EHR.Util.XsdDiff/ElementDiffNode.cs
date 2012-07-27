using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MohawkCollege.EHR.Util.SimpleXSD;

namespace MohawkCollege.EHR.Util.XsdDiff
{
    /// <summary>
    /// Diff node specialized for an element
    /// </summary>
    public class ElementDiffNode : DiffNode<XmlSchemaElement>
    {
        /// <summary>
        /// The display name
        /// </summary>
        public override string DisplayName
        {
            get
            {
                string retVal = "";
                if (Type == DiffType.None && Original.SchemaType != null || Compared == null && Original != null && Original.SchemaType != null) // No difference
                    retVal = string.Format("{0} ({1})", Original.Name, Original.SchemaType.Name);
                else if (Original != null && Compared != null && Original.SchemaType != null && Compared.SchemaType != null)
                    retVal = string.Format("{0} ({1}/{2})", Original.Name, Original.SchemaType.Name, Compared.SchemaType.Name);
                else if (Compared != null && Compared.SchemaType != null)
                    retVal = string.Format("{0} ({1})", Compared.Name, Compared.SchemaType.Name);
                else
                    retVal = FriendlyName;
                return retVal;
            }
        }

        /// <summary>
        /// Calculate difference between the two nodes
        /// </summary>
        public override void CalculateDiff()
        {
            // Call base
            base.CalculateDiff();

            // Did the type change?
            if ((Type == DiffType.None || Type == DiffType.ChildDifferent)
                && Original != null && Compared != null &&
                Original.SchemaType != null && Compared.SchemaType != null &&
                !Original.SchemaType.Name.Equals(Compared.SchemaType.Name))
            {
                Type = DiffType.TypeChanged;
                ChangeDetails = new string[] { 
                    Original.SchemaType.Name, 
                    Compared.SchemaType.Name
                };
            }

        }
    }
}
