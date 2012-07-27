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
    /// Represents the base of all XmlSchema objects
    /// </summary>
    public abstract class XmlSchemaObject
    {
        XmlSchemaObject parent;
        private string documentation = "";
        private XmlSchemaSet schema;
        private MifData mif;
        private string ns;
        private string name;
        protected System.Xml.Schema.XmlSchemaObject schemaPtr;

        /// <summary>
        /// Get the XmlSchemaObject that contains this schema object
        /// </summary>
        [Browsable(false)]
        public XmlSchemaObject Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Gets or sets the name of this type
        /// </summary>
        [ReadOnly(true)]
        public virtual String Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Get or set the namespace
        /// </summary>
        [ReadOnly(true)]
        public String Namespace
        {
            get { return ns; }
            set { ns = value; }
        }

        /// <summary>
        /// Get or set the mif data associated with this element
        /// </summary>
        [Browsable(false)]
        public MifData MifData
        {
            get { return mif; }
            set { mif = value; }
        }

        /// <summary>
        /// Gets or sets the parent schema that contains this object
        /// </summary>
        [Browsable(false)]
        public XmlSchemaSet Schema
        {
            get { return schema; }
            set { schema = value; }
        }

        /// <summary>
        /// Gets or sets the documentation of the type
        /// </summary>
        [ReadOnly(true)]
        public String Documentation
        {
            get { return documentation; }
            set { documentation = value; }
        }

        /// <summary>
        /// Creates a new instance of the XmlSchemaObject class
        /// </summary>
        /// <param name="schema">The schema that contains this type</param>
        /// <param name="parent">The parent object that contains this object</param>
        public XmlSchemaObject(XmlSchemaSet schema, XmlSchemaObject parent)
        {
            this.schema = schema;
            this.parent = parent;
        }

        /// <summary>
        /// Load the generic XmlSchemaObject attributes
        /// </summary>
        /// <param name="o">The object to load</param>
        protected void Load(System.Xml.Schema.XmlSchemaAnnotated o)
        {

            
            if(o.Annotation != null)
            {
                foreach (System.Xml.Schema.XmlSchemaObject so in o.Annotation.Items)
                    if (so is System.Xml.Schema.XmlSchemaDocumentation)                 // XS:Documentation
                    {
                        foreach (System.Xml.XmlNode nd in (so as System.Xml.Schema.XmlSchemaDocumentation).Markup)
                            if (nd.NodeType == System.Xml.XmlNodeType.Text)
                                documentation += nd.InnerText.Replace("\r\n","").Replace("\n","");
                    }
                    else if (so is System.Xml.Schema.XmlSchemaAppInfo && (so as System.Xml.Schema.XmlSchemaAppInfo).Markup.Length > 0)
                    {
                        this.mif = new MifData();
                        mif.Load(so as System.Xml.Schema.XmlSchemaAppInfo);
                    }

            }
             
        }

        public override string ToString()
        {
            return Name == null || Name.Length == 0 ? this.GetType().ToString() : this.Name;
        }
    }
}
