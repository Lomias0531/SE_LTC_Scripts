using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	[StaticEventOwner]
	[GenerateFieldAccessors]
	[MyCubeBlockType(typeof(MyObjectBuilder_CubeBlock))]
	public class MySlimBlock : IMyDestroyableObject, IMyDecalProxy, VRage.Game.ModAPI.IMySlimBlock, VRage.Game.ModAPI.Ingame.IMySlimBlock
	{
		[ProtoContract]
		public struct DoDamageSlimBlockMsg
		{
			protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDoDamageSlimBlockMsg_003C_003EGridEntityId_003C_003EAccessor : IMemberAccessor<DoDamageSlimBlockMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref DoDamageSlimBlockMsg owner, in long value)
				{
					owner.GridEntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref DoDamageSlimBlockMsg owner, out long value)
				{
					value = owner.GridEntityId;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDoDamageSlimBlockMsg_003C_003EPosition_003C_003EAccessor : IMemberAccessor<DoDamageSlimBlockMsg, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref DoDamageSlimBlockMsg owner, in Vector3I value)
				{
					owner.Position = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref DoDamageSlimBlockMsg owner, out Vector3I value)
				{
					value = owner.Position;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDoDamageSlimBlockMsg_003C_003EDamage_003C_003EAccessor : IMemberAccessor<DoDamageSlimBlockMsg, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref DoDamageSlimBlockMsg owner, in float value)
				{
					owner.Damage = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref DoDamageSlimBlockMsg owner, out float value)
				{
					value = owner.Damage;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDoDamageSlimBlockMsg_003C_003EType_003C_003EAccessor : IMemberAccessor<DoDamageSlimBlockMsg, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref DoDamageSlimBlockMsg owner, in MyStringHash value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref DoDamageSlimBlockMsg owner, out MyStringHash value)
				{
					value = owner.Type;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDoDamageSlimBlockMsg_003C_003EHitInfo_003C_003EAccessor : IMemberAccessor<DoDamageSlimBlockMsg, MyHitInfo?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref DoDamageSlimBlockMsg owner, in MyHitInfo? value)
				{
					owner.HitInfo = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref DoDamageSlimBlockMsg owner, out MyHitInfo? value)
				{
					value = owner.HitInfo;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDoDamageSlimBlockMsg_003C_003EAttackerEntityId_003C_003EAccessor : IMemberAccessor<DoDamageSlimBlockMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref DoDamageSlimBlockMsg owner, in long value)
				{
					owner.AttackerEntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref DoDamageSlimBlockMsg owner, out long value)
				{
					value = owner.AttackerEntityId;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDoDamageSlimBlockMsg_003C_003ECompoundBlockId_003C_003EAccessor : IMemberAccessor<DoDamageSlimBlockMsg, uint>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref DoDamageSlimBlockMsg owner, in uint value)
				{
					owner.CompoundBlockId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref DoDamageSlimBlockMsg owner, out uint value)
				{
					value = owner.CompoundBlockId;
				}
			}

			private class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDoDamageSlimBlockMsg_003C_003EActor : IActivator, IActivator<DoDamageSlimBlockMsg>
			{
				private sealed override object CreateInstance()
				{
					return default(DoDamageSlimBlockMsg);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override DoDamageSlimBlockMsg CreateInstance()
				{
					return (DoDamageSlimBlockMsg)(object)default(DoDamageSlimBlockMsg);
				}

				DoDamageSlimBlockMsg IActivator<DoDamageSlimBlockMsg>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long GridEntityId;

			[ProtoMember(4)]
			public Vector3I Position;

			[ProtoMember(7)]
			public float Damage;

			[ProtoMember(10)]
			public MyStringHash Type;

			[ProtoMember(13)]
			public MyHitInfo? HitInfo;

			[ProtoMember(16)]
			public long AttackerEntityId;

			[ProtoMember(19)]
			public uint CompoundBlockId;
		}

		protected sealed class DoDamageSlimBlockBatch_003C_003ESystem_Int64_0023System_Collections_Generic_List_00601_003CVRage_MyTuple_00602_003CVRageMath_Vector3I_0023System_Single_003E_003E_0023VRage_Utils_MyStringHash_0023System_Int64 : ICallSite<IMyEventOwner, long, List<MyTuple<Vector3I, float>>, MyStringHash, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridId, in List<MyTuple<Vector3I, float>> blocks, in MyStringHash damageType, in long attackerId, in DBNull arg5, in DBNull arg6)
			{
				DoDamageSlimBlockBatch(gridId, blocks, damageType, attackerId);
			}
		}

		protected sealed class DoDamageSlimBlock_003C_003ESandbox_Game_Entities_Cube_MySlimBlock_003C_003EDoDamageSlimBlockMsg : ICallSite<IMyEventOwner, DoDamageSlimBlockMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DoDamageSlimBlockMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				DoDamageSlimBlock(msg);
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_accumulatedDamage_003C_003EAccessor : IMemberAccessor<MySlimBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in float value)
			{
				owner.m_accumulatedDamage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out float value)
			{
				value = owner.m_accumulatedDamage;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EBlockDefinition_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyCubeBlockDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyCubeBlockDefinition value)
			{
				owner.BlockDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyCubeBlockDefinition value)
			{
				value = owner.BlockDefinition;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EMin_003C_003EAccessor : IMemberAccessor<MySlimBlock, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in Vector3I value)
			{
				owner.Min = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out Vector3I value)
			{
				value = owner.Min;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EMax_003C_003EAccessor : IMemberAccessor<MySlimBlock, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in Vector3I value)
			{
				owner.Max = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out Vector3I value)
			{
				value = owner.Max;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EOrientation_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyBlockOrientation>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyBlockOrientation value)
			{
				owner.Orientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyBlockOrientation value)
			{
				value = owner.Orientation;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MySlimBlock, Vector3I>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in Vector3I value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out Vector3I value)
			{
				value = owner.Position;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EBlockGeneralDamageModifier_003C_003EAccessor : IMemberAccessor<MySlimBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in float value)
			{
				owner.BlockGeneralDamageModifier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out float value)
			{
				value = owner.BlockGeneralDamageModifier;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_cubeGrid_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyCubeGrid>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyCubeGrid value)
			{
				owner.m_cubeGrid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyCubeGrid value)
			{
				value = owner.m_cubeGrid;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_colorMaskHSV_003C_003EAccessor : IMemberAccessor<MySlimBlock, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in Vector3 value)
			{
				owner.m_colorMaskHSV = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out Vector3 value)
			{
				value = owner.m_colorMaskHSV;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003ESkinSubtypeId_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyStringHash value)
			{
				owner.SkinSubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyStringHash value)
			{
				value = owner.SkinSubtypeId;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDithering_003C_003EAccessor : IMemberAccessor<MySlimBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in float value)
			{
				owner.Dithering = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out float value)
			{
				value = owner.Dithering;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EUsesDeformation_003C_003EAccessor : IMemberAccessor<MySlimBlock, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in bool value)
			{
				owner.UsesDeformation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out bool value)
			{
				value = owner.UsesDeformation;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDeformationRatio_003C_003EAccessor : IMemberAccessor<MySlimBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in float value)
			{
				owner.DeformationRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out float value)
			{
				value = owner.DeformationRatio;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_componentStack_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyComponentStack>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyComponentStack value)
			{
				owner.m_componentStack = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyComponentStack value)
			{
				value = owner.m_componentStack;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_stockpile_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyConstructionStockpile>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyConstructionStockpile value)
			{
				owner.m_stockpile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyConstructionStockpile value)
			{
				value = owner.m_stockpile;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_cachedMaxDeformation_003C_003EAccessor : IMemberAccessor<MySlimBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in float value)
			{
				owner.m_cachedMaxDeformation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out float value)
			{
				value = owner.m_cachedMaxDeformation;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_builtByID_003C_003EAccessor : IMemberAccessor<MySlimBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in long value)
			{
				owner.m_builtByID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out long value)
			{
				value = owner.m_builtByID;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003ENeighbours_003C_003EAccessor : IMemberAccessor<MySlimBlock, List<MySlimBlock>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in List<MySlimBlock> value)
			{
				owner.Neighbours = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out List<MySlimBlock> value)
			{
				value = owner.Neighbours;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_lastDamage_003C_003EAccessor : IMemberAccessor<MySlimBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in float value)
			{
				owner.m_lastDamage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out float value)
			{
				value = owner.m_lastDamage;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_lastAttackerId_003C_003EAccessor : IMemberAccessor<MySlimBlock, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in long value)
			{
				owner.m_lastAttackerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out long value)
			{
				value = owner.m_lastAttackerId;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_lastDamageType_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyStringHash value)
			{
				owner.m_lastDamageType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyStringHash value)
			{
				value = owner.m_lastDamageType;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003Em_isFunctionalChanged_003C_003EAccessor : IMemberAccessor<MySlimBlock, Action<MySlimBlock>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in Action<MySlimBlock> value)
			{
				owner.m_isFunctionalChanged = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out Action<MySlimBlock> value)
			{
				value = owner.m_isFunctionalChanged;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EMultiBlockDefinition_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyMultiBlockDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyMultiBlockDefinition value)
			{
				owner.MultiBlockDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyMultiBlockDefinition value)
			{
				value = owner.MultiBlockDefinition;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EMultiBlockId_003C_003EAccessor : IMemberAccessor<MySlimBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in int value)
			{
				owner.MultiBlockId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out int value)
			{
				value = owner.MultiBlockId;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EMultiBlockIndex_003C_003EAccessor : IMemberAccessor<MySlimBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in int value)
			{
				owner.MultiBlockIndex = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out int value)
			{
				value = owner.MultiBlockIndex;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EDisconnectFaces_003C_003EAccessor : IMemberAccessor<MySlimBlock, List<Vector3I>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in List<Vector3I> value)
			{
				owner.DisconnectFaces = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out List<Vector3I> value)
			{
				value = owner.DisconnectFaces;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EAccumulatedDamage_003C_003EAccessor : IMemberAccessor<MySlimBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in float value)
			{
				owner.AccumulatedDamage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out float value)
			{
				value = owner.AccumulatedDamage;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EFatBlock_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyCubeBlock>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyCubeBlock value)
			{
				owner.FatBlock = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyCubeBlock value)
			{
				value = owner.FatBlock;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003ECubeGrid_003C_003EAccessor : IMemberAccessor<MySlimBlock, MyCubeGrid>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in MyCubeGrid value)
			{
				owner.CubeGrid = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out MyCubeGrid value)
			{
				value = owner.CubeGrid;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EColorMaskHSV_003C_003EAccessor : IMemberAccessor<MySlimBlock, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in Vector3 value)
			{
				owner.ColorMaskHSV = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out Vector3 value)
			{
				value = owner.ColorMaskHSV;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EShowParts_003C_003EAccessor : IMemberAccessor<MySlimBlock, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in bool value)
			{
				owner.ShowParts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out bool value)
			{
				value = owner.ShowParts;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EUseDamageSystem_003C_003EAccessor : IMemberAccessor<MySlimBlock, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in bool value)
			{
				owner.UseDamageSystem = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out bool value)
			{
				value = owner.UseDamageSystem;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EUniqueId_003C_003EAccessor : IMemberAccessor<MySlimBlock, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in int value)
			{
				owner.UniqueId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out int value)
			{
				value = owner.UniqueId;
			}
		}

		protected class Sandbox_Game_Entities_Cube_MySlimBlock_003C_003EVRage_002EGame_002EModAPI_002EIMySlimBlock_002EDithering_003C_003EAccessor : IMemberAccessor<MySlimBlock, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySlimBlock owner, in float value)
			{
				owner.VRage_002EGame_002EModAPI_002EIMySlimBlock_002EDithering = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySlimBlock owner, out float value)
			{
				value = owner.VRage_002EGame_002EModAPI_002EIMySlimBlock_002EDithering;
			}
		}

		private static List<VertexArealBoneIndexWeight> m_boneIndexWeightTmp;

		private static MySoundPair CONSTRUCTION_START = new MySoundPair("PrgConstrPh01Start");

		private static MySoundPair CONSTRUCTION_PROG = new MySoundPair("PrgConstrPh02Proc");

		private static MySoundPair CONSTRUCTION_END = new MySoundPair("PrgConstrPh03Fin");

		private static MySoundPair DECONSTRUCTION_START = new MySoundPair("PrgDeconstrPh01Start");

		private static MySoundPair DECONSTRUCTION_PROG = new MySoundPair("PrgDeconstrPh02Proc");

		private static MySoundPair DECONSTRUCTION_END = new MySoundPair("PrgDeconstrPh03Fin");

		[ThreadStatic]
		private static Dictionary<string, int> m_tmpComponentsPerThread;

		[ThreadStatic]
		private static List<MyStockpileItem> m_tmpItemListPerThread;

		[ThreadStatic]
		private static List<Vector3I> m_tmpCubeNeighboursPerThread;

		[ThreadStatic]
		private static List<MySlimBlock> m_tmpBlocksPerThread;

		[ThreadStatic]
		private static List<MySlimBlock> m_tmpMultiBlocksPerThread;

		public static readonly MyTimedItemCache ConstructionParticlesTimedCache = new MyTimedItemCache(350);

		public static double ConstructionParticleSpaceMapping = 1.0;

		private float m_accumulatedDamage;

		public MyCubeBlockDefinition BlockDefinition;

		public Vector3I Min;

		public Vector3I Max;

		public MyBlockOrientation Orientation = MyBlockOrientation.Identity;

		public Vector3I Position;

		public float BlockGeneralDamageModifier = 1f;

		private MyCubeGrid m_cubeGrid;

		private Vector3 m_colorMaskHSV;

		public MyStringHash SkinSubtypeId;

		public float Dithering;

		public bool UsesDeformation = true;

		public float DeformationRatio = 1f;

		private MyComponentStack m_componentStack;

		private MyConstructionStockpile m_stockpile;

		private float m_cachedMaxDeformation;

		private long m_builtByID;

		public List<MySlimBlock> Neighbours = new List<MySlimBlock>();

		public float m_lastDamage;

		public long m_lastAttackerId;

		public MyStringHash m_lastDamageType = MyDamageType.Unknown;

		private Action<MySlimBlock> m_isFunctionalChanged;

		public MyMultiBlockDefinition MultiBlockDefinition;

		public int MultiBlockId;

		public int MultiBlockIndex = -1;

		private static readonly Dictionary<string, int> m_modelTotalFracturesCount = new Dictionary<string, int>();

		public List<Vector3I> DisconnectFaces = new List<Vector3I>();

		[ThreadStatic]
		private static List<uint> m_tmpIds;

		[ThreadStatic]
		private static List<MyTuple<Vector3I, float>> m_batchCache;

		private static Dictionary<string, int> m_tmpComponents => MyUtils.Init(ref m_tmpComponentsPerThread);

		private static List<MyStockpileItem> m_tmpItemList => MyUtils.Init(ref m_tmpItemListPerThread);

		private static List<Vector3I> m_tmpCubeNeighbours => MyUtils.Init(ref m_tmpCubeNeighboursPerThread);

		private static List<MySlimBlock> m_tmpBlocks => MyUtils.Init(ref m_tmpBlocksPerThread);

		private static List<MySlimBlock> m_tmpMultiBlocks => MyUtils.Init(ref m_tmpMultiBlocksPerThread);

		public float AccumulatedDamage
		{
			get
			{
				return m_accumulatedDamage;
			}
			private set
			{
				m_accumulatedDamage = value;
				if (m_accumulatedDamage > 0f)
				{
					CubeGrid.AddForDamageApplication(this);
				}
			}
		}

		public MyCubeBlock FatBlock
		{
			get;
			private set;
		}

		public Vector3D WorldPosition => CubeGrid.GridIntegerToWorld(Position);

		public BoundingBoxD WorldAABB => new BoundingBoxD(Min * CubeGrid.GridSize - CubeGrid.GridSizeHalfVector, Max * CubeGrid.GridSize + CubeGrid.GridSizeHalfVector).TransformFast(CubeGrid.PositionComp.WorldMatrix);

		public MyCubeGrid CubeGrid
		{
			get
			{
				return m_cubeGrid;
			}
			set
			{
				if (m_cubeGrid == value)
				{
					return;
				}
				bool flag = m_cubeGrid == null;
				MyCubeGrid cubeGrid = m_cubeGrid;
				m_cubeGrid = value;
				if (FatBlock != null && !flag)
				{
					FatBlock.OnCubeGridChanged(cubeGrid);
					if (this.CubeGridChanged != null)
					{
						this.CubeGridChanged(this, cubeGrid);
					}
				}
			}
		}

		public Vector3 ColorMaskHSV
		{
			get
			{
				return m_colorMaskHSV;
			}
			set
			{
				m_colorMaskHSV = value;
			}
		}

		public bool ShowParts
		{
			get;
			private set;
		}

		public bool IsFullIntegrity
		{
			get
			{
				if (m_componentStack != null)
				{
					return m_componentStack.IsFullIntegrity;
				}
				return true;
			}
		}

		public float BuildLevelRatio => m_componentStack.BuildRatio;

		public float BuildIntegrity => m_componentStack.BuildIntegrity;

		public bool IsFullyDismounted => m_componentStack.IsFullyDismounted;

		public bool IsDestroyed => m_componentStack.IsDestroyed;

		public bool UseDamageSystem
		{
			get;
			private set;
		}

		public float Integrity => m_componentStack.Integrity;

		public float MaxIntegrity => m_componentStack.MaxIntegrity;

		public float CurrentDamage => BuildIntegrity - Integrity;

		public float DamageRatio => 2f - m_componentStack.BuildIntegrity / MaxIntegrity;

		public bool StockpileAllocated => m_stockpile != null;

		public bool StockpileEmpty
		{
			get
			{
				if (StockpileAllocated)
				{
					return m_stockpile.IsEmpty();
				}
				return true;
			}
		}

		public bool HasDeformation
		{
			get
			{
				if (CubeGrid != null)
				{
					return CubeGrid.Skeleton.IsDeformed(Position, 0f, CubeGrid, checkBlockDefinition: true);
				}
				return false;
			}
		}

		public float MaxDeformation => m_cachedMaxDeformation;

		public MyComponentStack ComponentStack => m_componentStack;

		public bool YieldLastComponent => m_componentStack.YieldLastComponent;

		public long BuiltBy => m_builtByID;

		public int UniqueId
		{
			get;
			private set;
		}

		public bool IsMultiBlockPart
		{
			get
			{
				if (MyFakes.ENABLE_MULTIBLOCK_PART_IDS && MultiBlockId != 0 && MultiBlockDefinition != null)
				{
					return MultiBlockIndex != -1;
				}
				return false;
			}
		}

		public bool ForceBlockDestructible
		{
			get
			{
				if (FatBlock == null)
				{
					return false;
				}
				return FatBlock.ForceBlockDestructible;
			}
		}

		public long OwnerId
		{
			get
			{
				if (FatBlock != null && FatBlock.OwnerId != 0L)
				{
					return FatBlock.OwnerId;
				}
				CubeGrid.Components.TryGet(out MyGridOwnershipComponentBase component);
				return component?.GetBlockOwnerId(this) ?? 0;
			}
		}

		float IMyDestroyableObject.Integrity => Integrity;

		VRage.Game.ModAPI.IMyCubeBlock VRage.Game.ModAPI.IMySlimBlock.FatBlock => FatBlock;

		VRage.Game.ModAPI.Ingame.IMyCubeBlock VRage.Game.ModAPI.Ingame.IMySlimBlock.FatBlock => FatBlock;

		float VRage.Game.ModAPI.Ingame.IMySlimBlock.AccumulatedDamage => AccumulatedDamage;

		float VRage.Game.ModAPI.Ingame.IMySlimBlock.BuildIntegrity => BuildIntegrity;

		float VRage.Game.ModAPI.Ingame.IMySlimBlock.BuildLevelRatio => BuildLevelRatio;

		float VRage.Game.ModAPI.Ingame.IMySlimBlock.CurrentDamage => CurrentDamage;

		float VRage.Game.ModAPI.Ingame.IMySlimBlock.DamageRatio => DamageRatio;

		bool VRage.Game.ModAPI.Ingame.IMySlimBlock.HasDeformation => HasDeformation;

		bool VRage.Game.ModAPI.Ingame.IMySlimBlock.IsDestroyed => IsDestroyed;

		bool VRage.Game.ModAPI.Ingame.IMySlimBlock.IsFullIntegrity => IsFullIntegrity;

		bool VRage.Game.ModAPI.Ingame.IMySlimBlock.IsFullyDismounted => IsFullyDismounted;

		float VRage.Game.ModAPI.Ingame.IMySlimBlock.MaxDeformation => MaxDeformation;

		float VRage.Game.ModAPI.Ingame.IMySlimBlock.MaxIntegrity => MaxIntegrity;

		float VRage.Game.ModAPI.Ingame.IMySlimBlock.Mass => GetMass();

		bool VRage.Game.ModAPI.Ingame.IMySlimBlock.ShowParts => ShowParts;

		bool VRage.Game.ModAPI.Ingame.IMySlimBlock.StockpileAllocated => StockpileAllocated;

		bool VRage.Game.ModAPI.Ingame.IMySlimBlock.StockpileEmpty => StockpileEmpty;

		Vector3I VRage.Game.ModAPI.Ingame.IMySlimBlock.Position => Position;

		VRage.Game.ModAPI.Ingame.IMyCubeGrid VRage.Game.ModAPI.Ingame.IMySlimBlock.CubeGrid => CubeGrid;

		Vector3 VRage.Game.ModAPI.Ingame.IMySlimBlock.ColorMaskHSV => ColorMaskHSV;

		MyStringHash VRage.Game.ModAPI.Ingame.IMySlimBlock.SkinSubtypeId => SkinSubtypeId;

		VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMySlimBlock.CubeGrid => CubeGrid;

		MyDefinitionBase VRage.Game.ModAPI.IMySlimBlock.BlockDefinition => BlockDefinition;

		Vector3I VRage.Game.ModAPI.IMySlimBlock.Max => Max;

		Vector3I VRage.Game.ModAPI.IMySlimBlock.Min => Min;

		MyBlockOrientation VRage.Game.ModAPI.IMySlimBlock.Orientation => Orientation;

		List<VRage.Game.ModAPI.IMySlimBlock> VRage.Game.ModAPI.IMySlimBlock.Neighbours => Neighbours.Cast<VRage.Game.ModAPI.IMySlimBlock>().ToList();

		float VRage.Game.ModAPI.IMySlimBlock.Dithering
		{
			get
			{
				return Dithering;
			}
			set
			{
				Dithering = value;
				UpdateVisual(updatePhysics: false);
			}
		}

		long VRage.Game.ModAPI.Ingame.IMySlimBlock.OwnerId => OwnerId;

		SerializableDefinitionId VRage.Game.ModAPI.Ingame.IMySlimBlock.BlockDefinition => BlockDefinition.Id;

		long VRage.Game.ModAPI.IMySlimBlock.BuiltBy => m_builtByID;

		public static event Action<MyTerminalBlock, long> OnAnyBlockHackedChanged;

		public event Action<MySlimBlock, MyCubeGrid> CubeGridChanged;

		public int GetStockpileStamp()
		{
			if (m_stockpile == null)
			{
				return 0;
			}
			return m_stockpile.LastChangeStamp;
		}

		public void SubscribeForIsFunctionalChanged(Action<MySlimBlock> callback)
		{
			if (m_isFunctionalChanged == null && callback != null)
			{
				ComponentStack.IsFunctionalChanged += IsFunctionalChanged;
			}
			m_isFunctionalChanged = (Action<MySlimBlock>)Delegate.Combine(m_isFunctionalChanged, callback);
		}

		private void IsFunctionalChanged()
		{
			if (m_isFunctionalChanged != null)
			{
				m_isFunctionalChanged(this);
			}
		}

		public void UnsubscribeFromIsFunctionalChanged(Action<MySlimBlock> callback)
		{
			if (m_isFunctionalChanged != null)
			{
				m_isFunctionalChanged = (Action<MySlimBlock>)Delegate.Remove(m_isFunctionalChanged, callback);
				if (m_isFunctionalChanged == null)
				{
					ComponentStack.IsFunctionalChanged -= IsFunctionalChanged;
				}
			}
		}

		public MySlimBlock()
		{
			UniqueId = MyRandom.Instance.Next();
			UseDamageSystem = true;
		}

		public void DisableLastComponentYield()
		{
			m_componentStack.DisableLastComponentYield();
		}

		public bool Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid, MyCubeBlock fatBlock)
		{
			FatBlock = fatBlock;
			if (objectBuilder is MyObjectBuilder_CompoundCubeBlock)
			{
				BlockDefinition = MyCompoundCubeBlock.GetCompoundCubeBlockDefinition();
			}
			else if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(objectBuilder.GetId(), out BlockDefinition))
			{
				return false;
			}
			if (BlockDefinition == null)
			{
				return false;
			}
			if (BlockDefinition.CubeSize != cubeGrid.GridSizeEnum && !MySession.Static.Settings.EnableSupergridding)
			{
				return false;
			}
			m_componentStack = new MyComponentStack(BlockDefinition, objectBuilder.IntegrityPercent, objectBuilder.BuildPercent);
			m_componentStack.IsFunctionalChanged += m_componentStack_IsFunctionalChanged;
			if (MyCubeGridDefinitions.GetCubeRotationOptions(BlockDefinition) == MyRotationOptionsEnum.None)
			{
				objectBuilder.BlockOrientation = MyBlockOrientation.Identity;
			}
			UsesDeformation = BlockDefinition.UsesDeformation;
			DeformationRatio = BlockDefinition.DeformationRatio;
			Min = objectBuilder.Min;
			Orientation = objectBuilder.BlockOrientation;
			if (!Orientation.IsValid)
			{
				Orientation = MyBlockOrientation.Identity;
			}
			CubeGrid = cubeGrid;
			ColorMaskHSV = objectBuilder.ColorMaskHSV;
			SkinSubtypeId = MyStringHash.GetOrCompute(objectBuilder.SkinSubtypeId);
			if (BlockDefinition.CubeDefinition != null)
			{
				Orientation = MyCubeGridDefinitions.GetTopologyUniqueOrientation(BlockDefinition.CubeDefinition.CubeTopology, Orientation);
			}
			ComputeMax(BlockDefinition, Orientation, ref Min, out Max);
			Position = ComputePositionInGrid(new MatrixI(Orientation), BlockDefinition, Min);
			if (objectBuilder.MultiBlockId != 0 && objectBuilder.MultiBlockDefinition.HasValue && objectBuilder.MultiBlockIndex != -1)
			{
				MultiBlockDefinition = MyDefinitionManager.Static.TryGetMultiBlockDefinition(objectBuilder.MultiBlockDefinition.Value);
				if (MultiBlockDefinition != null)
				{
					MultiBlockId = objectBuilder.MultiBlockId;
					MultiBlockIndex = objectBuilder.MultiBlockIndex;
				}
			}
			UpdateShowParts(fixSkeleton: false);
			if (FatBlock == null)
			{
				bool num = !string.IsNullOrEmpty(BlockDefinition.Model);
				bool flag = BlockDefinition.BlockTopology == MyBlockTopology.Cube && !ShowParts;
				if (num || flag)
				{
					FatBlock = new MyCubeBlock();
				}
			}
			if (FatBlock != null)
			{
				FatBlock.SlimBlock = this;
				FatBlock.Init(objectBuilder, cubeGrid);
			}
			if (objectBuilder.ConstructionStockpile != null)
			{
				EnsureConstructionStockpileExists();
				m_stockpile.Init(objectBuilder.ConstructionStockpile);
			}
			else if (objectBuilder.ConstructionInventory != null)
			{
				EnsureConstructionStockpileExists();
				m_stockpile.Init(objectBuilder.ConstructionInventory);
			}
			if (MyFakes.SHOW_DAMAGE_EFFECTS && CubeGrid.CreatePhysics && FatBlock != null && !BlockDefinition.RatioEnoughForDamageEffect(BuildIntegrity / MaxIntegrity) && BlockDefinition.RatioEnoughForDamageEffect(Integrity / MaxIntegrity) && CurrentDamage > 0.01f)
			{
				FatBlock.SetDamageEffectDelayed(show: true);
			}
			UpdateMaxDeformation();
			m_builtByID = objectBuilder.BuiltBy;
			BlockGeneralDamageModifier = objectBuilder.BlockGeneralDamageModifier;
			return true;
		}

		private void m_componentStack_IsFunctionalChanged()
		{
			if (m_componentStack.IsFunctional)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(m_builtByID);
				if (myIdentity != null)
				{
					int num = BlockDefinition.PCU - MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
					myIdentity.BlockLimits.IncreaseBlocksBuilt(BlockDefinition.BlockPairName, num, CubeGrid, modifyBlockCount: false);
					if (myIdentity.BlockLimits != MySession.Static.GlobalBlockLimits && myIdentity.BlockLimits != MySession.Static.PirateBlockLimits)
					{
						MySession.Static.GlobalBlockLimits.IncreaseBlocksBuilt(BlockDefinition.BlockPairName, num, CubeGrid, modifyBlockCount: false);
					}
					CubeGrid.BlocksPCU += num;
				}
				else
				{
					_ = m_builtByID;
				}
				return;
			}
			MyIdentity myIdentity2 = MySession.Static.Players.TryGetIdentity(m_builtByID);
			if (myIdentity2 != null)
			{
				int num2 = BlockDefinition.PCU - MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST;
				myIdentity2.BlockLimits.DecreaseBlocksBuilt(BlockDefinition.BlockPairName, num2, CubeGrid, modifyBlockCount: false);
				if (myIdentity2.BlockLimits != MySession.Static.GlobalBlockLimits && myIdentity2.BlockLimits != MySession.Static.PirateBlockLimits)
				{
					MySession.Static.GlobalBlockLimits.DecreaseBlocksBuilt(BlockDefinition.BlockPairName, num2, CubeGrid, modifyBlockCount: false);
				}
				CubeGrid.BlocksPCU -= num2;
			}
		}

		public void ResumeDamageEffect()
		{
			if (FatBlock == null)
			{
				return;
			}
			if (MyFakes.SHOW_DAMAGE_EFFECTS && !BlockDefinition.RatioEnoughForDamageEffect(BuildIntegrity / MaxIntegrity) && BlockDefinition.RatioEnoughForDamageEffect(Integrity / MaxIntegrity))
			{
				if (CurrentDamage > 0f)
				{
					FatBlock.SetDamageEffect(show: true);
				}
			}
			else
			{
				FatBlock.SetDamageEffect(show: false);
			}
		}

		public void InitOrientation(Base6Directions.Direction Forward, Base6Directions.Direction Up)
		{
			if (MyCubeGridDefinitions.GetCubeRotationOptions(BlockDefinition) == MyRotationOptionsEnum.None)
			{
				Orientation = MyBlockOrientation.Identity;
			}
			else
			{
				Orientation = new MyBlockOrientation(Forward, Up);
			}
			if (BlockDefinition.CubeDefinition != null)
			{
				Orientation = MyCubeGridDefinitions.GetTopologyUniqueOrientation(BlockDefinition.CubeDefinition.CubeTopology, Orientation);
			}
		}

		public void InitOrientation(MyBlockOrientation orientation)
		{
			if (!orientation.IsValid)
			{
				Orientation = MyBlockOrientation.Identity;
			}
			InitOrientation(orientation.Forward, orientation.Up);
		}

		public void InitOrientation(ref Vector3I forward, ref Vector3I up)
		{
			InitOrientation(Base6Directions.GetDirection(forward), Base6Directions.GetDirection(up));
		}

		public MyObjectBuilder_CubeBlock GetObjectBuilder(bool copy = false)
		{
			return GetObjectBuilderInternal(copy);
		}

		public MyObjectBuilder_CubeBlock GetCopyObjectBuilder()
		{
			return GetObjectBuilderInternal(copy: true);
		}

		private MyObjectBuilder_CubeBlock GetObjectBuilderInternal(bool copy)
		{
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = null;
			myObjectBuilder_CubeBlock = ((FatBlock == null) ? ((MyObjectBuilder_CubeBlock)MyObjectBuilderSerializer.CreateNewObject(BlockDefinition.Id)) : FatBlock.GetObjectBuilderCubeBlock(copy));
			myObjectBuilder_CubeBlock.SubtypeName = BlockDefinition.Id.SubtypeName;
			myObjectBuilder_CubeBlock.Min = Min;
			myObjectBuilder_CubeBlock.BlockOrientation = Orientation;
			myObjectBuilder_CubeBlock.IntegrityPercent = m_componentStack.Integrity / m_componentStack.MaxIntegrity;
			myObjectBuilder_CubeBlock.BuildPercent = m_componentStack.BuildRatio;
			myObjectBuilder_CubeBlock.ColorMaskHSV = ColorMaskHSV;
			myObjectBuilder_CubeBlock.SkinSubtypeId = SkinSubtypeId.String;
			myObjectBuilder_CubeBlock.BuiltBy = m_builtByID;
			if (m_stockpile == null || m_stockpile.GetItems().Count == 0)
			{
				myObjectBuilder_CubeBlock.ConstructionStockpile = null;
			}
			else
			{
				myObjectBuilder_CubeBlock.ConstructionStockpile = m_stockpile.GetObjectBuilder();
			}
			if (IsMultiBlockPart)
			{
				myObjectBuilder_CubeBlock.MultiBlockDefinition = MultiBlockDefinition.Id;
				myObjectBuilder_CubeBlock.MultiBlockId = MultiBlockId;
				myObjectBuilder_CubeBlock.MultiBlockIndex = MultiBlockIndex;
			}
			myObjectBuilder_CubeBlock.BlockGeneralDamageModifier = BlockGeneralDamageModifier;
			return myObjectBuilder_CubeBlock;
		}

		public void AddNeighbours()
		{
			AddNeighbours(Min, new Vector3I(Min.X, Max.Y, Max.Z), -Vector3I.UnitX);
			AddNeighbours(Min, new Vector3I(Max.X, Min.Y, Max.Z), -Vector3I.UnitY);
			AddNeighbours(Min, new Vector3I(Max.X, Max.Y, Min.Z), -Vector3I.UnitZ);
			AddNeighbours(new Vector3I(Max.X, Min.Y, Min.Z), Max, Vector3I.UnitX);
			AddNeighbours(new Vector3I(Min.X, Max.Y, Min.Z), Max, Vector3I.UnitY);
			AddNeighbours(new Vector3I(Min.X, Min.Y, Max.Z), Max, Vector3I.UnitZ);
			if (FatBlock != null)
			{
				FatBlock.OnAddedNeighbours();
			}
		}

		private void AddNeighbours(Vector3I min, Vector3I max, Vector3I normalDirection)
		{
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
						AddNeighbour(pos, normalDirection);
						pos.Z++;
					}
					pos.Y++;
				}
				pos.X++;
			}
		}

		private void AddNeighbour(Vector3I pos, Vector3I dir)
		{
			MySlimBlock cubeBlock = CubeGrid.GetCubeBlock(pos + dir);
			if (cubeBlock == null || cubeBlock == this)
			{
				return;
			}
			if (MyFakes.ENABLE_COMPOUND_BLOCKS)
			{
				if (Neighbours.Contains(cubeBlock))
				{
					return;
				}
				MyCompoundCubeBlock myCompoundCubeBlock = FatBlock as MyCompoundCubeBlock;
				MyCompoundCubeBlock myCompoundCubeBlock2 = cubeBlock.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
					foreach (MySlimBlock block in myCompoundCubeBlock.GetBlocks())
					{
						MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
						if (myCompoundCubeBlock2 != null)
						{
							foreach (MySlimBlock block2 in myCompoundCubeBlock2.GetBlocks())
							{
								MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints2 = block2.BlockDefinition.GetBuildProgressModelMountPoints(block2.BuildLevelRatio);
								if (AddNeighbour(ref dir, block, buildProgressModelMountPoints, block2, buildProgressModelMountPoints2, this, cubeBlock))
								{
									return;
								}
							}
						}
						else
						{
							MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints3 = cubeBlock.BlockDefinition.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio);
							if (AddNeighbour(ref dir, block, buildProgressModelMountPoints, cubeBlock, buildProgressModelMountPoints3, this, cubeBlock))
							{
								break;
							}
						}
					}
					return;
				}
				MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints4 = BlockDefinition.GetBuildProgressModelMountPoints(BuildLevelRatio);
				if (myCompoundCubeBlock2 != null)
				{
					foreach (MySlimBlock block3 in myCompoundCubeBlock2.GetBlocks())
					{
						MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints5 = block3.BlockDefinition.GetBuildProgressModelMountPoints(block3.BuildLevelRatio);
						if (AddNeighbour(ref dir, this, buildProgressModelMountPoints4, block3, buildProgressModelMountPoints5, this, cubeBlock))
						{
							break;
						}
					}
					return;
				}
				MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints6 = cubeBlock.BlockDefinition.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio);
				AddNeighbour(ref dir, this, buildProgressModelMountPoints4, cubeBlock, buildProgressModelMountPoints6, this, cubeBlock);
			}
			else
			{
				MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints7 = BlockDefinition.GetBuildProgressModelMountPoints(BuildLevelRatio);
				MyCubeBlockDefinition.MountPoint[] buildProgressModelMountPoints8 = cubeBlock.BlockDefinition.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio);
				if (MyCubeGrid.CheckMountPointsForSide(BlockDefinition, buildProgressModelMountPoints7, ref Orientation, ref Position, ref dir, cubeBlock.BlockDefinition, buildProgressModelMountPoints8, ref cubeBlock.Orientation, ref cubeBlock.Position) && ConnectionAllowed(ref pos, ref dir, cubeBlock) && !Neighbours.Contains(cubeBlock))
				{
					Neighbours.Add(cubeBlock);
					cubeBlock.Neighbours.Add(this);
				}
			}
		}

		private static bool AddNeighbour(ref Vector3I dir, MySlimBlock thisBlock, MyCubeBlockDefinition.MountPoint[] thisMountPoints, MySlimBlock otherBlock, MyCubeBlockDefinition.MountPoint[] otherMountPoints, MySlimBlock thisParentBlock, MySlimBlock otherParentBlock)
		{
			if (MyCubeGrid.CheckMountPointsForSide(thisBlock.BlockDefinition, thisMountPoints, ref thisBlock.Orientation, ref thisBlock.Position, ref dir, otherBlock.BlockDefinition, otherMountPoints, ref otherBlock.Orientation, ref otherBlock.Position) && thisBlock.ConnectionAllowed(ref otherBlock.Position, ref dir, otherBlock))
			{
				thisParentBlock.Neighbours.Add(otherParentBlock);
				otherParentBlock.Neighbours.Add(thisParentBlock);
				return true;
			}
			return false;
		}

		private bool ConnectionAllowed(ref Vector3I otherBlockPos, ref Vector3I faceNormal, MySlimBlock other)
		{
			if (DisconnectFaces.Count > 0 && DisconnectFaces.Contains(faceNormal))
			{
				return false;
			}
			if (FatBlock != null && FatBlock.CheckConnectionAllowed)
			{
				return FatBlock.ConnectionAllowed(ref otherBlockPos, ref faceNormal, other.BlockDefinition);
			}
			return true;
		}

		public void RemoveNeighbours()
		{
			bool flag = true;
			foreach (MySlimBlock neighbour in Neighbours)
			{
				flag &= neighbour.Neighbours.Remove(this);
			}
			Neighbours.Clear();
			if (FatBlock != null)
			{
				FatBlock.OnRemovedNeighbours();
			}
		}

		private void UpdateShowParts(bool fixSkeleton)
		{
			if (BlockDefinition.BlockTopology != 0)
			{
				ShowParts = false;
				return;
			}
			float buildLevelRatio = BuildLevelRatio;
			if (BlockDefinition.BuildProgressModels != null && BlockDefinition.BuildProgressModels.Length != 0)
			{
				MyCubeBlockDefinition.BuildProgressModel buildProgressModel = BlockDefinition.BuildProgressModels[BlockDefinition.BuildProgressModels.Length - 1];
				ShowParts = (buildLevelRatio >= buildProgressModel.BuildRatioUpperBound);
			}
			else
			{
				ShowParts = true;
			}
			if (fixSkeleton && !ShowParts)
			{
				CubeGrid.FixSkeleton(this);
			}
		}

		public void UpdateMaxDeformation()
		{
			m_cachedMaxDeformation = CubeGrid.Skeleton.MaxDeformation(Position, CubeGrid);
		}

		public int CalculateCurrentModelID()
		{
			float buildLevelRatio = BuildLevelRatio;
			if (buildLevelRatio < 1f && BlockDefinition.BuildProgressModels != null && BlockDefinition.BuildProgressModels.Length != 0)
			{
				for (int i = 0; i < BlockDefinition.BuildProgressModels.Length; i++)
				{
					if (BlockDefinition.BuildProgressModels[i].BuildRatioUpperBound >= buildLevelRatio)
					{
						return i;
					}
				}
			}
			return -1;
		}

		public string CalculateCurrentModel(out Matrix orientation)
		{
			float buildLevelRatio = BuildLevelRatio;
			Orientation.GetMatrix(out orientation);
			if (buildLevelRatio < 1f && BlockDefinition.BuildProgressModels != null && BlockDefinition.BuildProgressModels.Length != 0)
			{
				for (int i = 0; i < BlockDefinition.BuildProgressModels.Length; i++)
				{
					if (BlockDefinition.BuildProgressModels[i].BuildRatioUpperBound >= buildLevelRatio)
					{
						if (BlockDefinition.BuildProgressModels[i].RandomOrientation)
						{
							orientation = MyCubeGridDefinitions.AllPossible90rotations[Math.Abs(Position.GetHashCode()) % MyCubeGridDefinitions.AllPossible90rotations.Length].GetFloatMatrix();
						}
						return BlockDefinition.BuildProgressModels[i].File;
					}
				}
			}
			if (FatBlock == null)
			{
				return BlockDefinition.Model;
			}
			return FatBlock.CalculateCurrentModel(out orientation);
		}

		public static Vector3I ComputePositionInGrid(MatrixI localMatrix, MyCubeBlockDefinition blockDefinition, Vector3I min)
		{
			Vector3I normal = blockDefinition.Center;
			Vector3I normal2 = blockDefinition.Size - 1;
			Vector3I.TransformNormal(ref normal2, ref localMatrix, out Vector3I result);
			Vector3I.TransformNormal(ref normal, ref localMatrix, out Vector3I result2);
			Vector3I vector3I = Vector3I.Abs(result);
			Vector3I result3 = result2 + min;
			if (result.X != vector3I.X)
			{
				result3.X += vector3I.X;
			}
			if (result.Y != vector3I.Y)
			{
				result3.Y += vector3I.Y;
			}
			if (result.Z != vector3I.Z)
			{
				result3.Z += vector3I.Z;
			}
			return result3;
		}

		public void SpawnFirstItemInConstructionStockpile()
		{
			if (!MySession.Static.CreativeMode)
			{
				EnsureConstructionStockpileExists();
				MyComponentStack.GroupInfo groupInfo = ComponentStack.GetGroupInfo(0);
				m_stockpile.ClearSyncList();
				m_stockpile.AddItems(1, groupInfo.Component.Id);
				CubeGrid.SendStockpileChanged(this, m_stockpile.GetSyncList());
				m_stockpile.ClearSyncList();
			}
		}

		public void MoveItemsToConstructionStockpile(MyInventoryBase fromInventory)
		{
			if (!MySession.Static.CreativeMode)
			{
				m_tmpComponents.Clear();
				GetMissingComponents(m_tmpComponents);
				if (m_tmpComponents.Count != 0)
				{
					EnsureConstructionStockpileExists();
					m_stockpile.ClearSyncList();
					foreach (KeyValuePair<string, int> tmpComponent in m_tmpComponents)
					{
						MyDefinitionId myDefinitionId = new MyDefinitionId(typeof(MyObjectBuilder_Component), tmpComponent.Key);
						int val = (int)MyCubeBuilder.BuildComponent.GetItemAmountCombined(fromInventory, myDefinitionId);
						int num = Math.Min(tmpComponent.Value, val);
						if (num > 0)
						{
							MyCubeBuilder.BuildComponent.RemoveItemsCombined(fromInventory, num, myDefinitionId);
							m_stockpile.AddItems(num, new MyDefinitionId(typeof(MyObjectBuilder_Component), tmpComponent.Key));
						}
					}
					CubeGrid.SendStockpileChanged(this, m_stockpile.GetSyncList());
					m_stockpile.ClearSyncList();
				}
			}
		}

		public bool MoveItemsFromConstructionStockpile(MyInventoryBase toInventory, MyItemFlags flags = MyItemFlags.None)
		{
			bool result = false;
			if (m_stockpile == null)
			{
				return result;
			}
			if (toInventory == null)
			{
				return result;
			}
			m_tmpItemList.Clear();
			foreach (MyStockpileItem item in m_stockpile.GetItems())
			{
				if (flags == MyItemFlags.None || (item.Content.Flags & flags) != 0)
				{
					m_tmpItemList.Add(item);
				}
			}
			m_stockpile.ClearSyncList();
			foreach (MyStockpileItem tmpItem in m_tmpItemList)
			{
				int val = toInventory.ComputeAmountThatFits(tmpItem.Content.GetId()).ToIntSafe();
				val = Math.Min(val, tmpItem.Amount);
				toInventory.AddItems(val, tmpItem.Content);
				m_stockpile.RemoveItems(val, tmpItem.Content);
				if (val <= 0)
				{
					result = true;
				}
			}
			CubeGrid.SendStockpileChanged(this, m_stockpile.GetSyncList());
			m_stockpile.ClearSyncList();
			return result;
		}

		public void MoveUnneededItemsFromConstructionStockpile(MyInventoryBase toInventory)
		{
			if (m_stockpile != null && toInventory != null)
			{
				m_tmpItemList.Clear();
				AcquireUnneededStockpileItems(m_tmpItemList);
				m_stockpile.ClearSyncList();
				foreach (MyStockpileItem tmpItem in m_tmpItemList)
				{
					int val = toInventory.ComputeAmountThatFits(tmpItem.Content.GetId()).ToIntSafe();
					val = Math.Min(val, tmpItem.Amount);
					toInventory.AddItems(val, tmpItem.Content);
					m_stockpile.RemoveItems(val, tmpItem.Content);
				}
				CubeGrid.SendStockpileChanged(this, m_stockpile.GetSyncList());
				m_stockpile.ClearSyncList();
			}
		}

		public void ClearConstructionStockpile(MyInventoryBase outputInventory)
		{
			if (!StockpileEmpty)
			{
				MyEntity myEntity = null;
				if (outputInventory != null && outputInventory.Container != null)
				{
					myEntity = (outputInventory.Container.Entity as MyEntity);
				}
				if (myEntity != null && myEntity.InventoryOwnerType() == MyInventoryOwnerTypeEnum.Character)
				{
					MoveItemsFromConstructionStockpile(outputInventory);
				}
				else
				{
					m_stockpile.ClearSyncList();
					m_tmpItemList.Clear();
					foreach (MyStockpileItem item in m_stockpile.GetItems())
					{
						m_tmpItemList.Add(item);
					}
					foreach (MyStockpileItem tmpItem in m_tmpItemList)
					{
						RemoveFromConstructionStockpile(tmpItem);
					}
					CubeGrid.SendStockpileChanged(this, m_stockpile.GetSyncList());
					m_stockpile.ClearSyncList();
				}
			}
			ReleaseConstructionStockpile();
		}

		private void RemoveFromConstructionStockpile(MyStockpileItem item)
		{
			m_stockpile.RemoveItems(item.Amount, item.Content.GetId(), item.Content.Flags);
		}

		private void AcquireUnneededStockpileItems(List<MyStockpileItem> outputList)
		{
			if (m_stockpile != null)
			{
				foreach (MyStockpileItem item in m_stockpile.GetItems())
				{
					bool flag = false;
					MyCubeBlockDefinition.Component[] components = BlockDefinition.Components;
					for (int i = 0; i < components.Length; i++)
					{
						if (components[i].Definition.Id.SubtypeId == item.Content.SubtypeId)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						outputList.Add(item);
					}
				}
			}
		}

		private void ReleaseUnneededStockpileItems()
		{
			if (m_stockpile != null && Sync.IsServer)
			{
				m_tmpItemList.Clear();
				AcquireUnneededStockpileItems(m_tmpItemList);
				m_stockpile.ClearSyncList();
				BoundingBoxD box = new BoundingBoxD(CubeGrid.GridIntegerToWorld(Min), CubeGrid.GridIntegerToWorld(Max));
				foreach (MyStockpileItem tmpItem in m_tmpItemList)
				{
					if (!((float)tmpItem.Amount < 0.01f))
					{
						MyEntity myEntity = MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(tmpItem.Amount, tmpItem.Content), box, CubeGrid.Physics);
						myEntity?.Physics.ApplyImpulse(MyUtils.GetRandomVector3Normalized() * myEntity.Physics.Mass / 5f, myEntity.PositionComp.GetPosition());
						m_stockpile.RemoveItems(tmpItem.Amount, tmpItem.Content);
					}
				}
				CubeGrid.SendStockpileChanged(this, m_stockpile.GetSyncList());
				m_stockpile.ClearSyncList();
			}
		}

		public int GetConstructionStockpileItemAmount(MyDefinitionId id)
		{
			if (m_stockpile == null)
			{
				return 0;
			}
			return m_stockpile.GetItemAmount(id);
		}

		public void SetToConstructionSite()
		{
			m_componentStack.DestroyCompletely();
		}

		public void GetMissingComponents(Dictionary<string, int> addToDictionary)
		{
			m_componentStack.GetMissingComponents(addToDictionary, m_stockpile);
		}

		private void ReleaseConstructionStockpile()
		{
			if (m_stockpile != null)
			{
				if (MyFakes.ENABLE_GENERATED_BLOCKS)
				{
					_ = BlockDefinition.IsGeneratedBlock;
				}
				m_stockpile = null;
			}
		}

		private void EnsureConstructionStockpileExists()
		{
			if (m_stockpile == null)
			{
				m_stockpile = new MyConstructionStockpile();
			}
		}

		public void SpawnConstructionStockpile()
		{
			if (m_stockpile == null)
			{
				return;
			}
			MatrixD worldMatrix = CubeGrid.WorldMatrix;
			int num = Max.RectangularDistance(Min) + 3;
			Vector3D position = Min;
			Vector3D position2 = Max;
			position *= (double)CubeGrid.GridSize;
			position2 *= (double)CubeGrid.GridSize;
			position = Vector3D.Transform(position, worldMatrix);
			position2 = Vector3D.Transform(position2, worldMatrix);
			Vector3D vector3D = (position + position2) / 2.0;
			Vector3 value = MyGravityProviderSystem.CalculateTotalGravityInPoint(vector3D);
			if (value.Length() != 0f)
			{
				value.Normalize();
				Vector3I? vector3I = CubeGrid.RayCastBlocks(vector3D, vector3D + value * num * CubeGrid.GridSize);
				if (!vector3I.HasValue)
				{
					position = vector3D;
				}
				else
				{
					position = vector3I.Value;
					position *= (double)CubeGrid.GridSize;
					position = Vector3D.Transform(position, worldMatrix);
					position -= value * CubeGrid.GridSize * 0.1f;
				}
			}
			foreach (MyStockpileItem item in m_stockpile.GetItems())
			{
				MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(item.Amount, item.Content), position, worldMatrix.Forward, worldMatrix.Up, CubeGrid.Physics);
			}
		}

		public bool CanContinueBuild(MyInventoryBase sourceInventory)
		{
			if (IsFullIntegrity || (sourceInventory == null && !MySession.Static.CreativeMode))
			{
				return false;
			}
			if (FatBlock != null && !FatBlock.CanContinueBuild())
			{
				return false;
			}
			return m_componentStack.CanContinueBuild(sourceInventory, m_stockpile);
		}

		public void FixBones(float oldDamage, float maxAllowedBoneMovement)
		{
			float num = CurrentDamage / oldDamage;
			if (oldDamage == 0f)
			{
				num = 0f;
			}
			float num2 = (1f - num) * MaxDeformation;
			if (MaxDeformation != 0f && num2 > maxAllowedBoneMovement)
			{
				num = 1f - maxAllowedBoneMovement / MaxDeformation;
			}
			if (num == 0f)
			{
				CubeGrid.ResetBlockSkeleton(this, updateSync: true);
			}
			if (num > 0f)
			{
				CubeGrid.MultiplyBlockSkeleton(this, num, updateSync: true);
			}
		}

		public bool DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId)
		{
			if (damage <= 0f)
			{
				return false;
			}
			if (sync)
			{
				if (Sync.IsServer)
				{
					DoDamageSynced(this, damage, damageType, hitInfo, attackerId);
					long attackerIdentityId = GetAttackerIdentityId(attackerId);
					long num = (CubeGrid.BigOwners.Count > 0) ? CubeGrid.BigOwners[0] : 0;
					if (attackerIdentityId != 0L && num != 0L && attackerIdentityId != num)
					{
						MySession.Static.Factions.DamageFactionPlayerReputation(attackerIdentityId, num, MyReputationDamageType.Damaging);
					}
				}
			}
			else
			{
				DoDamage(damage, damageType, hitInfo, addDirtyParts: true, attackerId);
			}
			return true;
		}

		private long GetAttackerIdentityId(long attackerId)
		{
			MyEntity entity = null;
			MyEntities.TryGetEntityById(attackerId, out entity);
			IMyControllableEntity myControllableEntity = entity as IMyControllableEntity;
			if (myControllableEntity != null)
			{
				MyControllerInfo controllerInfo = myControllableEntity.ControllerInfo;
				if (controllerInfo != null)
				{
					return controllerInfo.ControllingIdentityId;
				}
			}
			else
			{
				IMyGunBaseUser myGunBaseUser;
				if ((myGunBaseUser = (entity as IMyGunBaseUser)) != null)
				{
					return myGunBaseUser.OwnerId;
				}
				IMyHandheldGunObject<MyDeviceBase> myHandheldGunObject;
				if ((myHandheldGunObject = (entity as IMyHandheldGunObject<MyDeviceBase>)) != null)
				{
					return myHandheldGunObject.OwnerIdentityId;
				}
			}
			return 0L;
		}

		public void DoDamage(float damage, MyStringHash damageType, MyHitInfo? hitInfo = null, bool addDirtyParts = true, long attackerId = 0L)
		{
			if (CubeGrid.BlocksDestructionEnabled || ForceBlockDestructible)
			{
				damage = damage * BlockGeneralDamageModifier * CubeGrid.GridGeneralDamageModifier * BlockDefinition.GeneralDamageMultiplier;
				DoDamageInternal(damage, damageType, addDirtyParts, hitInfo, attackerId);
			}
		}

		private void DoDamageInternal(float damage, MyStringHash damageType, bool addDirtyParts = true, MyHitInfo? hitInfo = null, long attackerId = 0L)
		{
			damage *= DamageRatio;
			ulong num = MySession.Static.Players.TryGetSteamId(attackerId);
			if (num == 0L)
			{
				MyEntity entityById = MyEntities.GetEntityById(attackerId);
				MyAutomaticRifleGun myAutomaticRifleGun;
				if ((myAutomaticRifleGun = (entityById as MyAutomaticRifleGun)) != null && myAutomaticRifleGun.Owner != null)
				{
					MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(myAutomaticRifleGun.Owner);
					if (controllingPlayer != null)
					{
						num = controllingPlayer.Id.SteamId;
					}
				}
				MyHandDrill myHandDrill;
				if ((myHandDrill = (entityById as MyHandDrill)) != null && myHandDrill.Owner != null)
				{
					MyPlayer controllingPlayer2 = MySession.Static.Players.GetControllingPlayer(myHandDrill.Owner);
					if (controllingPlayer2 != null)
					{
						num = controllingPlayer2.Id.SteamId;
					}
				}
			}
			if (MySessionComponentSafeZones.IsActionAllowed(CubeGrid, MySafeZoneAction.Damage, 0L, num))
			{
				if (MyPerGameSettings.Destruction || MyFakes.ENABLE_VR_BLOCK_DEFORMATION_RATIO)
				{
					damage *= DeformationRatio;
				}
				try
				{
					if (FatBlock != null && !FatBlock.Closed && CubeGrid.Physics != null && CubeGrid.Physics.Enabled)
					{
						IMyDestroyableObject myDestroyableObject = FatBlock as IMyDestroyableObject;
						if (myDestroyableObject != null && myDestroyableObject.DoDamage(damage, damageType, sync: false, null, attackerId))
						{
							return;
						}
					}
				}
				catch
				{
				}
				MyDamageInformation info = new MyDamageInformation(isDeformation: false, damage, damageType, attackerId);
				if (UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseBeforeDamageApplied(this, ref info);
					damage = info.Amount;
				}
				MySession.Static.NegativeIntegrityTotal += damage;
				AccumulatedDamage += damage;
				if (m_componentStack.Integrity - AccumulatedDamage <= 1.52590219E-05f)
				{
					ApplyAccumulatedDamage(addDirtyParts, attackerId);
					CubeGrid.RemoveFromDamageApplication(this);
				}
				else if (MyFakes.SHOW_DAMAGE_EFFECTS && FatBlock != null && !FatBlock.Closed && !BlockDefinition.RatioEnoughForDamageEffect(BuildIntegrity / MaxIntegrity) && BlockDefinition.RatioEnoughForDamageEffect((Integrity - damage) / MaxIntegrity))
				{
					FatBlock.SetDamageEffect(show: true);
				}
				if (UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseAfterDamageApplied(this, info);
				}
				m_lastDamage = damage;
				m_lastAttackerId = attackerId;
				m_lastDamageType = damageType;
			}
		}

		void IMyDecalProxy.AddDecals(ref MyHitInfo hitInfo, MyStringHash source, object customdata, IMyDecalHandler decalHandler, MyStringHash material)
		{
			MyCubeGrid.MyCubeGridHitInfo myCubeGridHitInfo = customdata as MyCubeGrid.MyCubeGridHitInfo;
			if (myCubeGridHitInfo != null)
			{
				MyDecalRenderInfo myDecalRenderInfo = default(MyDecalRenderInfo);
				myDecalRenderInfo.Source = source;
				myDecalRenderInfo.Material = ((material.GetHashCode() == 0) ? MyStringHash.GetOrCompute(BlockDefinition.PhysicalMaterial.Id.SubtypeName) : material);
				MyDecalRenderInfo renderInfo = myDecalRenderInfo;
				if (FatBlock == null)
				{
					renderInfo.Position = Vector3D.Transform(hitInfo.Position, CubeGrid.PositionComp.WorldMatrixInvScaled);
					renderInfo.Normal = Vector3D.TransformNormal(hitInfo.Normal, CubeGrid.PositionComp.WorldMatrixInvScaled);
					renderInfo.RenderObjectIds = CubeGrid.Render.RenderObjectIDs;
				}
				else
				{
					renderInfo.Position = Vector3D.Transform(hitInfo.Position, FatBlock.PositionComp.WorldMatrixInvScaled);
					renderInfo.Normal = Vector3D.TransformNormal(hitInfo.Normal, FatBlock.PositionComp.WorldMatrixInvScaled);
					renderInfo.RenderObjectIds = FatBlock.Render.RenderObjectIDs;
				}
				VertexBoneIndicesWeights? affectingBoneIndicesWeights = myCubeGridHitInfo.Triangle.GetAffectingBoneIndicesWeights(ref m_boneIndexWeightTmp);
				if (affectingBoneIndicesWeights.HasValue)
				{
					renderInfo.BoneIndices = affectingBoneIndicesWeights.Value.Indices;
					renderInfo.BoneWeights = affectingBoneIndicesWeights.Value.Weights;
				}
				if (m_tmpIds == null)
				{
					m_tmpIds = new List<uint>();
				}
				else
				{
					m_tmpIds.Clear();
				}
				decalHandler.AddDecal(ref renderInfo, m_tmpIds);
				foreach (uint tmpId in m_tmpIds)
				{
					CubeGrid.RenderData.AddDecal(Position, myCubeGridHitInfo, tmpId);
				}
			}
		}

		public void ApplyDestructionDamage(float integrityRatioFromFracturedPieces)
		{
			if (MyFakes.ENABLE_FRACTURE_COMPONENT && Sync.IsServer && MyPerGameSettings.Destruction)
			{
				float damage = (ComponentStack.IntegrityRatio - integrityRatioFromFracturedPieces) * BlockDefinition.MaxIntegrity;
				if (CanApplyDestructionDamage(damage))
				{
					((IMyDestroyableObject)this).DoDamage(damage, MyDamageType.Destruction, sync: true, (MyHitInfo?)null, 0L);
				}
				else if (CanApplyDestructionDamage(MyDefinitionManager.Static.DestructionDefinition.DestructionDamage))
				{
					((IMyDestroyableObject)this).DoDamage(MyDefinitionManager.Static.DestructionDefinition.DestructionDamage, MyDamageType.Destruction, sync: true, (MyHitInfo?)null, 0L);
				}
			}
		}

		private bool CanApplyDestructionDamage(float damage)
		{
			if (damage <= 0f)
			{
				return false;
			}
			if (IsMultiBlockPart)
			{
				MyCubeGridMultiBlockInfo multiBlockInfo = CubeGrid.GetMultiBlockInfo(MultiBlockId);
				if (multiBlockInfo != null)
				{
					float totalMaxIntegrity = multiBlockInfo.GetTotalMaxIntegrity();
					foreach (MySlimBlock block in multiBlockInfo.Blocks)
					{
						float num = damage * block.MaxIntegrity / totalMaxIntegrity;
						num *= block.DamageRatio;
						num *= block.DeformationRatio;
						num += block.AccumulatedDamage;
						if (block.Integrity - num <= 1.52590219E-05f)
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
			damage *= DamageRatio;
			damage *= DeformationRatio;
			damage += AccumulatedDamage;
			return Integrity - damage > 1.52590219E-05f;
		}

		internal int GetTotalBreakableShapeChildrenCount()
		{
			if (FatBlock == null)
			{
				return 0;
			}
			string assetName = FatBlock.Model.AssetName;
			int value = 0;
			if (m_modelTotalFracturesCount.TryGetValue(assetName, out value))
			{
				return value;
			}
			MyModel modelOnlyData = MyModels.GetModelOnlyData(assetName);
			if (modelOnlyData.HavokBreakableShapes == null)
			{
				MyDestructionData.Static.LoadModelDestruction(assetName, BlockDefinition, Vector3.One);
			}
			int totalChildrenCount = modelOnlyData.HavokBreakableShapes[0].GetTotalChildrenCount();
			m_modelTotalFracturesCount.Add(assetName, totalChildrenCount);
			return totalChildrenCount;
		}

		public void ApplyAccumulatedDamage(bool addDirtyParts = true, long attackerId = 0L)
		{
			if (MySession.Static.SurvivalMode)
			{
				EnsureConstructionStockpileExists();
			}
			float integrity = Integrity;
			if (m_stockpile != null)
			{
				m_stockpile.ClearSyncList();
				m_componentStack.ApplyDamage(AccumulatedDamage, m_stockpile);
				if (Sync.IsServer)
				{
					CubeGrid.SendStockpileChanged(this, m_stockpile.GetSyncList());
				}
				m_stockpile.ClearSyncList();
			}
			else
			{
				m_componentStack.ApplyDamage(AccumulatedDamage);
			}
			if (!BlockDefinition.RatioEnoughForDamageEffect(integrity / MaxIntegrity) && BlockDefinition.RatioEnoughForDamageEffect(Integrity / MaxIntegrity) && FatBlock != null)
			{
				FatBlock.OnIntegrityChanged(BuildIntegrity, Integrity, setOwnership: false, MySession.Static.LocalPlayerId);
			}
			AccumulatedDamage = 0f;
			if (m_componentStack.IsDestroyed)
			{
				if (MyFakes.SHOW_DAMAGE_EFFECTS && FatBlock != null)
				{
					FatBlock.SetDamageEffect(show: false);
				}
				CubeGrid.RemoveDestroyedBlock(this, attackerId);
				if (addDirtyParts)
				{
					CubeGrid.Physics.AddDirtyBlock(this);
				}
				if (UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseDestroyed(this, new MyDamageInformation(isDeformation: false, m_lastDamage, m_lastDamageType, m_lastAttackerId));
				}
			}
		}

		public void UpdateVisual(bool updatePhysics = true)
		{
			bool flag = false;
			UpdateShowParts(fixSkeleton: true);
			if (!ShowParts)
			{
				if (FatBlock == null)
				{
					FatBlock = new MyCubeBlock();
					FatBlock.SlimBlock = this;
					FatBlock.Init();
					CubeGrid.Hierarchy.AddChild(FatBlock);
				}
				else
				{
					FatBlock.UpdateVisual();
				}
			}
			else if (FatBlock != null)
			{
				_ = FatBlock.WorldMatrix.Translation;
				CubeGrid.Hierarchy.RemoveChild(FatBlock);
				FatBlock.Close();
				FatBlock = null;
				flag = true;
			}
			CubeGrid.SetBlockDirty(this);
			if (flag)
			{
				CubeGrid.UpdateDirty(null, immediate: true);
			}
			if (updatePhysics && CubeGrid.Physics != null)
			{
				CubeGrid.Physics.AddDirtyArea(Min, Max);
			}
		}

		public void IncreaseMountLevelToDesiredRatio(float desiredIntegrityRatio, long welderOwnerPlayerId, MyInventoryBase outputInventory = null, float maxAllowedBoneMovement = 0f, bool isHelping = false, MyOwnershipShareModeEnum sharing = MyOwnershipShareModeEnum.Faction)
		{
			float num = desiredIntegrityRatio * MaxIntegrity - Integrity;
			if (!(num <= 0f))
			{
				IncreaseMountLevel(num / BlockDefinition.IntegrityPointsPerSec, welderOwnerPlayerId, outputInventory, maxAllowedBoneMovement, isHelping, sharing);
			}
		}

		public void DecreaseMountLevelToDesiredRatio(float desiredIntegrityRatio, MyInventoryBase outputInventory)
		{
			float num = desiredIntegrityRatio * MaxIntegrity;
			float num2 = Integrity - num;
			if (!(num2 <= 0f))
			{
				num2 = ((FatBlock == null) ? (num2 * BlockDefinition.DisassembleRatio) : (num2 * FatBlock.DisassembleRatio));
				DecreaseMountLevel(num2 / BlockDefinition.IntegrityPointsPerSec, outputInventory, useDefaultDeconstructEfficiency: true, 0L);
			}
		}

		public bool IncreaseMountLevel(float welderMountAmount, long welderOwnerIdentId, MyInventoryBase outputInventory = null, float maxAllowedBoneMovement = 0f, bool isHelping = false, MyOwnershipShareModeEnum sharing = MyOwnershipShareModeEnum.Faction, bool handWelded = false)
		{
			ulong user = 0uL;
			if (welderOwnerIdentId != 0L)
			{
				MySession.Static.Players.TryGetPlayerId(welderOwnerIdentId, out MyPlayer.PlayerId result);
				user = result.SteamId;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(CubeGrid, MySafeZoneAction.Building, 0L, user))
			{
				return false;
			}
			float buildIntegrity = ComponentStack.BuildIntegrity;
			float integrity = ComponentStack.Integrity;
			bool isFunctional = ComponentStack.IsFunctional;
			welderMountAmount *= BlockDefinition.IntegrityPointsPerSec;
			MySession.Static.PositiveIntegrityTotal += welderMountAmount;
			if (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(MySession.Static.Players.TryGetSteamId(welderOwnerIdentId)))
			{
				ClearConstructionStockpile(outputInventory);
			}
			else
			{
				MyEntity myEntity = null;
				if (outputInventory != null && outputInventory.Container != null)
				{
					myEntity = (outputInventory.Container.Entity as MyEntity);
				}
				if (myEntity != null && myEntity.InventoryOwnerType() == MyInventoryOwnerTypeEnum.Character)
				{
					MoveItemsFromConstructionStockpile(outputInventory, MyItemFlags.Damaged);
				}
			}
			float buildRatio = m_componentStack.BuildRatio;
			float currentDamage = CurrentDamage;
			if (BlockDefinition.RatioEnoughForOwnership(BuildLevelRatio) && FatBlock != null && FatBlock.OwnerId != welderOwnerIdentId && outputInventory != null && !isHelping)
			{
				FatBlock.OnIntegrityChanged(BuildIntegrity, Integrity, setOwnership: true, welderOwnerIdentId, sharing);
			}
			if (MyFakes.SHOW_DAMAGE_EFFECTS && FatBlock != null && !BlockDefinition.RatioEnoughForDamageEffect((Integrity + welderMountAmount) / MaxIntegrity))
			{
				FatBlock.SetDamageEffect(show: false);
			}
			bool flag = false;
			if (m_stockpile != null)
			{
				m_stockpile.ClearSyncList();
				m_componentStack.IncreaseMountLevel(welderMountAmount, m_stockpile);
				CubeGrid.SendStockpileChanged(this, m_stockpile.GetSyncList());
				m_stockpile.ClearSyncList();
			}
			else
			{
				m_componentStack.IncreaseMountLevel(welderMountAmount);
			}
			if (m_componentStack.IsFullIntegrity)
			{
				ReleaseConstructionStockpile();
				flag = true;
			}
			MyIntegrityChangeEnum integrityChangeType = MyIntegrityChangeEnum.Damage;
			if (BlockDefinition.ModelChangeIsNeeded(buildRatio, m_componentStack.BuildRatio) || BlockDefinition.ModelChangeIsNeeded(m_componentStack.BuildRatio, buildRatio))
			{
				flag = true;
				if (FatBlock != null && m_componentStack.IsFunctional)
				{
					integrityChangeType = MyIntegrityChangeEnum.ConstructionEnd;
				}
				UpdateVisual();
				if (FatBlock != null)
				{
					if (CalculateCurrentModelID() == 0)
					{
						integrityChangeType = MyIntegrityChangeEnum.ConstructionBegin;
					}
					else if (!m_componentStack.IsFunctional)
					{
						integrityChangeType = MyIntegrityChangeEnum.ConstructionProcess;
					}
				}
				PlayConstructionSound(integrityChangeType);
				CreateConstructionSmokes();
				if (CubeGrid.GridSystems.GasSystem != null)
				{
					CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioRaised(this);
				}
			}
			else if (m_componentStack.IsFunctional && !isFunctional)
			{
				integrityChangeType = MyIntegrityChangeEnum.ConstructionEnd;
				PlayConstructionSound(integrityChangeType);
			}
			if (HasDeformation)
			{
				CubeGrid.SetBlockDirty(this);
			}
			if (flag)
			{
				CubeGrid.RenderData.RemoveDecals(Position);
			}
			CubeGrid.SendIntegrityChanged(this, integrityChangeType, 0L);
			CubeGrid.OnIntegrityChanged(this, handWelded);
			if (ComponentStack.IsFunctional && !isFunctional)
			{
				MyCubeGrids.NotifyBlockFunctional(CubeGrid, this, handWelded);
			}
			if (maxAllowedBoneMovement != 0f)
			{
				FixBones(currentDamage, maxAllowedBoneMovement);
			}
			if (MyFakes.ENABLE_GENERATED_BLOCKS && !BlockDefinition.IsGeneratedBlock && BlockDefinition.GeneratedBlockDefinitions != null && BlockDefinition.GeneratedBlockDefinitions.Length != 0)
			{
				UpdateProgressGeneratedBlocks(buildRatio);
			}
			if (buildIntegrity == ComponentStack.BuildIntegrity)
			{
				return integrity != ComponentStack.Integrity;
			}
			return true;
		}

		public void DecreaseMountLevel(float grinderAmount, MyInventoryBase outputInventory, bool useDefaultDeconstructEfficiency = false, long identityId = 0L)
		{
			if (!Sync.IsServer || m_componentStack.IsFullyDismounted)
			{
				return;
			}
			ulong user = 0uL;
			if (identityId != 0L)
			{
				MySession.Static.Players.TryGetPlayerId(identityId, out MyPlayer.PlayerId result);
				user = result.SteamId;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(CubeGrid, MySafeZoneAction.Building, 0L, user))
			{
				return;
			}
			grinderAmount = ((FatBlock == null) ? (grinderAmount / BlockDefinition.DisassembleRatio) : (grinderAmount / FatBlock.DisassembleRatio));
			grinderAmount *= BlockDefinition.IntegrityPointsPerSec;
			float buildRatio = m_componentStack.BuildRatio;
			DeconstructStockpile(grinderAmount, outputInventory, useDefaultDeconstructEfficiency);
			float newRatio = (BuildIntegrity - grinderAmount) / BlockDefinition.MaxIntegrity;
			if (BlockDefinition.RatioEnoughForDamageEffect(BuildLevelRatio) && FatBlock != null && FatBlock.OwnerId != 0L)
			{
				FatBlock.OnIntegrityChanged(BuildIntegrity, Integrity, setOwnership: false, 0L);
			}
			long num = 0L;
			if (outputInventory != null && outputInventory.Entity != null)
			{
				IMyComponentOwner<MyIDModule> myComponentOwner = outputInventory.Entity as IMyComponentOwner<MyIDModule>;
				if (myComponentOwner != null && myComponentOwner.GetComponent(out MyIDModule component))
				{
					num = component.Owner;
				}
			}
			UpdateHackingIndicator(newRatio, buildRatio, num);
			bool num2 = BlockDefinition.ModelChangeIsNeeded(m_componentStack.BuildRatio, buildRatio);
			MyIntegrityChangeEnum integrityChangeType = MyIntegrityChangeEnum.Damage;
			if (num2)
			{
				UpdateVisual();
				if (FatBlock != null)
				{
					int num3 = CalculateCurrentModelID();
					integrityChangeType = ((num3 == -1 || BuildLevelRatio == 0f) ? MyIntegrityChangeEnum.DeconstructionEnd : ((num3 != BlockDefinition.BuildProgressModels.Length - 1) ? MyIntegrityChangeEnum.DeconstructionProcess : MyIntegrityChangeEnum.DeconstructionBegin));
					FatBlock.SetDamageEffect(show: false);
				}
				PlayConstructionSound(integrityChangeType, deconstruction: true);
				CreateConstructionSmokes();
				if (CubeGrid.GridSystems.GasSystem != null)
				{
					CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioLowered(this);
				}
			}
			if (MyFakes.ENABLE_GENERATED_BLOCKS && !BlockDefinition.IsGeneratedBlock && BlockDefinition.GeneratedBlockDefinitions != null && BlockDefinition.GeneratedBlockDefinitions.Length != 0)
			{
				UpdateProgressGeneratedBlocks(buildRatio);
			}
			CubeGrid.SendIntegrityChanged(this, integrityChangeType, num);
			CubeGrid.OnIntegrityChanged(this, handWelded: false);
		}

		public void FullyDismount(MyInventory outputInventory)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			DeconstructStockpile(BuildIntegrity, outputInventory);
			float buildRatio = m_componentStack.BuildRatio;
			if (BlockDefinition.ModelChangeIsNeeded(m_componentStack.BuildRatio, buildRatio))
			{
				UpdateVisual();
				PlayConstructionSound(MyIntegrityChangeEnum.DeconstructionEnd, deconstruction: true);
				CreateConstructionSmokes();
				if (CubeGrid.GridSystems.GasSystem != null)
				{
					CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioLowered(this);
				}
			}
		}

		private void DeconstructStockpile(float deconstructAmount, MyInventoryBase outputInventory, bool useDefaultDeconstructEfficiency = false)
		{
			if (MySession.Static.CreativeMode)
			{
				ClearConstructionStockpile(outputInventory);
			}
			else
			{
				EnsureConstructionStockpileExists();
			}
			if (m_stockpile != null)
			{
				m_stockpile.ClearSyncList();
				m_componentStack.DecreaseMountLevel(deconstructAmount, m_stockpile, useDefaultDeconstructEfficiency);
				CubeGrid.SendStockpileChanged(this, m_stockpile.GetSyncList());
				m_stockpile.ClearSyncList();
			}
			else
			{
				m_componentStack.DecreaseMountLevel(deconstructAmount, null, useDefaultDeconstructEfficiency);
			}
		}

		private void CreateConstructionSmokes()
		{
			Vector3 value = new Vector3(CubeGrid.GridSize) / 2f;
			BoundingBox boundingBox = new BoundingBox(Min * CubeGrid.GridSize - value, Max * CubeGrid.GridSize + value);
			if (FatBlock != null && FatBlock.Model != null)
			{
				BoundingBox boundingBox2 = new BoundingBox(FatBlock.Model.BoundingBox.Min, FatBlock.Model.BoundingBox.Max);
				FatBlock.Orientation.GetMatrix(out Matrix result);
				BoundingBox boundingBox3 = BoundingBox.CreateInvalid();
				Vector3[] corners = boundingBox2.GetCorners();
				foreach (Vector3 position in corners)
				{
					boundingBox3 = boundingBox3.Include(Vector3.Transform(position, result));
				}
				boundingBox = new BoundingBox(boundingBox3.Min + boundingBox.Center, boundingBox3.Max + boundingBox.Center);
			}
			if (ConstructionParticlesTimedCache.IsPlaceUsed(WorldPosition, ConstructionParticleSpaceMapping, MySandboxGame.TotalSimulationTimeInMilliseconds))
			{
				return;
			}
			boundingBox.Inflate(-0.3f);
			Vector3[] corners2 = boundingBox.GetCorners();
			float num = 0.25f;
			for (int j = 0; j < MyOrientedBoundingBox.StartVertices.Length; j++)
			{
				Vector3 vector = corners2[MyOrientedBoundingBox.StartVertices[j]];
				float num2 = 0f;
				float num3 = Vector3.Distance(vector, corners2[MyOrientedBoundingBox.EndVertices[j]]);
				Vector3 vector2 = num * Vector3.Normalize(corners2[MyOrientedBoundingBox.EndVertices[j]] - corners2[MyOrientedBoundingBox.StartVertices[j]]);
				while (num2 < num3)
				{
					Vector3D position2 = Vector3D.Transform(vector, CubeGrid.WorldMatrix);
					if (MyParticlesManager.TryCreateParticleEffect("Smoke_Construction", MatrixD.CreateTranslation(position2), out MyParticleEffect effect))
					{
						effect.Velocity = CubeGrid.Physics.LinearVelocity;
					}
					num2 += num;
					vector += vector2;
				}
			}
		}

		public override string ToString()
		{
			if (FatBlock == null)
			{
				return BlockDefinition.DisplayNameText.ToString();
			}
			return FatBlock.ToString();
		}

		public static void ComputeMax(MyCubeBlockDefinition definition, MyBlockOrientation orientation, ref Vector3I min, out Vector3I max)
		{
			Vector3I normal = definition.Size - 1;
			MatrixI matrix = new MatrixI(orientation);
			Vector3I.TransformNormal(ref normal, ref matrix, out normal);
			Vector3I.Abs(ref normal, out normal);
			max = min + normal;
		}

		public void SetIntegrity(float buildIntegrity, float integrity, MyIntegrityChangeEnum integrityChangeType, long grinderOwner)
		{
			float buildRatio = m_componentStack.BuildRatio;
			m_componentStack.SetIntegrity(buildIntegrity, integrity);
			if (FatBlock != null && !BlockDefinition.RatioEnoughForOwnership(buildRatio) && BlockDefinition.RatioEnoughForOwnership(m_componentStack.BuildRatio))
			{
				FatBlock.OnIntegrityChanged(buildIntegrity, integrity, setOwnership: true, MySession.Static.LocalPlayerId);
			}
			UpdateHackingIndicator(m_componentStack.BuildRatio, buildRatio, grinderOwner);
			if (MyFakes.SHOW_DAMAGE_EFFECTS && FatBlock != null && !BlockDefinition.RatioEnoughForDamageEffect(Integrity / MaxIntegrity))
			{
				FatBlock.SetDamageEffect(show: false);
			}
			bool flag = IsFullIntegrity;
			if (ModelChangeIsNeeded(m_componentStack.BuildRatio, buildRatio))
			{
				flag = true;
				UpdateVisual();
				if (integrityChangeType != 0)
				{
					CreateConstructionSmokes();
				}
				PlayConstructionSound(integrityChangeType);
				if (CubeGrid.GridSystems.GasSystem != null)
				{
					if (buildRatio > m_componentStack.BuildRatio)
					{
						CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioLowered(this);
					}
					else
					{
						CubeGrid.GridSystems.GasSystem.OnSlimBlockBuildRatioRaised(this);
					}
				}
			}
			if (flag)
			{
				CubeGrid.RenderData.RemoveDecals(Position);
			}
			if (MyFakes.ENABLE_GENERATED_BLOCKS && !BlockDefinition.IsGeneratedBlock && BlockDefinition.GeneratedBlockDefinitions != null && BlockDefinition.GeneratedBlockDefinitions.Length != 0)
			{
				UpdateProgressGeneratedBlocks(buildRatio);
			}
		}

		private void UpdateHackingIndicator(float newRatio, float oldRatio, long grinderOwner)
		{
			if (!(newRatio < oldRatio) || FatBlock == null || FatBlock.IDModule == null)
			{
				return;
			}
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = FatBlock.IDModule.GetUserRelationToOwner(grinderOwner);
			if (userRelationToOwner != MyRelationsBetweenPlayerAndBlock.Enemies && userRelationToOwner != MyRelationsBetweenPlayerAndBlock.Neutral)
			{
				return;
			}
			MyTerminalBlock myTerminalBlock = FatBlock as MyTerminalBlock;
			if (myTerminalBlock != null)
			{
				myTerminalBlock.HackAttemptTime = MySandboxGame.TotalSimulationTimeInMilliseconds;
				if (MySlimBlock.OnAnyBlockHackedChanged != null)
				{
					MySlimBlock.OnAnyBlockHackedChanged(myTerminalBlock, grinderOwner);
				}
			}
		}

		public void PlayConstructionSound(MyIntegrityChangeEnum integrityChangeType, bool deconstruction = false)
		{
			MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
			if (myEntity3DSoundEmitter == null)
			{
				return;
			}
			if (FatBlock != null)
			{
				myEntity3DSoundEmitter.SetPosition(FatBlock.PositionComp.GetPosition());
			}
			else
			{
				myEntity3DSoundEmitter.SetPosition(CubeGrid.PositionComp.GetPosition() + (Position - 1) * CubeGrid.GridSize);
			}
			switch (integrityChangeType)
			{
			case MyIntegrityChangeEnum.ConstructionBegin:
				if (deconstruction)
				{
					myEntity3DSoundEmitter.PlaySound(DECONSTRUCTION_START, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: true);
				}
				else
				{
					myEntity3DSoundEmitter.PlaySound(CONSTRUCTION_START, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: true);
				}
				break;
			case MyIntegrityChangeEnum.DeconstructionBegin:
				myEntity3DSoundEmitter.PlaySound(DECONSTRUCTION_START, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: true);
				break;
			case MyIntegrityChangeEnum.ConstructionEnd:
				if (deconstruction)
				{
					myEntity3DSoundEmitter.PlaySound(DECONSTRUCTION_END, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: true);
				}
				else
				{
					myEntity3DSoundEmitter.PlaySound(CONSTRUCTION_END, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: true);
				}
				break;
			case MyIntegrityChangeEnum.DeconstructionEnd:
				myEntity3DSoundEmitter.PlaySound(DECONSTRUCTION_END, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: true);
				break;
			case MyIntegrityChangeEnum.ConstructionProcess:
				if (deconstruction)
				{
					myEntity3DSoundEmitter.PlaySound(DECONSTRUCTION_PROG, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: true);
				}
				else
				{
					myEntity3DSoundEmitter.PlaySound(CONSTRUCTION_PROG, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: true);
				}
				break;
			case MyIntegrityChangeEnum.DeconstructionProcess:
				myEntity3DSoundEmitter.PlaySound(DECONSTRUCTION_PROG, stopPrevious: true, skipIntro: false, force2D: false, alwaysHearOnRealistic: true);
				break;
			default:
				myEntity3DSoundEmitter.PlaySound(MySoundPair.Empty);
				break;
			}
		}

		private bool ModelChangeIsNeeded(float a, float b)
		{
			if (a > b)
			{
				return BlockDefinition.ModelChangeIsNeeded(b, a);
			}
			return BlockDefinition.ModelChangeIsNeeded(a, b);
		}

		public void UpgradeBuildLevel()
		{
			float buildRatio = m_componentStack.BuildRatio;
			float num = 1f;
			MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = BlockDefinition.BuildProgressModels;
			foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in buildProgressModels)
			{
				if (buildProgressModel.BuildRatioUpperBound > buildRatio && buildProgressModel.BuildRatioUpperBound <= num)
				{
					num = buildProgressModel.BuildRatioUpperBound;
				}
			}
			float num2 = MathHelper.Clamp(num * 1.001f, 0f, 1f);
			m_componentStack.SetIntegrity(num2 * BlockDefinition.MaxIntegrity, num2 * BlockDefinition.MaxIntegrity);
		}

		public void RandomizeBuildLevel()
		{
			float num = MyUtils.GetRandomFloat(0f, 1f) * BlockDefinition.MaxIntegrity;
			m_componentStack.SetIntegrity(num, num);
		}

		internal void ChangeStockpile(List<MyStockpileItem> items)
		{
			EnsureConstructionStockpileExists();
			m_stockpile.Change(items);
			if (m_stockpile.IsEmpty())
			{
				ReleaseConstructionStockpile();
			}
		}

		internal void GetConstructionStockpileItems(List<MyStockpileItem> m_cacheStockpileItems)
		{
			if (m_stockpile != null)
			{
				foreach (MyStockpileItem item in m_stockpile.GetItems())
				{
					m_cacheStockpileItems.Add(item);
				}
			}
		}

		internal void RequestFillStockpile(MyInventory SourceInventory)
		{
			m_tmpComponents.Clear();
			GetMissingComponents(m_tmpComponents);
			using (Dictionary<string, int>.Enumerator enumerator = m_tmpComponents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (SourceInventory.ContainItems(contentId: new MyDefinitionId(subtypeName: enumerator.Current.Key, type: typeof(MyObjectBuilder_Component)), amount: 1))
					{
						CubeGrid.RequestFillStockpile(Position, SourceInventory);
						break;
					}
				}
			}
		}

		public void ComputeWorldCenter(out Vector3D worldCenter)
		{
			ComputeScaledCenter(out worldCenter);
			MatrixD matrix = CubeGrid.WorldMatrix;
			Vector3D.Transform(ref worldCenter, ref matrix, out worldCenter);
		}

		public void ComputeScaledCenter(out Vector3D scaledCenter)
		{
			scaledCenter = (Max + Min) * CubeGrid.GridSizeHalf;
		}

		public void ComputeScaledHalfExtents(out Vector3 scaledHalfExtents)
		{
			scaledHalfExtents = (Max + 1 - Min) * CubeGrid.GridSizeHalf;
		}

		public float GetMass()
		{
			if (FatBlock != null)
			{
				return FatBlock.GetMass();
			}
			Matrix orientation;
			if (MyDestructionData.Static != null)
			{
				return MyDestructionData.Static.GetBlockMass(CalculateCurrentModel(out orientation), BlockDefinition);
			}
			return BlockDefinition.Mass;
		}

		public void OnDestroyVisual()
		{
			if (MyFakes.SHOW_DAMAGE_EFFECTS && !CubeGrid.IsLargeDestroyInProgress)
			{
				bool num = (FatBlock != null && FatBlock.IsBuilt) || FatBlock == null;
				string text = num ? BlockDefinition.DestroyEffect : null;
				if (string.IsNullOrEmpty(text))
				{
					text = ((CubeGrid.GridSizeEnum == MyCubeSize.Large) ? "BlockDestroyed_Large3X" : "BlockDestroyed_Large");
				}
				MySoundPair mySoundPair = num ? BlockDefinition.DestroySound : null;
				if (mySoundPair == null || mySoundPair == MySoundPair.Empty)
				{
					mySoundPair = ((CubeGrid.GridSizeEnum == MyCubeSize.Large) ? MyExplosion.LargePoofSound : MyExplosion.SmallPoofSound);
				}
				bool flag = (FatBlock != null && FatBlock.Model != null && FatBlock.Model.BoundingSphere.Radius > 0.5f) || FatBlock == null;
				Vector3D vector3D = Vector3D.Zero;
				if (BlockDefinition.DestroyEffectOffset.HasValue && !BlockDefinition.DestroyEffectOffset.Value.Equals(Vector3.Zero))
				{
					Orientation.GetMatrix(out Matrix result);
					vector3D = Vector3D.Rotate(new Vector3D(Vector3.RotateAndScale(BlockDefinition.DestroyEffectOffset.Value, result)), CubeGrid.WorldMatrix);
				}
				BoundingSphereD explosionSphere = BoundingSphereD.CreateFromBoundingBox(WorldAABB);
				explosionSphere.Center += vector3D;
				if (MyFakes.DEBUG_DISPLAY_DESTROY_EFFECT_OFFSET)
				{
					Orientation.GetMatrix(out Matrix result2);
					MatrixD matrix = MatrixD.Multiply(new MatrixD(result2), CubeGrid.WorldMatrix);
					matrix.Translation = WorldPosition;
					MyRenderProxy.DebugDrawAxis(matrix, 1f, depthRead: false, skipScale: false, persistent: true);
					MyRenderProxy.DebugDrawLine3D(WorldPosition, explosionSphere.Center, Color.Red, Color.Yellow, depthRead: false, persistent: true);
				}
				bool flag2 = CubeGrid.Physics != null && CubeGrid.Physics.IsPlanetCrashing_PointConcealed(WorldPosition);
				MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
				myExplosionInfo.PlayerDamage = 0f;
				myExplosionInfo.Damage = 0f;
				myExplosionInfo.ExplosionType = MyExplosionTypeEnum.CUSTOM;
				myExplosionInfo.ExplosionSphere = explosionSphere;
				myExplosionInfo.LifespanMiliseconds = 700;
				myExplosionInfo.HitEntity = CubeGrid;
				myExplosionInfo.ParticleScale = 1f;
				myExplosionInfo.CustomEffect = text;
				myExplosionInfo.CustomSound = mySoundPair;
				myExplosionInfo.OwnerEntity = CubeGrid;
				myExplosionInfo.Direction = CubeGrid.WorldMatrix.Forward;
				myExplosionInfo.VoxelExplosionCenter = WorldPosition + vector3D;
				myExplosionInfo.ExplosionFlags = (MyExplosionFlags)((flag ? 1 : 0) | 8 | ((!flag2) ? 32 : 0));
				myExplosionInfo.VoxelCutoutScale = 0f;
				myExplosionInfo.PlaySound = true;
				myExplosionInfo.ApplyForceAndDamage = true;
				myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
				MyExplosionInfo explosionInfo = myExplosionInfo;
				if (CubeGrid.Physics != null)
				{
					explosionInfo.Velocity = CubeGrid.Physics.LinearVelocity;
				}
				MyExplosions.AddExplosion(ref explosionInfo, updateSync: false);
			}
		}

		void IMyDestroyableObject.OnDestroy()
		{
			if (FatBlock != null)
			{
				FatBlock.OnDestroy();
			}
			OnDestroyVisual();
			m_componentStack.DestroyCompletely();
			ReleaseUnneededStockpileItems();
			CubeGrid.RemoveFromDamageApplication(this);
			AccumulatedDamage = 0f;
		}

		internal void Transform(ref MatrixI transform)
		{
			Vector3I.Transform(ref Min, ref transform, out Vector3I result);
			Vector3I.Transform(ref Max, ref transform, out Vector3I result2);
			Vector3I.Transform(ref Position, ref transform, out Vector3I result3);
			Vector3I forward = Base6Directions.GetIntVector(transform.GetDirection(Orientation.Forward));
			Vector3I up = Base6Directions.GetIntVector(transform.GetDirection(Orientation.Up));
			InitOrientation(ref forward, ref up);
			Min = Vector3I.Min(result, result2);
			Max = Vector3I.Max(result, result2);
			Position = result3;
			if (FatBlock != null)
			{
				FatBlock.OnTransformed(ref transform);
			}
		}

		public void GetWorldBoundingBox(out BoundingBoxD aabb, bool useAABBFromBlockCubes = false)
		{
			if (FatBlock != null && !useAABBFromBlockCubes)
			{
				aabb = FatBlock.PositionComp.WorldAABB;
				return;
			}
			float gridSize = CubeGrid.GridSize;
			aabb = new BoundingBoxD(Min * gridSize - gridSize / 2f, Max * gridSize + gridSize / 2f);
			aabb = aabb.TransformFast(CubeGrid.WorldMatrix);
		}

		public static void SetBlockComponents(MyHudBlockInfo hudInfo, MySlimBlock block, MyInventoryBase availableInventory = null)
		{
			SetBlockComponentsInternal(hudInfo, block.BlockDefinition, block, availableInventory);
		}

		public static void SetBlockComponents(MyHudBlockInfo hudInfo, MyCubeBlockDefinition blockDefinition, MyInventoryBase availableInventory = null)
		{
			SetBlockComponentsInternal(hudInfo, blockDefinition, null, availableInventory);
		}

		private static void SetBlockComponentsInternal(MyHudBlockInfo hudInfo, MyCubeBlockDefinition blockDefinition, MySlimBlock block, MyInventoryBase availableInventory)
		{
			hudInfo.Components.Clear();
			hudInfo.InitBlockInfo(blockDefinition);
			hudInfo.ShowAvailable = MyPerGameSettings.AlwaysShowAvailableBlocksOnHud;
			if (!MyFakes.ENABLE_SMALL_GRID_BLOCK_COMPONENT_INFO && blockDefinition.CubeSize == MyCubeSize.Small)
			{
				return;
			}
			if (block != null)
			{
				hudInfo.BlockIntegrity = block.Integrity / block.MaxIntegrity;
			}
			if (block != null && block.IsMultiBlockPart)
			{
				MyCubeGridMultiBlockInfo multiBlockInfo = block.CubeGrid.GetMultiBlockInfo(block.MultiBlockId);
				if (multiBlockInfo == null)
				{
					return;
				}
				MyMultiBlockDefinition.MyMultiBlockPartDefinition[] blockDefinitions = multiBlockInfo.MultiBlockDefinition.BlockDefinitions;
				foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition in blockDefinitions)
				{
					if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(myMultiBlockPartDefinition.Id, out MyCubeBlockDefinition blockDefinition2))
					{
						hudInfo.AddComponentsForBlock(blockDefinition2);
					}
				}
				hudInfo.MergeSameComponents();
				foreach (MySlimBlock block2 in multiBlockInfo.Blocks)
				{
					for (int j = 0; j < block2.BlockDefinition.Components.Length; j++)
					{
						MyCubeBlockDefinition.Component component = block2.BlockDefinition.Components[j];
						MyComponentStack.GroupInfo groupInfo = block2.ComponentStack.GetGroupInfo(j);
						for (int k = 0; k < hudInfo.Components.Count; k++)
						{
							if (hudInfo.Components[k].DefinitionId == component.Definition.Id)
							{
								MyHudBlockInfo.ComponentInfo value = hudInfo.Components[k];
								value.MountedCount += groupInfo.MountedCount;
								hudInfo.Components[k] = value;
								break;
							}
						}
					}
				}
				for (int l = 0; l < hudInfo.Components.Count; l++)
				{
					if (availableInventory != null)
					{
						MyHudBlockInfo.ComponentInfo value2 = hudInfo.Components[l];
						value2.AvailableAmount = (int)MyCubeBuilder.BuildComponent.GetItemAmountCombined(availableInventory, value2.DefinitionId);
						hudInfo.Components[l] = value2;
					}
					int num = 0;
					foreach (MySlimBlock block3 in multiBlockInfo.Blocks)
					{
						if (!block3.StockpileEmpty)
						{
							num += block3.GetConstructionStockpileItemAmount(hudInfo.Components[l].DefinitionId);
						}
					}
					if (num > 0)
					{
						SetHudInfoComponentAmount(hudInfo, num, l);
					}
				}
				return;
			}
			if (block == null && blockDefinition.MultiBlock != null)
			{
				MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_MultiBlockDefinition), blockDefinition.MultiBlock);
				MyMultiBlockDefinition myMultiBlockDefinition = MyDefinitionManager.Static.TryGetMultiBlockDefinition(id);
				if (myMultiBlockDefinition == null)
				{
					return;
				}
				MyMultiBlockDefinition.MyMultiBlockPartDefinition[] blockDefinitions = myMultiBlockDefinition.BlockDefinitions;
				foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition2 in blockDefinitions)
				{
					if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(myMultiBlockPartDefinition2.Id, out MyCubeBlockDefinition blockDefinition3))
					{
						hudInfo.AddComponentsForBlock(blockDefinition3);
					}
				}
				hudInfo.MergeSameComponents();
				for (int m = 0; m < hudInfo.Components.Count; m++)
				{
					MyHudBlockInfo.ComponentInfo value3 = hudInfo.Components[m];
					value3.AvailableAmount = (int)MyCubeBuilder.BuildComponent.GetItemAmountCombined(availableInventory, value3.DefinitionId);
					hudInfo.Components[m] = value3;
				}
				return;
			}
			for (int n = 0; n < blockDefinition.Components.Length; n++)
			{
				MyComponentStack.GroupInfo groupInfo2 = default(MyComponentStack.GroupInfo);
				if (block != null)
				{
					groupInfo2 = block.ComponentStack.GetGroupInfo(n);
				}
				else
				{
					MyCubeBlockDefinition.Component component2 = blockDefinition.Components[n];
					groupInfo2.Component = component2.Definition;
					groupInfo2.TotalCount = component2.Count;
					groupInfo2.MountedCount = 0;
					groupInfo2.AvailableCount = 0;
					groupInfo2.Integrity = 0f;
					groupInfo2.MaxIntegrity = component2.Count * component2.Definition.MaxIntegrity;
				}
				AddBlockComponent(hudInfo, groupInfo2, availableInventory);
			}
			if (block == null || block.StockpileEmpty)
			{
				return;
			}
			MyCubeBlockDefinition.Component[] components = block.BlockDefinition.Components;
			foreach (MyCubeBlockDefinition.Component component3 in components)
			{
				int num2 = block.GetConstructionStockpileItemAmount(component3.Definition.Id);
				if (num2 <= 0)
				{
					continue;
				}
				for (int num3 = 0; num3 < hudInfo.Components.Count; num3++)
				{
					if (block.ComponentStack.GetGroupInfo(num3).Component == component3.Definition)
					{
						if (block.ComponentStack.IsFullyDismounted)
						{
							return;
						}
						num2 = SetHudInfoComponentAmount(hudInfo, num2, num3);
					}
				}
			}
		}

		private static int SetHudInfoComponentAmount(MyHudBlockInfo hudInfo, int amount, int i)
		{
			MyHudBlockInfo.ComponentInfo value = hudInfo.Components[i];
			amount -= (value.StockpileCount = Math.Min(value.TotalCount - value.MountedCount, amount));
			hudInfo.Components[i] = value;
			return amount;
		}

		private static void AddBlockComponent(MyHudBlockInfo hudInfo, MyComponentStack.GroupInfo groupInfo, MyInventoryBase availableInventory)
		{
			MyHudBlockInfo.ComponentInfo item = default(MyHudBlockInfo.ComponentInfo);
			item.DefinitionId = groupInfo.Component.Id;
			item.ComponentName = groupInfo.Component.DisplayNameText;
			item.Icons = groupInfo.Component.Icons;
			item.TotalCount = groupInfo.TotalCount;
			item.MountedCount = groupInfo.MountedCount;
			if (availableInventory != null)
			{
				item.AvailableAmount = (int)MyCubeBuilder.BuildComponent.GetItemAmountCombined(availableInventory, groupInfo.Component.Id);
			}
			hudInfo.Components.Add(item);
		}

		private void UpdateProgressGeneratedBlocks(float oldBuildRatio)
		{
			float buildRatio = ComponentStack.BuildRatio;
			if (oldBuildRatio == buildRatio)
			{
				return;
			}
			if (oldBuildRatio < buildRatio)
			{
				if (oldBuildRatio < BlockDefinition.BuildProgressToPlaceGeneratedBlocks && buildRatio >= BlockDefinition.BuildProgressToPlaceGeneratedBlocks)
				{
					CubeGrid.AdditionalModelGenerators.ForEach(delegate(IMyBlockAdditionalModelGenerator g)
					{
						g.GenerateBlocks(this);
					});
				}
			}
			else if (oldBuildRatio >= BlockDefinition.BuildProgressToPlaceGeneratedBlocks && buildRatio < BlockDefinition.BuildProgressToPlaceGeneratedBlocks)
			{
				m_tmpBlocks.Clear();
				CubeGrid.GetGeneratedBlocks(this, m_tmpBlocks);
				CubeGrid.RazeGeneratedBlocks(m_tmpBlocks);
				m_tmpBlocks.Clear();
			}
		}

		public MyFractureComponentCubeBlock GetFractureComponent()
		{
			if (FatBlock == null)
			{
				return null;
			}
			return FatBlock.GetFractureComponent();
		}

		private void RepairMultiBlock(long toolOwnerId)
		{
			MyCubeGridMultiBlockInfo multiBlockInfo = CubeGrid.GetMultiBlockInfo(MultiBlockId);
			if (multiBlockInfo != null && multiBlockInfo.IsFractured())
			{
				m_tmpMultiBlocks.AddRange(multiBlockInfo.Blocks);
				foreach (MySlimBlock tmpMultiBlock in m_tmpMultiBlocks)
				{
					if (tmpMultiBlock.GetFractureComponent() != null)
					{
						tmpMultiBlock.RepairFracturedBlock(toolOwnerId);
					}
				}
				m_tmpMultiBlocks.Clear();
			}
		}

		public void RepairFracturedBlockWithFullHealth(long toolOwnerId)
		{
			if (BlockDefinition.IsGeneratedBlock)
			{
				return;
			}
			if (MyFakes.ENABLE_MULTIBLOCK_CONSTRUCTION && IsMultiBlockPart)
			{
				RepairMultiBlock(toolOwnerId);
				if (!MySession.Static.SurvivalMode)
				{
					CubeGrid.AddMissingBlocksInMultiBlock(MultiBlockId, toolOwnerId);
				}
			}
			else if (GetFractureComponent() != null)
			{
				RepairFracturedBlock(toolOwnerId);
			}
		}

		internal void RepairFracturedBlock(long toolOwnerId)
		{
			if (FatBlock == null)
			{
				return;
			}
			RemoveFractureComponent();
			CubeGrid.GetGeneratedBlocks(this, m_tmpBlocks);
			foreach (MySlimBlock tmpBlock in m_tmpBlocks)
			{
				tmpBlock.RemoveFractureComponent();
				tmpBlock.SetGeneratedBlockIntegrity(this);
			}
			m_tmpBlocks.Clear();
			UpdateProgressGeneratedBlocks(0f);
			if (Sync.IsServer)
			{
				BoundingBoxD box = FatBlock.PositionComp.WorldAABB;
				if (BlockDefinition.CubeSize == MyCubeSize.Large)
				{
					box.Inflate(-0.16);
				}
				else
				{
					box.Inflate(-0.04);
				}
				MyFracturedPiecesManager.Static.RemoveFracturesInBox(ref box, 0f);
				CubeGrid.SendFractureComponentRepaired(this, toolOwnerId);
			}
		}

		internal void RemoveFractureComponent()
		{
			if (FatBlock.Components.Has<MyFractureComponentBase>())
			{
				FatBlock.Components.Remove<MyFractureComponentBase>();
				FatBlock.Render.UpdateRenderObject(visible: false);
				FatBlock.CreateRenderer(FatBlock.Render.PersistentFlags, FatBlock.Render.ColorMaskHsv, FatBlock.Render.ModelStorage);
				UpdateVisual();
				FatBlock.Render.UpdateRenderObject(visible: true);
				MySlimBlock cubeBlock = CubeGrid.GetCubeBlock(Position);
				cubeBlock?.CubeGrid.UpdateBlockNeighbours(cubeBlock);
			}
		}

		public void SetGeneratedBlockIntegrity(MySlimBlock generatingBlock)
		{
			if (BlockDefinition.IsGeneratedBlock)
			{
				float buildRatio = ComponentStack.BuildRatio;
				ComponentStack.SetIntegrity(generatingBlock.BuildLevelRatio * MaxIntegrity, generatingBlock.ComponentStack.IntegrityRatio * MaxIntegrity);
				if (ModelChangeIsNeeded(ComponentStack.BuildRatio, buildRatio))
				{
					UpdateVisual();
				}
			}
		}

		public void GetLocalMatrix(out Matrix localMatrix)
		{
			Orientation.GetMatrix(out localMatrix);
			localMatrix.Translation = (Min + Max) * 0.5f * CubeGrid.GridSize;
			Vector3.TransformNormal(ref BlockDefinition.ModelOffset, ref localMatrix, out Vector3 result);
			localMatrix.Translation += result;
		}

		private static void DoDamageSynced(MySlimBlock block, float damage, MyStringHash damageType, MyHitInfo? hitInfo, long attackerId)
		{
			SendDamage(block, damage, damageType, hitInfo, attackerId);
			block.DoDamage(damage, damageType, hitInfo, addDirtyParts: true, attackerId);
		}

		public static void SendDamageBatch(Dictionary<MySlimBlock, float> blocks, MyStringHash damageType, long attackerId)
		{
			if (blocks.Count != 0)
			{
				MyCubeGrid cubeGrid = blocks.FirstPair().Key.CubeGrid;
				if (!cubeGrid.MarkedForClose)
				{
					MyUtils.ClearCollectionToken<List<MyTuple<Vector3I, float>>, MyTuple<Vector3I, float>> clearCollectionToken = MyUtils.ReuseCollection(ref m_batchCache);
					try
					{
						List<MyTuple<Vector3I, float>> collection = clearCollectionToken.Collection;
						foreach (KeyValuePair<MySlimBlock, float> block in blocks)
						{
							MySlimBlock key = block.Key;
							if (cubeGrid.EntityId == key.CubeGrid.EntityId && !key.IsDestroyed)
							{
								collection.Add(MyTuple.Create(key.Position, block.Value));
							}
						}
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => DoDamageSlimBlockBatch, cubeGrid.EntityId, collection, damageType, attackerId);
					}
					finally
					{
						((IDisposable)clearCollectionToken).Dispose();
					}
				}
			}
		}

		[Event(null, 2812)]
		[Reliable]
		[Broadcast]
		private static void DoDamageSlimBlockBatch(long gridId, List<MyTuple<Vector3I, float>> blocks, MyStringHash damageType, long attackerId)
		{
			if (MyEntities.TryGetEntityById(gridId, out MyCubeGrid entity))
			{
				foreach (MyTuple<Vector3I, float> block in blocks)
				{
					MySlimBlock cubeBlock = entity.GetCubeBlock(block.Item1);
					if (cubeBlock == null || cubeBlock.IsDestroyed)
					{
						break;
					}
					float item = block.Item2;
					cubeBlock.DoDamage(item, damageType, null, addDirtyParts: true, attackerId);
				}
			}
		}

		public static void SendDamage(MySlimBlock block, float damage, MyStringHash damageType, MyHitInfo? hitInfo, long attackerId)
		{
			DoDamageSlimBlockMsg doDamageSlimBlockMsg = default(DoDamageSlimBlockMsg);
			doDamageSlimBlockMsg.GridEntityId = block.CubeGrid.EntityId;
			doDamageSlimBlockMsg.Position = block.Position;
			doDamageSlimBlockMsg.Damage = damage;
			doDamageSlimBlockMsg.HitInfo = hitInfo;
			doDamageSlimBlockMsg.AttackerEntityId = attackerId;
			doDamageSlimBlockMsg.CompoundBlockId = uint.MaxValue;
			doDamageSlimBlockMsg.Type = damageType;
			DoDamageSlimBlockMsg arg = doDamageSlimBlockMsg;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => DoDamageSlimBlock, arg);
		}

		[Event(null, 2856)]
		[Reliable]
		[Broadcast]
		private static void DoDamageSlimBlock(DoDamageSlimBlockMsg msg)
		{
			if (!MyEntities.TryGetEntityById(msg.GridEntityId, out MyCubeGrid entity))
			{
				return;
			}
			MySlimBlock mySlimBlock = entity.GetCubeBlock(msg.Position);
			if (mySlimBlock == null || mySlimBlock.IsDestroyed)
			{
				return;
			}
			if (msg.CompoundBlockId != uint.MaxValue && mySlimBlock.FatBlock is MyCompoundCubeBlock)
			{
				MySlimBlock block = (mySlimBlock.FatBlock as MyCompoundCubeBlock).GetBlock((ushort)msg.CompoundBlockId);
				if (block != null)
				{
					mySlimBlock = block;
				}
			}
			mySlimBlock.DoDamage(msg.Damage, msg.Type, msg.HitInfo, addDirtyParts: true, msg.AttackerEntityId);
		}

		public void RemoveAuthorship()
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(m_builtByID);
			if (myIdentity != null)
			{
				int pcu = 1;
				if (ComponentStack.IsFunctional)
				{
					pcu = BlockDefinition.PCU;
				}
				myIdentity.BlockLimits.DecreaseBlocksBuilt(BlockDefinition.BlockPairName, pcu, CubeGrid);
				if (myIdentity.BlockLimits != MySession.Static.GlobalBlockLimits && myIdentity.BlockLimits != MySession.Static.PirateBlockLimits)
				{
					MySession.Static.GlobalBlockLimits.DecreaseBlocksBuilt(BlockDefinition.BlockPairName, pcu, CubeGrid);
				}
			}
		}

		public void AddAuthorship()
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(m_builtByID);
			if (myIdentity != null)
			{
				int pcu = 1;
				if (ComponentStack.IsFunctional)
				{
					pcu = BlockDefinition.PCU;
				}
				myIdentity.BlockLimits.IncreaseBlocksBuilt(BlockDefinition.BlockPairName, pcu, CubeGrid);
				if (myIdentity.BlockLimits != MySession.Static.GlobalBlockLimits && myIdentity.BlockLimits != MySession.Static.PirateBlockLimits)
				{
					MySession.Static.GlobalBlockLimits.IncreaseBlocksBuilt(BlockDefinition.BlockPairName, pcu, CubeGrid);
				}
			}
			else
			{
				_ = m_builtByID;
			}
		}

		public void TransferAuthorship(long newOwner)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(m_builtByID);
			MyIdentity myIdentity2 = MySession.Static.Players.TryGetIdentity(newOwner);
			if (myIdentity != null && myIdentity2 != null)
			{
				int pcu = 1;
				if (ComponentStack.IsFunctional)
				{
					pcu = BlockDefinition.PCU;
				}
				myIdentity.BlockLimits.DecreaseBlocksBuilt(BlockDefinition.BlockPairName, pcu, CubeGrid);
				m_builtByID = newOwner;
				myIdentity2.BlockLimits.IncreaseBlocksBuilt(BlockDefinition.BlockPairName, pcu, CubeGrid);
			}
		}

		public void TransferAuthorshipClient(long newOwner)
		{
			m_builtByID = newOwner;
		}

		public void TransferLimits(MyBlockLimits oldLimits, MyBlockLimits newLimits)
		{
			int pcu = 1;
			if (ComponentStack.IsFunctional)
			{
				pcu = BlockDefinition.PCU;
			}
			oldLimits.DecreaseBlocksBuilt(BlockDefinition.BlockPairName, pcu, CubeGrid);
			newLimits.IncreaseBlocksBuilt(BlockDefinition.BlockPairName, pcu, CubeGrid);
		}

		void VRage.Game.ModAPI.IMySlimBlock.AddNeighbours()
		{
			AddNeighbours();
		}

		void VRage.Game.ModAPI.IMySlimBlock.ApplyAccumulatedDamage(bool addDirtyParts)
		{
			ApplyAccumulatedDamage(addDirtyParts, 0L);
		}

		string VRage.Game.ModAPI.IMySlimBlock.CalculateCurrentModel(out Matrix orientation)
		{
			return CalculateCurrentModel(out orientation);
		}

		void VRage.Game.ModAPI.IMySlimBlock.ComputeScaledCenter(out Vector3D scaledCenter)
		{
			ComputeScaledCenter(out scaledCenter);
		}

		void VRage.Game.ModAPI.IMySlimBlock.ComputeScaledHalfExtents(out Vector3 scaledHalfExtents)
		{
			ComputeScaledHalfExtents(out scaledHalfExtents);
		}

		void VRage.Game.ModAPI.IMySlimBlock.ComputeWorldCenter(out Vector3D worldCenter)
		{
			ComputeWorldCenter(out worldCenter);
		}

		void VRage.Game.ModAPI.IMySlimBlock.FixBones(float oldDamage, float maxAllowedBoneMovement)
		{
			FixBones(oldDamage, maxAllowedBoneMovement);
		}

		void VRage.Game.ModAPI.IMySlimBlock.FullyDismount(VRage.Game.ModAPI.IMyInventory outputInventory)
		{
			FullyDismount(outputInventory as MyInventory);
		}

		MyObjectBuilder_CubeBlock VRage.Game.ModAPI.IMySlimBlock.GetCopyObjectBuilder()
		{
			return GetCopyObjectBuilder();
		}

		MyObjectBuilder_CubeBlock VRage.Game.ModAPI.IMySlimBlock.GetObjectBuilder(bool copy)
		{
			return GetObjectBuilder(copy);
		}

		void VRage.Game.ModAPI.IMySlimBlock.InitOrientation(ref Vector3I forward, ref Vector3I up)
		{
			InitOrientation(ref forward, ref up);
		}

		void VRage.Game.ModAPI.IMySlimBlock.InitOrientation(Base6Directions.Direction Forward, Base6Directions.Direction Up)
		{
			InitOrientation(Forward, Up);
		}

		void VRage.Game.ModAPI.IMySlimBlock.InitOrientation(MyBlockOrientation orientation)
		{
			InitOrientation(orientation);
		}

		void VRage.Game.ModAPI.IMySlimBlock.RemoveNeighbours()
		{
			RemoveNeighbours();
		}

		void VRage.Game.ModAPI.IMySlimBlock.SetToConstructionSite()
		{
			SetToConstructionSite();
		}

		void VRage.Game.ModAPI.IMySlimBlock.SpawnConstructionStockpile()
		{
			SpawnConstructionStockpile();
		}

		void VRage.Game.ModAPI.IMySlimBlock.MoveItemsFromConstructionStockpile(VRage.Game.ModAPI.IMyInventory toInventory, MyItemFlags flags)
		{
			MoveItemsFromConstructionStockpile(toInventory as MyInventory, flags);
		}

		void VRage.Game.ModAPI.IMySlimBlock.SpawnFirstItemInConstructionStockpile()
		{
			SpawnFirstItemInConstructionStockpile();
		}

		void VRage.Game.ModAPI.IMySlimBlock.UpdateVisual()
		{
			UpdateVisual();
		}

		void VRage.Game.ModAPI.Ingame.IMySlimBlock.GetMissingComponents(Dictionary<string, int> addToDictionary)
		{
			GetMissingComponents(addToDictionary);
		}

		Vector3 VRage.Game.ModAPI.IMySlimBlock.GetColorMask()
		{
			return ColorMaskHSV;
		}

		void VRage.Game.ModAPI.IMySlimBlock.DecreaseMountLevel(float grinderAmount, VRage.Game.ModAPI.IMyInventory outputInventory, bool useDefaultDeconstructEfficiency)
		{
			DecreaseMountLevel(grinderAmount, outputInventory as MyInventoryBase, useDefaultDeconstructEfficiency, 0L);
		}

		void VRage.Game.ModAPI.IMySlimBlock.IncreaseMountLevel(float welderMountAmount, long welderOwnerPlayerId, VRage.Game.ModAPI.IMyInventory outputInventory, float maxAllowedBoneMovement, bool isHelping, MyOwnershipShareModeEnum share)
		{
			IncreaseMountLevel(welderMountAmount, welderOwnerPlayerId, outputInventory as MyInventoryBase, maxAllowedBoneMovement, isHelping, share);
		}

		int VRage.Game.ModAPI.IMySlimBlock.GetConstructionStockpileItemAmount(MyDefinitionId id)
		{
			return GetConstructionStockpileItemAmount(id);
		}

		void VRage.Game.ModAPI.IMySlimBlock.MoveItemsToConstructionStockpile(VRage.Game.ModAPI.IMyInventory fromInventory)
		{
			MoveItemsToConstructionStockpile(fromInventory as MyInventoryBase);
		}

		void VRage.Game.ModAPI.IMySlimBlock.ClearConstructionStockpile(VRage.Game.ModAPI.IMyInventory outputInventory)
		{
			ClearConstructionStockpile(outputInventory as MyInventoryBase);
		}

		bool VRage.Game.ModAPI.IMySlimBlock.CanContinueBuild(VRage.Game.ModAPI.IMyInventory sourceInventory)
		{
			return CanContinueBuild(sourceInventory as MyInventory);
		}

		void VRage.Game.ModAPI.IMySlimBlock.GetWorldBoundingBox(out BoundingBoxD aabb, bool useAABBFromBlockCubes)
		{
			GetWorldBoundingBox(out aabb, useAABBFromBlockCubes);
		}
	}
}
