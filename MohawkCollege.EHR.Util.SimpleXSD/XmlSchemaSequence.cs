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
    /// The XmlSchemaSequence class represents a sequence
    /// </summary>
    [Description("xs:sequence")]
    public class XmlSchemaSequence : XmlSchemaComplexContent
    {

        /// <summary>
        /// The content
        /// </summary>
        private List<XmlSchemaObject> content = new List<XmlSchemaObject>();

        /// <summary>
        /// Get the contents of this sequence
        /// </summary>
        [Browsable(false)]
        public List<XmlSchemaObject> Content
        {
            get { return content; }
        }

        /// <summary>
        /// Register an element
        /// </summary>
        /// <param name="e">The element to register</param>
        protected void RegisterElement(System.Xml.Schema.XmlSchemaElement e)
        {
            if (content == null) content = new List<XmlSchemaObject>();

            XmlSchemaElement ele = new XmlSchemaElement(Schema, this);
            ele.Load(e);
            content.Add(ele);

        }

        /// <summary>
        /// Register a sequence
        /// </summary>
        /// <param name="e">The sequence to register</param>
        protected void RegisterSequence(System.Xml.Schema.XmlSchemaSequence e)
        {
            if (content == null) content = new List<XmlSchemaObject>();

            XmlSchemaSequence seq = new XmlSchemaSequence(Schema, this);
            seq.Load(e);
            content.Add(seq);
        }

        /// <summary>
        /// Register a choice
        /// </summary>
        /// <param name="c"></param>
        protected void RegisterChoice(System.Xml.Schema.XmlSchemaChoice c)
        {
            if (content == null) content = new List<XmlSchemaObject>();

            XmlSchemaChoice chce = new XmlSchemaChoice(Schema, this);
            chce.Load(c);

            // If the choice only has one element, get rid of the choice
            if (chce.Content.Count == 1)
                content.Add(chce.Content[0]);
            else
                content.Add(chce);
        }

        /// <summary>
        /// Load this schema sequence from the XmlSchemaSequence object
        /// </summary>
        /// <param name="s">The sequence object to load</param>
        public void Load(System.Xml.Schema.XmlSchemaSequence s)
        {
            if (s == null) return;

            // Iterate through all
            foreach (System.Xml.Schema.XmlSchemaObject o in s.Items)
            {
                if (o is System.Xml.Schema.XmlSchemaElement)
                    RegisterElement(o as System.Xml.Schema.XmlSchemaElement);
                else if (o is System.Xml.Schema.XmlSchemaChoice)
                    RegisterChoice(o as System.Xml.Schema.XmlSchemaChoice);
                else if (o is System.Xml.Schema.XmlSchemaSequence)
                    RegisterSequence(o as System.Xml.Schema.XmlSchemaSequence);
                else if (o is System.Xml.Schema.XmlSchemaGroupRef)
                    Load((o as System.Xml.Schema.XmlSchemaGroupRef).Particle as System.Xml.Schema.XmlSchemaSequence);
                else if (o is System.Xml.Schema.XmlSchemaAny)
                    RegisterAny(o as System.Xml.Schema.XmlSchemaAny);
            }

        }

        private void RegisterAny(System.Xml.Schema.XmlSchemaAny xmlSchemaAny)
        {
            if (content == null) content = new List<XmlSchemaObject>();

            XmlSchemaAny any = new XmlSchemaAny(Schema, this);
            content.Add(any);
        }

        public XmlSchemaSequence(XmlSchemaSet schema, XmlSchemaObject parent) : base(schema, parent) { }

        public override int Length
        {
            get { return Content.Count; }
        }

        public override IEnumerator<XmlSchemaObject> GetEnumerator()
        {
            return (IEnumerator<XmlSchemaObject>)(content == null ? null : (Object)content.GetEnumerator());
        }
    }
}
