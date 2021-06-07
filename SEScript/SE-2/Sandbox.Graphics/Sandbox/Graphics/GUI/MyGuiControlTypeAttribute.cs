using System;
using VRage.Game.Common;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlTypeAttribute : MyFactoryTagAttribute
	{
		public MyGuiControlTypeAttribute(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
