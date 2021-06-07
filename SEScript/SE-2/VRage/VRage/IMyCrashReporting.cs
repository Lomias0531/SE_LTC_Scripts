using System;

namespace VRage
{
	public interface IMyCrashReporting
	{
		ExceptionType GetExceptionType(Exception e);

		void WriteMiniDump(string dumpPath, MyMiniDump.Options dumpFlags, IntPtr exceptionPointers);

		void SetNativeExceptionHandler(Action<IntPtr> handler);

		void PrepareCrashAnalyticsReporting(string logPath, string gameName, bool isGpuError, bool gdprConsent);

		bool ExtractCrashAnalyticsReport(string[] args, out string logPath, out string gameName, out bool isGpuError, out bool exitAfterReport);

		void CleanupCrashAnalytics();

		bool MessageBoxCrashForm(ref MyCrashScreenTexts texts, out string message, out string email);

		void MessageBoxModCrashForm(ref MyModCrashScreenTexts texts);

		void ExitProcessOnCrash(Exception exception);
	}
}
