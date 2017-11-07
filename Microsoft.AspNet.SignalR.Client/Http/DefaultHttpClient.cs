// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Infrastructure;
using UnityEngine.Networking;

namespace Microsoft.AspNet.SignalR.Client.Http
{
    /// <summary>
    /// The default <see cref="IHttpClient"/> implementation.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:Implement IDisposable", Justification = "Response task returned to the caller so cannot dispose Http Client")]
    public class DefaultHttpClient : IHttpClient
    {
        /// <summary>
        /// Makes an asynchronous http GET request to the specified url.
        /// </summary>
        /// <param name="url">The url to send the request to.</param>
        /// <param name="prepareRequest">A callback that initializes the request with default values.</param>
        /// <param name="isLongRunning">Indicates whether the request is long running</param>
        /// <returns>A <see cref="T:Task{IResponse}"/>.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Handler cannot be disposed before response is disposed")]
        public Task<IResponse> Get(string url, Action<IRequest> prepareRequest, bool isLongRunning)
        {
            return MainThreadDispatcher.Instance.Enqueue(async () =>
            {
                if (prepareRequest == null)
                {
                    throw new ArgumentNullException("prepareRequest");
                }

                var cts = new CancellationTokenSource();

                using (var requestMessage = UnityWebRequest.Get(url))
                {
                    var request = new HttpRequestMessageWrapper(requestMessage, () =>
                    {
                        cts.Cancel();
                        requestMessage.Dispose();
                    });

                    requestMessage.disposeDownloadHandlerOnDispose = true;
                    requestMessage.downloadHandler = new DownloadHandlerBuffer();
                    prepareRequest(request);

                    await requestMessage.SendAsync(cts.Token);
                    return await MainThreadDispatcher.Instance.Enqueue(() =>
                    {
                        if (requestMessage.isHttpError || requestMessage.isNetworkError)
                        {
                            throw new HttpClientException(requestMessage);
                        }

                        return (IResponse)new HttpResponseMessageWrapper(requestMessage);
                    });
                }
            }).Unwrap();
        }

        /// <summary>
        /// Makes an asynchronous http POST request to the specified url.
        /// </summary>
        /// <param name="url">The url to send the request to.</param>
        /// <param name="prepareRequest">A callback that initializes the request with default values.</param>
        /// <param name="postData">form url encoded data.</param>
        /// <param name="isLongRunning">Indicates whether the request is long running</param>
        /// <returns>A <see cref="T:Task{IResponse}"/>.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Handler cannot be disposed before response is disposed")]
        public Task<IResponse> Post(string url, Action<IRequest> prepareRequest, IDictionary<string, string> postData, bool isLongRunning)
        {
            return MainThreadDispatcher.Instance.Enqueue(async () =>
            {
                if (prepareRequest == null)
                {
                    throw new ArgumentNullException("prepareRequest");
                }

                var cts = new CancellationTokenSource();

                List<IMultipartFormSection> data = new List<IMultipartFormSection>();

                if (postData != null)
                {
                    data.Add(new MultipartFormDataSection(HttpHelper.ProcessPostData(postData)));
                }

                using (var requestMessage = UnityWebRequest.Post(url, data))
                {
                    var request = new HttpRequestMessageWrapper(requestMessage, () =>
                    {
                        cts.Cancel();
                        requestMessage.Dispose();
                    });

                    requestMessage.disposeDownloadHandlerOnDispose = true;
                    requestMessage.downloadHandler = new DownloadHandlerBuffer();
                    prepareRequest(request);

                    await requestMessage.SendAsync(cts.Token);
                    return await MainThreadDispatcher.Instance.Enqueue(() =>
                    {
                        if (requestMessage.isHttpError || requestMessage.isNetworkError)
                        {
                            throw new HttpClientException(requestMessage);
                        }

                        return (IResponse)new HttpResponseMessageWrapper(requestMessage);
                    });
                }
            }).Unwrap();
        }
    }
}
