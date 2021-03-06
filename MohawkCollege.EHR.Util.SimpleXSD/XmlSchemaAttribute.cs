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
using System.Text;
using System.ComponentModel;

namespace MohawkCollege.EHR.Util.SimpleXSD
{
    /// <summary>
    /// The XmlSchemaAttribute represents an XS:Attribute item
    /// </summary>
    [Description("xs:attribute")]
    public class XmlSchemaAttribute : XmlSchemaObject
    {

        /// <summary>
        /// Create a new instance of the XmlSchemaAttribute class
        /// </summary>
        public XmlSchemaAttribute(XmlSchemaSet schema, XmlSchemaObject parent) : base(schema, parent) { }

        // Attribute type
        private XmlSchemaSimpleType attType;
        private bool required = false;
        private bool prohibited = false;
        private string fixedValue = null;

        /// <summary>
        /// True if use of this attribute is prohibited
        /// </summary>
        [ReadOnly(true)]
        public Boolean Prohibited
        {
            get { return prohibited; }
        }

        /// <summary>
        /// Get if this attribute is fixed
        /// </summary>
        [ReadOnly(true)]
        public String FixedValue
        {
            get { return fixedValue; }
        }

        /// <summary>
        /// Gets or sets the type of attribute
        /// </summary>
        [ReadOnly(true)]
        public XmlSchemaSimpleType SchemaType
        {
            get { return attType; }
            set { attType = value; }
        }

        /// <summary>
        /// Get or set the requiredness of the attribute
        /// </summary>
        [ReadOnly(true)]
        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        /// <summary>
        /// Load the schema attribute
        /// </summary>
        /// <param name="a">The attribute to load</param>
        public void Load(System.Xml.Schema.XmlSchemaAttribute a)
        {
            base.Load(a);

            // First setup name, optionality, etc.
            this.Name = a.Name;
            this.Namespace = a.QualifiedName != null ? a.QualifiedName.Namespace : Schema.TargetNamespace;
            this.Required = (a.Use == System.Xml.Schema.XmlSchemaUse.Required);
            this.prohibited = (a.Use == System.Xml.Schema.XmlSchemaUse.Prohibited);
            this.fixedValue = a.FixedValue;

            // Setup the type
            //this.attType = System.Xml.Schema.XmlSchemaType. a.SchemaTypeName.Name
            if (!a.SchemaTypeName.IsEmpty)
                this.attType = Schema.FindType(a.SchemaTypeName.Name) as XmlSchemaSimpleType;
            else
            {
                // Look for a built in type
                
                SchemaType = new XmlSchemaSimpleType(Schema, this);
                SchemaType.Load(a.SchemaType);
            }
        }

    }
}
