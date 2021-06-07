using System;
using VRage.Game.Common;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyToolbarItemDescriptor : MyFactoryTagAttribute
	{
		public MyToolbarItemDescriptor(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
