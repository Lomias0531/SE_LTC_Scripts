using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Components
{
	[StaticEventOwner]
	[MyComponentType(typeof(MyEntityStatComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_EntityStatComponent), true)]
	public class MyEntityStatComponent : MyEntityComponentBase
	{
		[Serializable]
		private struct StatInfo
		{
			protected class Sandbox_Game_Components_MyEntityStatComponent_003C_003EStatInfo_003C_003EStatId_003C_003EAccessor : IMemberAccessor<StatInfo, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StatInfo owner, in MyStringHash value)
				{
					owner.StatId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StatInfo owner, out MyStringHash value)
				{
					value = owner.StatId;
				}
			}

			protected class Sandbox_Game_Components_MyEntityStatComponent_003C_003EStatInfo_003C_003EAmount_003C_003EAccessor : IMemberAccessor<StatInfo, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StatInfo owner, in float value)
				{
					owner.Amount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StatInfo owner, out float value)
				{
					value = owner.Amount;
				}
			}

			protected class Sandbox_Game_Components_MyEntityStatComponent_003C_003EStatInfo_003C_003ERegenLeft_003C_003EAccessor : IMemberAccessor<StatInfo, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StatInfo owner, in float value)
				{
					owner.RegenLeft = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StatInfo owner, out float value)
				{
					value = owner.RegenLeft;
				}
			}

			public MyStringHash StatId;

			public float Amount;

			public float RegenLeft;
		}

		protected sealed class OnStatChangedMessage_003C_003ESystem_Int64_0023System_Collections_Generic_List_00601_003CSandbox_Game_Components_MyEntityStatComponent_003C_003EStatInfo_003E : ICallSite<IMyEventOwner, long, List<StatInfo>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in List<StatInfo> changedStats, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnStatChangedMessage(entityId, changedStats);
			}
		}

		protected sealed class OnStatActionRequest_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnStatActionRequest(entityId);
			}
		}

		protected sealed class OnStatActionMessage_003C_003ESystem_Int64_0023System_Collections_Generic_Dictionary_00602_003CSystem_String_0023Sandbox_Game_MyStatLogic_003C_003EMyStatAction_003E : ICallSite<IMyEventOwner, long, Dictionary<string, MyStatLogic.MyStatAction>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Dictionary<string, MyStatLogic.MyStatAction> statActions, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnStatActionMessage(entityId, statActions);
			}
		}

		private class Sandbox_Game_Components_MyEntityStatComponent_003C_003EActor : IActivator, IActivator<MyEntityStatComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityStatComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityStatComponent CreateInstance()
			{
				return new MyEntityStatComponent();
			}

			MyEntityStatComponent IActivator<MyEntityStatComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Dictionary<MyStringHash, MyEntityStat> m_stats;

		protected List<MyStatLogic> m_scripts;

		private static List<MyEntityStat> m_statSyncList = new List<MyEntityStat>();

		private int m_updateCounter;

		private bool m_statActionsRequested;

		public DictionaryValuesReader<MyStringHash, MyEntityStat> Stats => new DictionaryValuesReader<MyStringHash, MyEntityStat>(m_stats);

		public override string ComponentTypeDebugString => "Stats";

		private void SendStatsChanged(List<MyEntityStat> stats)
		{
			List<StatInfo> list = new List<StatInfo>();
			foreach (MyEntityStat stat in stats)
			{
				stat.CalculateRegenLeftForLongestEffect();
				list.Add(new StatInfo
				{
					StatId = stat.StatId,
					Amount = stat.Value,
					RegenLeft = stat.StatRegenLeft
				});
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnStatChangedMessage, base.Entity.EntityId, list);
		}

		[Event(null, 55)]
		[Reliable]
		[Broadcast]
		private static void OnStatChangedMessage(long entityId, List<StatInfo> changedStats)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyEntity entity))
			{
				MyEntityStatComponent component = null;
				if (entity.Components.TryGet(out component))
				{
					foreach (StatInfo changedStat in changedStats)
					{
						if (component.TryGetStat(changedStat.StatId, out MyEntityStat outStat))
						{
							outStat.Value = changedStat.Amount;
							outStat.StatRegenLeft = changedStat.RegenLeft;
						}
					}
				}
			}
		}

		[Event(null, 76)]
		[Reliable]
		[Server]
		private static void OnStatActionRequest(long entityId)
		{
			MyEntity entity = null;
			if (!MyEntities.TryGetEntityById(entityId, out entity))
			{
				return;
			}
			MyEntityStatComponent component = null;
			if (entity.Components.TryGet(out component))
			{
				Dictionary<string, MyStatLogic.MyStatAction> dictionary = new Dictionary<string, MyStatLogic.MyStatAction>();
				foreach (MyStatLogic script in component.m_scripts)
				{
					foreach (KeyValuePair<string, MyStatLogic.MyStatAction> statAction in script.StatActions)
					{
						if (!dictionary.ContainsKey(statAction.Key))
						{
							dictionary.Add(statAction.Key, statAction.Value);
						}
					}
				}
				if (MyEventContext.Current.IsLocallyInvoked)
				{
					OnStatActionMessage(entityId, dictionary);
				}
				else
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnStatActionMessage, entityId, dictionary, MyEventContext.Current.Sender);
				}
			}
		}

		[Event(null, 108)]
		[Reliable]
		[Client]
		private static void OnStatActionMessage(long entityId, Dictionary<string, MyStatLogic.MyStatAction> statActions)
		{
			MyEntity entity = null;
			if (MyEntities.TryGetEntityById(entityId, out entity))
			{
				MyEntityStatComponent component = null;
				if (entity.Components.TryGet(out component))
				{
					MyStatLogic myStatLogic = new MyStatLogic();
					myStatLogic.Init(entity as IMyCharacter, component.m_stats, "LocalStatActionScript");
					foreach (KeyValuePair<string, MyStatLogic.MyStatAction> statAction in statActions)
					{
						myStatLogic.AddAction(statAction.Key, statAction.Value);
					}
					component.m_scripts.Add(myStatLogic);
				}
			}
		}

		public MyEntityStatComponent()
		{
			m_stats = new Dictionary<MyStringHash, MyEntityStat>(MyStringHash.Comparer);
			m_scripts = new List<MyStatLogic>();
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_ComponentBase myObjectBuilder_ComponentBase = base.Serialize();
			MyObjectBuilder_CharacterStatComponent myObjectBuilder_CharacterStatComponent = myObjectBuilder_ComponentBase as MyObjectBuilder_CharacterStatComponent;
			if (myObjectBuilder_CharacterStatComponent == null)
			{
				return myObjectBuilder_ComponentBase;
			}
			myObjectBuilder_CharacterStatComponent.Stats = null;
			myObjectBuilder_CharacterStatComponent.ScriptNames = null;
			if (m_stats != null && m_stats.Count > 0)
			{
				myObjectBuilder_CharacterStatComponent.Stats = new MyObjectBuilder_EntityStat[m_stats.Count];
				int num = 0;
				foreach (KeyValuePair<MyStringHash, MyEntityStat> stat in m_stats)
				{
					myObjectBuilder_CharacterStatComponent.Stats[num++] = stat.Value.GetObjectBuilder();
				}
			}
			if (m_scripts != null && m_scripts.Count > 0)
			{
				myObjectBuilder_CharacterStatComponent.ScriptNames = new string[m_scripts.Count];
				int num2 = 0;
				{
					foreach (MyStatLogic script in m_scripts)
					{
						myObjectBuilder_CharacterStatComponent.ScriptNames[num2++] = script.Name;
					}
					return myObjectBuilder_CharacterStatComponent;
				}
			}
			return myObjectBuilder_CharacterStatComponent;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase objectBuilder)
		{
			MyObjectBuilder_CharacterStatComponent myObjectBuilder_CharacterStatComponent = objectBuilder as MyObjectBuilder_CharacterStatComponent;
			foreach (MyStatLogic script in m_scripts)
			{
				script.Close();
			}
			m_scripts.Clear();
			if (myObjectBuilder_CharacterStatComponent != null)
			{
				if (myObjectBuilder_CharacterStatComponent.Stats != null)
				{
					MyObjectBuilder_EntityStat[] stats = myObjectBuilder_CharacterStatComponent.Stats;
					foreach (MyObjectBuilder_EntityStat myObjectBuilder_EntityStat in stats)
					{
						MyEntityStatDefinition definition = null;
						if (MyDefinitionManager.Static.TryGetDefinition(new MyDefinitionId(myObjectBuilder_EntityStat.TypeId, myObjectBuilder_EntityStat.SubtypeId), out definition) && definition.Enabled && ((definition.EnabledInCreative && MySession.Static.CreativeMode) || (definition.AvailableInSurvival && MySession.Static.SurvivalMode)))
						{
							AddStat(MyStringHash.GetOrCompute(definition.Name), myObjectBuilder_EntityStat, forceNewValues: true);
						}
					}
				}
				if (myObjectBuilder_CharacterStatComponent.ScriptNames != null && Sync.IsServer)
				{
					myObjectBuilder_CharacterStatComponent.ScriptNames = myObjectBuilder_CharacterStatComponent.ScriptNames.Distinct().ToArray();
					string[] scriptNames = myObjectBuilder_CharacterStatComponent.ScriptNames;
					foreach (string scriptName in scriptNames)
					{
						InitScript(scriptName);
					}
				}
			}
			base.Deserialize(objectBuilder);
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			MyEntityStatComponentDefinition myEntityStatComponentDefinition = definition as MyEntityStatComponentDefinition;
			if (myEntityStatComponentDefinition == null || !myEntityStatComponentDefinition.Enabled || MySession.Static == null || (!myEntityStatComponentDefinition.AvailableInSurvival && MySession.Static.SurvivalMode))
			{
				if (Sync.IsServer)
				{
					m_statActionsRequested = true;
				}
				return;
			}
			foreach (MyDefinitionId stat in myEntityStatComponentDefinition.Stats)
			{
				MyEntityStatDefinition definition2 = null;
				if (MyDefinitionManager.Static.TryGetDefinition(stat, out definition2) && definition2.Enabled && (definition2.EnabledInCreative || !MySession.Static.CreativeMode) && (definition2.AvailableInSurvival || !MySession.Static.SurvivalMode))
				{
					MyStringHash orCompute = MyStringHash.GetOrCompute(definition2.Name);
					MyEntityStat value = null;
					if (!m_stats.TryGetValue(orCompute, out value) || !(value.StatDefinition.Id.SubtypeId == definition2.Id.SubtypeId))
					{
						MyObjectBuilder_EntityStat myObjectBuilder_EntityStat = new MyObjectBuilder_EntityStat();
						myObjectBuilder_EntityStat.SubtypeName = stat.SubtypeName;
						myObjectBuilder_EntityStat.MaxValue = 1f;
						myObjectBuilder_EntityStat.Value = definition2.DefaultValue / definition2.MaxValue;
						AddStat(orCompute, myObjectBuilder_EntityStat);
					}
				}
			}
			if (Sync.IsServer)
			{
				foreach (string script in myEntityStatComponentDefinition.Scripts)
				{
					InitScript(script);
				}
				m_statActionsRequested = true;
			}
		}

		public virtual void Update()
		{
			if (base.Container.Entity != null)
			{
				if (!m_statActionsRequested)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnStatActionRequest, base.Entity.EntityId);
					m_statActionsRequested = true;
				}
				foreach (MyStatLogic script in m_scripts)
				{
					script.Update();
				}
				if (m_updateCounter++ % 10 == 0)
				{
					foreach (MyStatLogic script2 in m_scripts)
					{
						script2.Update10();
					}
				}
				foreach (MyEntityStat value in m_stats.Values)
				{
					value.Update();
					if (Sync.IsServer && value.ShouldSync)
					{
						m_statSyncList.Add(value);
					}
				}
				if (m_statSyncList.Count > 0)
				{
					SendStatsChanged(m_statSyncList);
					m_statSyncList.Clear();
				}
			}
		}

		public bool TryGetStat(MyStringHash statId, out MyEntityStat outStat)
		{
			return m_stats.TryGetValue(statId, out outStat);
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			foreach (MyStatLogic script in m_scripts)
			{
				script.Character = (base.Entity as IMyCharacter);
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			foreach (MyStatLogic script in m_scripts)
			{
				script.Close();
			}
			base.OnBeforeRemovedFromContainer();
		}

		public bool CanDoAction(string actionId, out MyTuple<ushort, MyStringHash> message, bool continuous = false)
		{
			message = new MyTuple<ushort, MyStringHash>(0, MyStringHash.NullOrEmpty);
			if (m_scripts == null || m_scripts.Count == 0)
			{
				return true;
			}
			bool flag = true;
			foreach (MyStatLogic script in m_scripts)
			{
				flag &= !script.CanDoAction(actionId, continuous, out MyTuple<ushort, MyStringHash> message2);
				if (message2.Item1 != 0)
				{
					message = message2;
				}
			}
			return !flag;
		}

		public bool DoAction(string actionId)
		{
			bool result = false;
			foreach (MyStatLogic script in m_scripts)
			{
				if (script.DoAction(actionId))
				{
					result = true;
				}
			}
			return result;
		}

		public void ApplyModifier(string modifierId)
		{
			foreach (MyStatLogic script in m_scripts)
			{
				script.ApplyModifier(modifierId);
			}
		}

		public float GetEfficiencyModifier(string modifierId)
		{
			float num = 1f;
			foreach (MyStatLogic script in m_scripts)
			{
				num *= script.GetEfficiencyModifier(modifierId);
			}
			return num;
		}

		private void InitScript(string scriptName)
		{
			Type value;
			if (scriptName == "SpaceStatEffect")
			{
				MySpaceStatEffect mySpaceStatEffect = new MySpaceStatEffect();
				mySpaceStatEffect.Init(base.Entity as IMyCharacter, m_stats, "SpaceStatEffect");
				m_scripts.Add(mySpaceStatEffect);
			}
			else if (MyScriptManager.Static.StatScripts.TryGetValue(scriptName, out value))
			{
				MyStatLogic myStatLogic = (MyStatLogic)Activator.CreateInstance(value);
				if (myStatLogic != null)
				{
					myStatLogic.Init(base.Entity as IMyCharacter, m_stats, scriptName);
					m_scripts.Add(myStatLogic);
				}
			}
		}

		private MyEntityStat AddStat(MyStringHash statId, MyObjectBuilder_EntityStat objectBuilder, bool forceNewValues = false)
		{
			MyEntityStat value = null;
			if (m_stats.TryGetValue(statId, out value))
			{
				if (!forceNewValues)
				{
					objectBuilder.Value = value.CurrentRatio;
				}
				value.ClearEffects();
				m_stats.Remove(statId);
			}
			value = new MyEntityStat();
			value.Init(objectBuilder);
			m_stats.Add(statId, value);
			return value;
		}
	}
}
