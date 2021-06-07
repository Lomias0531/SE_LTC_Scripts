using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Library.Extensions;

namespace VRage.Scripting.CompilerMethods
{
	public static class ModPerfCounter
	{
		private static Thread MainThread;

		private static int[] MainThreadCallStackDepth;

		private static bool IsMainThread => Thread.CurrentThread == MainThread;

		public static void InitModInfo(int modId)
		{
			MyArrayHelpers.InitOrReserve(ref MainThreadCallStackDepth, modId + 1);
			MainThreadCallStackDepth[modId] = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EnterMethod(int modId)
		{
			MyModWatchdog.ModMethodEnter(modId);
			if (IsMainThread && MainThreadCallStackDepth[modId]++ == 0)
			{
				MySimpleProfiler.Begin(MyModWatchdog.ModInfo[modId].FriendlyName, MySimpleProfiler.ProfilingBlockType.MOD, "EnterMethod");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExitMethod(int modId)
		{
			MyModWatchdog.ModMethodExit();
			if (IsMainThread && --MainThreadCallStackDepth[modId] == 0)
			{
				MySimpleProfiler.EndNoMemberPairingCheck();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReenterYieldMethod(int modId)
		{
			EnterMethod(modId);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T YieldGuard<T>(int modId, T value)
		{
			ExitMethod(modId);
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EnterMethod_Profile(int modId, [CallerMemberName] string memberName = "", [CallerLineNumber] int line = 0)
		{
			EnterMethod(modId);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExitMethod_Profile(int modId, [CallerMemberName] string memberName = "", [CallerLineNumber] int line = 0)
		{
			ExitMethod(modId);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ReenterYieldMethod_Profile(int modId, [CallerMemberName] string memberName = "", [CallerLineNumber] int line = 0)
		{
			ReenterYieldMethod(modId);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T YieldGuard_Profile<T>(int modId, T value, [CallerMemberName] string memberName = "", [CallerLineNumber] int line = 0)
		{
			return YieldGuard(modId, value);
		}

		private static string BlockName(int modId, string memberName, int line)
		{
			string text = MyModWatchdog.ModInfo[modId].FriendlyName;
			if (text.Length > 5)
			{
				text = text.Substring(0, 5);
			}
			return $"{text}_{memberName}:{line}";
		}

		internal static void InitializeUpdateThread(Thread updateThread)
		{
			MainThread = updateThread;
		}
	}
}
