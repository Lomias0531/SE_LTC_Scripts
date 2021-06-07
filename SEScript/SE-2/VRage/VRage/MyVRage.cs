using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRage.Collections;
using VRage.Profiler;
using VRage.Serialization;

namespace VRage
{
	public static class MyVRage
	{
		private static readonly List<Action> m_exitCallbacks = new List<Action>();

		public const string ProtobufferExtension = "B5";

		public static bool EnableMemoryPooling = true;

		public static IVRagePlatform Platform
		{
			get;
			private set;
		}

		public static void Init(IVRagePlatform platform)
		{
			Platform = platform;
			InitSettings();
			MyProfilerBlock.GetThreadAllocationStamp = Platform.GetThreadAllocationStamp;
			DependencyBatch.ErrorReportingFunction = (WorkItem.ErrorReportingFunction = MyMiniDump.CollectExceptionDump);
			ExpressionExtension.Factory = new PrecompiledActivatorFactory();
		}

		public static void Done()
		{
			foreach (Action exitCallback in m_exitCallbacks)
			{
				exitCallback();
			}
			m_exitCallbacks.Clear();
			Platform.Done();
			MyConcurrentBucketPool.OnExit();
		}

		public static void RegisterExitCallback(Action callback)
		{
			m_exitCallbacks.Add(callback);
		}

		private static void InitSettings()
		{
			MyConcurrentBucketPool.EnablePooling = EnableMemoryPooling;
		}
	}
}
