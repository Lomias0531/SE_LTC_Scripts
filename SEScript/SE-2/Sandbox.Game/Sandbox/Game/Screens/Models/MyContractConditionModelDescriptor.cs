using System;
using VRage.Game.Common;

namespace Sandbox.Game.Screens.Models
{
	public class MyContractConditionModelDescriptor : MyFactoryTagAttribute
	{
		public MyContractConditionModelDescriptor(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
