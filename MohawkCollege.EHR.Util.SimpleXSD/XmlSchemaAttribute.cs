/**
 * Copyright (c) 2008, Mohawk College of Applied Arts and Technology
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted 
 * provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright notice, this list of conditions 
 *       and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright notice, this list of 
 *       conditions and the following disclaimer in the documentation and/or other materials provided 
 *       with the distribution.
 *     * Neither the name of Mohawk College nor the names of its contributors may be used to endorse 
 *       or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR 
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
 * TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE. 
 * 
 * Author: Justin Fyfe

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
