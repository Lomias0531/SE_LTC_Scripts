using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_GunBase : MyObjectBuilder_DeviceBase
	{
		[ProtoContract]
		public class RemainingAmmoIns
		{
			protected class VRage_Game_MyObjectBuilder_GunBase_003C_003ERemainingAmmoIns_003C_003ESubtypeName_003C_003EAccessor : IMemberAccessor<RemainingAmmoIns, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RemainingAmmoIns owner, in string value)
				{
					owner.SubtypeName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RemainingAmmoIns owner, out string value)
				{
					value = owner.SubtypeName;
				}
			}

			protected class VRage_Game_MyObjectBuilder_GunBase_003C_003ERemainingAmmoIns_003C_003EAmount_003C_003EAccessor : IMemberAccessor<RemainingAmmoIns, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RemainingAmmoIns owner, in int value)
				{
					owner.Amount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RemainingAmmoIns owner, out int value)
				{
					value = owner.Amount;
				}
			}

			private class VRage_Game_MyObjectBuilder_GunBase_003C_003ERemainingAmmoIns_003C_003EActor : IActivator, IActivator<RemainingAmmoIns>
			{
				private sealed override object CreateInstance()
				{
					return new RemainingAmmoIns();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override RemainingAmmoIns CreateInstance()
				{
					return new RemainingAmmoIns();
				}

				RemainingAmmoIns IActivator<RemainingAmmoIns>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			[XmlAttribute]
			[Nullable]
			public string SubtypeName;

			[ProtoMember(4)]
			[XmlAttribute]
			public int Amount;
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003Em_remainingAmmos_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, SerializableDictionary<string, int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GunBase owner, in SerializableDictionary<string, int> value)
			{
				owner.m_remainingAmmos = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GunBase owner, out SerializableDictionary<string, int> value)
			{
				value = owner.m_remainingAmmos;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003ERemainingAmmo_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GunBase owner, in int value)
			{
				owner.RemainingAmmo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GunBase owner, out int value)
			{
				value = owner.RemainingAmmo;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003ECurrentAmmoMagazineName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GunBase owner, in string value)
			{
				owner.CurrentAmmoMagazineName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GunBase owner, out string value)
			{
				value = owner.CurrentAmmoMagazineName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003ERemainingAmmosList_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, List<RemainingAmmoIns>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GunBase owner, in List<RemainingAmmoIns> value)
			{
				owner.RemainingAmmosList = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GunBase owner, out List<RemainingAmmoIns> value)
			{
				value = owner.RemainingAmmosList;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003ELastShootTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GunBase owner, in long value)
			{
				owner.LastShootTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GunBase owner, out long value)
			{
				value = owner.LastShootTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003EInventoryItemId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DeviceBase_003C_003EInventoryItemId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, uint?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GunBase owner, in uint? value)
			{
				Set(ref *(MyObjectBuilder_DeviceBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GunBase owner, out uint? value)
			{
				Get(ref *(MyObjectBuilder_DeviceBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GunBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GunBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GunBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GunBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003ERemainingAmmos_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, SerializableDictionary<string, int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_GunBase owner, in SerializableDictionary<string, int> value)
			{
				owner.RemainingAmmos = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_GunBase owner, out SerializableDictionary<string, int> value)
			{
				value = owner.RemainingAmmos;
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GunBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GunBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_GunBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_GunBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_GunBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_GunBase_003C_003EActor : IActivator, IActivator<MyObjectBuilder_GunBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_GunBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_GunBase CreateInstance()
			{
				return new MyObjectBuilder_GunBase();
			}

			MyObjectBuilder_GunBase IActivator<MyObjectBuilder_GunBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private SerializableDictionary<string, int> m_remainingAmmos;

		[ProtoMember(7)]
		[DefaultValue(0)]
		public int RemainingAmmo;

		[ProtoMember(10)]
		[DefaultValue("")]
		public string CurrentAmmoMagazineName = "";

		[ProtoMember(13)]
		public List<RemainingAmmoIns> RemainingAmmosList = new List<RemainingAmmoIns>();

		[ProtoMember(16)]
		public long LastShootTime;

		[NoSerialize]
		public SerializableDictionary<string, int> RemainingAmmos
		{
			get
			{
				return m_remainingAmmos;
			}
			set
			{
				m_remainingAmmos = value;
				if (RemainingAmmosList == null)
				{
					RemainingAmmosList = new List<RemainingAmmoIns>();
				}
				foreach (KeyValuePair<string, int> item in value.Dictionary)
				{
					RemainingAmmoIns remainingAmmoIns = new RemainingAmmoIns();
					remainingAmmoIns.SubtypeName = item.Key;
					remainingAmmoIns.Amount = item.Value;
					RemainingAmmosList.Add(remainingAmmoIns);
				}
			}
		}

		public bool ShouldSerializeRemainingAmmos()
		{
			return false;
		}
	}
}
