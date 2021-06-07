using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentDebrisVoxel : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentDebrisVoxel_003C_003EActor : IActivator, IActivator<MyRenderComponentDebrisVoxel>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentDebrisVoxel();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentDebrisVoxel CreateInstance()
			{
				return new MyRenderComponentDebrisVoxel();
			}

			MyRenderComponentDebrisVoxel IActivator<MyRenderComponentDebrisVoxel>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float TexCoordOffset
		{
			get;
			set;
		}

		public float TexCoordScale
		{
			get;
			set;
		}

		public byte VoxelMaterialIndex
		{
			get;
			set;
		}

		public override void AddRenderObjects()
		{
			if (m_renderObjectIDs[0] == uint.MaxValue)
			{
				SetRenderObjectID(0, MyRenderProxy.CreateRenderVoxelDebris("Voxel debris", base.Model.AssetName, (Matrix)base.Container.Entity.PositionComp.WorldMatrix, TexCoordOffset, TexCoordScale, 1f, VoxelMaterialIndex, FadeIn));
			}
		}
	}
}
