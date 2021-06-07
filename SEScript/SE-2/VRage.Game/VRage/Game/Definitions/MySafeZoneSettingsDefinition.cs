using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace VRage.Game.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SafeZoneSettingsDefinition), null)]
	public class MySafeZoneSettingsDefinition : MyDefinitionBase
	{
		private class VRage_Game_Definitions_MySafeZoneSettingsDefinition_003C_003EActor : IActivator, IActivator<MySafeZoneSettingsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySafeZoneSettingsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySafeZoneSettingsDefinition CreateInstance()
			{
				return new MySafeZoneSettingsDefinition();
			}

			MySafeZoneSettingsDefinition IActivator<MySafeZoneSettingsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int EnableAnimationTimeMs;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SafeZoneSettingsDefinition myObjectBuilder_SafeZoneSettingsDefinition = builder as MyObjectBuilder_SafeZoneSettingsDefinition;
			EnableAnimationTimeMs = myObjectBuilder_SafeZoneSettingsDefinition.EnableAnimationTimeMs;
		}
	}
}
