using System;
using VRage.Game.Common;

namespace Sandbox.Game.Contracts
{
	public class MyContractConditionDescriptor : MyFactoryTagAttribute
	{
		public MyContractConditionDescriptor(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
