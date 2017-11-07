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
    public enum JSONNodeType
    {
        Array,
        Object,
        StringValue,
        IntValue,
        DoubleValue,
        BoolValue,
        NullValue,
        Nonexistant
    }
}
