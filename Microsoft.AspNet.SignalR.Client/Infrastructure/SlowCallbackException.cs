// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNet.SignalR.Client.Infrastructure
{
#if NET_4_6 || UNITY_WSA
    [Serializable]
#endif
    public class SlowCallbackException : Exception
    {
        public SlowCallbackException() { }
        public SlowCallbackException(string message) : base(message) { }
        public SlowCallbackException(string message, Exception inner) : base(message, inner) { }

#if NET_4_6 || UNITY_WSA
        protected SlowCallbackException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif
    }
}
