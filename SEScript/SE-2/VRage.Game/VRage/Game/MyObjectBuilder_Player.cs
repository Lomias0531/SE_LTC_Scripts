using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
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
	public class MyObjectBuilder_Player : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_Player_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in string value)
			{
				owner.DisplayName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out string value)
			{
				value = owner.DisplayName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EIdentityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in long value)
			{
				owner.IdentityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out long value)
			{
				value = owner.IdentityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EConnected_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in bool value)
			{
				owner.Connected = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out bool value)
			{
				value = owner.Connected;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EForceRealPlayer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in bool value)
			{
				owner.ForceRealPlayer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out bool value)
			{
				value = owner.ForceRealPlayer;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EToolbar_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, MyObjectBuilder_Toolbar>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in MyObjectBuilder_Toolbar value)
			{
				owner.Toolbar = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out MyObjectBuilder_Toolbar value)
			{
				value = owner.Toolbar;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EEntityCameraData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, List<CameraControllerSettings>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in List<CameraControllerSettings> value)
			{
				owner.EntityCameraData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out List<CameraControllerSettings> value)
			{
				value = owner.EntityCameraData;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EBuildColorSlots_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, List<Vector3>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in List<Vector3> value)
			{
				owner.BuildColorSlots = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out List<Vector3> value)
			{
				value = owner.BuildColorSlots;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003ESteamID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in ulong value)
			{
				owner.SteamID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out ulong value)
			{
				value = owner.SteamID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003Em_cameraData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, SerializableDictionary<long, CameraControllerSettings>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in SerializableDictionary<long, CameraControllerSettings> value)
			{
				owner.m_cameraData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out SerializableDictionary<long, CameraControllerSettings> value)
			{
				value = owner.m_cameraData;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EPlayerEntity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in long value)
			{
				owner.PlayerEntity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out long value)
			{
				value = owner.PlayerEntity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EPlayerModel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in string value)
			{
				owner.PlayerModel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out string value)
			{
				value = owner.PlayerModel;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EPlayerId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in long value)
			{
				owner.PlayerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out long value)
			{
				value = owner.PlayerId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003ELastActivity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in long value)
			{
				owner.LastActivity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out long value)
			{
				value = owner.LastActivity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EBuildArmorSkin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in string value)
			{
				owner.BuildArmorSkin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out string value)
			{
				value = owner.BuildArmorSkin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003EBuildColorSlot_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in int value)
			{
				owner.BuildColorSlot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out int value)
			{
				value = owner.BuildColorSlot;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Player, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Player owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Player owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Player, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Player owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Player owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003ECameraData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Player, SerializableDictionary<long, CameraControllerSettings>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Player owner, in SerializableDictionary<long, CameraControllerSettings> value)
			{
				owner.CameraData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Player owner, out SerializableDictionary<long, CameraControllerSettings> value)
			{
				value = owner.CameraData;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Player, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Player owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Player owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Player_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Player, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Player owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Player owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Player_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Player>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Player();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Player CreateInstance()
			{
				return new MyObjectBuilder_Player();
			}

			MyObjectBuilder_Player IActivator<MyObjectBuilder_Player>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string DisplayName;

		[ProtoMember(13)]
		public long IdentityId;

		[ProtoMember(16)]
		public bool Connected;

		[ProtoMember(19)]
		public bool ForceRealPlayer;

		[ProtoMember(22)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_Toolbar Toolbar;

		[ProtoMember(25)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<CameraControllerSettings> EntityCameraData;

		[ProtoMember(28)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<Vector3> BuildColorSlots;

		[NoSerialize]
		public ulong SteamID;

		[NoSerialize]
		private SerializableDictionary<long, CameraControllerSettings> m_cameraData;

		[NoSerialize]
		public long PlayerEntity;

		[NoSerialize]
		public string PlayerModel;

		[NoSerialize]
		public long PlayerId;

		[NoSerialize]
		public long LastActivity;

		[ProtoMember(31)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string BuildArmorSkin;

		[ProtoMember(35)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public int BuildColorSlot;

		[NoSerialize]
		public SerializableDictionary<long, CameraControllerSettings> CameraData
		{
			get
			{
				return m_cameraData;
			}
			set
			{
				m_cameraData = value;
			}
		}

		public bool ShouldSerializeBuildColorSlots()
		{
			return BuildColorSlots != null;
		}

		public bool ShouldSerializeSteamID()
		{
			return false;
		}

		public bool ShouldSerializeCameraData()
		{
			return false;
		}

		public bool ShouldSerializePlayerEntity()
		{
			return false;
		}

		public bool ShouldSerializePlayerModel()
		{
			return false;
		}

		public bool ShouldSerializePlayerId()
		{
			return false;
		}

		public bool ShouldSerializeLastActivity()
		{
			return false;
		}
	}
}
