using System;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.ModAPI.Contracts
{
	public class MyContractEscort : IMyContractEscort, IMyContract
	{
		public Vector3D Start
		{
			get;
			private set;
		}

		public Vector3D End
		{
			get;
			private set;
		}

		public long OwnerIdentityId
		{
			get;
			private set;
		}

		public long StartBlockId
		{
			get;
			private set;
		}

		public int MoneyReward
		{
			get;
			private set;
		}

		public int Collateral
		{
			get;
			private set;
		}

		public int Duration
		{
			get;
			private set;
		}

		public Action<long> OnContractAcquired
		{
			get;
			set;
		}

		public Action OnContractSucceeded
		{
			get;
			set;
		}

		public Action OnContractFailed
		{
			get;
			set;
		}

		public MyContractEscort(long startBlockId, int moneyReward, int collateral, int duration, Vector3D start, Vector3D end, long ownerIdentityId)
		{
			StartBlockId = startBlockId;
			MoneyReward = moneyReward;
			Collateral = collateral;
			Duration = duration;
			Start = start;
			End = end;
			OwnerIdentityId = ownerIdentityId;
		}
	}
}
