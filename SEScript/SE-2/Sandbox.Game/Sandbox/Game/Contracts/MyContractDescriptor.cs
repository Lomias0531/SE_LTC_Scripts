using System;
using VRage.Game.Common;

namespace Sandbox.Game.Contracts
{
	public class MyContractDescriptor : MyFactoryTagAttribute
	{
		public MyContractDescriptor(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
