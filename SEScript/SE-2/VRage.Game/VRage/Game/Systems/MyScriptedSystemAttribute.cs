using System;

namespace VRage.Game.Systems
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MyScriptedSystemAttribute : Attribute
	{
		public readonly string ScriptName;

		public MyScriptedSystemAttribute(string scriptName)
		{
			ScriptName = scriptName;
		}
	}
}
