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


namespace MohawkCollege.EHR.Util.SimpleXSD
{
    /// <summary>
    /// Delegate used when the XmlSchem object wishes to update listeners as to the current status of 
    /// loading
    /// </summary>
    public delegate void XmlSchemaLoadingHandler(object Sender, float perc);

    /// <summary>
    /// The XmlSchema object contains all of the XmlTypes represented by a .NET XmlSchemaCollection object
    /// </summary>
    public class XmlSchemaSet
    {

        private List<string> namespaces = new List<string>();

        /// <summary>
        /// Compare two xml schemas for sorting
        /// </summary>
        private class XmlSchemaComparison : IComparer<XmlSchemaType>
        {
            #region IComparer<XmlSchemaType> Members

            public int Compare(XmlSchemaType x, XmlSchemaType y)
            {
                if (x.Name == null)
                    return 0;
                else if (y.Name == null)
                    return 1;
                else
                    return x.Name.CompareTo(y.Name);
            }

            #endregion
        }

        private List<XmlSchemaType> types;
        private bool typesDirty = false;
        private List<XmlSchemaElement> elements;

        private System.Xml.Schema.XmlSchemaSet ss = new System.Xml.Schema.XmlSchemaSet(); // Used for reading from the XSD
         
        // The target namespace of the schema
        private string targetNamespace;

        /// <summary>
        /// Get the types included in the XmlSchema
        /// </summary>
        public List<XmlSchemaType> Types
        {
            get { return types; }
        }

        /// <summary>
        /// Create the Elements
        /// </summary>
        public List<XmlSchemaElement> Elements
        {
            get { return elements; }
        }

        /// <summary>
        /// Get or set the name of the target namespace
        /// </summary>
        public String TargetNamespace
        {
            get { return targetNamespace; }
            set { targetNamespace = value; }
        }

      
        /// <summary>
        /// Create a type in the schema object
        /// </summary>
        /// <param name="type">The type of schema to register</param>
        /// <param name="Name">The Name of the type</param>
        internal void CreateType(System.Xml.Schema.XmlSchemaType type, XmlQualifiedName Name)
        {
            if (Name == null || type == null) 
                return;

            type.Name = Name.Name;
            
            // Copy target namespace(s)
            foreach (System.Xml.XmlQualifiedName s in type.Namespaces.ToArray())
                if (!namespaces.Contains(s.Name) && s.Name != null)
                    namespaces.Add(s.Name);

            if (type is System.Xml.Schema.XmlSchemaComplexType)
                RegisterComplexType((System.Xml.Schema.XmlSchemaComplexType)type);
            else
                RegisterSimpleType((System.Xml.Schema.XmlSchemaSimpleType)type);
        }

        /// <summary>
        /// Register a complex type in this schema object
        /// </summary>
        /// <param name="type">The complex type to register</param>
        protected void RegisterComplexType(System.Xml.Schema.XmlSchemaComplexType type)
        {
            if (types == null) types = new List<XmlSchemaType>();

            if (FindType(type.Name) != null) return; // Already registered

            // Now create a type
            XmlSchemaComplexType cplx = new XmlSchemaComplexType(this, null);
            cplx.Load(type);

            // Finally, add
            types.Add(cplx);
            typesDirty = true;
        }

        /// <summary>
        /// Register a simple type in this schema object
        /// </summary>
        /// <param name="type">The simple type to register</param>
        protected void RegisterSimpleType(System.Xml.Schema.XmlSchemaSimpleType type)
        {
            if (types == null) types = new List<XmlSchemaType>();

            if (FindType(type.Name) != null) return; // Already registered

            // Now create a type
            XmlSchemaSimpleType simp = new XmlSchemaSimpleType(this, null);
            simp.Load(type);

            // Finally, add
            types.Add(simp);
            typesDirty = true;
        }

        /// <summary>
        /// Register an element with this schema
        /// </summary>
        /// <param name="element">The element to register</param>
        protected void RegisterElement(System.Xml.Schema.XmlSchemaElement element)
        {
            if (elements == null) elements = new List<XmlSchemaElement>();

            // Look to see if element is already registered
            foreach (XmlSchemaElement e in elements)
                if (e.Name == element.Name) return;

            XmlSchemaElement ele = new XmlSchemaElement(this, null);
            ele.Load(element);

            elements.Add(ele);

        }


        /// <summary>
        /// Find an XmlSchemaType located within this XmlSchema
        /// </summary>
        /// <param name="TypeName">The name of the type to locate</param>
        /// <returns>The located type</returns>
        public XmlSchemaType FindType(String TypeName)
        {

            if (TypeName == null || types.Count == 0) return null;

            // Data needs to be in alpha order
            if (typesDirty)
            {
                types.Sort(new XmlSchemaComparison());
                typesDirty = false;
            }

            int n = types.Count;
            int i = n / 2;
            int lBound = 0;
            int last = i == 0 ? 1 : 0;

            // Basic Binary Search of the types
            while (i != last && types.Count > i)
            {
                // Clean useless type
                if (types[i].Name == null)
                    types.RemoveAt(i);

                last = i;

                if (i >= types.Count ) break;

                if (TypeName.CompareTo(types[i].Name) > 0)
                {
                    lBound = i;
                    i += (n - i) / 2;
                }
                else if (TypeName.CompareTo(types[i].Name) < 0)
                {
                    n = i;
                    i -= (int)Math.Round(((i - lBound) / 2.0f) + 0.1f, MidpointRounding.AwayFromZero);
                    
                }
                else if (TypeName == types[i].Name)
                    return types[i];
                else
                    lBound = i;
            }

            return null;
        }

        /// <summary>
        /// Add a schema to this schema loader
        /// </summary>
        /// <param name="SchemaLocation">The schemalocation to load</param>
        public void Add(String SchemaLocation)
        {
            try
            {
                XmlReaderSettings set = new XmlReaderSettings() { ProhibitDtd = false };
                ss.Add(null, XmlReader.Create(SchemaLocation, set));
            }
            catch (Exception e)
            {
                throw new System.Xml.Schema.XmlSchemaException(String.Format("Can not load the schema '{0}' - {1}", SchemaLocation, e.Message), e);
            }
        }

        /// <summary>
        /// Add a schema to the schema set
        /// </summary>
        public void Add(XmlSchema xsd)
        {
            try
            {
                ss.Add(xsd);
            }
            catch (Exception e)
            {
                throw new XmlSchemaException("Can't add schema to the collection", e);
            }
        }

        
        /// <summary>
        /// Load the schema from an XSD into this XmlSchema object
        /// </summary>
        /// <param name="SchemaLocation">The location of the schema to import</param>
        public void Load()
        {
            // Load the schema into the schema set
            try
            {
                
                ss.Compile();

                // Iterate through types and create them
                int i = 0;
                namespaces.Clear(); // clear namespaces
                
                // Determine target namespace
                string targetNs = "";
                foreach (System.Xml.Schema.XmlSchema s in ss.Schemas())
                    if (targetNs != s.TargetNamespace && s.TargetNamespace != null)
                    {
                        targetNs = s.TargetNamespace;
                        break;
                    }
                
                targetNamespace = targetNs;

                foreach (Object o in ss.GlobalTypes.Values)
                {
                    if (o is System.Xml.Schema.XmlSchemaComplexType)
                        RegisterComplexType((System.Xml.Schema.XmlSchemaComplexType)o);
                    else if (o is System.Xml.Schema.XmlSchemaSimpleType)
                        RegisterSimpleType((System.Xml.Schema.XmlSchemaSimpleType)o);

                    // Alert for update
                    if (Loading != null) 
                        Loading(this, ((float)i / ((float)ss.GlobalTypes.Count * 2)));
                    i++;
                }

                // Iterate through root elements and create them
                foreach (Object o in ss.GlobalElements.Values)
                    if (o is System.Xml.Schema.XmlSchemaElement)
                        RegisterElement((System.Xml.Schema.XmlSchemaElement)o);

                // Iterate through types and compile them
                for (int p = 0; p < Types.Count; p++)
                {
                    XmlSchemaType t = Types[p];

                    if (t is XmlSchemaComplexType)
                        (t as XmlSchemaComplexType).Compile();

                    // Alert for update
                    if (Loading != null) Loading(this, ((float)i / ((float)Types.Count * 2)));
                    i++;
                }

                types.Sort(new XmlSchemaComparison());

            }
            catch (Exception e)
            {
                throw new System.Xml.Schema.XmlSchemaException(String.Format("Can not compile the schemas - {0}", e.Message), e);
            }
        }

        /// <summary>
        /// Fires when the schema wishes to update listers of the status of loading
        /// </summary>
        public event XmlSchemaLoadingHandler Loading;
    }
}
