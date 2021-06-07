using System;
using System.Reflection;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	[Obfuscation(Feature = "cw symbol renaming", Exclude = true)]
	public class MyGlobalEventHandler : Attribute
	{
		public MyDefinitionId EventDefinitionId;

		public MyGlobalEventHandler(Type objectBuilderType, string subtypeName)
		{
			MyObjectBuilderType type = objectBuilderType;
			EventDefinitionId = new MyDefinitionId(type, subtypeName);
		}
	}
}
