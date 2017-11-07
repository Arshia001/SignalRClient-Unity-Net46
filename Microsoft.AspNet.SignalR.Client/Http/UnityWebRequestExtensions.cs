using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class UnityWebRequestExtensions
{
    public static async Task SendAsync(this UnityWebRequest Request, CancellationToken CancellationToken)
    {
        var Op = Request.SendWebRequest();

        while (!Op.isDone && !CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(10);
        }

        CancellationToken.ThrowIfCancellationRequested();
    }
}
