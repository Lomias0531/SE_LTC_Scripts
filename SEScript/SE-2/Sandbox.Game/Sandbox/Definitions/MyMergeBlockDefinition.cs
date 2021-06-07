using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MergeBlockDefinition), null)]
	public class MyMergeBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyMergeBlockDefinition_003C_003EActor : IActivator, IActivator<MyMergeBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMergeBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMergeBlockDefinition CreateInstance()
			{
				return new MyMergeBlockDefinition();
			}

			MyMergeBlockDefinition IActivator<MyMergeBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float Strength;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_MergeBlockDefinition myObjectBuilder_MergeBlockDefinition = builder as MyObjectBuilder_MergeBlockDefinition;
			Strength = myObjectBuilder_MergeBlockDefinition.Strength;
		}
	}
}
