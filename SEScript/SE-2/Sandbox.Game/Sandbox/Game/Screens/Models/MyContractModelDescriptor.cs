using System;
using VRage.Game.Common;

namespace Sandbox.Game.Screens.Models
{
	public class MyContractModelDescriptor : MyFactoryTagAttribute
	{
		public MyContractModelDescriptor(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
