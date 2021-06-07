using System;
using VRage.Game.Common;

namespace Sandbox.Game.AI
{
	internal class MyAutopilotTypeAttribute : MyFactoryTagAttribute
	{
		public MyAutopilotTypeAttribute(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
