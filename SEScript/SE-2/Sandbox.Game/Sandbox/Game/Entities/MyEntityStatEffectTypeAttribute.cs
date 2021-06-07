using System;
using VRage.Game.Common;

namespace Sandbox.Game.Entities
{
	public class MyEntityStatEffectTypeAttribute : MyFactoryTagAttribute
	{
		public readonly Type MemoryType;

		public MyEntityStatEffectTypeAttribute(Type objectBuilderType)
			: base(objectBuilderType)
		{
			MemoryType = typeof(MyEntityStatRegenEffect);
		}

		public MyEntityStatEffectTypeAttribute(Type objectBuilderType, Type memoryType)
			: base(objectBuilderType)
		{
			MemoryType = memoryType;
		}
	}
}
