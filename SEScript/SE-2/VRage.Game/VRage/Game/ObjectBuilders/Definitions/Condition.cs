using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	[XmlType("Condition")]
	[MyObjectBuilderDefinition(null, null)]
	public class Condition : ConditionBase
	{
		protected class VRage_Game_ObjectBuilders_Definitions_Condition_003C_003ETerms_003C_003EAccessor : IMemberAccessor<Condition, ConditionBase[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Condition owner, in ConditionBase[] value)
			{
				owner.Terms = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Condition owner, out ConditionBase[] value)
			{
				value = owner.Terms;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Condition_003C_003EOperator_003C_003EAccessor : IMemberAccessor<Condition, StatLogicOperator>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Condition owner, in StatLogicOperator value)
			{
				owner.Operator = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Condition owner, out StatLogicOperator value)
			{
				value = owner.Operator;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Condition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<Condition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref Condition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref Condition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Condition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<Condition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref Condition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref Condition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Condition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<Condition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref Condition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref Condition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_Condition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<Condition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref Condition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref Condition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_Condition_003C_003EActor : IActivator, IActivator<Condition>
		{
			private sealed override object CreateInstance()
			{
				return new Condition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override Condition CreateInstance()
			{
				return new Condition();
			}

			Condition IActivator<Condition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("Term", Type = typeof(MyAbstractXmlSerializer<ConditionBase>))]
		public ConditionBase[] Terms;

		public StatLogicOperator Operator;

		public override bool Eval()
		{
			if (Terms != null)
			{
				bool flag = Terms[0].Eval();
				if (Operator == StatLogicOperator.Not)
				{
					return !flag;
				}
				for (int i = 1; i < Terms.Length; i++)
				{
					ConditionBase conditionBase = Terms[i];
					switch (Operator)
					{
					case StatLogicOperator.And:
						flag &= conditionBase.Eval();
						break;
					case StatLogicOperator.Or:
						flag |= conditionBase.Eval();
						break;
					}
				}
				return flag;
			}
			return false;
		}
	}
}
