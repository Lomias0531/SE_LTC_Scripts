using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	public class MyToolBarCollection
	{
		protected sealed class OnClearSlotRequest_003C_003ESystem_Int32_0023System_Int32 : ICallSite<IMyEventOwner, int, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int playerSerialId, in int index, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnClearSlotRequest(playerSerialId, index);
			}
		}

		protected sealed class OnChangeSlotItemRequest_003C_003ESystem_Int32_0023System_Int32_0023VRage_Game_DefinitionIdBlit : ICallSite<IMyEventOwner, int, int, DefinitionIdBlit, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int playerSerialId, in int index, in DefinitionIdBlit defId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChangeSlotItemRequest(playerSerialId, index, defId);
			}
		}

		protected sealed class OnChangeSlotBuilderItemRequest_003C_003ESystem_Int32_0023System_Int32_0023VRage_Game_MyObjectBuilder_ToolbarItem : ICallSite<IMyEventOwner, int, int, MyObjectBuilder_ToolbarItem, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int playerSerialId, in int index, in MyObjectBuilder_ToolbarItem itemBuilder, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChangeSlotBuilderItemRequest(playerSerialId, index, itemBuilder);
			}
		}

		protected sealed class OnNewToolbarRequest_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int playerSerialId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnNewToolbarRequest(playerSerialId);
			}
		}

		private Dictionary<MyPlayer.PlayerId, MyToolbar> m_playerToolbars = new Dictionary<MyPlayer.PlayerId, MyToolbar>();

		public static void RequestClearSlot(MyPlayer.PlayerId pid, int index)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnClearSlotRequest, pid.SerialId, index);
		}

		[Event(null, 34)]
		[Reliable]
		[Server]
		private static void OnClearSlotRequest(int playerSerialId, int index)
		{
			ulong senderIdSafe = GetSenderIdSafe();
			MyPlayer.PlayerId pid = new MyPlayer.PlayerId(senderIdSafe, playerSerialId);
			if (MySession.Static.Toolbars.ContainsToolbar(pid))
			{
				MySession.Static.Toolbars.TryGetPlayerToolbar(pid).SetItemAtIndex(index, null);
			}
		}

		public static void RequestChangeSlotItem(MyPlayer.PlayerId pid, int index, MyDefinitionId defId)
		{
			DefinitionIdBlit definitionIdBlit = default(DefinitionIdBlit);
			definitionIdBlit = defId;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChangeSlotItemRequest, pid.SerialId, index, definitionIdBlit);
		}

		[Event(null, 54)]
		[Reliable]
		[Server]
		private static void OnChangeSlotItemRequest(int playerSerialId, int index, DefinitionIdBlit defId)
		{
			ulong senderIdSafe = GetSenderIdSafe();
			MyPlayer.PlayerId pid = new MyPlayer.PlayerId(senderIdSafe, playerSerialId);
			if (MySession.Static.Toolbars.ContainsToolbar(pid))
			{
				MyDefinitionManager.Static.TryGetDefinition(defId, out MyDefinitionBase definition);
				if (definition != null)
				{
					MyToolbarItem item = MyToolbarItemFactory.CreateToolbarItem(MyToolbarItemFactory.ObjectBuilderFromDefinition(definition));
					MySession.Static.Toolbars.TryGetPlayerToolbar(pid)?.SetItemAtIndex(index, item);
				}
			}
		}

		public static void RequestChangeSlotItem(MyPlayer.PlayerId pid, int index, MyObjectBuilder_ToolbarItem itemBuilder)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChangeSlotBuilderItemRequest, pid.SerialId, index, itemBuilder);
		}

		[Event(null, 80)]
		[Reliable]
		[Server]
		private static void OnChangeSlotBuilderItemRequest(int playerSerialId, int index, [Serialize(MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))] MyObjectBuilder_ToolbarItem itemBuilder)
		{
			ulong senderIdSafe = GetSenderIdSafe();
			MyPlayer.PlayerId pid = new MyPlayer.PlayerId(senderIdSafe, playerSerialId);
			if (MySession.Static.Toolbars.ContainsToolbar(pid))
			{
				MyToolbarItem item = MyToolbarItemFactory.CreateToolbarItem(itemBuilder);
				MySession.Static.Toolbars.TryGetPlayerToolbar(pid)?.SetItemAtIndex(index, item);
			}
		}

		public static void RequestCreateToolbar(MyPlayer.PlayerId pid)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnNewToolbarRequest, pid.SerialId);
		}

		[Event(null, 101)]
		[Reliable]
		[Server]
		private static void OnNewToolbarRequest(int playerSerialId)
		{
			ulong senderIdSafe = GetSenderIdSafe();
			MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(senderIdSafe, playerSerialId);
			MySession.Static.Toolbars.CreateDefaultToolbar(playerId);
		}

		public bool AddPlayerToolbar(MyPlayer.PlayerId pid, MyToolbar toolbar)
		{
			if (toolbar == null)
			{
				return false;
			}
			if (!m_playerToolbars.TryGetValue(pid, out MyToolbar _))
			{
				m_playerToolbars.Add(pid, toolbar);
				return true;
			}
			return false;
		}

		public bool RemovePlayerToolbar(MyPlayer.PlayerId pid)
		{
			return m_playerToolbars.Remove(pid);
		}

		public MyToolbar TryGetPlayerToolbar(MyPlayer.PlayerId pid)
		{
			m_playerToolbars.TryGetValue(pid, out MyToolbar value);
			return value;
		}

		public bool ContainsToolbar(MyPlayer.PlayerId pid)
		{
			return m_playerToolbars.ContainsKey(pid);
		}

		public void LoadToolbars(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (checkpoint.AllPlayersData != null)
			{
				foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> item in checkpoint.AllPlayersData.Dictionary)
				{
					MyPlayer.PlayerId pid = new MyPlayer.PlayerId(item.Key.ClientId, item.Key.SerialId);
					MyToolbar myToolbar = new MyToolbar(MyToolbarType.Character);
					myToolbar.Init(item.Value.Toolbar, null, skipAssert: true);
					AddPlayerToolbar(pid, myToolbar);
				}
			}
		}

		public void SaveToolbars(MyObjectBuilder_Checkpoint checkpoint)
		{
			Dictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> dictionary = checkpoint.AllPlayersData.Dictionary;
			foreach (KeyValuePair<MyPlayer.PlayerId, MyToolbar> playerToolbar in m_playerToolbars)
			{
				MyObjectBuilder_Checkpoint.PlayerId playerId = new MyObjectBuilder_Checkpoint.PlayerId(playerToolbar.Key.SteamId);
				playerId.SerialId = playerToolbar.Key.SerialId;
				MyObjectBuilder_Checkpoint.PlayerId key = playerId;
				if (dictionary.ContainsKey(key))
				{
					dictionary[key].Toolbar = playerToolbar.Value.GetObjectBuilder();
				}
			}
		}

		private void CreateDefaultToolbar(MyPlayer.PlayerId playerId)
		{
			if (!ContainsToolbar(playerId))
			{
				MyToolbar myToolbar = new MyToolbar(MyToolbarType.Character);
				myToolbar.Init(MySession.Static.Scenario.DefaultToolbar, null, skipAssert: true);
				AddPlayerToolbar(playerId, myToolbar);
			}
		}

		private static ulong GetSenderIdSafe()
		{
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				return Sync.MyId;
			}
			return MyEventContext.Current.Sender.Value;
		}
	}
}
