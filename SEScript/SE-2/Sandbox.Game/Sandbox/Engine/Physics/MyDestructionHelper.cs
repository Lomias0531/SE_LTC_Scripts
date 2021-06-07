using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Replication;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Physics
{
	public static class MyDestructionHelper
	{
		public static readonly float MASS_REDUCTION_COEF = 0.04f;

		private static List<HkdShapeInstanceInfo> m_tmpInfos = new List<HkdShapeInstanceInfo>();

		private static List<HkdShapeInstanceInfo> m_tmpInfos2 = new List<HkdShapeInstanceInfo>();

		private static bool DontCreateFracture(HkdBreakableShape breakableShape)
		{
			if (!breakableShape.IsValid())
			{
				return false;
			}
			return (breakableShape.UserObject & 2) != 0;
		}

		public static bool IsFixed(HkdBreakableBodyInfo breakableBodyInfo)
		{
			new HkdBreakableBodyHelper(breakableBodyInfo).GetChildren(m_tmpInfos2);
			foreach (HkdShapeInstanceInfo item in m_tmpInfos2)
			{
				if (IsFixed(item.Shape))
				{
					m_tmpInfos2.Clear();
					return true;
				}
			}
			m_tmpInfos2.Clear();
			return false;
		}

		public static bool IsFixed(HkdBreakableShape breakableShape)
		{
			if (!breakableShape.IsValid())
			{
				return false;
			}
			if ((breakableShape.UserObject & 4) != 0)
			{
				return true;
			}
			breakableShape.GetChildren(m_tmpInfos);
			foreach (HkdShapeInstanceInfo tmpInfo in m_tmpInfos)
			{
				if ((tmpInfo.Shape.UserObject & 4) != 0)
				{
					m_tmpInfos.Clear();
					return true;
				}
			}
			m_tmpInfos.Clear();
			return false;
		}

		private static bool IsBodyWithoutGeneratedFracturedPieces(HkdBreakableBody b, MyCubeBlock block)
		{
			if (MyFakes.REMOVE_GENERATED_BLOCK_FRACTURES && (block == null || ContainsBlockWithoutGeneratedFracturedPieces(block)))
			{
				if (b.BreakableShape.IsCompound())
				{
					b.BreakableShape.GetChildren(m_tmpInfos);
					int num = m_tmpInfos.Count - 1;
					while (num >= 0 && DontCreateFracture(m_tmpInfos[num].Shape))
					{
						m_tmpInfos.RemoveAt(num);
						num--;
					}
					if (m_tmpInfos.Count == 0)
					{
						return true;
					}
					m_tmpInfos.Clear();
				}
				else if (DontCreateFracture(b.BreakableShape))
				{
					return true;
				}
			}
			return false;
		}

		public static MyFracturedPiece CreateFracturePiece(HkdBreakableBody b, ref MatrixD worldMatrix, List<MyDefinitionId> originalBlocks, MyCubeBlock block = null, bool sync = true)
		{
			if (IsBodyWithoutGeneratedFracturedPieces(b, block))
			{
				return null;
			}
			MyFracturedPiece pieceFromPool = MyFracturedPiecesManager.Static.GetPieceFromPool(0L);
			pieceFromPool.InitFromBreakableBody(b, worldMatrix, block);
			pieceFromPool.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			if (originalBlocks != null && originalBlocks.Count != 0)
			{
				pieceFromPool.OriginalBlocks.Clear();
				pieceFromPool.OriginalBlocks.AddRange(originalBlocks);
				if (MyDefinitionManager.Static.TryGetDefinition(originalBlocks[0], out MyPhysicalModelDefinition definition))
				{
					pieceFromPool.Physics.MaterialType = definition.PhysicalMaterial.Id.SubtypeId;
				}
			}
			if (MyFakes.ENABLE_FRACTURE_PIECE_SHAPE_CHECK)
			{
				pieceFromPool.DebugCheckValidShapes();
			}
			if (MyExternalReplicable.FindByObject(pieceFromPool) == null)
			{
				MyEntities.RaiseEntityCreated(pieceFromPool);
			}
			MyEntities.Add(pieceFromPool);
			return pieceFromPool;
		}

		public static void FixPosition(MyFracturedPiece fp)
		{
			HkdBreakableShape breakableShape = fp.Physics.BreakableBody.BreakableShape;
			if (breakableShape.GetChildrenCount() != 0)
			{
				breakableShape.GetChildren(m_tmpInfos);
				Vector3 translation = m_tmpInfos[0].GetTransform().Translation;
				if (translation.LengthSquared() < 1f)
				{
					m_tmpInfos.Clear();
					return;
				}
				List<HkdConnection> list = new List<HkdConnection>();
				HashSet<HkdBreakableShape> hashSet = new HashSet<HkdBreakableShape>();
				HashSet<HkdBreakableShape> hashSet2 = new HashSet<HkdBreakableShape>();
				hashSet.Add(breakableShape);
				breakableShape.GetConnectionList(list);
				fp.PositionComp.SetPosition(Vector3D.Transform(translation, fp.PositionComp.WorldMatrix));
				foreach (HkdShapeInstanceInfo tmpInfo in m_tmpInfos)
				{
					Matrix m = tmpInfo.GetTransform();
					m.Translation -= translation;
					tmpInfo.SetTransform(ref m);
					m_tmpInfos2.Add(tmpInfo);
					HkdBreakableShape hkdBreakableShape = tmpInfo.Shape;
					hkdBreakableShape.GetConnectionList(list);
					while (hkdBreakableShape.HasParent)
					{
						hkdBreakableShape = hkdBreakableShape.GetParent();
						if (hashSet.Add(hkdBreakableShape))
						{
							hkdBreakableShape.GetConnectionList(list);
						}
					}
					hashSet2.Add(tmpInfo.Shape);
				}
				m_tmpInfos.Clear();
				HkdBreakableShape hkdBreakableShape2 = new HkdCompoundBreakableShape(breakableShape, m_tmpInfos2);
				((HkdCompoundBreakableShape)hkdBreakableShape2).RecalcMassPropsFromChildren();
				hkdBreakableShape2.SetChildrenParent(hkdBreakableShape2);
				foreach (HkdConnection item in list)
				{
					HkBaseSystem.EnableAssert(390435339, enable: true);
					if (hashSet2.Contains(item.ShapeA) && hashSet2.Contains(item.ShapeB))
					{
						HkdConnection connection = item;
						hkdBreakableShape2.AddConnection(ref connection);
					}
				}
				fp.Physics.BreakableBody.BreakableShape = hkdBreakableShape2;
				m_tmpInfos2.Clear();
				((HkdCompoundBreakableShape)hkdBreakableShape2).RecalcMassPropsFromChildren();
			}
		}

		private static bool ContainsBlockWithoutGeneratedFracturedPieces(MyCubeBlock block)
		{
			if (!block.BlockDefinition.CreateFracturedPieces)
			{
				return true;
			}
			if (block is MyCompoundCubeBlock)
			{
				foreach (MySlimBlock block2 in (block as MyCompoundCubeBlock).GetBlocks())
				{
					if (!block2.BlockDefinition.CreateFracturedPieces)
					{
						return true;
					}
				}
			}
			if (block is MyFracturedBlock)
			{
				foreach (MyDefinitionId originalBlock in (block as MyFracturedBlock).OriginalBlocks)
				{
					if (!MyDefinitionManager.Static.GetCubeBlockDefinition(originalBlock).CreateFracturedPieces)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static MyFracturedPiece CreateFracturePiece(HkdBreakableShape shape, ref MatrixD worldMatrix, bool isStatic, MyDefinitionId? definition, bool sync)
		{
			MyFracturedPiece myFracturedPiece = CreateFracturePiece(ref shape, ref worldMatrix, isStatic);
			if (definition.HasValue)
			{
				myFracturedPiece.OriginalBlocks.Clear();
				myFracturedPiece.OriginalBlocks.Add(definition.Value);
				if (MyDefinitionManager.Static.TryGetDefinition(definition.Value, out MyPhysicalModelDefinition definition2))
				{
					myFracturedPiece.Physics.MaterialType = definition2.PhysicalMaterial.Id.SubtypeId;
				}
			}
			else
			{
				myFracturedPiece.Save = false;
			}
			if (myFracturedPiece.Save && MyFakes.ENABLE_FRACTURE_PIECE_SHAPE_CHECK)
			{
				myFracturedPiece.DebugCheckValidShapes();
			}
			if (MyExternalReplicable.FindByObject(myFracturedPiece) == null)
			{
				MyEntities.RaiseEntityCreated(myFracturedPiece);
			}
			MyEntities.Add(myFracturedPiece);
			return myFracturedPiece;
		}

		public static MyFracturedPiece CreateFracturePiece(MyFracturedBlock fracturedBlock, bool sync)
		{
			MatrixD worldMatrix = fracturedBlock.CubeGrid.PositionComp.WorldMatrix;
			worldMatrix.Translation = fracturedBlock.CubeGrid.GridIntegerToWorld(fracturedBlock.Position);
			MyFracturedPiece myFracturedPiece = CreateFracturePiece(ref fracturedBlock.Shape, ref worldMatrix, isStatic: false);
			myFracturedPiece.OriginalBlocks = fracturedBlock.OriginalBlocks;
			if (MyDefinitionManager.Static.TryGetDefinition(myFracturedPiece.OriginalBlocks[0], out MyPhysicalModelDefinition definition))
			{
				myFracturedPiece.Physics.MaterialType = definition.PhysicalMaterial.Id.SubtypeId;
			}
			if (MyFakes.ENABLE_FRACTURE_PIECE_SHAPE_CHECK)
			{
				myFracturedPiece.DebugCheckValidShapes();
			}
			if (MyExternalReplicable.FindByObject(myFracturedPiece) == null)
			{
				MyEntities.RaiseEntityCreated(myFracturedPiece);
			}
			MyEntities.Add(myFracturedPiece);
			return myFracturedPiece;
		}

		public static MyFracturedPiece CreateFracturePiece(MyFractureComponentCubeBlock fractureBlockComponent, bool sync)
		{
			if (!fractureBlockComponent.Block.BlockDefinition.CreateFracturedPieces)
			{
				return null;
			}
			if (!fractureBlockComponent.Shape.IsValid())
			{
				MyLog.Default.WriteLine("Invalid shape in fracture component, Id: " + fractureBlockComponent.Block.BlockDefinition.Id.ToString() + ", closed: " + fractureBlockComponent.Block.FatBlock.Closed);
				return null;
			}
			MatrixD worldMatrix = fractureBlockComponent.Block.FatBlock.WorldMatrix;
			MyFracturedPiece myFracturedPiece = CreateFracturePiece(ref fractureBlockComponent.Shape, ref worldMatrix, isStatic: false);
			myFracturedPiece.OriginalBlocks.Add(fractureBlockComponent.Block.BlockDefinition.Id);
			if (MyFakes.ENABLE_FRACTURE_PIECE_SHAPE_CHECK)
			{
				myFracturedPiece.DebugCheckValidShapes();
			}
			if (MyDefinitionManager.Static.TryGetDefinition(myFracturedPiece.OriginalBlocks[0], out MyPhysicalModelDefinition definition))
			{
				myFracturedPiece.Physics.MaterialType = definition.PhysicalMaterial.Id.SubtypeId;
			}
			if (MyExternalReplicable.FindByObject(myFracturedPiece) == null)
			{
				MyEntities.RaiseEntityCreated(myFracturedPiece);
			}
			MyEntities.Add(myFracturedPiece);
			return myFracturedPiece;
		}

		private static MyFracturedPiece CreateFracturePiece(ref HkdBreakableShape shape, ref MatrixD worldMatrix, bool isStatic)
		{
			MyFracturedPiece pieceFromPool = MyFracturedPiecesManager.Static.GetPieceFromPool(0L);
			pieceFromPool.PositionComp.WorldMatrix = worldMatrix;
			pieceFromPool.Physics.Flags = (isStatic ? RigidBodyFlag.RBF_STATIC : RigidBodyFlag.RBF_DEBRIS);
			MyPhysicsBody physics = pieceFromPool.Physics;
			HkMassProperties massProperties = default(HkMassProperties);
			shape.BuildMassProperties(ref massProperties);
			physics.InitialSolverDeactivation = HkSolverDeactivation.High;
			physics.CreateFromCollisionObject(shape.GetShape(), Vector3.Zero, worldMatrix, massProperties);
			physics.LinearDamping = MyPerGameSettings.DefaultLinearDamping;
			physics.AngularDamping = MyPerGameSettings.DefaultAngularDamping;
			physics.BreakableBody = new HkdBreakableBody(shape, physics.RigidBody, null, worldMatrix);
			physics.BreakableBody.AfterReplaceBody += physics.FracturedBody_AfterReplaceBody;
			if (pieceFromPool.SyncFlag)
			{
				pieceFromPool.CreateSync();
			}
			pieceFromPool.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			pieceFromPool.SetDataFromHavok(shape);
			pieceFromPool.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			shape.RemoveReference();
			return pieceFromPool;
		}

		public static void TriggerDestruction(HkWorld world, HkRigidBody body, Vector3 havokPosition, float radius = 0.0005f)
		{
			HkdFractureImpactDetails hkdFractureImpactDetails = HkdFractureImpactDetails.Create();
			hkdFractureImpactDetails.SetBreakingBody(body);
			hkdFractureImpactDetails.SetContactPoint(havokPosition);
			hkdFractureImpactDetails.SetDestructionRadius(radius);
			hkdFractureImpactDetails.SetBreakingImpulse(MyDestructionConstants.STRENGTH * 10f);
			hkdFractureImpactDetails.Flag |= HkdFractureImpactDetails.Flags.FLAG_DONT_RECURSE;
			MyPhysics.FractureImpactDetails details = default(MyPhysics.FractureImpactDetails);
			details.Details = hkdFractureImpactDetails;
			details.World = world;
			MyPhysics.EnqueueDestruction(details);
		}

		public static void TriggerDestruction(float destructionImpact, MyPhysicsBody body, Vector3D position, Vector3 normal, float maxDestructionRadius)
		{
			if (body.BreakableBody != null)
			{
				_ = body.Mass;
				float destructionRadius = Math.Min(destructionImpact / 8000f, maxDestructionRadius);
				float breakingImpulse = MyDestructionConstants.STRENGTH + destructionImpact / 10000f;
				float particleExpandVelocity = Math.Min(destructionImpact / 10000f, 3f);
				HkdFractureImpactDetails hkdFractureImpactDetails = HkdFractureImpactDetails.Create();
				hkdFractureImpactDetails.SetBreakingBody(body.RigidBody);
				hkdFractureImpactDetails.SetContactPoint(body.WorldToCluster(position));
				hkdFractureImpactDetails.SetDestructionRadius(destructionRadius);
				hkdFractureImpactDetails.SetBreakingImpulse(breakingImpulse);
				hkdFractureImpactDetails.SetParticleExpandVelocity(particleExpandVelocity);
				hkdFractureImpactDetails.SetParticlePosition(body.WorldToCluster(position - normal * 0.25f));
				hkdFractureImpactDetails.SetParticleMass(1E+07f);
				hkdFractureImpactDetails.ZeroCollidingParticleVelocity();
				hkdFractureImpactDetails.Flag = (hkdFractureImpactDetails.Flag | HkdFractureImpactDetails.Flags.FLAG_DONT_RECURSE | HkdFractureImpactDetails.Flags.FLAG_TRIGGERED_DESTRUCTION);
				MyPhysics.FractureImpactDetails details = default(MyPhysics.FractureImpactDetails);
				details.Details = hkdFractureImpactDetails;
				details.World = body.HavokWorld;
				details.ContactInWorld = position;
				details.Entity = (MyEntity)body.Entity;
				MyPhysics.EnqueueDestruction(details);
			}
		}

		public static float MassToHavok(float m)
		{
			if (MyPerGameSettings.Destruction)
			{
				return m * MASS_REDUCTION_COEF;
			}
			return m;
		}

		public static float MassFromHavok(float m)
		{
			if (MyPerGameSettings.Destruction)
			{
				return m / MASS_REDUCTION_COEF;
			}
			return m;
		}
	}
}
