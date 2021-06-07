using Sandbox.ModAPI;

namespace VRage.Scripting
{
	public class MyIngameScripting : IMyIngameScripting
	{
		public static IMyScriptBlacklist ScriptBlacklist;

		public static MyIngameScripting Static
		{
			get;
			internal set;
		}

		IMyScriptBlacklist IMyIngameScripting.ScriptBlacklist => ScriptBlacklist;

		static MyIngameScripting()
		{
			Static = new MyIngameScripting();
		}

		public MyIngameScripting()
		{
			ScriptBlacklist = MyScriptCompiler.Static.Whitelist;
		}

		public void Clean()
		{
			ScriptBlacklist = null;
			Static = null;
		}
	}
}
