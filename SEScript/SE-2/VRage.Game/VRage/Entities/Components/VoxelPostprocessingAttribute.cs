using System;
using VRage.Game.Common;

namespace VRage.Entities.Components
{
	public class VoxelPostprocessingAttribute : MyFactoryTagAttribute
	{
		public VoxelPostprocessingAttribute(Type objectBuilderType, bool mainBuilder = true)
			: base(objectBuilderType, mainBuilder)
		{
		}
	}
}
