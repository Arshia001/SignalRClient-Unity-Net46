/*
 * Created by Arshia001
 * Based heavily on http://wiki.unity3d.com/index.php/SimpleJSON by Bunny83
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotSoSimpleJSON
{
    public class JSONArray : JSONNode, IEnumerable<JSONNode>
    {
        private List<JSONNode> m_List = new List<JSONNode>();

        public override JSONNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                    return new JSONNonexistent();
                return m_List[aIndex];
            }
            set
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                    m_List.Add(value);
                else
                    m_List[aIndex] = value;
            }
        }

        public override JSONNodeType Type
        {
            get
            {
                return JSONNodeType.Array;
            }
        }

        public override JSONArray AsArray
        {
            get
            {
                return this;
            }
        }

        public override int Count
        {
            get { return m_List.Count; }
        }

        public override bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        public override void Add(JSONNode aItem)
        {
            m_List.Add(aItem);
        }

        public override JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_List.Count)
                return null;
            JSONNode tmp = m_List[aIndex];
            m_List.RemoveAt(aIndex);
            return tmp;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            m_List.Remove(aNode);
            return aNode;
        }

        public override IEnumerable<JSONNode> Children
        {
            get
            {
                return (IEnumerable<JSONNode>)GetEnumerator();
            }
        }

        public IEnumerator<JSONNode> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        protected override void WriteToStringBuilder(StringBuilder StringBuilder)
        {
            StringBuilder.Append("[");
            bool bFirst = true;
            foreach (JSONNode N in m_List)
            {
                if (bFirst)
                    bFirst = false;
                else
                    StringBuilder.Append(",");

                StringBuilder.Append(N.Serialize());
            }
            StringBuilder.Append("]");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_List.GetEnumerator();
        }
    }
}
