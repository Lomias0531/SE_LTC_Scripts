using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageRender.Messages;

namespace VRage.Game.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_SafeZoneTexturesDefinition), null)]
	public class MySafeZoneTexturesDefinition : MyDefinitionBase
	{
		private class VRage_Game_Definitions_MySafeZoneTexturesDefinition_003C_003EActor : IActivator, IActivator<MySafeZoneTexturesDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySafeZoneTexturesDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySafeZoneTexturesDefinition CreateInstance()
			{
				return new MySafeZoneTexturesDefinition();
			}

			MySafeZoneTexturesDefinition IActivator<MySafeZoneTexturesDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyTextureChange Texture;

		public MyStringHash DisplayTextId;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SafeZoneTexturesDefinition myObjectBuilder_SafeZoneTexturesDefinition = (MyObjectBuilder_SafeZoneTexturesDefinition)builder;
			Texture = new MyTextureChange
			{
				ColorMetalFileName = myObjectBuilder_SafeZoneTexturesDefinition.Alphamask,
				NormalGlossFileName = myObjectBuilder_SafeZoneTexturesDefinition.NormalGloss
			};
			DisplayTextId = MyStringHash.GetOrCompute(myObjectBuilder_SafeZoneTexturesDefinition.DisplayTextId);
		}
	}
}
