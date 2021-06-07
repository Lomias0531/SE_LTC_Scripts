using ProtoBuf;
using System.ComponentModel;
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
	public class MyObjectBuilder_SpawnGroupDefinition : MyObjectBuilder_DefinitionBase
	{
		[ProtoContract]
		public class SpawnGroupPrefab
		{
			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupPrefab_003C_003ESubtypeId_003C_003EAccessor : IMemberAccessor<SpawnGroupPrefab, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupPrefab owner, in string value)
				{
					owner.SubtypeId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupPrefab owner, out string value)
				{
					value = owner.SubtypeId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupPrefab_003C_003EPosition_003C_003EAccessor : IMemberAccessor<SpawnGroupPrefab, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupPrefab owner, in Vector3 value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupPrefab owner, out Vector3 value)
				{
					value = owner.Position;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupPrefab_003C_003EBeaconText_003C_003EAccessor : IMemberAccessor<SpawnGroupPrefab, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupPrefab owner, in string value)
				{
					owner.BeaconText = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupPrefab owner, out string value)
				{
					value = owner.BeaconText;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupPrefab_003C_003ESpeed_003C_003EAccessor : IMemberAccessor<SpawnGroupPrefab, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupPrefab owner, in float value)
				{
					owner.Speed = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupPrefab owner, out float value)
				{
					value = owner.Speed;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupPrefab_003C_003EPlaceToGridOrigin_003C_003EAccessor : IMemberAccessor<SpawnGroupPrefab, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupPrefab owner, in bool value)
				{
					owner.PlaceToGridOrigin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupPrefab owner, out bool value)
				{
					value = owner.PlaceToGridOrigin;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupPrefab_003C_003EResetOwnership_003C_003EAccessor : IMemberAccessor<SpawnGroupPrefab, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupPrefab owner, in bool value)
				{
					owner.ResetOwnership = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupPrefab owner, out bool value)
				{
					value = owner.ResetOwnership;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupPrefab_003C_003EBehaviour_003C_003EAccessor : IMemberAccessor<SpawnGroupPrefab, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupPrefab owner, in string value)
				{
					owner.Behaviour = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupPrefab owner, out string value)
				{
					value = owner.Behaviour;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupPrefab_003C_003EBehaviourActivationDistance_003C_003EAccessor : IMemberAccessor<SpawnGroupPrefab, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupPrefab owner, in float value)
				{
					owner.BehaviourActivationDistance = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupPrefab owner, out float value)
				{
					value = owner.BehaviourActivationDistance;
				}
			}

			private class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupPrefab_003C_003EActor : IActivator, IActivator<SpawnGroupPrefab>
			{
				private sealed override object CreateInstance()
				{
					return new SpawnGroupPrefab();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override SpawnGroupPrefab CreateInstance()
				{
					return new SpawnGroupPrefab();
				}

				SpawnGroupPrefab IActivator<SpawnGroupPrefab>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(1)]
			public string SubtypeId;

			[ProtoMember(4)]
			public Vector3 Position;

			[ProtoMember(7)]
			[DefaultValue("")]
			public string BeaconText = "";

			[ProtoMember(10)]
			[DefaultValue(10f)]
			public float Speed = 10f;

			[ProtoMember(13)]
			[DefaultValue(false)]
			public bool PlaceToGridOrigin;

			[ProtoMember(16)]
			public bool ResetOwnership = true;

			[ProtoMember(19)]
			public string Behaviour;

			[ProtoMember(22)]
			public float BehaviourActivationDistance = 1000f;
		}

		[ProtoContract]
		public class SpawnGroupVoxel
		{
			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupVoxel_003C_003EStorageName_003C_003EAccessor : IMemberAccessor<SpawnGroupVoxel, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupVoxel owner, in string value)
				{
					owner.StorageName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupVoxel owner, out string value)
				{
					value = owner.StorageName;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupVoxel_003C_003EOffset_003C_003EAccessor : IMemberAccessor<SpawnGroupVoxel, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupVoxel owner, in Vector3 value)
				{
					owner.Offset = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupVoxel owner, out Vector3 value)
				{
					value = owner.Offset;
				}
			}

			protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupVoxel_003C_003ECenterOffset_003C_003EAccessor : IMemberAccessor<SpawnGroupVoxel, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SpawnGroupVoxel owner, in bool value)
				{
					owner.CenterOffset = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SpawnGroupVoxel owner, out bool value)
				{
					value = owner.CenterOffset;
				}
			}

			private class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESpawnGroupVoxel_003C_003EActor : IActivator, IActivator<SpawnGroupVoxel>
			{
				private sealed override object CreateInstance()
				{
					return new SpawnGroupVoxel();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override SpawnGroupVoxel CreateInstance()
				{
					return new SpawnGroupVoxel();
				}

				SpawnGroupVoxel IActivator<SpawnGroupVoxel>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(25)]
			public string StorageName;

			[ProtoMember(28)]
			public Vector3 Offset;

			[ProtoMember(31, IsRequired = false)]
			public bool CenterOffset;
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EFrequency_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in float value)
			{
				owner.Frequency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out float value)
			{
				value = owner.Frequency;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EPrefabs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, SpawnGroupPrefab[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in SpawnGroupPrefab[] value)
			{
				owner.Prefabs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out SpawnGroupPrefab[] value)
			{
				value = owner.Prefabs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EVoxels_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, SpawnGroupVoxel[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in SpawnGroupVoxel[] value)
			{
				owner.Voxels = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out SpawnGroupVoxel[] value)
			{
				value = owner.Voxels;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EIsEncounter_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value)
			{
				owner.IsEncounter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value)
			{
				value = owner.IsEncounter;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EIsPirate_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value)
			{
				owner.IsPirate = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value)
			{
				value = owner.IsPirate;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EIsCargoShip_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value)
			{
				owner.IsCargoShip = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value)
			{
				value = owner.IsCargoShip;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EReactorsOn_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value)
			{
				owner.ReactorsOn = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value)
			{
				value = owner.ReactorsOn;
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_SpawnGroupDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SpawnGroupDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SpawnGroupDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SpawnGroupDefinition CreateInstance()
			{
				return new MyObjectBuilder_SpawnGroupDefinition();
			}

			MyObjectBuilder_SpawnGroupDefinition IActivator<MyObjectBuilder_SpawnGroupDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(34)]
		[DefaultValue(1f)]
		public float Frequency = 1f;

		[ProtoMember(37)]
		[XmlArrayItem("Prefab")]
		public SpawnGroupPrefab[] Prefabs;

		[ProtoMember(40)]
		[XmlArrayItem("Voxel")]
		public SpawnGroupVoxel[] Voxels;

		[ProtoMember(43)]
		[DefaultValue(false)]
		public bool IsEncounter;

		[ProtoMember(46)]
		[DefaultValue(false)]
		public bool IsPirate;

		[ProtoMember(49)]
		[DefaultValue(false)]
		public bool IsCargoShip;

		[ProtoMember(52)]
		[DefaultValue(false)]
		public bool ReactorsOn;
	}
}
