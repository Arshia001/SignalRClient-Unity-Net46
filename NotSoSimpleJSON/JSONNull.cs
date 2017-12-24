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

        public override bool IsEmpty
        {
            get
            {
                return true;
            }
        }

        public override JSONArray AsArray
        {
            get
            {
                return null;
            }
        }

        public override bool? AsBool
        {
            get
            {
                return null;
            }
        }

        public override int? AsInt
        {
            get
            {
                return null;
            }
        }

        public override double? AsDouble
        {
            get
            {
                return null;
            }
        }

        public override Dictionary<string, JSONNode> AsDictionary
        {
            get
            {
                return null;
            }
        }

        public override JSONObject AsObject
        {
            get
            {
                return null;
            }
        }

        public override string AsString
        {
            get
            {
                return null;
            }
        }

        protected override void WriteToStringBuilder(StringBuilder StringBuilder)
        {
            StringBuilder.Append("null");
        }
    }
}
