using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MohawkCollege.EHR.Util.SimpleXSD
{
    /// <summary>
    /// Represents an element whereby anything can be placed
    /// </summary>
    [Description("xs:any")]
    public class XmlSchemaAny : XmlSchemaObject
    {
        /// <summary>
        /// Create a new instance of the XmlSchemaElement class
        /// </summary>
        public XmlSchemaAny(XmlSchemaSet schema, XmlSchemaObject parent) : base(schema, parent) { }

        /// <summary>
        /// Get the name of the ANY particle
        /// </summary>
        [ReadOnly(true)]
        public override string Name
        {
            get
            {
                return "##any";
            }
            set
            {
                
            }
        }
    }
}
