// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NotSoSimpleJSON;

namespace Microsoft.AspNet.SignalR.Client.Hubs
{
    public class HubInvocation : IJSONSerializable
    {
        public string CallbackId { get; set; }

        public string Hub { get; set; }

        public string Method { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "This type is used for serialization")]
        public JSONArray Args { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "This type is used for serialization")]
        public Dictionary<string, JSONNode> State { get; set; }

        public static HubInvocation FromJson(JSONNode json)
        {
            if (json == null)
                return null;

            var Result = new HubInvocation();

            Result.CallbackId = json["I"].AsString;
            Result.Hub = json["H"].AsString;
            Result.Method = json["M"].AsString;
            Result.Args = json["A"].AsArray;
            Result.State = json["S"].AsDictionary;

            return Result;
        }

        public JSONNode ToJson()
        {
            var Result = new JSONObject();

            Result.Add("I", JSON.FromData(CallbackId));
            Result.Add("H", JSON.FromData(Hub));
            Result.Add("M", JSON.FromData(Method));
            Result.Add("A", JSON.FromData(Args));
            Result.Add("S", JSON.FromData(State));

            return Result;
        }
    }
}
