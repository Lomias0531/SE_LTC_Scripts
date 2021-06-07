using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_VoxelMapCollectionDefinition), null)]
	public class MyVoxelMapCollectionDefinition : MyDefinitionBase
	{
		private class Sandbox_Game_WorldEnvironment_Definitions_MyVoxelMapCollectionDefinition_003C_003EActor : IActivator, IActivator<MyVoxelMapCollectionDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelMapCollectionDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelMapCollectionDefinition CreateInstance()
			{
				return new MyVoxelMapCollectionDefinition();
			}

			MyVoxelMapCollectionDefinition IActivator<MyVoxelMapCollectionDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDiscreteSampler<MyDefinitionId> StorageFiles;

		public MyStringHash Modifier;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_VoxelMapCollectionDefinition myObjectBuilder_VoxelMapCollectionDefinition = builder as MyObjectBuilder_VoxelMapCollectionDefinition;
			if (myObjectBuilder_VoxelMapCollectionDefinition != null)
			{
				List<MyDefinitionId> list = new List<MyDefinitionId>();
				List<float> list2 = new List<float>();
				for (int i = 0; i < myObjectBuilder_VoxelMapCollectionDefinition.StorageDefs.Length; i++)
				{
					MyObjectBuilder_VoxelMapCollectionDefinition.VoxelMapStorage voxelMapStorage = myObjectBuilder_VoxelMapCollectionDefinition.StorageDefs[i];
					list.Add(new MyDefinitionId(typeof(MyObjectBuilder_VoxelMapStorageDefinition), voxelMapStorage.Storage));
					list2.Add(voxelMapStorage.Probability);
				}
				StorageFiles = new MyDiscreteSampler<MyDefinitionId>(list, list2);
				Modifier = MyStringHash.GetOrCompute(myObjectBuilder_VoxelMapCollectionDefinition.Modifier);
			}
		}
	}
}
