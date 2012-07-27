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
