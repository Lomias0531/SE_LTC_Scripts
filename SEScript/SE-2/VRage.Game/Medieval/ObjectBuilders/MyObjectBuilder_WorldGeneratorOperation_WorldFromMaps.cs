using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Medieval.ObjectBuilders
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("WorldFromMaps")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps : MyObjectBuilder_WorldGeneratorOperation
	{
		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in SerializableVector3 value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out SerializableVector3 value)
			{
				value = owner.Size;
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003EHeightMapFile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in string value)
			{
				owner.HeightMapFile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out string value)
			{
				value = owner.HeightMapFile;
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003EBiomeMapFile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in string value)
			{
				owner.BiomeMapFile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out string value)
			{
				value = owner.BiomeMapFile;
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003ETreeMapFile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in string value)
			{
				owner.TreeMapFile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out string value)
			{
				value = owner.TreeMapFile;
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003ETreeMaskFile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in string value)
			{
				owner.TreeMaskFile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out string value)
			{
				value = owner.TreeMaskFile;
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorOperation_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps();
			}

			MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps IActivator<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(16)]
		[XmlAttribute]
		public string Name;

		[ProtoMember(19)]
		public SerializableVector3 Size;

		[ProtoMember(22)]
		public string HeightMapFile;

		[ProtoMember(25)]
		public string BiomeMapFile;

		[ProtoMember(28)]
		public string TreeMapFile;

		[ProtoMember(31)]
		public string TreeMaskFile;
	}
}
