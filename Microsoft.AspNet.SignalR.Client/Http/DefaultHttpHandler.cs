//// Copyright (c) .NET Foundation. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//using System;
//using System.Net.Http;
//#if !NETFX_CORE && !PORTABLE
//using System.Security.Cryptography.X509Certificates;
//#endif

//namespace Microsoft.AspNet.SignalR.Client.Http
//{
//#if false
//    public class DefaultHttpHandler : WebRequestHandler
//#else
//    public class DefaultHttpHandler : HttpClientHandler
//#endif
//    {
//        private readonly IConnection _connection;

//        public DefaultHttpHandler(IConnection connection)
//        {
//            if (connection != null)
//            {
//                _connection = connection;
//            }
//            else
//            {
//                throw new ArgumentNullException("connection");
//            }

//            Credentials = _connection.Credentials;
//#if PORTABLE
//            if (this.SupportsPreAuthenticate())
//            {
//                PreAuthenticate = true;
//            }
//#elif NET_4_6
//            PreAuthenticate = true;
//#endif

//            if (_connection.CookieContainer != null)
//            {
//                CookieContainer = _connection.CookieContainer;
//            }

//#if !PORTABLE
//            if (_connection.Proxy != null)
//            {
//                Proxy = _connection.Proxy;
//            }
//#endif

//            // NOTE this will probably affect https in some way, we don't care yet
////#if NET_4_6
////            foreach (X509Certificate cert in _connection.Certificates)
////            {
////                ClientCertificates.Add(cert);
////            }
////#endif
//        }
//    }
//}
