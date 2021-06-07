using System.Diagnostics;
using System.Text;

namespace VRage.Utils
{
	public static class MyLogExtensions
	{
		[Conditional("DEBUG")]
		public static void Debug(this MyLog self, string message, params object[] args)
		{
			self.Log(MyLogSeverity.Debug, message, args);
		}

		[Conditional("DEBUG")]
		public static void Debug(this MyLog self, StringBuilder buillder)
		{
			self.Log(MyLogSeverity.Debug, buillder);
		}

		public static void Info(this MyLog self, string message, params object[] args)
		{
			self.Log(MyLogSeverity.Info, message, args);
		}

		public static void Info(this MyLog self, StringBuilder buillder)
		{
			self.Log(MyLogSeverity.Info, buillder);
		}

		public static void Warning(this MyLog self, string message, params object[] args)
		{
			self.Log(MyLogSeverity.Warning, message, args);
		}

		public static void Warning(this MyLog self, StringBuilder buillder)
		{
			self.Log(MyLogSeverity.Warning, buillder);
		}

		public static void Error(this MyLog self, string message, params object[] args)
		{
			self.Log(MyLogSeverity.Error, message, args);
		}

		public static void Error(this MyLog self, StringBuilder buillder)
		{
			self.Log(MyLogSeverity.Error, buillder);
		}

		public static void Critical(this MyLog self, string message, params object[] args)
		{
			self.Log(MyLogSeverity.Critical, message, args);
		}

		public static void Critical(this MyLog self, StringBuilder buillder)
		{
			self.Log(MyLogSeverity.Critical, buillder);
		}
	}
}
