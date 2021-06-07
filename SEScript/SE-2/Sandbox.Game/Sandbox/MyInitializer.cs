#define VRAGE
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using VRage;
using VRage.Common.Utils;
using VRage.Cryptography;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Library;
using VRage.Utils;
using VRageRender;

namespace Sandbox
{
	public static class MyInitializer
	{
		private static string m_appName;

		private static HashSet<string> m_ignoreList = new HashSet<string>();

		private static object m_exceptionSyncRoot = new object();

		private static IMyCrashReporting ErrorPlatform => MyVRage.Platform.CrashReporting;

		private static void ChecksumFailed(string filename, string hash)
		{
			if (!m_ignoreList.Contains(filename))
			{
				m_ignoreList.Add(filename);
				MySandboxGame.Log.WriteLine("Error: checksum of file '" + filename + "' failed " + hash);
			}
		}

		private static void ChecksumNotFound(IFileVerifier verifier, string filename)
		{
			MyChecksumVerifier myChecksumVerifier = (MyChecksumVerifier)verifier;
			if (!m_ignoreList.Contains(filename) && filename.StartsWith(myChecksumVerifier.BaseChecksumDir, StringComparison.InvariantCultureIgnoreCase) && filename.Substring(Math.Min(filename.Length, myChecksumVerifier.BaseChecksumDir.Length + 1)).StartsWith("Data", StringComparison.InvariantCultureIgnoreCase))
			{
				MySandboxGame.Log.WriteLine("Error: no checksum found for file '" + filename + "'");
				m_ignoreList.Add(filename);
			}
		}

		public static void InvokeBeforeRun(uint appId, string appName, string userDataPath, bool addDateToLog = false)
		{
			m_appName = appName;
			StringBuilder logName = GetLogName(appName, addDateToLog);
			string rootPath = MyFileSystem.RootPath;
			MyFileSystem.Init(Path.Combine(rootPath, "Content"), userDataPath);
			MySandboxGame.Log.Init(logName.ToString(), MyFinalBuildConstants.APP_VERSION_STRING);
			MyLog.Default = MySandboxGame.Log;
			InitExceptionHandling();
			bool flag = SteamHelpers.IsSteamPath(rootPath);
			bool flag2 = SteamHelpers.IsAppManifestPresent(rootPath, appId);
			Sandbox.Engine.Platform.Game.IsPirated = (!flag && !flag2);
			MySandboxGame.Log.WriteLine("Steam build: Always true");
			MySandboxGame.Log.WriteLineAndConsole(string.Format("Is official: {0} {1}{2}{3}", true, MyObfuscation.Enabled ? "[O]" : "[NO]", flag ? "[IS]" : "[NIS]", flag2 ? "[AMP]" : "[NAMP]"));
			MySandboxGame.Log.WriteLineAndConsole("Environment.ProcessorCount: " + MyEnvironment.ProcessorCount);
			MySandboxGame.Log.WriteLineAndConsole("Environment.OSVersion: " + MyVRage.Platform.GetOsName());
			MySandboxGame.Log.WriteLineAndConsole("Environment.CommandLine: " + Environment.CommandLine);
			MySandboxGame.Log.WriteLineAndConsole("Environment.Is64BitProcess: " + MyEnvironment.Is64BitProcess);
			MySandboxGame.Log.WriteLineAndConsole("Environment.Is64BitOperatingSystem: " + Environment.Is64BitOperatingSystem);
			MySandboxGame.Log.WriteLineAndConsole("Environment.Version: " + RuntimeInformation.FrameworkDescription);
			MySandboxGame.Log.WriteLineAndConsole("Environment.CurrentDirectory: " + Environment.CurrentDirectory);
			MySandboxGame.Log.WriteLineAndConsole("CPU Info: " + MyVRage.Platform.GetInfoCPU(out uint frequency));
			MySandboxGame.CPUFrequency = frequency;
			MySandboxGame.Log.WriteLine("IntPtr.Size: " + IntPtr.Size);
			MySandboxGame.Log.WriteLine("Default Culture: " + CultureInfo.CurrentCulture.Name);
			MySandboxGame.Log.WriteLine("Default UI Culture: " + CultureInfo.CurrentUICulture.Name);
			MySandboxGame.Config = new MyConfig(appName + ".cfg");
			MySandboxGame.Config.Load();
		}

		public static void InitExceptionHandling()
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
			ErrorPlatform.SetNativeExceptionHandler(delegate(IntPtr x)
			{
				ProcessUnhandledException(null, x);
			});
			Thread.CurrentThread.Name = "Main thread";
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			if (MyFakes.ENABLE_MINIDUMP_SENDING && MyFileSystem.IsInitialized)
			{
				MyMiniDump.CleanupOldDumps();
			}
			ErrorPlatform.CleanupCrashAnalytics();
		}

		public static StringBuilder GetLogName(string appName, bool addDateToLog)
		{
			StringBuilder stringBuilder = new StringBuilder(appName);
			if (addDateToLog)
			{
				stringBuilder.Append("_");
				stringBuilder.Append((object)new StringBuilder().GetFormatedDateTimeForFilename(DateTime.Now));
			}
			stringBuilder.Append(".log");
			return stringBuilder;
		}

		public static void InvokeAfterRun()
		{
			MySandboxGame.Log.Close();
		}

		public static void InitCheckSum()
		{
			try
			{
				string path = Path.Combine(MyFileSystem.ContentPath, "checksum.xml");
				if (!File.Exists(path))
				{
					MySandboxGame.Log.WriteLine("Checksum file is missing, game will run as usual but file integrity won't be verified");
				}
				else
				{
					using (FileStream fileStream = File.OpenRead(path))
					{
						MyChecksums myChecksums = (MyChecksums)new XmlSerializer(typeof(MyChecksums)).Deserialize(fileStream);
						MyChecksumVerifier myChecksumVerifier = new MyChecksumVerifier(myChecksums, MyFileSystem.ContentPath);
						myChecksumVerifier.ChecksumFailed += ChecksumFailed;
						myChecksumVerifier.ChecksumNotFound += ChecksumNotFound;
						fileStream.Position = 0L;
						SHA256 sHA = MySHA256.Create();
						sHA.Initialize();
						byte[] inArray = sHA.ComputeHash(fileStream);
						string b = "BgIAAACkAABSU0ExAAQAAAEAAQClSibD83Y6Akok8tAtkbMz4IpueWFra0QkkKcodwe2pV/RJAfyq5mLUGsF3JdTdu3VWn93VM+ZpL9CcMKS8HaaHmBZJn7k2yxNvU4SD+8PhiZ87iPqpkN2V+rz9nyPWTHDTgadYMmenKk2r7w4oYOooo5WXdkTVjAD50MroAONuQ==";
						MySandboxGame.Log.WriteLine("Checksum file hash: " + Convert.ToBase64String(inArray));
						MySandboxGame.Log.WriteLine($"Checksum public key valid: {myChecksums.PublicKey == b}, Key: {myChecksums.PublicKey}");
						MyFileSystem.FileVerifier = myChecksumVerifier;
					}
				}
			}
			catch
			{
			}
		}

		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			ProcessUnhandledException(args.ExceptionObject as Exception, IntPtr.Zero);
		}

		private static void ProcessUnhandledException(Exception exception, IntPtr exceptionPointers)
		{
			if (exception != null)
			{
				MyVRage.Platform.LogToExternalDebugger("Unhandled managed exception: " + Environment.NewLine + exception);
				MySandboxGame.Log.AppendToClosedLog(exception);
				HandleSpecialExceptions(exception);
			}
			else
			{
				MyVRage.Platform.LogToExternalDebugger("Unhandled native exception: " + Environment.NewLine + Environment.StackTrace);
				MySandboxGame.Log.AppendToClosedLog("Native exception occured: " + Environment.NewLine + Environment.StackTrace);
			}
			lock (m_exceptionSyncRoot)
			{
				MySandboxGame.Log.AppendToClosedLog("Showing message");
				if (!Sandbox.Engine.Platform.Game.IsDedicated || MyPerGameSettings.SendLogToKeen)
				{
					OnCrash(MySandboxGame.Log.GetFilePath(), MyPerGameSettings.GameName, MyPerGameSettings.MinimumRequirementsPage, MyPerGameSettings.RequiresDX11, exception, exceptionPointers);
				}
				MySandboxGame.Log.Close();
				MySimpleProfiler.LogPerformanceTestResults();
				if (!Debugger.IsAttached)
				{
					AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionHandler;
					ErrorPlatform.SetNativeExceptionHandler(null);
					ErrorPlatform.ExitProcessOnCrash(exception);
				}
			}
		}

		private static void HandleSpecialExceptions(Exception exception)
		{
			if (exception == null)
			{
				return;
			}
			ReflectionTypeLoadException ex;
			if ((ex = (exception as ReflectionTypeLoadException)) == null)
			{
				OutOfMemoryException ex2;
				if ((ex2 = (exception as OutOfMemoryException)) != null)
				{
					OutOfMemoryException e = ex2;
					MySandboxGame.Log.AppendToClosedLog("Handling out of memory exception... " + MySandboxGame.Config);
					if (MySandboxGame.Config.LowMemSwitchToLow == MyConfig.LowMemSwitch.ARMED && !MySandboxGame.Config.IsSetToLowQuality())
					{
						MySandboxGame.Log.AppendToClosedLog("Creating switch to low request");
						MySandboxGame.Config.LowMemSwitchToLow = MyConfig.LowMemSwitch.TRIGGERED;
						MySandboxGame.Config.Save();
						MySandboxGame.Log.AppendToClosedLog("Switch to low request created");
					}
					MySandboxGame.Log.AppendToClosedLog(e);
				}
			}
			else
			{
				Exception[] loaderExceptions = ex.LoaderExceptions;
				foreach (Exception e2 in loaderExceptions)
				{
					MySandboxGame.Log.AppendToClosedLog(e2);
				}
			}
			HandleSpecialExceptions(exception.InnerException);
		}

		private static bool IsModCrash(Exception e)
		{
			return e is ModCrashedException;
		}

		private static void OnCrash(string logPath, string gameName, string minimumRequirementsPage, bool requiresDX11, Exception exception, IntPtr exceptionPointers)
		{
			try
			{
				ExceptionType exceptionType = ErrorPlatform.GetExceptionType(exception);
				if (MyVideoSettingsManager.GpuUnderMinimum)
				{
					MyErrorReporter.ReportGpuUnderMinimumCrash(gameName, logPath, minimumRequirementsPage);
				}
				else if (!Sandbox.Engine.Platform.Game.IsDedicated && exceptionType == ExceptionType.OutOfMemory)
				{
					MyErrorReporter.ReportOutOfMemory(gameName, logPath, minimumRequirementsPage);
				}
				else if (!Sandbox.Engine.Platform.Game.IsDedicated && exceptionType == ExceptionType.OutOfVideoMemory)
				{
					MyErrorReporter.ReportOutOfVideoMemory(gameName, logPath, minimumRequirementsPage);
				}
				else
				{
					bool result = false;
					if (exception != null && exception.Data.Contains("Silent"))
					{
						bool.TryParse((string)exception.Data["Silent"], out result);
					}
					if (MyFakes.ENABLE_MINIDUMP_SENDING)
					{
						MyMiniDump.CollectCrashDump(exceptionPointers);
					}
					if (!result && !Debugger.IsAttached)
					{
						if (IsModCrash(exception))
						{
							ModCrashedException ex = (ModCrashedException)exception;
							MyModCrashScreenTexts myModCrashScreenTexts = default(MyModCrashScreenTexts);
							myModCrashScreenTexts.ModName = ex.ModContext.ModName;
							myModCrashScreenTexts.ModId = ex.ModContext.ModId;
							myModCrashScreenTexts.LogPath = logPath;
							myModCrashScreenTexts.Close = MyTexts.GetString(MyCommonTexts.Close);
							myModCrashScreenTexts.Text = MyTexts.GetString(MyCommonTexts.ModCrashedTheGame);
							myModCrashScreenTexts.Info = MyTexts.GetString(MyCommonTexts.ModCrashedTheGameInfo);
							MyModCrashScreenTexts texts = myModCrashScreenTexts;
							ErrorPlatform.MessageBoxModCrashForm(ref texts);
						}
						else
						{
							CLoseLog(MySandboxGame.Log);
							CLoseLog(MyRenderProxy.Log);
							bool gdprConsent = MySandboxGame.Config?.GDPRConsent.GetValueOrDefault(false) ?? true;
							ErrorPlatform.PrepareCrashAnalyticsReporting(logPath, gameName, requiresDX11 && exceptionType == ExceptionType.UnsupportedGpu, gdprConsent);
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				if (!Debugger.IsAttached)
				{
					try
					{
						if (MySpaceAnalytics.Instance != null)
						{
							MySpaceAnalytics.Instance.ReportGameCrash(exception);
							MySpaceAnalytics.Instance.EndSession();
							MyAnalyticsManager.Instance.FlushAndDispose();
						}
					}
					catch
					{
					}
				}
			}
			void CLoseLog(MyLog log)
			{
				try
				{
					log.Flush();
					log.Close();
				}
				catch
				{
				}
			}
		}
	}
}
