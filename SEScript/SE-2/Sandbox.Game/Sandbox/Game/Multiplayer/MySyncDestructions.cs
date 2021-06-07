using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	[PreloadRequired]
	public static class MySyncDestructions
	{
		protected sealed class OnAddDestructionEffectMessage_003C_003ESystem_String_0023VRageMath_Vector3D_0023VRageMath_Vector3_0023System_Single : ICallSite<IMyEventOwner, string, Vector3D, Vector3, float, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string effectName, in Vector3D position, in Vector3 direction, in float scale, in DBNull arg5, in DBNull arg6)
			{
				OnAddDestructionEffectMessage(effectName, position, direction, scale);
			}
		}

		protected sealed class OnCreateFracturePieceMessage_003C_003EVRage_Game_MyObjectBuilder_FracturedPiece : ICallSite<IMyEventOwner, MyObjectBuilder_FracturedPiece, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_FracturedPiece fracturePiece, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCreateFracturePieceMessage(fracturePiece);
			}
		}

		protected sealed class OnRemoveFracturePieceMessage_003C_003ESystem_Int64_0023System_Single : ICallSite<IMyEventOwner, long, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in float blendTime, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRemoveFracturePieceMessage(entityId, blendTime);
			}
		}

		protected sealed class OnFPManagerDbgMessage_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long createdId, in long removedId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnFPManagerDbgMessage(createdId, removedId);
			}
		}

		protected sealed class OnCreateFracturedBlockMessage_003C_003ESystem_Int64_0023VRageMath_Vector3I_0023VRage_Game_MyObjectBuilder_FracturedBlock : ICallSite<IMyEventOwner, long, Vector3I, MyObjectBuilder_FracturedBlock, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridId, in Vector3I position, in MyObjectBuilder_FracturedBlock fracturedBlock, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCreateFracturedBlockMessage(gridId, position, fracturedBlock);
			}
		}

		protected sealed class OnCreateFractureComponentMessage_003C_003ESystem_Int64_0023VRageMath_Vector3I_0023System_UInt16_0023VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase : ICallSite<IMyEventOwner, long, Vector3I, ushort, MyObjectBuilder_FractureComponentBase, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridId, in Vector3I position, in ushort compoundBlockId, in MyObjectBuilder_FractureComponentBase component, in DBNull arg5, in DBNull arg6)
			{
				OnCreateFractureComponentMessage(gridId, position, compoundBlockId, component);
			}
		}

		protected sealed class OnRemoveShapeFromFractureComponentMessage_003C_003ESystem_Int64_0023VRageMath_Vector3I_0023System_UInt16_0023System_String_003C_0023_003E : ICallSite<IMyEventOwner, long, Vector3I, ushort, string[], DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridId, in Vector3I position, in ushort compoundBlockId, in string[] shapeNames, in DBNull arg5, in DBNull arg6)
			{
				OnRemoveShapeFromFractureComponentMessage(gridId, position, compoundBlockId, shapeNames);
			}
		}

		protected sealed class OnRemoveFracturedPiecesMessage_003C_003ESystem_UInt64_0023VRageMath_Vector3D_0023System_Single : ICallSite<IMyEventOwner, ulong, Vector3D, float, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong userId, in Vector3D center, in float radius, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRemoveFracturedPiecesMessage(userId, center, radius);
			}
		}

		public static void AddDestructionEffect(string effectName, Vector3D position, Vector3 direction, float scale)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnAddDestructionEffectMessage, effectName, position, direction, scale);
		}

		[Event(null, 43)]
		[Server]
		[Broadcast]
		private static void OnAddDestructionEffectMessage(string effectName, Vector3D position, Vector3 direction, float scale)
		{
			MyGridPhysics.CreateDestructionEffect(effectName, position, direction, scale);
		}

		public static void CreateFracturePiece(MyObjectBuilder_FracturedPiece fracturePiece)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnCreateFracturePieceMessage, fracturePiece);
		}

		[Event(null, 55)]
		[Reliable]
		[Broadcast]
		private static void OnCreateFracturePieceMessage([Serialize(MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))] MyObjectBuilder_FracturedPiece fracturePiece)
		{
			MyFracturedPiece pieceFromPool = MyFracturedPiecesManager.Static.GetPieceFromPool(fracturePiece.EntityId, fromServer: true);
			try
			{
				pieceFromPool.Init(fracturePiece);
				MyEntities.Add(pieceFromPool);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Cannot add fracture piece: " + ex.Message);
				if (pieceFromPool != null)
				{
					MyFracturedPiecesManager.Static.RemoveFracturePiece(pieceFromPool, 0f, fromServer: true, sync: false);
					StringBuilder stringBuilder = new StringBuilder();
					foreach (MyObjectBuilder_FracturedPiece.Shape shape in fracturePiece.Shapes)
					{
						stringBuilder.Append(shape.Name).Append(" ");
					}
					MyLog.Default.WriteLine("Received fracture piece not added, no shape found. Shapes: " + stringBuilder.ToString());
				}
			}
		}

		public static void RemoveFracturePiece(long entityId, float blendTime)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRemoveFracturePieceMessage, entityId, blendTime);
		}

		[Event(null, 90)]
		[Reliable]
		[Broadcast]
		private static void OnRemoveFracturePieceMessage(long entityId, float blendTime)
		{
			if (MyEntities.TryGetEntityById(entityId, out MyFracturedPiece entity))
			{
				MyFracturedPiecesManager.Static.RemoveFracturePiece(entity, blendTime, fromServer: true, sync: false);
			}
		}

		[Conditional("DEBUG")]
		public static void FPManagerDbgMessage(long createdId, long removedId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnFPManagerDbgMessage, createdId, removedId);
		}

		[Event(null, 112)]
		[Reliable]
		[Server]
		private static void OnFPManagerDbgMessage(long createdId, long removedId)
		{
			MyFracturedPiecesManager.Static.DbgCheck(createdId, removedId);
		}

		public static void CreateFracturedBlock(MyObjectBuilder_FracturedBlock fracturedBlock, long gridId, Vector3I position)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnCreateFracturedBlockMessage, gridId, position, fracturedBlock);
		}

		[Event(null, 125)]
		[Reliable]
		[Broadcast]
		private static void OnCreateFracturedBlockMessage(long gridId, Vector3I position, [Serialize(MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))] MyObjectBuilder_FracturedBlock fracturedBlock)
		{
			if (MyEntities.TryGetEntityById(gridId, out MyCubeGrid entity))
			{
				entity.EnableGenerators(enable: false, fromServer: true);
				entity.CreateFracturedBlock(fracturedBlock, position);
				entity.EnableGenerators(enable: true, fromServer: true);
			}
		}

		public static void CreateFractureComponent(long gridId, Vector3I position, ushort compoundBlockId, MyObjectBuilder_FractureComponentBase component)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnCreateFractureComponentMessage, gridId, position, compoundBlockId, component);
		}

		[Event(null, 147)]
		[Reliable]
		[Broadcast]
		private static void OnCreateFractureComponentMessage(long gridId, Vector3I position, ushort compoundBlockId, [Serialize(MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))] MyObjectBuilder_FractureComponentBase component)
		{
			if (!MyEntities.TryGetEntityById(gridId, out MyEntity entity))
			{
				return;
			}
			MySlimBlock cubeBlock = (entity as MyCubeGrid).GetCubeBlock(position);
			if (cubeBlock == null || cubeBlock.FatBlock == null)
			{
				return;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
			if (myCompoundCubeBlock != null)
			{
				MySlimBlock block = myCompoundCubeBlock.GetBlock(compoundBlockId);
				if (block != null)
				{
					AddFractureComponent(component, block.FatBlock);
				}
			}
			else
			{
				AddFractureComponent(component, cubeBlock.FatBlock);
			}
		}

		private static void AddFractureComponent(MyObjectBuilder_FractureComponentBase obFractureComponent, MyEntity entity)
		{
			MyFractureComponentBase myFractureComponentBase = MyComponentFactory.CreateInstanceByTypeId(obFractureComponent.TypeId) as MyFractureComponentBase;
			if (myFractureComponentBase != null)
			{
				try
				{
					if (!entity.Components.Has<MyFractureComponentBase>())
					{
						entity.Components.Add(myFractureComponentBase);
						myFractureComponentBase.Deserialize(obFractureComponent);
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine("Cannot add received fracture component: " + ex.Message);
					if (entity.Components.Has<MyFractureComponentBase>())
					{
						MyCubeBlock myCubeBlock = entity as MyCubeBlock;
						if (myCubeBlock != null && myCubeBlock.SlimBlock != null)
						{
							myCubeBlock.SlimBlock.RemoveFractureComponent();
						}
						else
						{
							entity.Components.Remove<MyFractureComponentBase>();
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					foreach (MyObjectBuilder_FractureComponentBase.FracturedShape shape in obFractureComponent.Shapes)
					{
						stringBuilder.Append(shape.Name).Append(" ");
					}
					MyLog.Default.WriteLine("Received fracture component not added, no shape found. Shapes: " + stringBuilder.ToString());
				}
			}
		}

		public static void RemoveShapeFromFractureComponent(long gridId, Vector3I position, ushort compoundBlockId, string shapeName)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRemoveShapeFromFractureComponentMessage, gridId, position, compoundBlockId, new string[1]
			{
				shapeName
			});
		}

		public static void RemoveShapesFromFractureComponent(long gridId, Vector3I position, ushort compoundBlockId, List<string> shapeNames)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRemoveShapeFromFractureComponentMessage, gridId, position, compoundBlockId, shapeNames.ToArray());
		}

		[Event(null, 236)]
		[Reliable]
		[Broadcast]
		private static void OnRemoveShapeFromFractureComponentMessage(long gridId, Vector3I position, ushort compoundBlockId, string[] shapeNames)
		{
			if (!MyEntities.TryGetEntityById(gridId, out MyEntity entity))
			{
				return;
			}
			MySlimBlock cubeBlock = (entity as MyCubeGrid).GetCubeBlock(position);
			if (cubeBlock == null || cubeBlock.FatBlock == null)
			{
				return;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
			if (myCompoundCubeBlock != null)
			{
				MySlimBlock block = myCompoundCubeBlock.GetBlock(compoundBlockId);
				if (block != null)
				{
					RemoveFractureComponentChildShapes(block, shapeNames);
				}
			}
			else
			{
				RemoveFractureComponentChildShapes(cubeBlock, shapeNames);
			}
		}

		private static void RemoveFractureComponentChildShapes(MySlimBlock block, string[] shapeNames)
		{
			MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
			if (fractureComponent != null)
			{
				fractureComponent.RemoveChildShapes(shapeNames);
			}
			else
			{
				MyLog.Default.WriteLine("Cannot remove child shapes from fracture component, fracture component not found in block, BlockDefinition: " + block.BlockDefinition.Id.ToString() + ", Shapes: " + string.Join(", ", shapeNames));
			}
		}

		public static void RemoveFracturedPiecesRequest(ulong userId, Vector3D center, float radius)
		{
			if (Sync.IsServer)
			{
				MyFracturedPiecesManager.Static.RemoveFracturesInSphere(center, radius);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRemoveFracturedPiecesMessage, userId, center, radius);
			}
		}

		[Event(null, 296)]
		[Reliable]
		[Server]
		private static void OnRemoveFracturedPiecesMessage(ulong userId, Vector3D center, float radius)
		{
			if (MySession.Static.IsUserAdmin(userId))
			{
				MyFracturedPiecesManager.Static.RemoveFracturesInSphere(center, radius);
			}
		}
	}
}
