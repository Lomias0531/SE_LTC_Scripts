using ProtoBuf;
using System;
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
	public class MyObjectBuilder_Sector : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_Sector_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Sector owner, in Vector3I value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Sector owner, out Vector3I value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003ESectorEvents_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, MyObjectBuilder_GlobalEvents>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Sector owner, in MyObjectBuilder_GlobalEvents value)
			{
				owner.SectorEvents = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Sector owner, out MyObjectBuilder_GlobalEvents value)
			{
				value = owner.SectorEvents;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003EAppVersion_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Sector owner, in int value)
			{
				owner.AppVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Sector owner, out int value)
			{
				value = owner.AppVersion;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003EEncounters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, MyObjectBuilder_Encounters>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Sector owner, in MyObjectBuilder_Encounters value)
			{
				owner.Encounters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Sector owner, out MyObjectBuilder_Encounters value)
			{
				value = owner.Encounters;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003EEnvironment_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, MyObjectBuilder_EnvironmentSettings>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Sector owner, in MyObjectBuilder_EnvironmentSettings value)
			{
				owner.Environment = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Sector owner, out MyObjectBuilder_EnvironmentSettings value)
			{
				value = owner.Environment;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003EVoxelHandVolumeChanged_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Sector owner, in ulong value)
			{
				owner.VoxelHandVolumeChanged = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Sector owner, out ulong value)
			{
				value = owner.VoxelHandVolumeChanged;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Sector, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Sector owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Sector owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Sector, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Sector owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Sector owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003ESectorObjects_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, List<MyObjectBuilder_EntityBase>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Sector owner, in List<MyObjectBuilder_EntityBase> value)
			{
				owner.SectorObjects = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Sector owner, out List<MyObjectBuilder_EntityBase> value)
			{
				value = owner.SectorObjects;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Sector, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Sector owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Sector owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Sector_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Sector, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Sector owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Sector owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Sector_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Sector>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Sector();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Sector CreateInstance()
			{
				return new MyObjectBuilder_Sector();
			}

			MyObjectBuilder_Sector IActivator<MyObjectBuilder_Sector>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public Vector3I Position;

		[ProtoMember(7)]
		public MyObjectBuilder_GlobalEvents SectorEvents;

		[ProtoMember(10)]
		public int AppVersion;

		[ProtoMember(13)]
		[Obsolete]
		public MyObjectBuilder_Encounters Encounters;

		[ProtoMember(16)]
		public MyObjectBuilder_EnvironmentSettings Environment;

		[ProtoMember(19)]
		public ulong VoxelHandVolumeChanged;

		[ProtoMember(4)]
		[DynamicObjectBuilder(false)]
		[XmlArrayItem("MyObjectBuilder_EntityBase", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_EntityBase>))]
		public List<MyObjectBuilder_EntityBase> SectorObjects
		{
			get;
			set;
		}

		public bool ShouldSerializeEnvironment()
		{
			return Environment != null;
		}
	}
}
