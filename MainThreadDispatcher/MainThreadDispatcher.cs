using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

// Based VERY loosely on https://github.com/PimDeWitte/UnityMainThreadDispatcher/
public class MainThreadDispatcher : MonoBehaviour
{
    interface IDispatcher
    {
        void ExecuteAll(MainThreadDispatcher Owner);
    }

    class ActionDispatcher : IDispatcher
    {
        static ActionDispatcher _Instance;
        public static ActionDispatcher Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ActionDispatcher();
                return _Instance;
            }
        }

        public static readonly List<Action> ExecutionQueue = new List<Action>();

        void IDispatcher.ExecuteAll(MainThreadDispatcher Owner)
        {
            foreach (var A in ExecutionQueue)
                try
                {
                    A.Invoke();
                }
                catch (Exception Ex)
                {
                    Debug.LogException(Ex);
                }

            ExecutionQueue.Clear();
        }
    }

    class CoroutineDispatcher : IDispatcher
    {
        static CoroutineDispatcher _Instance;
        public static CoroutineDispatcher Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new CoroutineDispatcher();
                return _Instance;
            }
        }

        public static readonly List<IEnumerator> ExecutionQueue = new List<IEnumerator>();

        void IDispatcher.ExecuteAll(MainThreadDispatcher Owner)
        {
            foreach (var E in ExecutionQueue)
                try
                {
                    Owner.StartCoroutine(E);
                }
                catch (Exception Ex)
                {
                    Debug.LogException(Ex);
                }

            ExecutionQueue.Clear();
        }
    }

    class FuncDispatcher<T> : IDispatcher
    {
        static FuncDispatcher<T> _Instance;
        public static FuncDispatcher<T> Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FuncDispatcher<T>();
                return _Instance;
            }
        }

        public class ExecItem
        {
            public TaskCompletionSource<T> TCS;
            public Func<T> Func;
        }
        public static readonly List<ExecItem> ExecutionQueue = new List<ExecItem>();

        void IDispatcher.ExecuteAll(MainThreadDispatcher Owner)
        {
            foreach (var E in ExecutionQueue)
                try
                {
                    E.TCS.SetResult(E.Func.Invoke());
                }
                catch (Exception Ex)
                {
                    E.TCS.SetException(Ex);
                }

            ExecutionQueue.Clear();
        }
    }

    private static readonly HashSet<IDispatcher> Dispatchers = new HashSet<IDispatcher>();

    static bool IsMainThread()
    {
        return !System.Threading.Thread.CurrentThread.IsBackground;

	// This approach works, but has potential performance penalties and creates unnecessary error messages in the log
        //try
        //{
        //    var Dummy = Time.time;
        //    return true;
        //}
        //catch
        //{
        //    return false;
        //}
    }

    public void Update()
    {
        lock (Dispatchers)
        {
            foreach (var H in Dispatchers)
                H.ExecuteAll(this);

            Dispatchers.Clear();
        }
    }

    public void Enqueue(Action Action)
    {
        if (IsMainThread())
            Action();
        else
            lock (Dispatchers)
            {
                ActionDispatcher.ExecutionQueue.Add(Action);
                Dispatchers.Add(ActionDispatcher.Instance);
            }
    }

    public void Enqueue(IEnumerator Coroutine)
    {
        if (IsMainThread())
            StartCoroutine(Coroutine);
        else
            lock (Dispatchers)
            {
                CoroutineDispatcher.ExecutionQueue.Add(Coroutine);
                Dispatchers.Add(CoroutineDispatcher.Instance);
            }
    }

    public Task<T> Enqueue<T>(Func<T> Action)
    {
        if (IsMainThread())
            return Task.FromResult(Action());
        else
            lock (Dispatchers)
            {
                var TCS = new TaskCompletionSource<T>();
                FuncDispatcher<T>.ExecutionQueue.Add(new FuncDispatcher<T>.ExecItem { Func = Action, TCS = TCS });
                Dispatchers.Add(FuncDispatcher<T>.Instance);
                return TCS.Task;
            }
    }

    public static MainThreadDispatcher Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}