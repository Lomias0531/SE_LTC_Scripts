using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Gui;
using VRage.Groups;

namespace Sandbox.Game.Entities
{
	public abstract class MyDataReceiver : MyEntityComponentBase
	{
		public delegate void BroadcasterChangeInfo(MyDataBroadcaster broadcaster);

		protected List<MyDataBroadcaster> m_tmpBroadcasters = new List<MyDataBroadcaster>();

		protected HashSet<MyDataBroadcaster> m_broadcastersInRange = new HashSet<MyDataBroadcaster>();

		protected List<MyDataBroadcaster> m_lastBroadcastersInRange = new List<MyDataBroadcaster>();

		private HashSet<MyGridLogicalGroupData> m_broadcastersInRange_TopGrids = new HashSet<MyGridLogicalGroupData>();

		private HashSet<long> m_entitiesOnHud = new HashSet<long>();

		public bool Enabled
		{
			get;
			set;
		}

		public HashSet<MyDataBroadcaster> BroadcastersInRange => m_broadcastersInRange;

		public MyDataBroadcaster Broadcaster
		{
			get
			{
				MyDataBroadcaster component = null;
				if (base.Container != null)
				{
					base.Container.TryGet(out component);
				}
				return component;
			}
		}

		public override string ComponentTypeDebugString => "MyDataReciever";

		public event BroadcasterChangeInfo OnBroadcasterFound;

		public event BroadcasterChangeInfo OnBroadcasterLost;

		public void UpdateBroadcastersInRange()
		{
			m_broadcastersInRange.Clear();
			if (!MyFakes.ENABLE_RADIO_HUD || !Enabled)
			{
				return;
			}
			if (base.Entity.Components.TryGet(out MyDataBroadcaster component))
			{
				m_broadcastersInRange.Add(component);
			}
			GetBroadcastersInMyRange(ref m_broadcastersInRange);
			for (int num = m_lastBroadcastersInRange.Count - 1; num >= 0; num--)
			{
				MyDataBroadcaster myDataBroadcaster = m_lastBroadcastersInRange[num];
				if (!m_broadcastersInRange.Contains(myDataBroadcaster))
				{
					m_lastBroadcastersInRange.RemoveAtFast(num);
					this.OnBroadcasterLost?.Invoke(myDataBroadcaster);
				}
			}
			foreach (MyDataBroadcaster item in m_broadcastersInRange)
			{
				if (!m_lastBroadcastersInRange.Contains(item))
				{
					m_lastBroadcastersInRange.Add(item);
					this.OnBroadcasterFound?.Invoke(item);
				}
			}
		}

		public bool CanBeUsedByPlayer(long playerId)
		{
			return MyDataBroadcaster.CanBeUsedByPlayer(playerId, base.Entity);
		}

		protected abstract void GetBroadcastersInMyRange(ref HashSet<MyDataBroadcaster> broadcastersInRange);

		public void UpdateHud(bool showMyself = false)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && !MyHud.MinimalHud && !MyHud.CutsceneHud)
			{
				Clear();
				foreach (MyDataBroadcaster allRelayedBroadcaster in MyAntennaSystem.Static.GetAllRelayedBroadcasters(this, MySession.Static.LocalPlayerId, mutual: false))
				{
					bool allowBlink = allRelayedBroadcaster.CanBeUsedByPlayer(MySession.Static.LocalPlayerId);
					MyCubeGrid myCubeGrid = allRelayedBroadcaster.Entity.GetTopMostParent() as MyCubeGrid;
					if (myCubeGrid != null)
					{
						MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid);
						if (group != null)
						{
							MyGridLogicalGroupData groupData = group.GroupData;
							m_broadcastersInRange_TopGrids.Add(groupData);
						}
					}
					if (allRelayedBroadcaster.ShowOnHud)
					{
						foreach (MyHudEntityParams hudParam in allRelayedBroadcaster.GetHudParams(allowBlink))
						{
							if (!m_entitiesOnHud.Contains(hudParam.EntityId))
							{
								m_entitiesOnHud.Add(hudParam.EntityId);
								if (hudParam.BlinkingTime > 0f)
								{
									MyHud.HackingMarkers.RegisterMarker(hudParam.EntityId, hudParam);
								}
								else if (!MyHud.HackingMarkers.MarkerEntities.ContainsKey(hudParam.EntityId))
								{
									MyHud.LocationMarkers.RegisterMarker(hudParam.EntityId, hudParam);
								}
							}
						}
					}
				}
				if (MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.ShowPlayers))
				{
					foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
					{
						MyCharacter character = onlinePlayer.Character;
						if (character != null)
						{
							foreach (MyHudEntityParams hudParam2 in character.GetHudParams(allowBlink: false))
							{
								if (!m_entitiesOnHud.Contains(hudParam2.EntityId))
								{
									m_entitiesOnHud.Add(hudParam2.EntityId);
									MyHud.LocationMarkers.RegisterMarker(hudParam2.EntityId, hudParam2);
								}
							}
						}
					}
				}
			}
		}

		public bool HasAccessToLogicalGroup(MyGridLogicalGroupData group)
		{
			return m_broadcastersInRange_TopGrids.Contains(group);
		}

		public void Clear()
		{
			foreach (long item in m_entitiesOnHud)
			{
				MyHud.LocationMarkers.UnregisterMarker(item);
			}
			m_entitiesOnHud.Clear();
			m_broadcastersInRange_TopGrids.Clear();
		}
	}
}
