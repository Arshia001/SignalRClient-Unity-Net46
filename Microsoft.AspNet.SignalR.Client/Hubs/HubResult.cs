// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NotSoSimpleJSON;

namespace Microsoft.AspNet.SignalR.Client.Hubs
{
    /// <summary>
    /// Represents the result of a hub invocation.
    /// </summary>
    public class HubResult
    {
        /// <summary>
        /// The callback identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The progress update of the invocation
        /// </summary>
        public HubProgressUpdate ProgressUpdate { get; set; }

        /// <summary>
        /// The return value of the hub
        /// </summary>
        public JSONNode Result { get; set; }

        /// <summary>
        /// Indicates whether the Error is a <see cref="HubException"/>.
        /// </summary>
        public bool IsHubException { get; set; }

        /// <summary>
        /// The error message returned from the hub invocation.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Extra error data
        /// </summary>
        public JSONNode ErrorData { get; set; }

        /// <summary>
        /// The caller state from this hub.
        /// </summary>
        public JSONObject State { get; private set; }

        public static HubResult FromJson(JSONNode json)
        {
            var Result = new HubResult();

            Result.Id = json["I"].AsString;
            Result.ProgressUpdate = HubProgressUpdate.FromJson(json["P"]);
            Result.Result = json["R"];
            Result.IsHubException = json["H"].AsBool.GetValueOrDefault();
            Result.Error = json["E"].AsString;
            Result.ErrorData = json["D"];
            Result.State = json["S"].AsObject;

            return Result;
        }
    }
}
