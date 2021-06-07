using System;
using VRage.Game.Common;

namespace Sandbox.Game.World
{
	public class MyEventTypeAttribute : MyFactoryTagAttribute
	{
		public MyEventTypeAttribute(Type objectBuilderType, bool mainBuilder = true)
			: base(objectBuilderType, mainBuilder)
		{
		}
	}
}
