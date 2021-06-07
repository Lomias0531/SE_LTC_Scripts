using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AnimationSMCondition : MyObjectBuilder_Base
	{
		public enum MyOperationType
		{
			AlwaysFalse,
			AlwaysTrue,
			NotEqual,
			Less,
			LessOrEqual,
			Equal,
			GreaterOrEqual,
			Greater
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition_003C_003EValueLeft_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMCondition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMCondition owner, in string value)
			{
				owner.ValueLeft = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMCondition owner, out string value)
			{
				value = owner.ValueLeft;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition_003C_003EOperation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMCondition, MyOperationType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMCondition owner, in MyOperationType value)
			{
				owner.Operation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMCondition owner, out MyOperationType value)
			{
				value = owner.Operation;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition_003C_003EValueRight_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMCondition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationSMCondition owner, in string value)
			{
				owner.ValueRight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationSMCondition owner, out string value)
			{
				value = owner.ValueRight;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMCondition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMCondition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMCondition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMCondition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMCondition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMCondition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMCondition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMCondition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMCondition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMCondition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AnimationSMCondition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AnimationSMCondition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMCondition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMCondition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AnimationSMCondition CreateInstance()
			{
				return new MyObjectBuilder_AnimationSMCondition();
			}

			MyObjectBuilder_AnimationSMCondition IActivator<MyObjectBuilder_AnimationSMCondition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute("Lhs")]
		public string ValueLeft;

		[ProtoMember(4)]
		[XmlAttribute("Op")]
		public MyOperationType Operation;

		[ProtoMember(7)]
		[XmlAttribute("Rhs")]
		public string ValueRight;

		public override string ToString()
		{
			if (Operation == MyOperationType.AlwaysTrue)
			{
				return "true";
			}
			if (Operation == MyOperationType.AlwaysFalse)
			{
				return "false";
			}
			StringBuilder stringBuilder = new StringBuilder(128);
			stringBuilder.Append(ValueLeft);
			stringBuilder.Append(" ");
			switch (Operation)
			{
			case MyOperationType.Less:
				stringBuilder.Append("<");
				break;
			case MyOperationType.LessOrEqual:
				stringBuilder.Append("<=");
				break;
			case MyOperationType.Equal:
				stringBuilder.Append("==");
				break;
			case MyOperationType.GreaterOrEqual:
				stringBuilder.Append(">=");
				break;
			case MyOperationType.Greater:
				stringBuilder.Append(">");
				break;
			case MyOperationType.NotEqual:
				stringBuilder.Append("!=");
				break;
			default:
				stringBuilder.Append("???");
				break;
			}
			stringBuilder.Append(" ");
			stringBuilder.Append(ValueRight);
			return stringBuilder.ToString();
		}
	}
}
