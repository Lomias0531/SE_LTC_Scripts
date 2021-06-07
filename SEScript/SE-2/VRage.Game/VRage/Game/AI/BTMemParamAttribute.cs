using System;

namespace VRage.Game.AI
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
	public abstract class BTMemParamAttribute : Attribute
	{
		public abstract MyMemoryParameterType MemoryType
		{
			get;
		}
	}
}
