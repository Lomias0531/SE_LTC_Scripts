using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	/// <summary>
	/// Conjunction of several simple conditions. This conjunction is true if all contained conditions are true.
	/// </summary>
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	public class MyObjectBuilder_AnimationSMConditionsConjunction : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction_003C_003EConditions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, MyObjectBuilder_AnimationSMCondition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, in MyObjectBuilder_AnimationSMCondition[] value)
			{
				owner.Conditions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, out MyObjectBuilder_AnimationSMCondition[] value)
			{
				value = owner.Conditions;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMConditionsConjunction owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMConditionsConjunction>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMConditionsConjunction();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationSMConditionsConjunction CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMConditionsConjunction();
			}

			MyObjectBuilder_AnimationSMConditionsConjunction IActivator<MyObjectBuilder_AnimationSMConditionsConjunction>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		[XmlElement("Condition")]
		public MyObjectBuilder_AnimationSMCondition[] Conditions;

		/// <summary>
		/// Create deep copy of this conjuction of conditions.
		/// </summary>
		/// <returns></returns>
		public MyObjectBuilder_AnimationSMConditionsConjunction DeepCopy()
		{
			MyObjectBuilder_AnimationSMConditionsConjunction myObjectBuilder_AnimationSMConditionsConjunction = new MyObjectBuilder_AnimationSMConditionsConjunction();
			if (Conditions != null)
			{
				myObjectBuilder_AnimationSMConditionsConjunction.Conditions = new MyObjectBuilder_AnimationSMCondition[Conditions.Length];
				for (int i = 0; i < Conditions.Length; i++)
				{
					myObjectBuilder_AnimationSMConditionsConjunction.Conditions[i] = new MyObjectBuilder_AnimationSMCondition
					{
						Operation = Conditions[i].Operation,
						ValueLeft = Conditions[i].ValueLeft,
						ValueRight = Conditions[i].ValueRight
					};
				}
			}
			else
			{
				myObjectBuilder_AnimationSMConditionsConjunction.Conditions = null;
			}
			return myObjectBuilder_AnimationSMConditionsConjunction;
		}

		public override string ToString()
		{
			if (Conditions == null || Conditions.Length == 0)
			{
				return "[no content, false]";
			}
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder(512);
			stringBuilder.Append("[");
			MyObjectBuilder_AnimationSMCondition[] conditions = Conditions;
			foreach (MyObjectBuilder_AnimationSMCondition myObjectBuilder_AnimationSMCondition in conditions)
			{
				if (!flag)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.Append(myObjectBuilder_AnimationSMCondition.ToString());
				flag = false;
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}
	}
}
