using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugGazer
{
//  public class IndexedStringTrie()
    // synopsis: a memory efficient, trie inspired, indexed data collection (aka compressed string array)
    // 
    // goal: store a string ('payload') by matching it against an existing list of strings ('particles')
    // 
    // 1) if no match is found, the payload is stored as a new particle
    //    any subsequently stored payload is matched against all existing particles.
    // 2) if a particle is found that can (partly) represent the payload,
    //    an index-reference ('node') is created to the particle with a start-index and length of the sub-particle
    // 3) for the remainder of the payload the procedure is repeated until either not futher characters remain
    //    or more particles are found to match, in which case the payload is store as in 1)

    // for efficiecy, we dont store particles as induvidual objects, but rather
    // just allocate a 1MB memory block and fill it until it runs out, 
    // and allocate the next 1MB
    // 
    // Requirement for algoithm:
    // - compression should be near O(1) not use large amounts of temporary memory (would cause fragmentation)
    // -
    public class IndexedLZ77
    {
        const int memoryBlockSize = 16 * 1024;
        List<Node> mNodes = new List<Node>();               // the index to Nodes are refered to as 'NodeId'

        public Dictionary<string, string> mEncodeDict = new Dictionary<string, string>();

        class Node
        {
            public string Text;
            public Node Next = null;        // next node, null means end of string.
        }

        const int minimalLen = 10;

        // an index is returned that can be used to retrieve the string
        public int Add(string s)
        {
            Node node = new Node();
            int index = AddNode(node);
            if (s.Length < minimalLen)
            {
                node.Text = s;
                return index;
            }

            int offset = 0;
            int len = minimalLen;
            while ((offset+len) < s.Length)
            {
                string s1 = s.Substring(offset, len);
                string foundString = null;
                if (mEncodeDict.TryGetValue(s1, out foundString))
                {
                    len++;
                    // found, but maybe more can matched
                }
                else
                {
                    if (foundString != null)
                    {
                        Controller.WriteLine("store matched: " + foundString);
                        node.Text = foundString;
                        node.Next = new Node();
                        node = node.Next;
                        foundString = null;
                        offset = offset + len;
                        len = Math.Min(minimalLen, s.Length - (offset + len));
                    }

                    // not found, increase offset, until offset == minimalLen
                    if (offset == minimalLen)
                    {
                        Controller.WriteLine("store unmatched: " + s1);
                        string store = s.Substring(0, offset);
                        mEncodeDict[store] = store;
                        node.Text = store;
                        node.Next = new Node();
                        node = node.Next;
                    }
                    offset++;
                }

                {
                    mEncodeDict[s1] = s1;
                    if (foundString != null)
                    {
                        node.Text = foundString;
                    }
                    Controller.WriteLine("store: " + s1);
                    node = new Node();
                    node.Next = node;
                    offset = len;
                }
                len++;
            }
            return index;
        }

        int AddNode(Node node)
        {
            int nodeId = mNodes.Count;
            mNodes.Add(node);
            return nodeId;
        }

        // retrievel operator
        public string this[int nodeId] 
        { 
            get
            {
                return GetText(mNodes[nodeId]);
            }
        }

        public void Clear()
        {
            mNodes.Clear();
            mEncodeDict.Clear();
        }

        // may be called at any time to reset the encoder and reclaim some memory.
        public void ClearEncoderDict()
        {
            mEncodeDict.Clear();
        }

        string GetText(Node node)
        {
            StringBuilder sb = new StringBuilder();
            Recurse(node, sb);
            return sb.ToString();
        }

        void Recurse(Node node, StringBuilder sb)
        {
            sb.Append(node.Text);
            if (node.Next != null)
            {
                Recurse(node.Next, sb);
            }
        }

        // return the longest common substring
        string GetLCSString1(string str1, string str2)
        {
            int[,] num = new int[str1.Length, str2.Length];
            int maxLen = 0;
            int lastSubsBegin = 0;
            StringBuilder sequenceBuilder = new StringBuilder();

            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    if (str1[i] != str2[j])
                        num[i, j] = 0;
                    else
                    {
                        if (i == 0 || j == 0)
                            num[i, j] = 1;
                        else
                            num[i, j] = 1 + num[i - 1, j - 1];

                        if (num[i, j] > maxLen)
                        {
                            maxLen = num[i, j];
                            int thisSubsBegin = i - num[i, j] + 1;
                            if (lastSubsBegin == thisSubsBegin)
                            {
                                // If the current LCS is the same as the last time this block ran
                                sequenceBuilder.Append(str1[i]);
                            }
                            else
                            {
                                // Reset the string builder if a different LCS is found
                                lastSubsBegin = thisSubsBegin;
                                sequenceBuilder.Length = 0;
                                sequenceBuilder.Append(str1.Substring(lastSubsBegin, (i + 1) - lastSubsBegin));
                            }
                        }
                    }
                }
            }
            return sequenceBuilder.ToString();
        }

        // http://en.wikibooks.org/wiki/Algorithm_Implementation/Strings/Longest_common_substring
        public int GetLCSString2(string str1, string str2, out string sequence)
        {
            sequence = string.Empty;
            if (String.IsNullOrEmpty(str1) || String.IsNullOrEmpty(str2))
                return 0;

            int[,] num = new int[str1.Length, str2.Length];
            int maxlen = 0;
            int lastSubsBegin = 0;
            StringBuilder sequenceBuilder = new StringBuilder();

            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    if (str1[i] != str2[j])
                        num[i, j] = 0;
                    else
                    {
                        if ((i == 0) || (j == 0))
                            num[i, j] = 1;
                        else
                            num[i, j] = 1 + num[i - 1, j - 1];

                        if (num[i, j] > maxlen)
                        {
                            maxlen = num[i, j];
                            int thisSubsBegin = i - num[i, j] + 1;
                            if (lastSubsBegin == thisSubsBegin)
                            {//if the current LCS is the same as the last time this block ran
                                sequenceBuilder.Append(str1[i]);
                            }
                            else //this block resets the string builder if a different LCS is found
                            {
                                lastSubsBegin = thisSubsBegin;
                                sequenceBuilder.Length = 0; //clear it
                                sequenceBuilder.Append(str1.Substring(lastSubsBegin, (i + 1) - lastSubsBegin));
                            }
                        }
                    }
                }
            }
            sequence = sequenceBuilder.ToString();
            return maxlen;
        }
    }
}

