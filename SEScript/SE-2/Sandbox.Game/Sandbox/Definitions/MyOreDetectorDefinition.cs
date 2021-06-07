using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_OreDetectorDefinition), null)]
	public class MyOreDetectorDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyOreDetectorDefinition_003C_003EActor : IActivator, IActivator<MyOreDetectorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyOreDetectorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyOreDetectorDefinition CreateInstance()
			{
				return new MyOreDetectorDefinition();
			}

			MyOreDetectorDefinition IActivator<MyOreDetectorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float MaximumRange;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_OreDetectorDefinition myObjectBuilder_OreDetectorDefinition = builder as MyObjectBuilder_OreDetectorDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_OreDetectorDefinition.ResourceSinkGroup);
			MaximumRange = myObjectBuilder_OreDetectorDefinition.MaximumRange;
		}
	}
}
