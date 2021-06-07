using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Library.Extensions;
using VRage.Library.Threading;
using VRage.Scripting.CompilerMethods;
using VRage.Utils;

namespace VRage.Scripting
{
	public static class MyModWatchdog
	{
		public struct ModInfoT
		{
			public readonly string FriendlyName;

			public ModInfoT(string name)
			{
				FriendlyName = name;
			}
		}

		public struct RuntimeInfoT
		{
			public int RootModId;

			public int CallStackDepth;
		}

		private static int ModIdAllocator;

		public static ModInfoT[] ModInfo;

		private static SpinLockRef ModInfoLock;

		[ThreadStatic]
		private static RuntimeInfoT RuntimeInfo;

		public static ConcurrentDictionary<long, MyTuple<string, MyStringId>> Warnings
		{
			get;
			private set;
		}

		static MyModWatchdog()
		{
			ModIdAllocator = 0;
			ModInfoLock = new SpinLockRef();
			Warnings = new ConcurrentDictionary<long, MyTuple<string, MyStringId>>();
		}

		public static void Init(Thread updateThread)
		{
			ModPerfCounter.InitializeUpdateThread(updateThread);
		}

		public static bool ReportIncorrectBehaviour(MyStringId message)
		{
			int rootModId = RuntimeInfo.RootModId;
			int num = message.Id | rootModId;
			if (Warnings.ContainsKey(num))
			{
				return false;
			}
			string friendlyName = ModInfo[rootModId].FriendlyName;
			if (Warnings.TryAdd(num, MyTuple.Create(friendlyName, message)))
			{
				return false;
			}
			string format = string.Format(MyTexts.GetString(message), friendlyName);
			MyLog.Default.Log(MyLogSeverity.Error, format);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ModMethodEnter(int modId)
		{
			if (RuntimeInfo.CallStackDepth++ == 0)
			{
				RuntimeInfo.RootModId = modId;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ModMethodExit()
		{
			RuntimeInfo.CallStackDepth--;
			_ = 0;
		}

		public static int AllocateModId(string modName)
		{
			if (modName == null)
			{
				modName = "No Name";
			}
			using (ModInfoLock.Acquire())
			{
				Warnings.Clear();
				int num = ModIdAllocator++;
				MyArrayHelpers.InitOrReserve(ref ModInfo, num + 1);
				ModInfo[num] = new ModInfoT(modName);
				ModPerfCounter.InitModInfo(num);
				return num;
			}
		}
	}
}
