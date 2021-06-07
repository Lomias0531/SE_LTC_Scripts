using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Gps : MyObjectBuilder_Base
	{
		[ProtoContract]
		public struct Entry
		{
			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003Ename_003C_003EAccessor : IMemberAccessor<Entry, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in string value)
				{
					owner.name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out string value)
				{
					value = owner.name;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003Edescription_003C_003EAccessor : IMemberAccessor<Entry, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in string value)
				{
					owner.description = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out string value)
				{
					value = owner.description;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003Ecoords_003C_003EAccessor : IMemberAccessor<Entry, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in Vector3D value)
				{
					owner.coords = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out Vector3D value)
				{
					value = owner.coords;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003EisFinal_003C_003EAccessor : IMemberAccessor<Entry, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in bool value)
				{
					owner.isFinal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out bool value)
				{
					value = owner.isFinal;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003EshowOnHud_003C_003EAccessor : IMemberAccessor<Entry, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in bool value)
				{
					owner.showOnHud = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out bool value)
				{
					value = owner.showOnHud;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003EalwaysVisible_003C_003EAccessor : IMemberAccessor<Entry, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in bool value)
				{
					owner.alwaysVisible = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out bool value)
				{
					value = owner.alwaysVisible;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003Ecolor_003C_003EAccessor : IMemberAccessor<Entry, Color>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in Color value)
				{
					owner.color = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out Color value)
				{
					value = owner.color;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003EentityId_003C_003EAccessor : IMemberAccessor<Entry, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in long value)
				{
					owner.entityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out long value)
				{
					value = owner.entityId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003EisObjective_003C_003EAccessor : IMemberAccessor<Entry, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in bool value)
				{
					owner.isObjective = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out bool value)
				{
					value = owner.isObjective;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003EcontractId_003C_003EAccessor : IMemberAccessor<Entry, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in long value)
				{
					owner.contractId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out long value)
				{
					value = owner.contractId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<Entry, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Entry owner, in string value)
				{
					owner.DisplayName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Entry owner, out string value)
				{
					value = owner.DisplayName;
				}
			}

			private class VRage_Game_MyObjectBuilder_Gps_003C_003EEntry_003C_003EActor : IActivator, IActivator<Entry>
			{
				private sealed override object CreateInstance()
				{
					return default(Entry);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Entry CreateInstance()
				{
					return (Entry)(object)default(Entry);
				}

				Entry IActivator<Entry>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public string name;

			[ProtoMember(4)]
			public string description;

			[ProtoMember(7)]
			public Vector3D coords;

			[ProtoMember(10)]
			public bool isFinal;

			[ProtoMember(13)]
			public bool showOnHud;

			[ProtoMember(16)]
			public bool alwaysVisible;

			[ProtoMember(19)]
			public Color color;

			[ProtoMember(22, IsRequired = false)]
			public long entityId;

			[ProtoMember(28)]
			public bool isObjective;

			[ProtoMember(31, IsRequired = false)]
			public long contractId;

			[ProtoMember(25, IsRequired = false)]
			public string DisplayName
			{
				get;
				set;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Gps_003C_003EEntries_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Gps, List<Entry>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Gps owner, in List<Entry> value)
			{
				owner.Entries = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Gps owner, out List<Entry> value)
			{
				value = owner.Entries;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Gps_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Gps, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Gps owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Gps owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Gps_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Gps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Gps owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Gps owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Gps_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Gps, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Gps owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Gps owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Gps_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Gps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Gps owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Gps owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Gps_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Gps>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Gps();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Gps CreateInstance()
			{
				return new MyObjectBuilder_Gps();
			}

			MyObjectBuilder_Gps IActivator<MyObjectBuilder_Gps>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(31)]
		public List<Entry> Entries;
	}
}
