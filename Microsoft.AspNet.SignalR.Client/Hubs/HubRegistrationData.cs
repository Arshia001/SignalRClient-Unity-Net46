// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NotSoSimpleJSON;

namespace Microsoft.AspNet.SignalR.Client.Hubs
{
    public class HubRegistrationData : IJSONSerializable
    {
        public string Name { get; set; }

        public JSONNode ToJson()
        {
            var Result = new JSONObject();

            Result.Add("Name", JSON.FromData(Name));

            return Result;
        }
    }
}
