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
using System.Diagnostics;
using System.ComponentModel;

namespace MohawkCollege.EHR.Util.SimpleXSD
{
    /// <summary>
    /// The XmlSchemaSimpleType represents a simple type in the XSD
    /// </summary>
    [Description("xs:simpleType")]
    public class XmlSchemaSimpleType : XmlSchemaType
    {
        private Type sysType;
        private List<String> restrictions = new List<String>();
        private System.Xml.Schema.XmlTypeCode systemTypeCode;

        /// <summary>
        /// Get the system type code
        /// </summary>
        public System.Xml.Schema.XmlTypeCode SystemTypeCode
        {
            get
            {
                return systemTypeCode;
            }
        }

        /// <summary>
        /// Type of this type
        /// </summary>
        public enum SimpleType
        {
            Enum,
            Extension
        }

        private SimpleType type = SimpleType.Extension;

        /// <summary>
        /// Get or set restrictions on values for this type
        /// </summary>
        public List<String> Restrictions
        {
            get { return restrictions; }
            set { restrictions = value; }
        }

        /// <summary>
        /// Get or set the type of this simple type
        /// </summary>
        public SimpleType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Get or set the base type code
        /// </summary>
        public  Type SystemType
        {
            get { return sysType; }
            set { sysType = value; }
        }

        /// <summary>
        /// Create a new instance of the XmlSchemaSimpleType object
        /// </summary>
        public XmlSchemaSimpleType(XmlSchemaSet schema, XmlSchemaObject parent) : base(schema, parent) { }

        /// <summary>
        /// Load this Simple type from the .NET schema
        /// </summary>
        /// <param name="type">The type to load</param>
        public void Load(System.Xml.Schema.XmlSchemaSimpleType type)
        {
            if (type == null)
                return;
            this.Name = type.Name;
            this.Namespace = type.QualifiedName != null ? type.QualifiedName.Namespace : Schema.TargetNamespace;
            this.SystemType = type.Datatype.ValueType;
            this.systemTypeCode = type.Datatype.TypeCode;
            this.Type = SimpleType.Extension;

            // Deal with an enum
            if (type.Content is System.Xml.Schema.XmlSchemaSimpleTypeList)
            {
                Type = SimpleType.Enum;

                System.Xml.Schema.XmlSchemaSimpleTypeList stl = type.Content as System.Xml.Schema.XmlSchemaSimpleTypeList;

                this.SystemType = stl.BaseItemType.Datatype.ValueType;
                this.systemTypeCode = stl.BaseItemType.Datatype.TypeCode;

                System.Xml.Schema.XmlSchemaObjectCollection listitems = null;

                if (stl.ItemType != null && stl.ItemType.Content is System.Xml.Schema.XmlSchemaSimpleTypeRestriction) // Restriction meaning this is an enum
                    listitems = (stl.ItemType.Content as System.Xml.Schema.XmlSchemaSimpleTypeRestriction).Facets;
                else if (stl.BaseItemType.Content is System.Xml.Schema.XmlSchemaSimpleTypeUnion)
                {
                    listitems = new System.Xml.Schema.XmlSchemaObjectCollection();
                    foreach(System.Xml.Schema.XmlSchemaObject so in (stl.BaseItemType.Content as System.Xml.Schema.XmlSchemaSimpleTypeUnion).BaseMemberTypes)
                        if (so is System.Xml.Schema.XmlSchemaSimpleType)
                        {
                            foreach (System.Xml.Schema.XmlSchemaObject o in ((so as System.Xml.Schema.XmlSchemaSimpleType).Content as System.Xml.Schema.XmlSchemaSimpleTypeRestriction).Facets)
                                listitems.Add(o);
                        }
                }

                // TODO: Can lists appear somewhere else?

                // Iterate through the restrictions
                if(listitems != null)
                    foreach(System.Xml.Schema.XmlSchemaObject o in listitems)
                        if(o is System.Xml.Schema.XmlSchemaEnumerationFacet)
                            Restrictions.Add((o as System.Xml.Schema.XmlSchemaEnumerationFacet).Value);

            }

            //Debugger.Break();
        }


    }
}
