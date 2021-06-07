using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game
{
	public class MyStatLogic
	{
		[ProtoContract]
		public struct MyStatAction
		{
			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatAction_003C_003EStatId_003C_003EAccessor : IMemberAccessor<MyStatAction, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatAction owner, in MyStringHash value)
				{
					owner.StatId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatAction owner, out MyStringHash value)
				{
					value = owner.StatId;
				}
			}

			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatAction_003C_003ECost_003C_003EAccessor : IMemberAccessor<MyStatAction, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatAction owner, in float value)
				{
					owner.Cost = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatAction owner, out float value)
				{
					value = owner.Cost;
				}
			}

			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatAction_003C_003EAmountToActivate_003C_003EAccessor : IMemberAccessor<MyStatAction, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatAction owner, in float value)
				{
					owner.AmountToActivate = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatAction owner, out float value)
				{
					value = owner.AmountToActivate;
				}
			}

			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatAction_003C_003ECanPerformWithout_003C_003EAccessor : IMemberAccessor<MyStatAction, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatAction owner, in bool value)
				{
					owner.CanPerformWithout = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatAction owner, out bool value)
				{
					value = owner.CanPerformWithout;
				}
			}

			private class Sandbox_Game_MyStatLogic_003C_003EMyStatAction_003C_003EActor : IActivator, IActivator<MyStatAction>
			{
				private sealed override object CreateInstance()
				{
					return default(MyStatAction);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyStatAction CreateInstance()
				{
					return (MyStatAction)(object)default(MyStatAction);
				}

				MyStatAction IActivator<MyStatAction>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public MyStringHash StatId;

			[ProtoMember(4)]
			public float Cost;

			[ProtoMember(7)]
			public float AmountToActivate;

			[ProtoMember(10)]
			public bool CanPerformWithout;
		}

		[ProtoContract]
		public struct MyStatRegenModifier
		{
			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatRegenModifier_003C_003EStatId_003C_003EAccessor : IMemberAccessor<MyStatRegenModifier, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatRegenModifier owner, in MyStringHash value)
				{
					owner.StatId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatRegenModifier owner, out MyStringHash value)
				{
					value = owner.StatId;
				}
			}

			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatRegenModifier_003C_003EAmountMultiplier_003C_003EAccessor : IMemberAccessor<MyStatRegenModifier, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatRegenModifier owner, in float value)
				{
					owner.AmountMultiplier = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatRegenModifier owner, out float value)
				{
					value = owner.AmountMultiplier;
				}
			}

			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatRegenModifier_003C_003EDuration_003C_003EAccessor : IMemberAccessor<MyStatRegenModifier, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatRegenModifier owner, in float value)
				{
					owner.Duration = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatRegenModifier owner, out float value)
				{
					value = owner.Duration;
				}
			}

			private class Sandbox_Game_MyStatLogic_003C_003EMyStatRegenModifier_003C_003EActor : IActivator, IActivator<MyStatRegenModifier>
			{
				private sealed override object CreateInstance()
				{
					return default(MyStatRegenModifier);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyStatRegenModifier CreateInstance()
				{
					return (MyStatRegenModifier)(object)default(MyStatRegenModifier);
				}

				MyStatRegenModifier IActivator<MyStatRegenModifier>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(13)]
			public MyStringHash StatId;

			[ProtoMember(16)]
			public float AmountMultiplier;

			[ProtoMember(19)]
			public float Duration;
		}

		[ProtoContract]
		public struct MyStatEfficiencyModifier
		{
			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatEfficiencyModifier_003C_003EStatId_003C_003EAccessor : IMemberAccessor<MyStatEfficiencyModifier, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatEfficiencyModifier owner, in MyStringHash value)
				{
					owner.StatId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatEfficiencyModifier owner, out MyStringHash value)
				{
					value = owner.StatId;
				}
			}

			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatEfficiencyModifier_003C_003EThreshold_003C_003EAccessor : IMemberAccessor<MyStatEfficiencyModifier, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatEfficiencyModifier owner, in float value)
				{
					owner.Threshold = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatEfficiencyModifier owner, out float value)
				{
					value = owner.Threshold;
				}
			}

			protected class Sandbox_Game_MyStatLogic_003C_003EMyStatEfficiencyModifier_003C_003EEfficiencyMultiplier_003C_003EAccessor : IMemberAccessor<MyStatEfficiencyModifier, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyStatEfficiencyModifier owner, in float value)
				{
					owner.EfficiencyMultiplier = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyStatEfficiencyModifier owner, out float value)
				{
					value = owner.EfficiencyMultiplier;
				}
			}

			private class Sandbox_Game_MyStatLogic_003C_003EMyStatEfficiencyModifier_003C_003EActor : IActivator, IActivator<MyStatEfficiencyModifier>
			{
				private sealed override object CreateInstance()
				{
					return default(MyStatEfficiencyModifier);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyStatEfficiencyModifier CreateInstance()
				{
					return (MyStatEfficiencyModifier)(object)default(MyStatEfficiencyModifier);
				}

				MyStatEfficiencyModifier IActivator<MyStatEfficiencyModifier>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(22)]
			public MyStringHash StatId;

			[ProtoMember(25)]
			public float Threshold;

			[ProtoMember(28)]
			public float EfficiencyMultiplier;
		}

		private string m_scriptName;

		private IMyCharacter m_character;

		protected Dictionary<MyStringHash, MyEntityStat> m_stats;

		private bool m_enableAutoHealing = true;

		private Dictionary<string, MyStatAction> m_statActions = new Dictionary<string, MyStatAction>();

		private Dictionary<string, MyStatRegenModifier> m_statRegenModifiers = new Dictionary<string, MyStatRegenModifier>();

		private Dictionary<string, MyStatEfficiencyModifier> m_statEfficiencyModifiers = new Dictionary<string, MyStatEfficiencyModifier>();

		public const int STAT_VALUE_TOO_LOW = 4;

		public string Name => m_scriptName;

		public IMyCharacter Character
		{
			get
			{
				return m_character;
			}
			set
			{
				IMyCharacter character = m_character;
				m_character = value;
				OnCharacterChanged(character);
			}
		}

		protected bool EnableAutoHealing => m_enableAutoHealing;

		public Dictionary<string, MyStatAction> StatActions => m_statActions;

		public Dictionary<string, MyStatRegenModifier> StatRegenModifiers => m_statRegenModifiers;

		public Dictionary<string, MyStatEfficiencyModifier> StatEfficiencyModifiers => m_statEfficiencyModifiers;

		public virtual void Init(IMyCharacter character, Dictionary<MyStringHash, MyEntityStat> stats, string scriptName)
		{
			m_scriptName = scriptName;
			Character = character;
			m_stats = stats;
			InitSettings();
		}

		private void InitSettings()
		{
			m_enableAutoHealing = MySession.Static.Settings.AutoHealing;
		}

		public virtual void Update()
		{
		}

		public virtual void Update10()
		{
		}

		public virtual void Close()
		{
		}

		protected virtual void OnCharacterChanged(IMyCharacter oldCharacter)
		{
		}

		public void AddAction(string actionId, MyStatAction action)
		{
			m_statActions.Add(actionId, action);
		}

		public void AddModifier(string modifierId, MyStatRegenModifier modifier)
		{
			m_statRegenModifiers.Add(modifierId, modifier);
		}

		public void AddEfficiency(string modifierId, MyStatEfficiencyModifier modifier)
		{
			m_statEfficiencyModifiers.Add(modifierId, modifier);
		}

		public bool CanDoAction(string actionId, bool continuous, out MyTuple<ushort, MyStringHash> message)
		{
			if (!m_statActions.TryGetValue(actionId, out MyStatAction value))
			{
				message = new MyTuple<ushort, MyStringHash>(0, value.StatId);
				return true;
			}
			if (value.CanPerformWithout)
			{
				message = new MyTuple<ushort, MyStringHash>(0, value.StatId);
				return true;
			}
			if (!m_stats.TryGetValue(value.StatId, out MyEntityStat value2))
			{
				message = new MyTuple<ushort, MyStringHash>(0, value.StatId);
				return true;
			}
			if (continuous)
			{
				if (value2.Value < value.Cost)
				{
					message = new MyTuple<ushort, MyStringHash>(4, value.StatId);
					return false;
				}
			}
			else if (value2.Value < value.Cost || value2.Value < value.AmountToActivate)
			{
				message = new MyTuple<ushort, MyStringHash>(4, value.StatId);
				return false;
			}
			message = new MyTuple<ushort, MyStringHash>(0, value.StatId);
			return true;
		}

		public bool DoAction(string actionId)
		{
			if (!m_statActions.TryGetValue(actionId, out MyStatAction value))
			{
				return false;
			}
			if (!m_stats.TryGetValue(value.StatId, out MyEntityStat value2))
			{
				return false;
			}
			if (value.CanPerformWithout)
			{
				value2.Value -= Math.Min(value2.Value, value.Cost);
				return true;
			}
			if (((value.Cost >= 0f && value2.Value >= value.Cost) || value.Cost < 0f) && value2.Value >= value.AmountToActivate)
			{
				value2.Value -= value.Cost;
			}
			return true;
		}

		public void ApplyModifier(string modifierId)
		{
			if (m_statRegenModifiers.TryGetValue(modifierId, out MyStatRegenModifier value) && m_stats.TryGetValue(value.StatId, out MyEntityStat value2))
			{
				value2.ApplyRegenAmountMultiplier(value.AmountMultiplier, value.Duration);
			}
		}

		public float GetEfficiencyModifier(string modifierId)
		{
			if (!m_statEfficiencyModifiers.TryGetValue(modifierId, out MyStatEfficiencyModifier value))
			{
				return 1f;
			}
			if (!m_stats.TryGetValue(value.StatId, out MyEntityStat value2))
			{
				return 1f;
			}
			return value2.GetEfficiencyMultiplier(value.EfficiencyMultiplier, value.Threshold);
		}
	}
}
