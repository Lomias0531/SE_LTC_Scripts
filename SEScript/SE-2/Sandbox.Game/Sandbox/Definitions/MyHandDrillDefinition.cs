using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_HandDrillDefinition), null)]
	public class MyHandDrillDefinition : MyEngineerToolBaseDefinition
	{
		private class Sandbox_Definitions_MyHandDrillDefinition_003C_003EActor : IActivator, IActivator<MyHandDrillDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyHandDrillDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHandDrillDefinition CreateInstance()
			{
				return new MyHandDrillDefinition();
			}

			MyHandDrillDefinition IActivator<MyHandDrillDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float HarvestRatioMultiplier;

		public Vector3D ParticleOffset;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_HandDrillDefinition myObjectBuilder_HandDrillDefinition = builder as MyObjectBuilder_HandDrillDefinition;
			HarvestRatioMultiplier = myObjectBuilder_HandDrillDefinition.HarvestRatioMultiplier;
			_ = myObjectBuilder_HandDrillDefinition.ParticleOffset;
			ParticleOffset = myObjectBuilder_HandDrillDefinition.ParticleOffset;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_HandDrillDefinition obj = (MyObjectBuilder_HandDrillDefinition)base.GetObjectBuilder();
			obj.HarvestRatioMultiplier = HarvestRatioMultiplier;
			obj.ParticleOffset = ParticleOffset;
			return obj;
		}
	}
}
