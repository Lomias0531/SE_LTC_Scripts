using System;
using VRage.Game.Common;

namespace VRage.Game.Components
{
	public class MyComponentBuilderAttribute : MyFactoryTagAttribute
	{
		public MyComponentBuilderAttribute(Type objectBuilderType, bool mainBuilder = true)
			: base(objectBuilderType, mainBuilder)
		{
		}
	}
}
