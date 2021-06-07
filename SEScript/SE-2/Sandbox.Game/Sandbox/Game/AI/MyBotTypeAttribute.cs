using System;
using VRage.Game.Common;

namespace Sandbox.Game.AI
{
	public class MyBotTypeAttribute : MyFactoryTagAttribute
	{
		public MyBotTypeAttribute(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
