// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Microsoft.AspNet.SignalR.Client.Http
{
    public class HttpRequestMessageWrapper : IRequest
    {
        private readonly UnityWebRequest _httpRequestMessage;
        private readonly Action _cancel;

        public HttpRequestMessageWrapper(UnityWebRequest httpRequestMessage, Action cancel)
        {
            _httpRequestMessage = httpRequestMessage;
            _cancel = cancel;
        }

        public string UserAgent { get; set; }

        public string Accept { get; set; }

        public void Abort()
        {
            _cancel();
        }

        public void SetRequestHeaders(IDictionary<string, string> headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }

            if (UserAgent != null)
            {
                _httpRequestMessage.SetRequestHeader("User-Agent", UserAgent);
            }

            if (Accept != null)
            {
                _httpRequestMessage.SetRequestHeader("Accept", Accept);
            }

            foreach (KeyValuePair<string, string> headerEntry in headers)
            {
                _httpRequestMessage.SetRequestHeader(headerEntry.Key, headerEntry.Value);
            }
        }
    }
}
