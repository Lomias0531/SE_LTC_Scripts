using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	[ProtoContract]
	[XmlType("StatCondition")]
	[MyObjectBuilderDefinition(null, null)]
	public class StatCondition : ConditionBase
	{
		protected class VRage_Game_ObjectBuilders_Definitions_StatCondition_003C_003EOperator_003C_003EAccessor : IMemberAccessor<StatCondition, StatConditionOperator>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref StatCondition owner, in StatConditionOperator value)
			{
				owner.Operator = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref StatCondition owner, out StatConditionOperator value)
			{
				value = owner.Operator;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_StatCondition_003C_003EValue_003C_003EAccessor : IMemberAccessor<StatCondition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref StatCondition owner, in float value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref StatCondition owner, out float value)
			{
				value = owner.Value;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_StatCondition_003C_003EStatId_003C_003EAccessor : IMemberAccessor<StatCondition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref StatCondition owner, in MyStringHash value)
			{
				owner.StatId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref StatCondition owner, out MyStringHash value)
			{
				value = owner.StatId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_StatCondition_003C_003Em_relatedStat_003C_003EAccessor : IMemberAccessor<StatCondition, IMyHudStat>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref StatCondition owner, in IMyHudStat value)
			{
				owner.m_relatedStat = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref StatCondition owner, out IMyHudStat value)
			{
				value = owner.m_relatedStat;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_StatCondition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<StatCondition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref StatCondition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref StatCondition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_StatCondition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<StatCondition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref StatCondition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref StatCondition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_StatCondition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<StatCondition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref StatCondition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref StatCondition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_StatCondition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<StatCondition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref StatCondition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref StatCondition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_StatCondition_003C_003EActor : IActivator, IActivator<StatCondition>
		{
			private sealed override object CreateInstance()
			{
				return new StatCondition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override StatCondition CreateInstance()
			{
				return new StatCondition();
			}

			StatCondition IActivator<StatCondition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public StatConditionOperator Operator;

		public float Value;

		public MyStringHash StatId;

		private IMyHudStat m_relatedStat;

		public void SetStat(IMyHudStat stat)
		{
			m_relatedStat = stat;
		}

		public override bool Eval()
		{
			if (m_relatedStat == null)
			{
				return false;
			}
			switch (Operator)
			{
			case StatConditionOperator.Below:
				return m_relatedStat.CurrentValue < Value * m_relatedStat.MaxValue;
			case StatConditionOperator.Above:
				return m_relatedStat.CurrentValue > Value * m_relatedStat.MaxValue;
			case StatConditionOperator.Equal:
				return m_relatedStat.CurrentValue == Value * m_relatedStat.MaxValue;
			case StatConditionOperator.NotEqual:
				return m_relatedStat.CurrentValue != Value * m_relatedStat.MaxValue;
			default:
				return false;
			}
		}
	}
}
