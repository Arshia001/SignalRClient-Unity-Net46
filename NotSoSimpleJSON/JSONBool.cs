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
    public class JSONBool : JSONNode
    {
        bool data;

        public override JSONNodeType Type
        {
            get
            {
                return JSONNodeType.BoolValue;
            }
        }

        public override bool? AsBool
        {
            get
            {
                return data;
            }
        }

        public override string AsString
        {
            get
            {
                return Serialize();
            }
        }

        public JSONBool(bool data)
        {
            this.data = data;
        }

        protected override void WriteToStringBuilder(StringBuilder StringBuilder)
        {
            StringBuilder.Append(data);
        }
    }
}
