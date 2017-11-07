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
    public static class JSON
    {
        public static JSONNode Parse(string aJSON)
        {
            return JSONNode.Parse(aJSON);
        }

        public static JSONNode FromData(object data)
        {
            if (data == null)
                return new JSONNull();
            if (data is JSONNode)
                return (JSONNode)data;
            if (data is IJSONSerializable)
                return ((IJSONSerializable)data).ToJson();
            if (data is int || data is int?)
                return new JSONInt((int)data);
            if (data is float || data is float? || data is double || data is double?)
                return new JSONDouble((double)data);
            if (data is bool || data is bool?)
                return new JSONBool((bool)data);
            if (data is string)
                return new JSONString((string)data);
            if (data is IEnumerable)
            {
                var Result = new JSONArray();
                foreach (var Item in (IEnumerable)data)
                    Result.Add(FromData(Item));
                return Result;
            }
            if (data is IDictionary)
            {
                var Result = new JSONObject();
                var Dic = (IDictionary)data;
                foreach (var Key in Dic.Keys)
                    Result.Add(Key.ToString(), FromData(Dic[Key]));
                return Result;
            }

            throw new NotSupportedException();
        }
    }
}
