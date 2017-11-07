// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Infrastructure;
using NotSoSimpleJSON;

namespace Microsoft.AspNet.SignalR.Client.Hubs
{
    public class HubProxy : IHubProxy
    {
        private readonly string _hubName;
        private readonly IHubConnection _connection;
        private readonly Dictionary<string, JSONNode> _state = new Dictionary<string, JSONNode>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Subscription> _subscriptions = new Dictionary<string, Subscription>(StringComparer.OrdinalIgnoreCase);

        public HubProxy(IHubConnection connection, string hubName)
        {
            _connection = connection;
            _hubName = hubName;
        }

        public JSONNode this[string name]
        {
            get
            {
                lock (_state)
                {
                    JSONNode value;
                    _state.TryGetValue(name, out value);
                    return value;
                }
            }
            set
            {
                lock (_state)
                {
                    _state[name] = value;
                }
            }
        }

        public Subscription Subscribe(string eventName)
        {
            if (eventName == null)
            {
                throw new ArgumentNullException("eventName");
            }

            Subscription subscription;
            if (!_subscriptions.TryGetValue(eventName, out subscription))
            {
                subscription = new Subscription();
                _subscriptions.Add(eventName, subscription);
            }

            return subscription;
        }

        public void InvokeEvent(string eventName, JSONArray args)
        {
            Subscription subscription;
            if (_subscriptions.TryGetValue(eventName, out subscription))
            {
                subscription.OnReceived(args);
            }
        }

        public Task<JSONNode> Invoke(string method, params object[] args)
        {
            return Invoke(method, null, args);
        }

        public Task<JSONNode> Invoke(string method, Action<JSONNode> onProgress, params object[] args)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            JSONArray ArgsArray = new JSONArray();
            for (int i = 0; i < args.Length; i++)
            {
                ArgsArray.Add(JSON.FromData(args[i]));
            }

            var tcs = new DispatchingTaskCompletionSource<JSONNode>();
            var callbackId = _connection.RegisterCallback(result =>
            {
                if (result != null)
                {
                    if (result.Error != null)
                    {
                        if (result.IsHubException)
                        {
                            // A HubException was thrown
                            tcs.TrySetException(new HubException(result.Error, result.ErrorData));
                        }
                        else
                        {
                            tcs.TrySetException(new InvalidOperationException(result.Error));
                        }
                    }
                    else
                    {
                        try
                        {
                            if (result.State != null)
                            {
                                foreach (var pair in result.State)
                                {
                                    this[pair.Key] = pair.Value;
                                }
                            }

                            if (result.ProgressUpdate != null)
                            {
                                onProgress?.Invoke(result.ProgressUpdate.Data);
                            }
                            else if (result.Result != null)
                            {
                                tcs.TrySetResult(result.Result);
                            }
                            else
                            {
                                tcs.TrySetResult(new JSONNonexistent());
                            }
                        }
                        catch (Exception ex)
                        {
                            // If we failed to set the result for some reason or to update
                            // state then just fail the tcs.
                            tcs.TrySetUnwrappedException(ex);
                        }
                    }
                }
                else
                {
                    tcs.TrySetCanceled();
                }
            });

            var hubData = new HubInvocation
            {
                Hub = _hubName,
                Method = method,
                Args = ArgsArray,
                CallbackId = callbackId
            };

            if (_state.Count != 0)
            {
                hubData.State = _state;
            }

            var value = hubData.ToJson().Serialize();

            _connection.Send(value).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    _connection.RemoveCallback(callbackId);
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    _connection.RemoveCallback(callbackId);
                    tcs.TrySetUnwrappedException(task.Exception);
                }
            },
            TaskContinuationOptions.NotOnRanToCompletion);

            return tcs.Task;
        }
    }
}
