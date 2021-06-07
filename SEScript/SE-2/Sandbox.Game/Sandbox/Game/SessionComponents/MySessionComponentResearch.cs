using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666, typeof(MyObjectBuilder_SessionComponentResearch), null)]
	public class MySessionComponentResearch : MySessionComponentBase
	{
		protected sealed class UnlockResearchSuccess_003C_003ESystem_Int64_0023VRage_ObjectBuilders_SerializableDefinitionId_0023System_Int64 : ICallSite<IMyEventOwner, long, SerializableDefinitionId, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in SerializableDefinitionId id, in long unlockerId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UnlockResearchSuccess(identityId, id, unlockerId);
			}
		}

		protected sealed class FactionUnlockFailed_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				FactionUnlockFailed();
			}
		}

		protected sealed class CallShareResearch_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long toIdentity, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CallShareResearch(toIdentity);
			}
		}

		protected sealed class ShareResearch_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long toIdentity, in long fromIdentityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShareResearch(toIdentity, fromIdentityId);
			}
		}

		protected sealed class DebugUnlockAllResearchSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DebugUnlockAllResearchSync(identityId);
			}
		}

		protected sealed class AddRequiredResearchSync_003C_003EVRage_ObjectBuilders_SerializableDefinitionId : ICallSite<IMyEventOwner, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in SerializableDefinitionId itemId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddRequiredResearchSync(itemId);
			}
		}

		protected sealed class RemoveRequiredResearchSync_003C_003EVRage_ObjectBuilders_SerializableDefinitionId : ICallSite<IMyEventOwner, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in SerializableDefinitionId itemId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveRequiredResearchSync(itemId);
			}
		}

		protected sealed class ClearRequiredResearchSync_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ClearRequiredResearchSync();
			}
		}

		protected sealed class ResetResearchForAllSync_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ResetResearchForAllSync();
			}
		}

		protected sealed class LockResearchSync_003C_003ESystem_Int64_0023VRage_ObjectBuilders_SerializableDefinitionId : ICallSite<IMyEventOwner, long, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long characterId, in SerializableDefinitionId itemId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				LockResearchSync(characterId, itemId);
			}
		}

		protected sealed class UnlockResearchDirectSync_003C_003ESystem_Int64_0023VRage_ObjectBuilders_SerializableDefinitionId : ICallSite<IMyEventOwner, long, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long characterId, in SerializableDefinitionId itemId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UnlockResearchDirectSync(characterId, itemId);
			}
		}

		protected sealed class ResetResearchSync_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ResetResearchSync(identityId);
			}
		}

		protected sealed class SwitchWhitelistModeSync_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool whitelist, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SwitchWhitelistModeSync(whitelist);
			}
		}

		public bool DEBUG_SHOW_RESEARCH;

		public bool DEBUG_SHOW_RESEARCH_PRETTY = true;

		public static MySessionComponentResearch Static;

		private Dictionary<long, HashSet<MyDefinitionId>> m_unlockedResearch;

		private Dictionary<long, HashSet<MyDefinitionId>> m_unlockedBlocks;

		public List<MyDefinitionId> m_requiredResearch;

		private Dictionary<MyDefinitionId, List<MyDefinitionId>> m_unlocks;

		private Dictionary<long, bool> m_failedSent;

		private MyHudNotification m_unlockedResearchNotification;

		private MyHudNotification m_factionUnlockedResearchNotification;

		private MyHudNotification m_factionFailedResearchNotification;

		private MyHudNotification m_sharedResearchNotification;

		private MyHudNotification m_knownResearchNotification;

		public bool WhitelistMode
		{
			get;
			private set;
		}

		public override bool IsRequiredByGame => MyPerGameSettings.EnableResearch;

		public override Type[] Dependencies => base.Dependencies;

		public MySessionComponentResearch()
		{
			Static = this;
			m_unlockedResearch = new Dictionary<long, HashSet<MyDefinitionId>>();
			m_unlockedBlocks = new Dictionary<long, HashSet<MyDefinitionId>>();
			m_requiredResearch = new List<MyDefinitionId>();
			m_unlocks = new Dictionary<MyDefinitionId, List<MyDefinitionId>>();
			m_failedSent = new Dictionary<long, bool>();
			m_unlockedResearchNotification = new MyHudNotification(MyCommonTexts.NotificationResearchUnlocked, 2500, "White", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2);
			m_factionUnlockedResearchNotification = new MyHudNotification(MyCommonTexts.NotificationFactionResearchUnlocked, 2500, "White", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2);
			m_factionFailedResearchNotification = new MyHudNotification(MyCommonTexts.NotificationFactionResearchFailed, 2500, "White", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2);
			m_sharedResearchNotification = new MyHudNotification(MyCommonTexts.NotificationSharedResearch, 2500, "White", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2);
			m_knownResearchNotification = new MyHudNotification(MyCommonTexts.NotificationResearchKnown, 2500, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 2);
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			if (Static != null)
			{
				foreach (MyCubeBlockDefinition item in MyDefinitionManager.Static.GetDefinitionsOfType<MyCubeBlockDefinition>())
				{
					MyResearchBlockDefinition researchBlock = MyDefinitionManager.Static.GetResearchBlock(item.Id);
					if (researchBlock != null)
					{
						if (item.CubeSize == MyCubeSize.Large)
						{
							MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(item.BlockPairName);
							if (definitionGroup.Small != null)
							{
								_ = MyDefinitionManager.Static.GetResearchBlock(definitionGroup.Small.Id)?.UnlockedByGroups;
							}
						}
						string[] unlockedByGroups = researchBlock.UnlockedByGroups;
						foreach (string subtype in unlockedByGroups)
						{
							MyResearchGroupDefinition researchGroup = MyDefinitionManager.Static.GetResearchGroup(subtype);
							if (researchGroup != null && researchGroup.Members != null && researchGroup.Members.Length != 0)
							{
								m_requiredResearch.Add(item.Id);
								SerializableDefinitionId[] members = researchGroup.Members;
								foreach (SerializableDefinitionId v in members)
								{
									if (!m_unlocks.TryGetValue(v, out List<MyDefinitionId> value))
									{
										value = new List<MyDefinitionId>();
										m_unlocks.Add(v, value);
									}
									value.Add(item.Id);
								}
							}
						}
					}
				}
			}
			MyObjectBuilder_SessionComponentResearch myObjectBuilder_SessionComponentResearch = sessionComponent as MyObjectBuilder_SessionComponentResearch;
			if (myObjectBuilder_SessionComponentResearch != null && myObjectBuilder_SessionComponentResearch.Researches != null)
			{
				foreach (MyObjectBuilder_SessionComponentResearch.ResearchData research in myObjectBuilder_SessionComponentResearch.Researches)
				{
					HashSet<MyDefinitionId> hashSet = new HashSet<MyDefinitionId>();
					HashSet<MyDefinitionId> hashSet2 = new HashSet<MyDefinitionId>();
					foreach (SerializableDefinitionId definition in research.Definitions)
					{
						hashSet.Add(definition);
						if (Static.m_unlocks.TryGetValue(definition, out List<MyDefinitionId> value2))
						{
							foreach (MyDefinitionId item2 in value2)
							{
								hashSet2.Add(item2);
							}
						}
					}
					m_unlockedResearch.Add(research.IdentityId, hashSet);
					m_unlockedBlocks.Add(research.IdentityId, hashSet2);
				}
				WhitelistMode = myObjectBuilder_SessionComponentResearch.WhitelistMode;
				if (WhitelistMode)
				{
					m_requiredResearch.Clear();
				}
			}
			if (Sync.IsServer && MySession.Static.ResearchEnabled)
			{
				MyCubeGrids.BlockFunctional += OnBlockBuilt;
			}
		}

		private void OnBlockBuilt(MyCubeGrid grid, MySlimBlock block, bool handWelded)
		{
			if (handWelded)
			{
				long builtBy = block.BuiltBy;
				MyDefinitionId id = block.BlockDefinition.Id;
				IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(builtBy);
				if (myFaction != null)
				{
					foreach (long key in myFaction.Members.Keys)
					{
						if (MySession.Static.Players.IsPlayerOnline(key))
						{
							UnlockResearch(key, id, builtBy);
						}
					}
				}
				else
				{
					UnlockResearch(builtBy, id, builtBy);
				}
			}
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_SessionComponentResearch myObjectBuilder_SessionComponentResearch = new MyObjectBuilder_SessionComponentResearch();
			myObjectBuilder_SessionComponentResearch.Researches = new List<MyObjectBuilder_SessionComponentResearch.ResearchData>();
			foreach (KeyValuePair<long, HashSet<MyDefinitionId>> item in m_unlockedResearch)
			{
				if (item.Value.Count != 0)
				{
					List<SerializableDefinitionId> list = new List<SerializableDefinitionId>();
					foreach (MyDefinitionId item2 in item.Value)
					{
						list.Add(item2);
					}
					myObjectBuilder_SessionComponentResearch.Researches.Add(new MyObjectBuilder_SessionComponentResearch.ResearchData
					{
						IdentityId = item.Key,
						Definitions = list
					});
				}
			}
			myObjectBuilder_SessionComponentResearch.WhitelistMode = WhitelistMode;
			return myObjectBuilder_SessionComponentResearch;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_unlockedResearch = null;
			m_unlockedBlocks = null;
			m_requiredResearch = null;
			m_unlocks = null;
			if (Sync.IsServer && MySession.Static.ResearchEnabled)
			{
				MyCubeGrids.BlockFunctional -= OnBlockBuilt;
			}
		}

		public void UnlockResearch(long identityId, MyDefinitionId id, long unlockerId)
		{
			if (!m_unlockedResearch.TryGetValue(identityId, out HashSet<MyDefinitionId> value))
			{
				value = new HashSet<MyDefinitionId>();
				m_unlockedResearch.Add(identityId, value);
				m_unlockedBlocks.Add(identityId, new HashSet<MyDefinitionId>());
			}
			if (value.Contains(id))
			{
				return;
			}
			SerializableDefinitionId arg = id;
			if (!CanUse(identityId, id))
			{
				if (unlockerId != identityId)
				{
					if (!m_failedSent.TryGetValue(identityId, out bool value2) || !value2)
					{
						ulong num = MySession.Static.Players.TryGetSteamId(identityId);
						if (num != 0L)
						{
							MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => FactionUnlockFailed, new EndpointId(num));
							m_failedSent[identityId] = true;
						}
						return;
					}
				}
				else if (!MySession.Static.HasPlayerCreativeRights(MySession.Static.Players.TryGetSteamId(identityId)))
				{
					return;
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UnlockResearchSuccess, identityId, arg, unlockerId);
		}

		[Event(null, 285)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void UnlockResearchSuccess(long identityId, SerializableDefinitionId id, long unlockerId)
		{
			if (!MyDefinitionManager.Static.TryGetDefinition(id, out MyDefinitionBase definition) || !Static.UnlockBlocks(identityId, id) || MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.GetPlayerIdentityId() != identityId)
			{
				return;
			}
			if (unlockerId != identityId)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(unlockerId);
				if (myIdentity != null)
				{
					Static.m_factionUnlockedResearchNotification.SetTextFormatArguments(definition.DisplayNameText, myIdentity.DisplayName);
					MyHud.Notifications.Add(Static.m_factionUnlockedResearchNotification);
				}
			}
			else
			{
				Static.m_unlockedResearchNotification.SetTextFormatArguments(definition.DisplayNameText);
				MyHud.Notifications.Add(Static.m_unlockedResearchNotification);
			}
		}

		[Event(null, 321)]
		[Reliable]
		[Client]
		private static void FactionUnlockFailed()
		{
			MyHud.Notifications.Add(Static.m_factionFailedResearchNotification);
		}

		private bool UnlockBlocks(long identityId, MyDefinitionId researchedBlockId)
		{
			if (!m_unlockedBlocks.TryGetValue(identityId, out HashSet<MyDefinitionId> value))
			{
				value = new HashSet<MyDefinitionId>();
				m_unlockedBlocks[identityId] = value;
			}
			if (!m_unlockedResearch.TryGetValue(identityId, out HashSet<MyDefinitionId> value2))
			{
				value2 = new HashSet<MyDefinitionId>();
				m_unlockedResearch[identityId] = value2;
			}
			m_unlocks.TryGetValue(researchedBlockId, out List<MyDefinitionId> value3);
			bool result = false;
			if (value3 != null)
			{
				foreach (MyDefinitionId item in value3)
				{
					if (!value.Contains(item))
					{
						result = true;
						value.Add(item);
					}
				}
			}
			value2.Add(researchedBlockId);
			return result;
		}

		public bool CanUse(MyCharacter character, MyDefinitionId id)
		{
			if (character == null)
			{
				return true;
			}
			return CanUse(character.GetPlayerIdentityId(), id);
		}

		public bool CanUse(long identityId, MyDefinitionId id)
		{
			if (MySession.Static.ResearchEnabled && RequiresResearch(id))
			{
				return IsBlockUnlocked(identityId, id);
			}
			return true;
		}

		public bool RequiresResearch(MyDefinitionId id)
		{
			if (WhitelistMode)
			{
				return !m_requiredResearch.Contains(id);
			}
			return m_requiredResearch.Contains(id);
		}

		public bool IsBlockUnlocked(long identityId, MyDefinitionId id)
		{
			if (!m_unlockedBlocks.TryGetValue(identityId, out HashSet<MyDefinitionId> value))
			{
				return false;
			}
			return value.Contains(id);
		}

		[Event(null, 393)]
		[Reliable]
		[Server]
		public static void CallShareResearch(long toIdentity)
		{
			long arg = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ShareResearch, toIdentity, arg);
		}

		[Event(null, 400)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ShareResearch(long toIdentity, long fromIdentityId)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(fromIdentityId);
			if (myIdentity != null)
			{
				if (Static.m_unlockedResearch.TryGetValue(fromIdentityId, out HashSet<MyDefinitionId> value))
				{
					foreach (MyDefinitionId item in value)
					{
						Static.UnlockBlocks(toIdentity, item);
					}
				}
				if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.GetPlayerIdentityId() == toIdentity)
				{
					Static.m_sharedResearchNotification.SetTextFormatArguments(myIdentity.DisplayName);
					MyHud.Notifications.Add(Static.m_sharedResearchNotification);
				}
			}
		}

		public void ResetResearch(MyCharacter character)
		{
			if (character != null)
			{
				ResetResearch(character.GetPlayerIdentityId());
			}
		}

		public void ResetResearch(long identityId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ResetResearchSync, identityId);
		}

		public void DebugUnlockAllResearch(long identityId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => DebugUnlockAllResearchSync, identityId);
		}

		[Event(null, 444)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void DebugUnlockAllResearchSync(long identityId)
		{
			foreach (MyDefinitionId item in Static.m_requiredResearch)
			{
				if (!Static.m_unlockedBlocks.TryGetValue(identityId, out HashSet<MyDefinitionId> value))
				{
					value = new HashSet<MyDefinitionId>();
					Static.m_unlockedBlocks[identityId] = value;
				}
				if (!Static.m_unlockedResearch.TryGetValue(identityId, out HashSet<MyDefinitionId> value2))
				{
					value2 = new HashSet<MyDefinitionId>();
					Static.m_unlockedResearch[identityId] = value2;
				}
				if (!value.Contains(item))
				{
					value.Add(item);
				}
				value2.Add(item);
			}
		}

		public override void Draw()
		{
			base.Draw();
			if (!DEBUG_SHOW_RESEARCH)
			{
				return;
			}
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null)
			{
				long playerIdentityId = localCharacter.GetPlayerIdentityId();
				if (m_unlockedResearch.TryGetValue(playerIdentityId, out HashSet<MyDefinitionId> value))
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(10f, 180f), $"=== {MySession.Static.LocalHumanPlayer.DisplayName}'s Research ===", Color.DarkViolet, 0.8f);
					int num = 200;
					foreach (MyDefinitionId item in value)
					{
						if (DEBUG_SHOW_RESEARCH_PRETTY)
						{
							MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(item);
							if (definition is MyResearchDefinition)
							{
								MyRenderProxy.DebugDrawText2D(new Vector2(10f, num), $"[R] {definition.DisplayNameText}", Color.DarkViolet, 0.7f);
							}
							else
							{
								MyRenderProxy.DebugDrawText2D(new Vector2(10f, num), definition.DisplayNameText, Color.DarkViolet, 0.7f);
							}
						}
						else
						{
							MyRenderProxy.DebugDrawText2D(new Vector2(10f, num), item.ToString(), Color.DarkViolet, 0.7f);
						}
						num += 16;
					}
				}
			}
		}

		public void AddRequiredResearch(MyDefinitionId itemId)
		{
			if (!itemId.TypeId.IsNull)
			{
				SerializableDefinitionId arg = itemId;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AddRequiredResearchSync, arg);
			}
		}

		[Event(null, 518)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void AddRequiredResearchSync(SerializableDefinitionId itemId)
		{
			if (MyDefinitionManager.Static.TryGetDefinition(itemId, out MyDefinitionBase definition) && !Static.m_requiredResearch.Contains(definition.Id))
			{
				Static.m_requiredResearch.Add(definition.Id);
			}
		}

		public void RemoveRequiredResearch(MyDefinitionId itemId)
		{
			if (!itemId.TypeId.IsNull)
			{
				SerializableDefinitionId arg = itemId;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemoveRequiredResearchSync, arg);
			}
		}

		[Event(null, 540)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveRequiredResearchSync(SerializableDefinitionId itemId)
		{
			if (MyDefinitionManager.Static.TryGetDefinition(itemId, out MyDefinitionBase definition))
			{
				Static.m_requiredResearch.Remove(definition.Id);
			}
		}

		public void ClearRequiredResearch()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ClearRequiredResearchSync);
		}

		[Event(null, 555)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void ClearRequiredResearchSync()
		{
			Static.m_requiredResearch.Clear();
		}

		public void ResetResearchForAll()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ResetResearchForAllSync);
		}

		[Event(null, 566)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void ResetResearchForAllSync()
		{
			Static.m_unlockedResearch.Clear();
			Static.m_unlockedBlocks.Clear();
		}

		public void LockResearch(long characterId, MyDefinitionId itemId)
		{
			if (!itemId.TypeId.IsNull)
			{
				SerializableDefinitionId arg = itemId;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => LockResearchSync, characterId, arg);
			}
		}

		[Event(null, 584)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void LockResearchSync(long characterId, SerializableDefinitionId itemId)
		{
			if (MyDefinitionManager.Static.TryGetDefinition(itemId, out MyDefinitionBase definition) && Static.m_unlockedResearch.ContainsKey(characterId))
			{
				Static.m_unlockedResearch[characterId].Remove(definition.Id);
			}
		}

		public void UnlockResearchDirect(long characterId, MyDefinitionId itemId)
		{
			if (!itemId.TypeId.IsNull)
			{
				SerializableDefinitionId arg = itemId;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UnlockResearchDirectSync, characterId, arg);
			}
		}

		[Event(null, 605)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void UnlockResearchDirectSync(long characterId, SerializableDefinitionId itemId)
		{
			if (MyDefinitionManager.Static.TryGetDefinition(itemId, out MyDefinitionBase definition) && (!Static.m_unlockedResearch.ContainsKey(characterId) || !Static.m_unlockedResearch[characterId].Contains(definition.Id)))
			{
				Static.UnlockBlocks(characterId, itemId);
			}
		}

		[Event(null, 617)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void ResetResearchSync(long identityId)
		{
			if (Static.m_unlockedResearch.ContainsKey(identityId))
			{
				Static.m_unlockedResearch[identityId].Clear();
			}
			if (Static.m_unlockedBlocks.ContainsKey(identityId))
			{
				Static.m_unlockedBlocks[identityId].Clear();
			}
		}

		public void SwitchWhitelistMode(bool whitelist)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SwitchWhitelistModeSync, whitelist);
		}

		[Event(null, 638)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void SwitchWhitelistModeSync(bool whitelist)
		{
			Static.m_requiredResearch.Clear();
			Static.WhitelistMode = whitelist;
		}
	}
}
