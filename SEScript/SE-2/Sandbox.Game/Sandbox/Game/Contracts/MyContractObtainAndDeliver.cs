using System.Text;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;

namespace Sandbox.Game.Contracts
{
	[MyContractDescriptor(typeof(MyObjectBuilder_ContractObtainAndDeliver))]
	public class MyContractObtainAndDeliver : MyContract
	{
		public override MyObjectBuilder_Contract GetObjectBuilder()
		{
			return base.GetObjectBuilder();
		}

		public override void Init(MyObjectBuilder_Contract ob)
		{
			base.Init(ob);
			_ = (ob is MyObjectBuilder_ContractObtainAndDeliver);
		}

		public override MyDefinitionId? GetDefinitionId()
		{
			return new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "ObtainAndDeliver");
		}

		protected override void Activate_Internal(MyTimeSpan timeOfActivation)
		{
			base.Activate_Internal(timeOfActivation);
		}

		protected override void FailFor_Internal(long player, bool abandon = false)
		{
			base.FailFor_Internal(player, abandon);
		}

		protected override void FinishFor_Internal(long player, int rewardeeCount)
		{
			base.FinishFor_Internal(player, rewardeeCount);
		}

		public override string ToDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToDebugString());
			stringBuilder.Append(base.ContractCondition.ToDebugString());
			return stringBuilder.ToString();
		}

		public MyDefinitionId? GetItemId()
		{
			return (base.ContractCondition as MyContractConditionDeliverItems)?.ItemType;
		}

		protected override bool CanBeShared_Internal()
		{
			return true;
		}

		protected override bool CanPlayerReceiveReward(long identityId)
		{
			return true;
		}
	}
}
