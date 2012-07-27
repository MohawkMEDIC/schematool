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
using System.Xml.Schema;
using System.Xml;
using System.ComponentModel;

namespace MohawkCollege.EHR.Util.SimpleXSD
{
    /// <summary>
    /// Represents important data about an object according to the HL7v3 MIF
    /// </summary>
    public class MifData
    {
        /// <summary>
        /// Mif Conformance levels
        /// </summary>
        public enum MifConformance
        {
            O,
            P,
            R,
            M
        }

        private string businessName;
        private string className;
        private string documentation;
        private MifConformance conformance;

        /// <summary>
        /// Get the conformance level (as described by the MIF)
        /// </summary>
        [ReadOnly(true)]
        public MifConformance Conformance
        {
            get { return conformance; }
        }

        /// <summary>
        /// Gets or sets the business name of the object
        /// </summary>
        [ReadOnly(true)]
        public string BusinessName
        {
            get { return businessName; }
            set { businessName = value; }
        }

        /// <summary>
        /// Gets or sets the name of the class
        /// </summary>
        [ReadOnly(true)]
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        /// <summary>
        /// Gets or sets the MIF documentation of the class
        /// </summary>
        [ReadOnly(true)]
        public string Documentation
        {
            get { return documentation; }
            set { documentation = value; }
        }

        /// <summary>
        /// Load the MIF data from a .NET XmlSchemaAnnotation object
        /// </summary>
        /// <param name="annotation">The .NET XmlSchemaAnnotation object to load the MIF data from</param>
        public void Load(System.Xml.Schema.XmlSchemaAppInfo annotation)
        {
           
            // Attempt to load
            XmlNode mifAttributeNode = null;
            foreach (XmlNode nd in annotation.Markup)
                if ((nd.LocalName == "attribute" || nd.LocalName == "class" || nd.LocalName == "targetConnection") && nd.NamespaceURI == "urn:hl7-org:v3/mif")
                    mifAttributeNode = nd;

            // Mif attribute found?
            if (mifAttributeNode == null) return;

            if (mifAttributeNode.LocalName == "attribute" || mifAttributeNode.LocalName == "targetConnection") // Attribute style MIF
            {
                this.conformance = mifAttributeNode.Attributes["conformance"] != null ? (MifConformance)Enum.Parse(typeof(MifConformance), mifAttributeNode.Attributes["conformance"].Value) : MifConformance.O;
                if (mifAttributeNode.Attributes["isMandatory"] != null && mifAttributeNode.Attributes["isMandatory"].Value == "true")
                    this.conformance = MifConformance.M;
            }

            // Process child elements
            if (mifAttributeNode.SelectSingleNode("//*[local-name() = 'businessName']/@name") != null)
            {
                businessName = mifAttributeNode.SelectSingleNode("//*[local-name() = 'businessName']/@name").Value.Replace("*", "");
                if(businessName.IndexOf(":") > -1) businessName = businessName.Substring(businessName.IndexOf(":")+1);
            }

            // Definition
            if (mifAttributeNode.SelectSingleNode("//*[local-name() = 'definition']") != null)
                this.documentation = mifAttributeNode.SelectSingleNode("//*[local-name() = 'annotations']/*[local-name() = 'definition']").InnerText;

        }
    }
}
