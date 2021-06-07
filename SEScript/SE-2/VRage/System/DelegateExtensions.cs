namespace System
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class DelegateExtensions
    {
        private const bool ProfileDelegateInvocations = false;
        private static Func<object, int> m_getInvocationCount;
        private static Func<object, object[]> m_getInvocationList;
        [ThreadStatic]
        private static List<Delegate> m_invocationList;
        private const bool __Dummy_ = false;
        private static Action<string> m_profilerBegin = new Action<string>(<>c.<>9.<.cctor>b__18_0);
        private static Action<int> m_profilerEnd = new Action<int>(<>c.<>9.<.cctor>b__18_1);
        private static ConcurrentDictionary<MethodInfo, string> m_methodNameCache = new ConcurrentDictionary<MethodInfo, string>(MyEnvironment.ProcessorCount * 4, 100);

        public static void GetInvocationList(this MulticastDelegate @delegate, List<Delegate> result)
        {
            if (m_getInvocationList == null)
            {
                FieldInfo field = typeof(MulticastDelegate).GetField("_invocationList", ((BindingFlags) BindingFlags.NonPublic) | ((BindingFlags) BindingFlags.Instance));
                if (field == null)
                {
                    field = typeof(MulticastDelegate).GetField("delegates", ((BindingFlags) BindingFlags.NonPublic) | ((BindingFlags) BindingFlags.Instance));
                }
                Func<object, object> getter = field.CreateGetter<object, object>();
                m_getInvocationList = x => getter(x) as object[];
            }
            if (m_getInvocationCount == null)
            {
                FieldInfo field = typeof(MulticastDelegate).GetField("_invocationCount", ((BindingFlags) BindingFlags.NonPublic) | ((BindingFlags) BindingFlags.Instance));
                if (field != null)
                {
                    Func<object, IntPtr> func1 = field.CreateGetter<object, IntPtr>();
                    m_getInvocationCount = x => (int) func1(x);
                }
                else
                {
                    m_getInvocationCount = x => m_getInvocationList(x).Length;
                }
            }
            object[] objArray = m_getInvocationList(@delegate);
            if (objArray == null)
            {
                result.Add((Delegate) @delegate);
            }
            else
            {
                int num = m_getInvocationCount(@delegate);
                for (int i = 0; i < num; i++)
                {
                    result.Add((Delegate) objArray[i]);
                }
            }
        }

        private static string GetMethodName(MethodInfo method)
        {
            if (!m_methodNameCache.TryGetValue(method, out string orAdd))
            {
                string str2 = method.IsStatic ? "." : "#";
                orAdd = method.get_DeclaringType().Name + str2 + method.Name;
                orAdd = m_methodNameCache.GetOrAdd(method, orAdd);
            }
            return orAdd;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeIfNotNull(this Action handler)
        {
            if (handler != null)
            {
                handler();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeIfNotNull<T1>(this Action<T1> handler, T1 arg1)
        {
            if (handler != null)
            {
                handler(arg1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeIfNotNull<T1, T2>(this Action<T1, T2> handler, T1 arg1, T2 arg2)
        {
            if (handler != null)
            {
                handler(arg1, arg2);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeIfNotNull<T1, T2, T3>(this Action<T1, T2, T3> handler, T1 arg1, T2 arg2, T3 arg3)
        {
            if (handler != null)
            {
                handler(arg1, arg2, arg3);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeIfNotNull<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (handler != null)
            {
                handler(arg1, arg2, arg3, arg4);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeIfNotNull<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (handler != null)
            {
                handler(arg1, arg2, arg3, arg4, arg5);
            }
        }

        private static void InvokeProfiled<TDelegate, T1, T2, T3, T4, T5>(Action<TDelegate, T1, T2, T3, T4, T5> handler, TDelegate @delegate, T1 _1, T2 _2, T3 _3, T4 _4, T5 _5)
        {
            if (m_invocationList == null)
            {
                m_invocationList = new List<Delegate>();
            }
            int count = m_invocationList.Count;
            m_profilerBegin("DelegateInvoke");
            try
            {
                ((MulticastDelegate) @delegate).GetInvocationList(m_invocationList);
                int num2 = m_invocationList.Count;
                for (int i = count; i < num2; i++)
                {
                    Delegate delegate2 = m_invocationList[i];
                    m_profilerBegin(GetMethodName(delegate2.get_Method()));
                    try
                    {
                        handler((TDelegate) delegate2, _1, _2, _3, _4, _5);
                    }
                    finally
                    {
                        m_profilerEnd(0);
                    }
                }
            }
            finally
            {
                int num4 = m_invocationList.Count - count;
                m_invocationList.RemoveRange(count, num4);
                m_profilerEnd(num4);
            }
        }

        public static void SetupProfiler(Action<string> begin, Action<int> end)
        {
            m_profilerBegin = begin;
            m_profilerEnd = end;
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly DelegateExtensions.<>c <>9 = new DelegateExtensions.<>c();
            public static Action<Action, bool, bool, bool, bool, bool> <>9__1_0;
            public static Func<object, int> <>9__9_1;

            internal void <.cctor>b__18_0(string x)
            {
            }

            internal void <.cctor>b__18_1(int x)
            {
            }

            internal int <GetInvocationList>b__9_1(object x) => 
                DelegateExtensions.m_getInvocationList(x).Length;

            internal void <InvokeIfNotNull>b__1_0(Action _, bool _1, bool _2, bool _3, bool _4, bool _5)
            {
                _();
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__2<T1>
        {
            public static readonly DelegateExtensions.<>c__2<T1> <>9;
            public static Action<Action<T1>, T1, bool, bool, bool, bool> <>9__2_0;

            static <>c__2()
            {
                DelegateExtensions.<>c__2<T1>.<>9 = new DelegateExtensions.<>c__2<T1>();
            }

            internal void <InvokeIfNotNull>b__2_0(Action<T1> _, T1 _1, bool _2, bool _3, bool _4, bool _5)
            {
                _(_1);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__3<T1, T2>
        {
            public static readonly DelegateExtensions.<>c__3<T1, T2> <>9;
            public static Action<Action<T1, T2>, T1, T2, bool, bool, bool> <>9__3_0;

            static <>c__3()
            {
                DelegateExtensions.<>c__3<T1, T2>.<>9 = new DelegateExtensions.<>c__3<T1, T2>();
            }

            internal void <InvokeIfNotNull>b__3_0(Action<T1, T2> _, T1 _1, T2 _2, bool _3, bool _4, bool _5)
            {
                _(_1, _2);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__4<T1, T2, T3>
        {
            public static readonly DelegateExtensions.<>c__4<T1, T2, T3> <>9;
            public static Action<Action<T1, T2, T3>, T1, T2, T3, bool, bool> <>9__4_0;

            static <>c__4()
            {
                DelegateExtensions.<>c__4<T1, T2, T3>.<>9 = new DelegateExtensions.<>c__4<T1, T2, T3>();
            }

            internal void <InvokeIfNotNull>b__4_0(Action<T1, T2, T3> _, T1 _1, T2 _2, T3 _3, bool _4, bool _5)
            {
                _(_1, _2, _3);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__5<T1, T2, T3, T4>
        {
            public static readonly DelegateExtensions.<>c__5<T1, T2, T3, T4> <>9;
            public static Action<Action<T1, T2, T3, T4>, T1, T2, T3, T4, bool> <>9__5_0;

            static <>c__5()
            {
                DelegateExtensions.<>c__5<T1, T2, T3, T4>.<>9 = new DelegateExtensions.<>c__5<T1, T2, T3, T4>();
            }

            internal void <InvokeIfNotNull>b__5_0(Action<T1, T2, T3, T4> _, T1 _1, T2 _2, T3 _3, T4 _4, bool _5)
            {
                _(_1, _2, _3, _4);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__6<T1, T2, T3, T4, T5>
        {
            public static readonly DelegateExtensions.<>c__6<T1, T2, T3, T4, T5> <>9;
            public static Action<Action<T1, T2, T3, T4, T5>, T1, T2, T3, T4, T5> <>9__6_0;

            static <>c__6()
            {
                DelegateExtensions.<>c__6<T1, T2, T3, T4, T5>.<>9 = new DelegateExtensions.<>c__6<T1, T2, T3, T4, T5>();
            }

            internal void <InvokeIfNotNull>b__6_0(Action<T1, T2, T3, T4, T5> _, T1 _1, T2 _2, T3 _3, T4 _4, T5 _5)
            {
                _(_1, _2, _3, _4, _5);
            }
        }
    }
}

