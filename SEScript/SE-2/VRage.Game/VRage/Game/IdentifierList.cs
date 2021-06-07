using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct IdentifierList
	{
		protected class VRage_Game_IdentifierList_003C_003EOriginName_003C_003EAccessor : IMemberAccessor<IdentifierList, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref IdentifierList owner, in string value)
			{
				owner.OriginName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref IdentifierList owner, out string value)
			{
				value = owner.OriginName;
			}
		}

		protected class VRage_Game_IdentifierList_003C_003EOriginType_003C_003EAccessor : IMemberAccessor<IdentifierList, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref IdentifierList owner, in string value)
			{
				owner.OriginType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref IdentifierList owner, out string value)
			{
				value = owner.OriginType;
			}
		}

		protected class VRage_Game_IdentifierList_003C_003EIds_003C_003EAccessor : IMemberAccessor<IdentifierList, List<MyVariableIdentifier>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref IdentifierList owner, in List<MyVariableIdentifier> value)
			{
				owner.Ids = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref IdentifierList owner, out List<MyVariableIdentifier> value)
			{
				value = owner.Ids;
			}
		}

		private class VRage_Game_IdentifierList_003C_003EActor : IActivator, IActivator<IdentifierList>
		{
			private sealed override object CreateInstance()
			{
				return default(IdentifierList);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override IdentifierList CreateInstance()
			{
				return (IdentifierList)(object)default(IdentifierList);
			}

			IdentifierList IActivator<IdentifierList>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string OriginName;

		[ProtoMember(5)]
		public string OriginType;

		[ProtoMember(10)]
		public List<MyVariableIdentifier> Ids;
	}
}
