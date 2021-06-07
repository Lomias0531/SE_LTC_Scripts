using VRage.Game.ObjectBuilders.Definitions.Reputation;
using VRage.Network;

namespace VRage.Game.Definitions.Reputation
{
	[MyDefinitionType(typeof(MyObjectBuilder_ReputationSettingsDefinition), null)]
	public class MyReputationSettingsDefinition : MyDefinitionBase
	{
		private class VRage_Game_Definitions_Reputation_MyReputationSettingsDefinition_003C_003EActor : IActivator, IActivator<MyReputationSettingsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyReputationSettingsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyReputationSettingsDefinition CreateInstance()
			{
				return new MyReputationSettingsDefinition();
			}

			MyReputationSettingsDefinition IActivator<MyReputationSettingsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int MaxReputationGainInTime;

		public int ResetTimeMinForRepGain;

		public MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings DamageSettings;

		public MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings PirateDamageSettings;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ReputationSettingsDefinition myObjectBuilder_ReputationSettingsDefinition = builder as MyObjectBuilder_ReputationSettingsDefinition;
			DamageSettings = myObjectBuilder_ReputationSettingsDefinition.DamageSettings;
			PirateDamageSettings = myObjectBuilder_ReputationSettingsDefinition.PirateDamageSettings;
			MaxReputationGainInTime = myObjectBuilder_ReputationSettingsDefinition.MaxReputationGainInTime;
			ResetTimeMinForRepGain = myObjectBuilder_ReputationSettingsDefinition.ResetTimeMinForRepGain;
		}
	}
}
