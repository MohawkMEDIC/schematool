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

namespace MohawkCollege.EHR.Util.SimpleXSD
{
    /// <summary>
    /// The XmlType class provides a base class to XmlSimple types and XmlComplex types
    /// </summary>
    public abstract class XmlSchemaType : XmlSchemaObject
    {
        
        private XmlSchemaType baseClass;

        /// <summary>
        /// Get or set the class this object extends
        /// </summary>
        public XmlSchemaType BaseClass
        {
            get { return baseClass; }
            set { baseClass = value; }
        }

        /// <summary>
        /// Create a new instance of the XmlSchemaType class
        /// </summary>
        public XmlSchemaType(XmlSchemaSet schema, XmlSchemaObject parent) : base(schema, parent) { }

        /// <summary>
        /// Convert this object to a string representation
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0}#{1}", Namespace, Name);
        }
    }
}
