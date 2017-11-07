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
    public class JSONNull : JSONNode
    {
        public override JSONNodeType Type
        {
            get
            {
                return JSONNodeType.NullValue;
            }
        }

        protected override void WriteToStringBuilder(StringBuilder StringBuilder)
        {
            StringBuilder.Append("null");
        }
    }
}
