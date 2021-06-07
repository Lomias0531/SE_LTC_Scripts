using System;

namespace VRage.Game.Components
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MyEntityComponentDescriptor : Attribute
	{
		public Type EntityBuilderType;

		public string[] EntityBuilderSubTypeNames;

		public bool? EntityUpdate;

		[Obsolete("Use the 3 parameter overload instead!")]
		public MyEntityComponentDescriptor(Type entityBuilderType, params string[] entityBuilderSubTypeNames)
		{
			EntityBuilderType = entityBuilderType;
			EntityBuilderSubTypeNames = entityBuilderSubTypeNames;
			EntityUpdate = null;
		}

		public MyEntityComponentDescriptor(Type entityBuilderType, bool useEntityUpdate, params string[] entityBuilderSubTypeNames)
		{
			EntityBuilderType = entityBuilderType;
			EntityUpdate = useEntityUpdate;
			EntityBuilderSubTypeNames = entityBuilderSubTypeNames;
		}
	}
}
