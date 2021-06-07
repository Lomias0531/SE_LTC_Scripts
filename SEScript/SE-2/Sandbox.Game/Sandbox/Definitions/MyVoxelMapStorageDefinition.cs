using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_VoxelMapStorageDefinition), null)]
	public class MyVoxelMapStorageDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyVoxelMapStorageDefinition_003C_003EActor : IActivator, IActivator<MyVoxelMapStorageDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelMapStorageDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelMapStorageDefinition CreateInstance()
			{
				return new MyVoxelMapStorageDefinition();
			}

			MyVoxelMapStorageDefinition IActivator<MyVoxelMapStorageDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string StorageFile;

		public bool UseForProceduralRemovals;

		public bool UseForProceduralAdditions;

		public bool UseAsPrimaryProceduralAdditionShape;

		public float SpawnProbability;

		public HashSet<int> GeneratorVersions;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_VoxelMapStorageDefinition myObjectBuilder_VoxelMapStorageDefinition = (MyObjectBuilder_VoxelMapStorageDefinition)builder;
			StorageFile = myObjectBuilder_VoxelMapStorageDefinition.StorageFile;
			UseForProceduralRemovals = myObjectBuilder_VoxelMapStorageDefinition.UseForProceduralRemovals;
			UseForProceduralAdditions = myObjectBuilder_VoxelMapStorageDefinition.UseForProceduralAdditions;
			UseAsPrimaryProceduralAdditionShape = myObjectBuilder_VoxelMapStorageDefinition.UseAsPrimaryProceduralAdditionShape;
			SpawnProbability = myObjectBuilder_VoxelMapStorageDefinition.SpawnProbability;
			if (myObjectBuilder_VoxelMapStorageDefinition.ExplicitProceduralGeneratorVersions != null)
			{
				GeneratorVersions = new HashSet<int>(myObjectBuilder_VoxelMapStorageDefinition.ExplicitProceduralGeneratorVersions.Select(int.Parse));
			}
		}
	}
}
