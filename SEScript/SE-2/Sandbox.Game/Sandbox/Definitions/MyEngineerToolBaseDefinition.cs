using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_EngineerToolBaseDefinition), null)]
	public class MyEngineerToolBaseDefinition : MyHandItemDefinition
	{
		private class Sandbox_Definitions_MyEngineerToolBaseDefinition_003C_003EActor : IActivator, IActivator<MyEngineerToolBaseDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEngineerToolBaseDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEngineerToolBaseDefinition CreateInstance()
			{
				return new MyEngineerToolBaseDefinition();
			}

			MyEngineerToolBaseDefinition IActivator<MyEngineerToolBaseDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float SpeedMultiplier;

		public float DistanceMultiplier;

		public string Flare;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_EngineerToolBaseDefinition myObjectBuilder_EngineerToolBaseDefinition = builder as MyObjectBuilder_EngineerToolBaseDefinition;
			SpeedMultiplier = myObjectBuilder_EngineerToolBaseDefinition.SpeedMultiplier;
			DistanceMultiplier = myObjectBuilder_EngineerToolBaseDefinition.DistanceMultiplier;
			Flare = myObjectBuilder_EngineerToolBaseDefinition.Flare;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_EngineerToolBaseDefinition obj = (MyObjectBuilder_EngineerToolBaseDefinition)base.GetObjectBuilder();
			obj.SpeedMultiplier = SpeedMultiplier;
			obj.DistanceMultiplier = DistanceMultiplier;
			obj.Flare = Flare;
			return obj;
		}
	}
}
