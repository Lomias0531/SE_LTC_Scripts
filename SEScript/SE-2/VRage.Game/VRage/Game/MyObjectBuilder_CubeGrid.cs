using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
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
	public class MyObjectBuilder_CubeGrid : MyObjectBuilder_EntityBase
	{
		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EGridSizeEnum_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, MyCubeSize>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in MyCubeSize value)
			{
				owner.GridSizeEnum = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out MyCubeSize value)
			{
				value = owner.GridSizeEnum;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ECubeBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, List<MyObjectBuilder_CubeBlock>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in List<MyObjectBuilder_CubeBlock> value)
			{
				owner.CubeBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out List<MyObjectBuilder_CubeBlock> value)
			{
				value = owner.CubeBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EIsStatic_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.IsStatic = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.IsStatic;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EIsUnsupportedStation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.IsUnsupportedStation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.IsUnsupportedStation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ESkeleton_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, List<BoneInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in List<BoneInfo> value)
			{
				owner.Skeleton = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out List<BoneInfo> value)
			{
				value = owner.Skeleton;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ELinearVelocity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in SerializableVector3 value)
			{
				owner.LinearVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out SerializableVector3 value)
			{
				value = owner.LinearVelocity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EAngularVelocity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in SerializableVector3 value)
			{
				owner.AngularVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out SerializableVector3 value)
			{
				value = owner.AngularVelocity;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EXMirroxPlane_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, SerializableVector3I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in SerializableVector3I? value)
			{
				owner.XMirroxPlane = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out SerializableVector3I? value)
			{
				value = owner.XMirroxPlane;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EYMirroxPlane_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, SerializableVector3I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in SerializableVector3I? value)
			{
				owner.YMirroxPlane = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out SerializableVector3I? value)
			{
				value = owner.YMirroxPlane;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EZMirroxPlane_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, SerializableVector3I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in SerializableVector3I? value)
			{
				owner.ZMirroxPlane = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out SerializableVector3I? value)
			{
				value = owner.ZMirroxPlane;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EXMirroxOdd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.XMirroxOdd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.XMirroxOdd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EYMirroxOdd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.YMirroxOdd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.YMirroxOdd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EZMirroxOdd_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.ZMirroxOdd = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.ZMirroxOdd;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EDampenersEnabled_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.DampenersEnabled = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.DampenersEnabled;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EUsePositionForSpawn_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.UsePositionForSpawn = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.UsePositionForSpawn;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EPlanetSpawnHeightRatio_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in float value)
			{
				owner.PlanetSpawnHeightRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out float value)
			{
				value = owner.PlanetSpawnHeightRatio;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ESpawnRangeMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in float value)
			{
				owner.SpawnRangeMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out float value)
			{
				value = owner.SpawnRangeMin;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ESpawnRangeMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in float value)
			{
				owner.SpawnRangeMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out float value)
			{
				value = owner.SpawnRangeMax;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EConveyorLines_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, List<MyObjectBuilder_ConveyorLine>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in List<MyObjectBuilder_ConveyorLine> value)
			{
				owner.ConveyorLines = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out List<MyObjectBuilder_ConveyorLine> value)
			{
				value = owner.ConveyorLines;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EBlockGroups_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, List<MyObjectBuilder_BlockGroup>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in List<MyObjectBuilder_BlockGroup> value)
			{
				owner.BlockGroups = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out List<MyObjectBuilder_BlockGroup> value)
			{
				value = owner.BlockGroups;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EHandbrake_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.Handbrake = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.Handbrake;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in string value)
			{
				owner.DisplayName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out string value)
			{
				value = owner.DisplayName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EOxygenAmount_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, float[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in float[] value)
			{
				owner.OxygenAmount = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out float[] value)
			{
				value = owner.OxygenAmount;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EDestructibleBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.DestructibleBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.DestructibleBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EJumpDriveDirection_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, Vector3D?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in Vector3D? value)
			{
				owner.JumpDriveDirection = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out Vector3D? value)
			{
				value = owner.JumpDriveDirection;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EJumpRemainingTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in float? value)
			{
				owner.JumpRemainingTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out float? value)
			{
				value = owner.JumpRemainingTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ECreatePhysics_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.CreatePhysics = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.CreatePhysics;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EEnableSmallToLargeConnections_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.EnableSmallToLargeConnections = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.EnableSmallToLargeConnections;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EIsRespawnGrid_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.IsRespawnGrid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.IsRespawnGrid;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EplayedTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in int value)
			{
				owner.playedTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out int value)
			{
				value = owner.playedTime;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EGridGeneralDamageModifier_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in float value)
			{
				owner.GridGeneralDamageModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out float value)
			{
				value = owner.GridGeneralDamageModifier;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ELocalCoordSys_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in long value)
			{
				owner.LocalCoordSys = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out long value)
			{
				value = owner.LocalCoordSys;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EEditable_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.Editable = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.Editable;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ETargetingTargets_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in List<long> value)
			{
				owner.TargetingTargets = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out List<long> value)
			{
				value = owner.TargetingTargets;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ETargetingWhitelist_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.TargetingWhitelist = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.TargetingWhitelist;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EIsPowered_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in bool value)
			{
				owner.IsPowered = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out bool value)
			{
				value = owner.IsPowered;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EOxygenRooms_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CubeGrid, OxygenRoom[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in OxygenRoom[] value)
			{
				owner.OxygenRooms = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out OxygenRoom[] value)
			{
				value = owner.OxygenRooms;
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EEntityId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in long value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out long value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EPersistentFlags_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPersistentFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, MyPersistentEntityFlags2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in MyPersistentEntityFlags2 value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out MyPersistentEntityFlags2 value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EPositionAndOrientation_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPositionAndOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in MyPositionAndOrientation? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out MyPositionAndOrientation? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EComponentContainer_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EEntityDefinitionId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityDefinitionId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_CubeGrid_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CubeGrid, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_CubeGrid owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_CubeGrid owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_CubeGrid_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CubeGrid>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CubeGrid();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CubeGrid CreateInstance()
			{
				return new MyObjectBuilder_CubeGrid();
			}

			MyObjectBuilder_CubeGrid IActivator<MyObjectBuilder_CubeGrid>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyCubeSize GridSizeEnum;

		[ProtoMember(4)]
		[DynamicItem(typeof(MyObjectBuilderDynamicSerializer), true)]
		[XmlArrayItem("MyObjectBuilder_CubeBlock", Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_CubeBlock>))]
		public List<MyObjectBuilder_CubeBlock> CubeBlocks = new List<MyObjectBuilder_CubeBlock>();

		[ProtoMember(7)]
		[DefaultValue(false)]
		public bool IsStatic;

		[ProtoMember(10)]
		[DefaultValue(false)]
		public bool IsUnsupportedStation;

		[ProtoMember(13)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<BoneInfo> Skeleton;

		[ProtoMember(16)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableVector3 LinearVelocity;

		[ProtoMember(19)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableVector3 AngularVelocity;

		[ProtoMember(22)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableVector3I? XMirroxPlane;

		[ProtoMember(25)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableVector3I? YMirroxPlane;

		[ProtoMember(28)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public SerializableVector3I? ZMirroxPlane;

		[ProtoMember(31)]
		[DefaultValue(false)]
		public bool XMirroxOdd;

		[ProtoMember(34)]
		[DefaultValue(false)]
		public bool YMirroxOdd;

		[ProtoMember(37)]
		[DefaultValue(false)]
		public bool ZMirroxOdd;

		[ProtoMember(40)]
		[DefaultValue(true)]
		public bool DampenersEnabled = true;

		[ProtoMember(43)]
		[DefaultValue(false)]
		[Obsolete]
		public bool UsePositionForSpawn;

		[ProtoMember(46)]
		[DefaultValue(0.3f)]
		[Obsolete]
		public float PlanetSpawnHeightRatio = 0.3f;

		[ProtoMember(49)]
		[DefaultValue(500f)]
		[Obsolete]
		public float SpawnRangeMin = 500f;

		[ProtoMember(52)]
		[DefaultValue(650f)]
		[Obsolete]
		public float SpawnRangeMax = 650f;

		[ProtoMember(55)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<MyObjectBuilder_ConveyorLine> ConveyorLines = new List<MyObjectBuilder_ConveyorLine>();

		[ProtoMember(58)]
		public List<MyObjectBuilder_BlockGroup> BlockGroups = new List<MyObjectBuilder_BlockGroup>();

		[ProtoMember(61)]
		[DefaultValue(false)]
		public bool Handbrake;

		[ProtoMember(64)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string DisplayName;

		[ProtoMember(67)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public float[] OxygenAmount;

		[ProtoMember(70)]
		public bool DestructibleBlocks = true;

		[ProtoMember(73)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public Vector3D? JumpDriveDirection;

		[ProtoMember(76)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public float? JumpRemainingTime;

		[ProtoMember(79)]
		[DefaultValue(true)]
		public bool CreatePhysics = true;

		[ProtoMember(82)]
		[DefaultValue(true)]
		public bool EnableSmallToLargeConnections = true;

		[ProtoMember(85)]
		public bool IsRespawnGrid;

		[ProtoMember(88)]
		[DefaultValue(-1)]
		public int playedTime = -1;

		[ProtoMember(91)]
		[DefaultValue(1f)]
		public float GridGeneralDamageModifier = 1f;

		[ProtoMember(94)]
		public long LocalCoordSys;

		[ProtoMember(97)]
		[DefaultValue(true)]
		public bool Editable = true;

		[ProtoMember(100)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<long> TargetingTargets;

		[ProtoMember(103)]
		[DefaultValue(false)]
		public bool TargetingWhitelist;

		[ProtoMember(106, IsRequired = false)]
		[DefaultValue(true)]
		public bool IsPowered = true;

		[ProtoMember(109, IsRequired = false)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public OxygenRoom[] OxygenRooms;

		public bool ShouldSerializeSkeleton()
		{
			if (Skeleton != null)
			{
				return Skeleton.Count != 0;
			}
			return false;
		}

		public bool ShouldSerializeLinearVelocity()
		{
			return LinearVelocity != new SerializableVector3(0f, 0f, 0f);
		}

		public bool ShouldSerializeAngularVelocity()
		{
			return AngularVelocity != new SerializableVector3(0f, 0f, 0f);
		}

		public bool ShouldSerializeXMirroxPlane()
		{
			return XMirroxPlane.HasValue;
		}

		public bool ShouldSerializeYMirroxPlane()
		{
			return YMirroxPlane.HasValue;
		}

		public bool ShouldSerializeZMirroxPlane()
		{
			return ZMirroxPlane.HasValue;
		}

		public bool ShouldSerializeConveyorLines()
		{
			if (ConveyorLines != null)
			{
				return ConveyorLines.Count != 0;
			}
			return false;
		}

		public bool ShouldSerializeBlockGroups()
		{
			if (BlockGroups != null)
			{
				return BlockGroups.Count != 0;
			}
			return false;
		}

		public override void Remap(IMyRemapHelper remapHelper)
		{
			base.Remap(remapHelper);
			foreach (MyObjectBuilder_CubeBlock cubeBlock in CubeBlocks)
			{
				cubeBlock.Remap(remapHelper);
			}
		}

		public bool ShouldSerializeJumpDriveDirection()
		{
			return JumpDriveDirection.HasValue;
		}

		public bool ShouldSerializeJumpRemainingTime()
		{
			return JumpRemainingTime.HasValue;
		}

		public MyObjectBuilder_CubeGrid()
		{
			TargetingTargets = new List<long>();
		}
	}
}
