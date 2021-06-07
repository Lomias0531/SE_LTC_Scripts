using Sandbox.Game.World;
using System.Collections.Generic;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyCubeGridOwnershipManager
	{
		public Dictionary<long, int> PlayerOwnedBlocks;

		public Dictionary<long, int> PlayerOwnedValidBlocks;

		public List<long> BigOwners;

		public List<long> SmallOwners;

		public int MaxBlocks;

		public long gridEntityId;

		public bool NeedRecalculateOwners;

		private bool IsValidBlock(MyCubeBlock block)
		{
			return block.IsFunctional;
		}

		public void Init(MyCubeGrid grid)
		{
			PlayerOwnedBlocks = new Dictionary<long, int>();
			PlayerOwnedValidBlocks = new Dictionary<long, int>();
			BigOwners = new List<long>();
			SmallOwners = new List<long>();
			MaxBlocks = 0;
			gridEntityId = grid.EntityId;
			foreach (MyCubeBlock fatBlock in grid.GetFatBlocks())
			{
				long ownerId = fatBlock.OwnerId;
				if (ownerId != 0L)
				{
					if (!PlayerOwnedBlocks.ContainsKey(ownerId))
					{
						PlayerOwnedBlocks.Add(ownerId, 0);
					}
					PlayerOwnedBlocks[ownerId]++;
					if (IsValidBlock(fatBlock))
					{
						if (!PlayerOwnedValidBlocks.ContainsKey(ownerId))
						{
							PlayerOwnedValidBlocks.Add(ownerId, 0);
						}
						if (++PlayerOwnedValidBlocks[fatBlock.OwnerId] > MaxBlocks)
						{
							MaxBlocks = PlayerOwnedValidBlocks[ownerId];
						}
					}
				}
			}
			NeedRecalculateOwners = true;
		}

		internal void RecalculateOwners()
		{
			MaxBlocks = 0;
			foreach (long key in PlayerOwnedValidBlocks.Keys)
			{
				if (PlayerOwnedValidBlocks[key] > MaxBlocks)
				{
					MaxBlocks = PlayerOwnedValidBlocks[key];
				}
			}
			BigOwners.Clear();
			foreach (long key2 in PlayerOwnedValidBlocks.Keys)
			{
				if (PlayerOwnedValidBlocks[key2] == MaxBlocks)
				{
					BigOwners.Add(key2);
				}
			}
			if (SmallOwners.Contains(MySession.Static.LocalPlayerId))
			{
				MySession.Static.LocalHumanPlayer.RemoveGrid(gridEntityId);
			}
			SmallOwners.Clear();
			foreach (long key3 in PlayerOwnedBlocks.Keys)
			{
				SmallOwners.Add(key3);
				if (key3 == MySession.Static.LocalPlayerId)
				{
					MySession.Static.LocalHumanPlayer.AddGrid(gridEntityId);
				}
			}
		}

		public void ChangeBlockOwnership(MyCubeBlock block, long oldOwner, long newOwner)
		{
			DecreaseValue(ref PlayerOwnedBlocks, oldOwner);
			IncreaseValue(ref PlayerOwnedBlocks, newOwner);
			if (IsValidBlock(block))
			{
				DecreaseValue(ref PlayerOwnedValidBlocks, oldOwner);
				IncreaseValue(ref PlayerOwnedValidBlocks, newOwner);
			}
			NeedRecalculateOwners = true;
			block.CubeGrid.MarkForUpdate();
		}

		public void UpdateOnFunctionalChange(long ownerId, bool newFunctionalValue)
		{
			if (!newFunctionalValue)
			{
				DecreaseValue(ref PlayerOwnedValidBlocks, ownerId);
			}
			else
			{
				IncreaseValue(ref PlayerOwnedValidBlocks, ownerId);
			}
			NeedRecalculateOwners = true;
		}

		public void IncreaseValue(ref Dictionary<long, int> dict, long key)
		{
			if (key != 0L)
			{
				if (!dict.ContainsKey(key))
				{
					dict.Add(key, 0);
				}
				dict[key]++;
			}
		}

		public void DecreaseValue(ref Dictionary<long, int> dict, long key)
		{
			if (key != 0L && dict.ContainsKey(key))
			{
				dict[key]--;
				if (dict[key] == 0)
				{
					dict.Remove(key);
				}
			}
		}
	}
}
