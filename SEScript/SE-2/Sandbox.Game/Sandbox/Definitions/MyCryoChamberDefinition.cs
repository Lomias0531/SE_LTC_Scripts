using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_CryoChamberDefinition), null)]
	public class MyCryoChamberDefinition : MyCockpitDefinition
	{
		private class Sandbox_Definitions_MyCryoChamberDefinition_003C_003EActor : IActivator, IActivator<MyCryoChamberDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCryoChamberDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCryoChamberDefinition CreateInstance()
			{
				return new MyCryoChamberDefinition();
			}

			MyCryoChamberDefinition IActivator<MyCryoChamberDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string OverlayTexture;

		public string ResourceSinkGroup;

		public float IdlePowerConsumption;

		public MySoundPair OutsideSound;

		public MySoundPair InsideSound;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CryoChamberDefinition myObjectBuilder_CryoChamberDefinition = builder as MyObjectBuilder_CryoChamberDefinition;
			OverlayTexture = myObjectBuilder_CryoChamberDefinition.OverlayTexture;
			ResourceSinkGroup = myObjectBuilder_CryoChamberDefinition.ResourceSinkGroup;
			IdlePowerConsumption = myObjectBuilder_CryoChamberDefinition.IdlePowerConsumption;
			OutsideSound = new MySoundPair(myObjectBuilder_CryoChamberDefinition.OutsideSound);
			InsideSound = new MySoundPair(myObjectBuilder_CryoChamberDefinition.InsideSound);
		}
	}
}
