// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using UnityEngine.Networking;

namespace Microsoft.AspNet.SignalR.Client.Http
{
    public class HttpResponseMessageWrapper : IResponse
    {
        private string text;
        private byte[] bytes;

        public HttpResponseMessageWrapper(UnityWebRequest httpResponseMessage)
        {
            text = httpResponseMessage.downloadHandler.text;
            bytes = httpResponseMessage.downloadHandler.data;
        }

        public string ReadAsString()
        {
            return text;
        }

        public Stream GetStream()
        {
            return new MemoryStream(bytes);
        }

        protected virtual void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    _httpResponseMessage.Dispose();
            //}
        }

        public void Dispose()
        {
            //Dispose(true);
        }
    }
}
