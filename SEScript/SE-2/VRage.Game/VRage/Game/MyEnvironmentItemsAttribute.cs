using System;

namespace VRage.Game
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class MyEnvironmentItemsAttribute : Attribute
	{
		public readonly Type ItemDefinitionType;

		public MyEnvironmentItemsAttribute(Type itemDefinitionType)
		{
			ItemDefinitionType = itemDefinitionType;
		}
	}
}
