using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Networking;
using VRage.Groups;
using VRageMath;

namespace Sandbox.Game.Replication.History
{
	public static class MySnapshotCache
	{
		private struct MyItem
		{
			public MySnapshot Snapshot;

			public MySnapshotFlags SnapshotFlags;

			public bool Reset;
		}

		public static long DEBUG_ENTITY_ID;

		public static bool PROPAGATE_TO_CONNECTIONS = true;

		private static readonly Dictionary<MyEntity, MyItem> m_cache = new Dictionary<MyEntity, MyItem>();

		public static void Add(MyEntity entity, ref MySnapshot snapshot, MySnapshotFlags snapshotFlags, bool reset)
		{
			m_cache[entity] = new MyItem
			{
				Snapshot = snapshot,
				SnapshotFlags = snapshotFlags,
				Reset = reset
			};
		}

		public static void Apply()
		{
			foreach (KeyValuePair<MyEntity, MyItem> item in m_cache)
			{
				MyEntity key = item.Key;
				if (!key.Closed && !key.MarkedForClose)
				{
					if (MyFakes.SNAPSHOTCACHE_HIERARCHY)
					{
						MyEntity myEntity = key;
						do
						{
							myEntity = MySnapshot.GetParent(myEntity, out bool _);
						}
						while (myEntity != null && !m_cache.ContainsKey(myEntity));
						if (myEntity != null)
						{
							continue;
						}
					}
					MyItem value = item.Value;
					if (value.SnapshotFlags.ApplyPhysicsLinear || value.SnapshotFlags.ApplyPhysicsAngular)
					{
						ApplyPhysics(key, value);
					}
					bool applyPosition = value.SnapshotFlags.ApplyPosition;
					bool applyRotation = value.SnapshotFlags.ApplyRotation;
					if (applyPosition || applyRotation)
					{
						value.Snapshot.GetMatrix(key, out MatrixD mat, applyPosition, applyRotation);
						bool reset = MySnapshot.ApplyReset && value.Reset;
						MyCubeGrid myCubeGrid = key as MyCubeGrid;
						if (MyFakes.SNAPSHOTCACHE_HIERARCHY && myCubeGrid != null)
						{
							CalculateDiffs(key, ref mat, out MatrixD diffMat, out Vector3 diffPos);
							ApplyChildMatrix(myCubeGrid, ref mat, ref diffMat, ref diffPos, reset);
						}
						else
						{
							ApplyChildMatrixLite(key, ref mat, reset);
						}
					}
				}
			}
			m_cache.Clear();
		}

		private static void CalculateDiffs(MyEntity entity, ref MatrixD mat, out MatrixD diffMat, out Vector3 diffPos)
		{
			MatrixD worldMatrixInvScaled = entity.PositionComp.WorldMatrixInvScaled;
			diffMat = worldMatrixInvScaled * mat;
			Vector3 position = (entity.Physics != null && entity.Physics.RigidBody != null) ? entity.Physics.CenterOfMassLocal : entity.PositionComp.LocalAABB.Center;
			Vector3D.Transform(ref position, ref mat, out Vector3D result);
			MatrixD matrix = entity.PositionComp.WorldMatrix;
			Vector3D.Transform(ref position, ref matrix, out Vector3D result2);
			diffPos = result - result2;
		}

		private static void ApplyPhysics(MyEntity entity, MyItem value)
		{
			bool applyPhysicsLinear = value.SnapshotFlags.ApplyPhysicsLinear;
			bool applyPhysicsAngular = value.SnapshotFlags.ApplyPhysicsAngular;
			value.Snapshot.ApplyPhysics(entity, applyPhysicsAngular, applyPhysicsLinear, value.SnapshotFlags.ApplyPhysicsLocal);
		}

		private static void PropagateToConnections(MyEntity grid, ref MatrixD diffMat, ref Vector3 diffPos, bool reset)
		{
			MatrixD localDiffMat = diffMat;
			Vector3 localDiffPos = diffPos;
			MyGridPhysicalHierarchy.Static.ApplyOnAllChildren(grid, delegate(MyEntity child)
			{
				PropagateToChild(child, localDiffMat, localDiffPos, reset);
			});
		}

		private static void PropagateToChild(MyEntity child, MatrixD diffMat, Vector3 diffPos, bool reset)
		{
			MatrixD newChildMatrix = default(MatrixD);
			bool flag = false;
			bool inheritRotation;
			if (m_cache.TryGetValue(child, out MyItem value))
			{
				bool applyPosition = value.SnapshotFlags.ApplyPosition;
				bool applyRotation = value.SnapshotFlags.ApplyRotation;
				inheritRotation = value.SnapshotFlags.InheritRotation;
				if (applyPosition || applyRotation)
				{
					MatrixD newChildMatrix2;
					if (applyPosition != applyRotation)
					{
						CalculateMatrix(child, ref diffMat, ref diffPos, inheritRotation, out newChildMatrix2);
					}
					else
					{
						newChildMatrix2 = child.WorldMatrix;
					}
					value.Snapshot.GetMatrix(out newChildMatrix, ref newChildMatrix2, applyPosition, applyRotation);
					CalculateDiffs(child, ref newChildMatrix, out diffMat, out diffPos);
					flag = true;
				}
				if (value.SnapshotFlags.ApplyPhysicsLinear || value.SnapshotFlags.ApplyPhysicsAngular)
				{
					ApplyPhysics(child, value);
				}
				reset |= value.Reset;
			}
			else
			{
				inheritRotation = (child.LastSnapshotFlags == null || child.LastSnapshotFlags.InheritRotation);
			}
			if (!flag)
			{
				CalculateMatrix(child, ref diffMat, ref diffPos, inheritRotation, out newChildMatrix);
			}
			ApplyChildMatrix(child, ref newChildMatrix, ref diffMat, ref diffPos, reset);
		}

		private static void CalculateMatrix(MyEntity child, ref MatrixD diffMat, ref Vector3 diffPos, bool inheritRotation, out MatrixD newChildMatrix)
		{
			if (inheritRotation)
			{
				MyCharacter myCharacter = child as MyCharacter;
				if (myCharacter != null && myCharacter.Gravity.LengthSquared() > 0.1f)
				{
					if (myCharacter.Physics != null && myCharacter.Physics.CharacterProxy != null)
					{
						if (myCharacter.Physics.CharacterProxy.Supported)
						{
							Vector3D vector = child.WorldMatrix.Up;
							newChildMatrix = child.WorldMatrix * diffMat;
							Vector3D v = newChildMatrix.Up;
							double num = vector.Dot(ref v);
							if (Math.Abs(Math.Abs(num) - 1.0) > 9.9999997473787516E-05)
							{
								double angle = 0.0 - Math.Acos(num);
								Vector3D.Cross(ref vector, ref v, out Vector3D result);
								result.Normalize();
								MatrixD.CreateFromAxisAngle(ref result, angle, out MatrixD result2);
								Vector3D translation = newChildMatrix.Translation;
								newChildMatrix.Translation = Vector3D.Zero;
								MatrixD.Multiply(ref newChildMatrix, ref result2, out newChildMatrix);
								newChildMatrix.Translation = translation;
							}
						}
						else
						{
							newChildMatrix = child.WorldMatrix;
							newChildMatrix.Translation += diffPos;
						}
					}
					else
					{
						newChildMatrix = child.WorldMatrix * diffMat;
					}
				}
				else
				{
					newChildMatrix = child.WorldMatrix * diffMat;
				}
				newChildMatrix.Orthogonalize();
			}
			else
			{
				newChildMatrix = child.WorldMatrix;
				newChildMatrix.Translation += diffPos;
			}
		}

		private static void ApplyChildMatrix(MyEntity child, ref MatrixD mat, ref MatrixD diffMat, ref Vector3 diffPos, bool reset)
		{
			ApplyChildMatrixLite(child, ref mat, reset);
			if (PROPAGATE_TO_CONNECTIONS)
			{
				PropagateToConnections(child, ref diffMat, ref diffPos, reset);
			}
		}

		private static void ApplyChildMatrixLite(MyEntity child, ref MatrixD mat, bool reset)
		{
			MyCubeGrid myCubeGrid = child as MyCubeGrid;
			if (myCubeGrid == null || !myCubeGrid.IsStatic)
			{
				MyEntity myEntity = MySession.Static.CameraController as MyEntity;
				if (myEntity != null && (child == myEntity || child == myEntity.GetTopMostParent()))
				{
					MatrixD transformDelta = child.PositionComp.WorldMatrixInvScaled * mat;
					MyThirdPersonSpectator.Static.CompensateQuickTransformChange(ref transformDelta);
				}
				child.m_positionResetFromServer = reset;
				child.PositionComp.SetWorldMatrix(mat, MyGridPhysicalHierarchy.Static, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: false, reset);
				if (myCubeGrid != null && myCubeGrid.InScene)
				{
					MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node = MyCubeGridGroups.Static.Mechanical.GetNode(myCubeGrid);
					foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node> parentLink in node.ParentLinks)
					{
						(MyEntities.GetEntityById(parentLink.Key) as MyPistonBase)?.SetCurrentPosByTopGridMatrix();
					}
					foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node> childLink in node.ChildLinks)
					{
						(MyEntities.GetEntityById(childLink.Key) as MyPistonBase)?.SetCurrentPosByTopGridMatrix();
					}
				}
			}
		}
	}
}
