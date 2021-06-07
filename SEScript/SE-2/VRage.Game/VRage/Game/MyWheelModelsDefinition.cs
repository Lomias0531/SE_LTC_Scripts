using VRage.Game.Definitions;
using VRage.Network;

namespace VRage.Game
{
	[MyDefinitionType(typeof(MyObjectBuilder_WheelModelsDefinition), null)]
	public class MyWheelModelsDefinition : MyDefinitionBase
	{
		private class VRage_Game_MyWheelModelsDefinition_003C_003EActor : IActivator, IActivator<MyWheelModelsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWheelModelsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWheelModelsDefinition CreateInstance()
			{
				return new MyWheelModelsDefinition();
			}

			MyWheelModelsDefinition IActivator<MyWheelModelsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string AlternativeModel;

		public float AngularVelocityThreshold;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WheelModelsDefinition myObjectBuilder_WheelModelsDefinition = (MyObjectBuilder_WheelModelsDefinition)builder;
			AlternativeModel = myObjectBuilder_WheelModelsDefinition.AlternativeModel;
			AngularVelocityThreshold = myObjectBuilder_WheelModelsDefinition.AngularVelocityThreshold;
		}
	}
}
