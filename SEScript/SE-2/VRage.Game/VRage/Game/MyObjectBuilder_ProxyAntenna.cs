using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ProxyAntenna : MyObjectBuilder_EntityBase
	{
		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EHasReceiver_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in bool value)
			{
				owner.HasReceiver = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out bool value)
			{
				value = owner.HasReceiver;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EIsLaser_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in bool value)
			{
				owner.IsLaser = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out bool value)
			{
				value = owner.IsLaser;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EIsCharacter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in bool value)
			{
				owner.IsCharacter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out bool value)
			{
				value = owner.IsCharacter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in SerializableVector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out SerializableVector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EBroadcastRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in float value)
			{
				owner.BroadcastRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out float value)
			{
				value = owner.BroadcastRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EHudParams_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, List<MyObjectBuilder_HudEntityParams>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in List<MyObjectBuilder_HudEntityParams> value)
			{
				owner.HudParams = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out List<MyObjectBuilder_HudEntityParams> value)
			{
				value = owner.HudParams;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in long value)
			{
				owner.Owner = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out long value)
			{
				value = owner.Owner;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EShare_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in MyOwnershipShareModeEnum value)
			{
				owner.Share = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out MyOwnershipShareModeEnum value)
			{
				value = owner.Share;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EInfoEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in long value)
			{
				owner.InfoEntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out long value)
			{
				value = owner.InfoEntityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EInfoName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value)
			{
				owner.InfoName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value)
			{
				value = owner.InfoName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EAntennaEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in long value)
			{
				owner.AntennaEntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out long value)
			{
				value = owner.AntennaEntityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003ESuccessfullyContacting_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in long? value)
			{
				owner.SuccessfullyContacting = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out long? value)
			{
				value = owner.SuccessfullyContacting;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EStateText_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value)
			{
				owner.StateText = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value)
			{
				value = owner.StateText;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EHasRemote_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in bool value)
			{
				owner.HasRemote = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out bool value)
			{
				value = owner.HasRemote;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EMainRemoteOwner_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in long? value)
			{
				owner.MainRemoteOwner = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out long? value)
			{
				value = owner.MainRemoteOwner;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EMainRemoteId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in long? value)
			{
				owner.MainRemoteId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out long? value)
			{
				value = owner.MainRemoteId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EMainRemoteSharing_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyOwnershipShareModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in MyOwnershipShareModeEnum value)
			{
				owner.MainRemoteSharing = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out MyOwnershipShareModeEnum value)
			{
				value = owner.MainRemoteSharing;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EEntityId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in long value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out long value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EPersistentFlags_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPersistentFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyPersistentEntityFlags2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in MyPersistentEntityFlags2 value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out MyPersistentEntityFlags2 value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EPositionAndOrientation_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPositionAndOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in MyPositionAndOrientation? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out MyPositionAndOrientation? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EComponentContainer_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EEntityDefinitionId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityDefinitionId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ProxyAntenna_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ProxyAntenna>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ProxyAntenna();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ProxyAntenna CreateInstance()
			{
				return new MyObjectBuilder_ProxyAntenna();
			}

			MyObjectBuilder_ProxyAntenna IActivator<MyObjectBuilder_ProxyAntenna>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool HasReceiver;

		public bool IsLaser;

		public bool IsCharacter;

		public SerializableVector3D Position;

		public float BroadcastRadius;

		public List<MyObjectBuilder_HudEntityParams> HudParams;

		public long Owner;

		public MyOwnershipShareModeEnum Share;

		public long InfoEntityId;

		[Serialize(MyObjectFlags.DefaultZero)]
		public string InfoName;

		public long AntennaEntityId;

		[Serialize(MyObjectFlags.DefaultZero)]
		public long? SuccessfullyContacting;

		[Serialize(MyObjectFlags.DefaultZero)]
		public string StateText;

		public bool HasRemote;

		[Serialize(MyObjectFlags.DefaultZero)]
		public long? MainRemoteOwner;

		[Serialize(MyObjectFlags.DefaultZero)]
		public long? MainRemoteId;

		public MyOwnershipShareModeEnum MainRemoteSharing;
	}
}
