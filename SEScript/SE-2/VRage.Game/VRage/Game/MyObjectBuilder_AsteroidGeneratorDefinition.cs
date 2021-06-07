using ProtoBuf;
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
	public class MyObjectBuilder_AsteroidGeneratorDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EObjectSizeMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in int value)
			{
				owner.ObjectSizeMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out int value)
			{
				value = owner.ObjectSizeMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EObjectSizeMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in int value)
			{
				owner.ObjectSizeMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out int value)
			{
				value = owner.ObjectSizeMax;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003ESubCells_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in int value)
			{
				owner.SubCells = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out int value)
			{
				value = owner.SubCells;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EObjectMaxInCluster_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in int value)
			{
				owner.ObjectMaxInCluster = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out int value)
			{
				value = owner.ObjectMaxInCluster;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EObjectMinDistanceInCluster_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in int value)
			{
				owner.ObjectMinDistanceInCluster = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out int value)
			{
				value = owner.ObjectMinDistanceInCluster;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EObjectMaxDistanceInClusterMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in int value)
			{
				owner.ObjectMaxDistanceInClusterMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out int value)
			{
				value = owner.ObjectMaxDistanceInClusterMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EObjectMaxDistanceInClusterMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in int value)
			{
				owner.ObjectMaxDistanceInClusterMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out int value)
			{
				value = owner.ObjectMaxDistanceInClusterMax;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EObjectSizeMinCluster_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in int value)
			{
				owner.ObjectSizeMinCluster = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out int value)
			{
				value = owner.ObjectSizeMinCluster;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EObjectSizeMaxCluster_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in int value)
			{
				owner.ObjectSizeMaxCluster = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out int value)
			{
				value = owner.ObjectSizeMaxCluster;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EObjectDensityCluster_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in double value)
			{
				owner.ObjectDensityCluster = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out double value)
			{
				value = owner.ObjectDensityCluster;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EClusterDispersionAbsolute_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				owner.ClusterDispersionAbsolute = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				value = owner.ClusterDispersionAbsolute;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EAllowPartialClusterObjectOverlap_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				owner.AllowPartialClusterObjectOverlap = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				value = owner.AllowPartialClusterObjectOverlap;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EUseClusterDefAsAsteroid_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				owner.UseClusterDefAsAsteroid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				value = owner.UseClusterDefAsAsteroid;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003ERotateAsteroids_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				owner.RotateAsteroids = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				value = owner.RotateAsteroids;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EUseLinearPowOfTwoSizeDistribution_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				owner.UseLinearPowOfTwoSizeDistribution = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				value = owner.UseLinearPowOfTwoSizeDistribution;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EUseGeneratorSeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				owner.UseGeneratorSeed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				value = owner.UseGeneratorSeed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EUseClusterVariableSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				owner.UseClusterVariableSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				value = owner.UseClusterVariableSize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003ESeedTypeProbability_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, SerializableDictionary<MyObjectSeedType, double>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in SerializableDictionary<MyObjectSeedType, double> value)
			{
				owner.SeedTypeProbability = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out SerializableDictionary<MyObjectSeedType, double> value)
			{
				value = owner.SeedTypeProbability;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003ESeedClusterTypeProbability_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, SerializableDictionary<MyObjectSeedType, double>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in SerializableDictionary<MyObjectSeedType, double> value)
			{
				owner.SeedClusterTypeProbability = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out SerializableDictionary<MyObjectSeedType, double> value)
			{
				value = owner.SeedClusterTypeProbability;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_AsteroidGeneratorDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AsteroidGeneratorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AsteroidGeneratorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AsteroidGeneratorDefinition CreateInstance()
			{
				return new MyObjectBuilder_AsteroidGeneratorDefinition();
			}

			MyObjectBuilder_AsteroidGeneratorDefinition IActivator<MyObjectBuilder_AsteroidGeneratorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int ObjectSizeMin;

		[ProtoMember(4)]
		public int ObjectSizeMax;

		[ProtoMember(7)]
		public int SubCells;

		[ProtoMember(10)]
		public int ObjectMaxInCluster;

		[ProtoMember(13)]
		public int ObjectMinDistanceInCluster;

		[ProtoMember(16)]
		public int ObjectMaxDistanceInClusterMin;

		[ProtoMember(19)]
		public int ObjectMaxDistanceInClusterMax;

		[ProtoMember(22)]
		public int ObjectSizeMinCluster;

		[ProtoMember(25)]
		public int ObjectSizeMaxCluster;

		[ProtoMember(28)]
		public double ObjectDensityCluster;

		[ProtoMember(31)]
		public bool ClusterDispersionAbsolute;

		[ProtoMember(34)]
		public bool AllowPartialClusterObjectOverlap;

		[ProtoMember(37)]
		public bool UseClusterDefAsAsteroid;

		[ProtoMember(40)]
		public bool RotateAsteroids;

		[ProtoMember(43)]
		public bool UseLinearPowOfTwoSizeDistribution;

		[ProtoMember(46)]
		public bool UseGeneratorSeed;

		[ProtoMember(49)]
		public bool UseClusterVariableSize;

		[ProtoMember(52)]
		public SerializableDictionary<MyObjectSeedType, double> SeedTypeProbability;

		[ProtoMember(55)]
		public SerializableDictionary<MyObjectSeedType, double> SeedClusterTypeProbability;
	}
}
