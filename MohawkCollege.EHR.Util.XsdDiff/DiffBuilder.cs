using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MohawkCollege.EHR.Util.SimpleXSD;
using System.Diagnostics;

namespace MohawkCollege.EHR.Util.XsdDiff
{
    /// <summary>
    /// Build Diff trees
    /// </summary>
    public class DiffBuilder
    {

        public bool TreatSequenceAsNode { get; set; }

        /// <summary>
        /// Event fires to return the status of the comparison operation
        /// </summary>
        public event XmlSchemaLoadingHandler Comparing;

        private Dictionary<String, IDiffNode> currentDiffNodes = new Dictionary<string, IDiffNode>();

        /// <summary>
        /// Build a tree of only one schema
        /// </summary>
        /// <param name="schemaA"></param>
        /// <returns></returns>
        public IDiffNode BuildTree(XmlSchemaObject obj)
        {
            // Setup the schema
            IDiffNode retVal = null;
            if (obj is XmlSchemaAttribute)
                retVal = new AttributeDiffNode() { Original = obj as XmlSchemaAttribute };
            else if (obj is XmlSchemaElement)
            {
                retVal = new ElementDiffNode() { Original = obj as XmlSchemaElement };
                CopyChildNodes((obj as XmlSchemaElement).SchemaType as XmlSchemaComplexType, retVal, new Stack<string>(), true);
            }
            else if (obj is XmlSchemaChoice)
            {
                retVal = new ChoiceDiffNode() { Original = obj as XmlSchemaChoice };
                CopyChildNodes(obj as XmlSchemaChoice, retVal, new Stack<string>());
                retVal = retVal.Children[0];
            }
            else if (obj is XmlSchemaSequence)
            {
                retVal = new SequenceDiffNode() { Original = obj as XmlSchemaSequence };
                CopyChildNodes(obj as XmlSchemaSequence, retVal, new Stack<string>());
                if (TreatSequenceAsNode)
                    retVal = retVal.Children[0];
            }
            else
                return null;

            return retVal;
        }

        /// <summary>
        /// Build a diff tree for the two schemas
        /// </summary>
        public DiffTree BuildTree(XmlSchemaSet schemaA, XmlSchemaSet schemaB)
        {

            // Setup the schema
            DiffTree retVal = new DiffTree() { Original = schemaA, Compared = schemaB };

            // Now add all of the nodes in schemaA to the diff tree
            if (schemaA.Elements == null ||
                schemaB.Elements == null ||
                schemaA.Elements.Count == 0 || schemaB.Elements.Count == 0)
                throw new InvalidOperationException("Both schemas must have at least one root element in order to compare");

            Stack<String> typeStack = new Stack<string>();

            // Root node
            int i = 0;
            foreach (XmlSchemaElement ele in schemaA.Elements)
            {
                i++;
                ElementDiffNode rootNode = new ElementDiffNode() { Original = ele };
                if (rootNode.Original.SchemaType is XmlSchemaComplexType)
                    CopyChildNodes(rootNode.Original.SchemaType as XmlSchemaComplexType, rootNode, typeStack, false);
                retVal.Nodes.Add(rootNode);

                // Comparing, let any listeners know that we're still working
                if (Comparing != null)
                    Comparing(this, ((float)i / schemaA.Elements.Count)/2);

            }

            // Comparison time... the DiffBuilder really just checks for added/removed nodes and places the NET nodes
            // into the tree...
            i = 0;
            foreach (XmlSchemaElement ele in schemaB.Elements)
            {
                i++;
                ElementDiffNode bRootNode = new ElementDiffNode() { Original = ele };
                if (bRootNode.Original.SchemaType is XmlSchemaComplexType)
                    CopyChildNodes(bRootNode.Original.SchemaType as XmlSchemaComplexType, bRootNode, typeStack, false);

                if (Comparing != null)
                    Comparing(this, (float)(((float)i / schemaB.Elements.Count) / 2 + 0.5));

                CompareNodes(retVal.Nodes, bRootNode, null);

                // Is this a new root node?
                if (retVal.Nodes.Find(o => o.FriendlyName == bRootNode.FriendlyName) == null)
                    retVal.Nodes.Add(bRootNode);
            }

            retVal.CalculateDiff();

            return retVal;
        }

        private void CompareNodes(List<IDiffNode> haystack, IDiffNode needle, IDiffNode parent)
        {
            // Comparing
            if (Comparing != null)
                Comparing(this, -1.0f);
            switch (needle.TypeName)
            {
                case "XmlSchemaChoice":
                    var haystackNeedleChoice = from node in haystack
                                               where node.TypeName == "XmlSchemaChoice"
                                               select node;
                    // If there was only one other choice in the haystack, then we can assume these are the same choices and process them
                    if (haystackNeedleChoice.Count() == 1)
                    {
                        foreach (var node in haystackNeedleChoice)
                        {
                            node.SetData(node.GetOriginalNode(), needle.GetOriginalNode());
                            CompareNodes(node.Children, needle.Children, node);
                            break;
                        }
                    }
                    else // A little more complicated how can we tell if A.Choice1 matches B.Choice1 vs A.Choice2 and b.Choice1 if 
                    // something has been added... We need to find the most probable match
                    {
                        DiffNode<XmlSchemaChoice> cdn = FindProbableCandidate(haystackNeedleChoice, needle);
                        if (cdn == null) // This choice has been added
                        {
                            // Everything underneath of this needle is also "added"
                            SwapContent(needle);

                            // Add the "added record" to the parent
                            if (parent != null)
                                parent.AddChildPreserveOrder(needle);
                        }
                        else
                        {
                            cdn.SetData(cdn.GetOriginalNode(), needle.GetOriginalNode());
                            CompareNodes(cdn.Children, needle.Children, cdn);
                        }
                    }
                    break;
                case "XmlSchemaObject": // Mixed Content
                    var haystackNeedleAny = from node in haystack
                                            where node.TypeName == "XmlSchemaObject"
                                            select node;

                    // If there was only one other choice in the haystack, then we can assume these are the same choices and process them
                    if (haystackNeedleAny.Count() > 0)
                    {
                        foreach (var node in haystackNeedleAny)
                        {
                            node.SetData(node.GetOriginalNode(), needle.GetOriginalNode());
                            break;
                        }
                    }
                    break;
                default:
                    // Can we find the needle in the haystack?
                    var haystackNeedle = from node in haystack
                                         where needle.FriendlyName.Equals(node.FriendlyName) && node.TypeName == needle.TypeName
                                         select node;
                    if (haystackNeedle.Count() == 0) // We couldn't find the needle in the haystack, so the needle was added
                    {
                        // Everything underneath of this needle is also "added"
                        SwapContent(needle);

                        // Add the "added record" to the parent
                        if (parent != null)
                            parent.AddChildPreserveOrder(needle);
                    }
                    else // We found the needle in the haystack, the needle represents the "original"
                        foreach (var node in haystackNeedle)
                        {
                            node.SetData(node.GetOriginalNode(), needle.GetOriginalNode());
                            CompareNodes(node.Children, needle.Children, node);
                            break;
                        }
                    break;
            }
        }

        /// <summary>
        /// Find a probable candidate for match 
        /// </summary>
        private DiffNode<XmlSchemaChoice> FindProbableCandidate(IEnumerable<IDiffNode> haystackNeedleChoice, IDiffNode needle)
        {
            bool wasChoiceFound = false;
            foreach (IDiffNode candidate in haystackNeedleChoice)
            {
                if (candidate.TypeName != "XmlSchemaChoice") continue;

                wasChoiceFound = true;

                if (candidate.Children == null && needle.Children == null) // best we can do as children don't exist
                    return candidate as ChoiceDiffNode;
                else if (candidate.Children != null && needle.Children != null) // More details analisys
                {
                    // If we find all children originally in the choice in the new choice, we have a good match
                    int childrenInNeedle = 0;
                    foreach (IDiffNode child in candidate.Children)
                    {
                        bool foundChildInNeedle = false;
                        foreach (IDiffNode nChild in needle.Children)
                            foundChildInNeedle |= child.FullName.Equals(nChild.FullName); // We found an instance of the candidate child in the needle
                        childrenInNeedle += (int)Convert.ToUInt32(foundChildInNeedle);
                    }

                    if (childrenInNeedle == candidate.Children.Count) // we found all children of the original in the compared so it is a good match
                        return candidate as ChoiceDiffNode;

                    // The same is true for the reverse
                    childrenInNeedle = 0;
                    foreach (IDiffNode nChild in needle.Children)
                    {
                        bool foundNeedleInCandidate = false;
                        foreach (IDiffNode child in candidate.Children)
                            foundNeedleInCandidate |= child.FullName.Equals(nChild.FullName);
                        childrenInNeedle += (int)Convert.ToUInt32(foundNeedleInCandidate);
                    }

                    if (childrenInNeedle == needle.Children.Count)
                        return candidate as ChoiceDiffNode;
                }
            }

            return null;
        }


        /// <summary>
        /// Compare a list of needles to a the haystack
        /// </summary>
        private void CompareNodes(List<IDiffNode> haystack, List<IDiffNode> needles, IDiffNode parent)
        {
            foreach (IDiffNode node in needles ?? new List<IDiffNode>())
                CompareNodes(haystack, node, parent);
        }

        /// <summary>
        /// Swap the content
        /// </summary>
        private void SwapContent(IDiffNode node)
        {
            node.SetData(node.GetComparedNode(), node.GetOriginalNode());
            foreach (IDiffNode child in node.Children ?? new List<IDiffNode>())
            {
                SwapContent(child);
            }
            
        }

        /// <summary>
        /// Copy element data
        /// </summary>
        private void CopyChildNodes(XmlSchemaElement element, IDiffNode parent, Stack<String> typeStack)
        {
            if (element.MaxOccurs == "0")
            {
                if (parent.Children != null)
                {
                    IDiffNode foundNode = parent.Children.Find(o => o.FriendlyName == element.Name);
                    parent.Children.Remove(foundNode);
                }
                return;
            }
            else if (parent.Children != null &&
                parent.Children.Find(o => o.FriendlyName == element.Name) != null)
                return;

            ElementDiffNode rootNode = new ElementDiffNode() { Original = element };
            if (rootNode.Original.SchemaType is XmlSchemaComplexType)
                CopyChildNodes(rootNode.Original.SchemaType as XmlSchemaComplexType, rootNode, typeStack, true);
            parent.AddChild(rootNode);
        }

        private void CopyChildNodes(XmlSchemaSequence sequence, IDiffNode parent, Stack<String> typeStack)
        {
            if (sequence.Content == null || sequence.Content.Count == 0) return;

            IDiffNode seqNode = parent;

            if (TreatSequenceAsNode)
                seqNode = new SequenceDiffNode()
                {
                    Original = sequence
                };
            // Build the children of this 
            foreach (XmlSchemaObject child in sequence.Content)
                if (child is XmlSchemaChoice)
                    CopyChildNodes(child as XmlSchemaChoice, seqNode, typeStack);
                else if (child is XmlSchemaSequence)
                    CopyChildNodes(child as XmlSchemaSequence, seqNode, typeStack);
                else if (child is XmlSchemaElement)
                    CopyChildNodes(child as XmlSchemaElement, seqNode, typeStack);
                else if (child is XmlSchemaAny)
                    seqNode.AddChild(new DiffNode<XmlSchemaAny>() { Original = child as XmlSchemaAny });

            if (TreatSequenceAsNode)
                parent.AddChild(seqNode);
        }

        /// <summary>
        /// Copy child nodes for a choice
        /// </summary>
        private void CopyChildNodes(XmlSchemaChoice choice, IDiffNode parent, Stack<String> typeStack)
        {
            if (choice.Content == null || choice.Content.Count == 0) return;

            ChoiceDiffNode chcNode = new ChoiceDiffNode()
            {
                Original = choice
            };
            // Build the children of this 
            foreach (XmlSchemaObject child in choice.Content)
                if (child is XmlSchemaChoice)
                    CopyChildNodes(child as XmlSchemaChoice, chcNode, typeStack);
                else if (child is XmlSchemaSequence)
                    CopyChildNodes(child as XmlSchemaSequence, chcNode, typeStack);
                else if (child is XmlSchemaElement)
                    CopyChildNodes(child as XmlSchemaElement, chcNode, typeStack);
                else if (child is XmlSchemaAny)
                    chcNode.AddChild(new DiffNode<XmlSchemaAny>() { Original = child as XmlSchemaAny });

            // Don't duplicate on structure
            if (parent.Children == null || FindProbableCandidate(parent.Children, chcNode) == null)
                parent.AddChild(chcNode);
        }

        /// <summary>
        /// Process a diff node of xmlschemaelement
        /// </summary>
        /// <param name="rootNode"></param>
        private void CopyChildNodes(XmlSchemaComplexType type, IDiffNode parent, Stack<String> typeStack, bool quickSearch)
        {
            if (type == null || typeStack.Contains(type.Name))
                return;
            typeStack.Push(type.Name);

            IDiffNode idn = null;
            if (currentDiffNodes.TryGetValue(type.ToString(), out idn) && quickSearch)
            {
                // Validator
                typeStack.Pop(); // Pop off
                if (idn.Children != null)
                {
                    foreach (var child in idn.Children)
                        parent.AddChild(child.Clone());
                }
                return;
            }
            else if(quickSearch)
                currentDiffNodes.Add(type.ToString(), parent);

            // Process base content
            if (type.BaseClass != null && type.BaseClass is XmlSchemaComplexType)
                CopyChildNodes(type.BaseClass as XmlSchemaComplexType, parent, typeStack, false);

            foreach (XmlSchemaAttribute att in type.Attributes)
            {
                if (!att.Prohibited && (parent.Children == null || parent.Children.Find(o => o.FriendlyName == att.Name) == null))
                {
                    parent.AddChild(new AttributeDiffNode() { Original = att });
                }
                else if(parent.Children != null) // prohibited use, if it was declared in previous class lets remove it!
                {
                    IDiffNode foundNode = parent.Children.Find(o => o.FriendlyName == att.Name);
                    parent.Children.Remove(foundNode);
                }
            }

            // Process the type
            if (type.Content is XmlSchemaSequence)
                CopyChildNodes(type.Content as XmlSchemaSequence, parent, typeStack);
            else if (type.Content is XmlSchemaChoice)
                CopyChildNodes(type.Content as XmlSchemaChoice, parent, typeStack);

            if (type.Mixed && parent.Children != null && parent.Children.Find(o => o is MixedContentDiffNode) == null)
                parent.AddChild(new MixedContentDiffNode() { Original = type });


            string s = typeStack.Pop();
            System.Diagnostics.Debug.Assert(s.Equals(type.Name));

        }


    }
}
