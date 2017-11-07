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
    public class JSONObject : JSONNode, IEnumerable<KeyValuePair<string, JSONNode>>
    {
        private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

        public override JSONNode this[string aKey]
        {
            get
            {
                if (m_Dict.ContainsKey(aKey))
                    return m_Dict[aKey];
                else
                    return new JSONNonexistent();
            }
            set
            {
                m_Dict[aKey] = value;
            }
        }

        public override JSONNodeType Type
        {
            get
            {
                return JSONNodeType.Object;
            }
        }


        public override JSONObject AsObject
        {
            get
            {
                return this;
            }
        }

        public override Dictionary<string, JSONNode> AsDictionary
        {
            get
            {
                return new Dictionary<string, JSONNode>(m_Dict);
            }
        }

        public override int Count
        {
            get { return m_Dict.Count; }
        }

        public override bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            if (!string.IsNullOrEmpty(aKey))
                m_Dict[aKey] = aItem;
            else
                throw new NotSupportedException();
        }

        public override JSONNode Remove(string aKey)
        {
            if (!m_Dict.ContainsKey(aKey))
                return null;
            JSONNode tmp = m_Dict[aKey];
            m_Dict.Remove(aKey);
            return tmp;
        }

        public override JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_Dict.Count)
                return null;
            var item = m_Dict.ElementAt(aIndex);
            m_Dict.Remove(item.Key);
            return item.Value;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            try
            {
                var item = m_Dict.Where(k => k.Value == aNode).First();
                m_Dict.Remove(item.Key);
                return aNode;
            }
            catch
            {
                return null;
            }
        }

        public override IEnumerable<JSONNode> Children
        {
            get
            {
                foreach (KeyValuePair<string, JSONNode> N in m_Dict)
                    yield return N.Value;
            }
        }

        public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator()
        {
            return m_Dict.GetEnumerator();
        }

        protected override void WriteToStringBuilder(StringBuilder StringBuilder)
        {
            StringBuilder.Append("{");
            bool bFirst = true;
            foreach (KeyValuePair<string, JSONNode> N in m_Dict)
            {
                if (bFirst)
                    bFirst = false;
                else
                    StringBuilder.Append(",");

                StringBuilder.Append("\"" + Escape(N.Key) + "\":" + N.Value.Serialize());
            }
            StringBuilder.Append("}");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Dict.GetEnumerator();
        }
    }
}
