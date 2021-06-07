using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;

namespace Sandbox.Game.Contracts
{
	[MyContractDescriptor(typeof(MyObjectBuilder_ContractDeliver))]
	public class MyContractDeliver : MyContract
	{
		public double DeliverDistance
		{
			get;
			set;
		}

		public override MyObjectBuilder_Contract GetObjectBuilder()
		{
			MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
			(objectBuilder as MyObjectBuilder_ContractDeliver).DeliverDistance = DeliverDistance;
			return objectBuilder;
		}

		public override void Init(MyObjectBuilder_Contract ob)
		{
			base.Init(ob);
			MyObjectBuilder_ContractDeliver myObjectBuilder_ContractDeliver = ob as MyObjectBuilder_ContractDeliver;
			if (myObjectBuilder_ContractDeliver != null)
			{
				DeliverDistance = myObjectBuilder_ContractDeliver.DeliverDistance;
			}
		}

		public override string ToDebugString()
		{
			return new StringBuilder(base.ToDebugString()).ToString();
		}

		protected override MyActivationResults CanActivate_Internal(long playerId)
		{
			MyActivationResults myActivationResults = base.CanActivate_Internal(playerId);
			if (myActivationResults != 0)
			{
				return myActivationResults;
			}
			if (!CheckPlayerInventory(playerId))
			{
				return MyActivationResults.Fail_InsufficientInventorySpace;
			}
			return MyActivationResults.Success;
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

		public override MyDefinitionId? GetDefinitionId()
		{
			return new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Deliver");
		}

		private bool CheckPlayerInventory(long playerId)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(playerId);
			if (myIdentity == null)
			{
				return false;
			}
			MyCharacter character = myIdentity.Character;
			if (character == null)
			{
				return false;
			}
			if (character.InventoryCount <= 0)
			{
				return false;
			}
			MyInventoryBase inventoryBase = character.GetInventoryBase();
			MyPackageDefinition packageItem = GetPackageItem();
			if (packageItem == null)
			{
				return false;
			}
			float mass = packageItem.Mass;
			float volume = packageItem.Volume;
			if ((float)(inventoryBase.MaxMass - inventoryBase.CurrentMass) < mass || (float)(inventoryBase.MaxVolume - inventoryBase.CurrentVolume) < volume)
			{
				return false;
			}
			return true;
		}

		private MyPackageDefinition GetPackageItem()
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_Package), "Package");
			return MyDefinitionManager.Static.GetPhysicalItemDefinition(id) as MyPackageDefinition;
		}
	}
}
