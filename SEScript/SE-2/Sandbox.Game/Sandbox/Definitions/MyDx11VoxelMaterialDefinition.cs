using Medieval.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_Dx11VoxelMaterialDefinition), null)]
	public class MyDx11VoxelMaterialDefinition : MyVoxelMaterialDefinition
	{
		private class Sandbox_Definitions_MyDx11VoxelMaterialDefinition_003C_003EActor : IActivator, IActivator<MyDx11VoxelMaterialDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDx11VoxelMaterialDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDx11VoxelMaterialDefinition CreateInstance()
			{
				return new MyDx11VoxelMaterialDefinition();
			}

			MyDx11VoxelMaterialDefinition IActivator<MyDx11VoxelMaterialDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public new string VoxelHandPreview;

		protected override void Init(MyObjectBuilder_DefinitionBase ob)
		{
			base.Init(ob);
			MyObjectBuilder_Dx11VoxelMaterialDefinition myObjectBuilder_Dx11VoxelMaterialDefinition = (MyObjectBuilder_Dx11VoxelMaterialDefinition)ob;
			VoxelHandPreview = myObjectBuilder_Dx11VoxelMaterialDefinition.VoxelHandPreview;
		}
	}
}
