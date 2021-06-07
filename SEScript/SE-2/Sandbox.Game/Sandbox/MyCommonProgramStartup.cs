using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilder;
using VRage.GameServices;
using VRage.Library;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox
{
	public class MyCommonProgramStartup
	{
		private string[] m_args;

		private static IMyRender m_renderer;

		private MyBasicGameInfo GameInfo => MyPerGameSettings.BasicGameInfo;

		public static void MessageBoxWrapper(string caption, string text)
		{
			MyVRage.Platform.MessageBox(text, caption, MessageBoxOptions.OkOnly);
		}

		public MyCommonProgramStartup(string[] args)
		{
			int? gameVersion = GameInfo.GameVersion;
			MyFinalBuildConstants.APP_VERSION = (gameVersion.HasValue ? ((MyVersion)gameVersion.GetValueOrDefault()) : null);
			m_args = args;
		}

		public bool PerformReporting()
		{
			if (MyVRage.Platform.CrashReporting.ExtractCrashAnalyticsReport(m_args, out string logPath, out string gameName, out bool isGpuError, out bool exitAfterReport))
			{
				if (isGpuError)
				{
					string errorMessage = string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_MESSAGE_DX11_NOT_AVAILABLE).Replace("\\n", "\r\n"), m_args[1], m_args[2], GameInfo.MinimumRequirementsWeb);
					MyErrorReporter.Report(logPath, gameName, GameInfo.GameAcronym, errorMessage);
				}
				else
				{
					MyErrorReporter.ReportGeneral(logPath, gameName, GameInfo.GameAcronym);
				}
				return exitAfterReport;
			}
			return false;
		}

		public void PerformNotInteractiveReport()
		{
			MyErrorReporter.ReportNotInteractive(MyLog.Default.GetFilePath(), GameInfo.GameAcronym);
		}

		public void PerformAutoconnect()
		{
			if (MyFakes.ENABLE_CONNECT_COMMAND_LINE && m_args.Contains("+connect"))
			{
				int num = Array.IndexOf(m_args, "+connect");
				if (num + 1 < m_args.Length && IPAddressExtensions.TryParseEndpoint(m_args[num + 1], out Sandbox.Engine.Platform.Game.ConnectToServer))
				{
					Console.WriteLine(GameInfo.GameName + " " + MyFinalBuildConstants.APP_VERSION_STRING);
					Console.WriteLine("Obfuscated: " + MyObfuscation.Enabled + ", Platform: " + (MyEnvironment.Is64BitProcess ? " 64-bit" : " 32-bit"));
					Console.WriteLine("Connecting to: " + m_args[num + 1]);
				}
			}
		}

		public bool PerformColdStart()
		{
			if (m_args.Contains("-coldstart"))
			{
				MyGlobalTypeMetadata.Static.Init(loadSerializersAsync: false);
				Parallel.Scheduler = new PrioritizedScheduler(1, amd: false);
				int num = -1;
				List<string> list = new List<string>();
				Queue<AssemblyName> queue = new Queue<AssemblyName>();
				queue.Enqueue(Assembly.GetEntryAssembly().GetName());
				while (queue.Count > 0)
				{
					AssemblyName assemblyName = queue.Dequeue();
					if (!list.Contains(assemblyName.FullName))
					{
						list.Add(assemblyName.FullName);
						try
						{
							Assembly assembly = Assembly.Load(assemblyName);
							PreloadTypesFrom(assembly);
							num++;
							AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
							foreach (AssemblyName item in referencedAssemblies)
							{
								queue.Enqueue(item);
							}
						}
						catch (Exception)
						{
						}
					}
				}
				if (MyFakes.ENABLE_NGEN)
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "ngen"))
					{
						Verb = "runas"
					};
					processStartInfo.Arguments = "install SpaceEngineers.exe /silent /nologo";
					processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					try
					{
						Process.Start(processStartInfo).WaitForExit();
					}
					catch (Exception arg)
					{
						MyLog.Default.WriteLine("NGEN failed: " + arg);
					}
				}
				return true;
			}
			return false;
		}

		public bool IsRenderUpdateSyncEnabled()
		{
			return m_args.Contains("-render_sync");
		}

		public bool IsVideoRecordingEnabled()
		{
			return m_args.Contains("-video_record");
		}

		private static void PreloadTypesFrom(Assembly assembly)
		{
			if (assembly != null)
			{
				ForceStaticCtor((from type in assembly.GetTypes()
					where Attribute.IsDefined(type, typeof(PreloadRequiredAttribute))
					select type).ToArray());
			}
		}

		public static void ForceStaticCtor(Type[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				RuntimeHelpers.RunClassConstructor(types[i].TypeHandle);
			}
		}

		public string GetAppDataPath()
		{
			string result = null;
			int num = Array.IndexOf(m_args, "-appdata") + 1;
			if (num != 0 && m_args.Length > num)
			{
				string text = m_args[num];
				if (!text.StartsWith("-"))
				{
					result = Path.GetFullPath(Environment.ExpandEnvironmentVariables(text));
				}
			}
			return result;
		}

		public void InitSplashScreen()
		{
			if (MyFakes.ENABLE_SPLASHSCREEN && !m_args.Contains("-nosplash"))
			{
				MyVRage.Platform.ShowSplashScreen(GameInfo.SplashScreenImage, new Vector2(0.7f, 0.7f));
			}
		}

		public void DisposeSplashScreen()
		{
			MyVRage.Platform.HideSplashScreen();
		}

		public bool Check64Bit()
		{
			if (!MyEnvironment.Is64BitProcess && AssemblyExtensions.TryGetArchitecture("SteamSDK.dll") == ProcessorArchitecture.Amd64)
			{
				string str = GameInfo.GameName + " cannot be started in 64-bit mode, ";
				str = str + "because 64-bit version of .NET framework is not available or is broken." + MyEnvironment.NewLine + MyEnvironment.NewLine;
				str = str + "Do you want to open website with more information about this particular issue?" + MyEnvironment.NewLine + MyEnvironment.NewLine;
				str = str + "Press Yes to open website with info" + MyEnvironment.NewLine;
				str = str + "Press No to run in 32-bit mode (smaller potential of " + GameInfo.GameName + "!)" + MyEnvironment.NewLine;
				str += "Press Cancel to close this dialog";
				switch (MyMessageBox.Show(str, ".NET Framework 64-bit error", MessageBoxOptions.YesNoCancel))
				{
				case MessageBoxResult.Yes:
					MyVRage.Platform.OpenUrl("http://www.spaceengineersgame.com/64-bit-start-up-issue.html");
					break;
				case MessageBoxResult.No:
				{
					string location = Assembly.GetEntryAssembly().Location;
					string text = Path.Combine(new FileInfo(location).Directory.Parent.FullName, "Bin", Path.GetFileName(location));
					Process.Start(new ProcessStartInfo
					{
						FileName = text,
						WorkingDirectory = Path.GetDirectoryName(text),
						Arguments = "-fallback",
						UseShellExecute = false,
						WindowStyle = ProcessWindowStyle.Normal
					});
					break;
				}
				}
				return false;
			}
			return true;
		}

		public bool CheckSteamRunning()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				if (MyGameService.IsActive)
				{
					MyGameService.SetNotificationPosition(NotificationPosition.TopLeft);
					MySandboxGame.Log.WriteLineAndConsole("Steam.IsActive: " + MyGameService.IsActive);
					MySandboxGame.Log.WriteLineAndConsole("Steam.IsOnline: " + MyGameService.IsOnline);
					MySandboxGame.Log.WriteLineAndConsole("Steam.OwnsGame: " + MyGameService.OwnsGame);
					MySandboxGame.Log.WriteLineAndConsole("Steam.UserId: " + MyGameService.UserId);
					MySandboxGame.Log.WriteLineAndConsole("Steam.UserName: " + MyGameService.UserName);
					MySandboxGame.Log.WriteLineAndConsole("Steam.Branch: " + MyGameService.BranchName);
					MySandboxGame.Log.WriteLineAndConsole("Build date: " + MySandboxGame.BuildDateTime.ToString("yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture));
					MySandboxGame.Log.WriteLineAndConsole("Build version: " + MySandboxGame.BuildVersion.ToString());
				}
				else if ((!MyGameService.IsActive || !MyGameService.OwnsGame) && !MyFakes.ENABLE_RUN_WITHOUT_STEAM)
				{
					MessageBoxWrapper("Steam is not running!", "Please run this game from Steam." + MyEnvironment.NewLine + "(restart Steam if already running)");
					return false;
				}
			}
			return true;
		}
	}
}
