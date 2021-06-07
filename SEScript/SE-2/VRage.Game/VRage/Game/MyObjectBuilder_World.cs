using ProtoBuf;
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
	public class MyObjectBuilder_World : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_World_003C_003ECheckpoint_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_World, MyObjectBuilder_Checkpoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_World owner, in MyObjectBuilder_Checkpoint value)
			{
				owner.Checkpoint = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_World owner, out MyObjectBuilder_Checkpoint value)
			{
				value = owner.Checkpoint;
			}
		}

		protected class VRage_Game_MyObjectBuilder_World_003C_003ESector_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_World, MyObjectBuilder_Sector>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_World owner, in MyObjectBuilder_Sector value)
			{
				owner.Sector = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_World owner, out MyObjectBuilder_Sector value)
			{
				value = owner.Sector;
			}
		}

		protected class VRage_Game_MyObjectBuilder_World_003C_003EVoxelMaps_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_World, SerializableDictionary<string, byte[]>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_World owner, in SerializableDictionary<string, byte[]> value)
			{
				owner.VoxelMaps = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_World owner, out SerializableDictionary<string, byte[]> value)
			{
				value = owner.VoxelMaps;
			}
		}

		protected class VRage_Game_MyObjectBuilder_World_003C_003EClusters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_World, List<BoundingBoxD>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_World owner, in List<BoundingBoxD> value)
			{
				owner.Clusters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_World owner, out List<BoundingBoxD> value)
			{
				value = owner.Clusters;
			}
		}

		protected class VRage_Game_MyObjectBuilder_World_003C_003EPlanets_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_World, List<MyObjectBuilder_Planet>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_World owner, in List<MyObjectBuilder_Planet> value)
			{
				owner.Planets = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_World owner, out List<MyObjectBuilder_Planet> value)
			{
				value = owner.Planets;
			}
		}

		protected class VRage_Game_MyObjectBuilder_World_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_World, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_World owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_World owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_World_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_World, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_World owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_World owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_World_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_World, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_World owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_World owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_World_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_World, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_World owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_World owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_World_003C_003EActor : IActivator, IActivator<MyObjectBuilder_World>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_World();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_World CreateInstance()
			{
				return new MyObjectBuilder_World();
			}

			MyObjectBuilder_World IActivator<MyObjectBuilder_World>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyObjectBuilder_Checkpoint Checkpoint;

		[ProtoMember(4)]
		public MyObjectBuilder_Sector Sector;

		[ProtoMember(7)]
		public SerializableDictionary<string, byte[]> VoxelMaps;

		public List<BoundingBoxD> Clusters;

		public List<MyObjectBuilder_Planet> Planets;
	}
}
