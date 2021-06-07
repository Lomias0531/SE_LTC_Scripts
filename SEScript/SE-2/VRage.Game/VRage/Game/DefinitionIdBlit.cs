using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	public struct DefinitionIdBlit
	{
		protected class VRage_Game_DefinitionIdBlit_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<DefinitionIdBlit, MyRuntimeObjectBuilderId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref DefinitionIdBlit owner, in MyRuntimeObjectBuilderId value)
			{
				owner.TypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref DefinitionIdBlit owner, out MyRuntimeObjectBuilderId value)
			{
				value = owner.TypeId;
			}
		}

		protected class VRage_Game_DefinitionIdBlit_003C_003ESubtypeId_003C_003EAccessor : IMemberAccessor<DefinitionIdBlit, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref DefinitionIdBlit owner, in MyStringHash value)
			{
				owner.SubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref DefinitionIdBlit owner, out MyStringHash value)
			{
				value = owner.SubtypeId;
			}
		}

		protected class VRage_Game_DefinitionIdBlit_003C_003Em_typeIdSerialized_003C_003EAccessor : IMemberAccessor<DefinitionIdBlit, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref DefinitionIdBlit owner, in ushort value)
			{
				owner.m_typeIdSerialized = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref DefinitionIdBlit owner, out ushort value)
			{
				value = owner.m_typeIdSerialized;
			}
		}

		private class VRage_Game_DefinitionIdBlit_003C_003EActor : IActivator, IActivator<DefinitionIdBlit>
		{
			private sealed override object CreateInstance()
			{
				return default(DefinitionIdBlit);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override DefinitionIdBlit CreateInstance()
			{
				return (DefinitionIdBlit)(object)default(DefinitionIdBlit);
			}

			DefinitionIdBlit IActivator<DefinitionIdBlit>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[NoSerialize]
		public MyRuntimeObjectBuilderId TypeId;

		[ProtoMember(4)]
		public MyStringHash SubtypeId;

		[Serialize]
		private ushort m_typeIdSerialized
		{
			get
			{
				return TypeId.Value;
			}
			set
			{
				TypeId = new MyRuntimeObjectBuilderId(value);
			}
		}

		public bool IsValid => TypeId.Value != 0;

		public DefinitionIdBlit(MyObjectBuilderType type, MyStringHash subtypeId)
		{
			TypeId = (MyRuntimeObjectBuilderId)type;
			SubtypeId = subtypeId;
		}

		public DefinitionIdBlit(MyRuntimeObjectBuilderId typeId, MyStringHash subtypeId)
		{
			TypeId = typeId;
			SubtypeId = subtypeId;
		}

		public static implicit operator MyDefinitionId(DefinitionIdBlit id)
		{
			return new MyDefinitionId((MyObjectBuilderType)id.TypeId, id.SubtypeId);
		}

		public static implicit operator DefinitionIdBlit(MyDefinitionId id)
		{
			return new DefinitionIdBlit(id.TypeId, id.SubtypeId);
		}

		public override string ToString()
		{
			return ((MyDefinitionId)this).ToString();
		}
	}
}
