using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyPlanetMaps
	{
		protected class VRage_Game_MyPlanetMaps_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<MyPlanetMaps, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaps owner, in bool value)
			{
				owner.Material = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaps owner, out bool value)
			{
				value = owner.Material;
			}
		}

		protected class VRage_Game_MyPlanetMaps_003C_003EOres_003C_003EAccessor : IMemberAccessor<MyPlanetMaps, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaps owner, in bool value)
			{
				owner.Ores = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaps owner, out bool value)
			{
				value = owner.Ores;
			}
		}

		protected class VRage_Game_MyPlanetMaps_003C_003EBiome_003C_003EAccessor : IMemberAccessor<MyPlanetMaps, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaps owner, in bool value)
			{
				owner.Biome = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaps owner, out bool value)
			{
				value = owner.Biome;
			}
		}

		protected class VRage_Game_MyPlanetMaps_003C_003EOcclusion_003C_003EAccessor : IMemberAccessor<MyPlanetMaps, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaps owner, in bool value)
			{
				owner.Occlusion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaps owner, out bool value)
			{
				value = owner.Occlusion;
			}
		}

		private class VRage_Game_MyPlanetMaps_003C_003EActor : IActivator, IActivator<MyPlanetMaps>
		{
			private sealed override object CreateInstance()
			{
				return default(MyPlanetMaps);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetMaps CreateInstance()
			{
				return (MyPlanetMaps)(object)default(MyPlanetMaps);
			}

			MyPlanetMaps IActivator<MyPlanetMaps>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(43)]
		[XmlAttribute]
		public bool Material;

		[ProtoMember(44)]
		[XmlAttribute]
		public bool Ores;

		[ProtoMember(45)]
		[XmlAttribute]
		public bool Biome;

		[ProtoMember(46)]
		[XmlAttribute]
		public bool Occlusion;

		public MyPlanetMapTypeSet ToSet()
		{
			MyPlanetMapTypeSet myPlanetMapTypeSet = (MyPlanetMapTypeSet)0;
			if (Material)
			{
				myPlanetMapTypeSet |= MyPlanetMapTypeSet.Material;
			}
			if (Ores)
			{
				myPlanetMapTypeSet |= MyPlanetMapTypeSet.Ore;
			}
			if (Biome)
			{
				myPlanetMapTypeSet |= MyPlanetMapTypeSet.Biome;
			}
			return myPlanetMapTypeSet;
		}
	}
}
