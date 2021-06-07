using System;

namespace VRage.Game.AI
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
	public class BTInAttribute : BTMemParamAttribute
	{
		public override MyMemoryParameterType MemoryType => MyMemoryParameterType.IN;
	}
}
