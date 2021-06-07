using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Contracts
{
	public abstract class MyContractCondition
	{
		public long Id
		{
			get;
			private set;
		}

		public bool IsFinished
		{
			get;
			private set;
		}

		public long ContractId
		{
			get;
			private set;
		}

		public long StationEndId
		{
			get;
			private set;
		}

		public long FactionEndId
		{
			get;
			private set;
		}

		public long BlockEndId
		{
			get;
			private set;
		}

		public virtual void Init(MyObjectBuilder_ContractCondition builder)
		{
			Id = builder.Id;
			IsFinished = builder.IsFinished;
			ContractId = builder.ContractId;
			StationEndId = builder.StationEndId;
			FactionEndId = builder.FactionEndId;
			BlockEndId = builder.BlockEndId;
		}

		public virtual MyObjectBuilder_ContractCondition GetObjectBuilder()
		{
			MyObjectBuilder_ContractCondition myObjectBuilder_ContractCondition = MyContractConditionFactory.CreateObjectBuilder(this);
			myObjectBuilder_ContractCondition.Id = Id;
			myObjectBuilder_ContractCondition.IsFinished = IsFinished;
			myObjectBuilder_ContractCondition.ContractId = ContractId;
			myObjectBuilder_ContractCondition.StationEndId = StationEndId;
			myObjectBuilder_ContractCondition.FactionEndId = FactionEndId;
			myObjectBuilder_ContractCondition.BlockEndId = BlockEndId;
			return myObjectBuilder_ContractCondition;
		}

		public bool FinalizeCondition()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				return false;
			}
			MyContract activeContractById = component.GetActiveContractById(ContractId);
			if (activeContractById == null)
			{
				return false;
			}
			if (!activeContractById.IsOwnerOfCondition(this))
			{
				return false;
			}
			IsFinished = true;
			activeContractById.RecalculateUnfinishedCondiCount();
			return true;
		}

		public virtual string ToDebugString()
		{
			return "Base condition";
		}
	}
}
