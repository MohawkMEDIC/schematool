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
using System.Linq;
using System.Text;
using MohawkCollege.EHR.Util.SimpleXSD;

namespace MohawkCollege.EHR.Util.XsdDiff
{
    public class ChoiceDiffNode : DiffNode<XmlSchemaChoice>
    {
        /// <summary>
        /// Override display name
        /// </summary>
        public override string FriendlyName
        {
            get
            {
                return "<Choice>";
            }
        }

        /// <summary>
        /// Calculate if any of the children have changed
        /// </summary>
        public override void CalculateDiff()
        {
            base.CalculateDiff();
            if (Type == DiffType.None && Original != null && Compared != null)
            {
                int nOriginal = Original.Content == null ? 0 : Original.Content.Count,
                    nCompared = Compared.Content == null ? 0 : Compared.Content.Count;
                if (nOriginal != nCompared) // No match, children have changed
                    Type = DiffType.ChildDifferent;
                else if(Children != null)// are each of the child elements the same
                {
                    var differentChildren = from child in Children
                                            where child.Type != DiffType.None
                                            select child;
                    if (differentChildren.Count() > 0) Type = DiffType.ChildDifferent;
                }

            }
        }
    }
}
