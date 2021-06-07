using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders.Definitions.Components;

namespace VRage.Definitions.Components
{
	[MyDefinitionType(typeof(MyObjectBuilder_VoxelMesherComponentDefinition), null)]
	public class MyVoxelMesherComponentDefinition : MyDefinitionBase
	{
		private class VRage_Definitions_Components_MyVoxelMesherComponentDefinition_003C_003EActor : IActivator, IActivator<MyVoxelMesherComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelMesherComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelMesherComponentDefinition CreateInstance()
			{
				return new MyVoxelMesherComponentDefinition();
			}

			MyVoxelMesherComponentDefinition IActivator<MyVoxelMesherComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyObjectBuilder_VoxelPostprocessing> PostProcessingSteps = new List<MyObjectBuilder_VoxelPostprocessing>();

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_VoxelMesherComponentDefinition myObjectBuilder_VoxelMesherComponentDefinition = (MyObjectBuilder_VoxelMesherComponentDefinition)builder;
			if (myObjectBuilder_VoxelMesherComponentDefinition.PostprocessingSteps != null)
			{
				PostProcessingSteps = myObjectBuilder_VoxelMesherComponentDefinition.PostprocessingSteps;
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			return (MyObjectBuilder_VoxelMesherComponentDefinition)base.GetObjectBuilder();
		}
	}
}
