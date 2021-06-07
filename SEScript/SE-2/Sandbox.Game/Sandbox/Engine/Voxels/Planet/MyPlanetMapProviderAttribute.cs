using System;
using VRage.Game.Common;

namespace Sandbox.Engine.Voxels.Planet
{
	public class MyPlanetMapProviderAttribute : MyFactoryTagAttribute
	{
		public MyPlanetMapProviderAttribute(Type objectBuilderType, bool mainBuilder = true)
			: base(objectBuilderType, mainBuilder)
		{
		}
	}
}
