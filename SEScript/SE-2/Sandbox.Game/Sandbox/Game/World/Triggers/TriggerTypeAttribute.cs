using System;
using VRage.Game.Common;

namespace Sandbox.Game.World.Triggers
{
	public class TriggerTypeAttribute : MyFactoryTagAttribute
	{
		public TriggerTypeAttribute(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
