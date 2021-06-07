using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.FileSystem;

namespace VRage
{
	public static class MyMiniDump
	{
		[Flags]
		public enum Options : uint
		{
			Normal = 0x0,
			WithDataSegs = 0x1,
			WithFullMemory = 0x2,
			WithHandleData = 0x4,
			FilterMemory = 0x8,
			ScanMemory = 0x10,
			WithUnloadedModules = 0x20,
			WithIndirectlyReferencedMemory = 0x40,
			FilterModulePaths = 0x80,
			WithProcessThreadData = 0x100,
			WithPrivateReadWriteMemory = 0x200,
			WithoutOptionalData = 0x400,
			WithFullMemoryInfo = 0x800,
			WithThreadInfo = 0x1000,
			WithCodeSegs = 0x2000,
			WithoutAuxiliaryState = 0x4000,
			WithFullAuxiliaryState = 0x8000,
			WithPrivateWriteCopyMemory = 0x10000,
			IgnoreInaccessibleMemory = 0x20000
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct ExceptionInformation
		{
			public uint ThreadId;

			public IntPtr ExceptionPointers;

			[MarshalAs(UnmanagedType.Bool)]
			public bool ClientPointers;
		}

		[ThreadStatic]
		private static long m_lastDumpTimestamp;

		public static void CollectExceptionDump(Exception ex)
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime d = new DateTime(m_lastDumpTimestamp);
			if ((utcNow - d).TotalSeconds > 15.0)
			{
				string dumpPath = Path.Combine(MyFileSystem.UserDataPath, "MinidumpT" + Thread.CurrentThread.ManagedThreadId + ".dmp");
				Options dumpFlags = Options.WithProcessThreadData | Options.WithThreadInfo;
				MyVRage.Platform.CrashReporting.WriteMiniDump(dumpPath, dumpFlags, IntPtr.Zero);
				m_lastDumpTimestamp = utcNow.Ticks;
			}
		}

		public static void CollectCrashDump(IntPtr exceptionPointers)
		{
			string dumpPath = Path.Combine(MyFileSystem.UserDataPath, "Minidump.dmp");
			Options dumpFlags = Options.WithProcessThreadData | Options.WithThreadInfo;
			MyVRage.Platform.CrashReporting.WriteMiniDump(dumpPath, dumpFlags, exceptionPointers);
		}

		public static IEnumerable<string> FindActiveDumps(string directory)
		{
			DateTime now = DateTime.Now;
			string[] files = Directory.GetFiles(directory, "Minidump*.dmp", SearchOption.TopDirectoryOnly);
			foreach (string text in files)
			{
				if (text != null && File.Exists(text) && (File.GetCreationTime(text) - now).Minutes < 5)
				{
					yield return text;
				}
			}
		}

		public static void CleanupOldDumps()
		{
			string[] files = Directory.GetFiles(MyFileSystem.UserDataPath, "Minidump*.dmp", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
		}
	}
}
