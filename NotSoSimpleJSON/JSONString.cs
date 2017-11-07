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
    public class JSONString : JSONNode
    {
        string data;

        public override JSONNodeType Type
        {
            get
            {
                return JSONNodeType.StringValue;
            }
        }

        public override string AsString
        {
            get
            {
                return data;
            }
        }

        public override bool? AsBool
        {
            get
            {
                bool res;
                return bool.TryParse(data, out res) ? (bool?)res : null;
            }
        }

        public override int? AsInt
        {
            get
            {
                int res;
                return int.TryParse(data, out res) ? (int?)res : null;
            }
        }

        public override double? AsDouble
        {
            get
            {
                double res;
                return double.TryParse(data, out res) ? (double?)res : null;
            }
        }

        public JSONString(string data)
        {
            this.data = data;
        }

        protected override void WriteToStringBuilder(StringBuilder StringBuilder)
        {
            StringBuilder.Append("\"");
            StringBuilder.Append(Escape(data));
            StringBuilder.Append("\"");
        }
    }
}
