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
