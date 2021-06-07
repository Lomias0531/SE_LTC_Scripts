using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Planet : MyObjectBuilder_VoxelMap
	{
		[ProtoContract]
		public struct SavedSector
		{
			protected class VRage_Game_MyObjectBuilder_Planet_003C_003ESavedSector_003C_003EIdPos_003C_003EAccessor : IMemberAccessor<SavedSector, Vector3S>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SavedSector owner, in Vector3S value)
				{
					owner.IdPos = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SavedSector owner, out Vector3S value)
				{
					value = owner.IdPos;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Planet_003C_003ESavedSector_003C_003EIdDir_003C_003EAccessor : IMemberAccessor<SavedSector, Vector3B>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SavedSector owner, in Vector3B value)
				{
					owner.IdDir = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SavedSector owner, out Vector3B value)
				{
					value = owner.IdDir;
				}
			}

			protected class VRage_Game_MyObjectBuilder_Planet_003C_003ESavedSector_003C_003ERemovedItems_003C_003EAccessor : IMemberAccessor<SavedSector, HashSet<int>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SavedSector owner, in HashSet<int> value)
				{
					owner.RemovedItems = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SavedSector owner, out HashSet<int> value)
				{
					value = owner.RemovedItems;
				}
			}

			private class VRage_Game_MyObjectBuilder_Planet_003C_003ESavedSector_003C_003EActor : IActivator, IActivator<SavedSector>
			{
				private sealed override object CreateInstance()
				{
					return default(SavedSector);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override SavedSector CreateInstance()
				{
					return (SavedSector)(object)default(SavedSector);
				}

				SavedSector IActivator<SavedSector>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public Vector3S IdPos;

			[ProtoMember(4)]
			public Vector3B IdDir;

			[ProtoMember(7)]
			[XmlElement("Item")]
			[Nullable]
			public HashSet<int> RemovedItems;
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003ERadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in float value)
			{
				owner.Radius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out float value)
			{
				value = owner.Radius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EHasAtmosphere_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in bool value)
			{
				owner.HasAtmosphere = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out bool value)
			{
				value = owner.HasAtmosphere;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EAtmosphereRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in float value)
			{
				owner.AtmosphereRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out float value)
			{
				value = owner.AtmosphereRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EMinimumSurfaceRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in float value)
			{
				owner.MinimumSurfaceRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out float value)
			{
				value = owner.MinimumSurfaceRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EMaximumHillRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in float value)
			{
				owner.MaximumHillRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out float value)
			{
				value = owner.MaximumHillRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EAtmosphereWavelengths_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in Vector3 value)
			{
				owner.AtmosphereWavelengths = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out Vector3 value)
			{
				value = owner.AtmosphereWavelengths;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003ESavedEnviromentSectors_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, SavedSector[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in SavedSector[] value)
			{
				owner.SavedEnviromentSectors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out SavedSector[] value)
			{
				value = owner.SavedEnviromentSectors;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EGravityFalloff_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in float value)
			{
				owner.GravityFalloff = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out float value)
			{
				value = owner.GravityFalloff;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EMarkAreaEmpty_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in bool value)
			{
				owner.MarkAreaEmpty = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out bool value)
			{
				value = owner.MarkAreaEmpty;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EAtmosphereSettings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, MyAtmosphereSettings?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in MyAtmosphereSettings? value)
			{
				owner.AtmosphereSettings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out MyAtmosphereSettings? value)
			{
				value = owner.AtmosphereSettings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003ESurfaceGravity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in float value)
			{
				owner.SurfaceGravity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out float value)
			{
				value = owner.SurfaceGravity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003ESpawnsFlora_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in bool value)
			{
				owner.SpawnsFlora = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out bool value)
			{
				value = owner.SpawnsFlora;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EShowGPS_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in bool value)
			{
				owner.ShowGPS = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out bool value)
			{
				value = owner.ShowGPS;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003ESpherizeWithDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in bool value)
			{
				owner.SpherizeWithDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out bool value)
			{
				value = owner.SpherizeWithDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EPlanetGenerator_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in string value)
			{
				owner.PlanetGenerator = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out string value)
			{
				value = owner.PlanetGenerator;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003ESeed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Planet owner, in int value)
			{
				owner.Seed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Planet owner, out int value)
			{
				value = owner.Seed;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003Em_storageName_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMap_003C_003Em_storageName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VoxelMap*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VoxelMap*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EMutableStorage_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMap_003C_003EMutableStorage_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_VoxelMap*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_VoxelMap*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EContentChanged_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMap_003C_003EContentChanged_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, bool?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in bool? value)
			{
				Set(ref *(MyObjectBuilder_VoxelMap*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out bool? value)
			{
				Get(ref *(MyObjectBuilder_VoxelMap*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EFilename_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMap_003C_003EFilename_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VoxelMap*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VoxelMap*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EEntityId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in long value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out long value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EPersistentFlags_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPersistentFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyPersistentEntityFlags2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in MyPersistentEntityFlags2 value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out MyPersistentEntityFlags2 value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EPositionAndOrientation_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPositionAndOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in MyPositionAndOrientation? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out MyPositionAndOrientation? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EComponentContainer_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EEntityDefinitionId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityDefinitionId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003EStorageName_003C_003EAccessor : VRage_Game_MyObjectBuilder_VoxelMap_003C_003EStorageName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in string value)
			{
				Set(ref *(MyObjectBuilder_VoxelMap*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out string value)
			{
				Get(ref *(MyObjectBuilder_VoxelMap*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Planet_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Planet owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Planet owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Planet_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Planet>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Planet();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Planet CreateInstance()
			{
				return new MyObjectBuilder_Planet();
			}

			MyObjectBuilder_Planet IActivator<MyObjectBuilder_Planet>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		public float Radius;

		[ProtoMember(13)]
		public bool HasAtmosphere;

		[ProtoMember(16)]
		public float AtmosphereRadius;

		[ProtoMember(19)]
		public float MinimumSurfaceRadius;

		[ProtoMember(22)]
		public float MaximumHillRadius;

		[ProtoMember(25)]
		public Vector3 AtmosphereWavelengths;

		[ProtoMember(28)]
		[XmlArrayItem("Sector")]
		[Nullable]
		public SavedSector[] SavedEnviromentSectors;

		[ProtoMember(31)]
		public float GravityFalloff;

		[ProtoMember(34)]
		public bool MarkAreaEmpty;

		[ProtoMember(37)]
		[Nullable]
		public MyAtmosphereSettings? AtmosphereSettings;

		[ProtoMember(40)]
		public float SurfaceGravity = 1f;

		[ProtoMember(43)]
		public bool SpawnsFlora;

		[ProtoMember(46)]
		public bool ShowGPS;

		[ProtoMember(49)]
		public bool SpherizeWithDistance = true;

		[ProtoMember(52)]
		[Nullable]
		public string PlanetGenerator = "";

		[ProtoMember(55)]
		public int Seed;
	}
}
