// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.SignalR.Client.Infrastructure;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.Collections.Generic;
using NotSoSimpleJSON;

#if !PORTABLE
namespace Microsoft.AspNet.SignalR.Client.Hubs
{
    /// <summary>
    /// <see cref="T:System.IObservable{object[]}"/> implementation of a hub event.
    /// </summary>

    public class Hubservable : IObservable<JSONArray>
    {
        private readonly string _eventName;
        private readonly IHubProxy _proxy;

        public Hubservable(IHubProxy proxy, string eventName)
        {
            _proxy = proxy;
            _eventName = eventName;
        }

        public IDisposable Subscribe(IObserver<JSONArray> observer)
        {
            var subscription = _proxy.Subscribe(_eventName);
            subscription.Received += observer.OnNext;

            return new DisposableAction(() =>
            {
                subscription.Received -= observer.OnNext;
            });
        }
    }
}
#endif
