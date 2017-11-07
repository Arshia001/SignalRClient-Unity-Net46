// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NotSoSimpleJSON;

namespace Microsoft.AspNet.SignalR.Client.Hubs
{
    /// <summary>
    /// Represents a subscription to a hub method.
    /// </summary>
    public class Subscription
    {
        public event Action<JSONArray> Received;

        internal void OnReceived(JSONArray data)
        {
            Received?.Invoke(data);
        }
    }
}
