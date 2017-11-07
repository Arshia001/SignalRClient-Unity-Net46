// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NotSoSimpleJSON;
using System.Diagnostics;

namespace Microsoft.AspNet.SignalR.Client
{
    [DebuggerDisplay("{ConnectionId} {Url} -> {ProtocolVersion}")]
    public class NegotiationResponse
    {
        public string ConnectionId { get; set; }
        public string ConnectionToken { get; set; }
        public string Url { get; set; }
        public string ProtocolVersion { get; set; }
        public double DisconnectTimeout { get; set; }
        public bool TryWebSockets { get; set; }
        public double? KeepAliveTimeout { get; set; }
        public double TransportConnectTimeout { get; set; }

        public static NegotiationResponse FromJson(JSONNode json)
        {
            var Result = new NegotiationResponse();

            Result.ConnectionId = json["ConnectionId"].AsString;
            Result.ConnectionToken = json["ConnectionToken"].AsString;
            Result.Url = json["Url"].AsString;
            Result.ProtocolVersion = json["ProtocolVersion"].AsString;
            Result.DisconnectTimeout = json["DisconnectTimeout"].AsDouble.GetValueOrDefault();
            Result.TryWebSockets = json["TryWebSockets"].AsBool.GetValueOrDefault();
            Result.KeepAliveTimeout = json["KeepAliveTimeout"].AsDouble;
            Result.TransportConnectTimeout = json["TransportConnectTimeout"].AsDouble.GetValueOrDefault();

            return Result;
        }
    }
}
