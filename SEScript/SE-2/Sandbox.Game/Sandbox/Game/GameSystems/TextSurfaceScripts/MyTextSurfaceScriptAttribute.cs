using System;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class MyTextSurfaceScriptAttribute : Attribute
	{
		public string Id;

		public string DisplayName;

		public MyTextSurfaceScriptAttribute(string id, string displayName)
		{
			Id = id;
			DisplayName = displayName;
		}
	}
}
