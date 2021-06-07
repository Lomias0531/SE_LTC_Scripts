using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_VoxelHandDefinition), null)]
	public class MyVoxelHandDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyVoxelHandDefinition_003C_003EActor : IActivator, IActivator<MyVoxelHandDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelHandDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelHandDefinition CreateInstance()
			{
				return new MyVoxelHandDefinition();
			}

			MyVoxelHandDefinition IActivator<MyVoxelHandDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
		}
	}
}
