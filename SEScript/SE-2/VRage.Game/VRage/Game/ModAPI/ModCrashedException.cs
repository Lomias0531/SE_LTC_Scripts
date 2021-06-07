using System;

namespace VRage.Game.ModAPI
{
	public class ModCrashedException : Exception
	{
		public IMyModContext ModContext
		{
			get;
			private set;
		}

		public ModCrashedException(Exception innerException, IMyModContext modContext)
			: base("Mod crashed!", innerException)
		{
			ModContext = modContext;
		}
	}
}
