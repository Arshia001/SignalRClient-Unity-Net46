// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NotSoSimpleJSON;

namespace Microsoft.AspNet.SignalR.Client.Hubs
{
    public class HubProgressUpdate
    {
        /// <summary>
        /// The callback identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The progress value
        /// </summary>
        public JSONNode Data { get; set; }

        public static HubProgressUpdate FromJson(JSONNode json)
        {
            var Result = new HubProgressUpdate();

            Result.Id = json["I"].AsString;
            Result.Data = json["D"];

            return Result;
        }
    }
}
