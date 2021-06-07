using Havok;
using ParallelTasks;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.GameServices;
using VRage.Groups;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Sync;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageMath.PackedVector;
using VRageMath.Spatial;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
	[StaticEventOwner]
	[MyEntityType(typeof(MyObjectBuilder_CubeGrid), true)]
	public class MyCubeGrid : MyEntity, IMyGridConnectivityTest, IMyEventProxy, IMyEventOwner, IMySyncedEntity, VRage.Game.ModAPI.IMyCubeGrid, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.Game.ModAPI.Ingame.IMyCubeGrid
	{
		public enum MyTestDisconnectsReason
		{
			NoReason,
			BlockRemoved,
			SplitBlock
		}

		internal enum MyTestDynamicReason
		{
			NoReason,
			GridCopied,
			GridSplit,
			GridSplitByBlock,
			ConvertToShip
		}

		private struct DeformationPostponedItem
		{
			public Vector3I Position;

			public Vector3I Min;

			public Vector3I Max;
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct MyBlockBuildArea
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EDefinitionId_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, DefinitionIdBlit>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in DefinitionIdBlit value)
				{
					owner.DefinitionId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out DefinitionIdBlit value)
				{
					value = owner.DefinitionId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EColorMaskHSV_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, uint>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in uint value)
				{
					owner.ColorMaskHSV = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out uint value)
				{
					value = owner.ColorMaskHSV;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EPosInGrid_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3I value)
				{
					owner.PosInGrid = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3I value)
				{
					value = owner.PosInGrid;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EBlockMin_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3B>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3B value)
				{
					owner.BlockMin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3B value)
				{
					value = owner.BlockMin;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EBlockMax_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3B>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3B value)
				{
					owner.BlockMax = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3B value)
				{
					value = owner.BlockMax;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EBuildAreaSize_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3UByte>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3UByte value)
				{
					owner.BuildAreaSize = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3UByte value)
				{
					value = owner.BuildAreaSize;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EStepDelta_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Vector3B>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Vector3B value)
				{
					owner.StepDelta = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Vector3B value)
				{
					value = owner.StepDelta;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EOrientationForward_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Base6Directions.Direction>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Base6Directions.Direction value)
				{
					owner.OrientationForward = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Base6Directions.Direction value)
				{
					value = owner.OrientationForward;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003EOrientationUp_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, Base6Directions.Direction>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in Base6Directions.Direction value)
				{
					owner.OrientationUp = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out Base6Directions.Direction value)
				{
					value = owner.OrientationUp;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_003C_003ESkinId_003C_003EAccessor : IMemberAccessor<MyBlockBuildArea, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockBuildArea owner, in MyStringHash value)
				{
					owner.SkinId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockBuildArea owner, out MyStringHash value)
				{
					value = owner.SkinId;
				}
			}

			public DefinitionIdBlit DefinitionId;

			public uint ColorMaskHSV;

			public Vector3I PosInGrid;

			public Vector3B BlockMin;

			public Vector3B BlockMax;

			public Vector3UByte BuildAreaSize;

			public Vector3B StepDelta;

			public Base6Directions.Direction OrientationForward;

			public Base6Directions.Direction OrientationUp;

			public MyStringHash SkinId;
		}

		[ProtoContract]
		public struct MyBlockLocation
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EMin_003C_003EAccessor : IMemberAccessor<MyBlockLocation, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in Vector3I value)
				{
					owner.Min = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out Vector3I value)
				{
					value = owner.Min;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EMax_003C_003EAccessor : IMemberAccessor<MyBlockLocation, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in Vector3I value)
				{
					owner.Max = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out Vector3I value)
				{
					value = owner.Max;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003ECenterPos_003C_003EAccessor : IMemberAccessor<MyBlockLocation, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in Vector3I value)
				{
					owner.CenterPos = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out Vector3I value)
				{
					value = owner.CenterPos;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EOrientation_003C_003EAccessor : IMemberAccessor<MyBlockLocation, MyBlockOrientation>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in MyBlockOrientation value)
				{
					owner.Orientation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out MyBlockOrientation value)
				{
					value = owner.Orientation;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyBlockLocation, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EBlockDefinition_003C_003EAccessor : IMemberAccessor<MyBlockLocation, DefinitionIdBlit>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in DefinitionIdBlit value)
				{
					owner.BlockDefinition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out DefinitionIdBlit value)
				{
					value = owner.BlockDefinition;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MyBlockLocation, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockLocation owner, in long value)
				{
					owner.Owner = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockLocation owner, out long value)
				{
					value = owner.Owner;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003C_003EActor : IActivator, IActivator<MyBlockLocation>
			{
				private sealed override object CreateInstance()
				{
					return default(MyBlockLocation);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyBlockLocation CreateInstance()
				{
					return (MyBlockLocation)(object)default(MyBlockLocation);
				}

				MyBlockLocation IActivator<MyBlockLocation>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public Vector3I Min;

			[ProtoMember(4)]
			public Vector3I Max;

			[ProtoMember(7)]
			public Vector3I CenterPos;

			[ProtoMember(10)]
			public MyBlockOrientation Orientation;

			[ProtoMember(13)]
			public long EntityId;

			[ProtoMember(16)]
			public DefinitionIdBlit BlockDefinition;

			[ProtoMember(19)]
			public long Owner;

			public MyBlockLocation(MyDefinitionId blockDefinition, Vector3I min, Vector3I max, Vector3I center, Quaternion orientation, long entityId, long owner)
			{
				BlockDefinition = blockDefinition;
				Min = min;
				Max = max;
				CenterPos = center;
				Orientation = new MyBlockOrientation(ref orientation);
				EntityId = entityId;
				Owner = owner;
			}
		}

		[ProtoContract]
		public struct BlockPositionId
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003C_003EPosition_003C_003EAccessor : IMemberAccessor<BlockPositionId, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BlockPositionId owner, in Vector3I value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BlockPositionId owner, out Vector3I value)
				{
					value = owner.Position;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003C_003ECompoundId_003C_003EAccessor : IMemberAccessor<BlockPositionId, uint>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BlockPositionId owner, in uint value)
				{
					owner.CompoundId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BlockPositionId owner, out uint value)
				{
					value = owner.CompoundId;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003C_003EActor : IActivator, IActivator<BlockPositionId>
			{
				private sealed override object CreateInstance()
				{
					return default(BlockPositionId);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BlockPositionId CreateInstance()
				{
					return (BlockPositionId)(object)default(BlockPositionId);
				}

				BlockPositionId IActivator<BlockPositionId>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(22)]
			public Vector3I Position;

			[ProtoMember(25)]
			public uint CompoundId;
		}

		[ProtoContract]
		public struct MyBlockVisuals
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003EColorMaskHSV_003C_003EAccessor : IMemberAccessor<MyBlockVisuals, uint>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockVisuals owner, in uint value)
				{
					owner.ColorMaskHSV = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockVisuals owner, out uint value)
				{
					value = owner.ColorMaskHSV;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003ESkinId_003C_003EAccessor : IMemberAccessor<MyBlockVisuals, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockVisuals owner, in MyStringHash value)
				{
					owner.SkinId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockVisuals owner, out MyStringHash value)
				{
					value = owner.SkinId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003EApplyColor_003C_003EAccessor : IMemberAccessor<MyBlockVisuals, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockVisuals owner, in bool value)
				{
					owner.ApplyColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockVisuals owner, out bool value)
				{
					value = owner.ApplyColor;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003EApplySkin_003C_003EAccessor : IMemberAccessor<MyBlockVisuals, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyBlockVisuals owner, in bool value)
				{
					owner.ApplySkin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyBlockVisuals owner, out bool value)
				{
					value = owner.ApplySkin;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_003C_003EActor : IActivator, IActivator<MyBlockVisuals>
			{
				private sealed override object CreateInstance()
				{
					return default(MyBlockVisuals);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyBlockVisuals CreateInstance()
				{
					return (MyBlockVisuals)(object)default(MyBlockVisuals);
				}

				MyBlockVisuals IActivator<MyBlockVisuals>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(28)]
			public uint ColorMaskHSV;

			[ProtoMember(31)]
			public MyStringHash SkinId;

			[ProtoMember(33)]
			public bool ApplyColor;

			[ProtoMember(35)]
			public bool ApplySkin;

			public MyBlockVisuals(uint colorMaskHsv, MyStringHash skinId, bool applyColor = true, bool applySkin = true)
			{
				ColorMaskHSV = colorMaskHsv;
				SkinId = skinId;
				ApplyColor = applyColor;
				ApplySkin = applySkin;
			}
		}

		private enum NeighborOffsetIndex
		{
			XUP,
			XDOWN,
			YUP,
			YDOWN,
			ZUP,
			ZDOWN,
			XUP_YUP,
			XUP_YDOWN,
			XDOWN_YUP,
			XDOWN_YDOWN,
			YUP_ZUP,
			YUP_ZDOWN,
			YDOWN_ZUP,
			YDOWN_ZDOWN,
			XUP_ZUP,
			XUP_ZDOWN,
			XDOWN_ZUP,
			XDOWN_ZDOWN,
			XUP_YUP_ZUP,
			XUP_YUP_ZDOWN,
			XUP_YDOWN_ZUP,
			XUP_YDOWN_ZDOWN,
			XDOWN_YUP_ZUP,
			XDOWN_YUP_ZDOWN,
			XDOWN_YDOWN_ZUP,
			XDOWN_YDOWN_ZDOWN
		}

		private struct MyNeighbourCachedBlock
		{
			public Vector3I Position;

			public MyCubeBlockDefinition BlockDefinition;

			public MyBlockOrientation Orientation;

			public override int GetHashCode()
			{
				return Position.GetHashCode();
			}
		}

		public class BlockTypeCounter
		{
			private Dictionary<MyDefinitionId, int> m_countById = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

			internal int GetNextNumber(MyDefinitionId blockType)
			{
				int value = 0;
				m_countById.TryGetValue(blockType, out value);
				value++;
				m_countById[blockType] = value;
				return value;
			}
		}

		private class PasteGridData : WorkData
		{
			private List<MyObjectBuilder_CubeGrid> m_entities;

			private bool m_detectDisconnects;

			private Vector3 m_objectVelocity;

			private bool m_multiBlock;

			private bool m_instantBuild;

			private List<MyCubeGrid> m_results;

			private bool m_canPlaceGrid;

			private List<VRage.ModAPI.IMyEntity> m_resultIDs;

			private bool m_removeScripts;

			public readonly EndpointId SenderEndpointId;

			public readonly bool IsLocallyInvoked;

			public Vector3D? m_offset;

			public PasteGridData(List<MyObjectBuilder_CubeGrid> entities, bool detectDisconnects, Vector3 objectVelocity, bool multiBlock, bool instantBuild, bool shouldRemoveScripts, EndpointId senderEndpointId, bool isLocallyInvoked, Vector3D? offset)
			{
				m_entities = new List<MyObjectBuilder_CubeGrid>(entities);
				m_detectDisconnects = detectDisconnects;
				m_objectVelocity = objectVelocity;
				m_multiBlock = multiBlock;
				m_instantBuild = instantBuild;
				SenderEndpointId = senderEndpointId;
				IsLocallyInvoked = isLocallyInvoked;
				m_removeScripts = shouldRemoveScripts;
				m_offset = offset;
			}

			public void TryPasteGrid()
			{
				bool flag = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(SenderEndpointId.Value);
				if (!MySession.Static.SurvivalMode || flag)
				{
					for (int i = 0; i < m_entities.Count; i++)
					{
						m_entities[i] = (MyObjectBuilder_CubeGrid)m_entities[i].Clone();
					}
					MyEntities.RemapObjectBuilderCollection(m_entities);
					MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
					MySessionComponentGameInventory component2 = MySession.Static.GetComponent<MySessionComponentGameInventory>();
					foreach (MyObjectBuilder_CubeGrid entity in m_entities)
					{
						int num = 0;
						while (num < entity.CubeBlocks.Count)
						{
							MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = entity.CubeBlocks[num];
							if (m_removeScripts)
							{
								MyObjectBuilder_MyProgrammableBlock myObjectBuilder_MyProgrammableBlock = myObjectBuilder_CubeBlock as MyObjectBuilder_MyProgrammableBlock;
								if (myObjectBuilder_MyProgrammableBlock != null)
								{
									myObjectBuilder_MyProgrammableBlock.Program = null;
								}
							}
							myObjectBuilder_CubeBlock.SkinSubtypeId = component2.ValidateArmor(MyStringHash.GetOrCompute(myObjectBuilder_CubeBlock.SkinSubtypeId), SenderEndpointId.Value).String;
							if (!component.HasDefinitionDLC(new MyDefinitionId(myObjectBuilder_CubeBlock.TypeId, myObjectBuilder_CubeBlock.SubtypeId), SenderEndpointId.Value))
							{
								entity.CubeBlocks.RemoveAt(num);
							}
							else
							{
								num++;
							}
						}
					}
					_ = (m_instantBuild && flag);
					m_results = new List<MyCubeGrid>();
					MyEntityIdentifier.InEntityCreationBlock = true;
					MyEntityIdentifier.LazyInitPerThreadStorage(2048);
					m_canPlaceGrid = true;
					foreach (MyObjectBuilder_CubeGrid entity2 in m_entities)
					{
						MySandboxGame.Log.WriteLine("CreateCompressedMsg: Type: " + entity2.GetType().Name.ToString() + "  Name: " + entity2.Name + "  EntityID: " + entity2.EntityId.ToString("X8"));
						MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(entity2, fadeIn: false) as MyCubeGrid;
						if (myCubeGrid != null)
						{
							m_results.Add(myCubeGrid);
							m_canPlaceGrid &= TestPastedGridPlacement(myCubeGrid, testPhysics: false);
							if (!m_canPlaceGrid)
							{
								break;
							}
							long inventoryEntityId = 0L;
							if (m_instantBuild && flag)
							{
								ChangeOwnership(inventoryEntityId, myCubeGrid);
							}
							MySandboxGame.Log.WriteLine("Status: Exists(" + MyEntities.EntityExists(entity2.EntityId) + ") InScene(" + ((entity2.PersistentFlags & MyPersistentEntityFlags2.InScene) == MyPersistentEntityFlags2.InScene) + ")");
						}
					}
					m_resultIDs = new List<VRage.ModAPI.IMyEntity>();
					MyEntityIdentifier.GetPerThreadEntities(m_resultIDs);
					MyEntityIdentifier.ClearPerThreadEntities();
					MyEntityIdentifier.InEntityCreationBlock = false;
				}
			}

			private bool TestPastedGridPlacement(MyCubeGrid grid, bool testPhysics)
			{
				MyGridPlacementSettings settings = MyClipboardComponent.ClipboardDefinition.PastingSettings.GetGridPlacementSettings(grid.GridSizeEnum, grid.IsStatic);
				return TestPlacementArea(grid, grid.IsStatic, ref settings, grid.PositionComp.LocalAABB, !grid.IsStatic, null, testVoxel: true, testPhysics);
			}

			public void Callback()
			{
				if (!IsLocallyInvoked)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SendHudNotificationAfterPaste, SenderEndpointId);
				}
				else if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MyHud.PopRotatingWheelVisible();
				}
				if (m_canPlaceGrid)
				{
					foreach (MyCubeGrid result in m_results)
					{
						m_canPlaceGrid &= TestPastedGridPlacement(result, testPhysics: true);
						if (!m_canPlaceGrid)
						{
							break;
						}
					}
				}
				if (m_canPlaceGrid && m_results.Count > 0)
				{
					foreach (VRage.ModAPI.IMyEntity resultID in m_resultIDs)
					{
						MyEntityIdentifier.TryGetEntity(resultID.EntityId, out VRage.ModAPI.IMyEntity entity);
						if (entity == null)
						{
							MyEntityIdentifier.AddEntityWithId(resultID);
						}
					}
					AfterPaste(m_results, m_objectVelocity, m_detectDisconnects);
				}
				else
				{
					if (m_results != null)
					{
						foreach (MyCubeGrid result2 in m_results)
						{
							foreach (MySlimBlock block in result2.GetBlocks())
							{
								block.RemoveAuthorship();
							}
							result2.Close();
						}
					}
					if (!IsLocallyInvoked)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ShowPasteFailedOperation, SenderEndpointId);
					}
				}
				if (m_offset.HasValue)
				{
					foreach (MyCubeGrid result3 in m_results)
					{
						MatrixD worldMatrix = result3.WorldMatrix;
						worldMatrix.Translation += m_offset.Value;
						result3.WorldMatrix = worldMatrix;
					}
				}
			}
		}

		[Serializable]
		public struct RelativeOffset
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ERelativeOffset_003C_003EUse_003C_003EAccessor : IMemberAccessor<RelativeOffset, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RelativeOffset owner, in bool value)
				{
					owner.Use = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RelativeOffset owner, out bool value)
				{
					value = owner.Use;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ERelativeOffset_003C_003ERelativeToEntity_003C_003EAccessor : IMemberAccessor<RelativeOffset, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RelativeOffset owner, in bool value)
				{
					owner.RelativeToEntity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RelativeOffset owner, out bool value)
				{
					value = owner.RelativeToEntity;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ERelativeOffset_003C_003ESpawnerId_003C_003EAccessor : IMemberAccessor<RelativeOffset, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RelativeOffset owner, in long value)
				{
					owner.SpawnerId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RelativeOffset owner, out long value)
				{
					value = owner.SpawnerId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ERelativeOffset_003C_003EOriginalSpawnPoint_003C_003EAccessor : IMemberAccessor<RelativeOffset, Vector3D>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RelativeOffset owner, in Vector3D value)
				{
					owner.OriginalSpawnPoint = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RelativeOffset owner, out Vector3D value)
				{
					value = owner.OriginalSpawnPoint;
				}
			}

			public bool Use;

			public bool RelativeToEntity;

			public long SpawnerId;

			public Vector3D OriginalSpawnPoint;
		}

		[ProtoContract]
		public struct MySingleOwnershipRequest
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003C_003EBlockId_003C_003EAccessor : IMemberAccessor<MySingleOwnershipRequest, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MySingleOwnershipRequest owner, in long value)
				{
					owner.BlockId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MySingleOwnershipRequest owner, out long value)
				{
					value = owner.BlockId;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MySingleOwnershipRequest, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MySingleOwnershipRequest owner, in long value)
				{
					owner.Owner = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MySingleOwnershipRequest owner, out long value)
				{
					value = owner.Owner;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003C_003EActor : IActivator, IActivator<MySingleOwnershipRequest>
			{
				private sealed override object CreateInstance()
				{
					return default(MySingleOwnershipRequest);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MySingleOwnershipRequest CreateInstance()
				{
					return (MySingleOwnershipRequest)(object)default(MySingleOwnershipRequest);
				}

				MySingleOwnershipRequest IActivator<MySingleOwnershipRequest>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(28)]
			public long BlockId;

			[ProtoMember(31)]
			public long Owner;
		}

		[ProtoContract]
		public struct LocationIdentity
		{
			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003C_003ELocation_003C_003EAccessor : IMemberAccessor<LocationIdentity, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref LocationIdentity owner, in Vector3I value)
				{
					owner.Location = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref LocationIdentity owner, out Vector3I value)
				{
					value = owner.Location;
				}
			}

			protected class Sandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003C_003EId_003C_003EAccessor : IMemberAccessor<LocationIdentity, ushort>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref LocationIdentity owner, in ushort value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref LocationIdentity owner, out ushort value)
				{
					value = owner.Id;
				}
			}

			private class Sandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003C_003EActor : IActivator, IActivator<LocationIdentity>
			{
				private sealed override object CreateInstance()
				{
					return default(LocationIdentity);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override LocationIdentity CreateInstance()
				{
					return (LocationIdentity)(object)default(LocationIdentity);
				}

				LocationIdentity IActivator<LocationIdentity>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(34)]
			public Vector3I Location;

			[ProtoMember(37)]
			public ushort Id;
		}

		private class MyCubeGridPosition : MyPositionComponent
		{
			private class Sandbox_Game_Entities_MyCubeGrid_003C_003EMyCubeGridPosition_003C_003EActor : IActivator, IActivator<MyCubeGridPosition>
			{
				private sealed override object CreateInstance()
				{
					return new MyCubeGridPosition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyCubeGridPosition CreateInstance()
				{
					return new MyCubeGridPosition();
				}

				MyCubeGridPosition IActivator<MyCubeGridPosition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private MyCubeGrid m_grid;

			public override void OnAddedToContainer()
			{
				base.OnAddedToContainer();
				m_grid = (base.Container.Entity as MyCubeGrid);
			}

			protected override void OnWorldPositionChanged(object source, bool updateChildren, bool forceUpdateAllChildren)
			{
				m_grid.m_worldPositionChanged = true;
				base.OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
			}
		}

		private class MySyncGridThrustState
		{
			public Vector3B LastSendState;

			public int SleepFrames;

			public bool ShouldSend(Vector3B newThrust)
			{
				if (SleepFrames > 4 && LastSendState != newThrust)
				{
					SleepFrames = 0;
					LastSendState = newThrust;
					return true;
				}
				SleepFrames++;
				return false;
			}
		}

		public class MyCubeGridHitInfo
		{
			public MyIntersectionResultLineTriangleEx Triangle;

			public Vector3I Position;

			public int CubePartIndex = -1;

			public void Reset()
			{
				Triangle = default(MyIntersectionResultLineTriangleEx);
				Position = default(Vector3I);
				CubePartIndex = -1;
			}
		}

		private class AreaConnectivityTest : IMyGridConnectivityTest
		{
			private readonly Dictionary<Vector3I, Vector3I> m_lookup = new Dictionary<Vector3I, Vector3I>();

			private MyBlockOrientation m_orientation;

			private MyCubeBlockDefinition m_definition;

			private Vector3I m_posInGrid;

			private Vector3I m_blockMin;

			private Vector3I m_blockMax;

			private Vector3I m_stepDelta;

			public void Initialize(ref MyBlockBuildArea area, MyCubeBlockDefinition definition)
			{
				m_definition = definition;
				m_orientation = new MyBlockOrientation(area.OrientationForward, area.OrientationUp);
				m_posInGrid = area.PosInGrid;
				m_blockMin = area.BlockMin;
				m_blockMax = area.BlockMax;
				m_stepDelta = area.StepDelta;
				m_lookup.Clear();
			}

			public void AddBlock(Vector3UByte offset)
			{
				Vector3I vector3I = m_posInGrid + offset * m_stepDelta;
				Vector3I b = default(Vector3I);
				b.X = m_blockMin.X;
				while (b.X <= m_blockMax.X)
				{
					b.Y = m_blockMin.Y;
					while (b.Y <= m_blockMax.Y)
					{
						b.Z = m_blockMin.Z;
						while (b.Z <= m_blockMax.Z)
						{
							m_lookup.Add(vector3I + b, vector3I);
							b.Z++;
						}
						b.Y++;
					}
					b.X++;
				}
			}

			public void GetConnectedBlocks(Vector3I minI, Vector3I maxI, Dictionary<Vector3I, ConnectivityResult> outOverlappedCubeBlocks)
			{
				Vector3I key = default(Vector3I);
				key.X = minI.X;
				while (key.X <= maxI.X)
				{
					key.Y = minI.Y;
					while (key.Y <= maxI.Y)
					{
						key.Z = minI.Z;
						while (key.Z <= maxI.Z)
						{
							if (m_lookup.TryGetValue(key, out Vector3I value) && !outOverlappedCubeBlocks.ContainsKey(value))
							{
								outOverlappedCubeBlocks.Add(value, new ConnectivityResult
								{
									Definition = m_definition,
									FatBlock = null,
									Position = value,
									Orientation = m_orientation
								});
							}
							key.Z++;
						}
						key.Y++;
					}
					key.X++;
				}
			}
		}

		private struct TriangleWithMaterial
		{
			public MyTriangleVertexIndices triangle;

			public MyTriangleVertexIndices uvIndices;

			public string material;
		}

		protected sealed class OnGridChangedRPC_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnGridChangedRPC();
			}
		}

		protected sealed class CreateSplit_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Int64 : ICallSite<MyCubeGrid, List<Vector3I>, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> blocks, in long newEntityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateSplit_Implementation(blocks, newEntityId);
			}
		}

		protected sealed class CreateSplits_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_Cube_MyDisconnectHelper_003C_003EGroup_003E : ICallSite<MyCubeGrid, List<Vector3I>, List<MyDisconnectHelper.Group>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> blocks, in List<MyDisconnectHelper.Group> groups, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateSplits_Implementation(blocks, groups);
			}
		}

		protected sealed class RemovedBlocks_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E : ICallSite<MyCubeGrid, List<Vector3I>, List<Vector3I>, List<Vector3I>, List<Vector3I>, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> locationsWithGenerator, in List<Vector3I> destroyLocations, in List<Vector3I> DestructionDeformationLocation, in List<Vector3I> LocationsWithoutGenerator, in DBNull arg5, in DBNull arg6)
			{
				@this.RemovedBlocks(locationsWithGenerator, destroyLocations, DestructionDeformationLocation, LocationsWithoutGenerator);
			}
		}

		protected sealed class RemovedBlocksWithIds_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003E_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003E_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003E_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EBlockPositionId_003E : ICallSite<MyCubeGrid, List<BlockPositionId>, List<BlockPositionId>, List<BlockPositionId>, List<BlockPositionId>, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<BlockPositionId> removeBlockWithIdQueueWithGenerators, in List<BlockPositionId> destroyBlockWithIdQueueWithGenerators, in List<BlockPositionId> destroyBlockWithIdQueueWithoutGenerators, in List<BlockPositionId> removeBlockWithIdQueueWithoutGenerators, in DBNull arg5, in DBNull arg6)
			{
				@this.RemovedBlocksWithIds(removeBlockWithIdQueueWithGenerators, destroyBlockWithIdQueueWithGenerators, destroyBlockWithIdQueueWithoutGenerators, removeBlockWithIdQueueWithoutGenerators);
			}
		}

		protected sealed class RemoveBlocksBuiltByID_003C_003ESystem_Int64 : ICallSite<MyCubeGrid, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long identityID, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RemoveBlocksBuiltByID(identityID);
			}
		}

		protected sealed class TransferBlocksBuiltByID_003C_003ESystem_Int64_0023System_Int64 : ICallSite<MyCubeGrid, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long oldAuthor, in long newAuthor, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.TransferBlocksBuiltByID(oldAuthor, newAuthor);
			}
		}

		protected sealed class BuildBlockRequest_003C_003ESystem_UInt32_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_0023VRage_Game_MyObjectBuilder_CubeBlock_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, uint, MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in uint colorMaskHsv, in MyBlockLocation location, in MyObjectBuilder_CubeBlock blockObjectBuilder, in long builderEntityId, in bool instantBuild, in long ownerId)
			{
				@this.BuildBlockRequest(colorMaskHsv, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlockRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_0023VRage_Game_MyObjectBuilder_CubeBlock_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in MyBlockLocation location, in MyObjectBuilder_CubeBlock blockObjectBuilder, in long builderEntityId, in bool instantBuild, in long ownerId)
			{
				@this.BuildBlockRequest(visuals, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlockSucess_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_0023VRage_Game_MyObjectBuilder_CubeBlock_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, MyBlockLocation, MyObjectBuilder_CubeBlock, long, bool, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in MyBlockLocation location, in MyObjectBuilder_CubeBlock blockObjectBuilder, in long builderEntityId, in bool instantBuild, in long ownerId)
			{
				@this.BuildBlockSucess(visuals, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlocksRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Collections_Generic_HashSet_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003E_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, HashSet<MyBlockLocation>, long, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in HashSet<MyBlockLocation> locations, in long builderEntityId, in bool instantBuild, in long ownerId, in DBNull arg6)
			{
				@this.BuildBlocksRequest(visuals, locations, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlocksFailedNotify_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.BuildBlocksFailedNotify();
			}
		}

		protected sealed class BuildBlocksClient_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Collections_Generic_HashSet_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockLocation_003E_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, HashSet<MyBlockLocation>, long, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in HashSet<MyBlockLocation> locations, in long builderEntityId, in bool instantBuild, in long ownerId, in DBNull arg6)
			{
				@this.BuildBlocksClient(visuals, locations, builderEntityId, instantBuild, ownerId);
			}
		}

		protected sealed class BuildBlocksAreaRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_0023System_Int64_0023System_Boolean_0023System_Int64_0023System_UInt64 : ICallSite<MyCubeGrid, MyBlockBuildArea, long, bool, long, ulong, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockBuildArea area, in long builderEntityId, in bool instantBuild, in long ownerId, in ulong placingPlayer, in DBNull arg6)
			{
				@this.BuildBlocksAreaRequest(area, builderEntityId, instantBuild, ownerId, placingPlayer);
			}
		}

		protected sealed class BuildBlocksAreaClient_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockBuildArea_0023System_Int32_0023System_Collections_Generic_HashSet_00601_003CVRageMath_Vector3UByte_003E_0023System_Int64_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockBuildArea, int, HashSet<Vector3UByte>, long, bool, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockBuildArea area, in int entityIdSeed, in HashSet<Vector3UByte> failList, in long builderEntityId, in bool isAdmin, in long ownerId)
			{
				@this.BuildBlocksAreaClient(area, entityIdSeed, failList, builderEntityId, isAdmin, ownerId);
			}
		}

		protected sealed class RazeBlocksAreaRequest_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3UByte_0023System_Int64_0023System_UInt64 : ICallSite<MyCubeGrid, Vector3I, Vector3UByte, long, ulong, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in Vector3UByte size, in long builderEntityId, in ulong placingPlayer, in DBNull arg5, in DBNull arg6)
			{
				@this.RazeBlocksAreaRequest(pos, size, builderEntityId, placingPlayer);
			}
		}

		protected sealed class RazeBlocksAreaSuccess_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3UByte_0023System_Collections_Generic_HashSet_00601_003CVRageMath_Vector3UByte_003E : ICallSite<MyCubeGrid, Vector3I, Vector3UByte, HashSet<Vector3UByte>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in Vector3UByte size, in HashSet<Vector3UByte> resultFailList, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RazeBlocksAreaSuccess(pos, size, resultFailList);
			}
		}

		protected sealed class RazeBlocksRequest_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E_0023System_Int64_0023System_UInt64 : ICallSite<MyCubeGrid, List<Vector3I>, long, ulong, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> locations, in long builderEntityId, in ulong user, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RazeBlocksRequest(locations, builderEntityId, user);
			}
		}

		protected sealed class RazeBlocksClient_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E : ICallSite<MyCubeGrid, List<Vector3I>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> locations, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RazeBlocksClient(locations);
			}
		}

		protected sealed class ColorGridFriendlyRequest_003C_003EVRageMath_Vector3_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3 newHSV, in bool playSound, in long player, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ColorGridFriendlyRequest(newHSV, playSound, player);
			}
		}

		protected sealed class OnColorGridFriendly_003C_003EVRageMath_Vector3_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3 newHSV, in bool playSound, in long player, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnColorGridFriendly(newHSV, playSound, player);
			}
		}

		protected sealed class ColorBlockRequest_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3I_0023VRageMath_Vector3_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, Vector3, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I min, in Vector3I max, in Vector3 newHSV, in bool playSound, in long player, in DBNull arg6)
			{
				@this.ColorBlockRequest(min, max, newHSV, playSound, player);
			}
		}

		protected sealed class OnColorBlock_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3I_0023VRageMath_Vector3_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, Vector3, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I min, in Vector3I max, in Vector3 newHSV, in bool playSound, in long player, in DBNull arg6)
			{
				@this.OnColorBlock(min, max, newHSV, playSound, player);
			}
		}

		protected sealed class SkinGridFriendlyRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in bool playSound, in long player, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SkinGridFriendlyRequest(visuals, playSound, player);
			}
		}

		protected sealed class OnSkinGridFriendly_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, MyBlockVisuals, bool, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyBlockVisuals visuals, in bool playSound, in long player, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSkinGridFriendly(visuals, playSound, player);
			}
		}

		protected sealed class SkinBlockRequest_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3I_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, MyBlockVisuals, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I min, in Vector3I max, in MyBlockVisuals visuals, in bool playSound, in long player, in DBNull arg6)
			{
				@this.SkinBlockRequest(min, max, visuals, playSound, player);
			}
		}

		protected sealed class OnSkinBlock_003C_003EVRageMath_Vector3I_0023VRageMath_Vector3I_0023Sandbox_Game_Entities_MyCubeGrid_003C_003EMyBlockVisuals_0023System_Boolean_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, Vector3I, MyBlockVisuals, bool, long, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I min, in Vector3I max, in MyBlockVisuals visuals, in bool playSound, in long player, in DBNull arg6)
			{
				@this.OnSkinBlock(min, max, visuals, playSound, player);
			}
		}

		protected sealed class OnConvertToDynamic_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConvertToDynamic();
			}
		}

		protected sealed class ConvertToStatic_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ConvertToStatic();
			}
		}

		protected sealed class BlockIntegrityChanged_003C_003EVRageMath_Vector3I_0023System_UInt16_0023System_Single_0023System_Single_0023VRage_Game_ModAPI_MyIntegrityChangeEnum_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, ushort, float, float, MyIntegrityChangeEnum, long>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in ushort subBlockId, in float buildIntegrity, in float integrity, in MyIntegrityChangeEnum integrityChangeType, in long grinderOwner)
			{
				@this.BlockIntegrityChanged(pos, subBlockId, buildIntegrity, integrity, integrityChangeType, grinderOwner);
			}
		}

		protected sealed class BlockStockpileChanged_003C_003EVRageMath_Vector3I_0023System_UInt16_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyStockpileItem_003E : ICallSite<MyCubeGrid, Vector3I, ushort, List<MyStockpileItem>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in ushort subBlockId, in List<MyStockpileItem> items, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.BlockStockpileChanged(pos, subBlockId, items);
			}
		}

		protected sealed class FractureComponentRepaired_003C_003EVRageMath_Vector3I_0023System_UInt16_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, ushort, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I pos, in ushort subBlockId, in long toolOwner, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.FractureComponentRepaired(pos, subBlockId, toolOwner);
			}
		}

		protected sealed class PasteBlocksToGridServer_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_MyObjectBuilder_CubeGrid_003E_0023System_Int64_0023System_Boolean_0023System_Boolean : ICallSite<MyCubeGrid, List<MyObjectBuilder_CubeGrid>, long, bool, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<MyObjectBuilder_CubeGrid> gridsToMerge, in long inventoryEntityId, in bool multiBlock, in bool instantBuild, in DBNull arg5, in DBNull arg6)
			{
				@this.PasteBlocksToGridServer_Implementation(gridsToMerge, inventoryEntityId, multiBlock, instantBuild);
			}
		}

		protected sealed class PasteBlocksToGridClient_Implementation_003C_003EVRage_Game_MyObjectBuilder_CubeGrid_0023VRageMath_MatrixI : ICallSite<MyCubeGrid, MyObjectBuilder_CubeGrid, MatrixI, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyObjectBuilder_CubeGrid gridToMerge, in MatrixI mergeTransform, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PasteBlocksToGridClient_Implementation(gridToMerge, mergeTransform);
			}
		}

		protected sealed class TryCreateGrid_Implementation_003C_003EVRage_Game_MyCubeSize_0023System_Boolean_0023VRage_MyPositionAndOrientation_0023System_Int64_0023System_Boolean : ICallSite<IMyEventOwner, MyCubeSize, bool, MyPositionAndOrientation, long, bool, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyCubeSize cubeSize, in bool isStatic, in MyPositionAndOrientation position, in long inventoryEntityId, in bool instantBuild, in DBNull arg6)
			{
				TryCreateGrid_Implementation(cubeSize, isStatic, position, inventoryEntityId, instantBuild);
			}
		}

		protected sealed class StationClosingDenied_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				StationClosingDenied();
			}
		}

		protected sealed class OnGridClosedRequest_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnGridClosedRequest(entityId);
			}
		}

		protected sealed class TryPasteGrid_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_MyObjectBuilder_CubeGrid_003E_0023System_Boolean_0023VRageMath_Vector3_0023System_Boolean_0023System_Boolean_0023Sandbox_Game_Entities_MyCubeGrid_003C_003ERelativeOffset : ICallSite<IMyEventOwner, List<MyObjectBuilder_CubeGrid>, bool, Vector3, bool, bool, RelativeOffset>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<MyObjectBuilder_CubeGrid> entities, in bool detectDisconnects, in Vector3 objectVelocity, in bool multiBlock, in bool instantBuild, in RelativeOffset offset)
			{
				TryPasteGrid_Implementation(entities, detectDisconnects, objectVelocity, multiBlock, instantBuild, offset);
			}
		}

		protected sealed class ShowPasteFailedOperation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowPasteFailedOperation();
			}
		}

		protected sealed class SendHudNotificationAfterPaste_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendHudNotificationAfterPaste();
			}
		}

		protected sealed class OnBonesReceived_003C_003ESystem_Int32_0023System_Collections_Generic_List_00601_003CSystem_Byte_003E : ICallSite<MyCubeGrid, int, List<byte>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in int segmentsCount, in List<byte> boneByteList, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnBonesReceived(segmentsCount, boneByteList);
			}
		}

		protected sealed class OnBonesMultiplied_003C_003EVRageMath_Vector3I_0023System_Single : ICallSite<MyCubeGrid, Vector3I, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I blockLocation, in float multiplier, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnBonesMultiplied(blockLocation, multiplier);
			}
		}

		protected sealed class RelfectorStateRecived_003C_003EVRage_MyMultipleEnabledEnum : ICallSite<MyCubeGrid, MyMultipleEnabledEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyMultipleEnabledEnum value, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RelfectorStateRecived(value);
			}
		}

		protected sealed class OnStockpileFillRequest_003C_003EVRageMath_Vector3I_0023System_Int64_0023System_Byte : ICallSite<MyCubeGrid, Vector3I, long, byte, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I blockPosition, in long ownerEntityId, in byte inventoryIndex, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnStockpileFillRequest(blockPosition, ownerEntityId, inventoryIndex);
			}
		}

		protected sealed class OnSetToConstructionRequest_003C_003EVRageMath_Vector3I_0023System_Int64_0023System_Byte_0023System_Int64 : ICallSite<MyCubeGrid, Vector3I, long, byte, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in Vector3I blockPosition, in long ownerEntityId, in byte inventoryIndex, in long requestingPlayer, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSetToConstructionRequest(blockPosition, ownerEntityId, inventoryIndex, requestingPlayer);
			}
		}

		protected sealed class OnPowerProducerStateRequest_003C_003EVRage_MyMultipleEnabledEnum_0023System_Int64 : ICallSite<MyCubeGrid, MyMultipleEnabledEnum, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyMultipleEnabledEnum enabledState, in long playerId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnPowerProducerStateRequest(enabledState, playerId);
			}
		}

		protected sealed class OnConvertedToShipRequest_003C_003ESandbox_Game_Entities_MyCubeGrid_003C_003EMyTestDynamicReason : ICallSite<MyCubeGrid, MyTestDynamicReason, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in MyTestDynamicReason reason, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConvertedToShipRequest(reason);
			}
		}

		protected sealed class OnConvertToShipFailed_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConvertToShipFailed();
			}
		}

		protected sealed class OnConvertedToStationRequest_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConvertedToStationRequest();
			}
		}

		protected sealed class OnChangeOwnerRequest_003C_003ESystem_Int64_0023System_Int64_0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyCubeGrid, long, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long blockId, in long owner, in MyOwnershipShareModeEnum shareMode, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOwnerRequest(blockId, owner, shareMode);
			}
		}

		protected sealed class OnChangeOwner_003C_003ESystem_Int64_0023System_Int64_0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyCubeGrid, long, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long blockId, in long owner, in MyOwnershipShareModeEnum shareMode, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOwner(blockId, owner, shareMode);
			}
		}

		protected sealed class SetHandbrakeRequest_003C_003ESystem_Boolean : ICallSite<MyCubeGrid, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in bool v, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetHandbrakeRequest(v);
			}
		}

		protected sealed class OnChangeGridOwner_003C_003ESystem_Int64_0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyCubeGrid, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long playerId, in MyOwnershipShareModeEnum shareMode, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeGridOwner(playerId, shareMode);
			}
		}

		protected sealed class OnRemoveSplit_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_Vector3I_003E : ICallSite<MyCubeGrid, List<Vector3I>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<Vector3I> removedBlocks, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSplit(removedBlocks);
			}
		}

		protected sealed class OnChangeDisplayNameRequest_003C_003ESystem_String : ICallSite<MyCubeGrid, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in string displayName, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDisplayNameRequest(displayName);
			}
		}

		protected sealed class OnModifyGroupSuccess_003C_003ESystem_String_0023System_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<MyCubeGrid, string, List<long>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in string name, in List<long> blocks, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnModifyGroupSuccess(name, blocks);
			}
		}

		protected sealed class OnRazeBlockInCompoundBlockRequest_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003E : ICallSite<MyCubeGrid, List<LocationIdentity>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<LocationIdentity> locationsAndIds, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRazeBlockInCompoundBlockRequest(locationsAndIds);
			}
		}

		protected sealed class OnRazeBlockInCompoundBlockSuccess_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003ELocationIdentity_003E : ICallSite<MyCubeGrid, List<LocationIdentity>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in List<LocationIdentity> locationsAndIds, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRazeBlockInCompoundBlockSuccess(locationsAndIds);
			}
		}

		protected sealed class OnChangeOwnersRequest_003C_003EVRage_Game_MyOwnershipShareModeEnum_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003E_0023System_Int64 : ICallSite<IMyEventOwner, MyOwnershipShareModeEnum, List<MySingleOwnershipRequest>, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyOwnershipShareModeEnum shareMode, in List<MySingleOwnershipRequest> requests, in long requestingPlayer, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChangeOwnersRequest(shareMode, requests, requestingPlayer);
			}
		}

		protected sealed class OnChangeOwnersSuccess_003C_003EVRage_Game_MyOwnershipShareModeEnum_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyCubeGrid_003C_003EMySingleOwnershipRequest_003E : ICallSite<IMyEventOwner, MyOwnershipShareModeEnum, List<MySingleOwnershipRequest>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyOwnershipShareModeEnum shareMode, in List<MySingleOwnershipRequest> requests, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnChangeOwnersSuccess(shareMode, requests);
			}
		}

		protected sealed class OnLogHierarchy_003C_003E : ICallSite<MyCubeGrid, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnLogHierarchy();
			}
		}

		protected sealed class DepressurizeEffect_003C_003ESystem_Int64_0023VRageMath_Vector3I_0023VRageMath_Vector3I : ICallSite<IMyEventOwner, long, Vector3I, Vector3I, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridId, in Vector3I from, in Vector3I to, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DepressurizeEffect(gridId, from, to);
			}
		}

		protected sealed class MergeGrid_MergeClient_003C_003ESystem_Int64_0023VRage_SerializableVector3I_0023VRageMath_Base6Directions_003C_003EDirection_0023VRageMath_Base6Directions_003C_003EDirection_0023VRageMath_Vector3I : ICallSite<MyCubeGrid, long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, Vector3I, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long gridId, in SerializableVector3I gridOffset, in Base6Directions.Direction gridForward, in Base6Directions.Direction gridUp, in Vector3I mergingBlockPos, in DBNull arg6)
			{
				@this.MergeGrid_MergeClient(gridId, gridOffset, gridForward, gridUp, mergingBlockPos);
			}
		}

		protected sealed class MergeGrid_MergeBlockClient_003C_003ESystem_Int64_0023VRage_SerializableVector3I_0023VRageMath_Base6Directions_003C_003EDirection_0023VRageMath_Base6Directions_003C_003EDirection : ICallSite<MyCubeGrid, long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCubeGrid @this, in long gridId, in SerializableVector3I gridOffset, in Base6Directions.Direction gridForward, in Base6Directions.Direction gridUp, in DBNull arg5, in DBNull arg6)
			{
				@this.MergeGrid_MergeBlockClient(gridId, gridOffset, gridForward, gridUp);
			}
		}

		protected class m_handBrakeSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType handBrakeSync;
				ISyncType result = handBrakeSync = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyCubeGrid)P_0).m_handBrakeSync = (Sync<bool, SyncDirection.BothWays>)handBrakeSync;
				return result;
			}
		}

		protected class m_dampenersEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType dampenersEnabled;
				ISyncType result = dampenersEnabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyCubeGrid)P_0).m_dampenersEnabled = (Sync<bool, SyncDirection.BothWays>)dampenersEnabled;
				return result;
			}
		}

		protected class m_markedAsTrash_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType markedAsTrash;
				ISyncType result = markedAsTrash = new Sync<bool, SyncDirection.FromServer>(P_1, P_2);
				((MyCubeGrid)P_0).m_markedAsTrash = (Sync<bool, SyncDirection.FromServer>)markedAsTrash;
				return result;
			}
		}

		protected class m_isRespawnGrid_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isRespawnGrid;
				ISyncType result = isRespawnGrid = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyCubeGrid)P_0).m_isRespawnGrid = (Sync<bool, SyncDirection.BothWays>)isRespawnGrid;
				return result;
			}
		}

		protected class m_destructibleBlocks_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType destructibleBlocks;
				ISyncType result = destructibleBlocks = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyCubeGrid)P_0).m_destructibleBlocks = (Sync<bool, SyncDirection.BothWays>)destructibleBlocks;
				return result;
			}
		}

		protected class m_editable_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType editable;
				ISyncType result = editable = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyCubeGrid)P_0).m_editable = (Sync<bool, SyncDirection.BothWays>)editable;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyCubeGrid_003C_003EActor : IActivator, IActivator<MyCubeGrid>
		{
			private sealed override object CreateInstance()
			{
				return new MyCubeGrid();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCubeGrid CreateInstance()
			{
				return new MyCubeGrid();
			}

			MyCubeGrid IActivator<MyCubeGrid>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly int BLOCK_LIMIT_FOR_LARGE_DESTRUCTION;

		private static readonly int TRASH_HIGHLIGHT;

		private static MyCubeGridHitInfo m_hitInfoTmp;

		private static HashSet<MyBlockLocation> m_tmpBuildList;

		private static List<Vector3I> m_tmpPositionListReceive;

		private static List<Vector3I> m_tmpPositionListSend;

		private List<Vector3I> m_removeBlockQueueWithGenerators = new List<Vector3I>();

		private List<Vector3I> m_removeBlockQueueWithoutGenerators = new List<Vector3I>();

		private List<Vector3I> m_destroyBlockQueue = new List<Vector3I>();

		private List<Vector3I> m_destructionDeformationQueue = new List<Vector3I>();

		private List<BlockPositionId> m_destroyBlockWithIdQueueWithGenerators = new List<BlockPositionId>();

		private List<BlockPositionId> m_destroyBlockWithIdQueueWithoutGenerators = new List<BlockPositionId>();

		private List<BlockPositionId> m_removeBlockWithIdQueueWithGenerators = new List<BlockPositionId>();

		private List<BlockPositionId> m_removeBlockWithIdQueueWithoutGenerators = new List<BlockPositionId>();

		[ThreadStatic]
		private static List<byte> m_boneByteList;

		private List<long> m_tmpBlockIdList = new List<long>();

		private HashSet<MyCubeBlock> m_inventoryBlocks = new HashSet<MyCubeBlock>();

		private HashSet<MyCubeBlock> m_unsafeBlocks = new HashSet<MyCubeBlock>();

		private HashSet<MyDecoy> m_decoys;

		private bool m_isRazeBatchDelayed;

		private MyDelayedRazeBatch m_delayedRazeBatch;

		public HashSet<MyCockpit> m_occupiedBlocks = new HashSet<MyCockpit>();

		private Vector3 m_gravity = Vector3.Zero;

		private readonly Sync<bool, SyncDirection.BothWays> m_handBrakeSync;

		private readonly Sync<bool, SyncDirection.BothWays> m_dampenersEnabled;

		private static List<MyObjectBuilder_CubeGrid> m_recievedGrids;

		public bool IsAccessibleForProgrammableBlock = true;

		private bool m_largeDestroyInProgress;

		private readonly Sync<bool, SyncDirection.FromServer> m_markedAsTrash;

		private int m_trashHighlightCounter;

		private float m_totalBoneDisplacement;

		private static float m_precalculatedCornerBonesDisplacementDistance;

		internal MyVoxelSegmentation BonesToSend = new MyVoxelSegmentation();

		private MyVoxelSegmentation m_bonesToSendSecond = new MyVoxelSegmentation();

		private int m_bonesSendCounter;

		private MyDirtyRegion m_dirtyRegion = new MyDirtyRegion();

		private MyDirtyRegion m_dirtyRegionParallel = new MyDirtyRegion();

		private MyCubeSize m_gridSizeEnum;

		private Vector3I m_min = Vector3I.MaxValue;

		private Vector3I m_max = Vector3I.MinValue;

		private readonly ConcurrentDictionary<Vector3I, MyCube> m_cubes = new ConcurrentDictionary<Vector3I, MyCube>();

		private readonly FastResourceLock m_cubeLock = new FastResourceLock();

		private bool m_canHavePhysics = true;

		private bool m_hasStandAloneBlocks = true;

		private readonly HashSet<MySlimBlock> m_cubeBlocks = new HashSet<MySlimBlock>();

		private MyConcurrentList<MyCubeBlock> m_fatBlocks = new MyConcurrentList<MyCubeBlock>(100);

		private MyLocalityGrouping m_explosions = new MyLocalityGrouping(MyLocalityGrouping.GroupingMode.Overlaps);

		private Dictionary<Vector3, int> m_colorStatistics = new Dictionary<Vector3, int>();

		private int m_PCU;

		private bool m_IsPowered;

		private HashSet<MyCubeBlock> m_processedBlocks = new HashSet<MyCubeBlock>();

		private HashSet<MyCubeBlock> m_blocksForDraw = new HashSet<MyCubeBlock>();

		private List<MyCubeGrid> m_tmpGrids = new List<MyCubeGrid>();

		private MyTestDisconnectsReason m_disconnectsDirty;

		private bool m_blocksForDamageApplicationDirty;

		private bool m_boundsDirty;

		private int m_lastUpdatedDirtyBounds;

		private HashSet<MySlimBlock> m_blocksForDamageApplication = new HashSet<MySlimBlock>();

		private List<MySlimBlock> m_blocksForDamageApplicationCopy = new List<MySlimBlock>();

		private bool m_updatingDirty;

		private int m_resolvingSplits;

		private HashSet<Vector3UByte> m_tmpBuildFailList = new HashSet<Vector3UByte>();

		private List<Vector3UByte> m_tmpBuildOffsets = new List<Vector3UByte>();

		private List<MySlimBlock> m_tmpBuildSuccessBlocks = new List<MySlimBlock>();

		private static List<Vector3I> m_tmpBlockPositions;

		[ThreadStatic]
		private static List<MySlimBlock> m_tmpBlockListReceive;

		[ThreadStatic]
		private static List<MyCockpit> m_tmpOccupiedCockpitsPerThread;

		[ThreadStatic]
		private static List<MyObjectBuilder_BlockGroup> m_tmpBlockGroupsPerThread;

		public bool HasShipSoundEvents;

		public int NumberOfReactors;

		public float GridGeneralDamageModifier = 1f;

		internal MyGridSkeleton Skeleton;

		public readonly BlockTypeCounter BlockCounter = new BlockTypeCounter();

		public Dictionary<MyObjectBuilderType, int> BlocksCounters = new Dictionary<MyObjectBuilderType, int>();

		private const float m_gizmoMaxDistanceFromCamera = 100f;

		private const float m_gizmoDrawLineScale = 0.002f;

		private bool m_isStatic;

		public Vector3I? XSymmetryPlane;

		public Vector3I? YSymmetryPlane;

		public Vector3I? ZSymmetryPlane;

		public bool XSymmetryOdd;

		public bool YSymmetryOdd;

		public bool ZSymmetryOdd;

		private readonly Sync<bool, SyncDirection.BothWays> m_isRespawnGrid;

		public int m_playedTime;

		public bool ControlledFromTurret;

		private readonly Sync<bool, SyncDirection.BothWays> m_destructibleBlocks;

		private Sync<bool, SyncDirection.BothWays> m_editable;

		internal readonly List<MyBlockGroup> BlockGroups = new List<MyBlockGroup>();

		internal MyCubeGridOwnershipManager m_ownershipManager;

		public MyProjectorBase Projector;

		private bool m_isMarkedForEarlyDeactivation;

		public bool CreatePhysics;

		private static readonly HashSet<MyResourceSinkComponent> m_tmpSinks;

		private static List<LocationIdentity> m_tmpLocationsAndIdsSend;

		private static List<Tuple<Vector3I, ushort>> m_tmpLocationsAndIdsReceive;

		private bool m_smallToLargeConnectionsInitialized;

		private bool m_enableSmallToLargeConnections = true;

		private MyTestDynamicReason m_testDynamic;

		private bool m_worldPositionChanged;

		private bool m_hasAdditionalModelGenerators;

		public MyTerminalBlock MainCockpit;

		public MyTerminalBlock MainRemoteControl;

		private Dictionary<int, MyCubeGridMultiBlockInfo> m_multiBlockInfos;

		private float PREDICTION_SWITCH_TIME = 5f;

		private int PREDICTION_SWITCH_MIN_COUNTER = 30;

		private bool m_inventoryMassDirty;

		private static List<MyVoxelBase> m_overlappingVoxelsTmp;

		private static HashSet<MyVoxelBase> m_rootVoxelsToCutTmp;

		private static ConcurrentQueue<MyTuple<int, MyVoxelBase, Vector3I, Vector3I>> m_notificationQueue;

		private List<DeformationPostponedItem> m_deformationPostponed = new List<DeformationPostponedItem>();

		private static MyConcurrentPool<List<DeformationPostponedItem>> m_postponedListsPool;

		private Action m_OnUpdateDirtyCompleted;

		private Action m_UpdateDirtyInternal;

		private bool m_bonesSending;

		private WorkData m_workData = new WorkData();

		[ThreadStatic]
		private static HashSet<MyEntity> m_tmpQueryCubeBlocks;

		[ThreadStatic]
		private static HashSet<MySlimBlock> m_tmpQuerySlimBlocks;

		private bool m_generatorsEnabled = true;

		private static readonly Vector3I[] m_tmpBlockSurroundingOffsets;

		private MyHudNotification m_inertiaDampenersNotification;

		private MyGridClientState m_lastNetState;

		private List<long> m_targetingList = new List<long>();

		private bool m_targetingListIsWhitelist;

		private bool m_usesTargetingList;

		private Action m_convertToShipResult;

		private long m_closestParentId;

		public bool ForceDisablePrediction;

		private Action m_pendingGridReleases;

		private Action<MatrixD> m_updateMergingGrids;

		private const double GRID_PLACING_AREA_FIX_VALUE = 0.11;

		private const string EXPORT_DIRECTORY = "ExportedModels";

		private const string SOURCE_DIRECTORY = "SourceModels";

		private static readonly List<MyObjectBuilder_CubeGrid[]> m_prefabs;

		[ThreadStatic]
		private static List<MyEntity> m_tmpResultListPerThread;

		private static readonly List<MyVoxelBase> m_tmpVoxelList;

		private static int materialID;

		private static Vector2 tumbnailMultiplier;

		private static float m_maxDimensionPreviousRow;

		private static Vector3D m_newPositionForPlacedObject;

		private const int m_numRowsForPlacedObjects = 4;

		private static List<MyLineSegmentOverlapResult<MyEntity>> m_lineOverlapList;

		[ThreadStatic]
		private static List<HkBodyCollision> m_physicsBoxQueryListPerThread;

		[ThreadStatic]
		private static Dictionary<Vector3I, MySlimBlock> m_tmpCubeSet;

		private static readonly MyDisconnectHelper m_disconnectHelper;

		private static readonly List<NeighborOffsetIndex> m_neighborOffsetIndices;

		private static readonly List<float> m_neighborDistances;

		private static readonly List<Vector3I> m_neighborOffsets;

		[ThreadStatic]
		private static MyRandom m_deformationRng;

		[ThreadStatic]
		private static List<Vector3I> m_cacheRayCastCellsPerThread;

		[ThreadStatic]
		private static Dictionary<Vector3I, ConnectivityResult> m_cacheNeighborBlocksPerThread;

		[ThreadStatic]
		private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsAPerThread;

		[ThreadStatic]
		private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsBPerThread;

		private static readonly MyComponentList m_buildComponents;

		[ThreadStatic]
		private static List<MyPhysics.HitInfo> m_tmpHitListPerThread;

		private static readonly HashSet<Vector3UByte> m_tmpAreaMountpointPass;

		private static readonly AreaConnectivityTest m_areaOverlapTest;

		[ThreadStatic]
		private static List<Vector3I> m_tmpCubeNeighboursPerThread;

		private static readonly HashSet<Tuple<MySlimBlock, ushort?>> m_tmpBlocksInMultiBlock;

		private static readonly List<MySlimBlock> m_tmpSlimBlocks;

		[ThreadStatic]
		private static List<int> m_tmpMultiBlockIndicesPerThread;

		private static readonly Type m_gridSystemsType;

		private static readonly List<Tuple<Vector3I, ushort>> m_tmpRazeList;

		private static readonly List<Vector3I> m_tmpLocations;

		[ThreadStatic]
		private static Ref<HkBoxShape> m_lastQueryBoxPerThread;

		[ThreadStatic]
		private static MatrixD m_lastQueryTransform;

		private const double ROTATION_PRECISION = 0.0010000000474974513;

		public HashSet<MyCubeBlock> Inventories => m_inventoryBlocks;

		public HashSetReader<MyCubeBlock> UnsafeBlocks => m_unsafeBlocks;

		public HashSetReader<MyDecoy> Decoys => m_decoys;

		public HashSetReader<MyCockpit> OccupiedBlocks => m_occupiedBlocks;

		public SyncType SyncType
		{
			get;
			set;
		}

		public bool IsPowered => m_IsPowered;

		public int NumberOfGridColors => m_colorStatistics.Count;

		public ConcurrentCachingHashSet<Vector3I> DirtyBlocks
		{
			get
			{
				m_dirtyRegion.Cubes.ApplyChanges();
				return m_dirtyRegion.Cubes;
			}
		}

		public MyCubeGridRenderData RenderData => Render.RenderData;

		public HashSet<MyCubeBlock> BlocksForDraw => m_blocksForDraw;

		public bool IsSplit
		{
			get;
			set;
		}

		private static List<MyCockpit> m_tmpOccupiedCockpits => MyUtils.Init(ref m_tmpOccupiedCockpitsPerThread);

		private static List<MyObjectBuilder_BlockGroup> m_tmpBlockGroups => MyUtils.Init(ref m_tmpBlockGroupsPerThread);

		public List<IMyBlockAdditionalModelGenerator> AdditionalModelGenerators => Render.AdditionalModelGenerators;

		public MyCubeGridSystems GridSystems
		{
			get;
			private set;
		}

		public bool IsStatic
		{
			get
			{
				return m_isStatic;
			}
			private set
			{
				if (m_isStatic != value)
				{
					m_isStatic = value;
					NotifyIsStaticChanged(m_isStatic);
				}
			}
		}

		public bool DampenersEnabled => m_dampenersEnabled;

		public bool MarkedAsTrash => m_markedAsTrash;

		public bool IsUnsupportedStation
		{
			get;
			private set;
		}

		public float GridSize
		{
			get;
			private set;
		}

		public float GridScale
		{
			get;
			private set;
		}

		public float GridSizeHalf
		{
			get;
			private set;
		}

		public Vector3 GridSizeHalfVector
		{
			get;
			private set;
		}

		public float GridSizeQuarter
		{
			get;
			private set;
		}

		public Vector3 GridSizeQuarterVector
		{
			get;
			private set;
		}

		public float GridSizeR
		{
			get;
			private set;
		}

		public Vector3I Min => m_min;

		public Vector3I Max => m_max;

		public bool IsRespawnGrid
		{
			get
			{
				return m_isRespawnGrid;
			}
			set
			{
				m_isRespawnGrid.Value = value;
			}
		}

		public bool DestructibleBlocks
		{
			get
			{
				return m_destructibleBlocks;
			}
			set
			{
				m_destructibleBlocks.Value = value;
			}
		}

		public bool Editable
		{
			get
			{
				return m_editable;
			}
			set
			{
				m_editable.ValidateAndSet(value);
			}
		}

		public bool BlocksDestructionEnabled
		{
			get
			{
				if (MySession.Static.Settings.DestructibleBlocks)
				{
					return m_destructibleBlocks;
				}
				return false;
			}
		}

		public List<long> SmallOwners => m_ownershipManager.SmallOwners;

		public List<long> BigOwners
		{
			get
			{
				List<long> bigOwners = m_ownershipManager.BigOwners;
				if (bigOwners.Count == 0)
				{
					MyCubeGrid parent = MyGridPhysicalHierarchy.Static.GetParent(this);
					if (parent != null)
					{
						bigOwners = parent.BigOwners;
					}
				}
				return bigOwners;
			}
		}

		public MyCubeSize GridSizeEnum
		{
			get
			{
				return m_gridSizeEnum;
			}
			set
			{
				m_gridSizeEnum = value;
				GridSize = MyDefinitionManager.Static.GetCubeSize(value);
				GridSizeHalf = GridSize / 2f;
				GridSizeHalfVector = new Vector3(GridSizeHalf);
				GridSizeQuarter = GridSize / 4f;
				GridSizeQuarterVector = new Vector3(GridSizeQuarter);
				GridSizeR = 1f / GridSize;
			}
		}

		public new MyGridPhysics Physics
		{
			get
			{
				return (MyGridPhysics)base.Physics;
			}
			set
			{
				base.Physics = value;
			}
		}

		public int ShapeCount
		{
			get
			{
				if (Physics == null)
				{
					return 0;
				}
				return Physics.Shape.ShapeCount;
			}
		}

		public MyEntityThrustComponent EntityThrustComponent => base.Components.Get<MyEntityThrustComponent>();

		public bool IsMarkedForEarlyDeactivation
		{
			get
			{
				return m_isMarkedForEarlyDeactivation;
			}
			set
			{
				if (m_isMarkedForEarlyDeactivation != value)
				{
					m_isMarkedForEarlyDeactivation = value;
					MarkForUpdate();
				}
			}
		}

		public bool IsBlockTrasferInProgress
		{
			get;
			private set;
		}

		public float Mass
		{
			get
			{
				if (Physics == null)
				{
					return 0f;
				}
				if (!Sync.IsServer && IsStatic && Physics != null && Physics.Shape != null)
				{
					if (!Physics.Shape.MassProperties.HasValue)
					{
						return 0f;
					}
					return Physics.Shape.MassProperties.Value.Mass;
				}
				return Physics.Mass;
			}
		}

		public static int GridCounter
		{
			get;
			private set;
		}

		public int BlocksCount => m_cubeBlocks.Count;

		public int BlocksPCU
		{
			get
			{
				return m_PCU;
			}
			set
			{
				m_PCU = value;
			}
		}

		public HashSet<MySlimBlock> CubeBlocks => m_cubeBlocks;

		internal bool SmallToLargeConnectionsInitialized => m_smallToLargeConnectionsInitialized;

		internal bool EnableSmallToLargeConnections => m_enableSmallToLargeConnections;

		internal MyTestDynamicReason TestDynamic
		{
			get
			{
				return m_testDynamic;
			}
			set
			{
				if (m_testDynamic != value)
				{
					m_testDynamic = value;
					MarkForUpdate();
				}
			}
		}

		internal new MyRenderComponentCubeGrid Render
		{
			get
			{
				return (MyRenderComponentCubeGrid)base.Render;
			}
			set
			{
				base.Render = value;
			}
		}

		public long LocalCoordSystem
		{
			get;
			set;
		}

		internal bool NeedsPerFrameUpdate
		{
			get
			{
				if (!MyFakes.OPTIMIZE_GRID_UPDATES)
				{
					return true;
				}
				bool flag = MyPhysicsConfig.EnableGridSpeedDebugDraw || m_inventoryMassDirty || m_hasAdditionalModelGenerators || m_blocksForDamageApplicationDirty || m_disconnectsDirty != 0 || m_resolvingSplits > 0 || Skeleton.NeedsPerFrameUpdate || m_ownershipManager.NeedRecalculateOwners || (Physics != null && Physics.NeedsPerFrameUpdate) || MySessionComponentReplay.Static.IsEntityBeingReplayed(base.EntityId) || (Physics != null && Physics.IsDirty()) || m_deformationPostponed.Count > 0 || m_removeBlockQueueWithGenerators.Count > 0 || m_destroyBlockQueue.Count > 0 || m_destructionDeformationQueue.Count > 0 || m_removeBlockQueueWithoutGenerators.Count > 0 || m_removeBlockWithIdQueueWithGenerators.Count > 0 || m_removeBlockWithIdQueueWithoutGenerators.Count > 0 || m_destroyBlockWithIdQueueWithGenerators.Count > 0 || m_destroyBlockWithIdQueueWithoutGenerators.Count > 0 || TestDynamic != 0 || BonesToSend.InputCount > 0 || m_updateMergingGrids != null || MySessionComponentReplay.Static.HasEntityReplayData(base.EntityId) || MarkedAsTrash || (MyFakes.ENABLE_GRID_SYSTEM_UPDATE && GridSystems != null && GridSystems.NeedsPerFrameUpdate);
				flag |= IsDirty();
				if (!flag && (Sync.IsServer || IsClientPredicted))
				{
					flag |= (Physics != null && ((IsMarkedForEarlyDeactivation && !Physics.IsStatic) || (!IsMarkedForEarlyDeactivation && !IsStatic && Physics.IsStatic)));
				}
				return flag;
			}
		}

		internal bool NeedsPerFrameDraw
		{
			get
			{
				if (!MyFakes.OPTIMIZE_GRID_UPDATES)
				{
					return true;
				}
				return (byte)(0 | (IsDirty() ? 1 : 0) | ((ShowCenterOfMass || ShowGridPivot || ShowSenzorGizmos || ShowGravityGizmos || ShowAntennaGizmos) ? 1 : 0) | ((MyFakes.ENABLE_GRID_SYSTEM_UPDATE && GridSystems.NeedsPerFrameDraw) ? 1 : 0) | ((BlocksForDraw.Count > 0) ? 1 : 0) | (MarkedAsTrash ? 1 : 0)) != 0;
			}
		}

		public bool IsLargeDestroyInProgress
		{
			get
			{
				if (m_destroyBlockQueue.Count <= BLOCK_LIMIT_FOR_LARGE_DESTRUCTION)
				{
					return m_largeDestroyInProgress;
				}
				return true;
			}
		}

		public bool UsesTargetingList => m_usesTargetingList;

		public long ClosestParentId
		{
			get
			{
				return m_closestParentId;
			}
			set
			{
				if (m_closestParentId != value)
				{
					if (MyEntities.TryGetEntityById(m_closestParentId, out MyCubeGrid entity, allowClosed: true))
					{
						MyGridPhysicalHierarchy.Static.RemoveNonGridNode(entity, this);
					}
					if (MyEntities.TryGetEntityById(value, out entity))
					{
						m_closestParentId = value;
						MyGridPhysicalHierarchy.Static.AddNonGridNode(entity, this);
					}
					else
					{
						m_closestParentId = 0L;
					}
				}
			}
		}

		public bool IsClientPredicted
		{
			get;
			private set;
		}

		public bool IsClientPredictedWheel
		{
			get;
			private set;
		}

		public bool IsClientPredictedCar
		{
			get;
			private set;
		}

		public int TrashHighlightCounter => m_trashHighlightCounter;

		private static List<MyEntity> m_tmpResultList => MyUtils.Init(ref m_tmpResultListPerThread);

		public static bool ShowSenzorGizmos
		{
			get;
			set;
		}

		public static bool ShowGravityGizmos
		{
			get;
			set;
		}

		public static bool ShowAntennaGizmos
		{
			get;
			set;
		}

		public static bool ShowCenterOfMass
		{
			get;
			set;
		}

		public static bool ShowGridPivot
		{
			get;
			set;
		}

		private static List<HkBodyCollision> m_physicsBoxQueryList => MyUtils.Init(ref m_physicsBoxQueryListPerThread);

		private static List<Vector3I> m_cacheRayCastCells => MyUtils.Init(ref m_cacheRayCastCellsPerThread);

		private static Dictionary<Vector3I, ConnectivityResult> m_cacheNeighborBlocks => MyUtils.Init(ref m_cacheNeighborBlocksPerThread);

		private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsA => MyUtils.Init(ref m_cacheMountPointsAPerThread);

		private static List<MyCubeBlockDefinition.MountPoint> m_cacheMountPointsB => MyUtils.Init(ref m_cacheMountPointsBPerThread);

		private static List<MyPhysics.HitInfo> m_tmpHitList => MyUtils.Init(ref m_tmpHitListPerThread);

		private static List<Vector3I> m_tmpCubeNeighbours => MyUtils.Init(ref m_tmpCubeNeighboursPerThread);

		private static List<int> m_tmpMultiBlockIndices => MyUtils.Init(ref m_tmpMultiBlockIndicesPerThread);

		private static Ref<HkBoxShape> m_lastQueryBox
		{
			get
			{
				if (m_lastQueryBoxPerThread == null)
				{
					m_lastQueryBoxPerThread = new Ref<HkBoxShape>();
					m_lastQueryBoxPerThread.Value = new HkBoxShape(Vector3.One);
				}
				return m_lastQueryBoxPerThread;
			}
		}

		string VRage.Game.ModAPI.Ingame.IMyCubeGrid.CustomName
		{
			get
			{
				return base.DisplayName;
			}
			set
			{
				if (IsAccessibleForProgrammableBlock)
				{
					ChangeDisplayNameRequest(value);
				}
			}
		}

		string VRage.Game.ModAPI.IMyCubeGrid.CustomName
		{
			get
			{
				return base.DisplayName;
			}
			set
			{
				ChangeDisplayNameRequest(value);
			}
		}

		List<long> VRage.Game.ModAPI.IMyCubeGrid.BigOwners => BigOwners;

		List<long> VRage.Game.ModAPI.IMyCubeGrid.SmallOwners => SmallOwners;

		bool VRage.Game.ModAPI.IMyCubeGrid.IsRespawnGrid
		{
			get
			{
				return IsRespawnGrid;
			}
			set
			{
				IsRespawnGrid = value;
			}
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.IsStatic
		{
			get
			{
				return IsStatic;
			}
			set
			{
				if (value)
				{
					RequestConversionToStation();
				}
				else
				{
					RequestConversionToShip(null);
				}
			}
		}

		Vector3I? VRage.Game.ModAPI.IMyCubeGrid.XSymmetryPlane
		{
			get
			{
				return XSymmetryPlane;
			}
			set
			{
				XSymmetryPlane = value;
			}
		}

		Vector3I? VRage.Game.ModAPI.IMyCubeGrid.YSymmetryPlane
		{
			get
			{
				return YSymmetryPlane;
			}
			set
			{
				YSymmetryPlane = value;
			}
		}

		Vector3I? VRage.Game.ModAPI.IMyCubeGrid.ZSymmetryPlane
		{
			get
			{
				return ZSymmetryPlane;
			}
			set
			{
				ZSymmetryPlane = value;
			}
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.XSymmetryOdd
		{
			get
			{
				return XSymmetryOdd;
			}
			set
			{
				XSymmetryOdd = value;
			}
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.YSymmetryOdd
		{
			get
			{
				return YSymmetryOdd;
			}
			set
			{
				YSymmetryOdd = value;
			}
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.ZSymmetryOdd
		{
			get
			{
				return ZSymmetryOdd;
			}
			set
			{
				ZSymmetryOdd = value;
			}
		}

		public event Action<SyncBase> SyncPropertyChanged
		{
			add
			{
				SyncType.PropertyChanged += value;
			}
			remove
			{
				SyncType.PropertyChanged -= value;
			}
		}

		public event Action<MySlimBlock> OnBlockAdded;

		public event Action<MyCubeBlock> OnFatBlockAdded;

		public event Action<MySlimBlock> OnBlockRemoved;

		public event Action<MyCubeBlock> OnFatBlockRemoved;

		public event Action<MySlimBlock> OnBlockIntegrityChanged;

		public event Action<MySlimBlock> OnBlockClosed;

		public event Action<MyCubeBlock> OnFatBlockClosed;

		public event Action<MyCubeGrid> OnMassPropertiesChanged;

		public static event Action<MyCubeGrid> OnSplitGridCreated;

		public event Action<MyCubeGrid> OnBlockOwnershipChanged;

		[Obsolete("Use OnStaticChanged")]
		public event Action<bool> OnIsStaticChanged;

		public event Action<MyCubeGrid, bool> OnStaticChanged;

		public event Action<MyCubeGrid, MyCubeGrid> OnGridSplit;

		public event Action<MyCubeGrid> OnHierarchyUpdated;

		internal event Action<MyGridLogicalGroupData> AddedToLogicalGroup;

		internal event Action RemovedFromLogicalGroup;

		public event Action<int> OnHavokSystemIDChanged;

		public event Action<MyCubeGrid> OnNameChanged;

		public event Action<MyCubeGrid> OnGridChanged;

		event Action<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.OnBlockAdded
		{
			add
			{
				OnBlockAdded += GetDelegate(value);
			}
			remove
			{
				OnBlockAdded -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.OnBlockRemoved
		{
			add
			{
				OnBlockRemoved += GetDelegate(value);
			}
			remove
			{
				OnBlockRemoved -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnBlockOwnershipChanged
		{
			add
			{
				OnBlockOwnershipChanged += GetDelegate(value);
			}
			remove
			{
				OnBlockOwnershipChanged -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnGridChanged
		{
			add
			{
				OnGridChanged += GetDelegate(value);
			}
			remove
			{
				OnGridChanged -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.IMyCubeGrid> VRage.Game.ModAPI.IMyCubeGrid.OnGridSplit
		{
			add
			{
				OnGridSplit += GetDelegate(value);
			}
			remove
			{
				OnGridSplit -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMyCubeGrid, bool> VRage.Game.ModAPI.IMyCubeGrid.OnIsStaticChanged
		{
			add
			{
				OnStaticChanged += GetDelegate(value);
			}
			remove
			{
				OnStaticChanged -= GetDelegate(value);
			}
		}

		event Action<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.OnBlockIntegrityChanged
		{
			add
			{
				OnBlockIntegrityChanged += GetDelegate(value);
			}
			remove
			{
				OnBlockIntegrityChanged -= GetDelegate(value);
			}
		}

		static MyCubeGrid()
		{
			BLOCK_LIMIT_FOR_LARGE_DESTRUCTION = 3;
			TRASH_HIGHLIGHT = 300;
			m_tmpBuildList = new HashSet<MyBlockLocation>();
			m_tmpPositionListReceive = new List<Vector3I>();
			m_tmpPositionListSend = new List<Vector3I>();
			m_recievedGrids = new List<MyObjectBuilder_CubeGrid>();
			m_precalculatedCornerBonesDisplacementDistance = 0f;
			m_tmpBlockPositions = new List<Vector3I>();
			m_tmpBlockListReceive = new List<MySlimBlock>();
			m_tmpSinks = new HashSet<MyResourceSinkComponent>();
			m_tmpLocationsAndIdsSend = new List<LocationIdentity>();
			m_tmpLocationsAndIdsReceive = new List<Tuple<Vector3I, ushort>>();
			m_notificationQueue = new ConcurrentQueue<MyTuple<int, MyVoxelBase, Vector3I, Vector3I>>();
			m_postponedListsPool = new MyConcurrentPool<List<DeformationPostponedItem>>();
			m_tmpBlockSurroundingOffsets = new Vector3I[27]
			{
				new Vector3I(0, 0, 0),
				new Vector3I(1, 0, 0),
				new Vector3I(-1, 0, 0),
				new Vector3I(0, 0, 1),
				new Vector3I(0, 0, -1),
				new Vector3I(1, 0, 1),
				new Vector3I(-1, 0, 1),
				new Vector3I(1, 0, -1),
				new Vector3I(-1, 0, -1),
				new Vector3I(0, 1, 0),
				new Vector3I(1, 1, 0),
				new Vector3I(-1, 1, 0),
				new Vector3I(0, 1, 1),
				new Vector3I(0, 1, -1),
				new Vector3I(1, 1, 1),
				new Vector3I(-1, 1, 1),
				new Vector3I(1, 1, -1),
				new Vector3I(-1, 1, -1),
				new Vector3I(0, -1, 0),
				new Vector3I(1, -1, 0),
				new Vector3I(-1, -1, 0),
				new Vector3I(0, -1, 1),
				new Vector3I(0, -1, -1),
				new Vector3I(1, -1, 1),
				new Vector3I(-1, -1, 1),
				new Vector3I(1, -1, -1),
				new Vector3I(-1, -1, -1)
			};
			m_prefabs = new List<MyObjectBuilder_CubeGrid[]>();
			m_tmpVoxelList = new List<MyVoxelBase>();
			materialID = 0;
			tumbnailMultiplier = default(Vector2);
			m_maxDimensionPreviousRow = 0f;
			m_newPositionForPlacedObject = new Vector3D(0.0, 0.0, 0.0);
			m_lineOverlapList = new List<MyLineSegmentOverlapResult<MyEntity>>();
			m_tmpCubeSet = new Dictionary<Vector3I, MySlimBlock>(Vector3I.Comparer);
			m_disconnectHelper = new MyDisconnectHelper();
			m_neighborOffsetIndices = new List<NeighborOffsetIndex>(26);
			m_neighborDistances = new List<float>(26);
			m_neighborOffsets = new List<Vector3I>(26);
			m_buildComponents = new MyComponentList();
			m_tmpAreaMountpointPass = new HashSet<Vector3UByte>();
			m_areaOverlapTest = new AreaConnectivityTest();
			m_tmpBlocksInMultiBlock = new HashSet<Tuple<MySlimBlock, ushort?>>();
			m_tmpSlimBlocks = new List<MySlimBlock>();
			m_gridSystemsType = ChooseGridSystemsType();
			m_tmpRazeList = new List<Tuple<Vector3I, ushort>>();
			m_tmpLocations = new List<Vector3I>();
			for (int i = 0; i < 26; i++)
			{
				m_neighborOffsetIndices.Add((NeighborOffsetIndex)i);
				m_neighborDistances.Add(0f);
				m_neighborOffsets.Add(new Vector3I(0, 0, 0));
			}
			m_neighborOffsets[0] = new Vector3I(1, 0, 0);
			m_neighborOffsets[1] = new Vector3I(-1, 0, 0);
			m_neighborOffsets[2] = new Vector3I(0, 1, 0);
			m_neighborOffsets[3] = new Vector3I(0, -1, 0);
			m_neighborOffsets[4] = new Vector3I(0, 0, 1);
			m_neighborOffsets[5] = new Vector3I(0, 0, -1);
			m_neighborOffsets[6] = new Vector3I(1, 1, 0);
			m_neighborOffsets[7] = new Vector3I(1, -1, 0);
			m_neighborOffsets[8] = new Vector3I(-1, 1, 0);
			m_neighborOffsets[9] = new Vector3I(-1, -1, 0);
			m_neighborOffsets[10] = new Vector3I(0, 1, 1);
			m_neighborOffsets[11] = new Vector3I(0, 1, -1);
			m_neighborOffsets[12] = new Vector3I(0, -1, 1);
			m_neighborOffsets[13] = new Vector3I(0, -1, -1);
			m_neighborOffsets[14] = new Vector3I(1, 0, 1);
			m_neighborOffsets[15] = new Vector3I(1, 0, -1);
			m_neighborOffsets[16] = new Vector3I(-1, 0, 1);
			m_neighborOffsets[17] = new Vector3I(-1, 0, -1);
			m_neighborOffsets[18] = new Vector3I(1, 1, 1);
			m_neighborOffsets[19] = new Vector3I(1, 1, -1);
			m_neighborOffsets[20] = new Vector3I(1, -1, 1);
			m_neighborOffsets[21] = new Vector3I(1, -1, -1);
			m_neighborOffsets[22] = new Vector3I(-1, 1, 1);
			m_neighborOffsets[23] = new Vector3I(-1, 1, -1);
			m_neighborOffsets[24] = new Vector3I(-1, -1, 1);
			m_neighborOffsets[25] = new Vector3I(-1, -1, -1);
			GridCounter = 0;
		}

		public bool SwitchPower()
		{
			m_IsPowered = !m_IsPowered;
			return m_IsPowered;
		}

		internal void NotifyMassPropertiesChanged()
		{
			this.OnMassPropertiesChanged.InvokeIfNotNull(this);
		}

		internal void NotifyBlockAdded(MySlimBlock block)
		{
			this.OnBlockAdded.InvokeIfNotNull(block);
			if (block.FatBlock != null)
			{
				this.OnFatBlockAdded.InvokeIfNotNull(block.FatBlock);
			}
			GridSystems.OnBlockAdded(block);
		}

		internal void NotifyBlockRemoved(MySlimBlock block)
		{
			this.OnBlockRemoved.InvokeIfNotNull(block);
			if (block.FatBlock != null)
			{
				this.OnFatBlockRemoved.InvokeIfNotNull(block.FatBlock);
			}
			if (MyVisualScriptLogicProvider.BlockDestroyed != null)
			{
				SingleKeyEntityNameGridNameEvent blockDestroyed = MyVisualScriptLogicProvider.BlockDestroyed;
				string entityName = (block.FatBlock != null) ? block.FatBlock.Name : string.Empty;
				string name = Name;
				MyObjectBuilderType typeId = block.BlockDefinition.Id.TypeId;
				blockDestroyed(entityName, name, typeId.ToString(), block.BlockDefinition.Id.SubtypeName);
			}
			MyCubeGrids.NotifyBlockDestroyed(this, block);
			GridSystems.OnBlockRemoved(block);
			MarkForUpdate();
		}

		internal void NotifyBlockClosed(MySlimBlock block)
		{
			this.OnBlockClosed.InvokeIfNotNull(block);
			if (block.FatBlock != null)
			{
				this.OnFatBlockClosed.InvokeIfNotNull(block.FatBlock);
			}
		}

		internal void NotifyBlockIntegrityChanged(MySlimBlock block, bool handWelded)
		{
			this.OnBlockIntegrityChanged.InvokeIfNotNull(block);
			GridSystems.OnBlockIntegrityChanged(block);
			if (block.IsFullIntegrity)
			{
				MyCubeGrids.NotifyBlockFinished(this, block, handWelded);
			}
		}

		internal void NotifyBlockOwnershipChange(MyCubeGrid cubeGrid)
		{
			if (this.OnBlockOwnershipChanged != null)
			{
				this.OnBlockOwnershipChanged(cubeGrid);
			}
			GridSystems.OnBlockOwnershipChanged(cubeGrid);
		}

		internal void NotifyIsStaticChanged(bool newIsStatic)
		{
			if (this.OnIsStaticChanged != null)
			{
				this.OnIsStaticChanged(newIsStatic);
			}
			if (this.OnStaticChanged != null)
			{
				this.OnStaticChanged(this, newIsStatic);
			}
		}

		public void RaiseGridChanged()
		{
			this.OnGridChanged.InvokeIfNotNull(this);
		}

		public void OnTerminalOpened()
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnGridChangedRPC);
		}

		[Event(null, 668)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnGridChangedRPC()
		{
			RaiseGridChanged();
		}

		public bool HasMainCockpit()
		{
			return MainCockpit != null;
		}

		public bool IsMainCockpit(MyTerminalBlock cockpit)
		{
			return MainCockpit == cockpit;
		}

		public void SetMainCockpit(MyTerminalBlock cockpit)
		{
			MainCockpit = cockpit;
		}

		public bool HasMainRemoteControl()
		{
			return MainRemoteControl != null;
		}

		public bool IsMainRemoteControl(MyTerminalBlock remoteControl)
		{
			return MainRemoteControl == remoteControl;
		}

		public void SetMainRemoteControl(MyTerminalBlock remoteControl)
		{
			MainRemoteControl = remoteControl;
		}

		public int GetFatBlockCount<T>() where T : MyCubeBlock
		{
			int num = 0;
			foreach (MyCubeBlock fatBlock in GetFatBlocks())
			{
				if (fatBlock is T)
				{
					num++;
				}
			}
			return num;
		}

		public MyCubeGrid()
			: this(MyCubeSize.Large)
		{
			GridScale = 1f;
			Render = new MyRenderComponentCubeGrid();
			Render.NeedsDraw = true;
			base.PositionComp = new MyCubeGridPosition();
			IsUnsupportedStation = false;
			base.Hierarchy.QueryAABBImpl = QueryAABB;
			base.Hierarchy.QuerySphereImpl = QuerySphere;
			base.Hierarchy.QueryLineImpl = QueryLine;
			base.Components.Add(new MyGridTargeting());
			SyncType = SyncHelpers.Compose(this);
			m_handBrakeSync.ValueChanged += delegate
			{
				HandBrakeChanged();
			};
			m_dampenersEnabled.ValueChanged += delegate
			{
				DampenersEnabledChanged();
			};
			m_contactPoint.ValueChanged += delegate
			{
				OnContactPointChanged();
			};
			m_markedAsTrash.ValueChanged += delegate
			{
				MarkedAsTrashChanged();
			};
			m_UpdateDirtyInternal = UpdateDirtyInternal;
			m_OnUpdateDirtyCompleted = OnUpdateDirtyCompleted;
		}

		private MyCubeGrid(MyCubeSize gridSize)
		{
			GridScale = 1f;
			GridSizeEnum = gridSize;
			GridSize = MyDefinitionManager.Static.GetCubeSize(gridSize);
			GridSizeHalf = GridSize / 2f;
			GridSizeHalfVector = new Vector3(GridSizeHalf);
			GridSizeQuarter = GridSize / 4f;
			GridSizeQuarterVector = new Vector3(GridSizeQuarter);
			GridSizeR = 1f / GridSize;
			base.NeedsUpdate = (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
			Skeleton = new MyGridSkeleton();
			GridCounter++;
			AddDebugRenderComponent(new MyDebugRenderComponentCubeGrid(this));
			if (MyPerGameSettings.Destruction)
			{
				base.OnPhysicsChanged += delegate(MyEntity entity)
				{
					MyPhysics.RemoveDestructions(entity);
				};
			}
			if (MyFakes.ASSERT_CHANGES_IN_SIMULATION)
			{
				base.OnPhysicsChanged += delegate
				{
				};
				OnGridSplit += delegate
				{
				};
			}
		}

		private void CreateSystems()
		{
			GridSystems = (MyCubeGridSystems)Activator.CreateInstance(m_gridSystemsType, this);
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			InitInternal(objectBuilder, rebuildGrid: true);
		}

		[Conditional("DEBUG")]
		private void AssertNonPublicBlock(MyObjectBuilder_CubeBlock block)
		{
			MyCubeBlockDefinition blockDefinition;
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = UpgradeCubeBlock(block, out blockDefinition) as MyObjectBuilder_CompoundCubeBlock;
			if (myObjectBuilder_CompoundCubeBlock != null)
			{
				MyObjectBuilder_CubeBlock[] blocks = myObjectBuilder_CompoundCubeBlock.Blocks;
				for (int i = 0; i < blocks.Length; i++)
				{
					_ = blocks[i];
				}
			}
		}

		[Conditional("DEBUG")]
		private void AssertNonPublicBlocks(MyObjectBuilder_CubeGrid builder)
		{
			foreach (MyObjectBuilder_CubeBlock cubeBlock in builder.CubeBlocks)
			{
				_ = cubeBlock;
			}
		}

		private bool RemoveNonPublicBlock(MyObjectBuilder_CubeBlock block)
		{
			MyCubeBlockDefinition blockDefinition;
			MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = UpgradeCubeBlock(block, out blockDefinition) as MyObjectBuilder_CompoundCubeBlock;
			if (myObjectBuilder_CompoundCubeBlock != null)
			{
				myObjectBuilder_CompoundCubeBlock.Blocks = myObjectBuilder_CompoundCubeBlock.Blocks.Where((MyObjectBuilder_CubeBlock s) => !MyDefinitionManager.Static.TryGetCubeBlockDefinition(s.GetId(), out MyCubeBlockDefinition def) || def.Public || def.IsGeneratedBlock).ToArray();
				return myObjectBuilder_CompoundCubeBlock.Blocks.Length == 0;
			}
			if (blockDefinition != null && !blockDefinition.Public)
			{
				return true;
			}
			return false;
		}

		private void RemoveNonPublicBlocks(MyObjectBuilder_CubeGrid builder)
		{
			builder.CubeBlocks.RemoveAll((MyObjectBuilder_CubeBlock s) => RemoveNonPublicBlock(s));
		}

		private void InitInternal(MyObjectBuilder_EntityBase objectBuilder, bool rebuildGrid)
		{
			List<MyDefinitionId> list = new List<MyDefinitionId>();
			base.SyncFlag = true;
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (MyObjectBuilder_CubeGrid)objectBuilder;
			if (myObjectBuilder_CubeGrid != null)
			{
				GridSizeEnum = myObjectBuilder_CubeGrid.GridSizeEnum;
			}
			GridScale = MyDefinitionManager.Static.GetCubeSize(GridSizeEnum) / MyDefinitionManager.Static.GetCubeSizeOriginal(GridSizeEnum);
			base.Init(objectBuilder);
			Init(null, null, null, null);
			m_destructibleBlocks.SetLocalValue(myObjectBuilder_CubeGrid.DestructibleBlocks);
			_ = MyFakes.ASSERT_NON_PUBLIC_BLOCKS;
			if (MyFakes.REMOVE_NON_PUBLIC_BLOCKS)
			{
				RemoveNonPublicBlocks(myObjectBuilder_CubeGrid);
			}
			Render.CreateAdditionalModelGenerators(myObjectBuilder_CubeGrid?.GridSizeEnum ?? MyCubeSize.Large);
			m_hasAdditionalModelGenerators = (AdditionalModelGenerators.Count > 0);
			CreateSystems();
			if (myObjectBuilder_CubeGrid != null)
			{
				IsStatic = myObjectBuilder_CubeGrid.IsStatic;
				IsUnsupportedStation = myObjectBuilder_CubeGrid.IsUnsupportedStation;
				CreatePhysics = myObjectBuilder_CubeGrid.CreatePhysics;
				m_enableSmallToLargeConnections = myObjectBuilder_CubeGrid.EnableSmallToLargeConnections;
				GridSizeEnum = myObjectBuilder_CubeGrid.GridSizeEnum;
				Editable = myObjectBuilder_CubeGrid.Editable;
				m_IsPowered = myObjectBuilder_CubeGrid.IsPowered;
				GridSystems.BeforeBlockDeserialization(myObjectBuilder_CubeGrid);
				m_cubes.Clear();
				m_cubeBlocks.Clear();
				m_fatBlocks.Clear();
				m_inventoryBlocks.Clear();
				if (myObjectBuilder_CubeGrid.DisplayName == null)
				{
					base.DisplayName = MakeCustomName();
				}
				else
				{
					base.DisplayName = myObjectBuilder_CubeGrid.DisplayName;
				}
				m_tmpOccupiedCockpits.Clear();
				for (int i = 0; i < myObjectBuilder_CubeGrid.CubeBlocks.Count; i++)
				{
					MyObjectBuilder_CubeBlock objectBuilder2 = myObjectBuilder_CubeGrid.CubeBlocks[i];
					MySlimBlock mySlimBlock = AddBlock(objectBuilder2, testMerge: false);
					if (mySlimBlock == null)
					{
						continue;
					}
					if (mySlimBlock.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block in (mySlimBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
							if (!list.Contains(block.BlockDefinition.Id))
							{
								list.Add(block.BlockDefinition.Id);
							}
						}
					}
					else if (!list.Contains(mySlimBlock.BlockDefinition.Id))
					{
						list.Add(mySlimBlock.BlockDefinition.Id);
					}
					if (mySlimBlock.FatBlock is MyCockpit)
					{
						MyCockpit myCockpit = mySlimBlock.FatBlock as MyCockpit;
						if (myCockpit.Pilot != null)
						{
							m_tmpOccupiedCockpits.Add(myCockpit);
						}
					}
				}
				GridSystems.AfterBlockDeserialization();
				if (myObjectBuilder_CubeGrid.Skeleton != null)
				{
					Skeleton.Deserialize(myObjectBuilder_CubeGrid.Skeleton, GridSize, GridSize);
				}
				Render.RenderData.SetBasePositionHint(Min * GridSize - GridSize);
				if (rebuildGrid)
				{
					RebuildGrid();
				}
				foreach (MyObjectBuilder_BlockGroup blockGroup in myObjectBuilder_CubeGrid.BlockGroups)
				{
					AddGroup(blockGroup);
				}
				if (Physics != null)
				{
					Vector3 vector = myObjectBuilder_CubeGrid.LinearVelocity;
					Vector3 vector2 = myObjectBuilder_CubeGrid.AngularVelocity;
					Vector3.ClampToSphere(ref vector, Physics.GetMaxRelaxedLinearVelocity());
					Vector3.ClampToSphere(ref vector2, Physics.GetMaxRelaxedAngularVelocity());
					Physics.LinearVelocity = vector;
					Physics.AngularVelocity = vector2;
					if (!IsStatic)
					{
						Physics.Shape.BlocksConnectedToWorld.Clear();
					}
					if (MyPerGameSettings.InventoryMass)
					{
						m_inventoryMassDirty = true;
					}
				}
				XSymmetryPlane = myObjectBuilder_CubeGrid.XMirroxPlane;
				YSymmetryPlane = myObjectBuilder_CubeGrid.YMirroxPlane;
				ZSymmetryPlane = myObjectBuilder_CubeGrid.ZMirroxPlane;
				XSymmetryOdd = myObjectBuilder_CubeGrid.XMirroxOdd;
				YSymmetryOdd = myObjectBuilder_CubeGrid.YMirroxOdd;
				ZSymmetryOdd = myObjectBuilder_CubeGrid.ZMirroxOdd;
				GridSystems.Init(myObjectBuilder_CubeGrid);
				if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
				{
					m_ownershipManager = new MyCubeGridOwnershipManager();
					m_ownershipManager.Init(this);
				}
				if (base.Hierarchy != null)
				{
					base.Hierarchy.OnChildRemoved += Hierarchy_OnChildRemoved;
				}
			}
			Render.CastShadows = true;
			Render.NeedsResolveCastShadow = false;
			foreach (MyCockpit tmpOccupiedCockpit in m_tmpOccupiedCockpits)
			{
				tmpOccupiedCockpit.GiveControlToPilot();
			}
			m_tmpOccupiedCockpits.Clear();
			if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
			{
				PrepareMultiBlockInfos();
			}
			m_isRespawnGrid.SetLocalValue(myObjectBuilder_CubeGrid.IsRespawnGrid);
			m_playedTime = myObjectBuilder_CubeGrid.playedTime;
			GridGeneralDamageModifier = myObjectBuilder_CubeGrid.GridGeneralDamageModifier;
			LocalCoordSystem = myObjectBuilder_CubeGrid.LocalCoordSys;
			m_dampenersEnabled.SetLocalValue(myObjectBuilder_CubeGrid.DampenersEnabled);
			if (myObjectBuilder_CubeGrid.TargetingTargets != null)
			{
				m_targetingList = myObjectBuilder_CubeGrid.TargetingTargets;
			}
			m_targetingListIsWhitelist = myObjectBuilder_CubeGrid.TargetingWhitelist;
			m_usesTargetingList = (m_targetingList.Count > 0 || m_targetingListIsWhitelist);
			if (myObjectBuilder_CubeGrid.TargetingTargets != null)
			{
				m_targetingList = myObjectBuilder_CubeGrid.TargetingTargets;
			}
			m_targetingListIsWhitelist = myObjectBuilder_CubeGrid.TargetingWhitelist;
			m_usesTargetingList = (m_targetingList.Count > 0 || m_targetingListIsWhitelist);
		}

		private void Hierarchy_OnChildRemoved(VRage.ModAPI.IMyEntity obj)
		{
			m_fatBlocks.Remove(obj as MyCubeBlock);
		}

		private static MyCubeGrid CreateGridForSplit(MyCubeGrid originalGrid, long newEntityId)
		{
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = MyObjectBuilderSerializer.CreateNewObject(typeof(MyObjectBuilder_CubeGrid)) as MyObjectBuilder_CubeGrid;
			if (myObjectBuilder_CubeGrid == null)
			{
				MyLog.Default.WriteLine("CreateForSplit builder shouldn't be null! Original Grid info: " + originalGrid.ToString());
				return null;
			}
			myObjectBuilder_CubeGrid.EntityId = newEntityId;
			myObjectBuilder_CubeGrid.GridSizeEnum = originalGrid.GridSizeEnum;
			myObjectBuilder_CubeGrid.IsStatic = originalGrid.IsStatic;
			myObjectBuilder_CubeGrid.PersistentFlags = originalGrid.Render.PersistentFlags;
			myObjectBuilder_CubeGrid.PositionAndOrientation = new MyPositionAndOrientation(originalGrid.WorldMatrix);
			myObjectBuilder_CubeGrid.DampenersEnabled = originalGrid.m_dampenersEnabled;
			myObjectBuilder_CubeGrid.IsPowered = originalGrid.m_IsPowered;
			myObjectBuilder_CubeGrid.IsUnsupportedStation = originalGrid.IsUnsupportedStation;
			MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilderNoinit(myObjectBuilder_CubeGrid) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return null;
			}
			myCubeGrid.InitInternal(myObjectBuilder_CubeGrid, rebuildGrid: false);
			MyCubeGrid.OnSplitGridCreated.InvokeIfNotNull(myCubeGrid);
			return myCubeGrid;
		}

		public static void RemoveSplit(MyCubeGrid originalGrid, List<MySlimBlock> blocks, int offset, int count, bool sync = true)
		{
			for (int i = offset; i < offset + count; i++)
			{
				if (blocks.Count <= i)
				{
					continue;
				}
				MySlimBlock mySlimBlock = blocks[i];
				if (mySlimBlock != null)
				{
					if (mySlimBlock.FatBlock != null)
					{
						originalGrid.Hierarchy.RemoveChild(mySlimBlock.FatBlock);
					}
					bool enable = originalGrid.EnableGenerators(enable: false, fromServer: true);
					originalGrid.RemoveBlockInternal(mySlimBlock, close: true, markDirtyDisconnects: false);
					originalGrid.EnableGenerators(enable, fromServer: true);
					originalGrid.Physics.AddDirtyBlock(mySlimBlock);
				}
			}
			originalGrid.RemoveEmptyBlockGroups();
			if (sync && Sync.IsServer)
			{
				originalGrid.AnnounceRemoveSplit(blocks);
			}
		}

		public MyCubeGrid SplitByPlane(PlaneD plane)
		{
			m_tmpSlimBlocks.Clear();
			MyCubeGrid result = null;
			PlaneD plane2 = PlaneD.Transform(plane, base.PositionComp.WorldMatrixNormalizedInv);
			foreach (MySlimBlock block in GetBlocks())
			{
				BoundingBoxD boundingBoxD = new BoundingBoxD(block.Min * GridSize, block.Max * GridSize);
				boundingBoxD.Inflate(GridSize / 2f);
				if (boundingBoxD.Intersects(plane2) == PlaneIntersectionType.Back)
				{
					m_tmpSlimBlocks.Add(block);
				}
			}
			if (m_tmpSlimBlocks.Count != 0)
			{
				result = CreateSplit(this, m_tmpSlimBlocks, sync: true, 0L);
				m_tmpSlimBlocks.Clear();
			}
			return result;
		}

		public static MyCubeGrid CreateSplit(MyCubeGrid originalGrid, List<MySlimBlock> blocks, bool sync = true, long newEntityId = 0L)
		{
			MyCubeGrid myCubeGrid = CreateGridForSplit(originalGrid, newEntityId);
			if (myCubeGrid == null)
			{
				return null;
			}
			Vector3 value = originalGrid.Physics.CenterOfMassWorld;
			MyEntities.Add(myCubeGrid);
			MoveBlocks(originalGrid, myCubeGrid, blocks, 0, blocks.Count);
			myCubeGrid.RebuildGrid();
			if (!myCubeGrid.IsStatic)
			{
				myCubeGrid.Physics.UpdateMass();
			}
			if (originalGrid.IsStatic)
			{
				myCubeGrid.TestDynamic = MyTestDynamicReason.GridSplit;
				originalGrid.TestDynamic = MyTestDynamicReason.GridSplit;
			}
			myCubeGrid.Physics.AngularVelocity = originalGrid.Physics.AngularVelocity;
			myCubeGrid.Physics.LinearVelocity = originalGrid.Physics.GetVelocityAtPoint(myCubeGrid.Physics.CenterOfMassWorld);
			originalGrid.UpdatePhysicsShape();
			if (!originalGrid.IsStatic)
			{
				originalGrid.Physics.UpdateMass();
			}
			Vector3 value2 = Vector3.Cross(originalGrid.Physics.AngularVelocity, originalGrid.Physics.CenterOfMassWorld - value);
			originalGrid.Physics.LinearVelocity = originalGrid.Physics.LinearVelocity + value2;
			if (originalGrid.OnGridSplit != null)
			{
				originalGrid.OnGridSplit(originalGrid, myCubeGrid);
			}
			if (sync)
			{
				if (!Sync.IsServer)
				{
					return myCubeGrid;
				}
				m_tmpBlockPositions.Clear();
				foreach (MySlimBlock block in blocks)
				{
					m_tmpBlockPositions.Add(block.Position);
				}
				MyMultiplayer.RemoveForClientIfIncomplete(originalGrid);
				MyMultiplayer.RaiseEvent(originalGrid, (MyCubeGrid x) => x.CreateSplit_Implementation, m_tmpBlockPositions, myCubeGrid.EntityId);
			}
			return myCubeGrid;
		}

		[Event(null, 1272)]
		[Reliable]
		[Broadcast]
		public void CreateSplit_Implementation(List<Vector3I> blocks, long newEntityId)
		{
			m_tmpBlockListReceive.Clear();
			foreach (Vector3I block in blocks)
			{
				MySlimBlock cubeBlock = GetCubeBlock(block);
				if (cubeBlock == null)
				{
					MySandboxGame.Log.WriteLine("Block was null when trying to create a grid split. Desync?");
				}
				else
				{
					m_tmpBlockListReceive.Add(cubeBlock);
				}
			}
			CreateSplit(this, m_tmpBlockListReceive, sync: false, newEntityId);
			m_tmpBlockListReceive.Clear();
		}

		public static void CreateSplits(MyCubeGrid originalGrid, List<MySlimBlock> splitBlocks, List<MyDisconnectHelper.Group> groups, MyTestDisconnectsReason reason, bool sync = true)
		{
			if (originalGrid != null && originalGrid.Physics != null && groups != null && splitBlocks != null)
			{
				Vector3D centerOfMassWorld = originalGrid.Physics.CenterOfMassWorld;
				try
				{
					if (MyCubeGridSmallToLargeConnection.Static != null)
					{
						MyCubeGridSmallToLargeConnection.Static.BeforeGridSplit_SmallToLargeGridConnectivity(originalGrid);
					}
					for (int i = 0; i < groups.Count; i++)
					{
						MyDisconnectHelper.Group group = groups[i];
						CreateSplitForGroup(originalGrid, splitBlocks, ref group);
						groups[i] = group;
					}
					originalGrid.UpdatePhysicsShape();
					foreach (MyCubeGrid tmpGrid in originalGrid.m_tmpGrids)
					{
						tmpGrid.RebuildGrid();
						if (originalGrid.IsStatic && !MySession.Static.Settings.StationVoxelSupport)
						{
							tmpGrid.TestDynamic = ((reason == MyTestDisconnectsReason.SplitBlock) ? MyTestDynamicReason.GridSplitByBlock : MyTestDynamicReason.GridSplit);
							originalGrid.TestDynamic = ((reason == MyTestDisconnectsReason.SplitBlock) ? MyTestDynamicReason.GridSplitByBlock : MyTestDynamicReason.GridSplit);
						}
						tmpGrid.Physics.AngularVelocity = originalGrid.Physics.AngularVelocity;
						tmpGrid.Physics.LinearVelocity = originalGrid.Physics.GetVelocityAtPoint(tmpGrid.Physics.CenterOfMassWorld);
						Interlocked.Increment(ref originalGrid.m_resolvingSplits);
						tmpGrid.UpdateDirty(delegate
						{
							Interlocked.Decrement(ref originalGrid.m_resolvingSplits);
						});
						tmpGrid.UpdateGravity();
						tmpGrid.MarkForUpdate();
					}
					Vector3 value = Vector3.Cross(originalGrid.Physics.AngularVelocity, originalGrid.Physics.CenterOfMassWorld - centerOfMassWorld);
					originalGrid.Physics.LinearVelocity = originalGrid.Physics.LinearVelocity + value;
					originalGrid.MarkForUpdate();
					if (MyCubeGridSmallToLargeConnection.Static != null)
					{
						MyCubeGridSmallToLargeConnection.Static.AfterGridSplit_SmallToLargeGridConnectivity(originalGrid, originalGrid.m_tmpGrids);
					}
					Action<MyCubeGrid, MyCubeGrid> onGridSplit = originalGrid.OnGridSplit;
					if (onGridSplit != null)
					{
						foreach (MyCubeGrid tmpGrid2 in originalGrid.m_tmpGrids)
						{
							onGridSplit(originalGrid, tmpGrid2);
						}
					}
					foreach (MyCubeGrid tmpGrid3 in originalGrid.m_tmpGrids)
					{
						tmpGrid3.GridSystems.UpdatePower();
						if (tmpGrid3.GridSystems.ResourceDistributor != null)
						{
							tmpGrid3.GridSystems.ResourceDistributor.MarkForUpdate();
						}
					}
					if (sync && Sync.IsServer)
					{
						MyMultiplayer.RemoveForClientIfIncomplete(originalGrid);
						m_tmpBlockPositions.Clear();
						foreach (MySlimBlock splitBlock in splitBlocks)
						{
							m_tmpBlockPositions.Add(splitBlock.Position);
						}
						foreach (MyCubeGrid tmpGrid4 in originalGrid.m_tmpGrids)
						{
							tmpGrid4.IsSplit = true;
							MyMultiplayer.ReplicateImmediatelly(MyExternalReplicable.FindByObject(tmpGrid4), MyExternalReplicable.FindByObject(originalGrid));
							tmpGrid4.IsSplit = false;
						}
						MyMultiplayer.RaiseEvent(originalGrid, (MyCubeGrid x) => x.CreateSplits_Implementation, m_tmpBlockPositions, groups);
					}
				}
				finally
				{
					originalGrid.m_tmpGrids.Clear();
				}
			}
		}

		[Event(null, 1415)]
		[Reliable]
		[Broadcast]
		public void CreateSplits_Implementation(List<Vector3I> blocks, List<MyDisconnectHelper.Group> groups)
		{
			if (base.MarkedForClose)
			{
				return;
			}
			m_tmpBlockListReceive.Clear();
			for (int i = 0; i < groups.Count; i++)
			{
				MyDisconnectHelper.Group value = groups[i];
				int num = value.BlockCount;
				for (int j = value.FirstBlockIndex; j < value.FirstBlockIndex + value.BlockCount; j++)
				{
					MySlimBlock cubeBlock = GetCubeBlock(blocks[j]);
					if (cubeBlock == null)
					{
						MySandboxGame.Log.WriteLine("Block was null when trying to create a grid split. Desync?");
						num--;
						if (num == 0)
						{
							value.IsValid = false;
						}
					}
					m_tmpBlockListReceive.Add(cubeBlock);
				}
				groups[i] = value;
			}
			CreateSplits(this, m_tmpBlockListReceive, groups, MyTestDisconnectsReason.BlockRemoved, sync: false);
			m_tmpBlockListReceive.Clear();
		}

		private static void CreateSplitForGroup(MyCubeGrid originalGrid, List<MySlimBlock> splitBlocks, ref MyDisconnectHelper.Group group)
		{
			if (!originalGrid.IsStatic && Sync.IsServer && group.IsValid)
			{
				int num = 0;
				for (int i = group.FirstBlockIndex; i < group.FirstBlockIndex + group.BlockCount; i++)
				{
					if (MyDisconnectHelper.IsDestroyedInVoxels(splitBlocks[i]))
					{
						num++;
						if ((float)num / (float)group.BlockCount > 0.4f)
						{
							group.IsValid = false;
							break;
						}
					}
				}
			}
			group.IsValid = (group.IsValid && CanHavePhysics(splitBlocks, group.FirstBlockIndex, group.BlockCount) && HasStandAloneBlocks(splitBlocks, group.FirstBlockIndex, group.BlockCount));
			if (group.BlockCount == 1 && splitBlocks.Count > group.FirstBlockIndex && splitBlocks[group.FirstBlockIndex] != null)
			{
				MySlimBlock mySlimBlock = splitBlocks[group.FirstBlockIndex];
				if (mySlimBlock.FatBlock is MyFracturedBlock)
				{
					group.IsValid = false;
					if (Sync.IsServer)
					{
						MyDestructionHelper.CreateFracturePiece(mySlimBlock.FatBlock as MyFracturedBlock, sync: true);
					}
				}
				else if (mySlimBlock.FatBlock != null && mySlimBlock.FatBlock.Components.Has<MyFractureComponentBase>())
				{
					group.IsValid = false;
					if (Sync.IsServer)
					{
						MyFractureComponentCubeBlock fractureComponent = mySlimBlock.GetFractureComponent();
						if (fractureComponent != null)
						{
							MyDestructionHelper.CreateFracturePiece(fractureComponent, sync: true);
						}
					}
				}
				else if (mySlimBlock.FatBlock is MyCompoundCubeBlock)
				{
					MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
					bool flag = true;
					foreach (MySlimBlock block in myCompoundCubeBlock.GetBlocks())
					{
						flag &= block.FatBlock.Components.Has<MyFractureComponentBase>();
						if (!flag)
						{
							break;
						}
					}
					if (flag)
					{
						group.IsValid = false;
						if (Sync.IsServer)
						{
							foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
							{
								MyFractureComponentCubeBlock fractureComponent2 = block2.GetFractureComponent();
								if (fractureComponent2 != null)
								{
									MyDestructionHelper.CreateFracturePiece(fractureComponent2, sync: true);
								}
							}
						}
					}
				}
			}
			if (group.IsValid)
			{
				MyCubeGrid myCubeGrid = CreateGridForSplit(originalGrid, group.EntityId);
				if (myCubeGrid != null)
				{
					originalGrid.m_tmpGrids.Add(myCubeGrid);
					MoveBlocks(originalGrid, myCubeGrid, splitBlocks, group.FirstBlockIndex, group.BlockCount);
					myCubeGrid.SetInventoryMassDirty();
					myCubeGrid.Render.FadeIn = false;
					myCubeGrid.RebuildGrid();
					MyEntities.Add(myCubeGrid);
					group.EntityId = myCubeGrid.EntityId;
					if (myCubeGrid.IsStatic && Sync.IsServer)
					{
						MatrixD tranform = myCubeGrid.WorldMatrix;
						bool flag2 = MyCoordinateSystem.Static.IsLocalCoordSysExist(ref tranform, myCubeGrid.GridSize);
						if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
						{
							if (flag2)
							{
								MyCoordinateSystem.Static.RegisterCubeGrid(myCubeGrid);
							}
							else
							{
								MyCoordinateSystem.Static.CreateCoordSys(myCubeGrid, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter, sync: true);
							}
						}
					}
				}
				else
				{
					group.IsValid = false;
				}
			}
			if (!group.IsValid)
			{
				RemoveSplit(originalGrid, splitBlocks, group.FirstBlockIndex, group.BlockCount, sync: false);
			}
		}

		private void AddGroup(MyObjectBuilder_BlockGroup groupBuilder)
		{
			if (groupBuilder.Blocks.Count != 0)
			{
				MyBlockGroup myBlockGroup = new MyBlockGroup();
				myBlockGroup.Init(this, groupBuilder);
				BlockGroups.Add(myBlockGroup);
			}
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = (MyObjectBuilder_CubeGrid)base.GetObjectBuilder(copy);
			GetObjectBuilderInternal(myObjectBuilder_CubeGrid, copy);
			return myObjectBuilder_CubeGrid;
		}

		private void GetObjectBuilderInternal(MyObjectBuilder_CubeGrid ob, bool copy)
		{
			ob.GridSizeEnum = GridSizeEnum;
			if (ob.Skeleton == null)
			{
				ob.Skeleton = new List<BoneInfo>();
			}
			ob.Skeleton.Clear();
			Skeleton.Serialize(ob.Skeleton, GridSize, this);
			ob.IsStatic = IsStatic;
			ob.IsUnsupportedStation = IsUnsupportedStation;
			ob.Editable = Editable;
			ob.IsPowered = m_IsPowered;
			ob.CubeBlocks.Clear();
			foreach (MySlimBlock cubeBlock in m_cubeBlocks)
			{
				MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = null;
				myObjectBuilder_CubeBlock = ((!copy) ? cubeBlock.GetObjectBuilder() : cubeBlock.GetCopyObjectBuilder());
				if (myObjectBuilder_CubeBlock != null)
				{
					ob.CubeBlocks.Add(myObjectBuilder_CubeBlock);
				}
			}
			ob.PersistentFlags = Render.PersistentFlags;
			if (Physics != null)
			{
				ob.LinearVelocity = Physics.LinearVelocity;
				ob.AngularVelocity = Physics.AngularVelocity;
			}
			ob.XMirroxPlane = XSymmetryPlane;
			ob.YMirroxPlane = YSymmetryPlane;
			ob.ZMirroxPlane = ZSymmetryPlane;
			ob.XMirroxOdd = XSymmetryOdd;
			ob.YMirroxOdd = YSymmetryOdd;
			ob.ZMirroxOdd = ZSymmetryOdd;
			if (copy)
			{
				ob.Name = null;
			}
			ob.BlockGroups.Clear();
			foreach (MyBlockGroup blockGroup in BlockGroups)
			{
				ob.BlockGroups.Add(blockGroup.GetObjectBuilder());
			}
			ob.DisplayName = base.DisplayName;
			ob.DestructibleBlocks = DestructibleBlocks;
			ob.IsRespawnGrid = IsRespawnGrid;
			ob.playedTime = m_playedTime;
			ob.GridGeneralDamageModifier = GridGeneralDamageModifier;
			ob.LocalCoordSys = LocalCoordSystem;
			ob.TargetingWhitelist = m_targetingListIsWhitelist;
			ob.TargetingTargets = m_targetingList;
			GridSystems.GetObjectBuilder(ob);
		}

		internal void HavokSystemIDChanged(int id)
		{
			this.OnHavokSystemIDChanged.InvokeIfNotNull(id);
		}

		private void UpdatePhysicsShape()
		{
			Physics.UpdateShape();
		}

		public List<HkShape> GetShapesFromPosition(Vector3I pos)
		{
			return Physics.GetShapesFromPosition(pos);
		}

		private void UpdateGravity()
		{
			if (!IsStatic && Physics != null && Physics.Enabled && !Physics.IsWelded)
			{
				if (Physics.DisableGravity <= 0)
				{
					RecalculateGravity();
				}
				else
				{
					Physics.DisableGravity--;
				}
				if (!Physics.IsWelded && !Physics.RigidBody.Gravity.Equals(m_gravity, 0.01f))
				{
					Physics.Gravity = m_gravity;
					ActivatePhysics();
				}
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			UpdateGravity();
			base.UpdateOnceBeforeFrame();
			if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE || MyFakes.ENABLE_GRID_SYSTEM_ONCE_BEFORE_FRAME_UPDATE)
			{
				GridSystems.UpdateOnceBeforeFrame();
			}
			ActivatePhysics();
		}

		public void UpdatePredictionFlag()
		{
			bool flag = false;
			IsClientPredictedCar = false;
			if (MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID && !IsStatic && !ForceDisablePrediction)
			{
				MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot(this);
				if (root == this)
				{
					if ((!Sync.IsServer && MySession.Static.TopMostControlledEntity == this) || (Sync.IsServer && Sync.Players.GetControllingPlayer(this) != null))
					{
						if (!MyGridPhysicalHierarchy.Static.HasChildren(this) && !MyFixedGrids.IsRooted(this))
						{
							flag = true;
							if (Physics.PredictedContactsCounter > PREDICTION_SWITCH_MIN_COUNTER)
							{
								if (Physics.AnyPredictedContactEntities())
								{
									flag = false;
								}
								else if (Physics.PredictedContactLastTime + MyTimeSpan.FromSeconds(PREDICTION_SWITCH_TIME) < MySandboxGame.Static.SimulationTime)
								{
									Physics.PredictedContactsCounter = 0;
								}
							}
						}
						else if (MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CAR)
						{
							bool car = true;
							MyGridPhysicalHierarchy.Static.ApplyOnChildren(this, delegate(MyCubeGrid child)
							{
								if (MyGridPhysicalHierarchy.Static.GetEntityConnectingToParent(child) is MyMotorSuspension)
								{
									child.IsClientPredictedWheel = false;
									foreach (MyCubeBlock fatBlock in child.GetFatBlocks())
									{
										if (fatBlock is MyWheel)
										{
											child.IsClientPredictedWheel = true;
											break;
										}
									}
									if (!child.IsClientPredictedWheel)
									{
										car = false;
									}
								}
								else
								{
									car = false;
								}
							});
							flag = car;
							IsClientPredictedCar = car;
						}
					}
				}
				else if (root != this)
				{
					flag = root.IsClientPredicted;
				}
			}
			bool num = IsClientPredicted != flag;
			IsClientPredicted = flag;
			if (num)
			{
				Physics.UpdateConstraintsForceDisable();
			}
		}

		public override void UpdateBeforeSimulation()
		{
			UpdatePredictionFlag();
			if (MyPhysicsConfig.EnableGridSpeedDebugDraw && Physics != null)
			{
				Color color = (!(Physics.RigidBody.MaxLinearVelocity > 190f)) ? Color.Red : Color.Green;
				MyRenderProxy.DebugDrawText3D(base.PositionComp.GetPosition(), Physics.LinearVelocity.Length().ToString("F2"), color, 1f, depthRead: false);
				MyRenderProxy.DebugDrawText3D(base.PositionComp.GetPosition() + Vector3.One * 3f, Physics.AngularVelocity.Length().ToString("F2"), color, 1f, depthRead: false);
			}
			if (Sync.IsServer && Physics != null)
			{
				bool flag = false;
				if (IsMarkedForEarlyDeactivation)
				{
					if (!Physics.IsStatic)
					{
						flag = true;
						Physics.ConvertToStatic();
					}
				}
				else if (!IsStatic && Physics.IsStatic)
				{
					flag = true;
					Physics.ConvertToDynamic(GridSizeEnum == MyCubeSize.Large, isPredicted: false);
				}
				if (flag)
				{
					RaisePhysicsChanged();
				}
			}
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation");
			if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
			{
				GridSystems.UpdateBeforeSimulation();
			}
			if (m_hasAdditionalModelGenerators)
			{
				foreach (IMyBlockAdditionalModelGenerator additionalModelGenerator in AdditionalModelGenerators)
				{
					additionalModelGenerator.UpdateBeforeSimulation();
				}
			}
			DoLazyUpdates();
			base.UpdateBeforeSimulation();
			if (Physics != null)
			{
				Physics.UpdateBeforeSimulation();
			}
			if (MySessionComponentReplay.Static.IsEntityBeingReplayed(base.EntityId, out PerFrameData perFrameData))
			{
				if (perFrameData.MovementData.HasValue && !IsStatic && base.InScene)
				{
					MyShipController shipController = GridSystems.ControlSystem.GetShipController();
					if (shipController != null)
					{
						SerializableVector3 moveVector = perFrameData.MovementData.Value.MoveVector;
						Vector2 rotationIndicator = new Vector2(perFrameData.MovementData.Value.RotateVector.X, perFrameData.MovementData.Value.RotateVector.Y);
						float z = perFrameData.MovementData.Value.RotateVector.Z;
						shipController.MoveAndRotate(moveVector, rotationIndicator, z);
					}
				}
				if (perFrameData.SwitchWeaponData.HasValue)
				{
					MyShipController shipController2 = GridSystems.ControlSystem.GetShipController();
					if (shipController2 != null && perFrameData.SwitchWeaponData.Value.WeaponDefinition.HasValue && !perFrameData.SwitchWeaponData.Value.WeaponDefinition.Value.TypeId.IsNull)
					{
						shipController2.SwitchToWeapon(perFrameData.SwitchWeaponData.Value.WeaponDefinition.Value);
					}
				}
				if (perFrameData.ShootData.HasValue)
				{
					MyShipController shipController3 = GridSystems.ControlSystem.GetShipController();
					if (shipController3 != null)
					{
						if (perFrameData.ShootData.Value.Begin)
						{
							shipController3.BeginShoot((MyShootActionEnum)perFrameData.ShootData.Value.ShootAction);
						}
						else
						{
							shipController3.EndShoot((MyShootActionEnum)perFrameData.ShootData.Value.ShootAction);
						}
					}
				}
				if (perFrameData.ControlSwitchesData.HasValue)
				{
					MyShipController shipController4 = GridSystems.ControlSystem.GetShipController();
					if (shipController4 != null)
					{
						if (perFrameData.ControlSwitchesData.Value.SwitchDamping)
						{
							shipController4.SwitchDamping();
						}
						if (perFrameData.ControlSwitchesData.Value.SwitchLandingGears)
						{
							shipController4.SwitchLandingGears();
						}
						if (perFrameData.ControlSwitchesData.Value.SwitchLights)
						{
							shipController4.SwitchLights();
						}
						if (perFrameData.ControlSwitchesData.Value.SwitchReactors)
						{
							shipController4.SwitchReactors();
						}
						if (perFrameData.ControlSwitchesData.Value.SwitchThrusts)
						{
							shipController4.SwitchThrusts();
						}
					}
				}
				if (perFrameData.UseData.HasValue)
				{
					MyShipController shipController5 = GridSystems.ControlSystem.GetShipController();
					if (shipController5 != null)
					{
						if (perFrameData.UseData.Value.Use)
						{
							shipController5.Use();
						}
						else if (perFrameData.UseData.Value.UseContinues)
						{
							shipController5.UseContinues();
						}
						else if (perFrameData.UseData.Value.UseFinished)
						{
							shipController5.UseFinished();
						}
					}
				}
			}
			MySimpleProfiler.End("UpdateBeforeSimulation");
		}

		protected static float GetLineWidthForGizmo(IMyGizmoDrawableObject block, BoundingBox box)
		{
			float num = 100f;
			foreach (Vector3 corner in box.Corners)
			{
				num = (float)Math.Min(num, Math.Abs(MySector.MainCamera.GetDistanceFromPoint(Vector3.Transform(block.GetPositionInGrid() + corner, block.GetWorldMatrix()))));
			}
			Vector3 vector = box.Max - box.Min;
			float num2 = MathHelper.Max(1f, MathHelper.Min(MathHelper.Min(vector.X, vector.Y), vector.Z));
			return num * 0.002f / num2;
		}

		public bool IsGizmoDrawingEnabled()
		{
			if (!ShowSenzorGizmos && !ShowGravityGizmos)
			{
				return ShowAntennaGizmos;
			}
			return true;
		}

		public override void PrepareForDraw()
		{
			base.PrepareForDraw();
			GridSystems.PrepareForDraw();
			if (IsGizmoDrawingEnabled())
			{
				foreach (MySlimBlock cubeBlock in m_cubeBlocks)
				{
					if (cubeBlock.FatBlock is IMyGizmoDrawableObject)
					{
						DrawObjectGizmo(cubeBlock);
					}
				}
			}
			if (!NeedsPerFrameDraw)
			{
				Render.NeedsDraw = false;
			}
		}

		private static void DrawObjectGizmo(MySlimBlock block)
		{
			IMyGizmoDrawableObject myGizmoDrawableObject = block.FatBlock as IMyGizmoDrawableObject;
			if (!myGizmoDrawableObject.CanBeDrawn())
			{
				return;
			}
			Color color = myGizmoDrawableObject.GetGizmoColor();
			MatrixD worldMatrix = myGizmoDrawableObject.GetWorldMatrix();
			BoundingBox? boundingBox = myGizmoDrawableObject.GetBoundingBox();
			if (boundingBox.HasValue)
			{
				float lineWidthForGizmo = GetLineWidthForGizmo(myGizmoDrawableObject, boundingBox.Value);
				BoundingBoxD localbox = boundingBox.Value;
				MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 1, lineWidthForGizmo);
				return;
			}
			float radius = myGizmoDrawableObject.GetRadius();
			MySector.MainCamera.GetDistanceFromPoint(worldMatrix.Translation);
			float value = (float)((double)radius - MySector.MainCamera.GetDistanceFromPoint(worldMatrix.Translation));
			float lineThickness = 0.002f * Math.Min(100f, Math.Abs(value));
			int customViewProjectionMatrix = -1;
			MySimpleObjectDraw.DrawTransparentSphere(ref worldMatrix, radius, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 20, null, null, lineThickness, customViewProjectionMatrix);
			if (myGizmoDrawableObject.EnableLongDrawDistance() && MyFakes.ENABLE_LONG_DISTANCE_GIZMO_DRAWING)
			{
				MyBillboardViewProjection billboardViewProjection = default(MyBillboardViewProjection);
				billboardViewProjection.CameraPosition = MySector.MainCamera.Position;
				billboardViewProjection.ViewAtZero = default(Matrix);
				billboardViewProjection.Viewport = MySector.MainCamera.Viewport;
				float aspectRatio = billboardViewProjection.Viewport.Width / billboardViewProjection.Viewport.Height;
				billboardViewProjection.Projection = Matrix.CreatePerspectiveFieldOfView(MySector.MainCamera.FieldOfView, aspectRatio, 1f, 100f);
				billboardViewProjection.Projection.M33 = -1f;
				billboardViewProjection.Projection.M34 = -1f;
				billboardViewProjection.Projection.M43 = 0f;
				billboardViewProjection.Projection.M44 = 0f;
				customViewProjectionMatrix = 10;
				MyRenderProxy.AddBillboardViewProjection(customViewProjectionMatrix, billboardViewProjection);
				MySimpleObjectDraw.DrawTransparentSphere(ref worldMatrix, radius, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 20, null, null, lineThickness, customViewProjectionMatrix);
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation10");
			base.UpdateBeforeSimulation10();
			if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
			{
				GridSystems.UpdateBeforeSimulation10();
			}
			MySimpleProfiler.End("UpdateBeforeSimulation10");
		}

		public override void UpdateBeforeSimulation100()
		{
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation100");
			base.UpdateBeforeSimulation100();
			if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
			{
				GridSystems.UpdateBeforeSimulation100();
			}
			MySimpleProfiler.End("UpdateBeforeSimulation100");
		}

		internal void SetInventoryMassDirty()
		{
			m_inventoryMassDirty = true;
			MarkForUpdate();
		}

		public int GetCurrentMass()
		{
			float baseMass;
			float physicalMass;
			return (int)GetCurrentMass(out baseMass, out physicalMass);
		}

		public float GetCurrentMass(out float baseMass, out float physicalMass)
		{
			baseMass = 0f;
			physicalMass = 0f;
			float num = 0f;
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(this);
			if (group != null)
			{
				float blocksInventorySizeMultiplier = MySession.Static.Settings.BlocksInventorySizeMultiplier;
				{
					foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
					{
						MyCubeGrid nodeData = node.NodeData;
						if (nodeData != null && nodeData.Physics != null && nodeData.Physics.Shape != null)
						{
							HkMassProperties? massProperties = nodeData.Physics.Shape.MassProperties;
							HkMassProperties? baseMassProperties = nodeData.Physics.Shape.BaseMassProperties;
							if (!IsStatic && massProperties.HasValue && baseMassProperties.HasValue)
							{
								float num2 = massProperties.Value.Mass;
								float num3 = baseMassProperties.Value.Mass;
								foreach (MyCockpit occupiedBlock in nodeData.OccupiedBlocks)
								{
									MyCharacter pilot = occupiedBlock.Pilot;
									if (pilot != null)
									{
										float baseMass2 = pilot.BaseMass;
										float num4 = pilot.CurrentMass - baseMass2;
										num3 += baseMass2;
										num2 += num4 / blocksInventorySizeMultiplier;
									}
								}
								float num5 = (num2 - num3) * blocksInventorySizeMultiplier;
								baseMass += num3;
								num += num3 + num5;
								if (nodeData.Physics.WeldInfo.Parent == null || nodeData.Physics.WeldInfo.Parent == nodeData.Physics)
								{
									physicalMass += nodeData.Physics.Mass;
								}
							}
						}
					}
					return num;
				}
			}
			return num;
		}

		public override void UpdateAfterSimulation100()
		{
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateAfterSimulation100");
			base.UpdateAfterSimulation100();
			UpdateGravity();
			if (MyFakes.ENABLE_BOUNDINGBOX_SHRINKING && m_boundsDirty && MySandboxGame.TotalSimulationTimeInMilliseconds - m_lastUpdatedDirtyBounds > 30000)
			{
				Vector3I min = m_min;
				Vector3I max = m_max;
				RecalcBounds();
				m_boundsDirty = false;
				m_lastUpdatedDirtyBounds = MySandboxGame.TotalSimulationTimeInMilliseconds;
				if (GridSystems.GasSystem != null && (min != m_min || max != m_max))
				{
					GridSystems.GasSystem.OnCubeGridShrinked();
				}
			}
			if (MyFakes.ENABLE_GRID_SYSTEM_UPDATE)
			{
				GridSystems.UpdateAfterSimulation100();
			}
			MySimpleProfiler.End("UpdateAfterSimulation100");
		}

		public override void UpdateAfterSimulation()
		{
			MySimpleProfiler.Begin("Grid", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateAfterSimulation");
			base.UpdateAfterSimulation();
			ApplyDeformationPostponed();
			if (m_hasAdditionalModelGenerators)
			{
				foreach (IMyBlockAdditionalModelGenerator additionalModelGenerator in AdditionalModelGenerators)
				{
					additionalModelGenerator.UpdateAfterSimulation();
				}
			}
			SendRemovedBlocks();
			SendRemovedBlocksWithIds();
			if (!HasStandAloneBlocks())
			{
				m_worldPositionChanged = false;
				if (Sync.IsServer)
				{
					SetFadeOut(state: false);
					Close();
				}
				MySimpleProfiler.End("UpdateAfterSimulation");
				return;
			}
			if (MarkedAsTrash)
			{
				m_trashHighlightCounter--;
				if (TrashHighlightCounter <= 0 && Sync.IsServer)
				{
					MySessionComponentTrash.RemoveGrid(this);
				}
			}
			if (Sync.IsServer)
			{
				if (MyFakes.ENABLE_FRACTURE_COMPONENT)
				{
					if (Physics != null)
					{
						if (Physics.GetFractureBlockComponents().Count > 0)
						{
							try
							{
								foreach (MyFractureComponentBase.Info fractureBlockComponent in Physics.GetFractureBlockComponents())
								{
									CreateFractureBlockComponent(fractureBlockComponent);
								}
							}
							finally
							{
								Physics.ClearFractureBlockComponents();
							}
						}
						Physics.CheckLastDestroyedBlockFracturePieces();
					}
				}
				else if (Physics != null && Physics.GetFracturedBlocks().Count > 0)
				{
					bool enable = EnableGenerators(enable: false);
					foreach (MyFracturedBlock.Info fracturedBlock in Physics.GetFracturedBlocks())
					{
						CreateFracturedBlock(fracturedBlock);
					}
					EnableGenerators(enable);
				}
			}
			if (Sync.IsServer && TestDynamic != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertedToShipRequest, TestDynamic);
				TestDynamic = MyTestDynamicReason.NoReason;
			}
			DoLazyUpdates();
			if (!Sync.IsServer && Physics != null && !IsStatic && IsClientPredicted == Physics.IsStatic)
			{
				Physics.ConvertToDynamic(GridSizeEnum == MyCubeSize.Large, IsClientPredicted);
				UpdateGravity();
			}
			if (Physics != null && Physics.Enabled && m_inventoryMassDirty)
			{
				m_inventoryMassDirty = false;
				Physics.Shape.UpdateMassFromInventories(m_inventoryBlocks, Physics);
			}
			if (m_worldPositionChanged)
			{
				UpdateMergingGrids();
				m_worldPositionChanged = false;
			}
			if (Physics != null)
			{
				Physics.UpdateAfterSimulation();
			}
			GridSystems.UpdateAfterSimulation();
			if (!NeedsPerFrameUpdate)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				ClearDirty();
			}
			else if (!m_updatingDirty && m_dirtyRegion.IsDirty)
			{
				UpdateDirty();
			}
			if (Physics != null)
			{
				_ = IsStatic;
			}
			MySimpleProfiler.End("UpdateAfterSimulation");
		}

		internal void MarkForUpdate()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		internal void MarkForUpdateParallel()
		{
			if ((base.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) == 0)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					if (!base.Closed)
					{
						MarkForUpdate();
					}
				}, "MarkForUpdate");
			}
		}

		internal void MarkForDraw()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					Render.NeedsDraw = true;
				}
			}, "MarkForDraw()");
		}

		private void CreateFractureBlockComponent(MyFractureComponentBase.Info info)
		{
			if (info.Entity.MarkedForClose)
			{
				return;
			}
			MyFractureComponentCubeBlock myFractureComponentCubeBlock = new MyFractureComponentCubeBlock();
			info.Entity.Components.Add((MyFractureComponentBase)myFractureComponentCubeBlock);
			myFractureComponentCubeBlock.SetShape(info.Shape, info.Compound);
			if (!Sync.IsServer)
			{
				return;
			}
			MyCubeBlock myCubeBlock = info.Entity as MyCubeBlock;
			if (myCubeBlock == null)
			{
				return;
			}
			MyCubeGridSmallToLargeConnection.Static.RemoveBlockSmallToLargeConnection(myCubeBlock.SlimBlock);
			MySlimBlock cubeBlock = myCubeBlock.CubeGrid.GetCubeBlock(myCubeBlock.Position);
			MyCompoundCubeBlock myCompoundCubeBlock = (cubeBlock != null) ? (cubeBlock.FatBlock as MyCompoundCubeBlock) : null;
			if (myCompoundCubeBlock != null)
			{
				ushort? blockId = myCompoundCubeBlock.GetBlockId(myCubeBlock.SlimBlock);
				if (blockId.HasValue)
				{
					MyObjectBuilder_FractureComponentBase component = (MyObjectBuilder_FractureComponentBase)myFractureComponentCubeBlock.Serialize();
					MySyncDestructions.CreateFractureComponent(myCubeBlock.CubeGrid.EntityId, myCubeBlock.Position, blockId.Value, component);
				}
			}
			else
			{
				MyObjectBuilder_FractureComponentBase component2 = (MyObjectBuilder_FractureComponentBase)myFractureComponentCubeBlock.Serialize();
				MySyncDestructions.CreateFractureComponent(myCubeBlock.CubeGrid.EntityId, myCubeBlock.Position, ushort.MaxValue, component2);
			}
			myCubeBlock.SlimBlock.ApplyDestructionDamage(myFractureComponentCubeBlock.GetIntegrityRatioFromFracturedPieceCounts());
		}

		internal void RemoveGroup(MyBlockGroup group)
		{
			BlockGroups.Remove(group);
			GridSystems.RemoveGroup(group);
		}

		internal void RemoveGroupByName(string name)
		{
			MyBlockGroup myBlockGroup = BlockGroups.Find((MyBlockGroup g) => g.Name.CompareTo(name) == 0);
			if (myBlockGroup != null)
			{
				BlockGroups.Remove(myBlockGroup);
				GridSystems.RemoveGroup(myBlockGroup);
			}
		}

		internal void AddGroup(MyBlockGroup group)
		{
			foreach (MyBlockGroup blockGroup in BlockGroups)
			{
				if (blockGroup.Name.CompareTo(group.Name) == 0)
				{
					BlockGroups.Remove(blockGroup);
					group.Blocks.UnionWith(blockGroup.Blocks);
					break;
				}
			}
			BlockGroups.Add(group);
			GridSystems.AddGroup(group);
		}

		internal void OnAddedToGroup(MyGridLogicalGroupData group)
		{
			GridSystems.OnAddedToGroup(group);
			if (this.AddedToLogicalGroup != null)
			{
				this.AddedToLogicalGroup(group);
			}
		}

		internal void OnRemovedFromGroup(MyGridLogicalGroupData group)
		{
			GridSystems.OnRemovedFromGroup(group);
			if (this.RemovedFromLogicalGroup != null)
			{
				this.RemovedFromLogicalGroup();
			}
		}

		internal void OnAddedToGroup(MyGridPhysicalGroupData groupData)
		{
			GridSystems.OnAddedToGroup(groupData);
		}

		internal void OnRemovedFromGroup(MyGridPhysicalGroupData group)
		{
			GridSystems.OnRemovedFromGroup(group);
		}

		private void TryReduceGroupControl()
		{
			MyEntityController entityController = Sync.Players.GetEntityController(this);
			if (entityController == null || !(entityController.ControlledEntity is MyCockpit))
			{
				return;
			}
			MyCockpit myCockpit = entityController.ControlledEntity as MyCockpit;
			if (myCockpit.CubeGrid == this)
			{
				MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this);
				if (group != null)
				{
					foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
					{
						if (node.NodeData != this)
						{
							if (MySession.Static == null)
							{
								MyLog.Default.WriteLine("MySession.Static was null");
							}
							else if (MySession.Static.SyncLayer == null)
							{
								MyLog.Default.WriteLine("MySession.Static.SyncLayer was null");
							}
							else if (Sync.Clients == null)
							{
								MyLog.Default.WriteLine("Sync.Clients was null");
							}
							Sync.Players.TryReduceControl(myCockpit, node.NodeData);
						}
					}
				}
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Logical, this);
			MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Physical, this);
			MyCubeGridGroups.Static.AddNode(GridLinkTypeEnum.Mechanical, this);
			if (!base.IsPreview)
			{
				MyGridPhysicalHierarchy.Static.AddNode(this);
			}
			if (IsStatic)
			{
				MyFixedGrids.MarkGridRoot(this);
			}
			RecalculateGravity();
			UpdateGravity();
			MarkForUpdate();
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (!MyEntities.IsClosingAll)
			{
				MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Physical, this);
				MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Logical, this);
				MyCubeGridGroups.Static.RemoveNode(GridLinkTypeEnum.Mechanical, this);
			}
			if (!base.IsPreview)
			{
				MyGridPhysicalHierarchy.Static.RemoveNode(this);
			}
			MyFixedGrids.UnmarkGridRoot(this);
			ReleaseMerginGrids();
			if (m_unsafeBlocks.Count > 0)
			{
				MyUnsafeGridsSessionComponent.UnregisterGrid(this);
			}
		}

		protected override void BeforeDelete()
		{
			SendRemovedBlocks();
			SendRemovedBlocksWithIds();
			RemoveAuthorshipAll();
			m_cubes.Clear();
			m_targetingList.Clear();
			if (MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS_QUICK_UPDATE)
			{
				MyEntity3DSoundEmitter.UpdateEntityEmitters(removeUnused: true, updatePlaying: false, updateNotPlaying: false);
			}
			MyEntities.Remove(this);
			UnregisterBlocksBeforeClose();
			Render.CloseModelGenerators();
			base.BeforeDelete();
			GridCounter--;
		}

		private void UnregisterBlocks(List<MyCubeBlock> cubeBlocks)
		{
			foreach (MyCubeBlock cubeBlock in cubeBlocks)
			{
				GridSystems.UnregisterFromSystems(cubeBlock);
			}
		}

		private void UnregisterBlocksBeforeClose()
		{
			GridSystems.BeforeGridClose();
			UnregisterBlocks(m_fatBlocks.List);
			GridSystems.AfterGridClose();
		}

		public override bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? tri, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			bool intersectionWithLine = GetIntersectionWithLine(ref line, ref m_hitInfoTmp, flags);
			if (intersectionWithLine)
			{
				tri = m_hitInfoTmp.Triangle;
				return intersectionWithLine;
			}
			tri = null;
			return intersectionWithLine;
		}

		public bool GetIntersectionWithLine(ref LineD line, ref MyCubeGridHitInfo info, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			if (info == null)
			{
				info = new MyCubeGridHitInfo();
			}
			info.Reset();
			if (base.IsPreview)
			{
				return false;
			}
			if (Projector != null)
			{
				return false;
			}
			RayCastCells(line.From, line.To, m_cacheRayCastCells);
			if (m_cacheRayCastCells.Count == 0)
			{
				return false;
			}
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell))
				{
					MyCube myCube = m_cubes[cacheRayCastCell];
					GetBlockIntersection(myCube, ref line, flags, out MyIntersectionResultLineTriangleEx? t, out int cubePartIndex);
					if (t.HasValue)
					{
						info.Position = myCube.CubeBlock.Position;
						info.Triangle = t.Value;
						info.CubePartIndex = cubePartIndex;
						info.Triangle.UserObject = myCube;
						return true;
					}
				}
			}
			return false;
		}

		internal bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? t, out MySlimBlock slimBlock, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			t = null;
			slimBlock = null;
			RayCastCells(line.From, line.To, m_cacheRayCastCells);
			if (m_cacheRayCastCells.Count == 0)
			{
				return false;
			}
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell))
				{
					MyCube myCube = m_cubes[cacheRayCastCell];
					GetBlockIntersection(myCube, ref line, flags, out t, out int _);
					if (t.HasValue)
					{
						slimBlock = myCube.CubeBlock;
						break;
					}
				}
			}
			if (slimBlock != null && slimBlock.FatBlock is MyCompoundCubeBlock)
			{
				ListReader<MySlimBlock> blocks = (slimBlock.FatBlock as MyCompoundCubeBlock).GetBlocks();
				double num = double.MaxValue;
				MySlimBlock mySlimBlock = null;
				for (int i = 0; i < blocks.Count; i++)
				{
					MySlimBlock mySlimBlock2 = blocks.ItemAt(i);
					if (mySlimBlock2.FatBlock.GetIntersectionWithLine(ref line, out MyIntersectionResultLineTriangleEx? t2) && t2.HasValue)
					{
						double num2 = (t2.Value.IntersectionPointInWorldSpace - line.From).LengthSquared();
						if (num2 < num)
						{
							num = num2;
							mySlimBlock = mySlimBlock2;
						}
					}
				}
				slimBlock = mySlimBlock;
			}
			return t.HasValue;
		}

		public override bool GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			try
			{
				BoundingBoxD boundingBoxD = new BoundingBoxD(sphere.Center - new Vector3D(sphere.Radius), sphere.Center + new Vector3D(sphere.Radius));
				MatrixD m = MatrixD.Invert(base.WorldMatrix);
				boundingBoxD = boundingBoxD.TransformFast(ref m);
				Vector3 vector = boundingBoxD.Min;
				Vector3 vector2 = boundingBoxD.Max;
				Vector3I value = new Vector3I((int)Math.Round(vector.X * GridSizeR), (int)Math.Round(vector.Y * GridSizeR), (int)Math.Round(vector.Z * GridSizeR));
				Vector3I value2 = new Vector3I((int)Math.Round(vector2.X * GridSizeR), (int)Math.Round(vector2.Y * GridSizeR), (int)Math.Round(vector2.Z * GridSizeR));
				Vector3I vector3I = Vector3I.Min(value, value2);
				Vector3I vector3I2 = Vector3I.Max(value, value2);
				for (int i = vector3I.X; i <= vector3I2.X; i++)
				{
					for (int j = vector3I.Y; j <= vector3I2.Y; j++)
					{
						for (int k = vector3I.Z; k <= vector3I2.Z; k++)
						{
							if (m_cubes.ContainsKey(new Vector3I(i, j, k)))
							{
								MyCube myCube = m_cubes[new Vector3I(i, j, k)];
								if (myCube.CubeBlock.FatBlock == null || myCube.CubeBlock.FatBlock.Model == null)
								{
									if (myCube.CubeBlock.BlockDefinition.CubeDefinition.CubeTopology == MyCubeTopology.Box)
									{
										return true;
									}
									MyCubePart[] parts = myCube.Parts;
									foreach (MyCubePart obj in parts)
									{
										Vector3D v = Vector3D.Transform(matrix: (MatrixD)Matrix.Invert(obj.InstanceData.LocalMatrix * base.WorldMatrix), position: sphere.Center);
										BoundingSphere localSphere = new BoundingSphere(v, (float)sphere.Radius);
										if (obj.Model.GetTrianglePruningStructure().GetIntersectionWithSphere(ref localSphere))
										{
											return true;
										}
									}
								}
								else
								{
									MatrixD matrix2 = Matrix.Invert(myCube.CubeBlock.FatBlock.WorldMatrix);
									_ = (BoundingSphereD)new BoundingSphere(Vector3D.Transform(sphere.Center, matrix2), (float)sphere.Radius);
									bool intersectionWithSphere = myCube.CubeBlock.FatBlock.Model.GetTrianglePruningStructure().GetIntersectionWithSphere(myCube.CubeBlock.FatBlock, ref sphere);
									if (intersectionWithSphere)
									{
										return intersectionWithSphere;
									}
								}
							}
						}
					}
				}
				return false;
			}
			finally
			{
			}
		}

		public override string ToString()
		{
			string text = IsStatic ? "S" : "D";
			string text2 = GridSizeEnum.ToString();
			return "Grid_" + text + "_" + text2 + "_" + m_cubeBlocks.Count + " {" + base.EntityId.ToString("X8") + "}";
		}

		public Vector3I WorldToGridInteger(Vector3D coords)
		{
			return Vector3I.Round(Vector3D.Transform(coords, base.PositionComp.WorldMatrixNormalizedInv) * GridSizeR);
		}

		public Vector3D WorldToGridScaledLocal(Vector3D coords)
		{
			return Vector3D.Transform(coords, base.PositionComp.WorldMatrixNormalizedInv) * GridSizeR;
		}

		public static Vector3D GridIntegerToWorld(float gridSize, Vector3I gridCoords, MatrixD worldMatrix)
		{
			return Vector3D.Transform((Vector3D)(Vector3)gridCoords * (double)gridSize, worldMatrix);
		}

		public Vector3D GridIntegerToWorld(Vector3I gridCoords)
		{
			return GridIntegerToWorld(GridSize, gridCoords, base.WorldMatrix);
		}

		public Vector3D GridIntegerToWorld(Vector3D gridCoords)
		{
			return Vector3D.Transform(gridCoords * GridSize, base.WorldMatrix);
		}

		public Vector3I LocalToGridInteger(Vector3 localCoords)
		{
			localCoords *= GridSizeR;
			return Vector3I.Round(localCoords);
		}

		public bool CanAddCubes(Vector3I min, Vector3I max)
		{
			Vector3I next = min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref min, ref max);
			while (vector3I_RangeIterator.IsValid())
			{
				if (m_cubes.ContainsKey(next))
				{
					return false;
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			return true;
		}

		public bool CanAddCubes(Vector3I min, Vector3I max, MyBlockOrientation? orientation, MyCubeBlockDefinition definition)
		{
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && definition != null)
			{
				Vector3I next = min;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref min, ref max);
				while (vector3I_RangeIterator.IsValid())
				{
					if (!CanAddCube(next, orientation, definition))
					{
						return false;
					}
					vector3I_RangeIterator.GetNext(out next);
				}
				return true;
			}
			return CanAddCubes(min, max);
		}

		public bool CanAddCube(Vector3I pos, MyBlockOrientation? orientation, MyCubeBlockDefinition definition, bool ignoreSame = false)
		{
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && definition != null)
			{
				if (!CubeExists(pos))
				{
					return true;
				}
				MySlimBlock cubeBlock = GetCubeBlock(pos);
				if (cubeBlock != null)
				{
					MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
					if (myCompoundCubeBlock != null)
					{
						return myCompoundCubeBlock.CanAddBlock(definition, orientation, 0, ignoreSame);
					}
				}
				return false;
			}
			return !CubeExists(pos);
		}

		public void ClearSymmetries()
		{
			XSymmetryPlane = null;
			YSymmetryPlane = null;
			ZSymmetryPlane = null;
		}

		public bool IsTouchingAnyNeighbor(Vector3I min, Vector3I max)
		{
			Vector3I min2 = min;
			min2.X--;
			Vector3I max2 = max;
			max2.X = min2.X;
			if (!CanAddCubes(min2, max2))
			{
				return true;
			}
			Vector3I min3 = min;
			min3.Y--;
			Vector3I max3 = max;
			max3.Y = min3.Y;
			if (!CanAddCubes(min3, max3))
			{
				return true;
			}
			Vector3I min4 = min;
			min4.Z--;
			Vector3I max4 = max;
			max4.Z = min4.Z;
			if (!CanAddCubes(min4, max4))
			{
				return true;
			}
			Vector3I max5 = max;
			max5.X++;
			Vector3I min5 = min;
			min5.X = max5.X;
			if (!CanAddCubes(min5, max5))
			{
				return true;
			}
			Vector3I max6 = max;
			max6.Y++;
			Vector3I min6 = min;
			min6.Y = max6.Y;
			if (!CanAddCubes(min6, max6))
			{
				return true;
			}
			Vector3I max7 = max;
			max7.Z++;
			Vector3I min7 = min;
			min7.Z = max7.Z;
			if (!CanAddCubes(min7, max7))
			{
				return true;
			}
			return false;
		}

		public bool CanPlaceBlock(Vector3I min, Vector3I max, MyBlockOrientation orientation, MyCubeBlockDefinition definition, ulong placingPlayer = 0uL, int? ignoreMultiblockId = null, bool ignoreFracturedPieces = false)
		{
			MyGridPlacementSettings gridSettings = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(GridSizeEnum, IsStatic);
			return CanPlaceBlock(min, max, orientation, definition, ref gridSettings, placingPlayer, ignoreMultiblockId, ignoreFracturedPieces);
		}

		public bool CanPlaceBlock(Vector3I min, Vector3I max, MyBlockOrientation orientation, MyCubeBlockDefinition definition, ref MyGridPlacementSettings gridSettings, ulong placingPlayer = 0uL, int? ignoreMultiblockId = null, bool ignoreFracturedPieces = false)
		{
			if (!CanAddCubes(min, max, orientation, definition))
			{
				return false;
			}
			if (MyFakes.ENABLE_MULTIBLOCKS && MyFakes.ENABLE_MULTIBLOCK_CONSTRUCTION && !CanAddOtherBlockInMultiBlock(min, max, orientation, definition, ignoreMultiblockId))
			{
				return false;
			}
			return TestPlacementAreaCube(this, ref gridSettings, min, max, orientation, definition, placingPlayer, this, ignoreFracturedPieces);
		}

		private bool IsWithinWorldLimits(long ownerID, int blocksToBuild, int pcu, string name)
		{
			string failedBlockType;
			return MySession.Static.IsWithinWorldLimits(out failedBlockType, ownerID, name, pcu, blocksToBuild, BlocksCount) == MySession.LimitResult.Passed;
		}

		public void SetCubeDirty(Vector3I pos)
		{
			m_dirtyRegion.AddCube(pos);
			MySlimBlock cubeBlock = GetCubeBlock(pos);
			if (cubeBlock != null)
			{
				Physics.AddDirtyBlock(cubeBlock);
			}
			MarkForUpdate();
			MarkForDraw();
		}

		public void SetBlockDirty(MySlimBlock cubeBlock)
		{
			Vector3I next = cubeBlock.Min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref cubeBlock.Min, ref cubeBlock.Max);
			while (vector3I_RangeIterator.IsValid())
			{
				m_dirtyRegion.AddCube(next);
				vector3I_RangeIterator.GetNext(out next);
			}
			MarkForUpdate();
			MarkForDraw();
		}

		public void DebugDrawRange(Vector3I min, Vector3I max)
		{
			Vector3I next = min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref min, ref max);
			while (vector3I_RangeIterator.IsValid())
			{
				_ = next + 1;
				MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(next * GridSize, GridSizeHalfVector, Quaternion.Identity);
				obb.Transform(base.WorldMatrix);
				MyRenderProxy.DebugDrawOBB(obb, Color.White, 0.5f, depthRead: true, smooth: false);
				vector3I_RangeIterator.GetNext(out next);
			}
		}

		public void DebugDrawPositions(List<Vector3I> positions)
		{
			foreach (Vector3I position in positions)
			{
				_ = position + 1;
				MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(position * GridSize, GridSizeHalfVector, Quaternion.Identity);
				obb.Transform(base.WorldMatrix);
				MyRenderProxy.DebugDrawOBB(obb, Color.White.ToVector3(), 0.5f, depthRead: true, smooth: false);
			}
		}

		private MyObjectBuilder_CubeBlock UpgradeCubeBlock(MyObjectBuilder_CubeBlock block, out MyCubeBlockDefinition blockDefinition)
		{
			MyDefinitionId id = block.GetId();
			if (MyFakes.ENABLE_COMPOUND_BLOCKS)
			{
				if (block is MyObjectBuilder_CompoundCubeBlock)
				{
					MyObjectBuilder_CompoundCubeBlock myObjectBuilder_CompoundCubeBlock = block as MyObjectBuilder_CompoundCubeBlock;
					blockDefinition = MyCompoundCubeBlock.GetCompoundCubeBlockDefinition();
					if (blockDefinition == null)
					{
						return null;
					}
					if (myObjectBuilder_CompoundCubeBlock.Blocks.Length == 1)
					{
						MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = myObjectBuilder_CompoundCubeBlock.Blocks[0];
						if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(myObjectBuilder_CubeBlock.GetId(), out MyCubeBlockDefinition blockDefinition2) && !MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition2))
						{
							blockDefinition = blockDefinition2;
							return myObjectBuilder_CubeBlock;
						}
					}
					return block;
				}
				if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition) && MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition))
				{
					MyObjectBuilder_CompoundCubeBlock result = MyCompoundCubeBlock.CreateBuilder(block);
					MyCubeBlockDefinition compoundCubeBlockDefinition = MyCompoundCubeBlock.GetCompoundCubeBlockDefinition();
					if (compoundCubeBlockDefinition != null)
					{
						blockDefinition = compoundCubeBlockDefinition;
						return result;
					}
				}
			}
			if (block is MyObjectBuilder_Ladder)
			{
				MyObjectBuilder_Passage myObjectBuilder_Passage = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Passage>(block.SubtypeName);
				myObjectBuilder_Passage.BlockOrientation = block.BlockOrientation;
				myObjectBuilder_Passage.BuildPercent = block.BuildPercent;
				myObjectBuilder_Passage.EntityId = block.EntityId;
				myObjectBuilder_Passage.IntegrityPercent = block.IntegrityPercent;
				myObjectBuilder_Passage.Min = block.Min;
				blockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Passage), block.SubtypeId));
				block = myObjectBuilder_Passage;
				return block;
			}
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock2 = block;
			string[] array = new string[7]
			{
				"Red",
				"Yellow",
				"Blue",
				"Green",
				"Black",
				"White",
				"Gray"
			};
			Vector3[] array2 = new Vector3[7]
			{
				MyRenderComponentBase.OldRedToHSV,
				MyRenderComponentBase.OldYellowToHSV,
				MyRenderComponentBase.OldBlueToHSV,
				MyRenderComponentBase.OldGreenToHSV,
				MyRenderComponentBase.OldBlackToHSV,
				MyRenderComponentBase.OldWhiteToHSV,
				MyRenderComponentBase.OldGrayToHSV
			};
			if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition))
			{
				myObjectBuilder_CubeBlock2 = FindDefinitionUpgrade(block, out blockDefinition);
				if (myObjectBuilder_CubeBlock2 == null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (id.SubtypeName.EndsWith(array[i], StringComparison.InvariantCultureIgnoreCase))
						{
							string subtypeName = id.SubtypeName.Substring(0, id.SubtypeName.Length - array[i].Length);
							MyDefinitionId defId = new MyDefinitionId(id.TypeId, subtypeName);
							if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out blockDefinition))
							{
								myObjectBuilder_CubeBlock2 = block;
								myObjectBuilder_CubeBlock2.ColorMaskHSV = array2[i];
								myObjectBuilder_CubeBlock2.SubtypeName = subtypeName;
								return myObjectBuilder_CubeBlock2;
							}
						}
					}
				}
				if (myObjectBuilder_CubeBlock2 == null)
				{
					return null;
				}
			}
			return myObjectBuilder_CubeBlock2;
		}

		private MySlimBlock AddBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge)
		{
			try
			{
				if (Skeleton == null)
				{
					Skeleton = new MyGridSkeleton();
				}
				objectBuilder = UpgradeCubeBlock(objectBuilder, out MyCubeBlockDefinition blockDefinition);
				if (objectBuilder != null)
				{
					try
					{
						return AddCubeBlock(objectBuilder, testMerge, blockDefinition);
					}
					catch (DuplicateIdException ex)
					{
						string msg = "ERROR while adding cube " + blockDefinition.DisplayNameText.ToString() + ": " + ex.ToString();
						MyLog.Default.WriteLine(msg);
						return null;
					}
				}
				return null;
			}
			finally
			{
			}
		}

		private MySlimBlock AddCubeBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge, MyCubeBlockDefinition blockDefinition)
		{
			Vector3I min = objectBuilder.Min;
			MySlimBlock.ComputeMax(blockDefinition, objectBuilder.BlockOrientation, ref min, out Vector3I max);
			if (!CanAddCubes(min, max))
			{
				return null;
			}
			object obj = MyCubeBlockFactory.CreateCubeBlock(objectBuilder);
			MySlimBlock cubeBlock = obj as MySlimBlock;
			if (cubeBlock == null)
			{
				cubeBlock = new MySlimBlock();
			}
			if (!cubeBlock.Init(objectBuilder, this, obj as MyCubeBlock))
			{
				return null;
			}
			if (cubeBlock.FatBlock is MyCompoundCubeBlock && (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocksCount() == 0)
			{
				return null;
			}
			if (cubeBlock.FatBlock != null)
			{
				cubeBlock.FatBlock.Render.FadeIn = Render.FadeIn;
				cubeBlock.FatBlock.HookMultiplayer();
			}
			cubeBlock.AddNeighbours();
			BoundsInclude(cubeBlock);
			if (cubeBlock.FatBlock != null)
			{
				base.Hierarchy.AddChild(cubeBlock.FatBlock);
				GridSystems.RegisterInSystems(cubeBlock.FatBlock);
				if (cubeBlock.FatBlock.Render.NeedsDrawFromParent)
				{
					m_blocksForDraw.Add(cubeBlock.FatBlock);
					cubeBlock.FatBlock.Render.SetVisibilityUpdates(state: true);
				}
				MyObjectBuilderType typeId = cubeBlock.BlockDefinition.Id.TypeId;
				if (typeId != typeof(MyObjectBuilder_CubeBlock))
				{
					if (!BlocksCounters.ContainsKey(typeId))
					{
						BlocksCounters.Add(typeId, 0);
					}
					BlocksCounters[typeId]++;
				}
			}
			m_cubeBlocks.Add(cubeBlock);
			if (cubeBlock.FatBlock != null)
			{
				m_fatBlocks.Add(cubeBlock.FatBlock);
			}
			if (!m_colorStatistics.ContainsKey(cubeBlock.ColorMaskHSV))
			{
				m_colorStatistics.Add(cubeBlock.ColorMaskHSV, 0);
			}
			m_colorStatistics[cubeBlock.ColorMaskHSV]++;
			((MyBlockOrientation)objectBuilder.BlockOrientation).GetMatrix(out Matrix result);
			MyCubeGridDefinitions.GetRotatedBlockSize(blockDefinition, ref result, out Vector3I _);
			Vector3I normal = blockDefinition.Center;
			Vector3I.TransformNormal(ref normal, ref result, out Vector3I _);
			bool flag = true;
			Vector3I pos = cubeBlock.Min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref cubeBlock.Min, ref cubeBlock.Max);
			while (vector3I_RangeIterator.IsValid())
			{
				flag &= AddCube(cubeBlock, ref pos, result, blockDefinition);
				vector3I_RangeIterator.GetNext(out pos);
			}
			if (Physics != null)
			{
				Physics.AddBlock(cubeBlock);
			}
			FixSkeleton(cubeBlock);
			cubeBlock.AddAuthorship();
			if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
			{
				AddMultiBlockInfo(cubeBlock);
			}
			if (testMerge)
			{
				MyCubeGrid myCubeGrid = DetectMerge(cubeBlock);
				if (myCubeGrid != null && myCubeGrid != this)
				{
					cubeBlock = myCubeGrid.GetCubeBlock(cubeBlock.Position);
					myCubeGrid.AdditionalModelGenerators.ForEach(delegate(IMyBlockAdditionalModelGenerator g)
					{
						g.BlockAddedToMergedGrid(cubeBlock);
					});
				}
				else
				{
					NotifyBlockAdded(cubeBlock);
				}
			}
			else
			{
				NotifyBlockAdded(cubeBlock);
			}
			m_PCU += (cubeBlock.ComponentStack.IsFunctional ? cubeBlock.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
			if (cubeBlock.FatBlock is MyReactor)
			{
				NumberOfReactors++;
			}
			MarkForUpdate();
			MarkForDraw();
			return cubeBlock;
		}

		public void FixSkeleton(MySlimBlock cubeBlock)
		{
			float maxBoneError = MyGridSkeleton.GetMaxBoneError(GridSize);
			maxBoneError *= maxBoneError;
			Vector3I end = (cubeBlock.Min + Vector3I.One) * 2;
			Vector3I start = cubeBlock.Min * 2;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
			while (vector3I_RangeIterator.IsValid())
			{
				Vector3 definitionOffsetWithNeighbours = Skeleton.GetDefinitionOffsetWithNeighbours(cubeBlock.Min, start, this);
				if (definitionOffsetWithNeighbours.LengthSquared() < maxBoneError)
				{
					Skeleton.Bones.Remove(start);
				}
				else
				{
					Skeleton.Bones[start] = definitionOffsetWithNeighbours;
				}
				vector3I_RangeIterator.GetNext(out start);
			}
			if (cubeBlock.BlockDefinition.Skeleton == null || cubeBlock.BlockDefinition.Skeleton.Count <= 0 || Physics == null)
			{
				return;
			}
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					for (int k = -1; k <= 1; k++)
					{
						SetCubeDirty(new Vector3I(i, j, k) + cubeBlock.Min);
					}
				}
			}
		}

		public void EnqueueDestructionDeformationBlock(Vector3I position)
		{
			if (Sync.IsServer)
			{
				m_destructionDeformationQueue.Add(position);
				MarkForUpdate();
			}
		}

		public void EnqueueDestroyedBlock(Vector3I position)
		{
			if (Sync.IsServer)
			{
				m_destroyBlockQueue.Add(position);
				MarkForUpdate();
			}
		}

		public void EnqueueRemovedBlock(Vector3I position, bool generatorsEnabled)
		{
			if (Sync.IsServer)
			{
				if (generatorsEnabled)
				{
					m_removeBlockQueueWithGenerators.Add(position);
				}
				else
				{
					m_removeBlockQueueWithoutGenerators.Add(position);
				}
				MarkForUpdate();
			}
		}

		public void SendRemovedBlocks()
		{
			if (m_removeBlockQueueWithGenerators.Count > 0 || m_destroyBlockQueue.Count > 0 || m_destructionDeformationQueue.Count > 0 || m_removeBlockQueueWithoutGenerators.Count > 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RemovedBlocks, m_removeBlockQueueWithGenerators, m_destroyBlockQueue, m_destructionDeformationQueue, m_removeBlockQueueWithoutGenerators);
				m_removeBlockQueueWithGenerators.Clear();
				m_removeBlockQueueWithoutGenerators.Clear();
				m_destroyBlockQueue.Clear();
				m_destructionDeformationQueue.Clear();
			}
		}

		[Event(null, 3525)]
		[Reliable]
		[Broadcast]
		private void RemovedBlocks(List<Vector3I> locationsWithGenerator, List<Vector3I> destroyLocations, List<Vector3I> DestructionDeformationLocation, List<Vector3I> LocationsWithoutGenerator)
		{
			if (destroyLocations.Count > 0)
			{
				BlocksDestroyed(destroyLocations);
			}
			if (locationsWithGenerator.Count > 0)
			{
				BlocksRemovedWithGenerator(locationsWithGenerator);
			}
			if (LocationsWithoutGenerator.Count > 0)
			{
				BlocksRemovedWithoutGenerator(LocationsWithoutGenerator);
			}
			if (DestructionDeformationLocation.Count > 0)
			{
				BlocksDeformed(DestructionDeformationLocation);
			}
		}

		public void EnqueueRemovedBlockWithId(Vector3I position, ushort? compoundId, bool generatorsEnabled)
		{
			if (Sync.IsServer)
			{
				BlockPositionId blockPositionId = default(BlockPositionId);
				blockPositionId.Position = position;
				blockPositionId.CompoundId = (uint)(((int?)compoundId) ?? (-1));
				BlockPositionId item = blockPositionId;
				if (generatorsEnabled)
				{
					m_removeBlockWithIdQueueWithGenerators.Add(item);
				}
				else
				{
					m_removeBlockWithIdQueueWithoutGenerators.Add(item);
				}
				MarkForUpdate();
			}
		}

		public void EnqueueDestroyedBlockWithId(Vector3I position, ushort? compoundId, bool generatorEnabled)
		{
			if (Sync.IsServer)
			{
				BlockPositionId item;
				if (generatorEnabled)
				{
					List<BlockPositionId> destroyBlockWithIdQueueWithGenerators = m_destroyBlockWithIdQueueWithGenerators;
					item = new BlockPositionId
					{
						Position = position,
						CompoundId = (uint)(((int?)compoundId) ?? (-1))
					};
					destroyBlockWithIdQueueWithGenerators.Add(item);
				}
				else
				{
					List<BlockPositionId> destroyBlockWithIdQueueWithoutGenerators = m_destroyBlockWithIdQueueWithoutGenerators;
					item = new BlockPositionId
					{
						Position = position,
						CompoundId = (uint)(((int?)compoundId) ?? (-1))
					};
					destroyBlockWithIdQueueWithoutGenerators.Add(item);
				}
				MarkForUpdate();
			}
		}

		public void SendRemovedBlocksWithIds()
		{
			if (m_removeBlockWithIdQueueWithGenerators.Count > 0 || m_removeBlockWithIdQueueWithoutGenerators.Count > 0 || m_destroyBlockWithIdQueueWithGenerators.Count > 0 || m_destroyBlockWithIdQueueWithoutGenerators.Count > 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RemovedBlocksWithIds, m_removeBlockWithIdQueueWithGenerators, m_destroyBlockWithIdQueueWithGenerators, m_destroyBlockWithIdQueueWithoutGenerators, m_removeBlockWithIdQueueWithoutGenerators);
				m_removeBlockWithIdQueueWithGenerators.Clear();
				m_removeBlockWithIdQueueWithoutGenerators.Clear();
				m_destroyBlockWithIdQueueWithGenerators.Clear();
				m_destroyBlockWithIdQueueWithoutGenerators.Clear();
			}
		}

		[Event(null, 3593)]
		[Reliable]
		[Broadcast]
		private void RemovedBlocksWithIds(List<BlockPositionId> removeBlockWithIdQueueWithGenerators, List<BlockPositionId> destroyBlockWithIdQueueWithGenerators, List<BlockPositionId> destroyBlockWithIdQueueWithoutGenerators, List<BlockPositionId> removeBlockWithIdQueueWithoutGenerators)
		{
			if (destroyBlockWithIdQueueWithGenerators.Count > 0)
			{
				BlocksWithIdDestroyedWithGenerator(destroyBlockWithIdQueueWithGenerators);
			}
			if (destroyBlockWithIdQueueWithoutGenerators.Count > 0)
			{
				BlocksWithIdDestroyedWithoutGenerator(destroyBlockWithIdQueueWithoutGenerators);
			}
			if (removeBlockWithIdQueueWithGenerators.Count > 0)
			{
				BlocksWithIdRemovedWithGenerator(removeBlockWithIdQueueWithGenerators);
			}
			if (removeBlockWithIdQueueWithoutGenerators.Count > 0)
			{
				BlocksWithIdRemovedWithoutGenerator(removeBlockWithIdQueueWithoutGenerators);
			}
		}

		[Event(null, 3617)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		public void RemoveBlocksBuiltByID(long identityID)
		{
			foreach (MySlimBlock item in FindBlocksBuiltByID(identityID))
			{
				RemoveBlock(item, updatePhysics: true);
			}
		}

		[Event(null, 3629)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		public void TransferBlocksBuiltByID(long oldAuthor, long newAuthor)
		{
			foreach (MySlimBlock item in FindBlocksBuiltByID(oldAuthor))
			{
				item.TransferAuthorship(newAuthor);
			}
		}

		public void TransferBlocksBuiltByIDClient(long oldAuthor, long newAuthor)
		{
			foreach (MySlimBlock item in FindBlocksBuiltByID(oldAuthor))
			{
				item.TransferAuthorshipClient(newAuthor);
			}
		}

		public void TransferBlockLimitsBuiltByID(long author, MyBlockLimits oldLimits, MyBlockLimits newLimits)
		{
			foreach (MySlimBlock item in FindBlocksBuiltByID(author))
			{
				item.TransferLimits(oldLimits, newLimits);
			}
		}

		public HashSet<MySlimBlock> FindBlocksBuiltByID(long identityID)
		{
			return FindBlocksBuiltByID(identityID, new HashSet<MySlimBlock>());
		}

		public HashSet<MySlimBlock> FindBlocksBuiltByID(long identityID, HashSet<MySlimBlock> builtBlocks)
		{
			foreach (MySlimBlock cubeBlock in m_cubeBlocks)
			{
				if (cubeBlock.BuiltBy == identityID)
				{
					builtBlocks.Add(cubeBlock);
				}
			}
			return builtBlocks;
		}

		public MySlimBlock BuildGeneratedBlock(MyBlockLocation location, Vector3 colorMaskHsv, MyStringHash skinId)
		{
			MyDefinitionId id = location.BlockDefinition;
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
			location.Orientation.GetQuaternion(out Quaternion result);
			return BuildBlock(cubeBlockDefinition, colorMaskHsv, skinId, location.Min, result, location.Owner, location.EntityId, null);
		}

		[Event(null, 3683)]
		[Reliable]
		[Server]
		public void BuildBlockRequest(uint colorMaskHsv, MyBlockLocation location, [DynamicObjectBuilder(false)] MyObjectBuilder_CubeBlock blockObjectBuilder, long builderEntityId, bool instantBuild, long ownerId)
		{
			BuildBlockRequestInternal(new MyBlockVisuals(colorMaskHsv, MyStringHash.NullOrEmpty, applyColor: true, applySkin: false), location, blockObjectBuilder, builderEntityId, instantBuild, ownerId, MyEventContext.Current.Sender.Value);
		}

		[Event(null, 3690)]
		[Reliable]
		[Server]
		public void BuildBlockRequest(MyBlockVisuals visuals, MyBlockLocation location, [DynamicObjectBuilder(false)] MyObjectBuilder_CubeBlock blockObjectBuilder, long builderEntityId, bool instantBuild, long ownerId)
		{
			BuildBlockRequestInternal(visuals, location, blockObjectBuilder, builderEntityId, instantBuild, ownerId, MyEventContext.Current.Sender.Value);
		}

		public void BuildBlockRequestInternal(MyBlockVisuals visuals, MyBlockLocation location, MyObjectBuilder_CubeBlock blockObjectBuilder, long builderEntityId, bool instantBuild, long ownerId, ulong sender)
		{
			MyEntity entity = null;
			MyEntities.TryGetEntityById(builderEntityId, out entity);
			bool flag = sender == Sync.MyId || MySession.Static.HasPlayerCreativeRights(sender);
			if ((entity == null && !flag && !MySession.Static.CreativeMode) || !MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Building, builderEntityId, 0uL))
			{
				return;
			}
			if (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(location.BlockDefinition, sender) || (MySession.Static.ResearchEnabled && !flag && !MySessionComponentResearch.Static.CanUse(ownerId, location.BlockDefinition)))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(sender);
				return;
			}
			MyBlockLocation? resultBlock = null;
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(location.BlockDefinition, out MyCubeBlockDefinition blockDefinition);
			MyBlockOrientation orientation = location.Orientation;
			location.Orientation.GetQuaternion(out Quaternion result);
			MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = blockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
			int? ignoreMultiblockId = (blockObjectBuilder != null && blockObjectBuilder.MultiBlockId != 0) ? new int?(blockObjectBuilder.MultiBlockId) : null;
			Vector3I position = location.CenterPos;
			visuals.SkinId = (MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(visuals.SkinId, sender) ?? MyStringHash.NullOrEmpty);
			if (CanPlaceBlock(location.Min, location.Max, orientation, blockDefinition, 0uL, ignoreMultiblockId) && CheckConnectivity(this, blockDefinition, buildProgressModelMountPoints, ref result, ref position))
			{
				MySlimBlock mySlimBlock = BuildBlockSuccess(ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV), visuals.SkinId, location, blockObjectBuilder, ref resultBlock, entity, flag && instantBuild, ownerId);
				if (mySlimBlock != null && resultBlock.HasValue)
				{
					MyMultiplayer.RaiseEvent(mySlimBlock.CubeGrid, (MyCubeGrid x) => x.BuildBlockSucess, visuals, location, blockObjectBuilder, builderEntityId, flag && instantBuild, ownerId);
					AfterBuildBlockSuccess(resultBlock.Value, instantBuild);
				}
			}
		}

		[Event(null, 3746)]
		[Reliable]
		[Broadcast]
		public void BuildBlockSucess(MyBlockVisuals visuals, MyBlockLocation location, [DynamicObjectBuilder(false)] MyObjectBuilder_CubeBlock blockObjectBuilder, long builderEntityId, bool instantBuild, long ownerId)
		{
			MyEntity entity = null;
			MyEntities.TryGetEntityById(builderEntityId, out entity);
			MyBlockLocation? resultBlock = null;
			BuildBlockSuccess(ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV), visuals.SkinId, location, blockObjectBuilder, ref resultBlock, entity, instantBuild, ownerId);
			if (resultBlock.HasValue)
			{
				AfterBuildBlockSuccess(resultBlock.Value, instantBuild);
			}
		}

		public void BuildBlocks(ref MyBlockBuildArea area, long builderEntityId, long ownerId)
		{
			int num = area.BuildAreaSize.X * area.BuildAreaSize.Y * area.BuildAreaSize.Z;
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
			if (MySession.Static.CheckLimitsAndNotify(ownerId, cubeBlockDefinition.BlockPairName, num * cubeBlockDefinition.PCU, num, BlocksCount))
			{
				ulong steamId = MySession.Static.Players.TryGetSteamId(ownerId);
				if (MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(cubeBlockDefinition, steamId))
				{
					bool arg = MySession.Static.CreativeToolsEnabled(Sync.MyId);
					MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksAreaRequest, area, builderEntityId, arg, ownerId, Sync.MyId);
				}
			}
		}

		public void BuildBlocks(Vector3 colorMaskHsv, MyStringHash skinId, HashSet<MyBlockLocation> locations, long builderEntityId, long ownerId)
		{
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(locations.First().BlockDefinition);
			string blockPairName = cubeBlockDefinition.BlockPairName;
			bool flag = MySession.Static.CreativeToolsEnabled(Sync.MyId);
			bool flag2 = flag || MySession.Static.CreativeMode;
			if (MySession.Static.CheckLimitsAndNotify(ownerId, blockPairName, flag2 ? (locations.Count * cubeBlockDefinition.PCU) : locations.Count, locations.Count, BlocksCount))
			{
				ulong steamId = MySession.Static.Players.TryGetSteamId(ownerId);
				if (MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(cubeBlockDefinition, steamId))
				{
					MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksRequest, new MyBlockVisuals(colorMaskHsv.PackHSVToUint(), skinId), locations, builderEntityId, flag, ownerId);
				}
			}
		}

		[Event(null, 3798)]
		[Reliable]
		[Server]
		private void BuildBlocksRequest(MyBlockVisuals visuals, HashSet<MyBlockLocation> locations, long builderEntityId, bool instantBuild, long ownerId)
		{
			if (!MySession.Static.CreativeMode && !MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				instantBuild = false;
			}
			m_tmpBuildList.Clear();
			MyEntity entity = null;
			MyEntities.TryGetEntityById(builderEntityId, out entity);
			MyCubeBuilder.BuildComponent.GetBlocksPlacementMaterials(locations, this);
			bool flag = MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeMode;
			bool flag2 = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
			if ((entity == null && !flag2 && !MySession.Static.CreativeMode) || !MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Building, builderEntityId, MyEventContext.Current.Sender.Value) || (!MyCubeBuilder.BuildComponent.HasBuildingMaterials(entity) && !flag2))
			{
				return;
			}
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(locations.First().BlockDefinition);
			string blockPairName = cubeBlockDefinition.BlockPairName;
			if (!IsWithinWorldLimits(ownerId, locations.Count, flag ? (locations.Count * cubeBlockDefinition.PCU) : locations.Count, blockPairName))
			{
				return;
			}
			Vector3 colorMaskHsv = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
			ulong value = MyEventContext.Current.Sender.Value;
			visuals.SkinId = (MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(visuals.SkinId, value) ?? MyStringHash.NullOrEmpty);
			BuildBlocksSuccess(colorMaskHsv, visuals.SkinId, locations, m_tmpBuildList, entity, flag2 && instantBuild, ownerId, MyEventContext.Current.Sender.Value);
			if (m_tmpBuildList.Count > 0)
			{
				MySession.Static.TotalBlocksCreated += (uint)m_tmpBuildList.Count;
				if (MySession.Static.ControlledEntity is MyCockpit)
				{
					MySession.Static.TotalBlocksCreatedFromShips += (uint)m_tmpBuildList.Count;
				}
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksClient, visuals, m_tmpBuildList, builderEntityId, flag2 && instantBuild, ownerId);
				if (Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalPlayerId == ownerId)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
				}
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksFailedNotify, new EndpointId(MyEventContext.Current.Sender.Value));
			}
			AfterBuildBlocksSuccess(m_tmpBuildList, instantBuild);
		}

		[Event(null, 3862)]
		[Reliable]
		[Client]
		public void BuildBlocksFailedNotify()
		{
			if (MyCubeBuilder.Static != null)
			{
				MyCubeBuilder.Static.NotifyPlacementUnable();
			}
		}

		[Event(null, 3869)]
		[Reliable]
		[Broadcast]
		public void BuildBlocksClient(MyBlockVisuals visuals, HashSet<MyBlockLocation> locations, long builderEntityId, bool instantBuild, long ownerId)
		{
			m_tmpBuildList.Clear();
			MyEntity entity = null;
			MyEntities.TryGetEntityById(builderEntityId, out entity);
			BuildBlocksSuccess(ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV), visuals.SkinId, locations, m_tmpBuildList, entity, instantBuild, ownerId, 0uL);
			if (ownerId == MySession.Static.LocalPlayerId)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
			}
			AfterBuildBlocksSuccess(m_tmpBuildList, instantBuild);
		}

		[Event(null, 3883)]
		[Reliable]
		[Server]
		private void BuildBlocksAreaRequest(MyBlockBuildArea area, long builderEntityId, bool instantBuild, long ownerId, ulong placingPlayer)
		{
			if (!MySession.Static.CreativeMode && !MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				instantBuild = false;
			}
			try
			{
				bool flag = MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeMode;
				bool flag2 = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
				if ((ownerId != 0L || flag2 || MySession.Static.CreativeMode) && MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Building, builderEntityId, placingPlayer))
				{
					MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
					int num = area.BuildAreaSize.X * area.BuildAreaSize.Y * area.BuildAreaSize.Z;
					if (IsWithinWorldLimits(ownerId, num, flag ? (num * cubeBlockDefinition.PCU) : num, cubeBlockDefinition.BlockPairName))
					{
						int amount = area.BuildAreaSize.X * area.BuildAreaSize.Y * area.BuildAreaSize.Z;
						MyCubeBuilder.BuildComponent.GetBlockAmountPlacementMaterials(cubeBlockDefinition, amount);
						MyEntity entity = null;
						MyEntities.TryGetEntityById(builderEntityId, out entity);
						if (MyCubeBuilder.BuildComponent.HasBuildingMaterials(entity, testTotal: true) || flag2)
						{
							GetValidBuildOffsets(ref area, m_tmpBuildOffsets, m_tmpBuildFailList, placingPlayer);
							CheckAreaConnectivity(this, ref area, m_tmpBuildOffsets, m_tmpBuildFailList);
							int num2 = MyRandom.Instance.CreateRandomSeed();
							area.SkinId = (MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(area.SkinId, MyEventContext.Current.Sender.Value) ?? MyStringHash.NullOrEmpty);
							MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlocksAreaClient, area, num2, m_tmpBuildFailList, builderEntityId, flag2, ownerId);
							BuildBlocksArea(ref area, m_tmpBuildOffsets, builderEntityId, flag2, ownerId, num2);
						}
					}
				}
			}
			finally
			{
				m_tmpBuildOffsets.Clear();
				m_tmpBuildFailList.Clear();
			}
		}

		[Event(null, 3939)]
		[Reliable]
		[Broadcast]
		private void BuildBlocksAreaClient(MyBlockBuildArea area, int entityIdSeed, HashSet<Vector3UByte> failList, long builderEntityId, bool isAdmin, long ownerId)
		{
			try
			{
				GetAllBuildOffsetsExcept(ref area, failList, m_tmpBuildOffsets);
				BuildBlocksArea(ref area, m_tmpBuildOffsets, builderEntityId, isAdmin, ownerId, entityIdSeed);
			}
			finally
			{
				m_tmpBuildOffsets.Clear();
			}
		}

		private void BuildBlocksArea(ref MyBlockBuildArea area, List<Vector3UByte> validOffsets, long builderEntityId, bool isAdmin, long ownerId, int entityIdSeed)
		{
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
			if (cubeBlockDefinition != null)
			{
				Quaternion orientation = Base6Directions.GetOrientation(area.OrientationForward, area.OrientationUp);
				Vector3I b = area.StepDelta;
				MyEntity entity = null;
				MyEntities.TryGetEntityById(builderEntityId, out entity);
				try
				{
					bool flag = false;
					validOffsets.Sort(Vector3UByte.Comparer);
					using (MyRandom.Instance.PushSeed(entityIdSeed))
					{
						foreach (Vector3UByte validOffset in validOffsets)
						{
							Vector3I a = area.PosInGrid + validOffset * b;
							MySlimBlock mySlimBlock = BuildBlock(cubeBlockDefinition, ColorExtensions.UnpackHSVFromUint(area.ColorMaskHSV), area.SkinId, a + area.BlockMin, orientation, ownerId, MyEntityIdentifier.AllocateId(), entity, null, updateVolume: false, testMerge: false, isAdmin);
							if (mySlimBlock != null)
							{
								ChangeBlockOwner(mySlimBlock, ownerId);
								flag = true;
								m_tmpBuildSuccessBlocks.Add(mySlimBlock);
								if (ownerId == MySession.Static.LocalPlayerId)
								{
									MySession.Static.TotalBlocksCreated++;
									if (MySession.Static.ControlledEntity is MyCockpit)
									{
										MySession.Static.TotalBlocksCreatedFromShips++;
									}
								}
							}
						}
					}
					BoundingBoxD boundingBox = BoundingBoxD.CreateInvalid();
					foreach (MySlimBlock tmpBuildSuccessBlock in m_tmpBuildSuccessBlocks)
					{
						tmpBuildSuccessBlock.GetWorldBoundingBox(out BoundingBoxD aabb);
						boundingBox.Include(aabb);
						if (tmpBuildSuccessBlock.FatBlock != null)
						{
							tmpBuildSuccessBlock.FatBlock.OnBuildSuccess(ownerId, isAdmin);
						}
					}
					if (m_tmpBuildSuccessBlocks.Count > 0)
					{
						if (IsStatic && Sync.IsServer)
						{
							List<MyEntity> entitiesInAABB = MyEntities.GetEntitiesInAABB(ref boundingBox);
							foreach (MySlimBlock tmpBuildSuccessBlock2 in m_tmpBuildSuccessBlocks)
							{
								DetectMerge(tmpBuildSuccessBlock2, null, entitiesInAABB);
							}
							entitiesInAABB.Clear();
						}
						m_tmpBuildSuccessBlocks[0].PlayConstructionSound(MyIntegrityChangeEnum.ConstructionBegin);
						UpdateGridAABB();
					}
					if (MySession.Static.LocalPlayerId == ownerId)
					{
						if (flag)
						{
							MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
						}
						else
						{
							MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
						}
					}
				}
				finally
				{
					m_tmpBuildSuccessBlocks.Clear();
				}
			}
		}

		private void GetAllBuildOffsetsExcept(ref MyBlockBuildArea area, HashSet<Vector3UByte> exceptList, List<Vector3UByte> resultOffsets)
		{
			Vector3UByte item = default(Vector3UByte);
			item.X = 0;
			while (item.X < area.BuildAreaSize.X)
			{
				item.Y = 0;
				while (item.Y < area.BuildAreaSize.Y)
				{
					item.Z = 0;
					while (item.Z < area.BuildAreaSize.Z)
					{
						if (!exceptList.Contains(item))
						{
							resultOffsets.Add(item);
						}
						item.Z++;
					}
					item.Y++;
				}
				item.X++;
			}
		}

		private void GetValidBuildOffsets(ref MyBlockBuildArea area, List<Vector3UByte> resultOffsets, HashSet<Vector3UByte> resultFailList, ulong placingPlayer = 0uL)
		{
			Vector3I b = area.StepDelta;
			MyBlockOrientation orientation = new MyBlockOrientation(area.OrientationForward, area.OrientationUp);
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
			Vector3UByte vector3UByte = default(Vector3UByte);
			vector3UByte.X = 0;
			while (vector3UByte.X < area.BuildAreaSize.X)
			{
				vector3UByte.Y = 0;
				while (vector3UByte.Y < area.BuildAreaSize.Y)
				{
					vector3UByte.Z = 0;
					while (vector3UByte.Z < area.BuildAreaSize.Z)
					{
						Vector3I a = area.PosInGrid + vector3UByte * b;
						if (CanPlaceBlock(a + area.BlockMin, a + area.BlockMax, orientation, cubeBlockDefinition, placingPlayer))
						{
							resultOffsets.Add(vector3UByte);
						}
						else
						{
							resultFailList.Add(vector3UByte);
						}
						vector3UByte.Z++;
					}
					vector3UByte.Y++;
				}
				vector3UByte.X++;
			}
		}

		private void BuildBlocksSuccess(Vector3 colorMaskHsv, MyStringHash skinId, HashSet<MyBlockLocation> locations, HashSet<MyBlockLocation> resultBlocks, MyEntity builder, bool instantBuilt, long ownerId, ulong placingPlayer = 0uL)
		{
			bool flag = true;
			while (locations.Count > 0 && flag)
			{
				flag = false;
				foreach (MyBlockLocation location in locations)
				{
					MyBlockOrientation orientation = location.Orientation;
					orientation.GetQuaternion(out Quaternion result);
					Vector3I center = location.CenterPos;
					MyDefinitionManager.Static.TryGetCubeBlockDefinition(location.BlockDefinition, out MyCubeBlockDefinition blockDefinition);
					if (blockDefinition == null)
					{
						return;
					}
					MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = blockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
					if (!Sync.IsServer || CanPlaceWithConnectivity(location, ref result, ref center, blockDefinition, buildProgressModelMountPoints, placingPlayer))
					{
						MySlimBlock mySlimBlock = BuildBlock(blockDefinition, colorMaskHsv, skinId, location.Min, result, location.Owner, location.EntityId, builder, null, updateVolume: true, testMerge: false, instantBuilt);
						if (mySlimBlock != null)
						{
							ChangeBlockOwner(mySlimBlock, ownerId);
							MyBlockLocation item = location;
							resultBlocks.Add(item);
						}
						flag = true;
						locations.Remove(location);
						break;
					}
				}
			}
		}

		private bool CanPlaceWithConnectivity(MyBlockLocation location, ref Quaternion orientation, ref Vector3I center, MyCubeBlockDefinition blockDefinition, MyCubeBlockDefinition.MountPoint[] mountPoints, ulong placingPlayer = 0uL)
		{
			if (CanPlaceBlock(location.Min, location.Max, location.Orientation, blockDefinition, placingPlayer))
			{
				return CheckConnectivity(this, blockDefinition, mountPoints, ref orientation, ref center);
			}
			return false;
		}

		private MySlimBlock BuildBlockSuccess(Vector3 colorMaskHsv, MyStringHash skinId, MyBlockLocation location, MyObjectBuilder_CubeBlock objectBuilder, ref MyBlockLocation? resultBlock, MyEntity builder, bool instantBuilt, long ownerId)
		{
			location.Orientation.GetQuaternion(out Quaternion result);
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(location.BlockDefinition, out MyCubeBlockDefinition blockDefinition);
			if (blockDefinition == null)
			{
				return null;
			}
			MySlimBlock mySlimBlock = BuildBlock(blockDefinition, colorMaskHsv, skinId, location.Min, result, location.Owner, location.EntityId, instantBuilt ? null : builder, objectBuilder);
			if (mySlimBlock != null)
			{
				ChangeBlockOwner(mySlimBlock, ownerId);
				resultBlock = location;
				mySlimBlock.PlayConstructionSound(MyIntegrityChangeEnum.ConstructionBegin);
			}
			else
			{
				resultBlock = null;
			}
			return mySlimBlock;
		}

		private static void ChangeBlockOwner(MySlimBlock block, long ownerId)
		{
			if (block.FatBlock != null)
			{
				block.FatBlock.ChangeOwner(ownerId, MyOwnershipShareModeEnum.Faction);
			}
		}

		private void AfterBuildBlocksSuccess(HashSet<MyBlockLocation> builtBlocks, bool instantBuild)
		{
			foreach (MyBlockLocation builtBlock in builtBlocks)
			{
				AfterBuildBlockSuccess(builtBlock, instantBuild);
				MySlimBlock cubeBlock = GetCubeBlock(builtBlock.CenterPos);
				DetectMerge(cubeBlock);
			}
		}

		private void AfterBuildBlockSuccess(MyBlockLocation builtBlock, bool instantBuild)
		{
			MySlimBlock cubeBlock = GetCubeBlock(builtBlock.CenterPos);
			if (cubeBlock != null && cubeBlock.FatBlock != null)
			{
				cubeBlock.FatBlock.OnBuildSuccess(builtBlock.Owner, instantBuild);
			}
		}

		public void RazeBlocksDelayed(ref Vector3I pos, ref Vector3UByte size, long builderEntityId)
		{
			bool flag = false;
			Vector3UByte vec = default(Vector3UByte);
			vec.X = 0;
			while (vec.X <= size.X)
			{
				vec.Y = 0;
				while (vec.Y <= size.Y)
				{
					vec.Z = 0;
					while (vec.Z <= size.Z)
					{
						Vector3I pos2 = pos + vec;
						MySlimBlock cubeBlock = GetCubeBlock(pos2);
						if (cubeBlock != null && cubeBlock.FatBlock != null && !cubeBlock.FatBlock.IsSubBlock)
						{
							MyCockpit myCockpit = cubeBlock.FatBlock as MyCockpit;
							if (myCockpit != null && myCockpit.Pilot != null)
							{
								if (!flag)
								{
									flag = true;
									m_isRazeBatchDelayed = true;
									m_delayedRazeBatch = new MyDelayedRazeBatch(pos, size);
									m_delayedRazeBatch.Occupied = new HashSet<MyCockpit>();
								}
								m_delayedRazeBatch.Occupied.Add(myCockpit);
							}
						}
						vec.Z++;
					}
					vec.Y++;
				}
				vec.X++;
			}
			if (!flag)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksAreaRequest, pos, size, builderEntityId, Sync.MyId);
			}
			else if (!MySession.Static.CreativeMode && MyMultiplayer.Static != null && MySession.Static.IsUserAdmin(Sync.MyId))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedMessageBox, messageText: MyTexts.Get(MyCommonTexts.RemovePilotToo)));
			}
			else
			{
				OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum.NO);
			}
		}

		public void OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (!m_isRazeBatchDelayed)
			{
				return;
			}
			if (base.Closed)
			{
				m_delayedRazeBatch.Occupied.Clear();
				m_delayedRazeBatch = null;
				m_isRazeBatchDelayed = false;
				return;
			}
			if (result == MyGuiScreenMessageBox.ResultEnum.NO)
			{
				foreach (MyCockpit item in m_delayedRazeBatch.Occupied)
				{
					if (item.Pilot != null && !item.MarkedForClose)
					{
						item.RequestRemovePilot();
					}
				}
			}
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksAreaRequest, m_delayedRazeBatch.Pos, m_delayedRazeBatch.Size, MySession.Static.LocalCharacterEntityId, Sync.MyId);
			m_delayedRazeBatch.Occupied.Clear();
			m_delayedRazeBatch = null;
			m_isRazeBatchDelayed = false;
		}

		public void RazeBlocks(ref Vector3I pos, ref Vector3UByte size, long builderEntityId = 0L)
		{
			ulong arg = 0uL;
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksAreaRequest, pos, size, builderEntityId, arg);
		}

		[Event(null, 4275)]
		[Reliable]
		[Server]
		private void RazeBlocksAreaRequest(Vector3I pos, Vector3UByte size, long builderEntityId, ulong placingPlayer)
		{
			if (!MySession.Static.CreativeMode && !MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				try
				{
					Vector3UByte vector3UByte = default(Vector3UByte);
					vector3UByte.X = 0;
					while (vector3UByte.X <= size.X)
					{
						vector3UByte.Y = 0;
						while (vector3UByte.Y <= size.Y)
						{
							vector3UByte.Z = 0;
							while (vector3UByte.Z <= size.Z)
							{
								Vector3I pos2 = pos + vector3UByte;
								MySlimBlock cubeBlock = GetCubeBlock(pos2);
								if (cubeBlock == null || (cubeBlock.FatBlock != null && cubeBlock.FatBlock.IsSubBlock))
								{
									m_tmpBuildFailList.Add(vector3UByte);
								}
								vector3UByte.Z++;
							}
							vector3UByte.Y++;
						}
						vector3UByte.X++;
					}
					if (MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Building, builderEntityId, placingPlayer))
					{
						MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksAreaSuccess, pos, size, m_tmpBuildFailList);
						RazeBlocksAreaSuccess(pos, size, m_tmpBuildFailList);
					}
				}
				finally
				{
					m_tmpBuildFailList.Clear();
				}
			}
		}

		[Event(null, 4308)]
		[Reliable]
		[Broadcast]
		private void RazeBlocksAreaSuccess(Vector3I pos, Vector3UByte size, HashSet<Vector3UByte> resultFailList)
		{
			Vector3I min = Vector3I.MaxValue;
			Vector3I max = Vector3I.MinValue;
			Vector3UByte vector3UByte = default(Vector3UByte);
			if (MyFakes.ENABLE_MULTIBLOCKS)
			{
				vector3UByte.X = 0;
				while (vector3UByte.X <= size.X)
				{
					vector3UByte.Y = 0;
					while (vector3UByte.Y <= size.Y)
					{
						vector3UByte.Z = 0;
						while (vector3UByte.Z <= size.Z)
						{
							if (!resultFailList.Contains(vector3UByte))
							{
								Vector3I pos2 = pos + vector3UByte;
								MySlimBlock cubeBlock = GetCubeBlock(pos2);
								if (cubeBlock != null)
								{
									MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
									if (myCompoundCubeBlock != null)
									{
										m_tmpSlimBlocks.Clear();
										m_tmpSlimBlocks.AddRange(myCompoundCubeBlock.GetBlocks());
										foreach (MySlimBlock tmpSlimBlock in m_tmpSlimBlocks)
										{
											if (tmpSlimBlock.IsMultiBlockPart)
											{
												m_tmpBlocksInMultiBlock.Clear();
												GetBlocksInMultiBlock(tmpSlimBlock.MultiBlockId, m_tmpBlocksInMultiBlock);
												RemoveMultiBlocks(ref min, ref max, m_tmpBlocksInMultiBlock);
												m_tmpBlocksInMultiBlock.Clear();
											}
											else
											{
												ushort? blockId = myCompoundCubeBlock.GetBlockId(tmpSlimBlock);
												if (blockId.HasValue)
												{
													RemoveBlockInCompound(tmpSlimBlock.Position, blockId.Value, ref min, ref max);
												}
											}
										}
										m_tmpSlimBlocks.Clear();
									}
									else if (cubeBlock.IsMultiBlockPart)
									{
										m_tmpBlocksInMultiBlock.Clear();
										GetBlocksInMultiBlock(cubeBlock.MultiBlockId, m_tmpBlocksInMultiBlock);
										RemoveMultiBlocks(ref min, ref max, m_tmpBlocksInMultiBlock);
										m_tmpBlocksInMultiBlock.Clear();
									}
									else
									{
										MyFracturedBlock myFracturedBlock = cubeBlock.FatBlock as MyFracturedBlock;
										if (myFracturedBlock != null && myFracturedBlock.MultiBlocks != null && myFracturedBlock.MultiBlocks.Count > 0)
										{
											foreach (MyFracturedBlock.MultiBlockPartInfo multiBlock in myFracturedBlock.MultiBlocks)
											{
												if (multiBlock != null)
												{
													m_tmpBlocksInMultiBlock.Clear();
													if (MyDefinitionManager.Static.TryGetMultiBlockDefinition(multiBlock.MultiBlockDefinition) != null)
													{
														GetBlocksInMultiBlock(multiBlock.MultiBlockId, m_tmpBlocksInMultiBlock);
														RemoveMultiBlocks(ref min, ref max, m_tmpBlocksInMultiBlock);
													}
													m_tmpBlocksInMultiBlock.Clear();
												}
											}
										}
										else
										{
											min = Vector3I.Min(min, cubeBlock.Min);
											max = Vector3I.Max(max, cubeBlock.Max);
											RemoveBlockByCubeBuilder(cubeBlock);
										}
									}
								}
							}
							vector3UByte.Z++;
						}
						vector3UByte.Y++;
					}
					vector3UByte.X++;
				}
			}
			else
			{
				vector3UByte.X = 0;
				while (vector3UByte.X <= size.X)
				{
					vector3UByte.Y = 0;
					while (vector3UByte.Y <= size.Y)
					{
						vector3UByte.Z = 0;
						while (vector3UByte.Z <= size.Z)
						{
							if (!resultFailList.Contains(vector3UByte))
							{
								Vector3I pos3 = pos + vector3UByte;
								MySlimBlock cubeBlock2 = GetCubeBlock(pos3);
								if (cubeBlock2 != null)
								{
									min = Vector3I.Min(min, cubeBlock2.Min);
									max = Vector3I.Max(max, cubeBlock2.Max);
									RemoveBlockByCubeBuilder(cubeBlock2);
								}
							}
							vector3UByte.Z++;
						}
						vector3UByte.Y++;
					}
					vector3UByte.X++;
				}
			}
			if (Physics != null)
			{
				Physics.AddDirtyArea(min, max);
			}
		}

		private void RemoveMultiBlocks(ref Vector3I min, ref Vector3I max, HashSet<Tuple<MySlimBlock, ushort?>> tmpBlocksInMultiBlock)
		{
			foreach (Tuple<MySlimBlock, ushort?> item in tmpBlocksInMultiBlock)
			{
				if (item.Item2.HasValue)
				{
					RemoveBlockInCompound(item.Item1.Position, item.Item2.Value, ref min, ref max);
				}
				else
				{
					min = Vector3I.Min(min, item.Item1.Min);
					max = Vector3I.Max(max, item.Item1.Max);
					RemoveBlockByCubeBuilder(item.Item1);
				}
			}
		}

		public void RazeBlock(Vector3I position, ulong user = 0uL)
		{
			m_tmpPositionListSend.Clear();
			m_tmpPositionListSend.Add(position);
			RazeBlocks(m_tmpPositionListSend, 0L, user);
		}

		public void RazeBlocks(List<Vector3I> locations, long builderEntityId = 0L, ulong user = 0uL)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksRequest, locations, builderEntityId, user);
		}

		[Event(null, 4476)]
		[Reliable]
		[Server]
		public void RazeBlocksRequest(List<Vector3I> locations, long builderEntityId = 0L, ulong user = 0uL)
		{
			m_tmpPositionListReceive.Clear();
			if (MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Building, builderEntityId, MyEventContext.Current.IsLocallyInvoked ? user : MyEventContext.Current.Sender.Value))
			{
				RazeBlocksSuccess(locations, m_tmpPositionListReceive);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RazeBlocksClient, m_tmpPositionListReceive);
			}
		}

		[Event(null, 4489)]
		[Reliable]
		[Broadcast]
		public void RazeBlocksClient(List<Vector3I> locations)
		{
			m_tmpPositionListReceive.Clear();
			RazeBlocksSuccess(locations, m_tmpPositionListReceive);
		}

		private void RazeBlocksSuccess(List<Vector3I> locations, List<Vector3I> removedBlocks)
		{
			Vector3I vector3I = Vector3I.MaxValue;
			Vector3I vector3I2 = Vector3I.MinValue;
			foreach (Vector3I location in locations)
			{
				MySlimBlock cubeBlock = GetCubeBlock(location);
				if (cubeBlock != null)
				{
					removedBlocks.Add(location);
					vector3I = Vector3I.Min(vector3I, cubeBlock.Min);
					vector3I2 = Vector3I.Max(vector3I2, cubeBlock.Max);
					RemoveBlockByCubeBuilder(cubeBlock);
				}
			}
			if (Physics != null)
			{
				Physics.AddDirtyArea(vector3I, vector3I2);
			}
		}

		public void RazeGeneratedBlocks(List<Vector3I> locations)
		{
			Vector3I vector3I = Vector3I.MaxValue;
			Vector3I vector3I2 = Vector3I.MinValue;
			foreach (Vector3I location in locations)
			{
				MySlimBlock cubeBlock = GetCubeBlock(location);
				if (cubeBlock != null)
				{
					vector3I = Vector3I.Min(vector3I, cubeBlock.Min);
					vector3I2 = Vector3I.Max(vector3I2, cubeBlock.Max);
					RemoveBlockByCubeBuilder(cubeBlock);
				}
			}
			if (Physics != null)
			{
				Physics.AddDirtyArea(vector3I, vector3I2);
			}
		}

		private void RazeBlockInCompoundBlockSuccess(List<LocationIdentity> locationsAndIds, List<Tuple<Vector3I, ushort>> removedBlocks)
		{
			Vector3I min = Vector3I.MaxValue;
			Vector3I max = Vector3I.MinValue;
			foreach (LocationIdentity locationsAndId in locationsAndIds)
			{
				RemoveBlockInCompound(locationsAndId.Location, locationsAndId.Id, ref min, ref max, removedBlocks);
			}
			m_dirtyRegion.AddCubeRegion(min, max);
			if (Physics != null)
			{
				Physics.AddDirtyArea(min, max);
			}
			MarkForDraw();
		}

		private void RemoveBlockInCompound(Vector3I position, ushort compoundBlockId, ref Vector3I min, ref Vector3I max, List<Tuple<Vector3I, ushort>> removedBlocks = null)
		{
			MySlimBlock cubeBlock = GetCubeBlock(position);
			if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
			{
				MyCompoundCubeBlock compoundBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
				RemoveBlockInCompoundInternal(position, compoundBlockId, ref min, ref max, removedBlocks, cubeBlock, compoundBlock);
			}
		}

		public void RazeGeneratedBlocksInCompoundBlock(List<Tuple<Vector3I, ushort>> locationsAndIds)
		{
			Vector3I min = Vector3I.MaxValue;
			Vector3I max = Vector3I.MinValue;
			foreach (Tuple<Vector3I, ushort> locationsAndId in locationsAndIds)
			{
				MySlimBlock cubeBlock = GetCubeBlock(locationsAndId.Item1);
				if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
				{
					MyCompoundCubeBlock compoundBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
					RemoveBlockInCompoundInternal(locationsAndId.Item1, locationsAndId.Item2, ref min, ref max, null, cubeBlock, compoundBlock);
				}
			}
			m_dirtyRegion.AddCubeRegion(min, max);
			if (Physics != null)
			{
				Physics.AddDirtyArea(min, max);
			}
			MarkForDraw();
		}

		private void RemoveBlockInCompoundInternal(Vector3I position, ushort compoundBlockId, ref Vector3I min, ref Vector3I max, List<Tuple<Vector3I, ushort>> removedBlocks, MySlimBlock block, MyCompoundCubeBlock compoundBlock)
		{
			MySlimBlock block2 = compoundBlock.GetBlock(compoundBlockId);
			if (block2 != null && compoundBlock.Remove(block2))
			{
				removedBlocks?.Add(new Tuple<Vector3I, ushort>(position, compoundBlockId));
				min = Vector3I.Min(min, block.Min);
				max = Vector3I.Max(max, block.Max);
				if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
				{
					MyCubeGridSmallToLargeConnection.Static.RemoveBlockSmallToLargeConnection(block2);
				}
				NotifyBlockRemoved(block2);
			}
			if (compoundBlock.GetBlocksCount() == 0)
			{
				RemoveBlockByCubeBuilder(block);
			}
		}

		public void RazeGeneratedBlocks(List<MySlimBlock> generatedBlocks)
		{
			m_tmpRazeList.Clear();
			m_tmpLocations.Clear();
			foreach (MySlimBlock generatedBlock in generatedBlocks)
			{
				MySlimBlock cubeBlock = GetCubeBlock(generatedBlock.Position);
				if (cubeBlock != null)
				{
					if (cubeBlock.FatBlock is MyCompoundCubeBlock)
					{
						ushort? blockId = (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlockId(generatedBlock);
						if (blockId.HasValue)
						{
							m_tmpRazeList.Add(new Tuple<Vector3I, ushort>(generatedBlock.Position, blockId.Value));
						}
					}
					else
					{
						m_tmpLocations.Add(generatedBlock.Position);
					}
				}
			}
			if (m_tmpLocations.Count > 0)
			{
				RazeGeneratedBlocks(m_tmpLocations);
			}
			if (m_tmpRazeList.Count > 0)
			{
				RazeGeneratedBlocksInCompoundBlock(m_tmpRazeList);
			}
			m_tmpRazeList.Clear();
			m_tmpLocations.Clear();
		}

		public void ColorBlocks(Vector3I min, Vector3I max, Vector3 newHSV, bool playSound, bool validateOwnership)
		{
			long arg = validateOwnership ? MySession.Static.LocalPlayerId : 0;
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.ColorBlockRequest, min, max, newHSV, playSound, arg);
		}

		public void ColorGrid(Vector3 newHSV, bool playSound, bool validateOwnership)
		{
			long arg = validateOwnership ? MySession.Static.LocalPlayerId : 0;
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.ColorGridFriendlyRequest, newHSV, playSound, arg);
		}

		[Event(null, 4692)]
		[Reliable]
		[Server]
		private void ColorGridFriendlyRequest(Vector3 newHSV, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnColorGridFriendly, newHSV, playSound, player);
			}
		}

		[Event(null, 4701)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnColorGridFriendly(Vector3 newHSV, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				bool flag = false;
				foreach (MySlimBlock cubeBlock in CubeBlocks)
				{
					flag |= ChangeColorAndSkin(cubeBlock, newHSV);
				}
				if (playSound && flag)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
				}
			}
		}

		[Event(null, 4722)]
		[Reliable]
		[Server]
		private void ColorBlockRequest(Vector3I min, Vector3I max, Vector3 newHSV, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnColorBlock, min, max, newHSV, playSound, player);
			}
		}

		[Event(null, 4731)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnColorBlock(Vector3I min, Vector3I max, Vector3 newHSV, bool playSound, long player)
		{
			if (!ColorGridOrBlockRequestValidation(player))
			{
				return;
			}
			bool flag = false;
			Vector3I pos = default(Vector3I);
			pos.X = min.X;
			while (pos.X <= max.X)
			{
				pos.Y = min.Y;
				while (pos.Y <= max.Y)
				{
					pos.Z = min.Z;
					while (pos.Z <= max.Z)
					{
						MySlimBlock cubeBlock = GetCubeBlock(pos);
						if (cubeBlock != null)
						{
							flag |= ChangeColorAndSkin(cubeBlock, newHSV);
						}
						pos.Z++;
					}
					pos.Y++;
				}
				pos.X++;
			}
			if (playSound && flag && Vector3D.Distance(MySector.MainCamera.Position, Vector3D.Transform(min * GridSize, base.WorldMatrix)) < 200.0)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
			}
		}

		public static MyGameInventoryItem GetArmorSkinItem(MyStringHash skinId)
		{
			if (skinId == MyStringHash.NullOrEmpty || MyGameService.InventoryItems == null)
			{
				return null;
			}
			foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
			{
				if (inventoryItem.ItemDefinition != null && inventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor && !(MyStringHash.GetOrCompute(inventoryItem.ItemDefinition.AssetModifierId) != skinId))
				{
					return inventoryItem;
				}
			}
			return null;
		}

		public void SkinBlocks(Vector3I min, Vector3I max, Vector3? newHSV, MyStringHash? newSkin, bool playSound, bool validateOwnership)
		{
			long arg = validateOwnership ? MySession.Static.LocalPlayerId : 0;
			MyBlockVisuals arg2 = new MyBlockVisuals(newHSV.HasValue ? newHSV.Value.PackHSVToUint() : 0u, newSkin.HasValue ? newSkin.Value : MyStringHash.NullOrEmpty, newHSV.HasValue, newSkin.HasValue);
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.SkinBlockRequest, min, max, arg2, playSound, arg);
		}

		public void SkinGrid(Vector3 newHSV, MyStringHash newSkin, bool playSound, bool validateOwnership, bool applyColor, bool applySkin)
		{
			if (applyColor || applySkin)
			{
				long arg = validateOwnership ? MySession.Static.LocalPlayerId : 0;
				MyMultiplayer.RaiseEvent<MyCubeGrid, MyBlockVisuals, bool, long>(arg2: new MyBlockVisuals(newHSV.PackHSVToUint(), newSkin, applyColor, applySkin), arg1: this, action: (MyCubeGrid x) => x.SkinGridFriendlyRequest, arg3: playSound, arg4: arg);
			}
		}

		[Event(null, 4820)]
		[Reliable]
		[Server]
		private void SkinGridFriendlyRequest(MyBlockVisuals visuals, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				visuals.SkinId = (MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(visuals.SkinId, MyEventContext.Current.Sender.Value) ?? MyStringHash.NullOrEmpty);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnSkinGridFriendly, visuals, playSound, player);
			}
		}

		[Event(null, 4830)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnSkinGridFriendly(MyBlockVisuals visuals, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				Vector3 value = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
				bool flag = false;
				foreach (MySlimBlock cubeBlock in CubeBlocks)
				{
					flag |= ChangeColorAndSkin(cubeBlock, visuals.ApplyColor ? new Vector3?(value) : null, visuals.ApplySkin ? new MyStringHash?(visuals.SkinId) : null);
				}
				if (playSound && flag)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
				}
			}
		}

		[Event(null, 4853)]
		[Reliable]
		[Server]
		private void SkinBlockRequest(Vector3I min, Vector3I max, MyBlockVisuals visuals, bool playSound, long player)
		{
			if (ColorGridOrBlockRequestValidation(player))
			{
				visuals.SkinId = (MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(visuals.SkinId, MyEventContext.Current.Sender.Value) ?? MyStringHash.NullOrEmpty);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnSkinBlock, min, max, visuals, playSound, player);
			}
		}

		[Event(null, 4863)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnSkinBlock(Vector3I min, Vector3I max, MyBlockVisuals visuals, bool playSound, long player)
		{
			if (!ColorGridOrBlockRequestValidation(player))
			{
				return;
			}
			Vector3 value = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
			bool flag = false;
			Vector3I pos = default(Vector3I);
			pos.X = min.X;
			while (pos.X <= max.X)
			{
				pos.Y = min.Y;
				while (pos.Y <= max.Y)
				{
					pos.Z = min.Z;
					while (pos.Z <= max.Z)
					{
						MySlimBlock cubeBlock = GetCubeBlock(pos);
						if (cubeBlock != null)
						{
							flag |= ChangeColorAndSkin(cubeBlock, visuals.ApplyColor ? new Vector3?(value) : null, visuals.ApplySkin ? new MyStringHash?(visuals.SkinId) : null);
						}
						pos.Z++;
					}
					pos.Y++;
				}
				pos.X++;
			}
			if (playSound && flag && Vector3D.Distance(MySector.MainCamera.Position, Vector3D.Transform(min * GridSize, base.WorldMatrix)) < 200.0)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudColorBlock);
			}
		}

		public bool ColorGridOrBlockRequestValidation(long player)
		{
			if (player == 0L)
			{
				return true;
			}
			if (!Sync.IsServer)
			{
				return true;
			}
			if (BigOwners.Count == 0)
			{
				return true;
			}
			foreach (long bigOwner in BigOwners)
			{
				if (MyIDModule.GetRelationPlayerPlayer(bigOwner, player) == MyRelationsBetweenPlayers.Self)
				{
					return true;
				}
			}
			return false;
		}

		private MySlimBlock BuildBlock(MyCubeBlockDefinition blockDefinition, Vector3 colorMaskHsv, MyStringHash skinId, Vector3I min, Quaternion orientation, long owner, long entityId, MyEntity builderEntity, MyObjectBuilder_CubeBlock blockObjectBuilder = null, bool updateVolume = true, bool testMerge = true, bool buildAsAdmin = false)
		{
			MyBlockOrientation orientation2 = new MyBlockOrientation(ref orientation);
			if (blockObjectBuilder == null)
			{
				blockObjectBuilder = CreateBlockObjectBuilder(blockDefinition, min, orientation2, entityId, owner, builderEntity == null || !MySession.Static.SurvivalMode || buildAsAdmin);
				blockObjectBuilder.ColorMaskHSV = colorMaskHsv;
				blockObjectBuilder.SkinSubtypeId = skinId.String;
			}
			else
			{
				blockObjectBuilder.Min = min;
				blockObjectBuilder.Orientation = orientation;
			}
			MyCubeBuilder.BuildComponent.BeforeCreateBlock(blockDefinition, builderEntity, blockObjectBuilder, buildAsAdmin);
			MySlimBlock mySlimBlock = null;
			Vector3I vector3I = MySlimBlock.ComputePositionInGrid(new MatrixI(orientation2), blockDefinition, min);
			if (!MyEntities.IsInsideWorld(GridIntegerToWorld(vector3I)))
			{
				return null;
			}
			if (Sync.IsServer)
			{
				MyCubeBuilder.BuildComponent.GetBlockPlacementMaterials(blockDefinition, vector3I, blockObjectBuilder.BlockOrientation, this);
			}
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition))
			{
				MySlimBlock cubeBlock = GetCubeBlock(min);
				MyCompoundCubeBlock myCompoundCubeBlock = (cubeBlock != null) ? (cubeBlock.FatBlock as MyCompoundCubeBlock) : null;
				if (myCompoundCubeBlock != null)
				{
					if (myCompoundCubeBlock.CanAddBlock(blockDefinition, new MyBlockOrientation(ref orientation)))
					{
						object obj = MyCubeBlockFactory.CreateCubeBlock(blockObjectBuilder);
						mySlimBlock = (obj as MySlimBlock);
						if (mySlimBlock == null)
						{
							mySlimBlock = new MySlimBlock();
						}
						mySlimBlock.Init(blockObjectBuilder, this, obj as MyCubeBlock);
						mySlimBlock.FatBlock.HookMultiplayer();
						if (myCompoundCubeBlock.Add(mySlimBlock, out ushort _))
						{
							BoundsInclude(mySlimBlock);
							m_dirtyRegion.AddCube(min);
							if (Physics != null)
							{
								Physics.AddDirtyBlock(cubeBlock);
							}
							NotifyBlockAdded(mySlimBlock);
						}
					}
				}
				else
				{
					MyObjectBuilder_CompoundCubeBlock objectBuilder = MyCompoundCubeBlock.CreateBuilder(blockObjectBuilder);
					mySlimBlock = AddBlock(objectBuilder, testMerge);
				}
				MarkForDraw();
			}
			else
			{
				mySlimBlock = AddBlock(blockObjectBuilder, testMerge);
			}
			if (mySlimBlock != null)
			{
				mySlimBlock.CubeGrid.BoundsInclude(mySlimBlock);
				if (updateVolume)
				{
					mySlimBlock.CubeGrid.UpdateGridAABB();
				}
				if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
				{
					MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(mySlimBlock);
				}
				if (Sync.IsServer)
				{
					MyCubeBuilder.BuildComponent.AfterSuccessfulBuild(builderEntity, buildAsAdmin);
				}
				MyCubeGrids.NotifyBlockBuilt(this, mySlimBlock);
			}
			return mySlimBlock;
		}

		internal void PerformCutouts(List<MyGridPhysics.ExplosionInfo> explosions)
		{
			if (explosions.Count == 0)
			{
				return;
			}
			BoundingSphereD sphere = new BoundingSphereD(explosions[0].Position, explosions[0].Radius);
			for (int j = 0; j < explosions.Count; j++)
			{
				sphere.Include(new BoundingSphereD(explosions[j].Position, explosions[j].Radius));
			}
			using (MyUtils.ReuseCollection(ref m_rootVoxelsToCutTmp))
			{
				using (MyUtils.ReuseCollection(ref m_overlappingVoxelsTmp))
				{
					MySession.Static.VoxelMaps.GetAllOverlappingWithSphere(ref sphere, m_overlappingVoxelsTmp);
					foreach (MyVoxelBase item in m_overlappingVoxelsTmp)
					{
						m_rootVoxelsToCutTmp.Add(item.RootVoxel);
					}
					int skipCount = 0;
					Parallel.For(0, explosions.Count, delegate(int i)
					{
						MyGridPhysics.ExplosionInfo explosionInfo2 = explosions[i];
						BoundingSphereD sphere2 = new BoundingSphereD(explosionInfo2.Position, explosionInfo2.Radius);
						for (int k = 0; k < explosions.Count; k++)
						{
							if (k != i && new BoundingSphereD(explosions[k].Position, explosions[k].Radius).Contains(sphere2) == ContainmentType.Contains)
							{
								skipCount++;
								return;
							}
						}
						foreach (MyVoxelBase item2 in m_rootVoxelsToCutTmp)
						{
							if (MyVoxelGenerator.CutOutSphereFast(item2, ref explosionInfo2.Position, explosionInfo2.Radius, out Vector3I cacheMin, out Vector3I cacheMax, notifyChanged: false))
							{
								MyMultiplayer.RaiseEvent(item2, (MyVoxelBase x) => x.PerformCutOutSphereFast, explosionInfo2.Position, explosionInfo2.Radius, arg4: true);
								m_notificationQueue.Enqueue(MyTuple.Create(i, item2, cacheMin, cacheMax));
							}
						}
					}, 1, WorkPriority.VeryHigh, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Voxels, "CutOutVoxel"), blocking: true);
				}
			}
			bool flag = false;
			BoundingBoxD boundaries = BoundingBoxD.CreateInvalid();
			foreach (MyTuple<int, MyVoxelBase, Vector3I, Vector3I> item3 in m_notificationQueue)
			{
				flag = true;
				MyGridPhysics.ExplosionInfo explosionInfo = explosions[item3.Item1];
				boundaries.Include(new BoundingSphereD(explosionInfo.Position, explosionInfo.Radius));
				Vector3I voxelRangeMin = item3.Item3;
				Vector3I voxelRangeMax = item3.Item4;
				item3.Item2.RootVoxel.Storage.NotifyRangeChanged(ref voxelRangeMin, ref voxelRangeMax, MyStorageDataTypeFlags.Content);
			}
			if (flag)
			{
				MyShapeBox myShapeBox = new MyShapeBox();
				myShapeBox.Boundaries = boundaries;
				MyTuple<int, MyVoxelBase, Vector3I, Vector3I> result;
				while (m_notificationQueue.TryDequeue(out result))
				{
					BoundingBoxD cutOutBox = myShapeBox.GetWorldBoundaries();
					MyVoxelGenerator.NotifyVoxelChanged(MyVoxelBase.OperationType.Cut, result.Item2, ref cutOutBox);
				}
			}
		}

		public void ResetBlockSkeleton(MySlimBlock block, bool updateSync = false)
		{
			MultiplyBlockSkeleton(block, 0f, updateSync);
		}

		public void MultiplyBlockSkeleton(MySlimBlock block, float factor, bool updateSync = false)
		{
			if (Skeleton == null)
			{
				MyLog.Default.WriteLine("Skeleton null in MultiplyBlockSkeleton!" + this);
			}
			if (Physics == null)
			{
				MyLog.Default.WriteLine("Physics null in MultiplyBlockSkeleton!" + this);
			}
			if (block == null || Skeleton == null || Physics == null)
			{
				return;
			}
			Vector3I vector3I = block.Min * 2;
			Vector3I vector3I2 = block.Max * 2 + 2;
			bool flag = false;
			Vector3I pos = default(Vector3I);
			pos.Z = vector3I.Z;
			while (pos.Z <= vector3I2.Z)
			{
				pos.Y = vector3I.Y;
				while (pos.Y <= vector3I2.Y)
				{
					pos.X = vector3I.X;
					while (pos.X <= vector3I2.X)
					{
						flag |= Skeleton.MultiplyBone(ref pos, factor, ref block.Min, this);
						pos.X++;
					}
					pos.Y++;
				}
				pos.Z++;
			}
			if (!flag)
			{
				return;
			}
			if (Sync.IsServer && updateSync)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnBonesMultiplied, block.Position, factor);
			}
			vector3I = block.Min - Vector3I.One;
			vector3I2 = block.Max + Vector3I.One;
			pos.Z = vector3I.Z;
			while (pos.Z <= vector3I2.Z)
			{
				pos.Y = vector3I.Y;
				while (pos.Y <= vector3I2.Y)
				{
					pos.X = vector3I.X;
					while (pos.X <= vector3I2.X)
					{
						m_dirtyRegion.AddCube(pos);
						pos.X++;
					}
					pos.Y++;
				}
				pos.Z++;
			}
			Physics.AddDirtyArea(vector3I, vector3I2);
			MarkForDraw();
		}

		public void AddDirtyBone(Vector3I gridPosition, Vector3I boneOffset)
		{
			Skeleton.Wrap(ref gridPosition, ref boneOffset);
			Vector3I value = boneOffset - new Vector3I(1, 1, 1);
			Vector3I start = Vector3I.Min(value, new Vector3I(0, 0, 0));
			Vector3I end = Vector3I.Max(value, new Vector3I(0, 0, 0));
			Vector3I next = start;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
			while (vector3I_RangeIterator.IsValid())
			{
				m_dirtyRegion.AddCube(gridPosition + next);
				vector3I_RangeIterator.GetNext(out next);
			}
			MarkForDraw();
		}

		public MySlimBlock GetCubeBlock(Vector3I pos)
		{
			if (m_cubes.TryGetValue(pos, out MyCube value))
			{
				return value.CubeBlock;
			}
			return null;
		}

		public MySlimBlock GetCubeBlock(Vector3I pos, ushort? compoundId)
		{
			if (!compoundId.HasValue)
			{
				return GetCubeBlock(pos);
			}
			if (m_cubes.TryGetValue(pos, out MyCube value))
			{
				MyCompoundCubeBlock myCompoundCubeBlock = value.CubeBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
					return myCompoundCubeBlock.GetBlock(compoundId.Value);
				}
			}
			return null;
		}

		public T GetFirstBlockOfType<T>() where T : MyCubeBlock
		{
			foreach (MySlimBlock cubeBlock in m_cubeBlocks)
			{
				if (cubeBlock.FatBlock != null && cubeBlock.FatBlock is T)
				{
					return cubeBlock.FatBlock as T;
				}
			}
			return null;
		}

		public void FixTargetCubeLite(out Vector3I cube, Vector3D fractionalGridPosition)
		{
			cube = Vector3I.Round(fractionalGridPosition - 0.5);
		}

		public void FixTargetCube(out Vector3I cube, Vector3 fractionalGridPosition)
		{
			cube = Vector3I.Round(fractionalGridPosition);
			fractionalGridPosition += new Vector3(0.5f);
			if (m_cubes.ContainsKey(cube))
			{
				return;
			}
			Vector3 vector = fractionalGridPosition - cube;
			Vector3 vector2 = new Vector3(1f) - vector;
			m_neighborDistances[1] = vector.X;
			m_neighborDistances[0] = vector2.X;
			m_neighborDistances[3] = vector.Y;
			m_neighborDistances[2] = vector2.Y;
			m_neighborDistances[5] = vector.Z;
			m_neighborDistances[4] = vector2.Z;
			Vector3 value = vector * vector;
			Vector3 value2 = vector2 * vector2;
			m_neighborDistances[9] = (float)Math.Sqrt(value.X + value.Y);
			m_neighborDistances[8] = (float)Math.Sqrt(value.X + value2.Y);
			m_neighborDistances[7] = (float)Math.Sqrt(value2.X + value.Y);
			m_neighborDistances[6] = (float)Math.Sqrt(value2.X + value2.Y);
			m_neighborDistances[17] = (float)Math.Sqrt(value.X + value.Z);
			m_neighborDistances[16] = (float)Math.Sqrt(value.X + value2.Z);
			m_neighborDistances[15] = (float)Math.Sqrt(value2.X + value.Z);
			m_neighborDistances[14] = (float)Math.Sqrt(value2.X + value2.Z);
			m_neighborDistances[13] = (float)Math.Sqrt(value.Y + value.Z);
			m_neighborDistances[12] = (float)Math.Sqrt(value.Y + value2.Z);
			m_neighborDistances[11] = (float)Math.Sqrt(value2.Y + value.Z);
			m_neighborDistances[10] = (float)Math.Sqrt(value2.Y + value2.Z);
			Vector3 vector3 = value * vector;
			Vector3 vector4 = value2 * vector2;
			m_neighborDistances[25] = (float)Math.Pow(vector3.X + vector3.Y + vector3.Z, 0.33333333333333331);
			m_neighborDistances[24] = (float)Math.Pow(vector3.X + vector3.Y + vector4.Z, 0.33333333333333331);
			m_neighborDistances[23] = (float)Math.Pow(vector3.X + vector4.Y + vector3.Z, 0.33333333333333331);
			m_neighborDistances[22] = (float)Math.Pow(vector3.X + vector4.Y + vector4.Z, 0.33333333333333331);
			m_neighborDistances[21] = (float)Math.Pow(vector4.X + vector3.Y + vector3.Z, 0.33333333333333331);
			m_neighborDistances[20] = (float)Math.Pow(vector4.X + vector3.Y + vector4.Z, 0.33333333333333331);
			m_neighborDistances[19] = (float)Math.Pow(vector4.X + vector4.Y + vector3.Z, 0.33333333333333331);
			m_neighborDistances[18] = (float)Math.Pow(vector4.X + vector4.Y + vector4.Z, 0.33333333333333331);
			for (int i = 0; i < 25; i++)
			{
				for (int j = 0; j < 25 - i; j++)
				{
					float num = m_neighborDistances[(int)m_neighborOffsetIndices[j]];
					float num2 = m_neighborDistances[(int)m_neighborOffsetIndices[j + 1]];
					if (num > num2)
					{
						NeighborOffsetIndex value3 = m_neighborOffsetIndices[j];
						m_neighborOffsetIndices[j] = m_neighborOffsetIndices[j + 1];
						m_neighborOffsetIndices[j + 1] = value3;
					}
				}
			}
			Vector3I vector3I = default(Vector3I);
			int num3 = 0;
			while (true)
			{
				if (num3 < m_neighborOffsets.Count)
				{
					vector3I = m_neighborOffsets[(int)m_neighborOffsetIndices[num3]];
					if (m_cubes.ContainsKey(cube + vector3I))
					{
						break;
					}
					num3++;
					continue;
				}
				return;
			}
			cube += vector3I;
		}

		public HashSet<MySlimBlock> GetBlocks()
		{
			return m_cubeBlocks;
		}

		public ListReader<MyCubeBlock> GetFatBlocks()
		{
			return m_fatBlocks.ListUnsafe;
		}

		public MyFatBlockReader<T> GetFatBlocks<T>() where T : MyCubeBlock
		{
			return new MyFatBlockReader<T>(this);
		}

		public bool HasStandAloneBlocks()
		{
			if (m_hasStandAloneBlocks)
			{
				if (MyPerGameSettings.Game == GameEnum.SE_GAME)
				{
					foreach (MySlimBlock cubeBlock in m_cubeBlocks)
					{
						if (cubeBlock.BlockDefinition.IsStandAlone)
						{
							return true;
						}
					}
					m_hasStandAloneBlocks = false;
				}
				else
				{
					m_hasStandAloneBlocks = (m_cubeBlocks.Count > 0);
				}
			}
			return m_hasStandAloneBlocks;
		}

		public static bool HasStandAloneBlocks(List<MySlimBlock> blocks, int offset, int count)
		{
			if (offset < 0)
			{
				MySandboxGame.Log.WriteLine($"Negative offset in HasStandAloneBlocks - {offset}");
				return false;
			}
			for (int i = offset; i < offset + count && i < blocks.Count; i++)
			{
				MySlimBlock mySlimBlock = blocks[i];
				if (mySlimBlock != null && mySlimBlock.BlockDefinition.IsStandAlone)
				{
					return true;
				}
			}
			return false;
		}

		public bool CanHavePhysics()
		{
			if (m_canHavePhysics)
			{
				if (MyPerGameSettings.Game == GameEnum.SE_GAME)
				{
					foreach (MySlimBlock cubeBlock in m_cubeBlocks)
					{
						if (cubeBlock.BlockDefinition.HasPhysics)
						{
							return true;
						}
					}
					m_canHavePhysics = false;
				}
				else
				{
					m_canHavePhysics = (m_cubeBlocks.Count > 0);
				}
			}
			return m_canHavePhysics;
		}

		public static bool CanHavePhysics(List<MySlimBlock> blocks, int offset, int count)
		{
			if (offset < 0)
			{
				MySandboxGame.Log.WriteLine($"Negative offset in CanHavePhysics - {offset}");
				return false;
			}
			for (int i = offset; i < offset + count && i < blocks.Count; i++)
			{
				MySlimBlock mySlimBlock = blocks[i];
				if (mySlimBlock != null && mySlimBlock.BlockDefinition.HasPhysics)
				{
					return true;
				}
			}
			return false;
		}

		private void RebuildGrid(bool staticPhysics = false)
		{
			if (!HasStandAloneBlocks() || !CanHavePhysics())
			{
				return;
			}
			RecalcBounds();
			RemoveRedundantParts();
			if (Physics != null)
			{
				Physics.Close();
				Physics = null;
			}
			if (CreatePhysics)
			{
				Physics = new MyGridPhysics(this, null, staticPhysics);
				RaisePhysicsChanged();
				if (!Sync.IsServer && !IsClientPredicted)
				{
					Physics.RigidBody.UpdateMotionType(HkMotionType.Fixed);
				}
			}
		}

		[Event(null, 5500)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		public void OnConvertToDynamic()
		{
			if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
			{
				MyCubeGridSmallToLargeConnection.Static.ConvertToDynamic(this);
			}
			IsStatic = false;
			IsUnsupportedStation = false;
			if (MyCubeGridGroups.Static != null)
			{
				MyCubeGridGroups.Static.UpdateDynamicState(this);
			}
			SetInventoryMassDirty();
			Physics.ConvertToDynamic(GridSizeEnum == MyCubeSize.Large, IsClientPredicted);
			RaisePhysicsChanged();
			Physics.RigidBody.AddGravity();
			RecalculateGravity();
			MyFixedGrids.UnmarkGridRoot(this);
		}

		[Event(null, 5532)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		public void ConvertToStatic()
		{
			if (!IsStatic && Physics != null && !((double)Physics.AngularVelocity.LengthSquared() > 0.0001) && !((double)Physics.LinearVelocity.LengthSquared() > 0.0001))
			{
				IsStatic = true;
				IsUnsupportedStation = true;
				Physics.ConvertToStatic();
				RaisePhysicsChanged();
				MyFixedGrids.MarkGridRoot(this);
			}
		}

		public void DoDamage(float damage, MyHitInfo hitInfo, Vector3? localPos = null, long attackerId = 0L)
		{
			if (!Sync.IsServer || !MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Damage, 0L, 0uL))
			{
				return;
			}
			Vector3I cube;
			if (localPos.HasValue)
			{
				FixTargetCube(out cube, localPos.Value * GridSizeR);
			}
			else
			{
				FixTargetCube(out cube, Vector3D.Transform(hitInfo.Position, base.PositionComp.WorldMatrixInvScaled) * GridSizeR);
			}
			MySlimBlock mySlimBlock = GetCubeBlock(cube);
			if (mySlimBlock == null)
			{
				return;
			}
			if (MyFakes.ENABLE_FRACTURE_COMPONENT)
			{
				ushort? num = null;
				MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
					num = Physics.GetContactCompoundId(mySlimBlock.Position, hitInfo.Position);
					if (!num.HasValue)
					{
						return;
					}
					MySlimBlock block = myCompoundCubeBlock.GetBlock(num.Value);
					if (block == null)
					{
						return;
					}
					mySlimBlock = block;
				}
			}
			ApplyDestructionDeformation(mySlimBlock, damage, hitInfo, attackerId);
		}

		public void ApplyDestructionDeformation(MySlimBlock block, float damage = 1f, MyHitInfo? hitInfo = null, long attackerId = 0L)
		{
			if (MyPerGameSettings.Destruction)
			{
				((IMyDestroyableObject)block).DoDamage(damage, MyDamageType.Deformation, sync: true, hitInfo, attackerId);
				return;
			}
			EnqueueDestructionDeformationBlock(block.Position);
			ApplyDestructionDeformationInternal(block, sync: true, damage, attackerId);
		}

		private void ApplyDeformationPostponed()
		{
			if (m_deformationPostponed.Count > 0)
			{
				List<DeformationPostponedItem> cloned = m_deformationPostponed;
				Parallel.Start(delegate
				{
					foreach (DeformationPostponedItem item in cloned)
					{
						ApplyDestructionDeformationInternal(item);
					}
					cloned.Clear();
					m_postponedListsPool.Return(cloned);
				});
				m_deformationPostponed = m_postponedListsPool.Get();
				m_deformationPostponed.Clear();
			}
		}

		private void ApplyDestructionDeformationInternal(DeformationPostponedItem item)
		{
			if (base.Closed)
			{
				return;
			}
			if (m_deformationRng == null)
			{
				m_deformationRng = new MyRandom();
			}
			Vector3I minCube = Vector3I.MaxValue;
			Vector3I maxCube = Vector3I.MinValue;
			bool flag = false;
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					flag |= MoveCornerBones(item.Min, new Vector3I(i, 0, j), ref minCube, ref maxCube);
					flag |= MoveCornerBones(item.Min, new Vector3I(i, j, 0), ref minCube, ref maxCube);
					flag |= MoveCornerBones(item.Min, new Vector3I(0, i, j), ref minCube, ref maxCube);
				}
			}
			if (flag)
			{
				m_dirtyRegion.AddCubeRegion(minCube, maxCube);
			}
			m_deformationRng.SetSeed(item.Position.GetHashCode());
			float angleDeviation = MathF.PI / 8f;
			float gridSizeQuarter = GridSizeQuarter;
			Vector3I min = item.Min;
			for (int k = 0; k < 3; k++)
			{
				Vector3I dirtyMin = Vector3I.MaxValue;
				Vector3I dirtyMax = Vector3I.MinValue;
				flag = false;
				flag |= ApplyTable(min, MyCubeGridDeformationTables.ThinUpper[k], ref dirtyMin, ref dirtyMax, m_deformationRng, gridSizeQuarter, angleDeviation);
				if (flag | ApplyTable(min, MyCubeGridDeformationTables.ThinLower[k], ref dirtyMin, ref dirtyMax, m_deformationRng, gridSizeQuarter, angleDeviation))
				{
					dirtyMin -= Vector3I.One;
					dirtyMax += Vector3I.One;
					minCube = min;
					maxCube = min;
					Skeleton.Wrap(ref minCube, ref dirtyMin);
					Skeleton.Wrap(ref maxCube, ref dirtyMax);
					m_dirtyRegion.AddCubeRegion(minCube, maxCube);
				}
			}
			MySandboxGame.Static.Invoke(delegate
			{
				MarkForDraw();
			}, "ApplyDestructionDeformationInternal::MarkForDraw");
		}

		private float ApplyDestructionDeformationInternal(MySlimBlock block, bool sync, float damage = 1f, long attackerId = 0L, bool postponed = false)
		{
			if (!BlocksDestructionEnabled)
			{
				return 0f;
			}
			if (block.UseDamageSystem)
			{
				MyDamageInformation info = new MyDamageInformation(isDeformation: true, 1f, MyDamageType.Deformation, attackerId);
				MyDamageSystem.Static.RaiseBeforeDamageApplied(block, ref info);
				if (info.Amount == 0f)
				{
					return 0f;
				}
			}
			DeformationPostponedItem deformationPostponedItem = default(DeformationPostponedItem);
			deformationPostponedItem.Position = block.Position;
			deformationPostponedItem.Min = block.Min;
			deformationPostponedItem.Max = block.Max;
			DeformationPostponedItem item = deformationPostponedItem;
			m_totalBoneDisplacement = 0f;
			if (postponed)
			{
				m_deformationPostponed.Add(item);
				MarkForUpdate();
			}
			else
			{
				ApplyDestructionDeformationInternal(item);
			}
			if (sync)
			{
				float amount = m_totalBoneDisplacement * GridSize * 10f * damage;
				MyDamageInformation info2 = new MyDamageInformation(isDeformation: false, amount, MyDamageType.Deformation, attackerId);
				if (block.UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseBeforeDamageApplied(block, ref info2);
				}
				if (info2.Amount > 0f)
				{
					((IMyDestroyableObject)block).DoDamage(info2.Amount, MyDamageType.Deformation, sync: true, (MyHitInfo?)null, attackerId);
				}
			}
			return m_totalBoneDisplacement;
		}

		public void RemoveDestroyedBlock(MySlimBlock block, long attackerId = 0L)
		{
			if (!Sync.IsServer)
			{
				if (!MyFakes.ENABLE_FRACTURE_COMPONENT)
				{
					block.OnDestroyVisual();
				}
			}
			else
			{
				if (Physics == null)
				{
					return;
				}
				if (MyFakes.ENABLE_FRACTURE_COMPONENT)
				{
					bool flag = attackerId != 0;
					bool enable = EnableGenerators(flag);
					MySlimBlock cubeBlock = GetCubeBlock(block.Position);
					if (cubeBlock == null)
					{
						return;
					}
					if (cubeBlock == block)
					{
						EnqueueDestroyedBlockWithId(block.Position, null, flag);
						RemoveDestroyedBlockInternal(block);
						Physics.AddDirtyBlock(block);
					}
					else
					{
						MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
						if (myCompoundCubeBlock != null)
						{
							ushort? blockId = myCompoundCubeBlock.GetBlockId(block);
							if (blockId.HasValue)
							{
								EnqueueDestroyedBlockWithId(block.Position, blockId, flag);
								RemoveDestroyedBlockInternal(block);
								Physics.AddDirtyBlock(block);
							}
						}
					}
					EnableGenerators(enable);
					MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
					if (fractureComponent != null)
					{
						MyDestructionHelper.CreateFracturePiece(fractureComponent, sync: true);
					}
				}
				else
				{
					EnqueueDestroyedBlock(block.Position);
					RemoveDestroyedBlockInternal(block);
					Physics.AddDirtyBlock(block);
				}
			}
		}

		private void RemoveDestroyedBlockInternal(MySlimBlock block)
		{
			ApplyDestructionDeformationInternal(block, sync: false, 1f, 0L, postponed: true);
			((IMyDestroyableObject)block).OnDestroy();
			MySlimBlock cubeBlock = GetCubeBlock(block.Position);
			if (cubeBlock == block)
			{
				RemoveBlockInternal(block, close: true);
			}
			else
			{
				if (cubeBlock == null)
				{
					return;
				}
				MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
					ushort? blockId = myCompoundCubeBlock.GetBlockId(block);
					if (blockId.HasValue)
					{
						Vector3I min = Vector3I.MaxValue;
						Vector3I max = Vector3I.MinValue;
						RemoveBlockInCompound(block.Position, blockId.Value, ref min, ref max);
					}
				}
			}
		}

		private bool ApplyTable(Vector3I cubePos, MyCubeGridDeformationTables.DeformationTable table, ref Vector3I dirtyMin, ref Vector3I dirtyMax, MyRandom random, float maxLinearDeviation, float angleDeviation)
		{
			if (!m_cubes.ContainsKey(cubePos + table.Normal))
			{
				float maxValue = GridSize / 10f;
				using (MyUtils.ReuseCollection(ref m_tmpCubeSet))
				{
					GetExistingCubes(cubePos, table.CubeOffsets, m_tmpCubeSet);
					int num = 0;
					if (m_tmpCubeSet.Count > 0)
					{
						foreach (KeyValuePair<Vector3I, Matrix> item in table.OffsetTable)
						{
							Vector3I vector3I = item.Key >> 1;
							Vector3I vector3I2 = item.Key - Vector3I.One >> 1;
							if (m_tmpCubeSet.ContainsKey(vector3I) || (vector3I != vector3I2 && m_tmpCubeSet.ContainsKey(vector3I2)))
							{
								Vector3I boneOffset = item.Key;
								Vector3 clamp = new Vector3(GridSizeQuarter - random.NextFloat(0f, maxValue));
								Matrix matrix = item.Value;
								Vector3 moveDirection = random.NextDeviatingVector(ref matrix, angleDeviation) * random.NextFloat(1f, maxLinearDeviation);
								float displacementLength = moveDirection.Max();
								MoveBone(ref cubePos, ref boneOffset, ref moveDirection, ref displacementLength, ref clamp);
								num++;
							}
						}
					}
					m_tmpCubeSet.Clear();
				}
				dirtyMin = Vector3I.Min(dirtyMin, table.MinOffset);
				dirtyMax = Vector3I.Max(dirtyMax, table.MaxOffset);
				return true;
			}
			return false;
		}

		private void BlocksRemovedWithGenerator(List<Vector3I> blocksToRemove)
		{
			bool enable = EnableGenerators(enable: true, fromServer: true);
			BlocksRemoved(blocksToRemove);
			EnableGenerators(enable, fromServer: true);
		}

		private void BlocksRemovedWithoutGenerator(List<Vector3I> blocksToRemove)
		{
			bool enable = EnableGenerators(enable: false, fromServer: true);
			BlocksRemoved(blocksToRemove);
			EnableGenerators(enable, fromServer: true);
		}

		private void BlocksWithIdRemovedWithGenerator(List<BlockPositionId> blocksToRemove)
		{
			bool enable = EnableGenerators(enable: true, fromServer: true);
			BlocksWithIdRemoved(blocksToRemove);
			EnableGenerators(enable, fromServer: true);
		}

		private void BlocksWithIdRemovedWithoutGenerator(List<BlockPositionId> blocksToRemove)
		{
			bool enable = EnableGenerators(enable: false, fromServer: true);
			BlocksWithIdRemoved(blocksToRemove);
			EnableGenerators(enable, fromServer: true);
		}

		private void BlocksRemoved(List<Vector3I> blocksToRemove)
		{
			foreach (Vector3I item in blocksToRemove)
			{
				MySlimBlock cubeBlock = GetCubeBlock(item);
				if (cubeBlock != null)
				{
					RemoveBlockInternal(cubeBlock, close: true);
					Physics.AddDirtyBlock(cubeBlock);
				}
			}
		}

		private void BlocksWithIdRemoved(List<BlockPositionId> blocksToRemove)
		{
			foreach (BlockPositionId item in blocksToRemove)
			{
				if (item.CompoundId > 65535)
				{
					MySlimBlock cubeBlock = GetCubeBlock(item.Position);
					if (cubeBlock != null)
					{
						RemoveBlockInternal(cubeBlock, close: true);
						Physics.AddDirtyBlock(cubeBlock);
					}
				}
				else
				{
					Vector3I min = Vector3I.MaxValue;
					Vector3I max = Vector3I.MinValue;
					RemoveBlockInCompound(item.Position, (ushort)item.CompoundId, ref min, ref max);
					if (min != Vector3I.MaxValue)
					{
						Physics.AddDirtyArea(min, max);
					}
				}
			}
		}

		private void BlocksDestroyed(List<Vector3I> blockToDestroy)
		{
			m_largeDestroyInProgress = (blockToDestroy.Count > BLOCK_LIMIT_FOR_LARGE_DESTRUCTION);
			foreach (Vector3I item in blockToDestroy)
			{
				MySlimBlock cubeBlock = GetCubeBlock(item);
				if (cubeBlock != null)
				{
					RemoveDestroyedBlockInternal(cubeBlock);
					Physics.AddDirtyBlock(cubeBlock);
				}
			}
			m_largeDestroyInProgress = false;
		}

		private void BlocksWithIdDestroyedWithGenerator(List<BlockPositionId> blocksToRemove)
		{
			bool enable = EnableGenerators(enable: true, fromServer: true);
			BlocksWithIdRemoved(blocksToRemove);
			EnableGenerators(enable, fromServer: true);
		}

		private void BlocksWithIdDestroyedWithoutGenerator(List<BlockPositionId> blocksToRemove)
		{
			bool enable = EnableGenerators(enable: false, fromServer: true);
			BlocksWithIdRemoved(blocksToRemove);
			EnableGenerators(enable, fromServer: true);
		}

		private void BlocksDeformed(List<Vector3I> blockToDestroy)
		{
			foreach (Vector3I item in blockToDestroy)
			{
				MySlimBlock cubeBlock = GetCubeBlock(item);
				if (cubeBlock != null)
				{
					ApplyDestructionDeformationInternal(cubeBlock, sync: false, 1f, 0L);
					Physics.AddDirtyBlock(cubeBlock);
				}
			}
		}

		[Event(null, 6012)]
		[Reliable]
		[Broadcast]
		private void BlockIntegrityChanged(Vector3I pos, ushort subBlockId, float buildIntegrity, float integrity, MyIntegrityChangeEnum integrityChangeType, long grinderOwner)
		{
			MyCompoundCubeBlock myCompoundCubeBlock = null;
			MySlimBlock mySlimBlock = GetCubeBlock(pos);
			if (mySlimBlock != null)
			{
				myCompoundCubeBlock = (mySlimBlock.FatBlock as MyCompoundCubeBlock);
			}
			if (myCompoundCubeBlock != null)
			{
				mySlimBlock = myCompoundCubeBlock.GetBlock(subBlockId);
			}
			mySlimBlock?.SetIntegrity(buildIntegrity, integrity, integrityChangeType, grinderOwner);
		}

		[Event(null, 6029)]
		[Reliable]
		[Broadcast]
		private void BlockStockpileChanged(Vector3I pos, ushort subBlockId, List<MyStockpileItem> items)
		{
			MySlimBlock mySlimBlock = GetCubeBlock(pos);
			MyCompoundCubeBlock myCompoundCubeBlock = null;
			if (mySlimBlock != null)
			{
				myCompoundCubeBlock = (mySlimBlock.FatBlock as MyCompoundCubeBlock);
			}
			if (myCompoundCubeBlock != null)
			{
				mySlimBlock = myCompoundCubeBlock.GetBlock(subBlockId);
			}
			mySlimBlock?.ChangeStockpile(items);
		}

		[Event(null, 6048)]
		[Reliable]
		[Broadcast]
		private void FractureComponentRepaired(Vector3I pos, ushort subBlockId, long toolOwner)
		{
			MyCompoundCubeBlock myCompoundCubeBlock = null;
			MySlimBlock mySlimBlock = GetCubeBlock(pos);
			if (mySlimBlock != null)
			{
				myCompoundCubeBlock = (mySlimBlock.FatBlock as MyCompoundCubeBlock);
			}
			if (myCompoundCubeBlock != null)
			{
				mySlimBlock = myCompoundCubeBlock.GetBlock(subBlockId);
			}
			if (mySlimBlock != null && mySlimBlock.FatBlock != null)
			{
				mySlimBlock.RepairFracturedBlock(toolOwner);
			}
		}

		private void RemoveBlockByCubeBuilder(MySlimBlock block)
		{
			RemoveBlockInternal(block, close: true);
			if (block.FatBlock != null)
			{
				block.FatBlock.OnRemovedByCubeBuilder();
			}
		}

		private void RemoveBlockInternal(MySlimBlock block, bool close, bool markDirtyDisconnects = true)
		{
			if (!m_cubeBlocks.Contains(block))
			{
				return;
			}
			if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
			{
				RemoveMultiBlockInfo(block);
			}
			RenderData.RemoveDecals(block.Position);
			MyTerminalBlock myTerminalBlock = block.FatBlock as MyTerminalBlock;
			if (myTerminalBlock != null)
			{
				for (int i = 0; i < BlockGroups.Count; i++)
				{
					MyBlockGroup myBlockGroup = BlockGroups[i];
					if (myBlockGroup.Blocks.Contains(myTerminalBlock) && myBlockGroup.Blocks.Count == 1)
					{
						RemoveGroup(myBlockGroup);
						myBlockGroup.Blocks.Remove(myTerminalBlock);
						i--;
					}
				}
			}
			RemoveBlockParts(block);
			Parallel.Start(delegate
			{
				RemoveBlockEdges(block);
			});
			if (block.FatBlock != null)
			{
				if (block.FatBlock.InventoryCount > 0)
				{
					UnregisterInventory(block.FatBlock);
				}
				if (BlocksCounters.ContainsKey(block.BlockDefinition.Id.TypeId))
				{
					BlocksCounters[block.BlockDefinition.Id.TypeId]--;
				}
				block.FatBlock.IsBeingRemoved = true;
				GridSystems.UnregisterFromSystems(block.FatBlock);
				if (close)
				{
					block.FatBlock.Close();
				}
				else
				{
					base.Hierarchy.RemoveChild(block.FatBlock);
				}
				if (block.FatBlock.Render.NeedsDrawFromParent)
				{
					m_blocksForDraw.Remove(block.FatBlock);
					block.FatBlock.Render.SetVisibilityUpdates(state: false);
				}
			}
			block.RemoveNeighbours();
			block.RemoveAuthorship();
			m_PCU -= (block.ComponentStack.IsFunctional ? block.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
			m_cubeBlocks.Remove(block);
			if (block.FatBlock != null)
			{
				if (block.FatBlock is MyReactor)
				{
					NumberOfReactors--;
				}
				m_fatBlocks.Remove(block.FatBlock);
				block.FatBlock.IsBeingRemoved = false;
			}
			if (m_colorStatistics.ContainsKey(block.ColorMaskHSV))
			{
				m_colorStatistics[block.ColorMaskHSV]--;
				if (m_colorStatistics[block.ColorMaskHSV] <= 0)
				{
					m_colorStatistics.Remove(block.ColorMaskHSV);
				}
			}
			if (markDirtyDisconnects)
			{
				m_disconnectsDirty = MyTestDisconnectsReason.BlockRemoved;
			}
			Vector3I pos = block.Min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref block.Min, ref block.Max);
			while (vector3I_RangeIterator.IsValid())
			{
				Skeleton.MarkCubeRemoved(ref pos);
				vector3I_RangeIterator.GetNext(out pos);
			}
			if (block.FatBlock != null && block.FatBlock.IDModule != null)
			{
				ChangeOwner(block.FatBlock, block.FatBlock.IDModule.Owner, 0L);
			}
			if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
			{
				MyCubeGridSmallToLargeConnection.Static.RemoveBlockSmallToLargeConnection(block);
			}
			NotifyBlockRemoved(block);
			if (close)
			{
				NotifyBlockClosed(block);
			}
			m_boundsDirty = true;
			MarkForUpdate();
			MarkForDraw();
		}

		public void RemoveBlock(MySlimBlock block, bool updatePhysics = false)
		{
			if (Sync.IsServer && m_cubeBlocks.Contains(block))
			{
				EnqueueRemovedBlock(block.Min, m_generatorsEnabled);
				RemoveBlockInternal(block, close: true);
				if (updatePhysics)
				{
					Physics.AddDirtyBlock(block);
				}
			}
		}

		public void RemoveBlockWithId(MySlimBlock block, bool updatePhysics = false)
		{
			MySlimBlock cubeBlock = GetCubeBlock(block.Min);
			if (cubeBlock == null)
			{
				return;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
			ushort? compoundId = null;
			if (myCompoundCubeBlock != null)
			{
				compoundId = myCompoundCubeBlock.GetBlockId(block);
				if (!compoundId.HasValue)
				{
					return;
				}
			}
			RemoveBlockWithId(block.Min, compoundId, updatePhysics);
		}

		public void RemoveBlockWithId(Vector3I position, ushort? compoundId, bool updatePhysics = false)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			MySlimBlock cubeBlock = GetCubeBlock(position);
			if (cubeBlock != null)
			{
				EnqueueRemovedBlockWithId(cubeBlock.Min, compoundId, m_generatorsEnabled);
				if (compoundId.HasValue)
				{
					Vector3I min = Vector3I.Zero;
					Vector3I max = Vector3I.Zero;
					RemoveBlockInCompound(cubeBlock.Min, compoundId.Value, ref min, ref max);
				}
				else
				{
					RemoveBlockInternal(cubeBlock, close: true);
				}
				if (updatePhysics)
				{
					Physics.AddDirtyBlock(cubeBlock);
				}
			}
		}

		public void UpdateBlockNeighbours(MySlimBlock block)
		{
			if (m_cubeBlocks.Contains(block))
			{
				block.RemoveNeighbours();
				block.AddNeighbours();
				m_disconnectsDirty = MyTestDisconnectsReason.SplitBlock;
				MarkForUpdate();
			}
		}

		public Vector3 GetClosestCorner(Vector3I gridPos, Vector3 position)
		{
			return gridPos * GridSize - Vector3.SignNonZero(gridPos * GridSize - position) * GridSizeHalf;
		}

		public void DetectDisconnectsAfterFrame()
		{
			m_disconnectsDirty = MyTestDisconnectsReason.BlockRemoved;
			MarkForUpdate();
		}

		private void DetectDisconnects()
		{
			if (MyFakes.DETECT_DISCONNECTS && m_cubes.Count != 0 && Sync.IsServer)
			{
				MyPerformanceCounter.PerCameraDrawRead.CustomTimers.Remove("Mount points");
				MyPerformanceCounter.PerCameraDrawWrite.CustomTimers.Remove("Mount points");
				MyPerformanceCounter.PerCameraDrawRead.CustomTimers.Remove("Disconnect");
				MyPerformanceCounter.PerCameraDrawWrite.CustomTimers.Remove("Disconnect");
				MyPerformanceCounter.PerCameraDrawRead.StartTimer("Disconnect");
				MyPerformanceCounter.PerCameraDrawWrite.StartTimer("Disconnect");
				m_disconnectHelper.Disconnect(this, m_disconnectsDirty);
				m_disconnectsDirty = MyTestDisconnectsReason.NoReason;
				MyPerformanceCounter.PerCameraDrawRead.StopTimer("Disconnect");
				MyPerformanceCounter.PerCameraDrawWrite.StopTimer("Disconnect");
			}
		}

		public bool CubeExists(Vector3I pos)
		{
			return m_cubes.ContainsKey(pos);
		}

		public void UpdateDirty(Action callback = null, bool immediate = false)
		{
			if (!m_updatingDirty && m_resolvingSplits == 0)
			{
				m_updatingDirty = true;
				MyDirtyRegion dirtyRegion = m_dirtyRegion;
				m_dirtyRegion = m_dirtyRegionParallel;
				m_dirtyRegionParallel = dirtyRegion;
				if (immediate)
				{
					UpdateDirtyInternal();
					callback?.Invoke();
					OnUpdateDirtyCompleted();
				}
				else
				{
					Parallel.Start(m_UpdateDirtyInternal, callback = (Action)Delegate.Combine(callback, m_OnUpdateDirtyCompleted));
				}
			}
		}

		private void ClearDirty()
		{
			if (!m_updatingDirty && m_resolvingSplits == 0)
			{
				MyDirtyRegion dirtyRegion = m_dirtyRegion;
				m_dirtyRegion = m_dirtyRegionParallel;
				m_dirtyRegionParallel = dirtyRegion;
				m_dirtyRegionParallel.Cubes.Clear();
				MyCube result;
				while (m_dirtyRegionParallel.PartsToRemove.TryDequeue(out result))
				{
				}
			}
		}

		private void OnUpdateDirtyCompleted()
		{
			if (base.InScene)
			{
				UpdateInstanceData();
			}
			m_dirtyRegionParallel.Clear();
			m_updatingDirty = false;
			MarkForDraw();
			ReleaseMerginGrids();
		}

		public void UpdateDirtyInternal()
		{
			using (Pin())
			{
				if (!base.MarkedForClose)
				{
					m_dirtyRegionParallel.Cubes.ApplyChanges();
					foreach (Vector3I cube in m_dirtyRegionParallel.Cubes)
					{
						UpdateParts(cube);
					}
					MyCube result;
					while (m_dirtyRegionParallel.PartsToRemove.TryDequeue(out result))
					{
						UpdateParts(result.CubeBlock.Position);
						MyCubePart[] parts = result.Parts;
						foreach (MyCubePart part in parts)
						{
							Render.RenderData.RemoveCubePart(part);
						}
					}
					foreach (Vector3I cube2 in m_dirtyRegionParallel.Cubes)
					{
						MySlimBlock cubeBlock = GetCubeBlock(cube2);
						if (cubeBlock != null && cubeBlock.ShowParts && MyFakes.ENABLE_EDGES)
						{
							if (cubeBlock.Dithering >= 0f)
							{
								AddBlockEdges(cubeBlock);
							}
							else
							{
								RemoveBlockEdges(cubeBlock);
							}
							cubeBlock.UpdateMaxDeformation();
						}
						if (cubeBlock != null && cubeBlock.FatBlock != null && cubeBlock.FatBlock.Render != null && cubeBlock.FatBlock.Render.NeedsDrawFromParent)
						{
							m_blocksForDraw.Add(cubeBlock.FatBlock);
							cubeBlock.FatBlock.Render.SetVisibilityUpdates(state: true);
						}
					}
				}
			}
		}

		public bool IsDirty()
		{
			return m_dirtyRegion.IsDirty;
		}

		public void UpdateInstanceData()
		{
			Render.RebuildDirtyCells();
		}

		public bool TryGetCube(Vector3I position, out MyCube cube)
		{
			return m_cubes.TryGetValue(position, out cube);
		}

		private bool AddCube(MySlimBlock block, ref Vector3I pos, Matrix rotation, MyCubeBlockDefinition cubeBlockDefinition)
		{
			MyCube myCube = new MyCube
			{
				Parts = GetCubeParts(block.SkinSubtypeId, cubeBlockDefinition, pos, rotation, GridSize, GridScale),
				CubeBlock = block
			};
			MyCube orAdd = m_cubes.GetOrAdd(pos, myCube);
			if (myCube != orAdd)
			{
				return false;
			}
			m_dirtyRegion.AddCube(pos);
			MarkForDraw();
			return true;
		}

		private MyCube CreateCube(MySlimBlock block, Vector3I pos, Matrix rotation, MyCubeBlockDefinition cubeBlockDefinition)
		{
			return new MyCube
			{
				Parts = GetCubeParts(block.SkinSubtypeId, cubeBlockDefinition, pos, rotation, GridSize, GridScale),
				CubeBlock = block
			};
		}

		public bool ChangeColorAndSkin(MySlimBlock block, Vector3? newHSV = null, MyStringHash? skinSubtypeId = null)
		{
			try
			{
				MyStringHash skinSubtypeId2 = block.SkinSubtypeId;
				MyStringHash? rhs = skinSubtypeId;
				if (skinSubtypeId2 == rhs || !skinSubtypeId.HasValue)
				{
					Vector3 colorMaskHSV = block.ColorMaskHSV;
					Vector3? value = newHSV;
					if (colorMaskHSV == value || !newHSV.HasValue)
					{
						return false;
					}
				}
				if (newHSV.HasValue)
				{
					if (m_colorStatistics.TryGetValue(block.ColorMaskHSV, out int value2))
					{
						m_colorStatistics[block.ColorMaskHSV] = value2 - 1;
						if (m_colorStatistics[block.ColorMaskHSV] <= 0)
						{
							m_colorStatistics.Remove(block.ColorMaskHSV);
						}
					}
					block.ColorMaskHSV = newHSV.Value;
				}
				if (skinSubtypeId.HasValue)
				{
					block.SkinSubtypeId = skinSubtypeId.Value;
				}
				block.UpdateVisual(updatePhysics: false);
				if (newHSV.HasValue)
				{
					if (!m_colorStatistics.ContainsKey(block.ColorMaskHSV))
					{
						m_colorStatistics.Add(block.ColorMaskHSV, 0);
					}
					m_colorStatistics[block.ColorMaskHSV]++;
				}
				return true;
			}
			finally
			{
			}
		}

		private void UpdatePartInstanceData(MyCubePart part, Vector3I cubePos)
		{
			if (!m_cubes.TryGetValue(cubePos, out MyCube value))
			{
				return;
			}
			MySlimBlock cubeBlock = value.CubeBlock;
			if (cubeBlock != null)
			{
				part.InstanceData.SetColorMaskHSV(new Vector4(cubeBlock.ColorMaskHSV, cubeBlock.Dithering));
				part.SkinSubtypeId = value.CubeBlock.SkinSubtypeId;
			}
			if (part.Model.BoneMapping == null)
			{
				return;
			}
			Matrix orientation = part.InstanceData.LocalMatrix.GetOrientation();
			bool enableSkinning = false;
			part.InstanceData.BoneRange = GridSize;
			for (int i = 0; i < Math.Min(part.Model.BoneMapping.Length, 9); i++)
			{
				Vector3I bonePos = Vector3I.Round(Vector3.Transform((part.Model.BoneMapping[i] * 1f - Vector3.One) * 1f, orientation) + Vector3.One);
				Vector3UByte vector3UByte = Vector3UByte.Normalize(Skeleton.GetBone(cubePos, bonePos), GridSize);
				if (!Vector3UByte.IsMiddle(vector3UByte))
				{
					enableSkinning = true;
				}
				part.InstanceData[i] = vector3UByte;
			}
			part.InstanceData.EnableSkinning = enableSkinning;
		}

		private void UpdateParts(Vector3I pos)
		{
			MyCube value;
			bool flag = m_cubes.TryGetValue(pos, out value);
			if (flag && !value.CubeBlock.ShowParts)
			{
				RemoveBlockEdges(value.CubeBlock);
			}
			if (flag && value.CubeBlock.ShowParts)
			{
				MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(value.CubeBlock.BlockDefinition);
				value.CubeBlock.Orientation.GetMatrix(out Matrix result);
				if (Skeleton.IsDeformed(pos, 0.004f * GridSize, this, checkBlockDefinition: false))
				{
					RemoveBlockEdges(value.CubeBlock);
				}
				for (int i = 0; i < value.Parts.Length; i++)
				{
					UpdatePartInstanceData(value.Parts[i], pos);
					Render.RenderData.AddCubePart(value.Parts[i]);
					MyTileDefinition myTileDefinition = cubeTiles[i];
					if (myTileDefinition.IsEmpty)
					{
						continue;
					}
					Vector3 vec = Vector3.TransformNormal(myTileDefinition.Normal, result);
					Vector3 value2 = Vector3.TransformNormal(myTileDefinition.Up, result);
					if (!Base6Directions.IsBaseDirection(ref vec))
					{
						continue;
					}
					Vector3I key = pos + Vector3I.Round(vec);
					if (!m_cubes.TryGetValue(key, out MyCube value3) || !value3.CubeBlock.ShowParts)
					{
						continue;
					}
					value3.CubeBlock.Orientation.GetMatrix(out Matrix result2);
					MyTileDefinition[] cubeTiles2 = MyCubeGridDefinitions.GetCubeTiles(value3.CubeBlock.BlockDefinition);
					for (int j = 0; j < value3.Parts.Length; j++)
					{
						MyTileDefinition myTileDefinition2 = cubeTiles2[j];
						if (myTileDefinition2.IsEmpty)
						{
							continue;
						}
						Vector3 value4 = Vector3.TransformNormal(myTileDefinition2.Normal, result2);
						if (!((vec + value4).LengthSquared() < 0.001f))
						{
							continue;
						}
						if (value3.CubeBlock.Dithering != value.CubeBlock.Dithering)
						{
							Render.RenderData.AddCubePart(value3.Parts[j]);
							continue;
						}
						bool flag2 = false;
						if (myTileDefinition2.FullQuad && !myTileDefinition.IsRounded)
						{
							Render.RenderData.RemoveCubePart(value.Parts[i]);
							flag2 = true;
						}
						if (myTileDefinition.FullQuad && !myTileDefinition2.IsRounded)
						{
							Render.RenderData.RemoveCubePart(value3.Parts[j]);
							flag2 = true;
						}
						if (!flag2 && (myTileDefinition2.Up * myTileDefinition.Up).LengthSquared() > 0.001f && (Vector3.TransformNormal(myTileDefinition2.Up, result2) - value2).LengthSquared() < 0.001f)
						{
							if (!myTileDefinition.IsRounded && myTileDefinition2.IsRounded)
							{
								Render.RenderData.RemoveCubePart(value.Parts[i]);
							}
							if (myTileDefinition.IsRounded && !myTileDefinition2.IsRounded)
							{
								Render.RenderData.RemoveCubePart(value3.Parts[j]);
							}
						}
					}
				}
				return;
			}
			if (flag)
			{
				MyCubePart[] parts = value.Parts;
				foreach (MyCubePart part in parts)
				{
					Render.RenderData.RemoveCubePart(part);
				}
			}
			Vector3[] directions = Base6Directions.Directions;
			foreach (Vector3 vector in directions)
			{
				Vector3I key2 = pos + Vector3I.Round(vector);
				if (!m_cubes.TryGetValue(key2, out MyCube value5) || !value5.CubeBlock.ShowParts)
				{
					continue;
				}
				value5.CubeBlock.Orientation.GetMatrix(out Matrix result3);
				MyTileDefinition[] cubeTiles3 = MyCubeGridDefinitions.GetCubeTiles(value5.CubeBlock.BlockDefinition);
				for (int l = 0; l < value5.Parts.Length; l++)
				{
					Vector3 value6 = Vector3.Normalize(Vector3.TransformNormal(cubeTiles3[l].Normal, result3));
					if ((vector + value6).LengthSquared() < 0.001f)
					{
						Render.RenderData.AddCubePart(value5.Parts[l]);
					}
				}
			}
		}

		private void RemoveRedundantParts()
		{
			foreach (KeyValuePair<Vector3I, MyCube> cube in m_cubes)
			{
				UpdateParts(cube.Key);
			}
		}

		private void BoundsInclude(MySlimBlock block)
		{
			if (block != null)
			{
				m_min = Vector3I.Min(m_min, block.Min);
				m_max = Vector3I.Max(m_max, block.Max);
			}
		}

		private void BoundsIncludeUpdateAABB(MySlimBlock block)
		{
			BoundsInclude(block);
			UpdateGridAABB();
		}

		private void RecalcBounds()
		{
			m_min = Vector3I.MaxValue;
			m_max = Vector3I.MinValue;
			foreach (KeyValuePair<Vector3I, MyCube> cube in m_cubes)
			{
				m_min = Vector3I.Min(m_min, cube.Key);
				m_max = Vector3I.Max(m_max, cube.Key);
			}
			if (m_cubes.Count == 0)
			{
				m_min = -Vector3I.One;
				m_max = Vector3I.One;
			}
			UpdateGridAABB();
		}

		private void UpdateGridAABB()
		{
			base.PositionComp.LocalAABB = new BoundingBox(m_min * GridSize - GridSizeHalfVector, m_max * GridSize + GridSizeHalfVector);
		}

		private void ResetSkeleton()
		{
			Skeleton = new MyGridSkeleton();
		}

		private bool MoveCornerBones(Vector3I cubePos, Vector3I offset, ref Vector3I minCube, ref Vector3I maxCube)
		{
			Vector3I vector3I = Vector3I.Abs(offset);
			Vector3I vector3I2 = Vector3I.Shift(vector3I);
			Vector3I b = offset * vector3I2;
			Vector3I b2 = offset * Vector3I.Shift(vector3I2);
			Vector3 clamp = GridSizeQuarterVector;
			bool num = m_cubes.ContainsKey(cubePos + offset) & m_cubes.ContainsKey(cubePos + b) & m_cubes.ContainsKey(cubePos + b2);
			if (num)
			{
				Vector3I b3 = Vector3I.One - vector3I;
				Vector3I boneOffset = Vector3I.One + offset;
				Vector3I boneOffset2 = boneOffset + b3;
				Vector3I boneOffset3 = boneOffset - b3;
				Vector3 moveDirection = -offset * 0.25f;
				if (m_precalculatedCornerBonesDisplacementDistance <= 0f)
				{
					m_precalculatedCornerBonesDisplacementDistance = moveDirection.Length();
				}
				float precalculatedCornerBonesDisplacementDistance = m_precalculatedCornerBonesDisplacementDistance;
				precalculatedCornerBonesDisplacementDistance *= GridSize;
				moveDirection *= GridSize;
				MoveBone(ref cubePos, ref boneOffset, ref moveDirection, ref precalculatedCornerBonesDisplacementDistance, ref clamp);
				MoveBone(ref cubePos, ref boneOffset2, ref moveDirection, ref precalculatedCornerBonesDisplacementDistance, ref clamp);
				MoveBone(ref cubePos, ref boneOffset3, ref moveDirection, ref precalculatedCornerBonesDisplacementDistance, ref clamp);
				minCube = Vector3I.Min(Vector3I.Min(cubePos, minCube), cubePos + offset - b3);
				maxCube = Vector3I.Max(Vector3I.Max(cubePos, maxCube), cubePos + offset + b3);
			}
			return num;
		}

		private void GetExistingCubes(Vector3I cubePos, IEnumerable<Vector3I> offsets, Dictionary<Vector3I, MySlimBlock> resultSet)
		{
			resultSet.Clear();
			foreach (Vector3I offset in offsets)
			{
				Vector3I key = cubePos + offset;
				if (m_cubes.TryGetValue(key, out MyCube value) && !value.CubeBlock.IsDestroyed && value.CubeBlock.UsesDeformation)
				{
					resultSet[offset] = value.CubeBlock;
				}
			}
		}

		public void GetExistingCubes(Vector3I boneMin, Vector3I boneMax, Dictionary<Vector3I, MySlimBlock> resultSet, MyDamageInformation? damageInfo = null)
		{
			resultSet.Clear();
			Vector3I value = Vector3I.Floor((boneMin - Vector3I.One) / 2f);
			Vector3I value2 = Vector3I.Ceiling((boneMax - Vector3I.One) / 2f);
			MyDamageInformation info = damageInfo.HasValue ? damageInfo.Value : default(MyDamageInformation);
			Vector3I.Max(ref value, ref m_min, out value);
			Vector3I.Min(ref value2, ref m_max, out value2);
			Vector3I key = default(Vector3I);
			key.X = value.X;
			while (key.X <= value2.X)
			{
				key.Y = value.Y;
				while (key.Y <= value2.Y)
				{
					key.Z = value.Z;
					for (; key.Z <= value2.Z; key.Z++)
					{
						if (!m_cubes.TryGetValue(key, out MyCube value3) || !value3.CubeBlock.UsesDeformation)
						{
							continue;
						}
						if (value3.CubeBlock.UseDamageSystem && damageInfo.HasValue)
						{
							info.Amount = 1f;
							MyDamageSystem.Static.RaiseBeforeDamageApplied(value3.CubeBlock, ref info);
							if (info.Amount == 0f)
							{
								continue;
							}
						}
						resultSet[key] = value3.CubeBlock;
					}
					key.Y++;
				}
				key.X++;
			}
		}

		public MySlimBlock GetExistingCubeForBoneDeformations(ref Vector3I cube, ref MyDamageInformation damageInfo)
		{
			if (m_cubes.TryGetValue(cube, out MyCube value))
			{
				MySlimBlock cubeBlock = value.CubeBlock;
				if (cubeBlock.UsesDeformation)
				{
					if (cubeBlock.UseDamageSystem)
					{
						damageInfo.Amount = 1f;
						MyDamageSystem.Static.RaiseBeforeDamageApplied(cubeBlock, ref damageInfo);
						if (damageInfo.Amount == 0f)
						{
							return null;
						}
					}
					return cubeBlock;
				}
			}
			return null;
		}

		private void MoveBone(ref Vector3I cubePos, ref Vector3I boneOffset, ref Vector3 moveDirection, ref float displacementLength, ref Vector3 clamp)
		{
			m_totalBoneDisplacement += displacementLength;
			Vector3I pos = cubePos * 2 + boneOffset;
			Vector3 value = Vector3.Clamp(Skeleton[pos] + moveDirection, -clamp, clamp);
			Skeleton[pos] = value;
		}

		private void RemoveBlockParts(MySlimBlock block)
		{
			Vector3I key = default(Vector3I);
			key.X = block.Min.X;
			while (key.X <= block.Max.X)
			{
				key.Y = block.Min.Y;
				while (key.Y <= block.Max.Y)
				{
					key.Z = block.Min.Z;
					while (key.Z <= block.Max.Z)
					{
						if (m_cubes.TryRemove(key, out MyCube value))
						{
							m_dirtyRegion.PartsToRemove.Enqueue(value);
						}
						key.Z++;
					}
					key.Y++;
				}
				key.X++;
			}
			MarkForDraw();
		}

		public MyCubeGrid DetectMerge(MySlimBlock block, MyCubeGrid ignore = null, List<MyEntity> nearEntities = null, bool newGrid = false)
		{
			if (!IsStatic)
			{
				return null;
			}
			if (!Sync.IsServer)
			{
				return null;
			}
			if (block == null)
			{
				return null;
			}
			MyCubeGrid myCubeGrid = null;
			BoundingBoxD boundingBoxD = new BoundingBox(block.Min * GridSize - GridSizeHalf, block.Max * GridSize + GridSizeHalf);
			boundingBoxD.Inflate(GridSizeHalf);
			boundingBoxD = boundingBoxD.TransformFast(base.WorldMatrix);
			bool flag = false;
			if (nearEntities == null)
			{
				flag = true;
				nearEntities = MyEntities.GetEntitiesInAABB(ref boundingBoxD);
			}
			for (int i = 0; i < nearEntities.Count; i++)
			{
				MyCubeGrid myCubeGrid2 = nearEntities[i] as MyCubeGrid;
				MyCubeGrid myCubeGrid3 = myCubeGrid ?? this;
				if (myCubeGrid2 == null || myCubeGrid2 == this || myCubeGrid2 == ignore || myCubeGrid2.Physics == null || !myCubeGrid2.Physics.Enabled || !myCubeGrid2.IsStatic || myCubeGrid2.GridSizeEnum != myCubeGrid3.GridSizeEnum || !myCubeGrid3.IsMergePossible_Static(block, myCubeGrid2, out Vector3I _))
				{
					continue;
				}
				MyCubeGrid myCubeGrid4 = myCubeGrid3;
				MyCubeGrid myCubeGrid5 = myCubeGrid2;
				if (myCubeGrid2.BlocksCount > myCubeGrid3.BlocksCount || newGrid)
				{
					myCubeGrid4 = myCubeGrid2;
					myCubeGrid5 = myCubeGrid3;
				}
				Vector3I vector3I = Vector3I.Round(Vector3D.Transform(myCubeGrid5.PositionComp.GetPosition(), myCubeGrid4.PositionComp.WorldMatrixNormalizedInv) * GridSizeR);
				if (myCubeGrid4.CanMoveBlocksFrom(myCubeGrid5, vector3I))
				{
					if (newGrid)
					{
						MyMultiplayer.ReplicateImmediatelly(MyExternalReplicable.FindByObject(this), MyExternalReplicable.FindByObject(myCubeGrid4));
					}
					MyCubeGrid myCubeGrid6 = myCubeGrid4.MergeGrid_Static(myCubeGrid5, vector3I, block);
					if (myCubeGrid6 != null)
					{
						myCubeGrid = myCubeGrid6;
					}
				}
			}
			if (flag)
			{
				nearEntities.Clear();
			}
			return myCubeGrid;
		}

		private bool IsMergePossible_Static(MySlimBlock block, MyCubeGrid gridToMerge, out Vector3I gridOffset)
		{
			Vector3D position = base.PositionComp.GetPosition();
			position = Vector3D.Transform(position, gridToMerge.PositionComp.WorldMatrixNormalizedInv);
			gridOffset = -Vector3I.Round(position * GridSizeR);
			if (!IsOrientationsAligned(gridToMerge.WorldMatrix, base.WorldMatrix))
			{
				return false;
			}
			MatrixI matrix = gridToMerge.CalculateMergeTransform(this, -gridOffset);
			Vector3I.Transform(ref block.Position, ref matrix, out Vector3I result);
			MatrixI.Transform(ref block.Orientation, ref matrix).GetQuaternion(out Quaternion result2);
			MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
			return CheckConnectivity(gridToMerge, block.BlockDefinition, buildProgressModelMountPoints, ref result2, ref result);
		}

		public MatrixI CalculateMergeTransform(MyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			Vector3 vec = Vector3D.TransformNormal(gridToMerge.WorldMatrix.Forward, base.PositionComp.WorldMatrixNormalizedInv);
			Vector3 vec2 = Vector3D.TransformNormal(gridToMerge.WorldMatrix.Up, base.PositionComp.WorldMatrixNormalizedInv);
			Base6Directions.Direction closestDirection = Base6Directions.GetClosestDirection(vec);
			Base6Directions.Direction direction = Base6Directions.GetClosestDirection(vec2);
			if (direction == closestDirection)
			{
				direction = Base6Directions.GetPerpendicular(closestDirection);
			}
			return new MatrixI(ref gridOffset, closestDirection, direction);
		}

		public bool CanMergeCubes(MyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			MatrixI transform = CalculateMergeTransform(gridToMerge, gridOffset);
			foreach (KeyValuePair<Vector3I, MyCube> cube in gridToMerge.m_cubes)
			{
				Vector3I vector3I = Vector3I.Transform(cube.Key, transform);
				if (m_cubes.ContainsKey(vector3I))
				{
					MySlimBlock cubeBlock = GetCubeBlock(vector3I);
					if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
					{
						MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
						MySlimBlock cubeBlock2 = gridToMerge.GetCubeBlock(cube.Key);
						if (cubeBlock2.FatBlock is MyCompoundCubeBlock)
						{
							MyCompoundCubeBlock obj = cubeBlock2.FatBlock as MyCompoundCubeBlock;
							bool flag = true;
							foreach (MySlimBlock block in obj.GetBlocks())
							{
								MyBlockOrientation value = MatrixI.Transform(ref block.Orientation, ref transform);
								if (!myCompoundCubeBlock.CanAddBlock(block.BlockDefinition, value))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								continue;
							}
						}
						else
						{
							MyBlockOrientation value2 = MatrixI.Transform(ref cubeBlock2.Orientation, ref transform);
							if (myCompoundCubeBlock.CanAddBlock(cubeBlock2.BlockDefinition, value2))
							{
								continue;
							}
						}
					}
					return false;
				}
			}
			return true;
		}

		public void ChangeGridOwnership(long playerId, MyOwnershipShareModeEnum shareMode)
		{
			if (Sync.IsServer)
			{
				ChangeGridOwner(playerId, shareMode);
			}
		}

		private static void MoveBlocks(MyCubeGrid from, MyCubeGrid to, List<MySlimBlock> cubeBlocks, int offset, int count)
		{
			from.EnableGenerators(enable: false, fromServer: true);
			to.EnableGenerators(enable: false, fromServer: true);
			to.IsBlockTrasferInProgress = true;
			from.IsBlockTrasferInProgress = true;
			try
			{
				m_tmpBlockGroups.Clear();
				foreach (MyBlockGroup blockGroup in from.BlockGroups)
				{
					m_tmpBlockGroups.Add(blockGroup.GetObjectBuilder());
				}
				for (int i = offset; i < offset + count; i++)
				{
					MySlimBlock mySlimBlock = cubeBlocks[i];
					if (mySlimBlock != null)
					{
						if (mySlimBlock.FatBlock != null)
						{
							from.Hierarchy.RemoveChild(mySlimBlock.FatBlock);
						}
						from.RemoveBlockInternal(mySlimBlock, close: false, markDirtyDisconnects: false);
					}
				}
				if (from.Physics != null)
				{
					for (int j = offset; j < offset + count; j++)
					{
						MySlimBlock mySlimBlock2 = cubeBlocks[j];
						if (mySlimBlock2 != null)
						{
							from.Physics.AddDirtyBlock(mySlimBlock2);
						}
					}
				}
				for (int k = offset; k < offset + count; k++)
				{
					MySlimBlock mySlimBlock3 = cubeBlocks[k];
					if (mySlimBlock3 != null)
					{
						to.AddBlockInternal(mySlimBlock3);
						from.Skeleton.CopyTo(to.Skeleton, mySlimBlock3.Position);
					}
				}
				foreach (MyObjectBuilder_BlockGroup tmpBlockGroup in m_tmpBlockGroups)
				{
					MyBlockGroup myBlockGroup = new MyBlockGroup();
					myBlockGroup.Init(to, tmpBlockGroup);
					if (myBlockGroup.Blocks.Count > 0)
					{
						to.AddGroup(myBlockGroup);
					}
				}
				m_tmpBlockGroups.Clear();
				from.RemoveEmptyBlockGroups();
			}
			finally
			{
				from.EnableGenerators(enable: true, fromServer: true);
				to.EnableGenerators(enable: true, fromServer: true);
				to.IsBlockTrasferInProgress = false;
				from.IsBlockTrasferInProgress = false;
			}
		}

		private static void MoveBlocksByObjectBuilders(MyCubeGrid from, MyCubeGrid to, List<MySlimBlock> cubeBlocks, int offset, int count)
		{
			from.EnableGenerators(enable: false, fromServer: true);
			to.EnableGenerators(enable: false, fromServer: true);
			try
			{
				List<MyObjectBuilder_CubeBlock> list = new List<MyObjectBuilder_CubeBlock>();
				for (int i = offset; i < offset + count; i++)
				{
					MySlimBlock mySlimBlock = cubeBlocks[i];
					list.Add(mySlimBlock.GetObjectBuilder(copy: true));
				}
				MyEntityIdRemapHelper remapHelper = new MyEntityIdRemapHelper();
				foreach (MyObjectBuilder_CubeBlock item in list)
				{
					item.Remap(remapHelper);
				}
				for (int j = offset; j < offset + count; j++)
				{
					MySlimBlock block = cubeBlocks[j];
					from.RemoveBlockInternal(block, close: true, markDirtyDisconnects: false);
				}
				foreach (MyObjectBuilder_CubeBlock item2 in list)
				{
					to.AddBlock(item2, testMerge: false);
				}
			}
			finally
			{
				from.EnableGenerators(enable: true, fromServer: true);
				to.EnableGenerators(enable: true, fromServer: true);
			}
		}

		private void RemoveEmptyBlockGroups()
		{
			for (int i = 0; i < BlockGroups.Count; i++)
			{
				MyBlockGroup myBlockGroup = BlockGroups[i];
				if (myBlockGroup.Blocks.Count == 0)
				{
					RemoveGroup(myBlockGroup);
					i--;
				}
			}
		}

		private void AddBlockInternal(MySlimBlock block)
		{
			if (block.FatBlock != null)
			{
				block.FatBlock.UpdateWorldMatrix();
				if (block.FatBlock.InventoryCount > 0)
				{
					RegisterInventory(block.FatBlock);
				}
			}
			block.CubeGrid = this;
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && block.FatBlock is MyCompoundCubeBlock)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
				MySlimBlock cubeBlock = GetCubeBlock(block.Min);
				MyCompoundCubeBlock myCompoundCubeBlock2 = (cubeBlock != null) ? (cubeBlock.FatBlock as MyCompoundCubeBlock) : null;
				if (myCompoundCubeBlock2 != null)
				{
					bool flag = false;
					myCompoundCubeBlock.UpdateWorldMatrix();
					m_tmpSlimBlocks.Clear();
					foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
					{
						if (myCompoundCubeBlock2.Add(block2, out ushort _))
						{
							BoundsInclude(block2);
							m_dirtyRegion.AddCube(block2.Min);
							Physics.AddDirtyBlock(cubeBlock);
							m_tmpSlimBlocks.Add(block2);
							flag = true;
						}
					}
					MarkForDraw();
					foreach (MySlimBlock tmpSlimBlock in m_tmpSlimBlocks)
					{
						myCompoundCubeBlock.Remove(tmpSlimBlock, merged: true);
					}
					if (flag)
					{
						if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections)
						{
							MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(block);
						}
						foreach (MySlimBlock tmpSlimBlock2 in m_tmpSlimBlocks)
						{
							NotifyBlockAdded(tmpSlimBlock2);
						}
					}
					m_tmpSlimBlocks.Clear();
					return;
				}
			}
			m_cubeBlocks.Add(block);
			if (block.FatBlock != null)
			{
				m_fatBlocks.Add(block.FatBlock);
			}
			if (!m_colorStatistics.ContainsKey(block.ColorMaskHSV))
			{
				m_colorStatistics.Add(block.ColorMaskHSV, 0);
			}
			m_colorStatistics[block.ColorMaskHSV]++;
			block.AddNeighbours();
			BoundsInclude(block);
			if (block.FatBlock != null)
			{
				base.Hierarchy.AddChild(block.FatBlock);
				GridSystems.RegisterInSystems(block.FatBlock);
				if (block.FatBlock.Render.NeedsDrawFromParent)
				{
					m_blocksForDraw.Add(block.FatBlock);
					block.FatBlock.Render.SetVisibilityUpdates(state: true);
				}
				MyObjectBuilderType typeId = block.BlockDefinition.Id.TypeId;
				if (typeId != typeof(MyObjectBuilder_CubeBlock))
				{
					if (!BlocksCounters.ContainsKey(typeId))
					{
						BlocksCounters.Add(typeId, 0);
					}
					BlocksCounters[typeId]++;
				}
			}
			MyBlockOrientation orientation = block.Orientation;
			orientation.GetMatrix(out Matrix result);
			bool flag2 = true;
			Vector3I pos = default(Vector3I);
			pos.X = block.Min.X;
			while (pos.X <= block.Max.X)
			{
				pos.Y = block.Min.Y;
				while (pos.Y <= block.Max.Y)
				{
					pos.Z = block.Min.Z;
					while (pos.Z <= block.Max.Z)
					{
						flag2 &= AddCube(block, ref pos, result, block.BlockDefinition);
						pos.Z++;
					}
					pos.Y++;
				}
				pos.X++;
			}
			if (Physics != null)
			{
				Physics.AddBlock(block);
			}
			if (block.FatBlock != null)
			{
				ChangeOwner(block.FatBlock, 0L, block.FatBlock.OwnerId);
			}
			if (MyCubeGridSmallToLargeConnection.Static != null && m_enableSmallToLargeConnections && flag2)
			{
				MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(block);
			}
			if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS)
			{
				AddMultiBlockInfo(block);
			}
			NotifyBlockAdded(block);
			block.AddAuthorship();
			m_PCU += (block.ComponentStack.IsFunctional ? block.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
		}

		private bool IsDamaged(Vector3I bonePos, float epsilon = 0.04f)
		{
			if (Skeleton.TryGetBone(ref bonePos, out Vector3 bone))
			{
				return !MyUtils.IsZero(ref bone, epsilon * GridSize);
			}
			return false;
		}

		private void RemoveAuthorshipAll()
		{
			foreach (MySlimBlock block in GetBlocks())
			{
				block.RemoveAuthorship();
				m_PCU -= (block.ComponentStack.IsFunctional ? block.BlockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST);
			}
		}

		public void DismountAllCockpits()
		{
			foreach (MySlimBlock block in GetBlocks())
			{
				MyCockpit myCockpit = block.FatBlock as MyCockpit;
				if (myCockpit != null && myCockpit.Pilot != null)
				{
					myCockpit.Use();
				}
			}
		}

		private void AddBlockEdges(MySlimBlock block)
		{
			MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
			if (blockDefinition.BlockTopology != 0 || blockDefinition.CubeDefinition == null || !blockDefinition.CubeDefinition.ShowEdges)
			{
				return;
			}
			Vector3 translation = block.Position * GridSize;
			block.Orientation.GetMatrix(out Matrix result);
			result.Translation = translation;
			MyCubeGridDefinitions.TableEntry topologyInfo = MyCubeGridDefinitions.GetTopologyInfo(blockDefinition.CubeDefinition.CubeTopology);
			Vector3I a = block.Position * 2 + Vector3I.One;
			MyEdgeDefinition[] edges = topologyInfo.Edges;
			for (int i = 0; i < edges.Length; i++)
			{
				MyEdgeDefinition myEdgeDefinition = edges[i];
				Vector3 vector = Vector3.TransformNormal(myEdgeDefinition.Point0, block.Orientation);
				Vector3 vector2 = Vector3.TransformNormal(myEdgeDefinition.Point1, block.Orientation);
				Vector3 value = (vector + vector2) * 0.5f;
				if (!IsDamaged(a + Vector3I.Round(vector)) && !IsDamaged(a + Vector3I.Round(value)) && !IsDamaged(a + Vector3I.Round(vector2)))
				{
					vector = Vector3.Transform(myEdgeDefinition.Point0 * GridSizeHalf, ref result);
					vector2 = Vector3.Transform(myEdgeDefinition.Point1 * GridSizeHalf, ref result);
					Vector3 normal = Vector3.TransformNormal(topologyInfo.Tiles[myEdgeDefinition.Side0].Normal, block.Orientation);
					Vector3 normal2 = Vector3.TransformNormal(topologyInfo.Tiles[myEdgeDefinition.Side1].Normal, block.Orientation);
					Vector3 colorMaskHSV = block.ColorMaskHSV;
					colorMaskHSV.Y = (colorMaskHSV.Y + 1f) * 0.5f;
					colorMaskHSV.Z = (colorMaskHSV.Z + 1f) * 0.5f;
					Render.RenderData.AddEdgeInfo(ref vector, ref vector2, ref normal, ref normal2, colorMaskHSV.HSVtoColor(), block);
				}
			}
		}

		private void RemoveBlockEdges(MySlimBlock block)
		{
			using (Pin())
			{
				if (!base.MarkedForClose)
				{
					MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
					if (blockDefinition.BlockTopology == MyBlockTopology.Cube && blockDefinition.CubeDefinition != null)
					{
						Vector3 translation = block.Position * GridSize;
						block.Orientation.GetMatrix(out Matrix result);
						result.Translation = translation;
						MyEdgeDefinition[] edges = MyCubeGridDefinitions.GetTopologyInfo(blockDefinition.CubeDefinition.CubeTopology).Edges;
						for (int i = 0; i < edges.Length; i++)
						{
							MyEdgeDefinition myEdgeDefinition = edges[i];
							Vector3 point = Vector3.Transform(myEdgeDefinition.Point0 * GridSizeHalf, result);
							Vector3 point2 = Vector3.Transform(myEdgeDefinition.Point1 * GridSizeHalf, result);
							Render.RenderData.RemoveEdgeInfo(point, point2, block);
						}
					}
				}
			}
		}

		private long SendBones(MyVoxelSegmentationType segmentationType, out int bytes, out int segmentsCount, out int emptyBones)
		{
			_ = m_bonesToSendSecond.InputCount;
			long timestamp = Stopwatch.GetTimestamp();
			List<MyVoxelSegmentation.Segment> list = m_bonesToSendSecond.FindSegments(segmentationType);
			if (m_boneByteList == null)
			{
				m_boneByteList = new List<byte>();
			}
			else
			{
				m_boneByteList.Clear();
			}
			emptyBones = 0;
			foreach (MyVoxelSegmentation.Segment item in list)
			{
				emptyBones += ((!Skeleton.SerializePart(item.Min, item.Max, GridSize, m_boneByteList)) ? 1 : 0);
			}
			if (emptyBones != list.Count)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnBonesReceived, list.Count, m_boneByteList);
			}
			bytes = m_boneByteList.Count;
			segmentsCount = list.Count;
			return Stopwatch.GetTimestamp() - timestamp;
		}

		private void SendBonesAsync(WorkData workData)
		{
			_ = m_bonesToSendSecond.InputCount;
			MyTimeSpan.FromTicks(SendBones(MyVoxelSegmentationType.Simple, out int _, out int _, out int _));
			m_bonesToSendSecond.ClearInput();
			m_bonesSending = false;
		}

		private void DoLazyUpdates()
		{
			if (MyCubeGridSmallToLargeConnection.Static != null && !m_smallToLargeConnectionsInitialized && m_enableSmallToLargeConnections)
			{
				m_smallToLargeConnectionsInitialized = true;
				MyCubeGridSmallToLargeConnection.Static.AddGridSmallToLargeConnection(this);
			}
			m_smallToLargeConnectionsInitialized = true;
			if (!MyPerGameSettings.Destruction && BonesToSend.InputCount > 0 && m_bonesSendCounter++ > 10 && !m_bonesSending)
			{
				m_bonesSendCounter = 0;
				lock (BonesToSend)
				{
					MyVoxelSegmentation bonesToSend = BonesToSend;
					BonesToSend = m_bonesToSendSecond;
					m_bonesToSendSecond = bonesToSend;
				}
				_ = m_bonesToSendSecond.InputCount;
				if (Sync.IsServer)
				{
					m_bonesSending = true;
					m_workData.Priority = WorkPriority.Low;
					Parallel.Start(SendBonesAsync, null, m_workData);
				}
			}
			if (m_blocksForDamageApplicationDirty)
			{
				m_blocksForDamageApplicationCopy.AddRange(m_blocksForDamageApplication);
				foreach (MySlimBlock item in m_blocksForDamageApplicationCopy)
				{
					if (item.AccumulatedDamage > 0f)
					{
						item.ApplyAccumulatedDamage(addDirtyParts: true, 0L);
					}
				}
				m_blocksForDamageApplication.Clear();
				m_blocksForDamageApplicationCopy.Clear();
				m_blocksForDamageApplicationDirty = false;
			}
			if (m_disconnectsDirty != 0)
			{
				DetectDisconnects();
			}
			if (!MyPerGameSettings.Destruction)
			{
				Skeleton.RemoveUnusedBones(this);
			}
			if (m_ownershipManager.NeedRecalculateOwners)
			{
				m_ownershipManager.RecalculateOwners();
				m_ownershipManager.NeedRecalculateOwners = false;
				NotifyBlockOwnershipChange(this);
			}
		}

		internal void AddForDamageApplication(MySlimBlock block)
		{
			m_blocksForDamageApplication.Add(block);
			m_blocksForDamageApplicationDirty = true;
			MarkForUpdate();
		}

		internal void RemoveFromDamageApplication(MySlimBlock block)
		{
			m_blocksForDamageApplication.Remove(block);
			m_blocksForDamageApplicationDirty = (m_blocksForDamageApplication.Count > 0);
			if (m_blocksForDamageApplicationDirty)
			{
				MarkForUpdate();
			}
		}

		public bool GetLineIntersectionExactGrid(ref LineD line, ref Vector3I position, ref double distanceSquared)
		{
			return GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared, null);
		}

		public bool GetLineIntersectionExactGrid(ref LineD line, ref Vector3I position, ref double distanceSquared, MyPhysics.HitInfo? hitInfo = null)
		{
			RayCastCells(line.From, line.To, m_cacheRayCastCells, null, havokWorld: true);
			if (m_cacheRayCastCells.Count == 0)
			{
				return false;
			}
			m_tmpHitList.Clear();
			if (hitInfo.HasValue)
			{
				m_tmpHitList.Add(hitInfo.Value);
			}
			else
			{
				MyPhysics.CastRay(line.From, line.To, m_tmpHitList, 24);
			}
			if (m_tmpHitList.Count == 0)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < m_cacheRayCastCells.Count; i++)
			{
				Vector3I vector3I = m_cacheRayCastCells[i];
				m_cubes.TryGetValue(vector3I, out MyCube value);
				double num = double.MaxValue;
				if (value != null)
				{
					if (value.CubeBlock.FatBlock != null && !value.CubeBlock.FatBlock.BlockDefinition.UseModelIntersection)
					{
						if (m_tmpHitList.Count > 0)
						{
							int j = 0;
							if (MySession.Static.ControlledEntity != null)
							{
								for (; j < m_tmpHitList.Count - 1 && m_tmpHitList[j].HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity; j++)
								{
								}
							}
							if (j > 1 && m_tmpHitList[j].HkHitInfo.GetHitEntity() != this)
							{
								continue;
							}
							Vector3 gridSizeHalfVector = GridSizeHalfVector;
							Vector3D vector3D = Vector3D.Transform(m_tmpHitList[j].Position, base.PositionComp.WorldMatrixInvScaled);
							Vector3 vector = vector3I * GridSize;
							Vector3D value2 = vector3D - vector;
							double num2 = (value2.Max() > Math.Abs(value2.Min())) ? value2.Max() : value2.Min();
							value2.X = ((value2.X == num2) ? ((num2 > 0.0) ? 1 : (-1)) : 0);
							value2.Y = ((value2.Y == num2) ? ((num2 > 0.0) ? 1 : (-1)) : 0);
							value2.Z = ((value2.Z == num2) ? ((num2 > 0.0) ? 1 : (-1)) : 0);
							vector3D -= value2 * 0.059999998658895493;
							if (Vector3D.Max(vector3D, vector - gridSizeHalfVector) == vector3D && Vector3D.Min(vector3D, vector + gridSizeHalfVector) == vector3D)
							{
								num = Vector3D.DistanceSquared(line.From, m_tmpHitList[j].Position);
								if (num < distanceSquared)
								{
									position = vector3I;
									distanceSquared = num;
									flag = true;
									continue;
								}
							}
						}
					}
					else
					{
						GetBlockIntersection(value, ref line, IntersectionFlags.ALL_TRIANGLES, out MyIntersectionResultLineTriangleEx? t, out int _);
						if (t.HasValue)
						{
							num = Vector3.DistanceSquared(line.From, t.Value.IntersectionPointInWorldSpace);
						}
					}
				}
				if (num < distanceSquared)
				{
					distanceSquared = num;
					position = vector3I;
					flag = true;
				}
			}
			if (!flag)
			{
				for (int k = 0; k < m_cacheRayCastCells.Count; k++)
				{
					Vector3I vector3I2 = m_cacheRayCastCells[k];
					m_cubes.TryGetValue(vector3I2, out MyCube value3);
					double num3 = double.MaxValue;
					if (value3 == null || value3.CubeBlock.FatBlock == null || !value3.CubeBlock.FatBlock.BlockDefinition.UseModelIntersection)
					{
						if (m_tmpHitList.Count > 0)
						{
							int l = 0;
							if (MySession.Static.ControlledEntity != null)
							{
								for (; l < m_tmpHitList.Count - 1 && m_tmpHitList[l].HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity; l++)
								{
								}
							}
							if (l > 1 && m_tmpHitList[l].HkHitInfo.GetHitEntity() != this)
							{
								continue;
							}
							Vector3 gridSizeHalfVector2 = GridSizeHalfVector;
							Vector3D vector3D2 = Vector3D.Transform(m_tmpHitList[l].Position, base.PositionComp.WorldMatrixInvScaled);
							Vector3 vector2 = vector3I2 * GridSize;
							Vector3D value4 = vector3D2 - vector2;
							double num4 = (value4.Max() > Math.Abs(value4.Min())) ? value4.Max() : value4.Min();
							value4.X = ((value4.X == num4) ? ((num4 > 0.0) ? 1 : (-1)) : 0);
							value4.Y = ((value4.Y == num4) ? ((num4 > 0.0) ? 1 : (-1)) : 0);
							value4.Z = ((value4.Z == num4) ? ((num4 > 0.0) ? 1 : (-1)) : 0);
							vector3D2 -= value4 * 0.059999998658895493;
							if (Vector3D.Max(vector3D2, vector2 - gridSizeHalfVector2) == vector3D2 && Vector3D.Min(vector3D2, vector2 + gridSizeHalfVector2) == vector3D2)
							{
								if (value3 == null)
								{
									FixTargetCube(out Vector3I cube, vector3D2 * GridSizeR);
									if (!m_cubes.TryGetValue(cube, out value3))
									{
										continue;
									}
									vector3I2 = cube;
								}
								num3 = Vector3D.DistanceSquared(line.From, m_tmpHitList[l].Position);
								if (num3 < distanceSquared)
								{
									position = vector3I2;
									distanceSquared = num3;
									flag = true;
									continue;
								}
							}
						}
					}
					else
					{
						GetBlockIntersection(value3, ref line, IntersectionFlags.ALL_TRIANGLES, out MyIntersectionResultLineTriangleEx? t2, out int _);
						if (t2.HasValue)
						{
							num3 = Vector3.DistanceSquared(line.From, t2.Value.IntersectionPointInWorldSpace);
						}
					}
					if (num3 < distanceSquared)
					{
						distanceSquared = num3;
						position = vector3I2;
						flag = true;
					}
				}
			}
			m_tmpHitList.Clear();
			return flag;
		}

		private void GetBlockIntersection(MyCube cube, ref LineD line, IntersectionFlags flags, out MyIntersectionResultLineTriangleEx? t, out int cubePartIndex)
		{
			if (cube.CubeBlock.FatBlock != null)
			{
				if (cube.CubeBlock.FatBlock is MyCompoundCubeBlock)
				{
					MyCompoundCubeBlock obj = cube.CubeBlock.FatBlock as MyCompoundCubeBlock;
					MyIntersectionResultLineTriangleEx? myIntersectionResultLineTriangleEx = null;
					double num = double.MaxValue;
					foreach (MySlimBlock block in obj.GetBlocks())
					{
						block.Orientation.GetMatrix(out Matrix result);
						Vector3.TransformNormal(ref block.BlockDefinition.ModelOffset, ref result, out Vector3 result2);
						result.Translation = block.Position * GridSize + result2;
						MatrixD customInvMatrix = MatrixD.Invert(block.FatBlock.WorldMatrix);
						t = block.FatBlock.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, ref customInvMatrix, flags);
						if (!t.HasValue && block.FatBlock.Subparts != null)
						{
							foreach (KeyValuePair<string, MyEntitySubpart> subpart in block.FatBlock.Subparts)
							{
								customInvMatrix = MatrixD.Invert(subpart.Value.WorldMatrix);
								t = subpart.Value.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, ref customInvMatrix, flags);
								if (t.HasValue)
								{
									break;
								}
							}
						}
						if (t.HasValue)
						{
							MyIntersectionResultLineTriangleEx triangle = t.Value;
							double num2 = Vector3D.Distance(Vector3D.Transform(t.Value.IntersectionPointInObjectSpace, block.FatBlock.WorldMatrix), line.From);
							if (num2 < num)
							{
								num = num2;
								MatrixD? cubeWorldMatrix = block.FatBlock.WorldMatrix;
								TransformCubeToGrid(ref triangle, ref result, ref cubeWorldMatrix);
								myIntersectionResultLineTriangleEx = triangle;
							}
						}
					}
					t = myIntersectionResultLineTriangleEx;
				}
				else
				{
					cube.CubeBlock.FatBlock.GetIntersectionWithLine(ref line, out t);
					if (t.HasValue)
					{
						cube.CubeBlock.Orientation.GetMatrix(out Matrix result3);
						MyIntersectionResultLineTriangleEx triangle2 = t.Value;
						MatrixD? cubeWorldMatrix2 = cube.CubeBlock.FatBlock.WorldMatrix;
						TransformCubeToGrid(ref triangle2, ref result3, ref cubeWorldMatrix2);
						t = triangle2;
					}
				}
				cubePartIndex = -1;
				return;
			}
			MyIntersectionResultLineTriangleEx? myIntersectionResultLineTriangleEx2 = null;
			float num3 = float.MaxValue;
			int num4 = -1;
			for (int i = 0; i < cube.Parts.Length; i++)
			{
				MyCubePart myCubePart = cube.Parts[i];
				MatrixD matrix = myCubePart.InstanceData.LocalMatrix * base.WorldMatrix;
				MatrixD customInvMatrix2 = MatrixD.Invert(matrix);
				t = myCubePart.Model.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, ref customInvMatrix2, flags);
				if (t.HasValue)
				{
					MyIntersectionResultLineTriangleEx triangle3 = t.Value;
					float num5 = Vector3.Distance(Vector3.Transform(t.Value.IntersectionPointInObjectSpace, matrix), line.From);
					if (num5 < num3)
					{
						num3 = num5;
						Matrix cubeLocalMatrix = myCubePart.InstanceData.LocalMatrix;
						MatrixD? cubeWorldMatrix3 = null;
						TransformCubeToGrid(ref triangle3, ref cubeLocalMatrix, ref cubeWorldMatrix3);
						_ = (Vector3)triangle3.IntersectionPointInWorldSpace;
						myIntersectionResultLineTriangleEx2 = triangle3;
						num4 = i;
					}
				}
			}
			t = myIntersectionResultLineTriangleEx2;
			cubePartIndex = num4;
		}

		public static bool GetLineIntersection(ref LineD line, out MyCubeGrid grid, out Vector3I position, out double distanceSquared, Func<MyCubeGrid, bool> condition = null)
		{
			grid = null;
			position = default(Vector3I);
			distanceSquared = 3.4028234663852886E+38;
			MyEntities.OverlapAllLineSegment(ref line, m_lineOverlapList);
			foreach (MyLineSegmentOverlapResult<MyEntity> lineOverlap in m_lineOverlapList)
			{
				MyCubeGrid myCubeGrid = lineOverlap.Element as MyCubeGrid;
				if (myCubeGrid != null && (condition == null || condition(myCubeGrid)))
				{
					Vector3I? vector3I = myCubeGrid.RayCastBlocks(line.From, line.To);
					if (vector3I.HasValue)
					{
						Vector3 closestCorner = myCubeGrid.GetClosestCorner(vector3I.Value, line.From);
						float num = (float)Vector3D.DistanceSquared(line.From, Vector3D.Transform(closestCorner, myCubeGrid.WorldMatrix));
						if ((double)num < distanceSquared)
						{
							distanceSquared = num;
							grid = myCubeGrid;
							position = vector3I.Value;
						}
					}
				}
			}
			m_lineOverlapList.Clear();
			return grid != null;
		}

		public static bool GetLineIntersectionExact(ref LineD line, out MyCubeGrid grid, out Vector3I position, out double distanceSquared)
		{
			grid = null;
			position = default(Vector3I);
			distanceSquared = 3.4028234663852886E+38;
			double num = double.MaxValue;
			MyEntities.OverlapAllLineSegment(ref line, m_lineOverlapList);
			foreach (MyLineSegmentOverlapResult<MyEntity> lineOverlap in m_lineOverlapList)
			{
				MyCubeGrid myCubeGrid = lineOverlap.Element as MyCubeGrid;
				if (myCubeGrid != null && myCubeGrid.GetLineIntersectionExactAll(ref line, out double distance, out MySlimBlock _).HasValue && distance < num)
				{
					grid = myCubeGrid;
					num = distance;
				}
			}
			m_lineOverlapList.Clear();
			return grid != null;
		}

		public Vector3D? GetLineIntersectionExactAll(ref LineD line, out double distance, out MySlimBlock intersectedBlock)
		{
			intersectedBlock = null;
			distance = 3.4028234663852886E+38;
			Vector3I? vector3I = null;
			Vector3I position = Vector3I.Zero;
			double distanceSquared = double.MaxValue;
			if (GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared))
			{
				distanceSquared = (float)Math.Sqrt(distanceSquared);
				vector3I = position;
			}
			if (vector3I.HasValue)
			{
				distance = distanceSquared;
				intersectedBlock = GetCubeBlock(vector3I.Value);
				if (intersectedBlock == null)
				{
					return null;
				}
				return position;
			}
			return null;
		}

		public void GetBlocksInsideSphere(ref BoundingSphereD sphere, HashSet<MySlimBlock> blocks, bool checkTriangles = false)
		{
			blocks.Clear();
			if (base.PositionComp == null)
			{
				return;
			}
			BoundingBoxD aabb = BoundingBoxD.CreateFromSphere(sphere);
			MatrixD matrix = base.PositionComp.WorldMatrixNormalizedInv;
			Vector3D.Transform(ref sphere.Center, ref matrix, out Vector3D result);
			BoundingSphere localSphere = new BoundingSphere(result, (float)sphere.Radius);
			BoundingBox boundingBox = BoundingBox.CreateFromSphere(localSphere);
			Vector3D vector3D = boundingBox.Min;
			Vector3D vector3D2 = boundingBox.Max;
			Vector3I value = new Vector3I((int)Math.Round(vector3D.X * (double)GridSizeR), (int)Math.Round(vector3D.Y * (double)GridSizeR), (int)Math.Round(vector3D.Z * (double)GridSizeR));
			Vector3I value2 = new Vector3I((int)Math.Round(vector3D2.X * (double)GridSizeR), (int)Math.Round(vector3D2.Y * (double)GridSizeR), (int)Math.Round(vector3D2.Z * (double)GridSizeR));
			Vector3I start = Vector3I.Min(value, value2);
			Vector3I end = Vector3I.Max(value, value2);
			if ((end - start).Volume() < m_cubes.Count)
			{
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				Vector3I next = vector3I_RangeIterator.Current;
				while (vector3I_RangeIterator.IsValid())
				{
					if (m_cubes.TryGetValue(next, out MyCube value3))
					{
						AddBlockInSphere(ref aabb, blocks, checkTriangles, ref localSphere, value3);
					}
					vector3I_RangeIterator.GetNext(out next);
				}
			}
			else
			{
				foreach (MyCube value4 in m_cubes.Values)
				{
					AddBlockInSphere(ref aabb, blocks, checkTriangles, ref localSphere, value4);
				}
			}
		}

		private void AddBlockInSphere(ref BoundingBoxD aabb, HashSet<MySlimBlock> blocks, bool checkTriangles, ref BoundingSphere localSphere, MyCube cube)
		{
			MySlimBlock cubeBlock = cube.CubeBlock;
			if (!new BoundingBox(cubeBlock.Min * GridSize - GridSizeHalf, cubeBlock.Max * GridSize + GridSizeHalf).Intersects(localSphere))
			{
				return;
			}
			if (checkTriangles)
			{
				if (cubeBlock.FatBlock == null || cubeBlock.FatBlock.GetIntersectionWithAABB(ref aabb))
				{
					blocks.Add(cubeBlock);
				}
			}
			else
			{
				blocks.Add(cubeBlock);
			}
		}

		private void QuerySphere(BoundingSphereD sphere, List<MyEntity> blocks)
		{
			if (base.PositionComp == null)
			{
				return;
			}
			if (base.Closed)
			{
				MyLog.Default.WriteLine("Grid was Closed in MyCubeGrid.QuerySphere!");
			}
			if (sphere.Contains(base.PositionComp.WorldVolume) == ContainmentType.Contains)
			{
				foreach (MyCubeBlock fatBlock in m_fatBlocks)
				{
					if (!fatBlock.Closed)
					{
						blocks.Add(fatBlock);
						foreach (MyHierarchyComponentBase child in fatBlock.Hierarchy.Children)
						{
							MyEntity myEntity = (MyEntity)child.Entity;
							if (myEntity != null)
							{
								blocks.Add(myEntity);
							}
						}
					}
				}
				return;
			}
			BoundingBoxD boundingBoxD = new BoundingBoxD(sphere.Center - new Vector3D(sphere.Radius), sphere.Center + new Vector3D(sphere.Radius)).TransformFast(base.PositionComp.WorldMatrixNormalizedInv);
			Vector3D min = boundingBoxD.Min;
			Vector3D max = boundingBoxD.Max;
			Vector3I value = new Vector3I((int)Math.Round(min.X * (double)GridSizeR), (int)Math.Round(min.Y * (double)GridSizeR), (int)Math.Round(min.Z * (double)GridSizeR));
			Vector3I value2 = new Vector3I((int)Math.Round(max.X * (double)GridSizeR), (int)Math.Round(max.Y * (double)GridSizeR), (int)Math.Round(max.Z * (double)GridSizeR));
			Vector3I value3 = Vector3I.Min(value, value2);
			Vector3I value4 = Vector3I.Max(value, value2);
			value3 = Vector3I.Max(value3, Min);
			value4 = Vector3I.Min(value4, Max);
			if (value3.X > value4.X || value3.Y > value4.Y || value3.Z > value4.Z)
			{
				return;
			}
			Vector3 value5 = new Vector3(0.5f);
			BoundingBox box = default(BoundingBox);
			BoundingSphere boundingSphere = new BoundingSphere((Vector3)boundingBoxD.Center * GridSizeR, (float)sphere.Radius * GridSizeR);
			if ((value4 - value3).Size > m_cubeBlocks.Count)
			{
				foreach (MyCubeBlock fatBlock2 in m_fatBlocks)
				{
					if (!fatBlock2.Closed)
					{
						box.Min = fatBlock2.Min - value5;
						box.Max = fatBlock2.Max + value5;
						if (boundingSphere.Intersects(box))
						{
							blocks.Add(fatBlock2);
							foreach (MyHierarchyComponentBase child2 in fatBlock2.Hierarchy.Children)
							{
								MyEntity myEntity2 = (MyEntity)child2.Entity;
								if (myEntity2 != null)
								{
									blocks.Add(myEntity2);
								}
							}
						}
					}
				}
				return;
			}
			if (m_tmpQueryCubeBlocks == null)
			{
				m_tmpQueryCubeBlocks = new HashSet<MyEntity>();
			}
			if (m_cubes == null)
			{
				MyLog.Default.WriteLine("m_cubes null in MyCubeGrid.QuerySphere!");
			}
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref value3, ref value4);
			Vector3I next = vector3I_RangeIterator.Current;
			while (vector3I_RangeIterator.IsValid())
			{
				if (m_cubes.TryGetValue(next, out MyCube value6) && value6.CubeBlock.FatBlock != null && value6.CubeBlock.FatBlock != null && !value6.CubeBlock.FatBlock.Closed && !m_tmpQueryCubeBlocks.Contains(value6.CubeBlock.FatBlock))
				{
					box.Min = value6.CubeBlock.Min - value5;
					box.Max = value6.CubeBlock.Max + value5;
					if (boundingSphere.Intersects(box))
					{
						blocks.Add(value6.CubeBlock.FatBlock);
						m_tmpQueryCubeBlocks.Add(value6.CubeBlock.FatBlock);
						foreach (MyHierarchyComponentBase child3 in value6.CubeBlock.FatBlock.Hierarchy.Children)
						{
							MyEntity myEntity3 = (MyEntity)child3.Entity;
							if (myEntity3 != null)
							{
								blocks.Add(myEntity3);
								m_tmpQueryCubeBlocks.Add(myEntity3);
							}
						}
					}
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			m_tmpQueryCubeBlocks.Clear();
		}

		private void TransformCubeToGrid(ref MyIntersectionResultLineTriangleEx triangle, ref Matrix cubeLocalMatrix, ref MatrixD? cubeWorldMatrix)
		{
			if (!cubeWorldMatrix.HasValue)
			{
				MatrixD worldMatrix = base.WorldMatrix;
				triangle.IntersectionPointInObjectSpace = Vector3.Transform(triangle.IntersectionPointInObjectSpace, ref cubeLocalMatrix);
				triangle.IntersectionPointInWorldSpace = Vector3D.Transform(triangle.IntersectionPointInObjectSpace, worldMatrix);
				triangle.NormalInObjectSpace = Vector3.TransformNormal(triangle.NormalInObjectSpace, ref cubeLocalMatrix);
				triangle.NormalInWorldSpace = Vector3.TransformNormal(triangle.NormalInObjectSpace, worldMatrix);
			}
			else
			{
				Vector3 intersectionPointInObjectSpace = triangle.IntersectionPointInObjectSpace;
				Vector3 normalInObjectSpace = triangle.NormalInObjectSpace;
				triangle.IntersectionPointInObjectSpace = Vector3.Transform(intersectionPointInObjectSpace, ref cubeLocalMatrix);
				triangle.IntersectionPointInWorldSpace = Vector3D.Transform(intersectionPointInObjectSpace, cubeWorldMatrix.Value);
				triangle.NormalInObjectSpace = Vector3.TransformNormal(normalInObjectSpace, ref cubeLocalMatrix);
				triangle.NormalInWorldSpace = Vector3.TransformNormal(normalInObjectSpace, cubeWorldMatrix.Value);
			}
			triangle.Triangle.InputTriangle.Transform(ref cubeLocalMatrix);
		}

		private void QueryLine(LineD line, List<MyLineSegmentOverlapResult<MyEntity>> blocks)
		{
			MyLineSegmentOverlapResult<MyEntity> item = default(MyLineSegmentOverlapResult<MyEntity>);
			BoundingBoxD box = default(BoundingBoxD);
			MatrixD matrix = base.PositionComp.WorldMatrixNormalizedInv;
			Vector3D.Transform(ref line.From, ref matrix, out Vector3D result);
			Vector3D.Transform(ref line.To, ref matrix, out Vector3D result2);
			RayD rayD = new RayD(result, Vector3D.Normalize(result2 - result));
			RayCastCells(line.From, line.To, m_cacheRayCastCells);
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.TryGetValue(cacheRayCastCell, out MyCube value) && value.CubeBlock.FatBlock != null)
				{
					MyCubeBlock myCubeBlock = (MyCubeBlock)(item.Element = value.CubeBlock.FatBlock);
					box.Min = myCubeBlock.Min * GridSize - GridSizeHalfVector;
					box.Max = myCubeBlock.Max * GridSize + GridSizeHalfVector;
					double? num = rayD.Intersects(box);
					if (num.HasValue)
					{
						item.Distance = num.Value;
						blocks.Add(item);
					}
				}
			}
		}

		private void QueryAABB(BoundingBoxD box, List<MyEntity> blocks)
		{
			if (blocks == null || base.PositionComp == null)
			{
				return;
			}
			if (box.Contains(base.PositionComp.WorldAABB) == ContainmentType.Contains)
			{
				foreach (MyCubeBlock fatBlock2 in m_fatBlocks)
				{
					if (!fatBlock2.Closed)
					{
						blocks.Add(fatBlock2);
						if (fatBlock2.Hierarchy != null)
						{
							foreach (MyHierarchyComponentBase child in fatBlock2.Hierarchy.Children)
							{
								if (child.Container != null)
								{
									blocks.Add((MyEntity)child.Container.Entity);
								}
							}
						}
					}
				}
				return;
			}
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = MyOrientedBoundingBoxD.Create(box, base.PositionComp.WorldMatrixNormalizedInv);
			myOrientedBoundingBoxD.Center *= (double)GridSizeR;
			myOrientedBoundingBoxD.HalfExtent *= (double)GridSizeR;
			box = box.TransformFast(base.PositionComp.WorldMatrixNormalizedInv);
			Vector3D min = box.Min;
			Vector3D max = box.Max;
			Vector3I value = new Vector3I((int)Math.Round(min.X * (double)GridSizeR), (int)Math.Round(min.Y * (double)GridSizeR), (int)Math.Round(min.Z * (double)GridSizeR));
			Vector3I value2 = new Vector3I((int)Math.Round(max.X * (double)GridSizeR), (int)Math.Round(max.Y * (double)GridSizeR), (int)Math.Round(max.Z * (double)GridSizeR));
			Vector3I value3 = Vector3I.Min(value, value2);
			Vector3I value4 = Vector3I.Max(value, value2);
			value3 = Vector3I.Max(value3, Min);
			value4 = Vector3I.Min(value4, Max);
			if (value3.X > value4.X || value3.Y > value4.Y || value3.Z > value4.Z)
			{
				return;
			}
			Vector3 value5 = new Vector3(0.5f);
			BoundingBoxD box2 = default(BoundingBoxD);
			if ((value4 - value3).Size > m_cubeBlocks.Count)
			{
				foreach (MyCubeBlock fatBlock3 in m_fatBlocks)
				{
					if (!fatBlock3.Closed)
					{
						box2.Min = fatBlock3.Min - value5;
						box2.Max = fatBlock3.Max + value5;
						if (myOrientedBoundingBoxD.Intersects(ref box2))
						{
							blocks.Add(fatBlock3);
							if (fatBlock3.Hierarchy != null)
							{
								foreach (MyHierarchyComponentBase child2 in fatBlock3.Hierarchy.Children)
								{
									if (child2.Container != null)
									{
										blocks.Add((MyEntity)child2.Container.Entity);
									}
								}
							}
						}
					}
				}
				return;
			}
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref value3, ref value4);
			Vector3I next = vector3I_RangeIterator.Current;
			if (m_tmpQueryCubeBlocks == null)
			{
				m_tmpQueryCubeBlocks = new HashSet<MyEntity>();
			}
			while (vector3I_RangeIterator.IsValid())
			{
				if (m_cubes != null && m_cubes.TryGetValue(next, out MyCube value6) && value6.CubeBlock.FatBlock != null)
				{
					MyCubeBlock fatBlock = value6.CubeBlock.FatBlock;
					if (!m_tmpQueryCubeBlocks.Contains(fatBlock))
					{
						box2.Min = value6.CubeBlock.Min - value5;
						box2.Max = value6.CubeBlock.Max + value5;
						if (myOrientedBoundingBoxD.Intersects(ref box2))
						{
							m_tmpQueryCubeBlocks.Add(fatBlock);
							blocks.Add(fatBlock);
							if (fatBlock.Hierarchy != null)
							{
								foreach (MyHierarchyComponentBase child3 in fatBlock.Hierarchy.Children)
								{
									if (child3.Container != null)
									{
										blocks.Add((MyEntity)child3.Container.Entity);
										m_tmpQueryCubeBlocks.Add(fatBlock);
									}
								}
							}
						}
					}
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			m_tmpQueryCubeBlocks.Clear();
		}

		public void GetBlocksIntersectingOBB(BoundingBoxD box, MatrixD boxTransform, List<MySlimBlock> blocks)
		{
			if (blocks == null || base.PositionComp == null)
			{
				return;
			}
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = MyOrientedBoundingBoxD.Create(box, boxTransform);
			BoundingBoxD box2 = base.PositionComp.WorldAABB;
			if (myOrientedBoundingBoxD.Contains(ref box2) == ContainmentType.Contains)
			{
				foreach (MySlimBlock block in GetBlocks())
				{
					if (block.FatBlock == null || !block.FatBlock.Closed)
					{
						blocks.Add(block);
					}
				}
				return;
			}
			MatrixD matrixD = boxTransform * base.PositionComp.WorldMatrixNormalizedInv;
			MyOrientedBoundingBoxD myOrientedBoundingBoxD2 = MyOrientedBoundingBoxD.Create(box, matrixD);
			myOrientedBoundingBoxD2.Center *= (double)GridSizeR;
			myOrientedBoundingBoxD2.HalfExtent *= (double)GridSizeR;
			box = box.TransformFast(matrixD);
			Vector3D min = box.Min;
			Vector3D max = box.Max;
			Vector3I value = new Vector3I((int)Math.Round(min.X * (double)GridSizeR), (int)Math.Round(min.Y * (double)GridSizeR), (int)Math.Round(min.Z * (double)GridSizeR));
			Vector3I value2 = new Vector3I((int)Math.Round(max.X * (double)GridSizeR), (int)Math.Round(max.Y * (double)GridSizeR), (int)Math.Round(max.Z * (double)GridSizeR));
			Vector3I value3 = Vector3I.Min(value, value2);
			Vector3I value4 = Vector3I.Max(value, value2);
			value3 = Vector3I.Max(value3, Min);
			value4 = Vector3I.Min(value4, Max);
			if (value3.X > value4.X || value3.Y > value4.Y || value3.Z > value4.Z)
			{
				return;
			}
			Vector3 value5 = new Vector3(0.5f);
			BoundingBoxD box3 = default(BoundingBoxD);
			if ((value4 - value3).Size > m_cubeBlocks.Count)
			{
				foreach (MySlimBlock block2 in GetBlocks())
				{
					if (block2.FatBlock == null || !block2.FatBlock.Closed)
					{
						box3.Min = block2.Min - value5;
						box3.Max = block2.Max + value5;
						if (myOrientedBoundingBoxD2.Intersects(ref box3))
						{
							blocks.Add(block2);
						}
					}
				}
				return;
			}
			if (m_tmpQuerySlimBlocks == null)
			{
				m_tmpQuerySlimBlocks = new HashSet<MySlimBlock>();
			}
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref value3, ref value4);
			Vector3I next = vector3I_RangeIterator.Current;
			while (vector3I_RangeIterator.IsValid())
			{
				if (m_cubes != null && m_cubes.TryGetValue(next, out MyCube value6) && value6.CubeBlock != null)
				{
					MySlimBlock cubeBlock = value6.CubeBlock;
					if (!m_tmpQuerySlimBlocks.Contains(cubeBlock))
					{
						box3.Min = cubeBlock.Min - value5;
						box3.Max = cubeBlock.Max + value5;
						if (myOrientedBoundingBoxD2.Intersects(ref box3))
						{
							m_tmpQuerySlimBlocks.Add(cubeBlock);
							blocks.Add(cubeBlock);
						}
					}
				}
				vector3I_RangeIterator.GetNext(out next);
			}
			m_tmpQuerySlimBlocks.Clear();
		}

		public void GetBlocksInsideSpheres(ref BoundingSphereD sphere1, ref BoundingSphereD sphere2, ref BoundingSphereD sphere3, HashSet<MySlimBlock> blocks1, HashSet<MySlimBlock> blocks2, HashSet<MySlimBlock> blocks3, bool respectDeformationRatio, float detectionBlockHalfSize, ref MatrixD invWorldGrid)
		{
			blocks1.Clear();
			blocks2.Clear();
			blocks3.Clear();
			m_processedBlocks.Clear();
			Vector3D.Transform(ref sphere3.Center, ref invWorldGrid, out Vector3D result);
			Vector3I vector3I = Vector3I.Round((result - sphere3.Radius) * GridSizeR);
			Vector3I vector3I2 = Vector3I.Round((result + sphere3.Radius) * GridSizeR);
			Vector3 value = new Vector3(detectionBlockHalfSize);
			BoundingSphereD b = new BoundingSphereD(result, sphere1.Radius);
			BoundingSphereD b2 = new BoundingSphereD(result, sphere2.Radius);
			BoundingSphereD b3 = new BoundingSphereD(result, sphere3.Radius);
			if ((vector3I2.X - vector3I.X) * (vector3I2.Y - vector3I.Y) * (vector3I2.Z - vector3I.Z) < m_cubes.Count)
			{
				Vector3I key = default(Vector3I);
				key.X = vector3I.X;
				while (key.X <= vector3I2.X)
				{
					key.Y = vector3I.Y;
					while (key.Y <= vector3I2.Y)
					{
						key.Z = vector3I.Z;
						while (key.Z <= vector3I2.Z)
						{
							if (m_cubes.TryGetValue(key, out MyCube value2))
							{
								MySlimBlock cubeBlock = value2.CubeBlock;
								if (cubeBlock.FatBlock == null || !m_processedBlocks.Contains(cubeBlock.FatBlock))
								{
									m_processedBlocks.Add(cubeBlock.FatBlock);
									if (respectDeformationRatio)
									{
										b.Radius = sphere1.Radius * (double)cubeBlock.DeformationRatio;
										b2.Radius = sphere2.Radius * (double)cubeBlock.DeformationRatio;
										b3.Radius = sphere3.Radius * (double)cubeBlock.DeformationRatio;
									}
									BoundingBox boundingBox = (cubeBlock.FatBlock != null) ? new BoundingBox(cubeBlock.Min * GridSize - GridSizeHalf, cubeBlock.Max * GridSize + GridSizeHalf) : new BoundingBox(cubeBlock.Position * GridSize - value, cubeBlock.Position * GridSize + value);
									if (boundingBox.Intersects(b3))
									{
										if (boundingBox.Intersects(b2))
										{
											if (boundingBox.Intersects(b))
											{
												blocks1.Add(cubeBlock);
											}
											else
											{
												blocks2.Add(cubeBlock);
											}
										}
										else
										{
											blocks3.Add(cubeBlock);
										}
									}
								}
							}
							key.Z++;
						}
						key.Y++;
					}
					key.X++;
				}
			}
			else
			{
				foreach (MyCube value3 in m_cubes.Values)
				{
					MySlimBlock cubeBlock2 = value3.CubeBlock;
					if (cubeBlock2.FatBlock == null || !m_processedBlocks.Contains(cubeBlock2.FatBlock))
					{
						m_processedBlocks.Add(cubeBlock2.FatBlock);
						if (respectDeformationRatio)
						{
							b.Radius = sphere1.Radius * (double)cubeBlock2.DeformationRatio;
							b2.Radius = sphere2.Radius * (double)cubeBlock2.DeformationRatio;
							b3.Radius = sphere3.Radius * (double)cubeBlock2.DeformationRatio;
						}
						BoundingBox boundingBox2 = (cubeBlock2.FatBlock != null) ? new BoundingBox(cubeBlock2.Min * GridSize - GridSizeHalf, cubeBlock2.Max * GridSize + GridSizeHalf) : new BoundingBox(cubeBlock2.Position * GridSize - value, cubeBlock2.Position * GridSize + value);
						if (boundingBox2.Intersects(b3))
						{
							if (boundingBox2.Intersects(b2))
							{
								if (boundingBox2.Intersects(b))
								{
									blocks1.Add(cubeBlock2);
								}
								else
								{
									blocks2.Add(cubeBlock2);
								}
							}
							else
							{
								blocks3.Add(cubeBlock2);
							}
						}
					}
				}
			}
			m_processedBlocks.Clear();
		}

		internal HashSet<MyCube> RayCastBlocksAll(Vector3D worldStart, Vector3D worldEnd)
		{
			RayCastCells(worldStart, worldEnd, m_cacheRayCastCells);
			HashSet<MyCube> hashSet = new HashSet<MyCube>();
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell))
				{
					hashSet.Add(m_cubes[cacheRayCastCell]);
				}
			}
			return hashSet;
		}

		internal List<MyCube> RayCastBlocksAllOrdered(Vector3D worldStart, Vector3D worldEnd)
		{
			RayCastCells(worldStart, worldEnd, m_cacheRayCastCells);
			List<MyCube> list = new List<MyCube>();
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell) && !list.Contains(m_cubes[cacheRayCastCell]))
				{
					list.Add(m_cubes[cacheRayCastCell]);
				}
			}
			return list;
		}

		public Vector3I? RayCastBlocks(Vector3D worldStart, Vector3D worldEnd)
		{
			RayCastCells(worldStart, worldEnd, m_cacheRayCastCells);
			foreach (Vector3I cacheRayCastCell in m_cacheRayCastCells)
			{
				if (m_cubes.ContainsKey(cacheRayCastCell))
				{
					return cacheRayCastCell;
				}
			}
			return null;
		}

		public void RayCastCells(Vector3D worldStart, Vector3D worldEnd, List<Vector3I> outHitPositions, Vector3I? gridSizeInflate = null, bool havokWorld = false, bool clearOutHitPositions = true)
		{
			MatrixD matrix = base.PositionComp.WorldMatrixNormalizedInv;
			Vector3D.Transform(ref worldStart, ref matrix, out Vector3D result);
			Vector3D.Transform(ref worldEnd, ref matrix, out Vector3D result2);
			Vector3 gridSizeHalfVector = GridSizeHalfVector;
			result += gridSizeHalfVector;
			result2 += gridSizeHalfVector;
			Vector3I min = Min - Vector3I.One;
			Vector3I max = Max + Vector3I.One;
			if (gridSizeInflate.HasValue)
			{
				min -= gridSizeInflate.Value;
				max += gridSizeInflate.Value;
			}
			if (clearOutHitPositions)
			{
				outHitPositions.Clear();
			}
			MyGridIntersection.Calculate(outHitPositions, GridSize, result, result2, min, max);
		}

		public static void RayCastStaticCells(Vector3D worldStart, Vector3D worldEnd, List<Vector3I> outHitPositions, float gridSize, Vector3I? gridSizeInflate = null, bool havokWorld = false)
		{
			Vector3D lineStart = worldStart;
			Vector3D lineEnd = worldEnd;
			Vector3D vector3D = new Vector3D(gridSize * 0.5f);
			lineStart += vector3D;
			lineEnd += vector3D;
			Vector3I min = -Vector3I.One;
			Vector3I one = Vector3I.One;
			if (gridSizeInflate.HasValue)
			{
				min -= gridSizeInflate.Value;
				one += gridSizeInflate.Value;
			}
			outHitPositions.Clear();
			if (havokWorld)
			{
				MyGridIntersection.CalculateHavok(outHitPositions, gridSize, lineStart, lineEnd, min, one);
			}
			else
			{
				MyGridIntersection.Calculate(outHitPositions, gridSize, lineStart, lineEnd, min, one);
			}
		}

		void IMyGridConnectivityTest.GetConnectedBlocks(Vector3I minI, Vector3I maxI, Dictionary<Vector3I, ConnectivityResult> outOverlappedCubeBlocks)
		{
			Vector3I pos = default(Vector3I);
			pos.Z = minI.Z;
			while (pos.Z <= maxI.Z)
			{
				pos.Y = minI.Y;
				while (pos.Y <= maxI.Y)
				{
					pos.X = minI.X;
					while (pos.X <= maxI.X)
					{
						MySlimBlock cubeBlock = GetCubeBlock(pos);
						if (cubeBlock != null)
						{
							outOverlappedCubeBlocks[cubeBlock.Position] = new ConnectivityResult
							{
								Definition = cubeBlock.BlockDefinition,
								FatBlock = cubeBlock.FatBlock,
								Orientation = cubeBlock.Orientation,
								Position = cubeBlock.Position
							};
						}
						pos.X++;
					}
					pos.Y++;
				}
				pos.Z++;
			}
		}

		private string MakeCustomName()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int m = 10000;
			long num = MyMath.Mod(base.EntityId, m);
			string text = null;
			if (IsStatic)
			{
				text = MyTexts.GetString(MyCommonTexts.DetailStaticGrid);
			}
			else
			{
				switch (GridSizeEnum)
				{
				case MyCubeSize.Small:
					text = MyTexts.GetString(MyCommonTexts.DetailSmallGrid);
					break;
				case MyCubeSize.Large:
					text = MyTexts.GetString(MyCommonTexts.DetailLargeGrid);
					break;
				}
			}
			stringBuilder.Append(text ?? "Grid").Append(" ").Append(num.ToString());
			return stringBuilder.ToString();
		}

		public void ChangeOwner(MyCubeBlock block, long oldOwner, long newOwner)
		{
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				m_ownershipManager.ChangeBlockOwnership(block, oldOwner, newOwner);
			}
		}

		public void RecalculateOwners()
		{
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				m_ownershipManager.RecalculateOwners();
			}
		}

		public void UpdateOwnership(long ownerId, bool isFunctional)
		{
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				m_ownershipManager.UpdateOnFunctionalChange(ownerId, isFunctional);
			}
		}

		public override void Teleport(MatrixD worldMatrix, object source = null, bool ignoreAssert = false)
		{
			Dictionary<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> dictionary = new Dictionary<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>>();
			Dictionary<MyCubeGrid, Tuple<Vector3, Vector3>> dictionary2 = new Dictionary<MyCubeGrid, Tuple<Vector3, Vector3>>();
			HashSet<VRage.ModAPI.IMyEntity> hashSet = new HashSet<VRage.ModAPI.IMyEntity>();
			MyHashSetDictionary<MyCubeGrid, VRage.ModAPI.IMyEntity> myHashSetDictionary = new MyHashSetDictionary<MyCubeGrid, VRage.ModAPI.IMyEntity>();
			foreach (MyCubeBlock fatBlock in GetFatBlocks())
			{
				fatBlock.OnTeleport();
			}
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(this);
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
			{
				HashSet<VRage.ModAPI.IMyEntity> hashSet2 = new HashSet<VRage.ModAPI.IMyEntity>();
				hashSet2.Add(node.NodeData);
				node.NodeData.Hierarchy.GetChildrenRecursive(hashSet2);
				foreach (VRage.ModAPI.IMyEntity item in hashSet2)
				{
					if (item.Physics != null)
					{
						foreach (HkConstraint constraint in ((MyPhysicsBody)item.Physics).Constraints)
						{
							VRage.ModAPI.IMyEntity entity = constraint.RigidBodyA.GetEntity(0u);
							VRage.ModAPI.IMyEntity entity2 = constraint.RigidBodyB.GetEntity(0u);
							VRage.ModAPI.IMyEntity myEntity = (item == entity) ? entity2 : entity;
							if (!hashSet2.Contains(myEntity) && myEntity != null)
							{
								myHashSetDictionary.Add(node.NodeData, myEntity);
							}
						}
					}
				}
				dictionary.Add(node.NodeData, hashSet2);
			}
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item2 in dictionary)
			{
				foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item3 in dictionary)
				{
					if (myHashSetDictionary.TryGet(item2.Key, out HashSet<VRage.ModAPI.IMyEntity> list))
					{
						list.Remove(item3.Key);
						if (list.Count == 0)
						{
							myHashSetDictionary.Remove(item2.Key);
						}
					}
				}
			}
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item4 in dictionary.Reverse())
			{
				if (item4.Key.Physics != null)
				{
					dictionary2[item4.Key] = new Tuple<Vector3, Vector3>(item4.Key.Physics.LinearVelocity, item4.Key.Physics.AngularVelocity);
					foreach (VRage.ModAPI.IMyEntity item5 in item4.Value.Reverse())
					{
						if (item5.Physics != null && item5.Physics is MyPhysicsBody && !((MyPhysicsBody)item5.Physics).IsWelded)
						{
							if (item5.Physics.Enabled)
							{
								item5.Physics.Enabled = false;
							}
							else
							{
								hashSet.Add(item5);
							}
						}
					}
				}
			}
			Vector3D vector3D = worldMatrix.Translation - base.PositionComp.GetPosition();
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item6 in dictionary)
			{
				MatrixD worldMatrix2 = item6.Key.PositionComp.WorldMatrix;
				worldMatrix2.Translation += vector3D;
				item6.Key.PositionComp.SetWorldMatrix(worldMatrix2, source, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: true);
				if (myHashSetDictionary.TryGet(item6.Key, out HashSet<VRage.ModAPI.IMyEntity> list2))
				{
					foreach (VRage.ModAPI.IMyEntity item7 in list2)
					{
						MatrixD worldMatrix3 = item7.PositionComp.WorldMatrix;
						worldMatrix3.Translation += vector3D;
						item7.PositionComp.SetWorldMatrix(worldMatrix3, source, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: true);
					}
				}
			}
			BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node2 in group.Nodes)
			{
				boundingBoxD.Include(node2.NodeData.PositionComp.WorldAABB);
			}
			boundingBoxD = boundingBoxD.GetInflated(MyClusterTree.MinimumDistanceFromBorder);
			MyPhysics.EnsurePhysicsSpace(boundingBoxD);
			HkWorld hkWorld = null;
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item8 in dictionary)
			{
				if (item8.Key.Physics != null)
				{
					foreach (VRage.ModAPI.IMyEntity item9 in item8.Value)
					{
						if (item9.Physics != null && !((MyPhysicsBody)item9.Physics).IsWelded && !hashSet.Contains(item9))
						{
							((MyPhysicsBody)item9.Physics).LinearVelocity = dictionary2[item8.Key].Item1;
							((MyPhysicsBody)item9.Physics).AngularVelocity = dictionary2[item8.Key].Item2;
							((MyPhysicsBody)item9.Physics).EnableBatched();
							if (hkWorld == null)
							{
								hkWorld = ((MyPhysicsBody)item9.Physics).HavokWorld;
							}
						}
					}
				}
			}
			hkWorld?.FinishBatch();
			foreach (KeyValuePair<MyCubeGrid, HashSet<VRage.ModAPI.IMyEntity>> item10 in dictionary.Reverse())
			{
				if (item10.Key.Physics != null)
				{
					foreach (VRage.ModAPI.IMyEntity item11 in item10.Value.Reverse())
					{
						if (item11.Physics != null && item11.Physics is MyPhysicsBody && !((MyPhysicsBody)item11.Physics).IsWelded && !hashSet.Contains(item11))
						{
							((MyPhysicsBody)item11.Physics).FinishAddBatch();
						}
					}
				}
			}
		}

		public bool CanBeTeleported(MyGridJumpDriveSystem jumpingSystem, out MyGridJumpDriveSystem.MyJumpFailReason reason)
		{
			reason = MyGridJumpDriveSystem.MyJumpFailReason.None;
			if (MyFixedGrids.IsRooted(this))
			{
				reason = MyGridJumpDriveSystem.MyJumpFailReason.Static;
				return false;
			}
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in MyCubeGridGroups.Static.Physical.GetGroup(this).Nodes)
			{
				if (node.NodeData.Physics != null)
				{
					if (node.NodeData.IsStatic)
					{
						reason = MyGridJumpDriveSystem.MyJumpFailReason.Locked;
						return false;
					}
					if (MyFixedGrids.IsRooted(node.NodeData))
					{
						reason = MyGridJumpDriveSystem.MyJumpFailReason.Static;
						return false;
					}
					if (node.NodeData.GridSystems.JumpSystem.IsJumping && node.NodeData.GridSystems.JumpSystem != jumpingSystem)
					{
						reason = MyGridJumpDriveSystem.MyJumpFailReason.AlreadyJumping;
						return false;
					}
				}
			}
			return true;
		}

		public BoundingBoxD GetPhysicalGroupAABB()
		{
			if (base.MarkedForClose)
			{
				return BoundingBoxD.CreateInvalid();
			}
			BoundingBoxD worldAABB = base.PositionComp.WorldAABB;
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(this);
			if (group == null)
			{
				return worldAABB;
			}
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
			{
				if (node.NodeData.PositionComp != null)
				{
					worldAABB.Include(node.NodeData.PositionComp.WorldAABB);
				}
			}
			return worldAABB;
		}

		public MyFracturedBlock CreateFracturedBlock(MyObjectBuilder_FracturedBlock fracturedBlockBuilder, Vector3I position)
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FracturedBlock), "FracturedBlockLarge");
			MyDefinitionManager.Static.GetCubeBlockDefinition(id);
			if (m_cubes.TryGetValue(position, out MyCube value))
			{
				RemoveBlockInternal(value.CubeBlock, close: true);
			}
			fracturedBlockBuilder.CreatingFracturedBlock = true;
			MySlimBlock mySlimBlock = AddBlock(fracturedBlockBuilder, testMerge: false);
			if (mySlimBlock != null)
			{
				MyFracturedBlock myFracturedBlock = mySlimBlock.FatBlock as MyFracturedBlock;
				myFracturedBlock.Render.UpdateRenderObject(visible: true);
				UpdateBlockNeighbours(myFracturedBlock.SlimBlock);
				return myFracturedBlock;
			}
			return null;
		}

		private MyFracturedBlock CreateFracturedBlock(MyFracturedBlock.Info info)
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FracturedBlock), "FracturedBlockLarge");
			MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
			Vector3I position = info.Position;
			if (m_cubes.TryGetValue(position, out MyCube value))
			{
				RemoveBlock(value.CubeBlock);
			}
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = CreateBlockObjectBuilder(cubeBlockDefinition, position, new MyBlockOrientation(ref Quaternion.Identity), 0L, 0L, fullyBuilt: true);
			myObjectBuilder_CubeBlock.ColorMaskHSV = Vector3.Zero;
			(myObjectBuilder_CubeBlock as MyObjectBuilder_FracturedBlock).CreatingFracturedBlock = true;
			MySlimBlock mySlimBlock = AddBlock(myObjectBuilder_CubeBlock, testMerge: false);
			if (mySlimBlock == null)
			{
				info.Shape.RemoveReference();
				return null;
			}
			MyFracturedBlock myFracturedBlock = mySlimBlock.FatBlock as MyFracturedBlock;
			myFracturedBlock.OriginalBlocks = info.OriginalBlocks;
			myFracturedBlock.Orientations = info.Orientations;
			myFracturedBlock.MultiBlocks = info.MultiBlocks;
			myFracturedBlock.SetDataFromHavok(info.Shape, info.Compound);
			myFracturedBlock.Render.UpdateRenderObject(visible: true);
			UpdateBlockNeighbours(myFracturedBlock.SlimBlock);
			if (Sync.IsServer)
			{
				MySyncDestructions.CreateFracturedBlock((MyObjectBuilder_FracturedBlock)myFracturedBlock.GetObjectBuilderCubeBlock(), base.EntityId, position);
			}
			return myFracturedBlock;
		}

		public bool EnableGenerators(bool enable, bool fromServer = false)
		{
			bool generatorsEnabled = m_generatorsEnabled;
			if (Sync.IsServer || fromServer)
			{
				if (Render == null)
				{
					m_generatorsEnabled = false;
					return false;
				}
				if (m_generatorsEnabled != enable)
				{
					AdditionalModelGenerators.ForEach(delegate(IMyBlockAdditionalModelGenerator g)
					{
						g.EnableGenerator(enable);
					});
					m_generatorsEnabled = enable;
				}
			}
			return generatorsEnabled;
		}

		public MySlimBlock GetGeneratingBlock(MySlimBlock generatedBlock)
		{
			if (generatedBlock == null || !generatedBlock.BlockDefinition.IsGeneratedBlock)
			{
				return null;
			}
			foreach (IMyBlockAdditionalModelGenerator additionalModelGenerator in AdditionalModelGenerators)
			{
				MySlimBlock generatingBlock = additionalModelGenerator.GetGeneratingBlock(generatedBlock);
				if (generatingBlock != null)
				{
					return generatingBlock;
				}
			}
			return null;
		}

		public void GetGeneratedBlocks(MySlimBlock generatingBlock, List<MySlimBlock> outGeneratedBlocks)
		{
			outGeneratedBlocks.Clear();
			if (generatingBlock == null || generatingBlock.FatBlock is MyCompoundCubeBlock || generatingBlock.BlockDefinition.IsGeneratedBlock || generatingBlock.BlockDefinition.GeneratedBlockDefinitions == null || generatingBlock.BlockDefinition.GeneratedBlockDefinitions.Length == 0)
			{
				return;
			}
			Vector3I[] tmpBlockSurroundingOffsets = m_tmpBlockSurroundingOffsets;
			foreach (Vector3I b in tmpBlockSurroundingOffsets)
			{
				MySlimBlock cubeBlock = generatingBlock.CubeGrid.GetCubeBlock(generatingBlock.Position + b);
				if (cubeBlock != null && cubeBlock != generatingBlock)
				{
					if (cubeBlock.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
							if (block != generatingBlock && block.BlockDefinition.IsGeneratedBlock)
							{
								foreach (IMyBlockAdditionalModelGenerator additionalModelGenerator in AdditionalModelGenerators)
								{
									MySlimBlock generatingBlock2 = additionalModelGenerator.GetGeneratingBlock(block);
									if (generatingBlock == generatingBlock2)
									{
										outGeneratedBlocks.Add(block);
									}
								}
							}
						}
					}
					else if (cubeBlock.BlockDefinition.IsGeneratedBlock)
					{
						foreach (IMyBlockAdditionalModelGenerator additionalModelGenerator2 in AdditionalModelGenerators)
						{
							MySlimBlock generatingBlock3 = additionalModelGenerator2.GetGeneratingBlock(cubeBlock);
							if (generatingBlock == generatingBlock3)
							{
								outGeneratedBlocks.Add(cubeBlock);
							}
						}
					}
				}
			}
		}

		public void OnIntegrityChanged(MySlimBlock block, bool handWelded)
		{
			NotifyBlockIntegrityChanged(block, handWelded);
		}

		public void PasteBlocksToGrid(List<MyObjectBuilder_CubeGrid> gridsToMerge, long inventoryEntityId, bool multiBlock, bool instantBuild)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.PasteBlocksToGridServer_Implementation, gridsToMerge, inventoryEntityId, multiBlock, instantBuild);
		}

		[Event(null, 9516)]
		[Reliable]
		[Server]
		private void PasteBlocksToGridServer_Implementation(List<MyObjectBuilder_CubeGrid> gridsToMerge, long inventoryEntityId, bool multiBlock, bool instantBuild)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			bool num = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
			MyEntities.RemapObjectBuilderCollection(gridsToMerge);
			MatrixI arg = PasteBlocksServer(gridsToMerge);
			if (!(num && instantBuild) && MyEntities.TryGetEntityById(inventoryEntityId, out MyEntity entity) && entity != null)
			{
				MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory(entity);
				if (builderInventory != null)
				{
					if (multiBlock)
					{
						MyMultiBlockClipboard.TakeMaterialsFromBuilder(gridsToMerge, entity);
					}
					else
					{
						MyGridClipboard.CalculateItemRequirements(gridsToMerge, m_buildComponents);
						foreach (KeyValuePair<MyDefinitionId, int> totalMaterial in m_buildComponents.TotalMaterials)
						{
							builderInventory.RemoveItemsOfType(totalMaterial.Value, totalMaterial.Key);
						}
					}
				}
			}
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.PasteBlocksToGridClient_Implementation, gridsToMerge[0], arg);
			MyMultiplayer.GetReplicationServer()?.ResendMissingReplicableChildren(this);
		}

		[Event(null, 9560)]
		[Reliable]
		[Broadcast]
		private void PasteBlocksToGridClient_Implementation(MyObjectBuilder_CubeGrid gridToMerge, MatrixI mergeTransform)
		{
			PasteBlocksClient(gridToMerge, mergeTransform);
		}

		private void PasteBlocksClient(MyObjectBuilder_CubeGrid gridToMerge, MatrixI mergeTransform)
		{
			MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(gridToMerge, fadeIn: false) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyEntities.Add(myCubeGrid);
				MergeGridInternal(myCubeGrid, ref mergeTransform);
			}
		}

		private MatrixI PasteBlocksServer(List<MyObjectBuilder_CubeGrid> gridsToMerge)
		{
			MyCubeGrid myCubeGrid = null;
			foreach (MyObjectBuilder_CubeGrid item in gridsToMerge)
			{
				MyCubeGrid myCubeGrid2 = MyEntities.CreateFromObjectBuilder(item, fadeIn: false) as MyCubeGrid;
				if (myCubeGrid2 != null)
				{
					if (myCubeGrid == null)
					{
						myCubeGrid = myCubeGrid2;
					}
					MyEntities.Add(myCubeGrid2);
				}
			}
			MatrixI transform = CalculateMergeTransform(myCubeGrid, WorldToGridInteger(myCubeGrid.PositionComp.GetPosition()));
			MergeGridInternal(myCubeGrid, ref transform, disableBlockGenerators: false);
			return transform;
		}

		public static bool CanPasteGrid()
		{
			return MySession.Static.IsCopyPastingEnabled;
		}

		public MyCubeGrid GetBiggestGridInGroup()
		{
			MyCubeGrid result = this;
			double num = 0.0;
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in MyCubeGridGroups.Static.Physical.GetGroup(this).Nodes)
			{
				double volume = node.NodeData.PositionComp.WorldAABB.Size.Volume;
				if (volume > num)
				{
					num = volume;
					result = node.NodeData;
				}
			}
			return result;
		}

		public void ConvertFracturedBlocksToComponents()
		{
			List<MyFracturedBlock> list = new List<MyFracturedBlock>();
			foreach (MySlimBlock cubeBlock in m_cubeBlocks)
			{
				MyFracturedBlock myFracturedBlock = cubeBlock.FatBlock as MyFracturedBlock;
				if (myFracturedBlock != null)
				{
					list.Add(myFracturedBlock);
				}
			}
			bool enable = EnableGenerators(enable: false);
			try
			{
				foreach (MyFracturedBlock item in list)
				{
					MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = item.ConvertToOriginalBlocksWithFractureComponent();
					RemoveBlockInternal(item.SlimBlock, close: true, markDirtyDisconnects: false);
					if (myObjectBuilder_CubeBlock != null)
					{
						AddBlock(myObjectBuilder_CubeBlock, testMerge: false);
					}
				}
			}
			finally
			{
				EnableGenerators(enable);
			}
		}

		public void PrepareMultiBlockInfos()
		{
			foreach (MySlimBlock block in GetBlocks())
			{
				AddMultiBlockInfo(block);
			}
		}

		internal void AddMultiBlockInfo(MySlimBlock block)
		{
			MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
			if (myCompoundCubeBlock != null)
			{
				foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
				{
					if (block2.IsMultiBlockPart)
					{
						AddMultiBlockInfo(block2);
					}
				}
			}
			else if (block.IsMultiBlockPart)
			{
				if (m_multiBlockInfos == null)
				{
					m_multiBlockInfos = new Dictionary<int, MyCubeGridMultiBlockInfo>();
				}
				if (!m_multiBlockInfos.TryGetValue(block.MultiBlockId, out MyCubeGridMultiBlockInfo value))
				{
					value = new MyCubeGridMultiBlockInfo();
					value.MultiBlockId = block.MultiBlockId;
					value.MultiBlockDefinition = block.MultiBlockDefinition;
					value.MainBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinitionForMultiBlock(block.MultiBlockDefinition.Id.SubtypeName);
					m_multiBlockInfos.Add(block.MultiBlockId, value);
				}
				value.Blocks.Add(block);
			}
		}

		internal void RemoveMultiBlockInfo(MySlimBlock block)
		{
			if (m_multiBlockInfos != null)
			{
				MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
				MyCubeGridMultiBlockInfo value;
				if (myCompoundCubeBlock != null)
				{
					foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
					{
						if (block2.IsMultiBlockPart)
						{
							RemoveMultiBlockInfo(block2);
						}
					}
				}
				else if (block.IsMultiBlockPart && m_multiBlockInfos.TryGetValue(block.MultiBlockId, out value) && value.Blocks.Remove(block) && value.Blocks.Count == 0 && m_multiBlockInfos.Remove(block.MultiBlockId) && m_multiBlockInfos.Count == 0)
				{
					m_multiBlockInfos = null;
				}
			}
		}

		public MyCubeGridMultiBlockInfo GetMultiBlockInfo(int multiBlockId)
		{
			if (m_multiBlockInfos != null && m_multiBlockInfos.TryGetValue(multiBlockId, out MyCubeGridMultiBlockInfo value))
			{
				return value;
			}
			return null;
		}

		public void GetBlocksInMultiBlock(int multiBlockId, HashSet<Tuple<MySlimBlock, ushort?>> outMultiBlocks)
		{
			if (multiBlockId != 0)
			{
				MyCubeGridMultiBlockInfo multiBlockInfo = GetMultiBlockInfo(multiBlockId);
				if (multiBlockInfo != null)
				{
					foreach (MySlimBlock block in multiBlockInfo.Blocks)
					{
						MySlimBlock cubeBlock = GetCubeBlock(block.Position);
						MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
						if (myCompoundCubeBlock != null)
						{
							ushort? blockId = myCompoundCubeBlock.GetBlockId(block);
							outMultiBlocks.Add(new Tuple<MySlimBlock, ushort?>(cubeBlock, blockId));
						}
						else
						{
							outMultiBlocks.Add(new Tuple<MySlimBlock, ushort?>(cubeBlock, null));
						}
					}
				}
			}
		}

		public bool CanAddMultiBlocks(MyCubeGridMultiBlockInfo multiBlockInfo, ref MatrixI transform, List<int> multiBlockIndices)
		{
			foreach (int multiBlockIndex in multiBlockIndices)
			{
				if (multiBlockIndex < multiBlockInfo.MultiBlockDefinition.BlockDefinitions.Length)
				{
					MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition = multiBlockInfo.MultiBlockDefinition.BlockDefinitions[multiBlockIndex];
					if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(myMultiBlockPartDefinition.Id, out MyCubeBlockDefinition blockDefinition) || blockDefinition == null)
					{
						return false;
					}
					Vector3I vector3I = Vector3I.Transform(myMultiBlockPartDefinition.Min, ref transform);
					MatrixI leftMatrix = new MatrixI(myMultiBlockPartDefinition.Forward, myMultiBlockPartDefinition.Up);
					MatrixI.Multiply(ref leftMatrix, ref transform, out MatrixI result);
					MyBlockOrientation blockOrientation = result.GetBlockOrientation();
					if (!CanPlaceBlock(vector3I, vector3I, blockOrientation, blockDefinition, 0uL, multiBlockInfo.MultiBlockId, ignoreFracturedPieces: true))
					{
						return false;
					}
				}
			}
			return true;
		}

		public bool BuildMultiBlocks(MyCubeGridMultiBlockInfo multiBlockInfo, ref MatrixI transform, List<int> multiBlockIndices, long builderEntityId, MyStringHash skinId)
		{
			List<MyBlockLocation> list = new List<MyBlockLocation>();
			List<MyObjectBuilder_CubeBlock> list2 = new List<MyObjectBuilder_CubeBlock>();
			foreach (int multiBlockIndex in multiBlockIndices)
			{
				if (multiBlockIndex < multiBlockInfo.MultiBlockDefinition.BlockDefinitions.Length)
				{
					MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition = multiBlockInfo.MultiBlockDefinition.BlockDefinitions[multiBlockIndex];
					if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(myMultiBlockPartDefinition.Id, out MyCubeBlockDefinition blockDefinition) || blockDefinition == null)
					{
						return false;
					}
					Vector3I vector3I = Vector3I.Transform(myMultiBlockPartDefinition.Min, ref transform);
					MatrixI leftMatrix = new MatrixI(myMultiBlockPartDefinition.Forward, myMultiBlockPartDefinition.Up);
					MatrixI.Multiply(ref leftMatrix, ref transform, out MatrixI result);
					MyBlockOrientation blockOrientation = result.GetBlockOrientation();
					if (!CanPlaceBlock(vector3I, vector3I, blockOrientation, blockDefinition, 0uL, multiBlockInfo.MultiBlockId))
					{
						return false;
					}
					MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyObjectBuilderSerializer.CreateNewObject(myMultiBlockPartDefinition.Id) as MyObjectBuilder_CubeBlock;
					myObjectBuilder_CubeBlock.Orientation = Base6Directions.GetOrientation(blockOrientation.Forward, blockOrientation.Up);
					myObjectBuilder_CubeBlock.Min = vector3I;
					myObjectBuilder_CubeBlock.ColorMaskHSV = MyPlayer.SelectedColor;
					myObjectBuilder_CubeBlock.SkinSubtypeId = MyPlayer.SelectedArmorSkin;
					myObjectBuilder_CubeBlock.MultiBlockId = multiBlockInfo.MultiBlockId;
					myObjectBuilder_CubeBlock.MultiBlockIndex = multiBlockIndex;
					myObjectBuilder_CubeBlock.MultiBlockDefinition = multiBlockInfo.MultiBlockDefinition.Id;
					list2.Add(myObjectBuilder_CubeBlock);
					MyBlockLocation item = default(MyBlockLocation);
					item.Min = vector3I;
					item.Max = vector3I;
					item.CenterPos = vector3I;
					item.Orientation = new MyBlockOrientation(blockOrientation.Forward, blockOrientation.Up);
					item.BlockDefinition = myMultiBlockPartDefinition.Id;
					item.EntityId = MyEntityIdentifier.AllocateId();
					item.Owner = builderEntityId;
					list.Add(item);
				}
			}
			if (MySession.Static.SurvivalMode)
			{
				MyEntity entityById = MyEntities.GetEntityById(builderEntityId);
				if (entityById == null)
				{
					return false;
				}
				HashSet<MyBlockLocation> hashSet = new HashSet<MyBlockLocation>(list);
				MyCubeBuilder.BuildComponent.GetBlocksPlacementMaterials(hashSet, this);
				if (!MyCubeBuilder.BuildComponent.HasBuildingMaterials(entityById))
				{
					return false;
				}
			}
			MyBlockVisuals arg = new MyBlockVisuals(MyPlayer.SelectedColor.PackHSVToUint(), skinId);
			for (int i = 0; i < list.Count && i < list2.Count; i++)
			{
				MyBlockLocation arg2 = list[i];
				MyObjectBuilder_CubeBlock arg3 = list2[i];
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BuildBlockRequest, arg, arg2, arg3, builderEntityId, arg6: false, MySession.Static.LocalPlayerId);
			}
			return true;
		}

		private bool GetMissingBlocksMultiBlock(int multiblockId, out MyCubeGridMultiBlockInfo multiBlockInfo, out MatrixI transform, List<int> multiBlockIndices)
		{
			transform = default(MatrixI);
			multiBlockInfo = GetMultiBlockInfo(multiblockId);
			if (multiBlockInfo == null)
			{
				return false;
			}
			return multiBlockInfo.GetMissingBlocks(out transform, multiBlockIndices);
		}

		public bool CanAddMissingBlocksInMultiBlock(int multiBlockId)
		{
			try
			{
				if (!GetMissingBlocksMultiBlock(multiBlockId, out MyCubeGridMultiBlockInfo multiBlockInfo, out MatrixI transform, m_tmpMultiBlockIndices))
				{
					return false;
				}
				return CanAddMultiBlocks(multiBlockInfo, ref transform, m_tmpMultiBlockIndices);
			}
			finally
			{
				m_tmpMultiBlockIndices.Clear();
			}
		}

		public void AddMissingBlocksInMultiBlock(int multiBlockId, long toolOwnerId)
		{
			try
			{
				if (GetMissingBlocksMultiBlock(multiBlockId, out MyCubeGridMultiBlockInfo multiBlockInfo, out MatrixI transform, m_tmpMultiBlockIndices))
				{
					MyStringHash orCompute = MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin);
					BuildMultiBlocks(multiBlockInfo, ref transform, m_tmpMultiBlockIndices, toolOwnerId, orCompute);
				}
			}
			finally
			{
				m_tmpMultiBlockIndices.Clear();
			}
		}

		public bool CanAddOtherBlockInMultiBlock(Vector3I min, Vector3I max, MyBlockOrientation orientation, MyCubeBlockDefinition definition, int? ignoreMultiblockId)
		{
			if (m_multiBlockInfos == null)
			{
				return true;
			}
			foreach (KeyValuePair<int, MyCubeGridMultiBlockInfo> multiBlockInfo in m_multiBlockInfos)
			{
				if ((!ignoreMultiblockId.HasValue || ignoreMultiblockId.Value != multiBlockInfo.Key) && !multiBlockInfo.Value.CanAddBlock(ref min, ref max, orientation, definition))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsGridInCompleteState(MyCubeGrid grid)
		{
			foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
			{
				if (!cubeBlock.IsFullIntegrity || cubeBlock.BuildLevelRatio != 1f)
				{
					return false;
				}
			}
			return true;
		}

		public bool WillRemoveBlockSplitGrid(MySlimBlock testBlock)
		{
			return m_disconnectHelper.TryDisconnect(testBlock);
		}

		public MySlimBlock GetTargetedBlock(Vector3D position)
		{
			FixTargetCube(out Vector3I cube, Vector3D.Transform(position, base.PositionComp.WorldMatrixNormalizedInv) * GridSizeR);
			return GetCubeBlock(cube);
		}

		public MySlimBlock GetTargetedBlockLite(Vector3D position)
		{
			FixTargetCubeLite(out Vector3I cube, Vector3D.Transform(position, base.PositionComp.WorldMatrixNormalizedInv) * GridSizeR);
			return GetCubeBlock(cube);
		}

		[Event(null, 9986)]
		[Reliable]
		[Server]
		public static void TryCreateGrid_Implementation(MyCubeSize cubeSize, bool isStatic, MyPositionAndOrientation position, long inventoryEntityId, bool instantBuild)
		{
			bool flag = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value);
			MyDefinitionManager.Static.GetBaseBlockPrefabName(cubeSize, isStatic, MySession.Static.CreativeMode || (instantBuild && flag), out string prefabName);
			if (prefabName == null)
			{
				return;
			}
			MyObjectBuilder_CubeGrid[] gridPrefab = MyPrefabManager.Static.GetGridPrefab(prefabName);
			if (gridPrefab == null || gridPrefab.Length == 0)
			{
				return;
			}
			MyObjectBuilder_CubeGrid[] array = gridPrefab;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].PositionAndOrientation = position;
			}
			MyEntities.RemapObjectBuilderCollection(gridPrefab);
			if (!(instantBuild && flag))
			{
				if (MyEntities.TryGetEntityById(inventoryEntityId, out MyEntity entity) && entity != null)
				{
					MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory(entity);
					if (builderInventory != null)
					{
						MyGridClipboard.CalculateItemRequirements(gridPrefab, m_buildComponents);
						foreach (KeyValuePair<MyDefinitionId, int> totalMaterial in m_buildComponents.TotalMaterials)
						{
							builderInventory.RemoveItemsOfType(totalMaterial.Value, totalMaterial.Key);
						}
					}
				}
				else if (!flag && !MySession.Static.CreativeMode)
				{
					(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
					return;
				}
			}
			List<MyCubeGrid> list = new List<MyCubeGrid>();
			array = gridPrefab;
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in array)
			{
				MySandboxGame.Log.WriteLine("CreateCompressedMsg: Type: " + myObjectBuilder_CubeGrid.GetType().Name.ToString() + "  Name: " + myObjectBuilder_CubeGrid.Name + "  EntityID: " + myObjectBuilder_CubeGrid.EntityId.ToString("X8"));
				MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(myObjectBuilder_CubeGrid, fadeIn: false) as MyCubeGrid;
				if (myCubeGrid != null)
				{
					list.Add(myCubeGrid);
					if (instantBuild && flag)
					{
						ChangeOwnership(inventoryEntityId, myCubeGrid);
					}
					MySandboxGame.Log.WriteLine("Status: Exists(" + MyEntities.EntityExists(myObjectBuilder_CubeGrid.EntityId) + ") InScene(" + ((myObjectBuilder_CubeGrid.PersistentFlags & MyPersistentEntityFlags2.InScene) == MyPersistentEntityFlags2.InScene) + ")");
				}
			}
			AfterPaste(list, Vector3.Zero, detectDisconnects: false);
		}

		public void SendGridCloseRequest()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnGridClosedRequest, base.EntityId);
		}

		[Event(null, 10062)]
		[Reliable]
		[Client]
		private static void StationClosingDenied()
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MySpaceTexts.Economy_CantRemoveStation_Caption), messageText: MyTexts.Get(MySpaceTexts.Economy_CantRemoveStation_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: null, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		[Event(null, 10072)]
		[Reliable]
		[Server]
		private static void OnGridClosedRequest(long entityId)
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component != null && component.IsGridStation(entityId))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => StationClosingDenied, MyEventContext.Current.Sender);
				return;
			}
			MyLog.Default.WriteLineAndConsole("Closing grid request by user: " + MyEventContext.Current.Sender.Value);
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyEntities.TryGetEntityById(entityId, out MyEntity entity);
			if (entity == null)
			{
				return;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
				bool flag = false;
				bool flag2 = false;
				IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(num);
				if (myFaction != null)
				{
					flag2 = myFaction.IsLeader(num);
				}
				if (MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
				{
					flag = true;
				}
				else if (myCubeGrid.BigOwners.Count != 0)
				{
					foreach (long bigOwner in myCubeGrid.BigOwners)
					{
						if (bigOwner == num)
						{
							flag = true;
							break;
						}
						if (MySession.Static.Players.TryGetIdentity(bigOwner) != null && flag2)
						{
							IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(bigOwner);
							if (myFaction2 != null && myFaction.FactionId == myFaction2.FactionId)
							{
								flag = true;
								break;
							}
						}
					}
				}
				else
				{
					flag = true;
				}
				if (!flag)
				{
					return;
				}
			}
			MyLog.Default.Info($"OnGridClosedRequest removed entity '{entity.Name}:{entity.DisplayName}' with entity id '{entity.EntityId}'");
			if (!entity.MarkedForClose)
			{
				entity.Close();
			}
		}

		[Event(null, 10366)]
		[Reliable]
		[Server]
		public static void TryPasteGrid_Implementation(List<MyObjectBuilder_CubeGrid> entities, bool detectDisconnects, Vector3 objectVelocity, bool multiBlock, bool instantBuild, RelativeOffset offset)
		{
			MyLog.Default.WriteLineAndConsole("Pasting grid request by user: " + MyEventContext.Current.Sender.Value);
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsCopyPastingEnabledForUser(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			bool shouldRemoveScripts = !MySession.Static.IsUserScripter(MyEventContext.Current.Sender.Value);
			Vector3D? offset2 = null;
			if (offset.Use && offset.RelativeToEntity)
			{
				if (!MyEntityIdentifier.TryGetEntity(offset.SpawnerId, out VRage.ModAPI.IMyEntity entity))
				{
					return;
				}
				offset2 = (entity as MyEntity).WorldMatrix.Translation - offset.OriginalSpawnPoint;
			}
			MyEntities.RemapObjectBuilderCollection(entities);
			PasteGridData workData = new PasteGridData(entities, detectDisconnects, objectVelocity, multiBlock, instantBuild, shouldRemoveScripts, MyEventContext.Current.Sender, MyEventContext.Current.IsLocallyInvoked, offset2);
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.PrepareSwapData();
				MyEntityIdentifier.SwapPerThreadData();
			}
			Parallel.Start(TryPasteGrid_ImplementationInternal, OnPasteCompleted, workData);
			if (MySandboxGame.Config.SyncRendering)
			{
				MyEntityIdentifier.ClearSwapDataAndRestore();
			}
		}

		private static void TryPasteGrid_ImplementationInternal(WorkData workData)
		{
			PasteGridData pasteGridData = workData as PasteGridData;
			if (pasteGridData == null)
			{
				workData.FlagAsFailed();
			}
			else
			{
				pasteGridData.TryPasteGrid();
			}
		}

		private static void OnPasteCompleted(WorkData workData)
		{
			PasteGridData pasteGridData = workData as PasteGridData;
			if (pasteGridData == null)
			{
				workData.FlagAsFailed();
			}
			else
			{
				pasteGridData.Callback();
			}
		}

		[Event(null, 10429)]
		[Reliable]
		[Client]
		public static void ShowPasteFailedOperation()
		{
			MyHud.Notifications.Add(MyNotificationSingletons.PasteFailed);
		}

		[Event(null, 10435)]
		[Reliable]
		[Client]
		public static void SendHudNotificationAfterPaste()
		{
			MyHud.PopRotatingWheelVisible();
		}

		private static void ChangeOwnership(long inventoryEntityId, MyCubeGrid grid)
		{
			if (MyEntities.TryGetEntityById(inventoryEntityId, out MyEntity entity) && entity != null)
			{
				MyCharacter myCharacter = entity as MyCharacter;
				if (myCharacter != null)
				{
					grid.ChangeGridOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId, MyOwnershipShareModeEnum.Faction);
				}
			}
		}

		private static void AfterPaste(List<MyCubeGrid> grids, Vector3 objectVelocity, bool detectDisconnects)
		{
			foreach (MyCubeGrid grid in grids)
			{
				if (grid.IsStatic)
				{
					grid.TestDynamic = MyTestDynamicReason.GridCopied;
				}
				MyEntities.Add(grid);
				if (grid.Physics != null)
				{
					if (!grid.IsStatic)
					{
						grid.Physics.LinearVelocity = objectVelocity;
					}
					if (!grid.IsStatic && MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity.Physics != null && MySession.Static.ControlledEntity != null)
					{
						grid.Physics.AngularVelocity = MySession.Static.ControlledEntity.Entity.Physics.AngularVelocity;
					}
				}
				if (detectDisconnects)
				{
					grid.DetectDisconnectsAfterFrame();
				}
				if (grid.IsStatic)
				{
					foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
					{
						if (grid.DetectMerge(cubeBlock, null, null, newGrid: true) != null)
						{
							break;
						}
					}
				}
			}
			MatrixD tranform = grids[0].PositionComp.WorldMatrix;
			bool flag = MyCoordinateSystem.Static.IsLocalCoordSysExist(ref tranform, grids[0].GridSize);
			if (grids[0].GridSizeEnum == MyCubeSize.Large)
			{
				if (flag)
				{
					MyCoordinateSystem.Static.RegisterCubeGrid(grids[0]);
				}
				else
				{
					MyCoordinateSystem.Static.CreateCoordSys(grids[0], MyClipboardComponent.ClipboardDefinition.PastingSettings.StaticGridAlignToCenter, sync: true);
				}
			}
		}

		public void RecalculateGravity()
		{
			Vector3D worldPoint = (Physics == null || !(Physics.RigidBody != null)) ? base.PositionComp.GetPosition() : Physics.CenterOfMassWorld;
			m_gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldPoint);
		}

		public void ActivatePhysics()
		{
			if (Physics != null && Physics.Enabled)
			{
				Physics.RigidBody.Activate();
				if (Physics.RigidBody2 != null)
				{
					Physics.RigidBody2.Activate();
				}
			}
		}

		[Event(null, 10539)]
		[Reliable]
		[Broadcast]
		private void OnBonesReceived(int segmentsCount, List<byte> boneByteList)
		{
			byte[] data = boneByteList.ToArray();
			int dataIndex = 0;
			Vector3I cubeDirty = default(Vector3I);
			for (int i = 0; i < segmentsCount; i++)
			{
				Skeleton.DeserializePart(GridSize, data, ref dataIndex, out Vector3I minBone, out Vector3I maxBone);
				Vector3I cube = Vector3I.Zero;
				Vector3I cube2 = Vector3I.Zero;
				Skeleton.Wrap(ref cube, ref minBone);
				Skeleton.Wrap(ref cube2, ref maxBone);
				cube -= Vector3I.One;
				cube2 += Vector3I.One;
				cubeDirty.X = cube.X;
				while (cubeDirty.X <= cube2.X)
				{
					cubeDirty.Y = cube.Y;
					while (cubeDirty.Y <= cube2.Y)
					{
						cubeDirty.Z = cube.Z;
						while (cubeDirty.Z <= cube2.Z)
						{
							SetCubeDirty(cubeDirty);
							cubeDirty.Z++;
						}
						cubeDirty.Y++;
					}
					cubeDirty.X++;
				}
			}
		}

		[Event(null, 10573)]
		[Reliable]
		[Broadcast]
		private void OnBonesMultiplied(Vector3I blockLocation, float multiplier)
		{
			MySlimBlock cubeBlock = GetCubeBlock(blockLocation);
			if (cubeBlock != null)
			{
				MultiplyBlockSkeleton(cubeBlock, multiplier);
			}
		}

		public void SendReflectorState(MyMultipleEnabledEnum value)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.RelfectorStateRecived, value);
		}

		[Event(null, 10589)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Controlled)]
		[Broadcast]
		private void RelfectorStateRecived(MyMultipleEnabledEnum value)
		{
			GridSystems.ReflectorLightSystem.ReflectorStateChanged(value);
		}

		public void SendIntegrityChanged(MySlimBlock mySlimBlock, MyIntegrityChangeEnum integrityChangeType, long toolOwner)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BlockIntegrityChanged, mySlimBlock.Position, GetSubBlockId(mySlimBlock), mySlimBlock.BuildIntegrity, mySlimBlock.Integrity, integrityChangeType, toolOwner);
		}

		public void SendStockpileChanged(MySlimBlock mySlimBlock, List<MyStockpileItem> list)
		{
			if (list.Count > 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.BlockStockpileChanged, mySlimBlock.Position, GetSubBlockId(mySlimBlock), list);
			}
		}

		public void SendFractureComponentRepaired(MySlimBlock mySlimBlock, long toolOwner)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.FractureComponentRepaired, mySlimBlock.Position, GetSubBlockId(mySlimBlock), toolOwner);
		}

		private ushort GetSubBlockId(MySlimBlock slimBlock)
		{
			MySlimBlock cubeBlock = slimBlock.CubeGrid.GetCubeBlock(slimBlock.Position);
			MyCompoundCubeBlock myCompoundCubeBlock = null;
			if (cubeBlock != null)
			{
				myCompoundCubeBlock = (cubeBlock.FatBlock as MyCompoundCubeBlock);
			}
			if (myCompoundCubeBlock != null)
			{
				return myCompoundCubeBlock.GetBlockId(slimBlock) ?? 0;
			}
			return 0;
		}

		public void RequestFillStockpile(Vector3I blockPosition, MyInventory fromInventory)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnStockpileFillRequest, blockPosition, fromInventory.Owner.EntityId, fromInventory.InventoryIdx);
		}

		[Event(null, 10633)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnStockpileFillRequest(Vector3I blockPosition, long ownerEntityId, byte inventoryIndex)
		{
			MySlimBlock cubeBlock = GetCubeBlock(blockPosition);
			if (cubeBlock != null)
			{
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(ownerEntityId, out entity))
				{
					MyInventory inventory = ((entity != null && entity.HasInventory) ? entity : null).GetInventory(inventoryIndex);
					cubeBlock.MoveItemsToConstructionStockpile(inventory);
				}
			}
		}

		public void RequestSetToConstruction(Vector3I blockPosition, MyInventory fromInventory)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnSetToConstructionRequest, blockPosition, fromInventory.Owner.EntityId, fromInventory.InventoryIdx, MySession.Static.LocalPlayerId);
		}

		[Event(null, 10661)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnSetToConstructionRequest(Vector3I blockPosition, long ownerEntityId, byte inventoryIndex, long requestingPlayer)
		{
			MySlimBlock cubeBlock = GetCubeBlock(blockPosition);
			if (cubeBlock != null)
			{
				cubeBlock.SetToConstructionSite();
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(ownerEntityId, out entity))
				{
					MyInventoryBase inventory = ((entity != null && entity.HasInventory) ? entity : null).GetInventory(inventoryIndex);
					cubeBlock.MoveItemsToConstructionStockpile(inventory);
					cubeBlock.IncreaseMountLevel(MyWelder.WELDER_AMOUNT_PER_SECOND * 0.0166666675f, requestingPlayer);
				}
			}
		}

		public void ChangePowerProducerState(MyMultipleEnabledEnum enabledState, long playerId)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnPowerProducerStateRequest, enabledState, playerId);
		}

		[Event(null, 10692)]
		[Reliable]
		[Server]
		[Broadcast]
		private void OnPowerProducerStateRequest(MyMultipleEnabledEnum enabledState, long playerId)
		{
			GridSystems.SyncObject_PowerProducerStateChanged(enabledState, playerId);
		}

		public void RequestConversionToShip(Action result)
		{
			m_convertToShipResult = result;
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertedToShipRequest, MyTestDynamicReason.ConvertToShip);
		}

		public void RequestConversionToStation()
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertedToStationRequest);
		}

		[Event(null, 10710)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.BigOwnerSpaceMaster)]
		private void OnConvertedToShipRequest(MyTestDynamicReason reason)
		{
			if (!IsStatic || Physics == null || BlocksCount == 0 || ShouldBeStatic(this, reason))
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertToShipFailed, MyEventContext.Current.Sender);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnConvertToDynamic);
			}
		}

		[Event(null, 10723)]
		[Reliable]
		[Client]
		private void OnConvertToShipFailed()
		{
			if (m_convertToShipResult != null)
			{
				m_convertToShipResult();
			}
			m_convertToShipResult = null;
		}

		[Event(null, 10731)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.BigOwnerSpaceMaster)]
		public void OnConvertedToStationRequest()
		{
			if (!IsStatic && MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.ConvertToStation, 0L, MyEventContext.Current.Sender.Value))
			{
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.ConvertToStatic);
			}
		}

		public void ChangeOwnerRequest(MyCubeGrid grid, MyCubeBlock block, long playerId, MyOwnershipShareModeEnum shareMode)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeOwnerRequest, block.EntityId, playerId, shareMode);
		}

		[Event(null, 10750)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnChangeOwnerRequest(long blockId, long owner, MyOwnershipShareModeEnum shareMode)
		{
			MyCubeBlock entity = null;
			if (!MyEntities.TryGetEntityById(blockId, out entity))
			{
				return;
			}
			MyEntityOwnershipComponent myEntityOwnershipComponent = entity.Components.Get<MyEntityOwnershipComponent>();
			if (Sync.IsServer && entity.IDModule != null && (entity.IDModule.Owner == 0L || entity.IDModule.Owner == owner || owner == 0L))
			{
				OnChangeOwner(blockId, owner, shareMode);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeOwner, blockId, owner, shareMode);
				return;
			}
			if (Sync.IsServer && myEntityOwnershipComponent != null && (myEntityOwnershipComponent.OwnerId == 0L || myEntityOwnershipComponent.OwnerId == owner || owner == 0L))
			{
				OnChangeOwner(blockId, owner, shareMode);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeOwner, blockId, owner, shareMode);
				return;
			}
			bool flag = entity.BlockDefinition.ContainsComputer();
			if (entity.UseObjectsComponent != null)
			{
				flag = (flag || entity.UseObjectsComponent.GetDetectors("ownership").Count > 0);
			}
		}

		[Event(null, 10779)]
		[Reliable]
		[Broadcast]
		private void OnChangeOwner(long blockId, long owner, MyOwnershipShareModeEnum shareMode)
		{
			MyCubeBlock entity = null;
			if (MyEntities.TryGetEntityById(blockId, out entity))
			{
				entity.ChangeOwner(owner, shareMode);
			}
		}

		private void HandBrakeChanged()
		{
			GridSystems.WheelSystem.HandBrake = m_handBrakeSync;
		}

		[Event(null, 10794)]
		[Reliable]
		[Server]
		public void SetHandbrakeRequest(bool v)
		{
			m_handBrakeSync.Value = v;
		}

		internal void EnableDampingInternal(bool enableDampeners, bool updateProxy)
		{
			if (EntityThrustComponent == null || EntityThrustComponent.DampenersEnabled == enableDampeners)
			{
				return;
			}
			EntityThrustComponent.DampenersEnabled = enableDampeners;
			m_dampenersEnabled.Value = enableDampeners;
			if (Physics != null && Physics.RigidBody != null && !Physics.RigidBody.IsActive)
			{
				ActivatePhysics();
			}
			if (MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			MyCockpit myCockpit = MySession.Static.LocalHumanPlayer.Controller.ControlledEntity as MyCockpit;
			if (myCockpit != null && myCockpit.CubeGrid == this)
			{
				if (m_inertiaDampenersNotification == null)
				{
					m_inertiaDampenersNotification = new MyHudNotification();
				}
				m_inertiaDampenersNotification.Text = (EntityThrustComponent.DampenersEnabled ? MyCommonTexts.NotificationInertiaDampenersOn : MyCommonTexts.NotificationInertiaDampenersOff);
				MyHud.Notifications.Add(m_inertiaDampenersNotification);
				MyHud.ShipInfo.Reload();
				MyHud.SinkGroupInfo.Reload();
			}
		}

		private void DampenersEnabledChanged()
		{
			EnableDampingInternal(m_dampenersEnabled.Value, updateProxy: false);
		}

		public void ChangeGridOwner(long playerId, MyOwnershipShareModeEnum shareMode)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeGridOwner, playerId, shareMode);
			OnChangeGridOwner(playerId, shareMode);
		}

		[Event(null, 10841)]
		[Reliable]
		[Broadcast]
		private void OnChangeGridOwner(long playerId, MyOwnershipShareModeEnum shareMode)
		{
			foreach (MySlimBlock block in GetBlocks())
			{
				if (block.FatBlock != null && block.BlockDefinition.RatioEnoughForOwnership(block.BuildLevelRatio))
				{
					block.FatBlock.ChangeOwner(playerId, shareMode);
				}
			}
		}

		public void AnnounceRemoveSplit(List<MySlimBlock> blocks)
		{
			m_tmpPositionListSend.Clear();
			foreach (MySlimBlock block in blocks)
			{
				m_tmpPositionListSend.Add(block.Position);
			}
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnRemoveSplit, m_tmpPositionListSend);
		}

		[Event(null, 10863)]
		[Reliable]
		[Broadcast]
		private void OnRemoveSplit(List<Vector3I> removedBlocks)
		{
			m_tmpPositionListReceive.Clear();
			foreach (Vector3I removedBlock in removedBlocks)
			{
				MySlimBlock cubeBlock = GetCubeBlock(removedBlock);
				if (cubeBlock == null)
				{
					MySandboxGame.Log.WriteLine("Block was null when trying to remove a grid split. Desync?");
				}
				else
				{
					m_tmpBlockListReceive.Add(cubeBlock);
				}
			}
			RemoveSplit(this, m_tmpBlockListReceive, 0, m_tmpBlockListReceive.Count, sync: false);
			m_tmpBlockListReceive.Clear();
		}

		public void ChangeDisplayNameRequest(string displayName)
		{
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnChangeDisplayNameRequest, displayName);
		}

		[Event(null, 10890)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.BigOwner)]
		[Broadcast]
		private void OnChangeDisplayNameRequest(string displayName)
		{
			base.DisplayName = displayName;
			if (this.OnNameChanged != null)
			{
				this.OnNameChanged(this);
			}
		}

		public void ModifyGroup(MyBlockGroup group)
		{
			m_tmpBlockIdList.Clear();
			foreach (MyTerminalBlock block in group.Blocks)
			{
				m_tmpBlockIdList.Add(block.EntityId);
			}
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnModifyGroupSuccess, group.Name.ToString(), m_tmpBlockIdList);
		}

		[Event(null, 10908)]
		[Reliable]
		[Server(ValidationType.Access)]
		[BroadcastExcept]
		private void OnModifyGroupSuccess(string name, List<long> blocks)
		{
			if (blocks == null || blocks.Count == 0)
			{
				foreach (MyBlockGroup blockGroup in BlockGroups)
				{
					if (blockGroup.Name.ToString().Equals(name))
					{
						RemoveGroup(blockGroup);
						break;
					}
				}
				return;
			}
			MyBlockGroup myBlockGroup = new MyBlockGroup();
			myBlockGroup.Name.Clear().Append(name);
			foreach (long block in blocks)
			{
				MyTerminalBlock entity = null;
				if (MyEntities.TryGetEntityById(block, out entity))
				{
					myBlockGroup.Blocks.Add(entity);
				}
			}
			AddGroup(myBlockGroup);
		}

		public void RazeBlockInCompoundBlock(List<Tuple<Vector3I, ushort>> locationsAndIds)
		{
			ConvertToLocationIdentityList(locationsAndIds, m_tmpLocationsAndIdsSend);
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnRazeBlockInCompoundBlockRequest, m_tmpLocationsAndIdsSend);
		}

		[Event(null, 10942)]
		[Reliable]
		[Server]
		private void OnRazeBlockInCompoundBlockRequest(List<LocationIdentity> locationsAndIds)
		{
			OnRazeBlockInCompoundBlock(locationsAndIds);
			if (m_tmpLocationsAndIdsReceive.Count > 0)
			{
				ConvertToLocationIdentityList(m_tmpLocationsAndIdsReceive, m_tmpLocationsAndIdsSend);
				MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnRazeBlockInCompoundBlockSuccess, m_tmpLocationsAndIdsSend);
			}
		}

		[Event(null, 10955)]
		[Reliable]
		[Broadcast]
		private void OnRazeBlockInCompoundBlockSuccess(List<LocationIdentity> locationsAndIds)
		{
			OnRazeBlockInCompoundBlock(locationsAndIds);
		}

		private void OnRazeBlockInCompoundBlock(List<LocationIdentity> locationsAndIds)
		{
			m_tmpLocationsAndIdsReceive.Clear();
			RazeBlockInCompoundBlockSuccess(locationsAndIds, m_tmpLocationsAndIdsReceive);
		}

		private static void ConvertToLocationIdentityList(List<Tuple<Vector3I, ushort>> locationsAndIdsFrom, List<LocationIdentity> locationsAndIdsTo)
		{
			locationsAndIdsTo.Clear();
			locationsAndIdsTo.Capacity = locationsAndIdsFrom.Count;
			foreach (Tuple<Vector3I, ushort> item in locationsAndIdsFrom)
			{
				locationsAndIdsTo.Add(new LocationIdentity
				{
					Location = item.Item1,
					Id = item.Item2
				});
			}
		}

		public static void ChangeOwnersRequest(MyOwnershipShareModeEnum shareMode, List<MySingleOwnershipRequest> requests, long requestingPlayer)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChangeOwnersRequest, shareMode, requests, requestingPlayer);
		}

		[Event(null, 10982)]
		[Reliable]
		[Server(ValidationType.Access)]
		private static void OnChangeOwnersRequest(MyOwnershipShareModeEnum shareMode, List<MySingleOwnershipRequest> requests, long requestingPlayer)
		{
			MyCubeBlock entity = null;
			int num = 0;
			ulong num2 = MySession.Static.Players.TryGetSteamId(requestingPlayer);
			bool flag = false;
			if (MySession.Static.IsUserAdmin(num2))
			{
				if (num2 != 0L && MySession.Static.RemoteAdminSettings.TryGetValue(num2, out AdminSettingsEnum value) && value.HasFlag(AdminSettingsEnum.Untargetable))
				{
					num = requests.Count;
				}
				AdminSettingsEnum? adminSettingsEnum = null;
				if (num2 == Sync.MyId)
				{
					adminSettingsEnum = MySession.Static.AdminSettings;
				}
				else if (MySession.Static.RemoteAdminSettings.ContainsKey(num2))
				{
					adminSettingsEnum = MySession.Static.RemoteAdminSettings[num2];
				}
				if (((int?)adminSettingsEnum & 4) != 0)
				{
					flag = true;
				}
			}
			while (num < requests.Count)
			{
				MySingleOwnershipRequest mySingleOwnershipRequest = requests[num];
				if (MyEntities.TryGetEntityById(mySingleOwnershipRequest.BlockId, out entity))
				{
					MyEntityOwnershipComponent myEntityOwnershipComponent = entity.Components.Get<MyEntityOwnershipComponent>();
					if (Sync.IsServer && flag)
					{
						num++;
					}
					else if (Sync.IsServer && entity.IDModule != null && (entity.IDModule.Owner == 0L || entity.IDModule.Owner == requestingPlayer || mySingleOwnershipRequest.Owner == 0L))
					{
						num++;
					}
					else if (Sync.IsServer && myEntityOwnershipComponent != null && (myEntityOwnershipComponent.OwnerId == 0L || myEntityOwnershipComponent.OwnerId == requestingPlayer || mySingleOwnershipRequest.Owner == 0L))
					{
						num++;
					}
					else
					{
						requests.RemoveAtFast(num);
					}
				}
				else
				{
					num++;
				}
			}
			if (requests.Count > 0)
			{
				OnChangeOwnersSuccess(shareMode, requests);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnChangeOwnersSuccess, shareMode, requests);
			}
		}

		[Event(null, 11048)]
		[Reliable]
		[Broadcast]
		private static void OnChangeOwnersSuccess(MyOwnershipShareModeEnum shareMode, List<MySingleOwnershipRequest> requests)
		{
			foreach (MySingleOwnershipRequest request in requests)
			{
				MyCubeBlock entity = null;
				if (MyEntities.TryGetEntityById(request.BlockId, out entity))
				{
					entity.ChangeOwner(request.Owner, shareMode);
				}
			}
		}

		public override void SerializeControls(BitStream stream)
		{
			MyShipController myShipController = null;
			if (!IsStatic && base.InScene)
			{
				myShipController = GridSystems.ControlSystem.GetShipController();
			}
			if (myShipController != null)
			{
				stream.WriteBool(value: true);
				myShipController.GetNetState().Serialize(stream);
			}
			else
			{
				stream.WriteBool(value: false);
			}
		}

		public override void DeserializeControls(BitStream stream, bool outOfOrder)
		{
			if (stream.ReadBool())
			{
				MyGridClientState lastNetState = new MyGridClientState(stream);
				if (!outOfOrder)
				{
					m_lastNetState = lastNetState;
				}
				MyShipController shipController = GridSystems.ControlSystem.GetShipController();
				if (shipController != null && !shipController.ControllerInfo.IsLocallyControlled())
				{
					shipController.SetNetState(m_lastNetState);
				}
			}
			else
			{
				m_lastNetState.Valid = false;
			}
		}

		public override void ResetControls()
		{
			m_lastNetState.Valid = false;
			MyShipController shipController = GridSystems.ControlSystem.GetShipController();
			if (shipController != null && !shipController.ControllerInfo.IsLocallyControlled())
			{
				shipController.ClearMovementControl();
			}
		}

		public override void ApplyLastControls()
		{
			if (m_lastNetState.Valid)
			{
				MyShipController shipController = GridSystems.ControlSystem.GetShipController();
				if (shipController != null && !shipController.ControllerInfo.IsLocallyControlled())
				{
					shipController.SetNetState(m_lastNetState);
				}
			}
		}

		public void TargetingAddId(long id)
		{
			if (!m_targetingList.Contains(id))
			{
				m_targetingList.Add(id);
			}
			m_usesTargetingList = (m_targetingList.Count > 0 || m_targetingListIsWhitelist);
		}

		public void TargetingRemoveId(long id)
		{
			if (m_targetingList.Contains(id))
			{
				m_targetingList.Remove(id);
			}
			m_usesTargetingList = (m_targetingList.Count > 0 || m_targetingListIsWhitelist);
		}

		public void TargetingSetWhitelist(bool whitelist)
		{
			m_targetingListIsWhitelist = whitelist;
			m_usesTargetingList = (m_targetingList.Count > 0 || m_targetingListIsWhitelist);
		}

		public bool TargetingCanAttackGrid(long id)
		{
			if (m_targetingListIsWhitelist)
			{
				return m_targetingList.Contains(id);
			}
			return !m_targetingList.Contains(id);
		}

		public void HierarchyUpdated(MyCubeGrid root)
		{
			MyGridPhysics physics = Physics;
			if (physics != null)
			{
				if (this != root)
				{
					physics.SetRelaxedRigidBodyMaxVelocities();
				}
				else
				{
					physics.SetDefaultRigidBodyMaxVelocities();
				}
			}
			this.OnHierarchyUpdated.InvokeIfNotNull(this);
		}

		public void RegisterInventory(MyCubeBlock block)
		{
			m_inventoryBlocks.Add(block);
			m_inventoryMassDirty = true;
		}

		public void UnregisterInventory(MyCubeBlock block)
		{
			m_inventoryBlocks.Remove(block);
			m_inventoryMassDirty = true;
		}

		public void RegisterUnsafeBlock(MyCubeBlock block)
		{
			if (m_unsafeBlocks.Add(block))
			{
				if (m_unsafeBlocks.Count == 1)
				{
					MyUnsafeGridsSessionComponent.RegisterGrid(this);
				}
				else
				{
					MyUnsafeGridsSessionComponent.OnGridChanged(this);
				}
			}
		}

		public void UnregisterUnsafeBlock(MyCubeBlock block)
		{
			if (m_unsafeBlocks.Remove(block))
			{
				if (m_unsafeBlocks.Count == 0)
				{
					MyUnsafeGridsSessionComponent.UnregisterGrid(this);
				}
				else
				{
					MyUnsafeGridsSessionComponent.OnGridChanged(this);
				}
			}
		}

		public void RegisterDecoy(MyDecoy block)
		{
			if (m_decoys == null)
			{
				m_decoys = new HashSet<MyDecoy>();
			}
			m_decoys.Add(block);
		}

		public void UnregisterDecoy(MyDecoy block)
		{
			m_decoys.Remove(block);
		}

		public void RegisterOccupiedBlock(MyCockpit block)
		{
			m_occupiedBlocks.Add(block);
		}

		public void UnregisterOccupiedBlock(MyCockpit block)
		{
			m_occupiedBlocks.Remove(block);
		}

		private void OnContactPointChanged()
		{
			if (Physics == null || base.Closed || base.MarkedForClose || Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			ContactPointData value = m_contactPoint.Value;
			MyEntity entity = null;
			if (MyEntities.TryGetEntityById(value.EntityId, out entity) && entity.Physics != null)
			{
				Vector3D worldPosition = value.LocalPosition + base.PositionComp.WorldMatrix.Translation;
				if ((value.ContactPointType & ContactPointData.ContactPointDataTypes.Sounds) != 0)
				{
					MyAudioComponent.PlayContactSoundInternal(this, entity, worldPosition, value.Normal, value.SeparatingSpeed);
				}
				if ((value.ContactPointType & ContactPointData.ContactPointDataTypes.AnyParticle) != 0)
				{
					Physics.PlayCollisionParticlesInternal(entity, ref worldPosition, ref value.Normal, ref value.SeparatingVelocity, value.SeparatingSpeed, value.Impulse, value.ContactPointType);
				}
			}
		}

		public void UpdateParticleContactPoint(long entityId, ref Vector3 relativePosition, ref Vector3 normal, ref Vector3 separatingVelocity, float separatingSpeed, float impulse, ContactPointData.ContactPointDataTypes flags)
		{
			if (flags != 0)
			{
				ContactPointData contactPointData = default(ContactPointData);
				contactPointData.EntityId = entityId;
				contactPointData.LocalPosition = relativePosition;
				contactPointData.Normal = normal;
				contactPointData.ContactPointType = flags;
				contactPointData.SeparatingVelocity = separatingVelocity;
				contactPointData.SeparatingSpeed = separatingSpeed;
				contactPointData.Impulse = impulse;
				ContactPointData localValue = contactPointData;
				m_contactPoint.SetLocalValue(localValue);
			}
		}

		public void MarkAsTrash()
		{
			m_markedAsTrash.Value = true;
		}

		private void MarkedAsTrashChanged()
		{
			if (MarkedAsTrash)
			{
				MarkForDraw();
				MarkForUpdate();
				m_trashHighlightCounter = TRASH_HIGHLIGHT;
			}
		}

		public void LogHierarchy()
		{
			OnLogHierarchy();
			MyMultiplayer.RaiseEvent(this, (MyCubeGrid x) => x.OnLogHierarchy);
		}

		[Event(null, 11378)]
		[Reliable]
		[Server]
		public void OnLogHierarchy()
		{
			MyGridPhysicalHierarchy.Static.Log(MyGridPhysicalHierarchy.Static.GetRoot(this));
		}

		[Event(null, 11385)]
		[Reliable]
		[Server]
		[Broadcast]
		public static void DepressurizeEffect(long gridId, Vector3I from, Vector3I to)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				DepressurizeEffect_Implementation(gridId, from, to);
			}, "CubeGrid - DepressurizeEffect");
		}

		public static void DepressurizeEffect_Implementation(long gridId, Vector3I from, Vector3I to)
		{
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(gridId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyGridGasSystem.AddDepressurizationEffects(myCubeGrid, from, to);
			}
		}

		public MyCubeGrid MergeGrid_MergeBlock(MyCubeGrid gridToMerge, Vector3I gridOffset, bool checkMergeOrder = true)
		{
			if (checkMergeOrder && !ShouldBeMergedToThis(gridToMerge))
			{
				return null;
			}
			MatrixI transform = CalculateMergeTransform(gridToMerge, gridOffset);
			MyMultiplayer.RaiseBlockingEvent(this, gridToMerge, (Func<MyCubeGrid, Action<long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction>>)((MyCubeGrid x) => x.MergeGrid_MergeBlockClient), gridToMerge.EntityId, (SerializableVector3I)transform.Translation, transform.Forward, transform.Up, default(EndpointId));
			return MergeGridInternal(gridToMerge, ref transform);
		}

		private bool ShouldBeMergedToThis(MyCubeGrid gridToMerge)
		{
			bool flag = IsRooted(this);
			bool flag2 = IsRooted(gridToMerge);
			if (flag && !flag2)
			{
				return true;
			}
			if (flag2 && !flag)
			{
				return false;
			}
			return BlocksCount > gridToMerge.BlocksCount;
		}

		private static bool IsRooted(MyCubeGrid grid)
		{
			if (grid.IsStatic)
			{
				return true;
			}
			MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(grid);
			if (group == null)
			{
				return false;
			}
			foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
			{
				if (MyFixedGrids.IsRooted(node.NodeData))
				{
					return true;
				}
			}
			return false;
		}

		[Event(null, 94)]
		[Reliable]
		[Broadcast]
		[Blocking]
		private void MergeGrid_MergeClient(long gridId, SerializableVector3I gridOffset, Base6Directions.Direction gridForward, Base6Directions.Direction gridUp, Vector3I mergingBlockPos)
		{
			MyCubeGrid entity = null;
			if (MyEntities.TryGetEntityById(gridId, out entity))
			{
				MatrixI transform = new MatrixI(gridOffset, gridForward, gridUp);
				MyCubeGrid myCubeGrid = MergeGridInternal(entity, ref transform);
				MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(mergingBlockPos);
				foreach (IMyBlockAdditionalModelGenerator additionalModelGenerator in myCubeGrid.AdditionalModelGenerators)
				{
					additionalModelGenerator.BlockAddedToMergedGrid(cubeBlock);
				}
			}
		}

		[Event(null, 113)]
		[Reliable]
		[Broadcast]
		[Blocking]
		private void MergeGrid_MergeBlockClient(long gridId, SerializableVector3I gridOffset, Base6Directions.Direction gridForward, Base6Directions.Direction gridUp)
		{
			MyCubeGrid entity = null;
			if (MyEntities.TryGetEntityById(gridId, out entity))
			{
				MatrixI transform = new MatrixI(gridOffset, gridForward, gridUp);
				MergeGridInternal(entity, ref transform);
			}
		}

		private MyCubeGrid MergeGrid_Static(MyCubeGrid gridToMerge, Vector3I gridOffset, MySlimBlock triggeringMergeBlock)
		{
			MatrixI transform = CalculateMergeTransform(gridToMerge, gridOffset);
			Vector3I vector3I = triggeringMergeBlock.Position;
			if (triggeringMergeBlock.CubeGrid != this)
			{
				vector3I = Vector3I.Transform(vector3I, transform);
			}
			MyMultiplayer.RaiseBlockingEvent(this, gridToMerge, (Func<MyCubeGrid, Action<long, SerializableVector3I, Base6Directions.Direction, Base6Directions.Direction, Vector3I>>)((MyCubeGrid x) => x.MergeGrid_MergeClient), gridToMerge.EntityId, (SerializableVector3I)transform.Translation, transform.Forward, transform.Up, vector3I, default(EndpointId));
			MyCubeGrid myCubeGrid = MergeGridInternal(gridToMerge, ref transform);
			foreach (IMyBlockAdditionalModelGenerator additionalModelGenerator in myCubeGrid.AdditionalModelGenerators)
			{
				additionalModelGenerator.BlockAddedToMergedGrid(triggeringMergeBlock);
			}
			return myCubeGrid;
		}

		private MyCubeGrid MergeGridInternal(MyCubeGrid gridToMerge, ref MatrixI transform, bool disableBlockGenerators = true)
		{
			if (MyCubeGridSmallToLargeConnection.Static != null)
			{
				MyCubeGridSmallToLargeConnection.Static.BeforeGridMerge_SmallToLargeGridConnectivity(this, gridToMerge);
			}
			MyRenderComponentCubeGrid tmpRenderComponent = gridToMerge.Render;
			tmpRenderComponent.DeferRenderRelease = true;
			Matrix transformMatrix = transform.GetFloatMatrix();
			transformMatrix.Translation *= GridSize;
			Action<MatrixD> updateMergingComponentWM = delegate(MatrixD matrix)
			{
				tmpRenderComponent.UpdateRenderObjectMatrices(transformMatrix * matrix);
			};
			Action releaseRenderOldRenderComponent = null;
			releaseRenderOldRenderComponent = delegate
			{
				tmpRenderComponent.DeferRenderRelease = false;
				m_updateMergingGrids = (Action<MatrixD>)Delegate.Remove(m_updateMergingGrids, updateMergingComponentWM);
				m_pendingGridReleases = (Action)Delegate.Remove(m_pendingGridReleases, releaseRenderOldRenderComponent);
			};
			m_updateMergingGrids = (Action<MatrixD>)Delegate.Combine(m_updateMergingGrids, updateMergingComponentWM);
			m_pendingGridReleases = (Action)Delegate.Combine(m_pendingGridReleases, releaseRenderOldRenderComponent);
			MarkForUpdate();
			MoveBlocksAndClose(gridToMerge, this, transform, disableBlockGenerators);
			UpdateGridAABB();
			if (Physics != null)
			{
				UpdatePhysicsShape();
			}
			if (MyCubeGridSmallToLargeConnection.Static != null)
			{
				MyCubeGridSmallToLargeConnection.Static.AfterGridMerge_SmallToLargeGridConnectivity(this);
			}
			updateMergingComponentWM(base.WorldMatrix);
			return this;
		}

		private static void MoveBlocksAndClose(MyCubeGrid from, MyCubeGrid to, MatrixI transform, bool disableBlockGenerators = true)
		{
			from.MarkedForClose = true;
			to.IsBlockTrasferInProgress = true;
			from.IsBlockTrasferInProgress = true;
			try
			{
				if (disableBlockGenerators)
				{
					from.EnableGenerators(enable: false, fromServer: true);
					to.EnableGenerators(enable: false, fromServer: true);
				}
				MyEntities.Remove(from);
				MyBlockGroup[] array = from.BlockGroups.ToArray();
				foreach (MyBlockGroup group in array)
				{
					to.AddGroup(group);
				}
				from.BlockGroups.Clear();
				from.UnregisterBlocksBeforeClose();
				foreach (MySlimBlock cubeBlock in from.m_cubeBlocks)
				{
					if (cubeBlock.FatBlock != null)
					{
						from.Hierarchy.RemoveChild(cubeBlock.FatBlock);
					}
					cubeBlock.RemoveNeighbours();
					cubeBlock.RemoveAuthorship();
				}
				if (from.Physics != null)
				{
					from.Physics.Close();
					from.Physics = null;
					from.RaisePhysicsChanged();
				}
				foreach (MySlimBlock cubeBlock2 in from.m_cubeBlocks)
				{
					cubeBlock2.Transform(ref transform);
					to.AddBlockInternal(cubeBlock2);
				}
				from.Skeleton.CopyTo(to.Skeleton, transform, to);
				if (disableBlockGenerators)
				{
					from.EnableGenerators(enable: true, fromServer: true);
					to.EnableGenerators(enable: true, fromServer: true);
				}
				from.m_blocksForDraw.Clear();
				from.m_cubeBlocks.Clear();
				from.m_fatBlocks.Clear();
				from.m_cubes.Clear();
				from.MarkedForClose = false;
				if (Sync.IsServer)
				{
					from.Close();
				}
			}
			finally
			{
				to.IsBlockTrasferInProgress = false;
				from.IsBlockTrasferInProgress = false;
			}
		}

		private void UpdateMergingGrids()
		{
			if (m_updateMergingGrids != null)
			{
				m_updateMergingGrids(base.WorldMatrix);
			}
		}

		private void ReleaseMerginGrids()
		{
			if (m_pendingGridReleases != null)
			{
				m_pendingGridReleases();
			}
		}

		private bool CanMoveBlocksFrom(MyCubeGrid grid, Vector3I blockOffset)
		{
			try
			{
				MatrixI transformation = CalculateMergeTransform(grid, blockOffset);
				foreach (KeyValuePair<Vector3I, MyCube> cube in grid.m_cubes)
				{
					Vector3I key = Vector3I.Transform(cube.Key, transformation);
					if (m_cubes.ContainsKey(key))
					{
						return false;
					}
				}
				return true;
			}
			finally
			{
			}
		}

		public static void Preload()
		{
		}

		public static void GetCubeParts(MyCubeBlockDefinition block, Vector3I inputPosition, Matrix rotation, float gridSize, List<string> outModels, List<MatrixD> outLocalMatrices, List<Vector3> outLocalNormals, List<Vector4UByte> outPatternOffsets, bool topologyCheck)
		{
			outModels.Clear();
			outLocalMatrices.Clear();
			outLocalNormals.Clear();
			outPatternOffsets.Clear();
			if (block.CubeDefinition == null)
			{
				return;
			}
			if (topologyCheck)
			{
				Base6Directions.Direction direction = Base6Directions.GetDirection(Vector3I.Round(rotation.Forward));
				Base6Directions.Direction direction2 = Base6Directions.GetDirection(Vector3I.Round(rotation.Up));
				MyCubeGridDefinitions.GetTopologyUniqueOrientation(block.CubeDefinition.CubeTopology, new MyBlockOrientation(direction, direction2)).GetMatrix(out rotation);
			}
			MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(block);
			int num = cubeTiles.Length;
			int num2 = 0;
			int num3 = 32768;
			float epsilon = 0.01f;
			for (int i = 0; i < num; i++)
			{
				MyTileDefinition myTileDefinition = cubeTiles[num2 + i];
				MatrixD item = (MatrixD)myTileDefinition.LocalMatrix * rotation;
				Vector3 vector = Vector3.Transform(myTileDefinition.Normal, rotation.GetOrientation());
				if (topologyCheck && myTileDefinition.Id != MyStringId.NullOrEmpty)
				{
					MyCubeGridDefinitions.TileGridOrientations.TryGetValue(myTileDefinition.Id, out Dictionary<Vector3I, MyTileDefinition> value);
					if (value.TryGetValue(new Vector3I(Vector3.Sign(vector)), out MyTileDefinition value2))
					{
						item = value2.LocalMatrix;
					}
				}
				Vector3I vector3I = inputPosition;
				if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base && myTileDefinition.Id == MyStringId.NullOrEmpty)
				{
					Vector3I vector3I2 = new Vector3I(-Vector3.Sign(vector.MaxAbsComponent()));
					vector3I += vector3I2;
				}
				string text = block.CubeDefinition.Model[i];
				Vector2I vector2I = block.CubeDefinition.PatternSize[i];
				Vector2I vector2I2 = block.CubeDefinition.ScaleTile[i];
				int num4 = (int)MyModels.GetModelOnlyData(text).PatternScale;
				vector2I = new Vector2I(vector2I.X * num4, vector2I.Y * num4);
				int num5 = 0;
				int num6 = 0;
				float num7 = Vector3.Dot(Vector3.UnitY, vector);
				float num8 = Vector3.Dot(Vector3.UnitX, vector);
				float num9 = Vector3.Dot(Vector3.UnitZ, vector);
				if (MyUtils.IsZero(Math.Abs(num7) - 1f, epsilon))
				{
					int num10 = (vector3I.X + num3) / vector2I.Y;
					int num11 = MyMath.Mod(num10 + (int)((double)num10 * Math.Sin((float)num10 * 10f)), vector2I.X);
					num5 = MyMath.Mod(vector3I.Z + vector3I.Y + num11 + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.X + num3, vector2I.Y);
					if (Math.Sign(num7) == 1)
					{
						num6 = vector2I.Y - 1 - num6;
					}
				}
				else if (MyUtils.IsZero(Math.Abs(num8) - 1f, epsilon))
				{
					int num12 = (vector3I.Z + num3) / vector2I.Y;
					int num13 = MyMath.Mod(num12 + (int)((double)num12 * Math.Sin((float)num12 * 10f)), vector2I.X);
					num5 = MyMath.Mod(vector3I.X + vector3I.Y + num13 + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.Z + num3, vector2I.Y);
					if (Math.Sign(num8) == 1)
					{
						num6 = vector2I.Y - 1 - num6;
					}
				}
				else if (MyUtils.IsZero(Math.Abs(num9) - 1f, epsilon))
				{
					int num14 = (vector3I.Y + num3) / vector2I.Y;
					int num15 = MyMath.Mod(num14 + (int)((double)num14 * Math.Sin((float)num14 * 10f)), vector2I.X);
					num5 = MyMath.Mod(vector3I.X + num15 + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.Y + num3, vector2I.Y);
					if (Math.Sign(num9) == 1)
					{
						num5 = vector2I.X - 1 - num5;
					}
				}
				else if (MyUtils.IsZero(num8, epsilon))
				{
					num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.Z * vector2I2.Y + num3, vector2I.Y);
					if (Math.Sign(num9) == -1)
					{
						if (Math.Sign(num7) == 1)
						{
							if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
							{
								if (num9 < -0.5f)
								{
									num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
									num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								}
								else
								{
									num6 = vector2I.Y - 1 - num6;
									num5 = vector2I.X - 1 - num5;
								}
							}
							else
							{
								num6 = vector2I.Y - 1 - num6;
								num5 = vector2I.X - 1 - num5;
							}
						}
						else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if ((double)num9 < -0.5)
							{
								num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
								num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								num5 = vector2I.X - 1 - num5;
								num6 = vector2I.Y - 1 - num6;
							}
							else
							{
								num6 = vector2I.Y - 1 - num6;
							}
						}
						else
						{
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else if (Math.Sign(num7) == -1)
					{
						if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if (num9 > 0.5f)
							{
								num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
								num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								num6 = vector2I.Y - 1 - num6;
							}
							else
							{
								num5 = vector2I.X - 1 - num5;
							}
						}
						else
						{
							num5 = vector2I.X - 1 - num5;
						}
					}
					else if ((block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip) && !((double)num7 > 0.5))
					{
						num5 = MyMath.Mod(vector3I.X * vector2I2.X + num3, vector2I.X);
						num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
						num5 = vector2I.X - 1 - num5;
					}
				}
				else if (MyUtils.IsZero(num9, epsilon))
				{
					num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.X * vector2I2.Y + num3, vector2I.Y);
					if (Math.Sign(num8) == 1)
					{
						if (Math.Sign(num7) == 1)
						{
							if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
							{
								if (num8 > 0.5f)
								{
									num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
									num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								}
								else
								{
									num5 = vector2I.X - 1 - num5;
								}
							}
							else
							{
								num5 = vector2I.X - 1 - num5;
							}
						}
						else if ((block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip) && !(num7 < -0.5f))
						{
							num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
							num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
							num5 = vector2I.X - 1 - num5;
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else if (Math.Sign(num7) == 1)
					{
						if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if (num7 > 0.5f)
							{
								num6 = vector2I.Y - 1 - num6;
							}
							else
							{
								num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
								num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
								num5 = vector2I.X - 1 - num5;
							}
						}
						else
						{
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
					{
						if (num7 < -0.5f)
						{
							num5 = vector2I.X - 1 - num5;
							num6 = vector2I.Y - 1 - num6;
						}
						else
						{
							num5 = MyMath.Mod(vector3I.Z * vector2I2.X + num3, vector2I.X);
							num6 = MyMath.Mod(vector3I.Y * vector2I2.Y + num3, vector2I.Y);
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else
					{
						num5 = vector2I.X - 1 - num5;
						num6 = vector2I.Y - 1 - num6;
					}
				}
				else if (MyUtils.IsZero(num7, epsilon))
				{
					num5 = MyMath.Mod(vector3I.Y * vector2I2.X + num3, vector2I.X);
					num6 = MyMath.Mod(vector3I.Z * vector2I2.Y + num3, vector2I.Y);
					if (Math.Sign(num9) == -1)
					{
						if (Math.Sign(num8) == 1)
						{
							if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
							{
								if (num9 < -0.5f)
								{
									num5 = vector2I.X - 1 - num5;
									num6 = vector2I.Y - 1 - num6;
								}
								else
								{
									num6 = vector2I.Y - 1 - num6;
								}
							}
							else
							{
								num5 = vector2I.X - 1 - num5;
							}
						}
						else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if ((double)num9 < -0.5)
							{
								num6 = vector2I.Y - 1 - num6;
							}
							else
							{
								num5 = vector2I.X - 1 - num5;
								num6 = vector2I.Y - 1 - num6;
							}
						}
					}
					else if (Math.Sign(num8) == 1)
					{
						if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
						{
							if (num8 > 0.5f)
							{
								num5 = vector2I.X - 1 - num5;
							}
							else
							{
								num5 = MyMath.Mod(vector3I.Y * vector2I2.X + num3, vector2I.X);
								num6 = MyMath.Mod(vector3I.X * vector2I2.Y + num3, vector2I.Y);
							}
						}
						else
						{
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else if (block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Base || block.CubeDefinition.CubeTopology == MyCubeTopology.Slope2Tip)
					{
						if (!((double)num8 < -0.5))
						{
							num5 = MyMath.Mod(vector3I.Y * vector2I2.X + num3, vector2I.X);
							num6 = MyMath.Mod(vector3I.X * vector2I2.Y + num3, vector2I.Y);
							num5 = vector2I.X - 1 - num5;
							num6 = vector2I.Y - 1 - num6;
						}
					}
					else
					{
						num5 = vector2I.X - 1 - num5;
						num6 = vector2I.Y - 1 - num6;
					}
				}
				item.Translation = inputPosition * gridSize;
				if (myTileDefinition.DontOffsetTexture)
				{
					num5 = 0;
					num6 = 0;
				}
				outPatternOffsets.Add(new Vector4UByte((byte)num5, (byte)num6, (byte)vector2I.X, (byte)vector2I.Y));
				outModels.Add(text);
				outLocalMatrices.Add(item);
				outLocalNormals.Add(vector);
			}
		}

		public static void CheckAreaConnectivity(MyCubeGrid grid, ref MyBlockBuildArea area, List<Vector3UByte> validOffsets, HashSet<Vector3UByte> resultFailList)
		{
			try
			{
				MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(area.DefinitionId);
				if (cubeBlockDefinition != null)
				{
					Quaternion rotation = Base6Directions.GetOrientation(area.OrientationForward, area.OrientationUp);
					Vector3I b = area.StepDelta;
					MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = cubeBlockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
					for (int num = validOffsets.Count - 1; num >= 0; num--)
					{
						Vector3I position = area.PosInGrid + validOffsets[num] * b;
						if (CheckConnectivity(grid, cubeBlockDefinition, buildProgressModelMountPoints, ref rotation, ref position))
						{
							m_tmpAreaMountpointPass.Add(validOffsets[num]);
							validOffsets.RemoveAtFast(num);
						}
					}
					m_areaOverlapTest.Initialize(ref area, cubeBlockDefinition);
					foreach (Vector3UByte item in m_tmpAreaMountpointPass)
					{
						m_areaOverlapTest.AddBlock(item);
					}
					int num2 = int.MaxValue;
					while (validOffsets.Count > 0 && validOffsets.Count < num2)
					{
						num2 = validOffsets.Count;
						for (int num3 = validOffsets.Count - 1; num3 >= 0; num3--)
						{
							Vector3I position2 = area.PosInGrid + validOffsets[num3] * b;
							if (CheckConnectivity(m_areaOverlapTest, cubeBlockDefinition, buildProgressModelMountPoints, ref rotation, ref position2))
							{
								m_tmpAreaMountpointPass.Add(validOffsets[num3]);
								m_areaOverlapTest.AddBlock(validOffsets[num3]);
								validOffsets.RemoveAtFast(num3);
							}
						}
					}
					foreach (Vector3UByte validOffset in validOffsets)
					{
						resultFailList.Add(validOffset);
					}
					validOffsets.Clear();
					validOffsets.AddRange(m_tmpAreaMountpointPass);
				}
			}
			finally
			{
				m_tmpAreaMountpointPass.Clear();
			}
		}

		public static bool CheckMergeConnectivity(MyCubeGrid hitGrid, MyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			MatrixI transformation = hitGrid.CalculateMergeTransform(gridToMerge, gridOffset);
			transformation.GetBlockOrientation().GetQuaternion(out Quaternion result);
			foreach (MySlimBlock block in gridToMerge.GetBlocks())
			{
				Vector3I position = Vector3I.Transform(block.Position, transformation);
				block.Orientation.GetQuaternion(out Quaternion result2);
				result2 = result * result2;
				MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
				if (CheckConnectivity(hitGrid, block.BlockDefinition, buildProgressModelMountPoints, ref result2, ref position))
				{
					return true;
				}
			}
			return false;
		}

		public static bool CheckConnectivity(IMyGridConnectivityTest grid, MyCubeBlockDefinition def, MyCubeBlockDefinition.MountPoint[] mountPoints, ref Quaternion rotation, ref Vector3I position)
		{
			try
			{
				if (mountPoints == null)
				{
					return false;
				}
				Vector3I value = def.Center;
				Vector3I value2 = def.Size;
				Vector3I.Transform(ref value, ref rotation, out Vector3I _);
				Vector3I.Transform(ref value2, ref rotation, out Vector3I _);
				for (int i = 0; i < mountPoints.Length; i++)
				{
					MyCubeBlockDefinition.MountPoint thisMountPoint = mountPoints[i];
					Vector3 value3 = thisMountPoint.Start - value;
					Vector3 value4 = thisMountPoint.End - value;
					if (MyFakes.ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK)
					{
						Vector3 vector = Vector3.Min(thisMountPoint.Start, thisMountPoint.End);
						Vector3 vector2 = Vector3.Max(thisMountPoint.Start, thisMountPoint.End);
						Vector3I b = Vector3I.One - Vector3I.Abs(thisMountPoint.Normal);
						Vector3I a = Vector3I.One - b;
						Vector3 value5 = a * vector + Vector3.Clamp(vector, Vector3.Zero, value2) * b + 0.001f * b;
						Vector3 value6 = a * vector2 + Vector3.Clamp(vector2, Vector3.Zero, value2) * b - 0.001f * b;
						value3 = value5 - value;
						value4 = value6 - value;
					}
					Vector3I value7 = Vector3I.Floor(value3);
					Vector3I value8 = Vector3I.Floor(value4);
					Vector3.Transform(ref value3, ref rotation, out Vector3 result3);
					Vector3.Transform(ref value4, ref rotation, out Vector3 result4);
					Vector3I.Transform(ref value7, ref rotation, out Vector3I result5);
					Vector3I.Transform(ref value8, ref rotation, out Vector3I result6);
					Vector3I b2 = Vector3I.Floor(result3);
					Vector3I b3 = Vector3I.Floor(result4);
					Vector3I value9 = result5 - b2;
					Vector3I value10 = result6 - b3;
					result3 += (Vector3)value9;
					result4 += (Vector3)value10;
					Vector3 value11 = position + result3;
					Vector3 value12 = position + result4;
					m_cacheNeighborBlocks.Clear();
					Vector3 vector3 = Vector3.Min(value11, value12);
					Vector3 vector4 = Vector3.Max(value11, value12);
					Vector3I otherBlockMinPos = Vector3I.Floor(vector3);
					Vector3I otherBlockMaxPos = Vector3I.Floor(vector4);
					grid.GetConnectedBlocks(otherBlockMinPos, otherBlockMaxPos, m_cacheNeighborBlocks);
					if (m_cacheNeighborBlocks.Count != 0)
					{
						Vector3I.Transform(ref thisMountPoint.Normal, ref rotation, out Vector3I result7);
						otherBlockMinPos -= result7;
						otherBlockMaxPos -= result7;
						Vector3I faceNormal = -result7;
						foreach (ConnectivityResult value13 in m_cacheNeighborBlocks.Values)
						{
							if (value13.Position == position)
							{
								if (MyFakes.ENABLE_COMPOUND_BLOCKS && (value13.FatBlock == null || !value13.FatBlock.CheckConnectionAllowed || value13.FatBlock.ConnectionAllowed(ref otherBlockMinPos, ref otherBlockMaxPos, ref faceNormal, def)) && value13.FatBlock is MyCompoundCubeBlock)
								{
									foreach (MySlimBlock block in (value13.FatBlock as MyCompoundCubeBlock).GetBlocks())
									{
										MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
										if (CheckNeighborMountPointsForCompound(vector3, vector4, thisMountPoint, ref result7, def, value13.Position, block.BlockDefinition, buildProgressModelMountPoints, block.Orientation, m_cacheMountPointsA))
										{
											return true;
										}
									}
								}
							}
							else if (value13.FatBlock == null || !value13.FatBlock.CheckConnectionAllowed || value13.FatBlock.ConnectionAllowed(ref otherBlockMinPos, ref otherBlockMaxPos, ref faceNormal, def))
							{
								if (value13.FatBlock is MyCompoundCubeBlock)
								{
									foreach (MySlimBlock block2 in (value13.FatBlock as MyCompoundCubeBlock).GetBlocks())
									{
										MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints2 = block2.BlockDefinition.GetBuildProgressModelMountPoints(block2.BuildLevelRatio);
										if (CheckNeighborMountPoints(vector3, vector4, thisMountPoint, ref result7, def, value13.Position, block2.BlockDefinition, buildProgressModelMountPoints2, block2.Orientation, m_cacheMountPointsA))
										{
											return true;
										}
									}
								}
								else
								{
									float currentIntegrityRatio = 1f;
									if (value13.FatBlock != null && value13.FatBlock.SlimBlock != null)
									{
										currentIntegrityRatio = value13.FatBlock.SlimBlock.BuildLevelRatio;
									}
									MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints3 = value13.Definition.GetBuildProgressModelMountPoints(currentIntegrityRatio);
									if (CheckNeighborMountPoints(vector3, vector4, thisMountPoint, ref result7, def, value13.Position, value13.Definition, buildProgressModelMountPoints3, value13.Orientation, m_cacheMountPointsA))
									{
										return true;
									}
								}
							}
						}
					}
				}
				return false;
			}
			finally
			{
				m_cacheNeighborBlocks.Clear();
			}
		}

		public static bool CheckConnectivitySmallBlockToLargeGrid(MyCubeGrid grid, MyCubeBlockDefinition def, ref Quaternion rotation, ref Vector3I addNormal)
		{
			try
			{
				MyCubeBlockDefinition.MountPoint[] mountPoints = def.MountPoints;
				if (mountPoints == null)
				{
					return false;
				}
				for (int i = 0; i < mountPoints.Length; i++)
				{
					MyCubeBlockDefinition.MountPoint mountPoint = mountPoints[i];
					Vector3I.Transform(ref mountPoint.Normal, ref rotation, out Vector3I result);
					if (addNormal == -result)
					{
						return true;
					}
				}
				return false;
			}
			finally
			{
				m_cacheNeighborBlocks.Clear();
			}
		}

		public static bool CheckNeighborMountPoints(Vector3 currentMin, Vector3 currentMax, MyCubeBlockDefinition.MountPoint thisMountPoint, ref Vector3I thisMountPointTransformedNormal, MyCubeBlockDefinition thisDefinition, Vector3I neighborPosition, MyCubeBlockDefinition neighborDefinition, MyCubeBlockDefinition.MountPoint[] neighborMountPoints, MyBlockOrientation neighborOrientation, List<MyCubeBlockDefinition.MountPoint> otherMountPoints)
		{
			if (!thisMountPoint.Enabled)
			{
				return false;
			}
			BoundingBox boundingBox = new BoundingBox(currentMin - neighborPosition, currentMax - neighborPosition);
			TransformMountPoints(otherMountPoints, neighborDefinition, neighborMountPoints, ref neighborOrientation);
			foreach (MyCubeBlockDefinition.MountPoint otherMountPoint in otherMountPoints)
			{
				if ((((thisMountPoint.ExclusionMask & otherMountPoint.PropertiesMask) == 0 && (thisMountPoint.PropertiesMask & otherMountPoint.ExclusionMask) == 0) || !(thisDefinition.Id != neighborDefinition.Id)) && otherMountPoint.Enabled && (!MyFakes.ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK || !(thisMountPointTransformedNormal + otherMountPoint.Normal != Vector3I.Zero)))
				{
					BoundingBox box = new BoundingBox(Vector3.Min(otherMountPoint.Start, otherMountPoint.End), Vector3.Max(otherMountPoint.Start, otherMountPoint.End));
					if (boundingBox.Intersects(box))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool CheckNeighborMountPointsForCompound(Vector3 currentMin, Vector3 currentMax, MyCubeBlockDefinition.MountPoint thisMountPoint, ref Vector3I thisMountPointTransformedNormal, MyCubeBlockDefinition thisDefinition, Vector3I neighborPosition, MyCubeBlockDefinition neighborDefinition, MyCubeBlockDefinition.MountPoint[] neighborMountPoints, MyBlockOrientation neighborOrientation, List<MyCubeBlockDefinition.MountPoint> otherMountPoints)
		{
			if (!thisMountPoint.Enabled)
			{
				return false;
			}
			BoundingBox boundingBox = new BoundingBox(currentMin - neighborPosition, currentMax - neighborPosition);
			TransformMountPoints(otherMountPoints, neighborDefinition, neighborMountPoints, ref neighborOrientation);
			foreach (MyCubeBlockDefinition.MountPoint otherMountPoint in otherMountPoints)
			{
				if ((((thisMountPoint.ExclusionMask & otherMountPoint.PropertiesMask) == 0 && (thisMountPoint.PropertiesMask & otherMountPoint.ExclusionMask) == 0) || !(thisDefinition.Id != neighborDefinition.Id)) && otherMountPoint.Enabled && (!MyFakes.ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK || !(thisMountPointTransformedNormal - otherMountPoint.Normal != Vector3I.Zero)))
				{
					BoundingBox box = new BoundingBox(Vector3.Min(otherMountPoint.Start, otherMountPoint.End), Vector3.Max(otherMountPoint.Start, otherMountPoint.End));
					if (boundingBox.Intersects(box))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool CheckMountPointsForSide(MyCubeBlockDefinition defA, MyCubeBlockDefinition.MountPoint[] mountPointsA, ref MyBlockOrientation orientationA, ref Vector3I positionA, ref Vector3I normalA, MyCubeBlockDefinition defB, MyCubeBlockDefinition.MountPoint[] mountPointsB, ref MyBlockOrientation orientationB, ref Vector3I positionB)
		{
			TransformMountPoints(m_cacheMountPointsA, defA, mountPointsA, ref orientationA);
			TransformMountPoints(m_cacheMountPointsB, defB, mountPointsB, ref orientationB);
			return CheckMountPointsForSide(m_cacheMountPointsA, ref orientationA, ref positionA, defA.Id, ref normalA, m_cacheMountPointsB, ref orientationB, ref positionB, defB.Id);
		}

		public static bool CheckMountPointsForSide(List<MyCubeBlockDefinition.MountPoint> transormedA, ref MyBlockOrientation orientationA, ref Vector3I positionA, MyDefinitionId idA, ref Vector3I normalA, List<MyCubeBlockDefinition.MountPoint> transormedB, ref MyBlockOrientation orientationB, ref Vector3I positionB, MyDefinitionId idB)
		{
			Vector3I value = positionB - positionA;
			Vector3I b = -normalA;
			for (int i = 0; i < transormedA.Count; i++)
			{
				if (!transormedA[i].Enabled)
				{
					continue;
				}
				MyCubeBlockDefinition.MountPoint mountPoint = transormedA[i];
				if (mountPoint.Normal != normalA)
				{
					continue;
				}
				Vector3 min = Vector3.Min(mountPoint.Start, mountPoint.End);
				Vector3 max = Vector3.Max(mountPoint.Start, mountPoint.End);
				min -= (Vector3)value;
				max -= (Vector3)value;
				BoundingBox boundingBox = new BoundingBox(min, max);
				for (int j = 0; j < transormedB.Count; j++)
				{
					if (!transormedB[j].Enabled)
					{
						continue;
					}
					MyCubeBlockDefinition.MountPoint mountPoint2 = transormedB[j];
					if (!(mountPoint2.Normal != b) && (((mountPoint.ExclusionMask & mountPoint2.PropertiesMask) == 0 && (mountPoint.PropertiesMask & mountPoint2.ExclusionMask) == 0) || !(idA != idB)))
					{
						BoundingBox box = new BoundingBox(Vector3.Min(mountPoint2.Start, mountPoint2.End), Vector3.Max(mountPoint2.Start, mountPoint2.End));
						if (boundingBox.Intersects(box))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private static void ConvertNextGrid(bool placeOnly)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.ConvertingObjs)), null, null, null, null, null, delegate
			{
				ConvertNextPrefab(m_prefabs, placeOnly);
			}, 1000));
		}

		private static void ConvertNextPrefab(List<MyObjectBuilder_CubeGrid[]> prefabs, bool placeOnly)
		{
			if (prefabs.Count > 0)
			{
				MyObjectBuilder_CubeGrid[] array = prefabs[0];
				_ = prefabs.Count;
				prefabs.RemoveAt(0);
				if (placeOnly)
				{
					float radius = GetBoundingSphereForGrids(array).Radius;
					m_maxDimensionPreviousRow = MathHelper.Max(radius, m_maxDimensionPreviousRow);
					if (prefabs.Count % 4 != 0)
					{
						m_newPositionForPlacedObject.X += 2f * radius + 10f;
					}
					else
					{
						m_newPositionForPlacedObject.X = 0f - (2f * radius + 10f);
						m_newPositionForPlacedObject.Z -= 2f * m_maxDimensionPreviousRow + 30f;
						m_maxDimensionPreviousRow = 0f;
					}
					PlacePrefabToWorld(array, MySector.MainCamera.Position + m_newPositionForPlacedObject);
					ConvertNextPrefab(m_prefabs, placeOnly);
				}
				else
				{
					List<MyCubeGrid> list = new List<MyCubeGrid>();
					MyObjectBuilder_CubeGrid[] array2 = array;
					foreach (MyObjectBuilder_CubeGrid objectBuilder in array2)
					{
						list.Add(MyEntities.CreateFromObjectBuilderAndAdd(objectBuilder, fadeIn: false) as MyCubeGrid);
					}
					ExportToObjFile(list, convertModelsFromSBC: true, exportObjAndSBC: false);
					foreach (MyCubeGrid item in list)
					{
						item.Close();
					}
				}
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(MyTexts.GetString(MyCommonTexts.ConvertToObjDone))));
			}
		}

		private static BoundingSphere GetBoundingSphereForGrids(MyObjectBuilder_CubeGrid[] currentPrefab)
		{
			BoundingSphere result = new BoundingSphere(Vector3.Zero, float.MinValue);
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in currentPrefab)
			{
				BoundingSphere boundingSphere = myObjectBuilder_CubeGrid.CalculateBoundingSphere();
				MatrixD m = myObjectBuilder_CubeGrid.PositionAndOrientation.HasValue ? myObjectBuilder_CubeGrid.PositionAndOrientation.Value.GetMatrix() : MatrixD.Identity;
				result.Include(boundingSphere.Transform(m));
			}
			return result;
		}

		public static void StartConverting(bool placeOnly)
		{
			string path = Path.Combine(MyFileSystem.UserDataPath, "SourceModels");
			if (Directory.Exists(path))
			{
				m_prefabs.Clear();
				string[] files = Directory.GetFiles(path, "*.zip");
				for (int i = 0; i < files.Length; i++)
				{
					foreach (string file in MyFileSystem.GetFiles(files[i], "*.sbc", MySearchOption.AllDirectories))
					{
						if (MyFileSystem.FileExists(file))
						{
							MyObjectBuilder_Definitions objectBuilder = null;
							MyObjectBuilderSerializer.DeserializeXML(file, out objectBuilder);
							if (objectBuilder.Prefabs[0].CubeGrids != null)
							{
								m_prefabs.Add(objectBuilder.Prefabs[0].CubeGrids);
							}
						}
					}
				}
				ConvertNextPrefab(m_prefabs, placeOnly);
			}
		}

		public static void ConvertPrefabsToObjs()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.ConvertingObjs)), null, null, null, null, null, delegate
			{
				StartConverting(placeOnly: false);
			}, 1000));
		}

		public static void PackFiles(string path, string objectName)
		{
			if (!Directory.Exists(path))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjFailed), path))));
				return;
			}
			using (MyZipArchive arc = MyZipArchive.OpenOnFile(Path.Combine(path, objectName + "_objFiles.zip"), ZipArchiveMode.Create))
			{
				PackFilesToDirectory(path, "*.png", arc);
				PackFilesToDirectory(path, "*.obj", arc);
				PackFilesToDirectory(path, "*.mtl", arc);
			}
			using (MyZipArchive arc2 = MyZipArchive.OpenOnFile(Path.Combine(path, objectName + ".zip"), ZipArchiveMode.Create))
			{
				PackFilesToDirectory(path, objectName + ".png", arc2);
				PackFilesToDirectory(path, "*.sbc", arc2);
			}
			RemoveFilesFromDirectory(path, "*.png");
			RemoveFilesFromDirectory(path, "*.sbc");
			RemoveFilesFromDirectory(path, "*.obj");
			RemoveFilesFromDirectory(path, "*.mtl");
		}

		private static void RemoveFilesFromDirectory(string path, string fileType)
		{
			string[] files = Directory.GetFiles(path, fileType);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
		}

		private static void PackFilesToDirectory(string path, string searchString, MyZipArchive arc)
		{
			int startIndex = path.Length + 1;
			string[] files = Directory.GetFiles(path, searchString, SearchOption.AllDirectories);
			foreach (string text in files)
			{
				using (FileStream fileStream = File.Open(text, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (Stream destination = arc.AddFile(text.Substring(startIndex), CompressionLevel.Optimal).GetStream())
					{
						fileStream.CopyTo(destination, 4096);
					}
				}
			}
		}

		public static void ExportObject(MyCubeGrid baseGrid, bool convertModelsFromSBC, bool exportObjAndSBC = false)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.ExportingToObj)), null, null, null, null, null, delegate
			{
				List<MyCubeGrid> list = new List<MyCubeGrid>();
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(baseGrid).Nodes)
				{
					list.Add(node.NodeData);
				}
				ExportToObjFile(list, convertModelsFromSBC, exportObjAndSBC);
			}, 1000));
		}

		private static void ExportToObjFile(List<MyCubeGrid> baseGrids, bool convertModelsFromSBC, bool exportObjAndSBC)
		{
			materialID = 0;
			MyValueFormatter.GetFormatedDateTimeForFilename(DateTime.Now);
			string name = MyUtils.StripInvalidChars(baseGrids[0].DisplayName.Replace(' ', '_'));
			string path = MyFileSystem.UserDataPath;
			string path2 = "ExportedModels";
			if (!convertModelsFromSBC || exportObjAndSBC)
			{
				path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				path2 = MyPerGameSettings.GameNameSafe + "_ExportedModels";
			}
			string folder = Path.Combine(path, path2, name);
			int num = 0;
			while (Directory.Exists(folder))
			{
				num++;
				folder = Path.Combine(path, path2, $"{name}_{num:000}");
			}
			MyUtils.CreateFolder(folder);
			if (!convertModelsFromSBC || exportObjAndSBC)
			{
				bool flag = false;
				string prefabPath = Path.Combine(folder, name + ".sbc");
				foreach (MyCubeGrid baseGrid in baseGrids)
				{
					foreach (MySlimBlock cubeBlock in baseGrid.CubeBlocks)
					{
						if (!cubeBlock.BlockDefinition.Context.IsBaseGame)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					CreatePrefabFile(baseGrids, name, prefabPath);
					MyRenderProxy.TakeScreenshot(tumbnailMultiplier, Path.Combine(folder, name + ".png"), debug: false, ignoreSprites: true, showNotification: false);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjComplete), folder)), null, null, null, null, null, delegate
					{
						PackFiles(folder, name);
					}));
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjModded), folder))));
				}
			}
			if (exportObjAndSBC || convertModelsFromSBC)
			{
				List<Vector3> vertices = new List<Vector3>();
				List<TriangleWithMaterial> triangles = new List<TriangleWithMaterial>();
				List<Vector2> uvs = new List<Vector2>();
				List<MyExportModel.Material> materials = new List<MyExportModel.Material>();
				int currVerticesCount = 0;
				try
				{
					GetModelDataFromGrid(baseGrids, vertices, triangles, uvs, materials, currVerticesCount);
					string filename = Path.Combine(folder, name + ".obj");
					string matFilename = Path.Combine(folder, name + ".mtl");
					CreateObjFile(name, filename, matFilename, vertices, triangles, uvs, materials, currVerticesCount);
					List<renderColoredTextureProperties> list = new List<renderColoredTextureProperties>();
					CreateMaterialFile(folder, matFilename, materials, list);
					if (list.Count > 0)
					{
						MyRenderProxy.RenderColoredTextures(list);
					}
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjComplete), folder)), null, null, null, null, null, delegate
					{
						ConvertNextGrid(placeOnly: false);
					}, 1000));
				}
				catch (Exception ex)
				{
					MySandboxGame.Log.WriteLine("Error while exporting to obj file.");
					MySandboxGame.Log.WriteLine(ex.ToString());
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ExportToObjFailed), folder))));
				}
			}
		}

		private static void CreatePrefabFile(List<MyCubeGrid> baseGrid, string name, string prefabPath)
		{
			Vector2I backBufferResolution = MyRenderProxy.BackBufferResolution;
			tumbnailMultiplier.X = 400f / (float)backBufferResolution.X;
			tumbnailMultiplier.Y = 400f / (float)backBufferResolution.Y;
			List<MyObjectBuilder_CubeGrid> list = new List<MyObjectBuilder_CubeGrid>();
			foreach (MyCubeGrid item in baseGrid)
			{
				list.Add((MyObjectBuilder_CubeGrid)item.GetObjectBuilder());
			}
			MyPrefabManager.SavePrefabToPath(name, prefabPath, list);
		}

		private static void GetModelDataFromGrid(List<MyCubeGrid> baseGrid, List<Vector3> vertices, List<TriangleWithMaterial> triangles, List<Vector2> uvs, List<MyExportModel.Material> materials, int currVerticesCount)
		{
			MatrixD matrix = MatrixD.Invert(baseGrid[0].WorldMatrix);
			foreach (MyCubeGrid item in baseGrid)
			{
				MatrixD m = item.WorldMatrix * matrix;
				foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in item.RenderData.Cells)
				{
					foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cell.Value.CubeParts)
					{
						MyCubePart key = cubePart.Key;
						Vector3 colorMaskHSV = new Vector3(key.InstanceData.ColorMaskHSV.X, key.InstanceData.ColorMaskHSV.Y, key.InstanceData.ColorMaskHSV.Z);
						Vector2 offsetUV = new Vector2(key.InstanceData.GetTextureOffset(0), key.InstanceData.GetTextureOffset(1));
						ExtractModelDataForObj(key.Model, key.InstanceData.LocalMatrix * (Matrix)m, vertices, triangles, uvs, ref offsetUV, materials, ref currVerticesCount, colorMaskHSV);
					}
				}
				foreach (MySlimBlock block in item.GetBlocks())
				{
					if (block.FatBlock != null)
					{
						if (block.FatBlock is MyPistonBase)
						{
							block.FatBlock.UpdateOnceBeforeFrame();
						}
						else if (block.FatBlock is MyCompoundCubeBlock)
						{
							foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
							{
								ExtractModelDataForObj(block2.FatBlock.Model, block2.FatBlock.PositionComp.WorldMatrix * matrix, vertices, triangles, uvs, ref Vector2.Zero, materials, ref currVerticesCount, block2.ColorMaskHSV);
								ProcessChildrens(vertices, triangles, uvs, materials, ref currVerticesCount, block2.FatBlock.PositionComp.WorldMatrix * matrix, block2.ColorMaskHSV, block2.FatBlock.Hierarchy.Children);
							}
							continue;
						}
						ExtractModelDataForObj(block.FatBlock.Model, block.FatBlock.PositionComp.WorldMatrix * matrix, vertices, triangles, uvs, ref Vector2.Zero, materials, ref currVerticesCount, block.ColorMaskHSV);
						ProcessChildrens(vertices, triangles, uvs, materials, ref currVerticesCount, block.FatBlock.PositionComp.WorldMatrix * matrix, block.ColorMaskHSV, block.FatBlock.Hierarchy.Children);
					}
				}
			}
		}

		private static void CreateObjFile(string name, string filename, string matFilename, List<Vector3> vertices, List<TriangleWithMaterial> triangles, List<Vector2> uvs, List<MyExportModel.Material> materials, int currVerticesCount)
		{
			using (StreamWriter streamWriter = new StreamWriter(filename))
			{
				streamWriter.WriteLine("mtllib {0}", Path.GetFileName(matFilename));
				streamWriter.WriteLine();
				streamWriter.WriteLine("#");
				streamWriter.WriteLine("# {0}", name);
				streamWriter.WriteLine("#");
				streamWriter.WriteLine();
				streamWriter.WriteLine("# vertices");
				List<int> list = new List<int>(vertices.Count);
				Dictionary<Vector3D, int> dictionary = new Dictionary<Vector3D, int>(vertices.Count / 5);
				int num = 1;
				foreach (Vector3 vertex in vertices)
				{
					if (!dictionary.TryGetValue(vertex, out int value))
					{
						value = num++;
						dictionary.Add(vertex, value);
						streamWriter.WriteLine("v {0} {1} {2}", vertex.X, vertex.Y, vertex.Z);
					}
					list.Add(value);
				}
				dictionary = null;
				List<int> list2 = new List<int>(vertices.Count);
				Dictionary<Vector2, int> dictionary2 = new Dictionary<Vector2, int>(vertices.Count / 5);
				streamWriter.WriteLine("# {0} vertices", vertices.Count);
				streamWriter.WriteLine();
				streamWriter.WriteLine("# texture coordinates");
				num = 1;
				foreach (Vector2 uv in uvs)
				{
					if (!dictionary2.TryGetValue(uv, out int value2))
					{
						value2 = num++;
						dictionary2.Add(uv, value2);
						streamWriter.WriteLine("vt {0} {1}", uv.X, uv.Y);
					}
					list2.Add(value2);
				}
				dictionary2 = null;
				streamWriter.WriteLine("# {0} texture coords", uvs.Count);
				streamWriter.WriteLine();
				streamWriter.WriteLine("# faces");
				streamWriter.WriteLine("o {0}", name);
				int num2 = 0;
				foreach (MyExportModel.Material material in materials)
				{
					num2++;
					string exportedMaterialName = material.ExportedMaterialName;
					streamWriter.WriteLine();
					streamWriter.WriteLine("g {0}_part{1}", name, num2);
					streamWriter.WriteLine("usemtl {0}", exportedMaterialName);
					streamWriter.WriteLine("s off");
					for (int i = 0; i < triangles.Count; i++)
					{
						if (exportedMaterialName == triangles[i].material)
						{
							TriangleWithMaterial triangleWithMaterial = triangles[i];
							MyTriangleVertexIndices triangle = triangleWithMaterial.triangle;
							MyTriangleVertexIndices uvIndices = triangleWithMaterial.uvIndices;
							streamWriter.WriteLine("f {0}/{3} {1}/{4} {2}/{5}", list[triangle.I0 - 1], list[triangle.I1 - 1], list[triangle.I2 - 1], list2[uvIndices.I0 - 1], list2[uvIndices.I1 - 1], list2[uvIndices.I2 - 1]);
						}
					}
				}
				streamWriter.WriteLine("# {0} faces", triangles.Count);
			}
		}

		private static void CreateMaterialFile(string folder, string matFilename, List<MyExportModel.Material> materials, List<renderColoredTextureProperties> texturesToRender)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			using (StreamWriter streamWriter = new StreamWriter(matFilename))
			{
				foreach (MyExportModel.Material material in materials)
				{
					string exportedMaterialName = material.ExportedMaterialName;
					streamWriter.WriteLine("newmtl {0}", exportedMaterialName);
					if (MyFakes.ENABLE_EXPORT_MTL_DIAGNOSTICS)
					{
						Vector3 colorMaskHSV = material.ColorMaskHSV;
						streamWriter.WriteLine("# HSV Mask: {0}", colorMaskHSV.ToString("F2"));
						streamWriter.WriteLine("# IsGlass: {0}", material.IsGlass);
						streamWriter.WriteLine("# AddMapsMap: {0}", material.AddMapsTexture ?? "Null");
						streamWriter.WriteLine("# AlphamaskMap: {0}", material.AlphamaskTexture ?? "Null");
						streamWriter.WriteLine("# ColorMetalMap: {0}", material.ColorMetalTexture ?? "Null");
						streamWriter.WriteLine("# NormalGlossMap: {0}", material.NormalGlossTexture ?? "Null");
					}
					if (!material.IsGlass)
					{
						streamWriter.WriteLine("Ka 1.000 1.000 1.000");
						streamWriter.WriteLine("Kd 1.000 1.000 1.000");
						streamWriter.WriteLine("Ks 0.100 0.100 0.100");
						streamWriter.WriteLine((material.AlphamaskTexture == null) ? "d 1.0" : "d 0.0");
					}
					else
					{
						streamWriter.WriteLine("Ka 0.000 0.000 0.000");
						streamWriter.WriteLine("Kd 0.000 0.000 0.000");
						streamWriter.WriteLine("Ks 0.900 0.900 0.900");
						streamWriter.WriteLine("d 0.350");
					}
					streamWriter.WriteLine("Ns 95.00");
					streamWriter.WriteLine("illum 2");
					if (material.ColorMetalTexture != null)
					{
						string format = exportedMaterialName + "_{0}.png";
						string text = string.Format(format, "ca");
						string text2 = string.Format(format, "ng");
						streamWriter.WriteLine("map_Ka {0}", text);
						streamWriter.WriteLine("map_Kd {0}", text);
						if (material.AlphamaskTexture != null)
						{
							streamWriter.WriteLine("map_d {0}", text);
						}
						bool flag = false;
						if (material.NormalGlossTexture != null)
						{
							if (dictionary.TryGetValue(material.NormalGlossTexture, out string value))
							{
								text2 = value;
							}
							else
							{
								flag = true;
								dictionary.Add(material.NormalGlossTexture, text2);
							}
							streamWriter.WriteLine("map_Bump {0}", text2);
						}
						texturesToRender.Add(new renderColoredTextureProperties
						{
							ColorMaskHSV = material.ColorMaskHSV,
							TextureAddMaps = material.AddMapsTexture,
							TextureAplhaMask = material.AlphamaskTexture,
							TextureColorMetal = material.ColorMetalTexture,
							TextureNormalGloss = (flag ? material.NormalGlossTexture : null),
							PathToSave_ColorAlpha = Path.Combine(folder, text),
							PathToSave_NormalGloss = Path.Combine(folder, text2)
						});
					}
					streamWriter.WriteLine();
				}
			}
		}

		private static void ProcessChildrens(List<Vector3> vertices, List<TriangleWithMaterial> triangles, List<Vector2> uvs, List<MyExportModel.Material> materials, ref int currVerticesCount, Matrix parentMatrix, Vector3 HSV, ListReader<MyHierarchyComponentBase> childrens)
		{
			foreach (MyHierarchyComponentBase item in childrens)
			{
				VRage.ModAPI.IMyEntity entity = item.Container.Entity;
				MyModel model = (entity as MyEntity).Model;
				if (model != null)
				{
					ExtractModelDataForObj(model, entity.LocalMatrix * parentMatrix, vertices, triangles, uvs, ref Vector2.Zero, materials, ref currVerticesCount, HSV);
				}
				ProcessChildrens(vertices, triangles, uvs, materials, ref currVerticesCount, entity.LocalMatrix * parentMatrix, HSV, entity.Hierarchy.Children);
			}
		}

		public static void PlacePrefabsToWorld()
		{
			m_newPositionForPlacedObject = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition();
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.NONE_TIMEOUT, new StringBuilder(MyTexts.GetString(MyCommonTexts.PlacingObjectsToScene)), null, null, null, null, null, delegate
			{
				StartConverting(placeOnly: true);
			}, 1000));
		}

		public static void PlacePrefabToWorld(MyObjectBuilder_CubeGrid[] currentPrefab, Vector3D position, List<MyCubeGrid> createdGrids = null)
		{
			Vector3D v = Vector3D.Zero;
			Vector3D value = Vector3D.Zero;
			bool flag = true;
			MyEntities.RemapObjectBuilderCollection(currentPrefab);
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in currentPrefab)
			{
				if (myObjectBuilder_CubeGrid.PositionAndOrientation.HasValue)
				{
					if (flag)
					{
						value = position - myObjectBuilder_CubeGrid.PositionAndOrientation.Value.Position;
						flag = false;
						v = position;
					}
					else
					{
						v = myObjectBuilder_CubeGrid.PositionAndOrientation.Value.Position + value;
					}
				}
				MyPositionAndOrientation value2 = myObjectBuilder_CubeGrid.PositionAndOrientation.Value;
				value2.Position = v;
				myObjectBuilder_CubeGrid.PositionAndOrientation = value2;
				MyCubeGrid myCubeGrid = MyEntities.CreateFromObjectBuilder(myObjectBuilder_CubeGrid, fadeIn: false) as MyCubeGrid;
				if (myCubeGrid != null)
				{
					myCubeGrid.ClearSymmetries();
					myCubeGrid.Physics.LinearVelocity = Vector3D.Zero;
					myCubeGrid.Physics.AngularVelocity = Vector3D.Zero;
					createdGrids?.Add(myCubeGrid);
					MyEntities.Add(myCubeGrid);
				}
			}
		}

		public static MyCubeGrid GetTargetGrid()
		{
			MyEntity myEntity = MyCubeBuilder.Static.FindClosestGrid();
			if (myEntity == null)
			{
				myEntity = GetTargetEntity();
			}
			return myEntity as MyCubeGrid;
		}

		public static MyEntity GetTargetEntity()
		{
			LineD ray = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 10000f);
			m_tmpHitList.Clear();
			MyPhysics.CastRay(ray.From, ray.To, m_tmpHitList, 15);
			m_tmpHitList.RemoveAll((MyPhysics.HitInfo hit) => MySession.Static.ControlledEntity != null && hit.HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity);
			if (m_tmpHitList.Count == 0)
			{
				using (MyUtils.ReuseCollection(ref m_lineOverlapList))
				{
					MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray, m_lineOverlapList);
					if (m_lineOverlapList.Count > 0)
					{
						return m_lineOverlapList[0].Element.GetTopMostParent();
					}
					return null;
				}
			}
			return m_tmpHitList[0].HkHitInfo.GetHitEntity() as MyEntity;
		}

		public static bool TryRayCastGrid(ref LineD worldRay, out MyCubeGrid hitGrid, out Vector3D worldHitPos)
		{
			try
			{
				MyPhysics.CastRay(worldRay.From, worldRay.To, m_tmpHitList);
				foreach (MyPhysics.HitInfo tmpHit in m_tmpHitList)
				{
					MyCubeGrid myCubeGrid = tmpHit.HkHitInfo.GetHitEntity() as MyCubeGrid;
					if (myCubeGrid != null)
					{
						worldHitPos = tmpHit.Position;
						MyRenderProxy.DebugDrawAABB(new BoundingBoxD(worldHitPos - 0.01, worldHitPos + 0.01), Color.Wheat.ToVector3());
						hitGrid = myCubeGrid;
						return true;
					}
				}
				hitGrid = null;
				worldHitPos = default(Vector3D);
				return false;
			}
			finally
			{
				m_tmpHitList.Clear();
			}
		}

		public static bool TestBlockPlacementArea(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, ref Vector3D translation, ref Quaternion rotation, ref Vector3 halfExtents, ref BoundingBoxD localAabb, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null)
		{
			MyCubeGrid touchingGrid;
			return TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translation, ref rotation, ref halfExtents, ref localAabb, out touchingGrid, placingPlayer, ignoredEntity);
		}

		public static bool TestBlockPlacementArea(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, ref Vector3D translationObsolete, ref Quaternion rotation, ref Vector3 halfExtentsObsolete, ref BoundingBoxD localAabb, out MyCubeGrid touchingGrid, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool ignoreFracturedPieces = false, bool testVoxel = true)
		{
			touchingGrid = null;
			MatrixD m = targetGrid?.WorldMatrix ?? MatrixD.Identity;
			if (!MyEntities.IsInsideWorld(m.Translation))
			{
				return false;
			}
			Vector3 halfExtents = localAabb.HalfExtents;
			halfExtents += settings.SearchHalfExtentsDeltaAbsolute;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtents -= new Vector3D(0.11);
			}
			Vector3D translation = localAabb.TransformFast(ref m).Center;
			Quaternion.CreateFromRotationMatrix(m).Normalize();
			if (testVoxel && settings.VoxelPlacement.HasValue && settings.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Both)
			{
				bool flag = IsAabbInsideVoxel(m, localAabb, settings);
				if (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.InVoxel)
				{
					flag = !flag;
				}
				if (flag)
				{
					return false;
				}
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(localAabb.TransformFast(ref m), MySafeZoneAction.Building, 0L, placingPlayer))
			{
				return false;
			}
			if (blockDefinition != null && blockDefinition.UseModelIntersection)
			{
				MyModel modelOnlyData = MyModels.GetModelOnlyData(blockDefinition.Model);
				if (modelOnlyData != null)
				{
					modelOnlyData.CheckLoadingErrors(blockDefinition.Context, out bool errorFound);
					if (errorFound)
					{
						MyDefinitionErrors.Add(blockDefinition.Context, "There was error during loading of model, please check log file.", TErrorSeverity.Error);
					}
				}
				if (modelOnlyData != null && modelOnlyData.HavokCollisionShapes != null)
				{
					blockOrientation.GetMatrix(out Matrix result);
					Vector3.TransformNormal(ref blockDefinition.ModelOffset, ref result, out Vector3 result2);
					translation += result2;
					int num = modelOnlyData.HavokCollisionShapes.Length;
					HkShape[] array = new HkShape[num];
					for (int i = 0; i < num; i++)
					{
						array[i] = modelOnlyData.HavokCollisionShapes[i];
					}
					HkListShape shape = new HkListShape(array, num, HkReferencePolicy.None);
					Quaternion quaternion = Quaternion.CreateFromForwardUp(Base6Directions.GetVector(blockOrientation.Forward), Base6Directions.GetVector(blockOrientation.Up));
					rotation *= quaternion;
					MyPhysics.GetPenetrationsShape(shape, ref translation, ref rotation, m_physicsBoxQueryList, 7);
					shape.Base.RemoveReference();
				}
				else
				{
					MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 7);
				}
			}
			else
			{
				MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 7);
			}
			m_lastQueryBox.Value.HalfExtents = halfExtents;
			m_lastQueryTransform = MatrixD.CreateFromQuaternion(rotation);
			m_lastQueryTransform.Translation = translation;
			return TestPlacementAreaInternal(targetGrid, ref settings, blockDefinition, blockOrientation, ref localAabb, ignoredEntity, ref m, out touchingGrid, dynamicBuildMode: false, ignoreFracturedPieces);
		}

		public static bool TestPlacementAreaCube(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, Vector3I min, Vector3I max, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool ignoreFracturedPieces = false)
		{
			MyCubeGrid touchingGrid = null;
			return TestPlacementAreaCube(targetGrid, ref settings, min, max, blockOrientation, blockDefinition, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces);
		}

		public static bool TestPlacementAreaCube(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, Vector3I min, Vector3I max, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, out MyCubeGrid touchingGrid, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null, bool ignoreFracturedPieces = false)
		{
			touchingGrid = null;
			MatrixD matrix = targetGrid?.WorldMatrix ?? MatrixD.Identity;
			if (!MyEntities.IsInsideWorld(matrix.Translation))
			{
				return false;
			}
			float num = targetGrid?.GridSize ?? MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);
			Vector3 halfExtentsObsolete = ((max - min) * num + num) / 2f;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtentsObsolete -= new Vector3D(0.11);
			}
			else
			{
				halfExtentsObsolete -= new Vector3(0.03f, 0.03f, 0.03f);
			}
			MatrixD matrix2 = MatrixD.CreateTranslation((max + min) * 0.5f * num) * matrix;
			BoundingBoxD localAabb = BoundingBoxD.CreateInvalid();
			localAabb.Include(min * num - num / 2f);
			localAabb.Include(max * num + num / 2f);
			Vector3D translationObsolete = matrix2.Translation;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(matrix2);
			return TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translationObsolete, ref rotation, ref halfExtentsObsolete, ref localAabb, out touchingGrid, placingPlayer, ignoredEntity, ignoreFracturedPieces);
		}

		public static bool TestPlacementAreaCubeNoAABBInflate(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, Vector3I min, Vector3I max, MyBlockOrientation blockOrientation, MyCubeBlockDefinition blockDefinition, out MyCubeGrid touchingGrid, ulong placingPlayer = 0uL, MyEntity ignoredEntity = null)
		{
			touchingGrid = null;
			MatrixD matrix = targetGrid?.WorldMatrix ?? MatrixD.Identity;
			if (!MyEntities.IsInsideWorld(matrix.Translation))
			{
				return false;
			}
			float num = targetGrid?.GridSize ?? MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);
			Vector3 halfExtentsObsolete = ((max - min) * num + num) / 2f;
			MatrixD matrix2 = MatrixD.CreateTranslation((max + min) * 0.5f * num) * matrix;
			BoundingBoxD localAabb = BoundingBoxD.CreateInvalid();
			localAabb.Include(min * num - num / 2f);
			localAabb.Include(max * num + num / 2f);
			Vector3D translationObsolete = matrix2.Translation;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(matrix2);
			return TestBlockPlacementArea(targetGrid, ref settings, blockOrientation, blockDefinition, ref translationObsolete, ref rotation, ref halfExtentsObsolete, ref localAabb, out touchingGrid, placingPlayer, ignoredEntity);
		}

		public static bool TestPlacementArea(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, BoundingBoxD localAabb, bool dynamicBuildMode, MyEntity ignoredEntity = null)
		{
			MatrixD m = targetGrid.WorldMatrix;
			if (!MyEntities.IsInsideWorld(m.Translation))
			{
				return false;
			}
			Vector3 halfExtents = localAabb.HalfExtents;
			halfExtents += settings.SearchHalfExtentsDeltaAbsolute;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtents -= new Vector3D(0.11);
			}
			Vector3D translation = localAabb.TransformFast(ref m).Center;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(m);
			rotation.Normalize();
			MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 18);
			m_lastQueryBox.Value.HalfExtents = halfExtents;
			m_lastQueryTransform = MatrixD.CreateFromQuaternion(rotation);
			m_lastQueryTransform.Translation = translation;
			MyCubeGrid touchingGrid;
			return TestPlacementAreaInternal(targetGrid, ref settings, null, null, ref localAabb, ignoredEntity, ref m, out touchingGrid, dynamicBuildMode);
		}

		public static bool TestPlacementArea(MyCubeGrid targetGrid, bool targetGridIsStatic, ref MyGridPlacementSettings settings, BoundingBoxD localAabb, bool dynamicBuildMode, MyEntity ignoredEntity = null, bool testVoxel = true, bool testPhysics = true)
		{
			MatrixD m = targetGrid.WorldMatrix;
			if (!MyEntities.IsInsideWorld(m.Translation))
			{
				return false;
			}
			Vector3 halfExtents = localAabb.HalfExtents;
			halfExtents += settings.SearchHalfExtentsDeltaAbsolute;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtents -= new Vector3D(0.11);
			}
			Vector3D translation = localAabb.TransformFast(ref m).Center;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(m);
			rotation.Normalize();
			if (testVoxel && settings.VoxelPlacement.HasValue && settings.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Both)
			{
				bool flag = IsAabbInsideVoxel(m, localAabb, settings);
				if (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.InVoxel)
				{
					flag = !flag;
				}
				if (flag)
				{
					return false;
				}
			}
			bool result = true;
			if (testPhysics)
			{
				MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 7);
				m_lastQueryBox.Value.HalfExtents = halfExtents;
				m_lastQueryTransform = MatrixD.CreateFromQuaternion(rotation);
				m_lastQueryTransform.Translation = translation;
				result = TestPlacementAreaInternal(targetGrid, targetGridIsStatic, ref settings, null, null, ref localAabb, ignoredEntity, ref m, out MyCubeGrid _, dynamicBuildMode);
			}
			return result;
		}

		public static bool IsAabbInsideVoxel(MatrixD worldMatrix, BoundingBoxD localAabb, MyGridPlacementSettings settings)
		{
			if (!settings.VoxelPlacement.HasValue)
			{
				return false;
			}
			BoundingBoxD box = localAabb.TransformFast(ref worldMatrix);
			List<MyVoxelBase> list = new List<MyVoxelBase>();
			MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, list);
			foreach (MyVoxelBase item in list)
			{
				if (settings.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Volumetric && item.IsAnyAabbCornerInside(ref worldMatrix, localAabb))
				{
					return true;
				}
				if (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.Volumetric && !TestPlacementVoxelMapPenetration(item, settings, ref localAabb, ref worldMatrix))
				{
					return true;
				}
			}
			return false;
		}

		public static bool TestBlockPlacementArea(MyCubeBlockDefinition blockDefinition, MyBlockOrientation? blockOrientation, MatrixD worldMatrix, ref MyGridPlacementSettings settings, BoundingBoxD localAabb, bool dynamicBuildMode, MyEntity ignoredEntity = null, bool testVoxel = true)
		{
			if (!MyEntities.IsInsideWorld(worldMatrix.Translation))
			{
				return false;
			}
			Vector3 halfExtents = localAabb.HalfExtents;
			halfExtents += settings.SearchHalfExtentsDeltaAbsolute;
			if (MyFakes.ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA)
			{
				halfExtents -= new Vector3D(0.11);
			}
			Vector3D translation = localAabb.TransformFast(ref worldMatrix).Center;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(worldMatrix);
			rotation.Normalize();
			MyGridPlacementSettings settings2 = settings;
			if (dynamicBuildMode && blockDefinition.CubeSize == MyCubeSize.Large)
			{
				settings2.VoxelPlacement = new VoxelPlacementSettings
				{
					PlacementMode = VoxelPlacementMode.Both
				};
			}
			if (testVoxel && !TestVoxelPlacement(blockDefinition, settings2, dynamicBuildMode, worldMatrix, localAabb))
			{
				return false;
			}
			MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_physicsBoxQueryList, 7);
			m_lastQueryBox.Value.HalfExtents = halfExtents;
			m_lastQueryTransform = MatrixD.CreateFromQuaternion(rotation);
			m_lastQueryTransform.Translation = translation;
			MyCubeGrid touchingGrid;
			return TestPlacementAreaInternal(null, ref settings2, blockDefinition, blockOrientation, ref localAabb, ignoredEntity, ref worldMatrix, out touchingGrid, dynamicBuildMode);
		}

		public static bool TestVoxelPlacement(MyCubeBlockDefinition blockDefinition, MyGridPlacementSettings settingsCopy, bool dynamicBuildMode, MatrixD worldMatrix, BoundingBoxD localAabb)
		{
			if (blockDefinition.VoxelPlacement.HasValue)
			{
				settingsCopy.VoxelPlacement = (dynamicBuildMode ? blockDefinition.VoxelPlacement.Value.DynamicMode : blockDefinition.VoxelPlacement.Value.StaticMode);
			}
			if (!MyEntities.IsInsideWorld(worldMatrix.Translation))
			{
				return false;
			}
			if (settingsCopy.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.None)
			{
				return false;
			}
			if (settingsCopy.VoxelPlacement.Value.PlacementMode != VoxelPlacementMode.Both)
			{
				bool flag = IsAabbInsideVoxel(worldMatrix, localAabb, settingsCopy);
				if (settingsCopy.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.InVoxel)
				{
					flag = !flag;
				}
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		private static void ExtractModelDataForObj(MyModel model, Matrix matrix, List<Vector3> vertices, List<TriangleWithMaterial> triangles, List<Vector2> uvs, ref Vector2 offsetUV, List<MyExportModel.Material> materials, ref int currVerticesCount, Vector3 colorMaskHSV)
		{
			if (!model.HasUV)
			{
				model.LoadUV = true;
				model.UnloadData();
				model.LoadData();
			}
			MyExportModel myExportModel = new MyExportModel(model);
			int verticesCount = myExportModel.GetVerticesCount();
			List<HalfVector2> uVsForModel = GetUVsForModel(myExportModel, verticesCount);
			if (uVsForModel.Count != verticesCount)
			{
				return;
			}
			List<MyExportModel.Material> list = CreateMaterialsForModel(materials, colorMaskHSV, myExportModel);
			for (int i = 0; i < verticesCount; i++)
			{
				vertices.Add(Vector3.Transform(model.GetVertex(i), matrix));
				Vector2 vector = uVsForModel[i].ToVector2() / model.PatternScale + offsetUV;
				uvs.Add(new Vector2(vector.X, 0f - vector.Y));
			}
			for (int j = 0; j < myExportModel.GetTrianglesCount(); j++)
			{
				int num = -1;
				for (int k = 0; k < list.Count; k++)
				{
					if (j <= list[k].LastTri)
					{
						num = k;
						break;
					}
				}
				MyTriangleVertexIndices triangle = myExportModel.GetTriangle(j);
				string material = "EmptyMaterial";
				if (num != -1)
				{
					material = list[num].ExportedMaterialName;
				}
				triangles.Add(new TriangleWithMaterial
				{
					material = material,
					triangle = new MyTriangleVertexIndices(triangle.I0 + 1 + currVerticesCount, triangle.I1 + 1 + currVerticesCount, triangle.I2 + 1 + currVerticesCount),
					uvIndices = new MyTriangleVertexIndices(triangle.I0 + 1 + currVerticesCount, triangle.I1 + 1 + currVerticesCount, triangle.I2 + 1 + currVerticesCount)
				});
			}
			currVerticesCount += verticesCount;
		}

		private static List<HalfVector2> GetUVsForModel(MyExportModel renderModel, int modelVerticesCount)
		{
			return renderModel.GetTexCoords().ToList();
		}

		private static List<MyExportModel.Material> CreateMaterialsForModel(List<MyExportModel.Material> materials, Vector3 colorMaskHSV, MyExportModel renderModel)
		{
			List<MyExportModel.Material> materials2 = renderModel.GetMaterials();
			List<MyExportModel.Material> list = new List<MyExportModel.Material>(materials2.Count);
			foreach (MyExportModel.Material item2 in materials2)
			{
				MyExportModel.Material? material = null;
				foreach (MyExportModel.Material material2 in materials)
				{
					if ((double)(colorMaskHSV - material2.ColorMaskHSV).AbsMax() < 0.01 && item2.EqualsMaterialWise(material2))
					{
						material = material2;
						break;
					}
				}
				MyExportModel.Material item = item2;
				item.ColorMaskHSV = colorMaskHSV;
				if (material.HasValue)
				{
					item.ExportedMaterialName = material.Value.ExportedMaterialName;
				}
				else
				{
					materialID++;
					item.ExportedMaterialName = "material_" + materialID;
					materials.Add(item);
				}
				list.Add(item);
			}
			return list;
		}

		private static MyCubePart[] GetCubeParts(MyStringHash skinSubtypeId, MyCubeBlockDefinition block, Vector3I position, MatrixD rotation, float gridSize, float gridScale)
		{
			List<string> list = new List<string>();
			List<MatrixD> list2 = new List<MatrixD>();
			List<Vector3> outLocalNormals = new List<Vector3>();
			List<Vector4UByte> list3 = new List<Vector4UByte>();
			GetCubeParts(block, position, rotation, gridSize, list, list2, outLocalNormals, list3, topologyCheck: true);
			MyCubePart[] array = new MyCubePart[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				MyCubePart myCubePart = new MyCubePart();
				MyModel modelOnlyData = MyModels.GetModelOnlyData(list[i]);
				modelOnlyData.Rescale(gridScale);
				myCubePart.Init(modelOnlyData, skinSubtypeId, list2[i], gridScale);
				myCubePart.InstanceData.SetTextureOffset(list3[i]);
				array[i] = myCubePart;
			}
			return array;
		}

		private static bool TestPlacementAreaInternal(MyCubeGrid targetGrid, ref MyGridPlacementSettings settings, MyCubeBlockDefinition blockDefinition, MyBlockOrientation? blockOrientation, ref BoundingBoxD localAabb, MyEntity ignoredEntity, ref MatrixD worldMatrix, out MyCubeGrid touchingGrid, bool dynamicBuildMode = false, bool ignoreFracturedPieces = false)
		{
			return TestPlacementAreaInternal(targetGrid, targetGrid?.IsStatic ?? (!dynamicBuildMode), ref settings, blockDefinition, blockOrientation, ref localAabb, ignoredEntity, ref worldMatrix, out touchingGrid, dynamicBuildMode, ignoreFracturedPieces);
		}

		private static bool TestPlacementAreaInternalWithEntities(MyCubeGrid targetGrid, bool targetGridIsStatic, ref MyGridPlacementSettings settings, ref BoundingBoxD localAabb, MyEntity ignoredEntity, ref MatrixD worldMatrix, bool dynamicBuildMode = false)
		{
			MyCubeGrid touchingGrid = null;
			float gridSize = targetGrid.GridSize;
			bool flag = targetGridIsStatic;
			localAabb.TransformFast(ref worldMatrix);
			bool entityOverlap = false;
			bool touchingStaticGrid = false;
			foreach (MyEntity tmpResult in m_tmpResultList)
			{
				if ((ignoredEntity == null || (tmpResult != ignoredEntity && tmpResult.GetTopMostParent() != ignoredEntity)) && tmpResult.Physics != null)
				{
					MyCubeGrid myCubeGrid = tmpResult as MyCubeGrid;
					if (myCubeGrid != null)
					{
						if (flag != myCubeGrid.IsStatic || gridSize == myCubeGrid.GridSize)
						{
							TestGridPlacement(ref settings, ref worldMatrix, ref touchingGrid, gridSize, flag, ref localAabb, null, null, ref entityOverlap, ref touchingStaticGrid, myCubeGrid);
							if (entityOverlap)
							{
								break;
							}
						}
					}
					else
					{
						MyCharacter myCharacter = tmpResult as MyCharacter;
						if (myCharacter != null && myCharacter.PositionComp.WorldAABB.Intersects(targetGrid.PositionComp.WorldAABB))
						{
							entityOverlap = true;
							break;
						}
					}
				}
			}
			m_tmpResultList.Clear();
			if (entityOverlap)
			{
				return false;
			}
			_ = targetGrid.IsStatic;
			return true;
		}

		private static void TestGridPlacement(ref MyGridPlacementSettings settings, ref MatrixD worldMatrix, ref MyCubeGrid touchingGrid, float gridSize, bool isStatic, ref BoundingBoxD localAABB, MyCubeBlockDefinition blockDefinition, MyBlockOrientation? blockOrientation, ref bool entityOverlap, ref bool touchingStaticGrid, MyCubeGrid grid)
		{
			BoundingBoxD boundingBoxD = localAABB.TransformFast(ref worldMatrix);
			MatrixD m = grid.PositionComp.WorldMatrixNormalizedInv;
			boundingBoxD.TransformFast(ref m);
			Vector3D position = Vector3D.Transform(localAABB.Min, worldMatrix);
			Vector3D position2 = Vector3D.Transform(localAABB.Max, worldMatrix);
			Vector3D value = Vector3D.Transform(position, m);
			Vector3D value2 = Vector3D.Transform(position2, m);
			Vector3D value3 = Vector3D.Min(value, value2);
			Vector3D value4 = Vector3D.Max(value, value2);
			Vector3D value5 = (value3 + gridSize / 2f) / grid.GridSize;
			Vector3D value6 = (value4 - gridSize / 2f) / grid.GridSize;
			Vector3I value7 = Vector3I.Round(value5);
			Vector3I value8 = Vector3I.Round(value6);
			Vector3I min = Vector3I.Min(value7, value8);
			Vector3I max = Vector3I.Max(value7, value8);
			MyBlockOrientation? orientation = null;
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && isStatic && grid.IsStatic && blockOrientation.HasValue)
			{
				blockOrientation.Value.GetMatrix(out Matrix result);
				Matrix matrix = result * worldMatrix;
				matrix *= m;
				matrix.Translation = Vector3.Zero;
				Base6Directions.Direction forward = Base6Directions.GetForward(ref matrix);
				Base6Directions.Direction up = Base6Directions.GetUp(ref matrix);
				if (Base6Directions.IsValidBlockOrientation(forward, up))
				{
					orientation = new MyBlockOrientation(forward, up);
				}
			}
			if (!grid.CanAddCubes(min, max, orientation, blockDefinition))
			{
				entityOverlap = true;
			}
			else if (settings.CanAnchorToStaticGrid && grid.IsTouchingAnyNeighbor(min, max))
			{
				touchingStaticGrid = true;
				if (touchingGrid == null)
				{
					touchingGrid = grid;
				}
			}
		}

		private static bool TestPlacementAreaInternal(MyCubeGrid targetGrid, bool targetGridIsStatic, ref MyGridPlacementSettings settings, MyCubeBlockDefinition blockDefinition, MyBlockOrientation? blockOrientation, ref BoundingBoxD localAabb, MyEntity ignoredEntity, ref MatrixD worldMatrix, out MyCubeGrid touchingGrid, bool dynamicBuildMode = false, bool ignoreFracturedPieces = false)
		{
			touchingGrid = null;
			float num = targetGrid?.GridSize ?? ((blockDefinition != null) ? MyDefinitionManager.Static.GetCubeSize(blockDefinition.CubeSize) : MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large));
			bool flag = targetGridIsStatic;
			bool entityOverlap = false;
			bool touchingStaticGrid = false;
			foreach (HkBodyCollision physicsBoxQuery in m_physicsBoxQueryList)
			{
				MyEntity myEntity = physicsBoxQuery.Body.GetEntity(0u) as MyEntity;
				if (myEntity != null && myEntity.GetTopMostParent().GetPhysicsBody() != null && (!ignoreFracturedPieces || !(myEntity is MyFracturedPiece)) && (myEntity.GetTopMostParent().GetPhysicsBody().WeldInfo.Children.Count != 0 || ignoredEntity == null || (myEntity != ignoredEntity && myEntity.GetTopMostParent() != ignoredEntity)))
				{
					MyPhysicsComponentBase physics = myEntity.GetTopMostParent().Physics;
					if (physics == null || !physics.IsPhantom)
					{
						MyCubeGrid myCubeGrid = myEntity.GetTopMostParent() as MyCubeGrid;
						if (myEntity.GetTopMostParent().GetPhysicsBody().WeldInfo.Children.Count > 0)
						{
							if (myEntity != ignoredEntity && TestQueryIntersection(myEntity.GetPhysicsBody().GetShape(), myEntity.WorldMatrix))
							{
								entityOverlap = true;
								if (touchingGrid == null)
								{
									touchingGrid = (myEntity as MyCubeGrid);
								}
								break;
							}
							foreach (MyPhysicsBody child in myEntity.GetPhysicsBody().WeldInfo.Children)
							{
								if (child.Entity != ignoredEntity && TestQueryIntersection(child.WeldedRigidBody.GetShape(), child.Entity.WorldMatrix))
								{
									if (touchingGrid == null)
									{
										touchingGrid = (child.Entity as MyCubeGrid);
									}
									entityOverlap = true;
									break;
								}
							}
							if (entityOverlap)
							{
								break;
							}
						}
						else
						{
							if (myCubeGrid == null || ((!flag || !myCubeGrid.IsStatic) && (!MyFakes.ENABLE_DYNAMIC_SMALL_GRID_MERGING || flag || myCubeGrid.IsStatic || blockDefinition == null || blockDefinition.CubeSize != myCubeGrid.GridSizeEnum) && (!flag || !myCubeGrid.IsStatic || blockDefinition == null || blockDefinition.CubeSize != myCubeGrid.GridSizeEnum)))
							{
								entityOverlap = true;
								break;
							}
							if (flag != myCubeGrid.IsStatic || num == myCubeGrid.GridSize)
							{
								if (!IsOrientationsAligned(myCubeGrid.WorldMatrix, worldMatrix))
								{
									entityOverlap = true;
								}
								else
								{
									TestGridPlacement(ref settings, ref worldMatrix, ref touchingGrid, num, flag, ref localAabb, blockDefinition, blockOrientation, ref entityOverlap, ref touchingStaticGrid, myCubeGrid);
									if (entityOverlap)
									{
										break;
									}
								}
							}
						}
					}
				}
			}
			m_tmpResultList.Clear();
			m_physicsBoxQueryList.Clear();
			if (entityOverlap)
			{
				return false;
			}
			return true;
		}

		private static bool IsOrientationsAligned(MatrixD transform1, MatrixD transform2)
		{
			double num = Vector3D.Dot(transform1.Forward, transform2.Forward);
			if ((num > 0.0010000000474974513 && num < 0.99899999995250255) || (num < -0.0010000000474974513 && num > -0.99899999995250255))
			{
				return false;
			}
			double num2 = Vector3D.Dot(transform1.Up, transform2.Up);
			if ((num2 > 0.0010000000474974513 && num2 < 0.99899999995250255) || (num2 < -0.0010000000474974513 && num2 > -0.99899999995250255))
			{
				return false;
			}
			double num3 = Vector3D.Dot(transform1.Right, transform2.Right);
			if ((num3 > 0.0010000000474974513 && num3 < 0.99899999995250255) || (num3 < -0.0010000000474974513 && num3 > -0.99899999995250255))
			{
				return false;
			}
			return true;
		}

		private static bool TestQueryIntersection(HkShape shape, MatrixD transform)
		{
			MatrixD lastQueryTransform = m_lastQueryTransform;
			MatrixD m = transform;
			m.Translation -= lastQueryTransform.Translation;
			lastQueryTransform.Translation = Vector3D.Zero;
			Matrix transform2 = lastQueryTransform;
			Matrix transform3 = m;
			return MyPhysics.IsPenetratingShapeShape(m_lastQueryBox.Value, ref transform2, shape, ref transform3);
		}

		public static bool TestPlacementVoxelMapOverlap(MyVoxelBase voxelMap, ref MyGridPlacementSettings settings, ref BoundingBoxD localAabb, ref MatrixD worldMatrix, bool touchingStaticGrid = false)
		{
			BoundingBoxD boundingBox = localAabb.TransformFast(ref worldMatrix);
			int num = 2;
			if (voxelMap == null)
			{
				voxelMap = MySession.Static.VoxelMaps.GetVoxelMapWhoseBoundingBoxIntersectsBox(ref boundingBox, null);
			}
			if (voxelMap != null && voxelMap.IsAnyAabbCornerInside(ref worldMatrix, localAabb))
			{
				num = 1;
			}
			bool result = true;
			switch (num)
			{
			case 1:
				result = (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.Both);
				break;
			case 2:
				result = (settings.VoxelPlacement.Value.PlacementMode == VoxelPlacementMode.OutsideVoxel || (settings.CanAnchorToStaticGrid && touchingStaticGrid));
				break;
			}
			return result;
		}

		private static bool TestPlacementVoxelMapPenetration(MyVoxelBase voxelMap, MyGridPlacementSettings settings, ref BoundingBoxD localAabb, ref MatrixD worldMatrix, bool touchingStaticGrid = false)
		{
			float num = 0f;
			if (voxelMap != null)
			{
				MyTuple<float, float> voxelContentInBoundingBox_Fast = voxelMap.GetVoxelContentInBoundingBox_Fast(localAabb, worldMatrix);
				_ = localAabb.Volume;
				num = (voxelContentInBoundingBox_Fast.Item2.IsValid() ? voxelContentInBoundingBox_Fast.Item2 : 0f);
			}
			if (num <= settings.VoxelPlacement.Value.MaxAllowed)
			{
				if (!(num >= settings.VoxelPlacement.Value.MinAllowed))
				{
					return settings.CanAnchorToStaticGrid && touchingStaticGrid;
				}
				return true;
			}
			return false;
		}

		public static void TransformMountPoints(List<MyCubeBlockDefinition.MountPoint> outMountPoints, MyCubeBlockDefinition def, MyCubeBlockDefinition.MountPoint[] mountPoints, ref MyBlockOrientation orientation)
		{
			outMountPoints.Clear();
			if (mountPoints != null)
			{
				orientation.GetMatrix(out Matrix result);
				Vector3I center = def.Center;
				for (int i = 0; i < mountPoints.Length; i++)
				{
					MyCubeBlockDefinition.MountPoint mountPoint = mountPoints[i];
					MyCubeBlockDefinition.MountPoint item = default(MyCubeBlockDefinition.MountPoint);
					Vector3 position = mountPoint.Start - center;
					Vector3 position2 = mountPoint.End - center;
					Vector3I.Transform(ref mountPoint.Normal, ref result, out item.Normal);
					Vector3.Transform(ref position, ref result, out item.Start);
					Vector3.Transform(ref position2, ref result, out item.End);
					item.ExclusionMask = mountPoint.ExclusionMask;
					item.PropertiesMask = mountPoint.PropertiesMask;
					item.Enabled = mountPoint.Enabled;
					Vector3I position3 = Vector3I.Floor(mountPoint.Start) - center;
					Vector3I position4 = Vector3I.Floor(mountPoint.End) - center;
					Vector3I.Transform(ref position3, ref result, out position3);
					Vector3I.Transform(ref position4, ref result, out position4);
					Vector3I b = Vector3I.Floor(item.Start);
					Vector3I b2 = Vector3I.Floor(item.End);
					Vector3I value = position3 - b;
					Vector3I value2 = position4 - b2;
					item.Start += (Vector3)value;
					item.End += (Vector3)value2;
					outMountPoints.Add(item);
				}
			}
		}

		internal static MyObjectBuilder_CubeBlock CreateBlockObjectBuilder(MyCubeBlockDefinition definition, Vector3I min, MyBlockOrientation orientation, long entityID, long owner, bool fullyBuilt)
		{
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = (MyObjectBuilder_CubeBlock)MyObjectBuilderSerializer.CreateNewObject(definition.Id);
			myObjectBuilder_CubeBlock.BuildPercent = (fullyBuilt ? 1f : 1.52590219E-05f);
			myObjectBuilder_CubeBlock.IntegrityPercent = (fullyBuilt ? 1f : 1.52590219E-05f);
			myObjectBuilder_CubeBlock.EntityId = entityID;
			myObjectBuilder_CubeBlock.Min = min;
			myObjectBuilder_CubeBlock.BlockOrientation = orientation;
			myObjectBuilder_CubeBlock.BuiltBy = owner;
			if (definition.ContainsComputer())
			{
				myObjectBuilder_CubeBlock.Owner = 0L;
				myObjectBuilder_CubeBlock.ShareMode = MyOwnershipShareModeEnum.All;
			}
			return myObjectBuilder_CubeBlock;
		}

		private static Vector3 ConvertVariantToHsvColor(Color variantColor)
		{
			switch (variantColor.PackedValue)
			{
			case 4278190335u:
				return MyRenderComponentBase.OldRedToHSV;
			case 4278255615u:
				return MyRenderComponentBase.OldYellowToHSV;
			case 4294901760u:
				return MyRenderComponentBase.OldBlueToHSV;
			case 4278222848u:
				return MyRenderComponentBase.OldGreenToHSV;
			case 4278190080u:
				return MyRenderComponentBase.OldBlackToHSV;
			case uint.MaxValue:
				return MyRenderComponentBase.OldWhiteToHSV;
			default:
				return MyRenderComponentBase.OldGrayToHSV;
			}
		}

		internal static MyObjectBuilder_CubeBlock FindDefinitionUpgrade(MyObjectBuilder_CubeBlock block, out MyCubeBlockDefinition blockDefinition)
		{
			foreach (MyCubeBlockDefinition item in MyDefinitionManager.Static.GetAllDefinitions().OfType<MyCubeBlockDefinition>())
			{
				if (item.Id.SubtypeId == block.SubtypeId && !string.IsNullOrEmpty(block.SubtypeId.String))
				{
					blockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(item.Id);
					return MyObjectBuilder_CubeBlock.Upgrade(block, blockDefinition.Id.TypeId, block.SubtypeName);
				}
			}
			blockDefinition = null;
			return null;
		}

		public static Vector3I StaticGlobalGrid_WorldToUGInt(Vector3D worldPos, float gridSize, bool staticGridAlignToCenter)
		{
			return Vector3I.Round(StaticGlobalGrid_WorldToUG(worldPos, gridSize, staticGridAlignToCenter));
		}

		public static Vector3D StaticGlobalGrid_WorldToUG(Vector3D worldPos, float gridSize, bool staticGridAlignToCenter)
		{
			Vector3D result = worldPos / gridSize;
			if (!staticGridAlignToCenter)
			{
				result += Vector3D.Half;
			}
			return result;
		}

		public static Vector3D StaticGlobalGrid_UGToWorld(Vector3D ugPos, float gridSize, bool staticGridAlignToCenter)
		{
			if (staticGridAlignToCenter)
			{
				return gridSize * ugPos;
			}
			return gridSize * (ugPos - Vector3D.Half);
		}

		private static Type ChooseGridSystemsType()
		{
			Type gridSystemsType = typeof(MyCubeGridSystems);
			ChooseGridSystemsType(ref gridSystemsType, MyPlugins.GameAssembly);
			ChooseGridSystemsType(ref gridSystemsType, MyPlugins.SandboxAssembly);
			ChooseGridSystemsType(ref gridSystemsType, MyPlugins.UserAssemblies);
			return gridSystemsType;
		}

		private static void ChooseGridSystemsType(ref Type gridSystemsType, Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					ChooseGridSystemsType(ref gridSystemsType, assembly);
				}
			}
		}

		private static void ChooseGridSystemsType(ref Type gridSystemsType, Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			int num = 0;
			Type type;
			while (true)
			{
				if (num < types.Length)
				{
					type = types[num];
					if (typeof(MyCubeGridSystems).IsAssignableFrom(type))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			gridSystemsType = type;
		}

		private static bool ShouldBeStatic(MyCubeGrid grid, MyTestDynamicReason testReason)
		{
			if (testReason == MyTestDynamicReason.NoReason)
			{
				return true;
			}
			if (grid.IsUnsupportedStation && testReason != MyTestDynamicReason.ConvertToShip)
			{
				return true;
			}
			if (grid.GridSizeEnum == MyCubeSize.Small && MyCubeGridSmallToLargeConnection.Static != null && MyCubeGridSmallToLargeConnection.Static.TestGridSmallToLargeConnection(grid))
			{
				return true;
			}
			if (testReason != MyTestDynamicReason.GridSplitByBlock && testReason != MyTestDynamicReason.ConvertToShip)
			{
				grid.RecalcBounds();
				MyGridPlacementSettings settings = default(MyGridPlacementSettings);
				VoxelPlacementSettings voxelPlacementSettings = default(VoxelPlacementSettings);
				voxelPlacementSettings.PlacementMode = VoxelPlacementMode.Volumetric;
				VoxelPlacementSettings value = voxelPlacementSettings;
				settings.VoxelPlacement = value;
				if (!IsAabbInsideVoxel(grid.WorldMatrix, grid.PositionComp.LocalAABB, settings))
				{
					return false;
				}
				if (grid.GetBlocks().Count > 1024)
				{
					return grid.IsStatic;
				}
			}
			BoundingBoxD box = grid.PositionComp.WorldAABB;
			if (MyGamePruningStructure.AnyVoxelMapInBox(ref box))
			{
				foreach (MySlimBlock block in grid.GetBlocks())
				{
					if (IsInVoxels(block))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsInVoxels(MySlimBlock block, bool checkForPhysics = true)
		{
			if (block.CubeGrid.Physics == null && checkForPhysics)
			{
				return false;
			}
			if (MyPerGameSettings.Destruction && block.CubeGrid.GridSizeEnum == MyCubeSize.Large)
			{
				return block.CubeGrid.Physics.Shape.BlocksConnectedToWorld.Contains(block.Position);
			}
			block.GetWorldBoundingBox(out BoundingBoxD aabb);
			m_tmpVoxelList.Clear();
			MyGamePruningStructure.GetAllVoxelMapsInBox(ref aabb, m_tmpVoxelList);
			float gridSize = block.CubeGrid.GridSize;
			BoundingBoxD aabb2 = new BoundingBoxD(gridSize * ((Vector3D)block.Min - 0.5), gridSize * ((Vector3D)block.Max + 0.5));
			MatrixD aabbWorldTransform = block.CubeGrid.WorldMatrix;
			foreach (MyVoxelBase tmpVoxel in m_tmpVoxelList)
			{
				if (tmpVoxel.IsAnyAabbCornerInside(ref aabbWorldTransform, aabb2))
				{
					return true;
				}
			}
			return false;
		}

		public static void CreateGridGroupLink(GridLinkTypeEnum type, long linkId, MyCubeGrid parent, MyCubeGrid child)
		{
			MyCubeGridGroups.Static.CreateLink(type, linkId, parent, child);
		}

		public static bool BreakGridGroupLink(GridLinkTypeEnum type, long linkId, MyCubeGrid parent, MyCubeGrid child)
		{
			return MyCubeGridGroups.Static.BreakLink(type, linkId, parent, child);
		}

		public static void KillAllCharacters(MyCubeGrid grid)
		{
			if (grid != null && Sync.IsServer)
			{
				foreach (MyCockpit fatBlock in grid.GetFatBlocks<MyCockpit>())
				{
					if (fatBlock != null && fatBlock.Pilot != null && !fatBlock.Pilot.IsDead)
					{
						fatBlock.Pilot.DoDamage(1000f, MyDamageType.Suicide, updateSync: true, fatBlock.Pilot.EntityId);
						fatBlock.RemovePilot();
					}
				}
			}
		}

		public static void ResetInfoGizmos()
		{
			ShowSenzorGizmos = false;
			ShowGravityGizmos = false;
			ShowCenterOfMass = false;
			ShowGridPivot = false;
			ShowAntennaGizmos = false;
		}

		VRage.Game.ModAPI.IMySlimBlock VRage.Game.ModAPI.IMyCubeGrid.AddBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge)
		{
			return AddBlock(objectBuilder, testMerge);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.ApplyDestructionDeformation(VRage.Game.ModAPI.IMySlimBlock block)
		{
			if (block is MySlimBlock)
			{
				ApplyDestructionDeformation(block as MySlimBlock, 1f, null, 0L);
			}
		}

		MatrixI VRage.Game.ModAPI.IMyCubeGrid.CalculateMergeTransform(VRage.Game.ModAPI.IMyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			if (gridToMerge is MyCubeGrid)
			{
				return CalculateMergeTransform(gridToMerge as MyCubeGrid, gridOffset);
			}
			return default(MatrixI);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.CanMergeCubes(VRage.Game.ModAPI.IMyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			if (gridToMerge is MyCubeGrid)
			{
				return CanMergeCubes(gridToMerge as MyCubeGrid, gridOffset);
			}
			return false;
		}

		void VRage.Game.ModAPI.IMyCubeGrid.GetBlocks(List<VRage.Game.ModAPI.IMySlimBlock> blocks, Func<VRage.Game.ModAPI.IMySlimBlock, bool> collect)
		{
			foreach (MySlimBlock block in GetBlocks())
			{
				if (collect == null || collect(block))
				{
					blocks.Add(block);
				}
			}
		}

		List<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMyCubeGrid.GetBlocksInsideSphere(ref BoundingSphereD sphere)
		{
			HashSet<MySlimBlock> hashSet = new HashSet<MySlimBlock>();
			GetBlocksInsideSphere(ref sphere, hashSet);
			List<VRage.Game.ModAPI.IMySlimBlock> list = new List<VRage.Game.ModAPI.IMySlimBlock>(hashSet.Count);
			foreach (MySlimBlock item in hashSet)
			{
				list.Add(item);
			}
			return list;
		}

		VRage.Game.ModAPI.IMySlimBlock VRage.Game.ModAPI.IMyCubeGrid.GetCubeBlock(Vector3I pos)
		{
			return GetCubeBlock(pos);
		}

		bool VRage.Game.ModAPI.Ingame.IMyCubeGrid.IsSameConstructAs(VRage.Game.ModAPI.Ingame.IMyCubeGrid other)
		{
			return IsSameConstructAs((MyCubeGrid)other);
		}

		Vector3D? VRage.Game.ModAPI.IMyCubeGrid.GetLineIntersectionExactAll(ref LineD line, out double distance, out VRage.Game.ModAPI.IMySlimBlock intersectedBlock)
		{
			MySlimBlock intersectedBlock2;
			Vector3D? lineIntersectionExactAll = GetLineIntersectionExactAll(ref line, out distance, out intersectedBlock2);
			intersectedBlock = intersectedBlock2;
			return lineIntersectionExactAll;
		}

		VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeGrid.MergeGrid_MergeBlock(VRage.Game.ModAPI.IMyCubeGrid gridToMerge, Vector3I gridOffset)
		{
			if (gridToMerge is MyCubeGrid)
			{
				return MergeGrid_MergeBlock(gridToMerge as MyCubeGrid, gridOffset);
			}
			return null;
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RemoveBlock(VRage.Game.ModAPI.IMySlimBlock block, bool updatePhysics)
		{
			if (block is MySlimBlock)
			{
				RemoveBlock(block as MySlimBlock, updatePhysics);
			}
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RemoveDestroyedBlock(VRage.Game.ModAPI.IMySlimBlock block)
		{
			if (block is MySlimBlock)
			{
				RemoveDestroyedBlock(block as MySlimBlock, 0L);
			}
		}

		void VRage.Game.ModAPI.IMyCubeGrid.UpdateBlockNeighbours(VRage.Game.ModAPI.IMySlimBlock block)
		{
			if (block is MySlimBlock)
			{
				UpdateBlockNeighbours(block as MySlimBlock);
			}
		}

		void VRage.Game.ModAPI.IMyCubeGrid.ChangeGridOwnership(long playerId, MyOwnershipShareModeEnum shareMode)
		{
			ChangeGridOwnership(playerId, shareMode);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.ClearSymmetries()
		{
			ClearSymmetries();
		}

		void VRage.Game.ModAPI.IMyCubeGrid.ColorBlocks(Vector3I min, Vector3I max, Vector3 newHSV)
		{
			ColorBlocks(min, max, newHSV, playSound: false, validateOwnership: false);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.SkinBlocks(Vector3I min, Vector3I max, Vector3? newHSV, string newSkin)
		{
			SkinBlocks(min, max, newHSV, MyStringHash.GetOrCompute(newSkin), playSound: false, validateOwnership: false);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.FixTargetCube(out Vector3I cube, Vector3 fractionalGridPosition)
		{
			FixTargetCube(out cube, fractionalGridPosition);
		}

		Vector3 VRage.Game.ModAPI.IMyCubeGrid.GetClosestCorner(Vector3I gridPos, Vector3 position)
		{
			return GetClosestCorner(gridPos, position);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.GetLineIntersectionExactGrid(ref LineD line, ref Vector3I position, ref double distanceSquared)
		{
			return GetLineIntersectionExactGrid(ref line, ref position, ref distanceSquared);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.IsTouchingAnyNeighbor(Vector3I min, Vector3I max)
		{
			return IsTouchingAnyNeighbor(min, max);
		}

		Vector3I? VRage.Game.ModAPI.IMyCubeGrid.RayCastBlocks(Vector3D worldStart, Vector3D worldEnd)
		{
			return RayCastBlocks(worldStart, worldEnd);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RayCastCells(Vector3D worldStart, Vector3D worldEnd, List<Vector3I> outHitPositions, Vector3I? gridSizeInflate, bool havokWorld)
		{
			RayCastCells(worldStart, worldEnd, outHitPositions, gridSizeInflate, havokWorld);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RazeBlock(Vector3I position)
		{
			RazeBlock(position, 0uL);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RazeBlocks(ref Vector3I pos, ref Vector3UByte size)
		{
			RazeBlocks(ref pos, ref size, 0L);
		}

		void VRage.Game.ModAPI.IMyCubeGrid.RazeBlocks(List<Vector3I> locations)
		{
			RazeBlocks(locations, 0L, 0uL);
		}

		Vector3I VRage.Game.ModAPI.IMyCubeGrid.WorldToGridInteger(Vector3D coords)
		{
			return WorldToGridInteger(coords);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.WillRemoveBlockSplitGrid(VRage.Game.ModAPI.IMySlimBlock testBlock)
		{
			return WillRemoveBlockSplitGrid((MySlimBlock)testBlock);
		}

		private Action<MySlimBlock> GetDelegate(Action<VRage.Game.ModAPI.IMySlimBlock> value)
		{
			return (Action<MySlimBlock>)Delegate.CreateDelegate(typeof(Action<MySlimBlock>), value.Target, value.Method);
		}

		private Action<MyCubeGrid> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeGrid> value)
		{
			return (Action<MyCubeGrid>)Delegate.CreateDelegate(typeof(Action<MyCubeGrid>), value.Target, value.Method);
		}

		private Action<MyCubeGrid, MyCubeGrid> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeGrid, VRage.Game.ModAPI.IMyCubeGrid> value)
		{
			return (Action<MyCubeGrid, MyCubeGrid>)Delegate.CreateDelegate(typeof(Action<MyCubeGrid, MyCubeGrid>), value.Target, value.Method);
		}

		private Action<MyCubeGrid, bool> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeGrid, bool> value)
		{
			return (Action<MyCubeGrid, bool>)Delegate.CreateDelegate(typeof(Action<MyCubeGrid, bool>), value.Target, value.Method);
		}

		VRage.Game.ModAPI.Ingame.IMySlimBlock VRage.Game.ModAPI.Ingame.IMyCubeGrid.GetCubeBlock(Vector3I position)
		{
			VRage.Game.ModAPI.Ingame.IMySlimBlock cubeBlock = GetCubeBlock(position);
			if (cubeBlock != null && cubeBlock.FatBlock != null && cubeBlock.FatBlock is MyTerminalBlock && (cubeBlock.FatBlock as MyTerminalBlock).IsAccessibleForProgrammableBlock)
			{
				return cubeBlock;
			}
			return null;
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.CanAddCube(Vector3I pos)
		{
			return CanAddCube(pos, null, null);
		}

		bool VRage.Game.ModAPI.IMyCubeGrid.CanAddCubes(Vector3I min, Vector3I max)
		{
			return CanAddCubes(min, max);
		}

		VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeGrid.SplitByPlane(PlaneD plane)
		{
			return SplitByPlane(plane);
		}

		VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeGrid.Split(List<VRage.Game.ModAPI.IMySlimBlock> blocks, bool sync)
		{
			return CreateSplit(this, blocks.ConvertAll((VRage.Game.ModAPI.IMySlimBlock x) => (MySlimBlock)x), sync, 0L);
		}

		public bool IsInSameLogicalGroupAs(VRage.Game.ModAPI.IMyCubeGrid other)
		{
			if (this != other)
			{
				return MyCubeGridGroups.Static.Logical.GetGroup(this) == MyCubeGridGroups.Static.Logical.GetGroup((MyCubeGrid)other);
			}
			return true;
		}

		public bool IsSameConstructAs(VRage.Game.ModAPI.IMyCubeGrid other)
		{
			if (this != other)
			{
				return MyCubeGridGroups.Static.Mechanical.GetGroup(this) == MyCubeGridGroups.Static.Mechanical.GetGroup((MyCubeGrid)other);
			}
			return true;
		}

		public bool IsRoomAtPositionAirtight(Vector3I pos)
		{
			return GridSystems.GasSystem.GetOxygenRoomForCubeGridPosition(ref pos)?.IsAirtight ?? false;
		}
	}
}
