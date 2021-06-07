using System;
using VRage.Game.Common;

namespace Sandbox.Game.World
{
	public class MyEnvironmentalParticleLogicTypeAttribute : MyFactoryTagAttribute
	{
		public MyEnvironmentalParticleLogicTypeAttribute(Type objectBuilderType, bool mainBuilder = true)
			: base(objectBuilderType, mainBuilder)
		{
		}
	}
}
