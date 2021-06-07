using System;

namespace VRage.Game.AI
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
	public class BTInOutAttribute : BTMemParamAttribute
	{
		public override MyMemoryParameterType MemoryType => MyMemoryParameterType.IN_OUT;
	}
}
