# SignalR Client for Unity on .Net 4.6
This is a port of Microsoft.AspNet.SignalR.Client which uses the new .Net 4.6 compatibility level offered by Unity. It is based on version 2.2.2 of the SignalR client libraries and supports web sockets. Changes were applied to the following parts of the original source:

* The dependency on Newtonsoft.Json has been removed. A lightweight JSON library created specifically for Unity is used instead. The JSON library resides in the `NotSoSimpleJSON` directory and is based (rather heavily) on [SimpleJSON](http://wiki.unity3d.com/index.php/SimpleJSON) by Bunny83. See the section on `NotSoSimpleJSON` below.
* The original library uses `System.Net.Http` to make HTTP requests. This API *should* be available, but I never managed to get it working on Android, so the port uses Unity's own UnityWebRequest to handle HTTP connections.
* The original handles types automatically by using `JSON.Net`'s generic API. Since Il2cpp doesn't handle reflection that well, such functionality is missing from the JSON module and as a result, the top-level API exposes JSON objects instead of pure CLR types. See Usage below.
* This repo also includes a utility to handle dispatching calls to the main thread inside Unity, based loosely on [UnityMainThreadDispatcher](https://github.com/PimDeWitte/UnityMainThreadDispatcher) by PimDeWitte. See the section on `MainThreadDispatcher` below.
* Also included is a TextWriter that you can use to direct the client's log to Unity's console.

The library has been tested to work on Windows Desktop and Android. It is also fully compatible with Il2cpp (tested on Android), which means it should work on pretty much any platform that Unity supports, but I haven't tested that out yet.

# Usage
Using this port is similar to the original SignalR client. However, instead of strongly typed template functions, you get JSON objects to work with when making and receiving calls to/from the server. Also note that you need a MainThreadDispatcher object in your scene for the library to work.

```
using NotSoSimpleJSON;
using Microsoft.AspNet.SignalR.Client;
using UnityEngine;

public class SignalRConnectionHandler : MonoBehaviour
{
    HubConnection Connection;
    IHubProxy HubProxy;

    async void Start()
    {
        Debug.Log("Connecting");
            
        Connection = new HubConnection("http://myserver.com:8080");
        Connection.TraceLevel = TraceLevels.All;
        Connection.TraceWriter = new UnityConsoleTextWriter();

        HubProxy = Connection.CreateHubProxy("MyHub");
        // Note: no template arguments. The library instead
        // passes a JSONArray containing all parameters to
        // the callback method.
        HubProxy.On("MyCallback", OnMyCallback);
            
        await Connection.Start();
        Debug.Log("Connected");

        // Note: basic CLR types and any object implementing
        // IJSONSerializable can be used as an argument. 
        // The result is of type JSONNode, which can be a
        // single value or a JSON hierarchy depending on
        // what the server returns.
        var CallResult = await HubProxy.Invoke("MyMethod", arg1, arg2);
    }

    private void OnDestroy()
    {
        // Note: this will be a blocking call. This is how
        // the original SignalR client is implemented by
        // Microsoft.
        Connection.Stop();
    }

    // The JSONArray will contain all arguments to the function.
    // They are accessible by index. Each of the arguments may
    // be a single value or a JSON hierarchy depending on what
    // the server returns.
    void OnMyCallback(JSONArray Args)
    {
        var Arg1 = Args[0].AsInt;
        var Arg2 = Args[1].AsString;
    }
}
```

# NotSoSimpleJSON
... is based on SimpleJSON by Bunny83 and was taken from the community wiki. It supports a few operations that the original doesn't handle well:

* Any object can be converted to a JSON node with a single call: `JSON.FromData`. It supports basic CLR types that map directly to JSON primitives, as well as objects that implement IJSONSerializable (also new).
* There is a new node type, `JSONNonexistent`, which stands for any data not specifically included in the source and can only be used for deserialization. Instead of checking if specific keys exist in a JSON object, you can just grab the `jsonObject["SomeKey"]` result. If you attempt to convert it to CLR types, the result will always be `null`.
* Support for distinct data classes (`JSONInt`, `JSONDouble`, etc.) and parse-time type conversion has been added to the original library. It used to have only one `JSONData` type before. A recent update seems to have added the same features to the original.
* A number of usability improvements (including support for typed enumeration) have been made as well.

If you need a JSON parser, feel free to take just this folder and add it to your project. It should be easier to use than the original. It may or may not be faster.

# MainThreadDispatcher
... is based loosely on UnityMainThreadDispatcher by PimDeWitte. It supports `Action`s, Coroutines and (most notably) `Func<T>`s. The `Func<T>` overload returns a Task which will be completed once the operation runs on the main thread. You can await this Task to get the result.

Again, if you need this functionality, feel free to take just this folder.

To use the dispatcher, first add it to an empty game object in the first scene of your application, then use code similar to the following anywhere else:

```
var Result = await MainThreadDispatcher.Instance.Enqueue(() => { return 1; });
```

# Limitations
I have tested this port to work with Windows Desktop builds and Android/Il2cpp builds. It should work on other platforms as well. If you happen to test it out on another platform, tell me about it so I can update this text.

I have a feeling that `UnityWebRequest` doesn't play well with the SSE transport. I mainly wanted this library to work with web sockets (and it does), so I haven't investigated the issue, but the SSE transport always seems to time out and fail, and the client just falls back to long polling in the absence of web sockets.

Also notable, this library has not been tested extensively, so be careful what you do with it. If I discover any bugs, I'll update the repo. Bug reports are welcome as well.