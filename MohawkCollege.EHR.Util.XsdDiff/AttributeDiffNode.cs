﻿/**
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
using System.Linq;
using System.Text;
using MohawkCollege.EHR.Util.SimpleXSD;

namespace MohawkCollege.EHR.Util.XsdDiff
{
    /// <summary>
    /// Diff node for an XmlSchemaAttribute
    /// </summary>
    public class AttributeDiffNode : DiffNode<XmlSchemaAttribute>
    {
        /// <summary>
        /// The display name
        /// </summary>
        public override string DisplayName
        {
            get
            {
                if (Type == DiffType.None && Original.SchemaType != null || Compared == null && Original != null && Original.SchemaType != null) // No difference
                    return string.Format("{0} ({1})", Original.Name, Original.SchemaType.Name);
                else if (Original != null && Compared != null && Original.SchemaType != null && Compared.SchemaType != null)
                    return string.Format("{0} ({1}/{2})", Original.Name, Original.SchemaType.Name, Compared.SchemaType.Name);
                else if (Compared != null && Compared.SchemaType != null)
                    return string.Format("{0} ({1})", Compared.Name, Compared.SchemaType.Name);
                else
                    return FriendlyName;
            }
        }

        /// <summary>
        /// Calculate difference between the two nodes
        /// </summary>
        public override void CalculateDiff()
        {
            // Call base
            base.CalculateDiff();

            // Did the type change?
            if ((Type == DiffType.None || Type == DiffType.ChildDifferent)
                && Original != null && Compared != null &&
                Original.SchemaType != null && Compared.SchemaType != null &&
                Original.SchemaType.Name != Compared.SchemaType.Name)
            {
                Type = DiffType.TypeChanged;
                ChangeDetails = new string[] { 
                    Original.SchemaType.Name, 
                    Compared.SchemaType.Name
                };
            }
            else if(Type == DiffType.None && Original != null && Compared != null &&
                Original.FixedValue != null && Compared.FixedValue != null &&
                !Original.FixedValue.Equals(Compared.FixedValue))
            {
                Type = DiffType.FixedChanged;
                ChangeDetails = new string[] {
                    Original.FixedValue, 
                    Compared.FixedValue
                };
            }

        }
    }
}
