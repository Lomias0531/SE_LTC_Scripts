using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Definitions.Components;
using VRage.Game.Components;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Voxels;
using VRage.Voxels.DualContouring;
using VRageMath;

namespace VRage.Entities.Components
{
	public class MyVoxelMesherComponent : MyEntityComponentBase
	{
		private class VRage_Entities_Components_MyVoxelMesherComponent_003C_003EActor : IActivator, IActivator<MyVoxelMesherComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelMesherComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelMesherComponent CreateInstance()
			{
				return new MyVoxelMesherComponent();
			}

			MyVoxelMesherComponent IActivator<MyVoxelMesherComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private List<MyVoxelPostprocessing> m_postprocessingSteps = new List<MyVoxelPostprocessing>();

		public VRage.Game.Voxels.IMyStorage Storage
		{
			get
			{
				IMyVoxelBase myVoxelBase = base.Entity as IMyVoxelBase;
				if (myVoxelBase != null)
				{
					return (VRage.Game.Voxels.IMyStorage)myVoxelBase.Storage;
				}
				return null;
			}
		}

		public ListReader<MyVoxelPostprocessing> PostprocessingSteps => m_postprocessingSteps;

		public override string ComponentTypeDebugString => "MyVoxelMesherComponent";

		public void Init(MyVoxelMesherComponentDefinition def)
		{
			if (def == null)
			{
				throw new Exception("Definition {0} is not a valid MyVoxelMesherComponentDefinition.");
			}
			foreach (MyObjectBuilder_VoxelPostprocessing postProcessingStep in def.PostProcessingSteps)
			{
				MyVoxelPostprocessing myVoxelPostprocessing = MyVoxelPostprocessing.Factory.CreateInstance(postProcessingStep.TypeId);
				myVoxelPostprocessing.Init(postProcessingStep);
				m_postprocessingSteps.Add(myVoxelPostprocessing);
			}
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
		}

		public virtual MyMesherResult CalculateMesh(int lod, Vector3I lodVoxelMin, Vector3I lodVoxelMax, MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial, MyVoxelRequestFlags flags = (MyVoxelRequestFlags)0, VrVoxelMesh target = null)
		{
			return MyDualContouringMesher.Static.Calculate(this, lod, lodVoxelMin, lodVoxelMax, properties, flags, target);
		}
	}
}
