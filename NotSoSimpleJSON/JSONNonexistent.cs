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
    class JSONNonexistent : JSONNode
    {
        public override JSONNodeType Type
        {
            get
            {
                return JSONNodeType.Nonexistant;
            }
        }

        protected override void WriteToStringBuilder(StringBuilder StringBuilder)
        {
            throw new NotSupportedException();
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

        public override JSONObject AsObject
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

        public override int? AsInt
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

        public override int Count
        {
            get
            {
                return 0;
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return true;
            }
        }
    }
}
