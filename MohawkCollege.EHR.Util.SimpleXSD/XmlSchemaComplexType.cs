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
    /// The XmlSchemaComplexType class represents a complex type
    /// </summary>
    [Description("xs:complexType")]
    public class XmlSchemaComplexType : XmlSchemaType
    {
        private XmlSchemaComplexContent content;
        private List<XmlSchemaAttribute> attributes = new List<XmlSchemaAttribute>();
        private bool mixed;
        private bool abst;
        private bool compiled = false;

        /// <summary>
        /// Get or set the abstract value
        /// </summary>
        [ReadOnly(true)]
        public Boolean Abstract
        {
            get { return abst; }
        }

        /// <summary>
        /// Mixed content?
        /// </summary>
        [ReadOnly(true)]
        public Boolean Mixed
        {
            get { return mixed; }
        }

        /// <summary>
        /// Attributes in this complex type
        /// </summary>
        [Browsable(false)]
        public List<XmlSchemaAttribute> Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// Get XmlSchema Elements
        /// </summary>
        [Browsable(false)]
        public XmlSchemaComplexContent Content
        {
            get { return content; }
        }

        /// <summary>
        /// Register an XS:ALL
        /// </summary>
        /// <param name="a">The XS:All to register</param>
        protected void RegisterAll(System.Xml.Schema.XmlSchemaAll a)
        {
            
        }

        /// <summary>
        /// Register an attribute in this item's attribute array
        /// </summary>
        /// <param name="a">The attribute to register</param>
        protected void RegisterAttribute(System.Xml.Schema.XmlSchemaAttribute a)
        {
            if (attributes == null) attributes = new List<XmlSchemaAttribute>();

            // Search up parent tree to see if this attribute needs to be added
            XmlSchemaComplexType prnt = (XmlSchemaComplexType)this.BaseClass;
            while (prnt != null)
            {
                prnt.Compile();

                if (prnt.Attributes != null)
                {
                    foreach (XmlSchemaAttribute at in prnt.Attributes)
                        if (at.Name == a.Name && at.FixedValue == a.FixedValue && a.Use != System.Xml.Schema.XmlSchemaUse.Prohibited) // Found so we don't need it
                            return;
                }
                prnt = (XmlSchemaComplexType)prnt.BaseClass;
            }


            // Now load the attribute
            XmlSchemaAttribute att = new XmlSchemaAttribute(Schema, this);
            att.Load(a);
            attributes.Add(att);
        }

        /// <summary>
        /// Register an XS:Sequence
        /// </summary>
        /// <param name="s">The XS:Sequence to register</param>
        protected void RegisterSequence(System.Xml.Schema.XmlSchemaSequence s)
        {
            XmlSchemaSequence seq = new XmlSchemaSequence(Schema, this);
            seq.Load(s);

            List<Object> gc = new List<object>();

            // Clean sequence 
            foreach (XmlSchemaObject o in seq.Content)
            {
                XmlSchemaComplexType prnt = (XmlSchemaComplexType)this.BaseClass;
                while (prnt != null)
                {
                    prnt.Compile();

                    if (prnt.Content != null)
                    {
                        foreach (XmlSchemaObject ot in prnt.Content)
                            if (o.GetType() == ot.GetType() && o.Name == ot.Name && ot.Namespace == o.Namespace &&
                                (o as XmlSchemaComplexContent).MinOccurs == (ot as XmlSchemaComplexContent).MinOccurs &&
                                (o as XmlSchemaComplexContent).MaxOccurs == (ot as XmlSchemaComplexContent).MaxOccurs) // Found so we don't need it
                                gc.Add(o);
                            else if (o.GetType() == ot.GetType() && o.Name == ot.Name && ot.Namespace == o.Namespace && o.MifData == null) // Copy mif data as it may be useful (this is most likely a restriction)
                                o.MifData = ot.MifData;
                    }
                    prnt = (XmlSchemaComplexType)prnt.BaseClass;
                }
            }

            // Clean garbage
            foreach (Object o in gc) seq.Content.Remove(o as XmlSchemaObject);

            content = seq;
        }

        /// <summary>
        /// Register an XS:Choice
        /// </summary>
        /// <param name="c">The XS:Choice to register</param>
        protected void RegisterChoice(System.Xml.Schema.XmlSchemaChoice c)
        {
            XmlSchemaChoice chce = new XmlSchemaChoice(Schema, this);
            chce.Load(c);
            content = chce;
        }

        /// <summary>
        /// Register content in this type
        /// </summary>
        /// <param name="c">The content to register</param>
        protected void RegisterContent(System.Xml.Schema.XmlSchemaParticle c)
        {
            if (c == null) return;

            if (c is System.Xml.Schema.XmlSchemaSequence)
                RegisterSequence(c as System.Xml.Schema.XmlSchemaSequence);
            else if (c is System.Xml.Schema.XmlSchemaChoice)
                RegisterChoice(c as System.Xml.Schema.XmlSchemaChoice);
            else if (c is System.Xml.Schema.XmlSchemaAll)
                RegisterAll(c as System.Xml.Schema.XmlSchemaAll) ;
            else if (c is System.Xml.Schema.XmlSchemaGroupRef) // TODO: Xml Group References!!!
                ;

        }

        /// <summary>
        /// Register restriction in this type
        /// </summary>
        /// <param name="c">The content to register</param>
        protected void RegisterRestriction(System.Xml.Schema.XmlSchemaParticle c)
        {
            if (c == null) return;

            // Restriction means we need to override some elements
            if (c is System.Xml.Schema.XmlSchemaSequence)
                RegisterSequence(c as System.Xml.Schema.XmlSchemaSequence);
            else if (c is System.Xml.Schema.XmlSchemaChoice)
                RegisterChoice(c as System.Xml.Schema.XmlSchemaChoice);
            else if (c is System.Xml.Schema.XmlSchemaAll)
                RegisterAll(c as System.Xml.Schema.XmlSchemaAll) ;
            else if (c is System.Xml.Schema.XmlSchemaGroupRef) // TODO: Xml Group References!!!
                ;

        }

        /// <summary>
        /// Create a new instance of the XmlSchemaComplexType type
        /// </summary>
        public XmlSchemaComplexType(XmlSchemaSet schema, XmlSchemaObject parent) : base(schema, parent) { }

        /// <summary>
        /// Load this Complex type from the .NET schema
        /// </summary>
        /// <param name="type">The type to load</param>
        public void Load(System.Xml.Schema.XmlSchemaComplexType type)
        {
            base.Load(type);

            this.Name = type.Name ?? type.QualifiedName.Name;
            this.Namespace = !type.QualifiedName.IsEmpty ? type.QualifiedName.Namespace : Schema.TargetNamespace;
            schemaPtr = type;
            this.mixed = type.IsMixed;
            this.abst = type.IsAbstract;

            // Find base class
            if (type.BaseXmlSchemaType != null && type.BaseXmlSchemaType.Name != null)
                BaseClass = Schema.FindType(type.BaseXmlSchemaType.Name);

        }

        /// <summary>
        /// Compile this type
        /// </summary>
        public void Compile()
        {
            if (compiled) return;

            System.Xml.Schema.XmlSchemaComplexType type = (schemaPtr as System.Xml.Schema.XmlSchemaComplexType);

            // Load attributes (HL7 schemas like AttributeUses
            foreach (System.Xml.Schema.XmlSchemaObject o in type.AttributeUses.Values)
            {
                if (o is System.Xml.Schema.XmlSchemaAttributeGroup)
                    foreach (System.Xml.Schema.XmlSchemaObject xso in (o as System.Xml.Schema.XmlSchemaAttributeGroup).Attributes)
                        RegisterAttribute(xso as System.Xml.Schema.XmlSchemaAttribute);
                else if (o is System.Xml.Schema.XmlSchemaAttribute && (o as System.Xml.Schema.XmlSchemaAttribute).Name != null)
                    RegisterAttribute(o as System.Xml.Schema.XmlSchemaAttribute);
            }

            // Load Complex Elements
            if (type.Particle != null)
                RegisterContent(type.Particle);
            else if (type.ContentModel is System.Xml.Schema.XmlSchemaComplexContent)
            {
                // Correct base class if possible
                if (BaseClass == null && type.ContentModel.Content is System.Xml.Schema.XmlSchemaComplexContentExtension) // Determine base class
                    this.BaseClass = Schema.FindType((type.ContentModel.Content as System.Xml.Schema.XmlSchemaComplexContentExtension).BaseTypeName.Name);

                // Go through the particle(s)
                if (type.ContentModel.Content is System.Xml.Schema.XmlSchemaComplexContentExtension)
                    RegisterContent((type.ContentModel.Content as System.Xml.Schema.XmlSchemaComplexContentExtension).Particle);
                else if (type.ContentModel.Content is System.Xml.Schema.XmlSchemaComplexContentRestriction)
                    RegisterRestriction((type.ContentModel.Content as System.Xml.Schema.XmlSchemaComplexContentRestriction).Particle);

                // If sequence is null then create an empty one
                if (Content == null)
                    content = new XmlSchemaSequence(Schema, this);
            }
            else
            {
                // Simple content
            }

            compiled = true;
        }

        public int ContentLength
        {
            get { return (content == null ? 0 : Content.Length) + (Attributes == null ? 0 : Attributes.Count); }
        }

        /// <summary>
        /// Count the number of common attributes/elements with another type
        /// </summary>
        /// <param name="t">The type to find commonality with</param>
        /// <returns>The number of common attributes and elements</returns>
        public int Commonality(XmlSchemaComplexType t)
        {
            int total = 0;

            foreach (XmlSchemaAttribute ao in this.Attributes)
                foreach (XmlSchemaAttribute ai in t.Attributes)
                    if (ao.SchemaType == ai.SchemaType && ao.Name == ai.Name && ao.FixedValue == ai.FixedValue && ao.Required == ai.Required)
                    {
                        total++; // Count this as a match if name, type and fixed value (if any) are identical
                        break;
                    }
                
            // Content
            if (Content is XmlSchemaSequence && t.Content is XmlSchemaSequence)
            {
                // Iterate through 
                foreach(XmlSchemaObject oo in (Content as XmlSchemaSequence).Content)
                    foreach(XmlSchemaObject oi in (t.Content as XmlSchemaSequence).Content)
                        if (oo.GetType() == oi.GetType()) // Match the system.type (ie: Sequence and Sequence)
                        {
                            try
                            {
                                if (oo is XmlSchemaElement && oo.Name == oi.Name && 
                                    ((oo as XmlSchemaElement).SchemaType != null && (oi as XmlSchemaElement).SchemaType != null ||
                                    (oo as XmlSchemaElement).SchemaType == null && (oi as XmlSchemaElement).SchemaType == null)
                                    &&
                                    (((oo as XmlSchemaElement).SchemaType == (oi as XmlSchemaElement).SchemaType) ||
                                    ((oo as XmlSchemaElement).SchemaType as XmlSchemaComplexType).Commonality(((oi as XmlSchemaElement).SchemaType as XmlSchemaComplexType)) == ((oi as XmlSchemaElement).SchemaType as XmlSchemaComplexType).ContentLength) &&
                                    (oo as XmlSchemaElement).MinOccurs == (oi as XmlSchemaElement).MinOccurs && (oo as XmlSchemaElement).MaxOccurs == (oi as XmlSchemaElement).MaxOccurs)
                                {
                                    total++;
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                e = e;
                            }
                        }
            }
            else if (Content is XmlSchemaChoice && t.Content is XmlSchemaChoice)
            {
                Debugger.Break();
            }

            return total;
        }
    }
}
