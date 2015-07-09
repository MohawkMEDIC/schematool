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
    /// The XmlSchemaComplexContent class represents content that can belong within the body of a complex type
    /// </summary>
    public abstract class XmlSchemaComplexContent : XmlSchemaObject
    {
        private string minOccurs = "0";
        private string maxOccurs = "1";

        [Browsable(false)]
        public abstract int Length { get; }

        /// <summary>
        /// Get or set the minimum occurences of the element
        /// </summary>
        [ReadOnly(true)]
        public string MinOccurs
        {
            get { return minOccurs; }
            set { minOccurs = value; }
        }

        /// <summary>
        /// Get or set the maximum occurences of the element
        /// </summary>
        [ReadOnly(true)]
        public string MaxOccurs
        {
            get { return maxOccurs; }
            set { maxOccurs = value; }
        }

        public abstract IEnumerator<XmlSchemaObject> GetEnumerator();

        public XmlSchemaComplexContent(XmlSchemaSet schema, XmlSchemaObject parent) : base(schema, parent) { }
    }
}
