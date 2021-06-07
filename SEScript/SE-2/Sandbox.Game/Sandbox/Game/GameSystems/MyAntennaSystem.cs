using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 588, typeof(MyObjectBuilder_AntennaSessionComponent), null)]
	public class MyAntennaSystem : MySessionComponentBase
	{
		public struct BroadcasterInfo
		{
			public long EntityId;

			public string Name;
		}

		public class BroadcasterInfoComparer : IEqualityComparer<BroadcasterInfo>
		{
			public bool Equals(BroadcasterInfo x, BroadcasterInfo y)
			{
				if (x.EntityId == y.EntityId)
				{
					return string.Equals(x.Name, y.Name);
				}
				return false;
			}

			public int GetHashCode(BroadcasterInfo obj)
			{
				int num = obj.EntityId.GetHashCode();
				if (obj.Name != null)
				{
					num = ((num * 397) ^ obj.Name.GetHashCode());
				}
				return num;
			}
		}

		private static MyAntennaSystem m_static;

		private List<long> m_addedItems = new List<long>();

		private HashSet<BroadcasterInfo> m_output = new HashSet<BroadcasterInfo>(new BroadcasterInfoComparer());

		private HashSet<MyDataBroadcaster> m_tempPlayerRelayedBroadcasters = new HashSet<MyDataBroadcaster>();

		private List<MyDataBroadcaster> m_tempGridBroadcastersFromPlayer = new List<MyDataBroadcaster>();

		private HashSet<MyDataReceiver> m_tmpReceivers = new HashSet<MyDataReceiver>();

		private HashSet<MyDataBroadcaster> m_tmpBroadcasters = new HashSet<MyDataBroadcaster>();

		private HashSet<MyDataBroadcaster> m_tmpRelayedBroadcasters = new HashSet<MyDataBroadcaster>();

		private Dictionary<long, MyLaserBroadcaster> m_laserAntennas = new Dictionary<long, MyLaserBroadcaster>();

		private Dictionary<long, MyProxyAntenna> m_proxyAntennas = new Dictionary<long, MyProxyAntenna>();

		private Dictionary<long, HashSet<MyDataBroadcaster>> m_proxyGrids = new Dictionary<long, HashSet<MyDataBroadcaster>>();

		public static MyAntennaSystem Static => m_static;

		public Dictionary<long, MyLaserBroadcaster> LaserAntennas => m_laserAntennas;

		public override void LoadData()
		{
			m_static = this;
			base.LoadData();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_addedItems.Clear();
			m_addedItems = null;
			m_output.Clear();
			m_output = null;
			m_tempGridBroadcastersFromPlayer.Clear();
			m_tempGridBroadcastersFromPlayer = null;
			m_tempPlayerRelayedBroadcasters.Clear();
			m_tempPlayerRelayedBroadcasters = null;
			m_static = null;
		}

		public HashSet<BroadcasterInfo> GetConnectedGridsInfo(MyEntity interactedEntityRepresentative, MyPlayer player = null, bool mutual = true, bool accessible = false)
		{
			m_output.Clear();
			if (player == null)
			{
				player = MySession.Static.LocalHumanPlayer;
				if (player == null)
				{
					return m_output;
				}
			}
			MyIdentity identity = player.Identity;
			m_tmpReceivers.Clear();
			m_tmpRelayedBroadcasters.Clear();
			if (interactedEntityRepresentative == null)
			{
				return m_output;
			}
			m_output.Add(new BroadcasterInfo
			{
				EntityId = interactedEntityRepresentative.EntityId,
				Name = interactedEntityRepresentative.DisplayName
			});
			GetAllRelayedBroadcasters(interactedEntityRepresentative, identity.IdentityId, mutual, m_tmpRelayedBroadcasters);
			foreach (MyDataBroadcaster tmpRelayedBroadcaster in m_tmpRelayedBroadcasters)
			{
				if (!accessible || tmpRelayedBroadcaster.CanBeUsedByPlayer(identity.IdentityId))
				{
					m_output.Add(tmpRelayedBroadcaster.Info);
				}
			}
			return m_output;
		}

		public MyEntity GetBroadcasterParentEntity(MyDataBroadcaster broadcaster)
		{
			if (broadcaster.Entity is MyCubeBlock)
			{
				return (broadcaster.Entity as MyCubeBlock).CubeGrid;
			}
			return broadcaster.Entity as MyEntity;
		}

		public MyCubeGrid GetLogicalGroupRepresentative(MyCubeGrid grid)
		{
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid);
			if (group == null || group.Nodes.Count == 0)
			{
				return grid;
			}
			MyCubeGrid nodeData = group.Nodes.First().NodeData;
			foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
			{
				if (node.NodeData.GetBlocks().Count > nodeData.GetBlocks().Count)
				{
					nodeData = node.NodeData;
				}
			}
			return nodeData;
		}

		public void GetEntityBroadcasters(MyEntity entity, ref HashSet<MyDataBroadcaster> output, long playerId = 0L)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				output.Add(myCharacter.RadioBroadcaster);
				MyCubeGrid myCubeGrid = myCharacter.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid != null)
				{
					GetCubeGridGroupBroadcasters(myCubeGrid, output, playerId);
				}
				return;
			}
			MyCubeBlock myCubeBlock = entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				GetCubeGridGroupBroadcasters(myCubeBlock.CubeGrid, output, playerId);
				return;
			}
			MyCubeGrid myCubeGrid2 = entity as MyCubeGrid;
			if (myCubeGrid2 != null)
			{
				GetCubeGridGroupBroadcasters(myCubeGrid2, output, playerId);
				return;
			}
			MyProxyAntenna myProxyAntenna = entity as MyProxyAntenna;
			if (myProxyAntenna != null)
			{
				GetProxyGridBroadcasters(myProxyAntenna, ref output, playerId);
			}
		}

		public static void GetCubeGridGroupBroadcasters(MyCubeGrid grid, HashSet<MyDataBroadcaster> output, long playerId = 0L)
		{
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid);
			if (group != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node member in group.m_members)
				{
					foreach (MyDataBroadcaster broadcaster in member.NodeData.GridSystems.RadioSystem.Broadcasters)
					{
						if (playerId == 0L || broadcaster.CanBeUsedByPlayer(playerId))
						{
							output.Add(broadcaster);
						}
					}
				}
			}
			else
			{
				foreach (MyDataBroadcaster broadcaster2 in grid.GridSystems.RadioSystem.Broadcasters)
				{
					if (playerId == 0L || broadcaster2.CanBeUsedByPlayer(playerId))
					{
						output.Add(broadcaster2);
					}
				}
			}
		}

		private void GetProxyGridBroadcasters(MyProxyAntenna proxy, ref HashSet<MyDataBroadcaster> output, long playerId = 0L)
		{
			if (m_proxyGrids.TryGetValue(proxy.Info.EntityId, out HashSet<MyDataBroadcaster> value))
			{
				foreach (MyDataBroadcaster item in value)
				{
					if (playerId == 0L || item.CanBeUsedByPlayer(playerId))
					{
						output.Add(item);
					}
				}
			}
		}

		public void GetEntityReceivers(MyEntity entity, ref HashSet<MyDataReceiver> output, long playerId = 0L)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				output.Add(myCharacter.RadioReceiver);
				MyCubeGrid myCubeGrid = myCharacter.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid != null)
				{
					GetCubeGridGroupReceivers(myCubeGrid, ref output, playerId);
				}
				return;
			}
			MyCubeBlock myCubeBlock = entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				GetCubeGridGroupReceivers(myCubeBlock.CubeGrid, ref output, playerId);
				return;
			}
			MyCubeGrid myCubeGrid2 = entity as MyCubeGrid;
			if (myCubeGrid2 != null)
			{
				GetCubeGridGroupReceivers(myCubeGrid2, ref output, playerId);
				return;
			}
			MyProxyAntenna myProxyAntenna = entity as MyProxyAntenna;
			if (myProxyAntenna != null)
			{
				GetProxyGridReceivers(myProxyAntenna, ref output, playerId);
			}
		}

		private void GetCubeGridGroupReceivers(MyCubeGrid grid, ref HashSet<MyDataReceiver> output, long playerId = 0L)
		{
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid);
			if (group != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node member in group.m_members)
				{
					foreach (MyDataReceiver receiver in member.NodeData.GridSystems.RadioSystem.Receivers)
					{
						if (playerId == 0L || receiver.CanBeUsedByPlayer(playerId))
						{
							output.Add(receiver);
						}
					}
				}
			}
			else
			{
				foreach (MyDataReceiver receiver2 in grid.GridSystems.RadioSystem.Receivers)
				{
					if (playerId == 0L || receiver2.CanBeUsedByPlayer(playerId))
					{
						output.Add(receiver2);
					}
				}
			}
		}

		private void GetProxyGridReceivers(MyProxyAntenna proxy, ref HashSet<MyDataReceiver> output, long playerId = 0L)
		{
			if (m_proxyGrids.TryGetValue(proxy.Info.EntityId, out HashSet<MyDataBroadcaster> value))
			{
				foreach (MyDataBroadcaster item in value)
				{
					if (item.Receiver != null && (playerId == 0L || item.CanBeUsedByPlayer(playerId)))
					{
						output.Add(item.Receiver);
					}
				}
			}
		}

		public HashSet<MyDataBroadcaster> GetAllRelayedBroadcasters(MyDataReceiver receiver, long identityId, bool mutual, HashSet<MyDataBroadcaster> output = null)
		{
			if (output == null)
			{
				output = m_tmpBroadcasters;
				output.Clear();
			}
			foreach (MyDataBroadcaster item in receiver.BroadcastersInRange)
			{
				if (!output.Contains(item) && !item.Closed && (!mutual || (item.Receiver != null && receiver.Broadcaster != null && item.Receiver.BroadcastersInRange.Contains(receiver.Broadcaster))))
				{
					output.Add(item);
					if (item.Receiver != null && item.CanBeUsedByPlayer(identityId))
					{
						GetAllRelayedBroadcasters(item.Receiver, identityId, mutual, output);
					}
				}
			}
			return output;
		}

		public HashSet<MyDataBroadcaster> GetAllRelayedBroadcasters(MyEntity entity, long identityId, bool mutual = true, HashSet<MyDataBroadcaster> output = null)
		{
			if (output == null)
			{
				output = m_tmpBroadcasters;
				output.Clear();
			}
			m_tmpReceivers.Clear();
			GetEntityReceivers(entity, ref m_tmpReceivers, identityId);
			foreach (MyDataReceiver tmpReceiver in m_tmpReceivers)
			{
				GetAllRelayedBroadcasters(tmpReceiver, identityId, mutual, output);
			}
			return output;
		}

		public bool CheckConnection(MyIdentity sender, MyIdentity receiver)
		{
			if (sender == receiver)
			{
				return true;
			}
			if (sender.Character == null || receiver.Character == null)
			{
				return false;
			}
			return CheckConnection(receiver.Character.RadioReceiver, sender.Character.RadioBroadcaster, receiver.IdentityId, mutual: false);
		}

		public bool CheckConnection(MyDataReceiver receiver, MyDataBroadcaster broadcaster, long playerIdentityId, bool mutual)
		{
			if (receiver == null || broadcaster == null)
			{
				return false;
			}
			return GetAllRelayedBroadcasters(receiver, playerIdentityId, mutual).Contains(broadcaster);
		}

		public bool CheckConnection(MyEntity receivingEntity, MyDataBroadcaster broadcaster, long playerIdentityId, bool mutual)
		{
			if (receivingEntity == null || broadcaster == null)
			{
				return false;
			}
			return GetAllRelayedBroadcasters(receivingEntity, playerIdentityId, mutual).Contains(broadcaster);
		}

		public bool CheckConnection(MyDataReceiver receiver, MyEntity broadcastingEntity, long playerIdentityId, bool mutual)
		{
			if (receiver == null || broadcastingEntity == null)
			{
				return false;
			}
			m_tmpBroadcasters.Clear();
			m_tmpRelayedBroadcasters.Clear();
			GetAllRelayedBroadcasters(receiver, playerIdentityId, mutual, m_tmpRelayedBroadcasters);
			GetEntityBroadcasters(broadcastingEntity, ref m_tmpBroadcasters, playerIdentityId);
			foreach (MyDataBroadcaster tmpRelayedBroadcaster in m_tmpRelayedBroadcasters)
			{
				if (m_tmpBroadcasters.Contains(tmpRelayedBroadcaster))
				{
					return true;
				}
			}
			return false;
		}

		public bool CheckConnection(MyEntity broadcastingEntity, MyEntity receivingEntity, MyPlayer player, bool mutual = true)
		{
			MyCubeGrid myCubeGrid = broadcastingEntity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				broadcastingEntity = GetLogicalGroupRepresentative(myCubeGrid);
			}
			MyCubeGrid myCubeGrid2 = receivingEntity as MyCubeGrid;
			if (myCubeGrid2 != null)
			{
				receivingEntity = GetLogicalGroupRepresentative(myCubeGrid2);
			}
			foreach (BroadcasterInfo item in GetConnectedGridsInfo(receivingEntity, player, mutual))
			{
				if (item.EntityId == broadcastingEntity.EntityId)
				{
					return true;
				}
			}
			return false;
		}

		public void RegisterAntenna(MyDataBroadcaster broadcaster)
		{
			MyProxyAntenna value;
			if (broadcaster.Entity is MyProxyAntenna)
			{
				MyProxyAntenna myProxyAntenna = broadcaster.Entity as MyProxyAntenna;
				m_proxyAntennas[broadcaster.AntennaEntityId] = myProxyAntenna;
				RegisterProxyGrid(broadcaster);
				if (MyEntities.GetEntityById(broadcaster.AntennaEntityId) == null)
				{
					myProxyAntenna.Active = true;
				}
			}
			else if (m_proxyAntennas.TryGetValue(broadcaster.AntennaEntityId, out value))
			{
				value.Active = false;
			}
		}

		public void UnregisterAntenna(MyDataBroadcaster broadcaster)
		{
			MyProxyAntenna value;
			if (broadcaster.Entity is MyProxyAntenna)
			{
				MyProxyAntenna obj = broadcaster.Entity as MyProxyAntenna;
				m_proxyAntennas.Remove(broadcaster.AntennaEntityId);
				UnregisterProxyGrid(broadcaster);
				obj.Active = false;
			}
			else if (m_proxyAntennas.TryGetValue(broadcaster.AntennaEntityId, out value))
			{
				value.Active = true;
			}
		}

		private void RegisterProxyGrid(MyDataBroadcaster broadcaster)
		{
			if (!m_proxyGrids.TryGetValue(broadcaster.Info.EntityId, out HashSet<MyDataBroadcaster> value))
			{
				value = new HashSet<MyDataBroadcaster>();
				m_proxyGrids.Add(broadcaster.Info.EntityId, value);
			}
			value.Add(broadcaster);
		}

		private void UnregisterProxyGrid(MyDataBroadcaster broadcaster)
		{
			if (m_proxyGrids.TryGetValue(broadcaster.Info.EntityId, out HashSet<MyDataBroadcaster> value))
			{
				value.Remove(broadcaster);
				if (value.Count == 0)
				{
					m_proxyGrids.Remove(broadcaster.Info.EntityId);
				}
			}
		}

		public void AddLaser(long id, MyLaserBroadcaster laser, bool register = true)
		{
			if (register)
			{
				RegisterAntenna(laser);
			}
			m_laserAntennas.Add(id, laser);
		}

		public void RemoveLaser(long id, bool register = true)
		{
			if (m_laserAntennas.TryGetValue(id, out MyLaserBroadcaster value))
			{
				m_laserAntennas.Remove(id);
				if (register)
				{
					UnregisterAntenna(value);
				}
			}
		}
	}
}
