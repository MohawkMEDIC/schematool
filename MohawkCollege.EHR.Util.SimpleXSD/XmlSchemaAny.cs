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
    /// Represents an element whereby anything can be placed
    /// </summary>
    [Description("xs:any")]
    public class XmlSchemaAny : XmlSchemaObject
    {
        /// <summary>
        /// Create a new instance of the XmlSchemaElement class
        /// </summary>
        public XmlSchemaAny(XmlSchemaSet schema, XmlSchemaObject parent) : base(schema, parent) { }

        /// <summary>
        /// Get the name of the ANY particle
        /// </summary>
        [ReadOnly(true)]
        public override string Name
        {
            get
            {
                return "##any";
            }
            set
            {
                
            }
        }
    }
}
