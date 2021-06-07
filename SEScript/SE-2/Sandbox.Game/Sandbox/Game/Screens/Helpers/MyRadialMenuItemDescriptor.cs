using System;
using VRage.Game.Common;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyRadialMenuItemDescriptor : MyFactoryTagAttribute
	{
		public MyRadialMenuItemDescriptor(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
