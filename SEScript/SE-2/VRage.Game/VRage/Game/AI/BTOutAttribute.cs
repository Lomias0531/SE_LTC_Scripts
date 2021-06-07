using System;

namespace VRage.Game.AI
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
	public class BTOutAttribute : BTMemParamAttribute
	{
		public override MyMemoryParameterType MemoryType => MyMemoryParameterType.OUT;
	}
}
