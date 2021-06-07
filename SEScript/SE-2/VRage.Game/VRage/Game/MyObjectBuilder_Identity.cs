using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Identity : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_Identity_003C_003EIdentityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in long value)
			{
				owner.IdentityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out long value)
			{
				value = owner.IdentityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in string value)
			{
				owner.DisplayName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out string value)
			{
				value = owner.DisplayName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003ECharacterEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in long value)
			{
				owner.CharacterEntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out long value)
			{
				value = owner.CharacterEntityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003EModel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in string value)
			{
				owner.Model = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out string value)
			{
				value = owner.Model;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003EColorMask_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, SerializableVector3?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in SerializableVector3? value)
			{
				owner.ColorMask = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out SerializableVector3? value)
			{
				value = owner.ColorMask;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003EBlockLimitModifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in int value)
			{
				owner.BlockLimitModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out int value)
			{
				value = owner.BlockLimitModifier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003ELastLoginTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, DateTime>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in DateTime value)
			{
				owner.LastLoginTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out DateTime value)
			{
				value = owner.LastLoginTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003ESavedCharacters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, HashSet<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in HashSet<long> value)
			{
				owner.SavedCharacters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out HashSet<long> value)
			{
				value = owner.SavedCharacters;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003ELastLogoutTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, DateTime>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in DateTime value)
			{
				owner.LastLogoutTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out DateTime value)
			{
				value = owner.LastLogoutTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003ERespawnShips_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in List<long> value)
			{
				owner.RespawnShips = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out List<long> value)
			{
				value = owner.RespawnShips;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003ELastDeathPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, Vector3D?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in Vector3D? value)
			{
				owner.LastDeathPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out Vector3D? value)
			{
				value = owner.LastDeathPosition;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003EActiveContracts_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in List<long> value)
			{
				owner.ActiveContracts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out List<long> value)
			{
				value = owner.ActiveContracts;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003ETransferedPCUDelta_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in int value)
			{
				owner.TransferedPCUDelta = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out int value)
			{
				value = owner.TransferedPCUDelta;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Identity, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Identity owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Identity owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Identity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Identity owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Identity owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003EPlayerId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Identity owner, in long value)
			{
				owner.PlayerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Identity owner, out long value)
			{
				value = owner.PlayerId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Identity, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Identity owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Identity owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Identity_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Identity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Identity owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Identity owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Identity_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Identity>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Identity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Identity CreateInstance()
			{
				return new MyObjectBuilder_Identity();
			}

			MyObjectBuilder_Identity IActivator<MyObjectBuilder_Identity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long IdentityId;

		[ProtoMember(4)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string DisplayName;

		[ProtoMember(7)]
		public long CharacterEntityId;

		[ProtoMember(10)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string Model;

		[ProtoMember(13)]
		public SerializableVector3? ColorMask;

		[ProtoMember(16)]
		public int BlockLimitModifier;

		[ProtoMember(19)]
		public DateTime LastLoginTime;

		[ProtoMember(22, IsRequired = false)]
		public HashSet<long> SavedCharacters;

		[ProtoMember(25)]
		public DateTime LastLogoutTime;

		[ProtoMember(28)]
		public List<long> RespawnShips;

		[ProtoMember(31, IsRequired = false)]
		public Vector3D? LastDeathPosition;

		[ProtoMember(33)]
		public List<long> ActiveContracts;

		[ProtoMember(35)]
		public int TransferedPCUDelta;

		[NoSerialize]
		public long PlayerId
		{
			get
			{
				return IdentityId;
			}
			set
			{
				IdentityId = value;
			}
		}

		public bool ShouldSerializePlayerId()
		{
			return false;
		}

		public bool ShouldSerializeColorMask()
		{
			return ColorMask.HasValue;
		}
	}
}
