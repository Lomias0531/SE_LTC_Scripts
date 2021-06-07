using System;
using VRage.Game.Common;

namespace Sandbox.Game.Entities.Cube
{
	public class MyCubeBlockTypeAttribute : MyFactoryTagAttribute
	{
		public MyCubeBlockTypeAttribute(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
