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
