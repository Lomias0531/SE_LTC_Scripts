using LitJson;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Http;
using VRageRender;

namespace Sandbox
{
	public class MyErrorReporter
	{
		public struct SessionMetadata
		{
			public static readonly SessionMetadata DEFAULT = new SessionMetadata
			{
				UniqueUserIdentifier = Guid.NewGuid().ToString(),
				SessionId = 0L
			};

			public string UniqueUserIdentifier;

			public long SessionId;
		}

		private class MyCrashInfo
		{
			public string ReportVersion = "1.0";

			public string UniqueUserIdentifier = string.Empty;

			public long SessionID;

			public string GameID = string.Empty;

			public string GameVersion = string.Empty;

			public string Feedback = string.Empty;

			public string Email = string.Empty;

			public string ReportType = string.Empty;
		}

		public static string SUPPORT_EMAIL = "support@keenswh.com";

		public static string MESSAGE_BOX_CAPTION = "{LOCG:Error_Message_Caption}";

		public static string APP_ALREADY_RUNNING = "{LOCG:Error_AlreadyRunning}";

		public static string APP_ERROR_CAPTION = "{LOCG:Error_Error_Caption}";

		public static string APP_LOG_REPORT_FAILED = "{LOCG:Error_Failed}";

		public static string APP_LOG_REPORT_THANK_YOU = "{LOCG:Error_ThankYou}";

		public static string APP_ERROR_MESSAGE = "{LOCG:Error_Error_Message}";

		public static string APP_ERROR_MESSAGE_DX11_NOT_AVAILABLE = "{LOCG:Error_DX11}";

		public static string APP_ERROR_MESSAGE_LOW_GPU = "{LOCG:Error_GPU_Low}";

		public static string APP_ERROR_MESSAGE_NOT_DX11_GPU = "{LOCG:Error_GPU_NotDX11}";

		public static string APP_ERROR_MESSAGE_DRIVER_NOT_INSTALLED = "{LOCG:Error_GPU_Drivers}";

		public static string APP_WARNING_MESSAGE_OLD_DRIVER = "{LOCG:Error_GPU_OldDriver}";

		public static string APP_WARNING_MESSAGE_UNSUPPORTED_GPU = "{LOCG:Error_GPU_Unsupported}";

		public static string APP_ERROR_OUT_OF_MEMORY = "{LOCG:Error_OutOfMemmory}";

		public static string APP_ERROR_OUT_OF_VIDEO_MEMORY = "{LOCG:Error_GPU_OutOfMemory}";

		private static bool AllowSendDialog(string gameName, string logfile, string errorMessage)
		{
			return MyMessageBox.Show(string.Format(errorMessage, gameName, logfile), gameName, MessageBoxOptions.YesNo | MessageBoxOptions.IconHand | MessageBoxOptions.IconQuestion) == MessageBoxResult.Yes;
		}

		public static void ReportRendererCrash(string logfile, string gameName, string minimumRequirementsPage, MyRenderExceptionEnum type)
		{
			string format;
			switch (type)
			{
			case MyRenderExceptionEnum.GpuNotSupported:
				format = MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_LOW_GPU).ToString().Replace("\\n", "\r\n");
				break;
			case MyRenderExceptionEnum.DriverNotInstalled:
				format = MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_DRIVER_NOT_INSTALLED).ToString().Replace("\\n", "\r\n");
				break;
			default:
				format = MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_LOW_GPU).ToString().Replace("\\n", "\r\n");
				break;
			}
			MyMessageBox.Show(string.Format(format, logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static MessageBoxResult ReportOldDrivers(string gameName, string cardName, string driverUpdateLink)
		{
			return MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_WARNING_MESSAGE_OLD_DRIVER).ToString().Replace("\\n", "\r\n"), gameName, cardName, driverUpdateLink), gameName, MessageBoxOptions.OkCancel | MessageBoxOptions.AbortRetryIgnore | MessageBoxOptions.IconHand | MessageBoxOptions.IconQuestion);
		}

		public static void ReportNotCompatibleGPU(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_WARNING_MESSAGE_UNSUPPORTED_GPU).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static void ReportNotDX11GPUCrash(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_NOT_DX11_GPU).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static void ReportGpuUnderMinimumCrash(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_LOW_GPU).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static void ReportOutOfMemory(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_ERROR_OUT_OF_MEMORY).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static void ReportOutOfVideoMemory(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_ERROR_OUT_OF_VIDEO_MEMORY).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		private static void MessageBox(string caption, string text)
		{
			MyMessageBox.Show(text, caption, MessageBoxOptions.OkOnly);
		}

		private static bool DisplayCommonError(string logContent)
		{
			ErrorInfo[] infos = MyErrorTexts.Infos;
			foreach (ErrorInfo errorInfo in infos)
			{
				if (logContent.Contains(errorInfo.Match))
				{
					MessageBox(errorInfo.Caption, errorInfo.Message);
					return true;
				}
			}
			return false;
		}

		private static bool LoadAndDisplayCommonError(string logName)
		{
			try
			{
				if (logName != null && File.Exists(logName))
				{
					using (FileStream stream = File.Open(logName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						using (StreamReader streamReader = new StreamReader(stream))
						{
							return DisplayCommonError(streamReader.ReadToEnd());
						}
					}
				}
			}
			catch
			{
			}
			return false;
		}

		private static void ReportInternal(string logName, string gameName, string id, string email = null, string feedback = null)
		{
			if (TrySendReport(logName, id, email, feedback))
			{
				MessageBox(gameName, MyTexts.SubstituteTexts(APP_LOG_REPORT_THANK_YOU).Replace("\\n", "\r\n"));
			}
			else
			{
				MessageBox(string.Format(MyTexts.SubstituteTexts(APP_ERROR_CAPTION).Replace("\\n", "\r\n"), gameName), string.Format(MyTexts.SubstituteTexts(APP_LOG_REPORT_FAILED).Replace("\\n", "\r\n"), gameName, logName, MyTexts.SubstituteTexts(SUPPORT_EMAIL)));
			}
		}

		public static void ReportNotInteractive(string logName, string id)
		{
			TrySendReport(logName, id);
		}

		public static void ReportGeneral(string logName, string gameName, string id)
		{
			if (!string.IsNullOrWhiteSpace(logName) && !LoadAndDisplayCommonError(logName))
			{
				MyCrashScreenTexts myCrashScreenTexts;
				MyCrashScreenTexts texts;
				if (MyTexts.Exists(MyCoreTexts.CrashScreen_MainText))
				{
					myCrashScreenTexts = default(MyCrashScreenTexts);
					myCrashScreenTexts.GameName = gameName;
					myCrashScreenTexts.LogName = logName;
					myCrashScreenTexts.MainText = MyTexts.Get(MyCoreTexts.CrashScreen_MainText).ToString();
					myCrashScreenTexts.Log = MyTexts.Get(MyCoreTexts.CrashScreen_Log).ToString();
					myCrashScreenTexts.EmailText = MyTexts.Get(MyCoreTexts.CrashScreen_EmailText).ToString();
					myCrashScreenTexts.Email = MyTexts.Get(MyCoreTexts.CrashScreen_Email).ToString();
					myCrashScreenTexts.Detail = MyTexts.Get(MyCoreTexts.CrashScreen_Detail).ToString();
					myCrashScreenTexts.Yes = MyTexts.Get(MyCoreTexts.CrashScreen_Yes).ToString();
					texts = myCrashScreenTexts;
				}
				else
				{
					myCrashScreenTexts = default(MyCrashScreenTexts);
					myCrashScreenTexts.GameName = gameName;
					myCrashScreenTexts.LogName = logName;
					myCrashScreenTexts.MainText = "Space Engineers had a problem and crashed! We apologize for the inconvenience. Please click Send Log if you would like to help us analyze and fix the problem. For more information, check the log below";
					myCrashScreenTexts.Log = "log";
					myCrashScreenTexts.EmailText = "Additionally, you can send us your email in case a member of our support staff needs more information about this error. \r\n \r\n If you would not mind being contacted about this issue please provide your e-mail address below. By sending the log, I grant my consent to the processing of my personal data (E-mail, Steam ID and IP address) to Keen SWH LTD. United Kingdom and it subsidiaries, in order for these data to be processed for the purpose of tracking the crash and requesting feedback with the intent to improve the game performance. I grant this consent for an indefinite term until my express revocation thereof. I confirm that I have been informed that the provision of these data is voluntary, and that I have the right to request their deletion. Registration is non-transferable. More information about the processing of my personal data in the scope required by legal regulations, in particular Regulation (EU) 2016/679 of the European Parliament and of the Council, can be found as of 25 May 2018 here. \r\n";
					myCrashScreenTexts.Email = "Email (optional)";
					myCrashScreenTexts.Detail = "To help us resolve the problem, please provide a description of what you were doing when it occurred (optional)";
					myCrashScreenTexts.Yes = "Send Log";
					texts = myCrashScreenTexts;
				}
				if (MyVRage.Platform.CrashReporting.MessageBoxCrashForm(ref texts, out string message, out string email))
				{
					ReportInternal(logName, gameName, id, email, message);
				}
			}
		}

		public static void Report(string logName, string gameName, string id, string errorMessage)
		{
			if (!LoadAndDisplayCommonError(logName) && AllowSendDialog(gameName, logName, errorMessage) && logName != null)
			{
				ReportInternal(logName, gameName, id);
			}
		}

		public static void ReportAppAlreadyRunning(string gameName)
		{
			MyVRage.Platform.MessageBox(string.Format(MyTexts.SubstituteTexts(APP_ALREADY_RUNNING).Replace("\\n", "\r\n"), gameName), string.Format(MyTexts.SubstituteTexts(MESSAGE_BOX_CAPTION).Replace("\\n", "\r\n"), gameName), MessageBoxOptions.OkOnly);
		}

		private static bool TrySendReport(string logName, string gameId, string email = null, string feedback = null)
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(email))
			{
				stringBuilder.AppendLine("Email: " + email);
			}
			if (!string.IsNullOrWhiteSpace(feedback))
			{
				stringBuilder.AppendLine("Feedback: " + feedback);
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			SessionMetadata metadata = SessionMetadata.DEFAULT;
			Tuple<string, string> tuple = null;
			try
			{
				string text = stringBuilder.ToString();
				if (logName != null && File.Exists(logName))
				{
					using (FileStream stream = File.Open(logName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						using (StreamReader streamReader = new StreamReader(stream))
						{
							text += streamReader.ReadToEnd();
						}
					}
				}
				tuple = new Tuple<string, string>(logName, text);
				TryExtractMetadataFromLog(tuple, ref metadata);
			}
			catch
			{
				return false;
			}
			try
			{
				string text2 = stringBuilder.ToString();
				string[] files = Directory.GetFiles(Path.GetDirectoryName(logName), "VRageRender*.log", SearchOption.TopDirectoryOnly);
				foreach (string text3 in files)
				{
					if (text3 != null && File.Exists(text3))
					{
						using (FileStream stream2 = File.Open(text3, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
						{
							using (StreamReader streamReader2 = new StreamReader(stream2))
							{
								text2 += streamReader2.ReadToEnd();
							}
						}
					}
				}
				dictionary["VRageRender.log"] = Encoding.UTF8.GetBytes(text2);
			}
			catch
			{
			}
			if (MyFakes.ENABLE_MINIDUMP_SENDING)
			{
				try
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						bool flag = false;
						using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
						{
							foreach (string item in MyMiniDump.FindActiveDumps(Path.GetDirectoryName(logName)))
							{
								flag = true;
								using (Stream destination = zipArchive.CreateEntry(Path.GetFileName(item)).Open())
								{
									using (FileStream fileStream = File.Open(item, FileMode.Open))
									{
										fileStream.CopyTo(destination);
									}
								}
							}
						}
						if (flag)
						{
							byte[] array2 = dictionary["Minidumps.zip"] = memoryStream.ToArray();
						}
					}
				}
				catch
				{
				}
			}
			bool flag2 = TrySendToOpicka(ref metadata, gameId, tuple, dictionary, email, feedback);
			return TrySendToHetzner(ref metadata, gameId, tuple, dictionary, email, feedback) || flag2;
		}

		private static void TryExtractMetadataFromLog(Tuple<string, string> mainLog, ref SessionMetadata metadata)
		{
			string uniqueUserIdentifier = metadata.UniqueUserIdentifier;
			long sessionId = metadata.SessionId;
			try
			{
				using (StringReader stringReader = new StringReader(mainLog.Item2))
				{
					for (string text = stringReader.ReadLine(); text != null; text = stringReader.ReadLine())
					{
						int num = text.IndexOf("Analytics uuid:");
						if (num >= 0)
						{
							uniqueUserIdentifier = text.Substring(num + "Analytics uuid:".Length).Trim();
						}
						num = text.IndexOf("Analytics session:");
						if (num >= 0 && long.TryParse(text.Substring(num + "Analytics session:".Length).Trim(), out long result))
						{
							sessionId = result;
						}
					}
				}
			}
			catch
			{
			}
			metadata.UniqueUserIdentifier = uniqueUserIdentifier;
			metadata.SessionId = sessionId;
		}

		private static bool TrySendToHetzner(ref SessionMetadata metadata, string gameId, Tuple<string, string> log, Dictionary<string, byte[]> additionalFiles, string email, string feedback)
		{
			if (log == null || string.IsNullOrWhiteSpace(log.Item1) || string.IsNullOrWhiteSpace(log.Item2) || string.IsNullOrWhiteSpace(gameId))
			{
				return false;
			}
			string value = "";
			if (additionalFiles.TryGetValue("VRageRender.log", out byte[] value2))
			{
				value = Encoding.UTF8.GetString(value2);
			}
			additionalFiles.TryGetValue("Minidumps.zip", out byte[] value3);
			try
			{
				string url = "https://minerwars.keenswh.com/SubmitLog.aspx?id=" + gameId;
				HttpStatusCode httpStatusCode = (HttpStatusCode)0;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						binaryWriter.Write(log.Item2);
						binaryWriter.Write(value);
						if (MyFakes.ENABLE_MINIDUMP_SENDING && value3 != null)
						{
							binaryWriter.Write(value3.Length);
							binaryWriter.Write(value3);
						}
						HttpData[] parameters = new HttpData[2]
						{
							new HttpData("Content-Type", "application/octet-stream", HttpDataType.HttpHeader),
							new HttpData("application/octet-stream", memoryStream.ToArray(), HttpDataType.RequestBody)
						};
						httpStatusCode = MyVRage.Platform.Http.SendRequest(url, parameters, HttpMethod.POST, out string _);
					}
				}
				return httpStatusCode == HttpStatusCode.OK;
			}
			catch
			{
			}
			return false;
		}

		private static bool TrySendToOpicka(ref SessionMetadata metadata, string id, Tuple<string, string> log, Dictionary<string, byte[]> additionalFiles, string email, string feedback, string reportType = "crash")
		{
			if (log == null || string.IsNullOrWhiteSpace(log.Item1) || string.IsNullOrWhiteSpace(log.Item2) || string.IsNullOrWhiteSpace(id))
			{
				return false;
			}
			try
			{
				string value = JsonMapper.ToJson(new MyCrashInfo
				{
					UniqueUserIdentifier = metadata.UniqueUserIdentifier,
					SessionID = metadata.SessionId,
					GameID = id,
					GameVersion = MyFinalBuildConstants.APP_VERSION_STRING.ToString(),
					Email = email,
					Feedback = feedback,
					ReportType = reportType
				});
				HttpStatusCode httpStatusCode = (HttpStatusCode)0;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						binaryWriter.Write("metadata");
						binaryWriter.Write(value);
						binaryWriter.Write("log");
						binaryWriter.Write(log.Item1);
						binaryWriter.Write(log.Item2);
						foreach (KeyValuePair<string, byte[]> additionalFile in additionalFiles)
						{
							binaryWriter.Write("file");
							binaryWriter.Write(additionalFile.Key);
							binaryWriter.Write(additionalFile.Value.Length);
							binaryWriter.Write(additionalFile.Value);
						}
						HttpData[] parameters = new HttpData[2]
						{
							new HttpData("Content-Type", "application/octet-stream", HttpDataType.HttpHeader),
							new HttpData("application/octet-stream", memoryStream.ToArray(), HttpDataType.RequestBody)
						};
						httpStatusCode = MyVRage.Platform.Http.SendRequest("https://crashlogs.keenswh.com/api/Report", parameters, HttpMethod.POST, out string _);
					}
				}
				return httpStatusCode == HttpStatusCode.OK;
			}
			catch
			{
			}
			return false;
		}
	}
}
