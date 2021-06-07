using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Faction
	{
		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EFactionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in long value)
			{
				owner.FactionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out long value)
			{
				value = owner.FactionId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003ETag_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in string value)
			{
				owner.Tag = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out string value)
			{
				value = owner.Tag;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EDescription_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in string value)
			{
				owner.Description = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out string value)
			{
				value = owner.Description;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EPrivateInfo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in string value)
			{
				owner.PrivateInfo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out string value)
			{
				value = owner.PrivateInfo;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EMembers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, List<MyObjectBuilder_FactionMember>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in List<MyObjectBuilder_FactionMember> value)
			{
				owner.Members = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out List<MyObjectBuilder_FactionMember> value)
			{
				value = owner.Members;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EJoinRequests_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, List<MyObjectBuilder_FactionMember>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in List<MyObjectBuilder_FactionMember> value)
			{
				owner.JoinRequests = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out List<MyObjectBuilder_FactionMember> value)
			{
				value = owner.JoinRequests;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EAutoAcceptMember_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in bool value)
			{
				owner.AutoAcceptMember = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out bool value)
			{
				value = owner.AutoAcceptMember;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EAutoAcceptPeace_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in bool value)
			{
				owner.AutoAcceptPeace = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out bool value)
			{
				value = owner.AutoAcceptPeace;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EAcceptHumans_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in bool value)
			{
				owner.AcceptHumans = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out bool value)
			{
				value = owner.AcceptHumans;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EEnableFriendlyFire_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in bool value)
			{
				owner.EnableFriendlyFire = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out bool value)
			{
				value = owner.EnableFriendlyFire;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EFactionType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, MyFactionTypes>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in MyFactionTypes value)
			{
				owner.FactionType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out MyFactionTypes value)
			{
				value = owner.FactionType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EStations_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, List<MyObjectBuilder_Station>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in List<MyObjectBuilder_Station> value)
			{
				owner.Stations = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out List<MyObjectBuilder_Station> value)
			{
				value = owner.Stations;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003ECustomColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in SerializableVector3 value)
			{
				owner.CustomColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out SerializableVector3 value)
			{
				value = owner.CustomColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EIconColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in SerializableVector3 value)
			{
				owner.IconColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out SerializableVector3 value)
			{
				value = owner.IconColor;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003EFactionIcon_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in string value)
			{
				owner.FactionIcon = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out string value)
			{
				value = owner.FactionIcon;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Faction_003C_003ETransferedPCUDelta_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Faction owner, in int value)
			{
				owner.TransferedPCUDelta = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Faction owner, out int value)
			{
				value = owner.TransferedPCUDelta;
			}
		}

		private class VRage_Game_MyObjectBuilder_Faction_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Faction>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Faction();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Faction CreateInstance()
			{
				return new MyObjectBuilder_Faction();
			}

			MyObjectBuilder_Faction IActivator<MyObjectBuilder_Faction>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		public long FactionId;

		[ProtoMember(13)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string Tag;

		[ProtoMember(16)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string Name;

		[ProtoMember(19)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string Description;

		[ProtoMember(22)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string PrivateInfo;

		[ProtoMember(25)]
		public List<MyObjectBuilder_FactionMember> Members;

		[ProtoMember(28)]
		public List<MyObjectBuilder_FactionMember> JoinRequests;

		[ProtoMember(31)]
		public bool AutoAcceptMember;

		[ProtoMember(34)]
		public bool AutoAcceptPeace;

		[ProtoMember(37)]
		public bool AcceptHumans = true;

		[ProtoMember(40)]
		public bool EnableFriendlyFire = true;

		[ProtoMember(43)]
		public MyFactionTypes FactionType;

		[ProtoMember(46)]
		public List<MyObjectBuilder_Station> Stations;

		[ProtoMember(49)]
		public SerializableVector3 CustomColor;

		[ProtoMember(51)]
		public SerializableVector3 IconColor;

		[ProtoMember(52)]
		public string FactionIcon;

		[ProtoMember(53)]
		public int TransferedPCUDelta;
	}
}
