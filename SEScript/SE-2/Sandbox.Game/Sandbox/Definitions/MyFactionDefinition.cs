using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_FactionDefinition), null)]
	public class MyFactionDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyFactionDefinition_003C_003EActor : IActivator, IActivator<MyFactionDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyFactionDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFactionDefinition CreateInstance()
			{
				return new MyFactionDefinition();
			}

			MyFactionDefinition IActivator<MyFactionDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Tag;

		public string Name;

		public string Founder;

		public MyStringId FactionIcon;

		public bool AcceptHumans;

		public bool AutoAcceptMember;

		public bool EnableFriendlyFire;

		public bool IsDefault;

		public MyRelationsBetweenFactions DefaultRelation;

		public MyRelationsBetweenFactions DefaultRelationToPlayers;

		public long StartingBalance;

		public MyFactionTypes Type;

		public bool DiscoveredByDefault;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_FactionDefinition myObjectBuilder_FactionDefinition = builder as MyObjectBuilder_FactionDefinition;
			Tag = myObjectBuilder_FactionDefinition.Tag;
			Name = myObjectBuilder_FactionDefinition.Name;
			Founder = myObjectBuilder_FactionDefinition.Founder;
			AcceptHumans = myObjectBuilder_FactionDefinition.AcceptHumans;
			AutoAcceptMember = myObjectBuilder_FactionDefinition.AutoAcceptMember;
			EnableFriendlyFire = myObjectBuilder_FactionDefinition.EnableFriendlyFire;
			IsDefault = myObjectBuilder_FactionDefinition.IsDefault;
			DefaultRelation = myObjectBuilder_FactionDefinition.DefaultRelation;
			StartingBalance = myObjectBuilder_FactionDefinition.StartingBalance;
			Type = myObjectBuilder_FactionDefinition.Type;
			DiscoveredByDefault = myObjectBuilder_FactionDefinition.DiscoveredByDefault;
			FactionIcon = MyStringId.GetOrCompute(myObjectBuilder_FactionDefinition.FactionIcon);
		}

		public override void Postprocess()
		{
			base.Postprocess();
			MyDefinitionManager.Static.RegisterFactionDefinition(this);
		}
	}
}
