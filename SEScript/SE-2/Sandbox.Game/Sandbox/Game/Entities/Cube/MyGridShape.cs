using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Models;
using VRage.Groups;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	public class MyGridShape : IDisposable
	{
		public const int MAX_SHAPE_COUNT = 64879;

		private MyVoxelSegmentation m_segmenter;

		private MyCubeBlockCollector m_blockCollector = new MyCubeBlockCollector();

		private HkMassProperties m_massProperties;

		private HkMassProperties m_originalMassProperties;

		private bool m_originalMassPropertiesSet;

		private HashSet<MySlimBlock> m_tmpRemovedBlocks = new HashSet<MySlimBlock>();

		private HashSet<Vector3I> m_tmpRemovedCubes = new HashSet<Vector3I>();

		private HashSet<Vector3I> m_tmpAdditionalCubes = new HashSet<Vector3I>();

		private MyCubeGrid m_grid;

		private HkGridShape m_root;

		private static FastResourceLock m_shapeAccessLock = new FastResourceLock();

		private Dictionary<Vector3I, HkdShapeInstanceInfo> m_blocksShapes = new Dictionary<Vector3I, HkdShapeInstanceInfo>();

		private const int MassCellSize = 4;

		private MyGridMassComputer m_massElements;

		public static uint INVALID_COMPOUND_ID = uint.MaxValue;

		[ThreadStatic]
		private static List<Vector3S> m_removalMins;

		[ThreadStatic]
		private static List<Vector3S> m_removalMaxes;

		[ThreadStatic]
		private static List<bool> m_removalResults;

		private HashSet<Vector3I> m_updateConnections = new HashSet<Vector3I>();

		private List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();

		private List<MyVoxelBase> m_overlappingVoxels = new List<MyVoxelBase>();

		public HashSet<Vector3I> BlocksConnectedToWorld = new HashSet<Vector3I>();

		private List<HkdShapeInstanceInfo> m_shapeInfosList = new List<HkdShapeInstanceInfo>();

		private List<HkdShapeInstanceInfo> m_shapeInfosList2 = new List<HkdShapeInstanceInfo>();

		private List<HkdConnection> m_connectionsToAddCache = new List<HkdConnection>();

		private List<HkShape> m_khpShapeList = new List<HkShape>();

		private static List<HkdShapeInstanceInfo> m_tmpChildren = new List<HkdShapeInstanceInfo>();

		private HashSet<MySlimBlock> m_processedBlock = new HashSet<MySlimBlock>();

		private static List<HkdShapeInstanceInfo> m_shapeInfosList3 = new List<HkdShapeInstanceInfo>();

		private Dictionary<Vector3I, List<HkdConnection>> m_connections = new Dictionary<Vector3I, List<HkdConnection>>();

		private static object m_sharedParentLock = new object();

		private bool m_isSharedTensorDirty;

		public float BreakImpulse
		{
			get
			{
				if (m_grid == null || m_grid.Physics == null || m_grid.Physics.IsStatic)
				{
					return 1E+07f;
				}
				return Math.Max(m_grid.Physics.Mass * MyFakes.DEFORMATION_MINIMUM_VELOCITY, MyFakes.DEFORMATION_MIN_BREAK_IMPULSE);
			}
		}

		public HkdBreakableShape BreakableShape
		{
			get;
			set;
		}

		public HkMassProperties? MassProperties
		{
			get
			{
				if (!m_grid.IsStatic)
				{
					return m_massProperties;
				}
				return null;
			}
		}

		public HkMassProperties? BaseMassProperties
		{
			get
			{
				if (!m_grid.IsStatic && m_originalMassPropertiesSet)
				{
					return m_originalMassProperties;
				}
				return null;
			}
		}

		public int ShapeCount => m_root.ShapeCount;

		public static FastResourceLock NativeShapeLock => m_shapeAccessLock;

		public MyGridShape(MyCubeGrid grid)
		{
			m_grid = grid;
			if (!MyPerGameSettings.Destruction)
			{
				if (MyPerGameSettings.UseGridSegmenter)
				{
					m_segmenter = new MyVoxelSegmentation();
				}
				m_massElements = new MyGridMassComputer(4);
				try
				{
					m_blockCollector.Collect(grid, m_segmenter, MyVoxelSegmentationType.Simple, m_massElements);
					m_root = new HkGridShape(m_grid.GridSize, HkReferencePolicy.None);
					AddShapesFromCollector();
					if (!m_grid.IsStatic)
					{
						UpdateMassProperties();
					}
				}
				finally
				{
					m_blockCollector.Clear();
				}
			}
		}

		public void GetShapesInInterval(Vector3I min, Vector3I max, List<HkShape> shapeList)
		{
			m_root.GetShapesInInterval(min, max, shapeList);
		}

		public List<HkShape> GetShapesFromPosition(Vector3I pos)
		{
			List<HkShape> list = new List<HkShape>();
			m_root.GetShape(pos, list);
			return list;
		}

		private void AddShapesFromCollector()
		{
			int num = 0;
			for (int i = 0; i < m_blockCollector.ShapeInfos.Count; i++)
			{
				MyCubeBlockCollector.ShapeInfo shapeInfo = m_blockCollector.ShapeInfos[i];
				HkShape[] obj = null;
				Span<HkShape> shapes = default(Span<HkShape>);
				shapes = ((shapeInfo.Count >= 256) ? ((Span<HkShape>)(obj = new HkShape[shapeInfo.Count])) : stackalloc HkShape[shapeInfo.Count]);
				for (int j = 0; j < shapeInfo.Count; j++)
				{
					shapes[j] = m_blockCollector.Shapes[num + j];
				}
				num += shapeInfo.Count;
				if (m_root.ShapeCount + shapeInfo.Count > 64879)
				{
					MyHud.Notifications.Add(MyNotificationSingletons.GridReachedPhysicalLimit);
				}
				if (m_root.ShapeCount + shapeInfo.Count < 65536)
				{
					m_root.AddShapes(shapes, new Vector3S(shapeInfo.Min), new Vector3S(shapeInfo.Max));
				}
				GC.KeepAlive(obj);
			}
		}

		private void UpdateMassProperties()
		{
			m_massProperties = m_massElements.UpdateMass();
			if (!m_originalMassPropertiesSet)
			{
				m_originalMassProperties = m_massProperties;
				m_originalMassPropertiesSet = true;
			}
		}

		public void Dispose()
		{
			foreach (List<HkdConnection> value in m_connections.Values)
			{
				foreach (HkdConnection item in value)
				{
					item.RemoveReference();
				}
			}
			m_connections.Clear();
			if (BreakableShape?.IsValid() ?? false)
			{
				BreakableShape.RemoveReference();
				BreakableShape.ClearHandle();
			}
			foreach (HkdShapeInstanceInfo value2 in m_blocksShapes.Values)
			{
				if (!value2.IsReferenceValid())
				{
					MyLog.Default.WriteLine("Block shape was disposed already in MyGridShape.Dispose!");
				}
				if (value2.Shape.IsValid())
				{
					value2.Shape.RemoveReference();
				}
				value2.RemoveReference();
			}
			m_blocksShapes.Clear();
			if (!MyPerGameSettings.Destruction)
			{
				m_root.Base.RemoveReference();
			}
		}

		public void UnmarkBreakable(HkRigidBody rigidBody)
		{
			if (m_grid.GetPhysicsBody().HavokWorld != null && m_grid.BlocksDestructionEnabled)
			{
				UnmarkBreakable(m_grid.GetPhysicsBody().HavokWorld, rigidBody);
			}
		}

		public void MarkBreakable(HkRigidBody rigidBody)
		{
			if (m_grid.GetPhysicsBody().HavokWorld != null && m_grid.BlocksDestructionEnabled)
			{
				MarkBreakable(m_grid.GetPhysicsBody().HavokWorld, rigidBody);
			}
		}

		public void RefreshBlocks(HkRigidBody rigidBody, HashSet<Vector3I> dirtyBlocks)
		{
			m_originalMassPropertiesSet = false;
			UpdateDirtyBlocks(dirtyBlocks);
			if (rigidBody.GetMotionType() != HkMotionType.Keyframed && !MyPerGameSettings.Destruction)
			{
				UpdateMassProperties();
			}
		}

		[Conditional("DEBUG")]
		private void CheckShapePositions(List<MyCubeBlockCollector.ShapeInfo> infos)
		{
			foreach (MyCubeBlockCollector.ShapeInfo info in infos)
			{
				Vector3I vector3I = default(Vector3I);
				vector3I.X = info.Min.X;
				while (vector3I.X <= info.Max.X)
				{
					vector3I.Y = info.Min.Y;
					while (vector3I.Y <= info.Max.Y)
					{
						vector3I.Z = info.Min.Z;
						while (vector3I.Z <= info.Max.Z)
						{
							vector3I.Z++;
						}
						vector3I.Y++;
					}
					vector3I.X++;
				}
			}
		}

		private static void ExpandBlock(Vector3I cubePos, MyCubeGrid grid, HashSet<MySlimBlock> existingBlocks, HashSet<Vector3I> checkList, HashSet<Vector3I> expandResult)
		{
			MySlimBlock cubeBlock = grid.GetCubeBlock(cubePos);
			if (cubeBlock == null || !existingBlocks.Add(cubeBlock))
			{
				return;
			}
			Vector3I item = default(Vector3I);
			item.X = cubeBlock.Min.X;
			while (item.X <= cubeBlock.Max.X)
			{
				item.Y = cubeBlock.Min.Y;
				while (item.Y <= cubeBlock.Max.Y)
				{
					item.Z = cubeBlock.Min.Z;
					while (item.Z <= cubeBlock.Max.Z)
					{
						if (!checkList.Contains(item))
						{
							expandResult.Add(item);
						}
						item.Z++;
					}
					item.Y++;
				}
				item.X++;
			}
		}

		private static void ExpandBlock(Vector3I cubePos, MyCubeGrid grid, HashSet<MySlimBlock> existingBlocks, HashSet<Vector3I> expandResult)
		{
			MySlimBlock cubeBlock = grid.GetCubeBlock(cubePos);
			if (cubeBlock == null || !existingBlocks.Add(cubeBlock))
			{
				return;
			}
			Vector3I item = default(Vector3I);
			item.X = cubeBlock.Min.X;
			while (item.X <= cubeBlock.Max.X)
			{
				item.Y = cubeBlock.Min.Y;
				while (item.Y <= cubeBlock.Max.Y)
				{
					item.Z = cubeBlock.Min.Z;
					while (item.Z <= cubeBlock.Max.Z)
					{
						expandResult.Add(item);
						item.Z++;
					}
					item.Y++;
				}
				item.X++;
			}
		}

		internal void UpdateDirtyBlocks(HashSet<Vector3I> dirtyCubes, bool recreateShape = true)
		{
			if (dirtyCubes.Count <= 0)
			{
				return;
			}
			if (MyPerGameSettings.Destruction && BreakableShape.IsValid())
			{
				int num = 0;
				HashSet<MySlimBlock> hashSet = new HashSet<MySlimBlock>();
				foreach (Vector3I dirtyCube in dirtyCubes)
				{
					UpdateConnections(dirtyCube);
					BlocksConnectedToWorld.Remove(dirtyCube);
					if (m_blocksShapes.ContainsKey(dirtyCube))
					{
						HkdShapeInstanceInfo hkdShapeInstanceInfo = m_blocksShapes[dirtyCube];
						hkdShapeInstanceInfo.Shape.RemoveReference();
						hkdShapeInstanceInfo.RemoveReference();
						m_blocksShapes.Remove(dirtyCube);
					}
					MySlimBlock cubeBlock = m_grid.GetCubeBlock(dirtyCube);
					if (cubeBlock != null && !hashSet.Contains(cubeBlock))
					{
						if (cubeBlock.Position != dirtyCube && m_blocksShapes.ContainsKey(cubeBlock.Position))
						{
							HkdShapeInstanceInfo hkdShapeInstanceInfo2 = m_blocksShapes[cubeBlock.Position];
							hkdShapeInstanceInfo2.Shape.RemoveReference();
							hkdShapeInstanceInfo2.RemoveReference();
							m_blocksShapes.Remove(cubeBlock.Position);
						}
						hashSet.Add(cubeBlock);
						num++;
					}
				}
				foreach (MySlimBlock item in hashSet)
				{
					Matrix blockTransform;
					HkdBreakableShape hkdBreakableShape = CreateBlockShape(item, out blockTransform);
					if (hkdBreakableShape != null)
					{
						m_blocksShapes[item.Position] = new HkdShapeInstanceInfo(hkdBreakableShape, blockTransform);
					}
				}
				foreach (HkdShapeInstanceInfo value in m_blocksShapes.Values)
				{
					m_shapeInfosList.Add(value);
				}
				if (hashSet.Count > 0)
				{
					FindConnectionsToWorld(hashSet);
				}
				if (recreateShape)
				{
					BreakableShape.RemoveReference();
					BreakableShape = new HkdCompoundBreakableShape(null, m_shapeInfosList);
					BreakableShape.SetChildrenParent(BreakableShape);
					BreakableShape.BuildMassProperties(ref m_massProperties);
					BreakableShape.SetStrenghtRecursively(MyDestructionConstants.STRENGTH, 0.7f);
				}
				UpdateConnectionsManually(BreakableShape, m_updateConnections);
				m_updateConnections.Clear();
				AddConnections();
				m_shapeInfosList.Clear();
			}
			else
			{
				try
				{
					if (m_removalMins == null)
					{
						m_removalMins = new List<Vector3S>();
					}
					if (m_removalMaxes == null)
					{
						m_removalMaxes = new List<Vector3S>();
					}
					if (m_removalResults == null)
					{
						m_removalResults = new List<bool>();
					}
					foreach (Vector3I dirtyCube2 in dirtyCubes)
					{
						if (m_tmpRemovedCubes.Add(dirtyCube2))
						{
							ExpandBlock(dirtyCube2, m_grid, m_tmpRemovedBlocks, m_tmpRemovedCubes);
						}
					}
					m_removalMins.Clear();
					m_removalMaxes.Clear();
					m_removalResults.Clear();
					using (NativeShapeLock.AcquireExclusiveUsing())
					{
						m_root.RemoveShapes(m_tmpRemovedCubes, m_removalMins, m_removalMaxes, m_removalResults);
						Vector3I vector3I = default(Vector3I);
						for (int i = 0; i < m_removalMins.Count; i++)
						{
							if (m_removalResults[i])
							{
								vector3I.X = m_removalMins[i].X;
								while (vector3I.X <= m_removalMaxes[i].X)
								{
									vector3I.Y = m_removalMins[i].Y;
									while (vector3I.Y <= m_removalMaxes[i].Y)
									{
										vector3I.Z = m_removalMins[i].Z;
										while (vector3I.Z <= m_removalMaxes[i].Z)
										{
											if (m_tmpRemovedCubes.Add(vector3I))
											{
												ExpandBlock(vector3I, m_grid, m_tmpRemovedBlocks, m_tmpRemovedCubes, m_tmpAdditionalCubes);
											}
											vector3I.Z++;
										}
										vector3I.Y++;
									}
									vector3I.X++;
								}
							}
						}
						while (m_tmpAdditionalCubes.Count > 0)
						{
							m_removalMins.Clear();
							m_removalMaxes.Clear();
							m_removalResults.Clear();
							m_root.RemoveShapes(m_tmpAdditionalCubes, m_removalMins, m_removalMaxes, m_removalResults);
							m_tmpAdditionalCubes.Clear();
							for (int j = 0; j < m_removalMins.Count; j++)
							{
								if (m_removalResults[j])
								{
									vector3I.X = m_removalMins[j].X;
									while (vector3I.X <= m_removalMaxes[j].X)
									{
										vector3I.Y = m_removalMins[j].Y;
										while (vector3I.Y <= m_removalMaxes[j].Y)
										{
											vector3I.Z = m_removalMins[j].Z;
											while (vector3I.Z <= m_removalMaxes[j].Z)
											{
												if (m_tmpRemovedCubes.Add(vector3I))
												{
													ExpandBlock(vector3I, m_grid, m_tmpRemovedBlocks, m_tmpRemovedCubes, m_tmpAdditionalCubes);
												}
												vector3I.Z++;
											}
											vector3I.Y++;
										}
										vector3I.X++;
									}
								}
							}
						}
						m_blockCollector.CollectArea(m_grid, m_tmpRemovedCubes, m_segmenter, MyVoxelSegmentationType.Simple, m_massElements);
						AddShapesFromCollector();
					}
				}
				finally
				{
					m_removalMins.Clear();
					m_removalMaxes.Clear();
					m_removalResults.Clear();
					m_blockCollector.Clear();
					m_tmpRemovedBlocks.Clear();
					m_tmpRemovedCubes.Clear();
					m_tmpAdditionalCubes.Clear();
				}
			}
		}

		private void UpdateConnections(Vector3I dirty)
		{
			foreach (Vector3I item in new List<Vector3I>(7)
			{
				dirty,
				dirty + Vector3I.Up,
				dirty + Vector3I.Down,
				dirty + Vector3I.Left,
				dirty + Vector3I.Right,
				dirty + Vector3I.Forward,
				dirty + Vector3I.Backward
			})
			{
				if (m_connections.ContainsKey(item))
				{
					foreach (HkdConnection item2 in m_connections[item])
					{
						item2.RemoveReference();
					}
					m_connections[item].Clear();
				}
				MySlimBlock cubeBlock = m_grid.GetCubeBlock(item);
				if (cubeBlock != null)
				{
					if (m_connections.ContainsKey(cubeBlock.Position))
					{
						foreach (HkdConnection item3 in m_connections[cubeBlock.Position])
						{
							item3.RemoveReference();
						}
						m_connections[cubeBlock.Position].Clear();
					}
					m_updateConnections.Add(cubeBlock.Position);
				}
				m_updateConnections.Add(item);
			}
		}

		public void UpdateShape(HkRigidBody rigidBody, HkRigidBody rigidBody2, HkdBreakableBody destructionBody)
		{
			if (destructionBody != null)
			{
				destructionBody.BreakableShape = BreakableShape;
				CreateConnectionToWorld(destructionBody, m_grid.Physics.HavokWorld);
				return;
			}
			rigidBody.SetShape(m_root);
			if (rigidBody2 != null)
			{
				rigidBody2.SetShape(m_root);
			}
		}

		private void FindConnectionsToWorld(HashSet<MySlimBlock> blocks)
		{
			if (m_grid.IsStatic && (m_grid.Physics == null || !(m_grid.Physics.LinearVelocity.LengthSquared() > 0f)))
			{
				int num = 0;
				Quaternion identity = Quaternion.Identity;
				MatrixD aabbWorldTransform = m_grid.WorldMatrix;
				BoundingBoxD box = m_grid.PositionComp.WorldAABB;
				MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, m_overlappingVoxels);
				foreach (MySlimBlock block in blocks)
				{
					BoundingBox geometryLocalBox = block.FatBlock.GetGeometryLocalBox();
					Vector3 halfExtents = geometryLocalBox.Size / 2f;
					block.ComputeScaledCenter(out Vector3D scaledCenter);
					scaledCenter += geometryLocalBox.Center;
					scaledCenter = Vector3D.Transform(scaledCenter, aabbWorldTransform);
					block.Orientation.GetMatrix(out Matrix result);
					identity = Quaternion.CreateFromRotationMatrix(result * aabbWorldTransform.GetOrientation());
					MyPhysics.GetPenetrationsBox(ref halfExtents, ref scaledCenter, ref identity, m_penetrations, 14);
					num++;
					bool flag = false;
					foreach (HkBodyCollision penetration in m_penetrations)
					{
						IMyEntity collisionEntity = penetration.GetCollisionEntity();
						if (collisionEntity != null && collisionEntity is MyVoxelBase)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						BoundingBoxD aabb = (BoundingBoxD)geometryLocalBox + (Vector3D)block.Position;
						foreach (MyVoxelBase overlappingVoxel in m_overlappingVoxels)
						{
							if (overlappingVoxel.IsAnyAabbCornerInside(ref aabbWorldTransform, aabb))
							{
								flag = true;
								break;
							}
						}
					}
					m_penetrations.Clear();
					if (flag && !BlocksConnectedToWorld.Contains(block.Position))
					{
						m_blocksShapes[block.Position].GetChildren(m_shapeInfosList2);
						for (int i = 0; i < m_shapeInfosList2.Count; i++)
						{
							HkdShapeInstanceInfo hkdShapeInstanceInfo = m_shapeInfosList2[i];
							if (hkdShapeInstanceInfo.Shape.GetChildrenCount() > 0)
							{
								hkdShapeInstanceInfo.Shape.GetChildren(m_shapeInfosList2);
							}
							else
							{
								hkdShapeInstanceInfo.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
							}
						}
						m_shapeInfosList2.Clear();
						BlocksConnectedToWorld.Add(block.Position);
					}
				}
				m_overlappingVoxels.Clear();
			}
		}

		public void FindConnectionsToWorld()
		{
			FindConnectionsToWorld(m_grid.GetBlocks());
		}

		public void RecalculateConnectionsToWorld(HashSet<MySlimBlock> blocks)
		{
			BlocksConnectedToWorld.Clear();
			FindConnectionsToWorld(blocks);
		}

		public void CreateConnectionToWorld(HkdBreakableBody destructionBody, HkWorld havokWorld)
		{
			if (BlocksConnectedToWorld.Count != 0)
			{
				HkdFixedConnectivity hkdFixedConnectivity = HkdFixedConnectivity.Create();
				foreach (Vector3I item in BlocksConnectedToWorld)
				{
					HkdFixedConnectivity.Connection c = new HkdFixedConnectivity.Connection(Vector3.Zero, Vector3.Up, 1f, m_blocksShapes[item].Shape, havokWorld.GetFixedBody(), 0);
					hkdFixedConnectivity.AddConnection(ref c);
					c.RemoveReference();
				}
				destructionBody.SetFixedConnectivity(hkdFixedConnectivity);
				hkdFixedConnectivity.RemoveReference();
			}
		}

		public HkdBreakableShape CreateBreakableShape()
		{
			m_blocksShapes.Clear();
			foreach (MySlimBlock block in m_grid.GetBlocks())
			{
				Matrix blockTransform;
				HkdBreakableShape hkdBreakableShape = CreateBlockShape(block, out blockTransform);
				if (hkdBreakableShape != null)
				{
					HkdShapeInstanceInfo hkdShapeInstanceInfo = new HkdShapeInstanceInfo(hkdBreakableShape, blockTransform);
					m_shapeInfosList.Add(hkdShapeInstanceInfo);
					m_blocksShapes[block.Position] = hkdShapeInstanceInfo;
				}
			}
			if (m_blocksShapes.Count == 0)
			{
				return null;
			}
			if (BreakableShape.IsValid())
			{
				BreakableShape.RemoveReference();
			}
			BreakableShape = new HkdCompoundBreakableShape(null, m_shapeInfosList);
			BreakableShape.SetChildrenParent(BreakableShape);
			try
			{
				BreakableShape.SetStrenghtRecursively(MyDestructionConstants.STRENGTH, 0.7f);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				MyLog.Default.WriteLine("BS Valid: " + BreakableShape.IsValid());
				MyLog.Default.WriteLine("BS Child count: " + BreakableShape.GetChildrenCount());
				MyLog.Default.WriteLine("Grid shapes: " + m_shapeInfosList.Count);
				foreach (HkdShapeInstanceInfo shapeInfos in m_shapeInfosList)
				{
					if (!shapeInfos.Shape.IsValid())
					{
						MyLog.Default.WriteLine("Invalid child!");
					}
					else
					{
						MyLog.Default.WriteLine("Child strength: " + shapeInfos.Shape.GetStrenght());
					}
				}
				MyLog.Default.WriteLine("Grid Blocks count: " + m_grid.GetBlocks().Count);
				MyLog.Default.WriteLine("Grid MarkedForClose: " + m_grid.MarkedForClose);
				HashSet<MyDefinitionId> hashSet = new HashSet<MyDefinitionId>();
				foreach (MySlimBlock block2 in m_grid.GetBlocks())
				{
					if (block2.FatBlock != null && block2.FatBlock.MarkedForClose)
					{
						MyLog.Default.WriteLine("Block marked for close: " + block2.BlockDefinition.Id);
					}
					if (hashSet.Count >= 50)
					{
						break;
					}
					if (block2.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block3 in (block2.FatBlock as MyCompoundCubeBlock).GetBlocks())
						{
							hashSet.Add(block3.BlockDefinition.Id);
							if (block3.FatBlock != null && block3.FatBlock.MarkedForClose)
							{
								MyLog.Default.WriteLine("Block in compound marked for close: " + block3.BlockDefinition.Id);
							}
						}
					}
					else
					{
						hashSet.Add(block2.BlockDefinition.Id);
					}
				}
				foreach (MyDefinitionId item in hashSet)
				{
					MyLog.Default.WriteLine("Block definition: " + item);
				}
				throw new InvalidOperationException();
			}
			CreateConnectionsManually(BreakableShape);
			m_shapeInfosList.Clear();
			return BreakableShape;
		}

		private static bool HasBreakableShape(string model, MyCubeBlockDefinition block)
		{
			MyModel modelOnlyData = MyModels.GetModelOnlyData(model);
			if (modelOnlyData != null && modelOnlyData.HavokBreakableShapes != null)
			{
				return modelOnlyData.HavokBreakableShapes.Length != 0;
			}
			return false;
		}

		private static HkdBreakableShape GetBreakableShape(string model, MyCubeBlockDefinition block, bool forceLoadDestruction = false)
		{
			if (MyFakes.LAZY_LOAD_DESTRUCTION || forceLoadDestruction)
			{
				MyModel modelOnlyData = MyModels.GetModelOnlyData(model);
				if (modelOnlyData.HavokBreakableShapes == null)
				{
					MyDestructionData.Static.LoadModelDestruction(model, block, modelOnlyData.BoundingBoxSize);
				}
			}
			return MyDestructionData.Static.BlockShapePool.GetBreakableShape(model, block);
		}

		private HkdBreakableShape CreateBlockShape(MySlimBlock b, out Matrix blockTransform)
		{
			blockTransform = Matrix.Identity;
			if (b.FatBlock == null)
			{
				return null;
			}
			HkdBreakableShape hkdBreakableShape = new HkdBreakableShape();
			Matrix result = Matrix.Identity;
			if (b.FatBlock is MyCompoundCubeBlock)
			{
				blockTransform.Translation = b.FatBlock.PositionComp.LocalMatrix.Translation;
				MyCompoundCubeBlock myCompoundCubeBlock = b.FatBlock as MyCompoundCubeBlock;
				if (myCompoundCubeBlock.GetBlocksCount() == 1)
				{
					MySlimBlock mySlimBlock = myCompoundCubeBlock.GetBlocks()[0];
					ushort? blockId = myCompoundCubeBlock.GetBlockId(mySlimBlock);
					MyFractureComponentBase fractureComponent = mySlimBlock.GetFractureComponent();
					if (fractureComponent != null)
					{
						hkdBreakableShape = fractureComponent.Shape;
						hkdBreakableShape.AddReference();
					}
					else
					{
						MyCubeBlockDefinition blockDefinition = mySlimBlock.FatBlock.BlockDefinition;
						Matrix orientation;
						string text = mySlimBlock.CalculateCurrentModel(out orientation);
						if (!MyFakes.LAZY_LOAD_DESTRUCTION && !HasBreakableShape(text, blockDefinition))
						{
							MySandboxGame.Log.WriteLine("Breakable shape not preallocated: " + text + " definition: " + blockDefinition);
							GetBreakableShape(text, blockDefinition, forceLoadDestruction: true);
						}
						if (MyFakes.LAZY_LOAD_DESTRUCTION || HasBreakableShape(text, blockDefinition))
						{
							hkdBreakableShape = GetBreakableShape(text, blockDefinition);
						}
					}
					if (hkdBreakableShape.IsValid())
					{
						HkPropertyBase prop = new HkSimpleValueProperty((uint)blockId.Value);
						hkdBreakableShape.SetPropertyRecursively(256, prop);
						prop.RemoveReference();
					}
					mySlimBlock.Orientation.GetMatrix(out result);
					blockTransform = result * blockTransform;
				}
				else
				{
					_ = b.Position * m_grid.GridSize;
					float num = 0f;
					foreach (MySlimBlock block in myCompoundCubeBlock.GetBlocks())
					{
						block.Orientation.GetMatrix(out result);
						result.Translation = Vector3.Zero;
						ushort? blockId2 = myCompoundCubeBlock.GetBlockId(block);
						MyFractureComponentBase fractureComponent2 = block.GetFractureComponent();
						if (fractureComponent2 != null)
						{
							hkdBreakableShape = fractureComponent2.Shape;
							hkdBreakableShape.UserObject |= 1u;
							hkdBreakableShape.AddReference();
							m_shapeInfosList2.Add(new HkdShapeInstanceInfo(hkdBreakableShape, result));
						}
						else
						{
							MyCubeBlockDefinition blockDefinition2 = block.BlockDefinition;
							Matrix orientation2;
							string text2 = block.CalculateCurrentModel(out orientation2);
							if (!MyFakes.LAZY_LOAD_DESTRUCTION && !HasBreakableShape(text2, blockDefinition2))
							{
								MySandboxGame.Log.WriteLine("Breakable shape not preallocated: " + text2 + " definition: " + blockDefinition2);
								GetBreakableShape(text2, blockDefinition2, forceLoadDestruction: true);
							}
							if (MyFakes.LAZY_LOAD_DESTRUCTION || HasBreakableShape(text2, blockDefinition2))
							{
								hkdBreakableShape = GetBreakableShape(text2, blockDefinition2);
								hkdBreakableShape.UserObject |= 1u;
								num += blockDefinition2.Mass;
								m_shapeInfosList2.Add(new HkdShapeInstanceInfo(hkdBreakableShape, result));
							}
						}
						if (hkdBreakableShape.IsValid())
						{
							HkPropertyBase prop2 = new HkSimpleValueProperty((uint)blockId2.Value);
							hkdBreakableShape.SetPropertyRecursively(256, prop2);
							prop2.RemoveReference();
						}
					}
					if (m_shapeInfosList2.Count == 0)
					{
						return null;
					}
					HkdBreakableShape hkdBreakableShape2 = new HkdCompoundBreakableShape(null, m_shapeInfosList2);
					((HkdCompoundBreakableShape)hkdBreakableShape2).RecalcMassPropsFromChildren();
					HkMassProperties massProperties = default(HkMassProperties);
					hkdBreakableShape2.BuildMassProperties(ref massProperties);
					hkdBreakableShape = new HkdBreakableShape(hkdBreakableShape2.GetShape(), ref massProperties);
					hkdBreakableShape2.RemoveReference();
					foreach (HkdShapeInstanceInfo item in m_shapeInfosList2)
					{
						HkdShapeInstanceInfo shapeInfo = item;
						hkdBreakableShape.AddShape(ref shapeInfo);
					}
					for (int i = 0; i < m_shapeInfosList2.Count; i++)
					{
						for (int j = 0; j < m_shapeInfosList2.Count; j++)
						{
							if (i != j)
							{
								ConnectShapesWithChildren(hkdBreakableShape, m_shapeInfosList2[i].Shape, m_shapeInfosList2[j].Shape);
							}
						}
					}
					foreach (HkdShapeInstanceInfo item2 in m_shapeInfosList2)
					{
						item2.Shape.RemoveReference();
						item2.RemoveReference();
					}
					m_shapeInfosList2.Clear();
				}
			}
			else
			{
				b.Orientation.GetMatrix(out blockTransform);
				blockTransform.Translation = b.FatBlock.PositionComp.LocalMatrix.Translation;
				Matrix orientation3;
				string text3 = b.CalculateCurrentModel(out orientation3);
				if (b.FatBlock is MyFracturedBlock)
				{
					hkdBreakableShape = (b.FatBlock as MyFracturedBlock).Shape;
					if (!hkdBreakableShape.IsValid())
					{
						throw new Exception("Fractured block Breakable shape invalid!");
					}
					hkdBreakableShape.AddReference();
				}
				else
				{
					MyFractureComponentBase fractureComponent3 = b.GetFractureComponent();
					if (fractureComponent3 != null)
					{
						hkdBreakableShape = fractureComponent3.Shape;
						hkdBreakableShape.AddReference();
					}
					else
					{
						if (!MyFakes.LAZY_LOAD_DESTRUCTION && !HasBreakableShape(text3, b.BlockDefinition))
						{
							MySandboxGame.Log.WriteLine("Breakable shape not preallocated: " + text3 + " definition: " + b.BlockDefinition);
							GetBreakableShape(text3, b.BlockDefinition, forceLoadDestruction: true);
						}
						if (MyFakes.LAZY_LOAD_DESTRUCTION || HasBreakableShape(text3, b.BlockDefinition))
						{
							hkdBreakableShape = GetBreakableShape(text3, b.BlockDefinition);
						}
					}
				}
			}
			HkPropertyBase prop3 = new HkVec3IProperty(b.Position);
			if (!hkdBreakableShape.IsValid())
			{
				MySandboxGame.Log.WriteLine(string.Concat("BreakableShape not valid: ", b.BlockDefinition.Id, " pos: ", b.Min, " grid cubes: ", b.CubeGrid.BlocksCount));
				if (b.FatBlock is MyCompoundCubeBlock)
				{
					MyCompoundCubeBlock myCompoundCubeBlock2 = b.FatBlock as MyCompoundCubeBlock;
					MySandboxGame.Log.WriteLine("Compound blocks count: " + myCompoundCubeBlock2.GetBlocksCount());
					foreach (MySlimBlock block2 in myCompoundCubeBlock2.GetBlocks())
					{
						MySandboxGame.Log.WriteLine("Block in compound: " + block2.BlockDefinition.Id);
					}
				}
			}
			hkdBreakableShape.SetPropertyRecursively(255, prop3);
			prop3.RemoveReference();
			return hkdBreakableShape;
		}

		private void UpdateConnectionsManually(HkdBreakableShape shape, HashSet<Vector3I> dirtyCubes)
		{
			uint num = 0u;
			foreach (Vector3I dirtyCube in dirtyCubes)
			{
				MySlimBlock cubeBlock = m_grid.GetCubeBlock(dirtyCube);
				if (cubeBlock != null && !m_processedBlock.Contains(cubeBlock))
				{
					if (!m_connections.ContainsKey(cubeBlock.Position))
					{
						m_connections[cubeBlock.Position] = new List<HkdConnection>();
					}
					List<HkdConnection> blockConnections = m_connections[cubeBlock.Position];
					foreach (MySlimBlock neighbour in cubeBlock.Neighbours)
					{
						ConnectBlocks(shape, cubeBlock, neighbour, blockConnections);
						num++;
					}
					m_processedBlock.Add(cubeBlock);
				}
			}
			m_processedBlock.Clear();
		}

		public void CreateConnectionsManually(HkdBreakableShape shape)
		{
			m_connections.Clear();
			foreach (MySlimBlock cubeBlock in m_grid.CubeBlocks)
			{
				if (m_blocksShapes.ContainsKey(cubeBlock.Position))
				{
					if (!m_connections.ContainsKey(cubeBlock.Position))
					{
						m_connections[cubeBlock.Position] = new List<HkdConnection>();
					}
					List<HkdConnection> blockConnections = m_connections[cubeBlock.Position];
					foreach (MySlimBlock neighbour in cubeBlock.Neighbours)
					{
						if (m_blocksShapes.ContainsKey(neighbour.Position))
						{
							ConnectBlocks(shape, cubeBlock, neighbour, blockConnections);
						}
					}
				}
			}
			AddConnections();
		}

		private void AddConnections()
		{
			int num = 0;
			foreach (List<HkdConnection> value in m_connections.Values)
			{
				num += value.Count;
			}
			BreakableShape.ClearConnections();
			BreakableShape.ReplaceConnections(m_connections, num);
		}

		private bool CheckConnection(HkdConnection c)
		{
			HkdBreakableShape hkdBreakableShape = c.ShapeA;
			while (hkdBreakableShape.HasParent)
			{
				hkdBreakableShape = hkdBreakableShape.GetParent();
			}
			if (hkdBreakableShape != BreakableShape)
			{
				return false;
			}
			hkdBreakableShape = c.ShapeB;
			while (hkdBreakableShape.HasParent)
			{
				hkdBreakableShape = hkdBreakableShape.GetParent();
			}
			if (hkdBreakableShape != BreakableShape)
			{
				return false;
			}
			return true;
		}

		private void ConnectBlocks(HkdBreakableShape parent, MySlimBlock blockA, MySlimBlock blockB, List<HkdConnection> blockConnections)
		{
			if (!m_blocksShapes.ContainsKey(blockA.Position) || !m_blocksShapes.ContainsKey(blockB.Position))
			{
				return;
			}
			HkdShapeInstanceInfo hkdShapeInstanceInfo = m_blocksShapes[blockA.Position];
			HkdShapeInstanceInfo hkdShapeInstanceInfo2 = m_blocksShapes[blockB.Position];
			hkdShapeInstanceInfo2.GetChildren(m_shapeInfosList2);
			bool flag = hkdShapeInstanceInfo2.Shape.GetChildrenCount() == 0;
			foreach (HkdShapeInstanceInfo item3 in m_shapeInfosList2)
			{
				HkdShapeInstanceInfo current = item3;
				current.DynamicParent = HkdShapeInstanceInfo.INVALID_INDEX;
			}
			Vector3 value = blockB.Position * m_grid.GridSize;
			Vector3 vector = blockA.Position * m_grid.GridSize;
			Vector3 value2 = blockB.Position - blockA.Position;
			value2 = Vector3.Normalize(value2);
			Matrix orientation = hkdShapeInstanceInfo2.GetTransform().GetOrientation();
			for (int i = 0; i < m_shapeInfosList2.Count; i++)
			{
				HkdShapeInstanceInfo hkdShapeInstanceInfo3 = m_shapeInfosList2[i];
				Matrix transform = hkdShapeInstanceInfo3.GetTransform();
				for (ushort dynamicParent = hkdShapeInstanceInfo3.DynamicParent; dynamicParent != HkdShapeInstanceInfo.INVALID_INDEX; dynamicParent = m_shapeInfosList2[dynamicParent].DynamicParent)
				{
					transform *= m_shapeInfosList2[dynamicParent].GetTransform();
				}
				transform *= orientation;
				hkdShapeInstanceInfo3.Shape.GetShape().GetLocalAABB(0.1f, out Vector4 min, out Vector4 max);
				Vector3 value3 = value + Vector3.Transform(new Vector3(min), transform);
				if (((vector - value3) * value2).AbsMax() > 1.35f)
				{
					value3 = value + Vector3.Transform(new Vector3(max), transform);
					if (((vector - value3) * value2).AbsMax() > 1.35f)
					{
						continue;
					}
				}
				flag = true;
				HkdConnection item = CreateConnection(hkdShapeInstanceInfo.Shape, hkdShapeInstanceInfo3.Shape, vector, value + Vector3.Transform(hkdShapeInstanceInfo3.CoM, transform));
				blockConnections.Add(item);
				hkdShapeInstanceInfo3.GetChildren(m_shapeInfosList2);
				for (int j = m_shapeInfosList2.Count - hkdShapeInstanceInfo3.Shape.GetChildrenCount(); j < m_shapeInfosList2.Count; j++)
				{
					HkdShapeInstanceInfo hkdShapeInstanceInfo4 = m_shapeInfosList2[j];
					hkdShapeInstanceInfo4.DynamicParent = (ushort)i;
				}
			}
			if (flag)
			{
				HkdConnection item2 = CreateConnection(hkdShapeInstanceInfo.Shape, hkdShapeInstanceInfo2.Shape, blockA.Position * m_grid.GridSize, blockB.Position * m_grid.GridSize);
				blockConnections.Add(item2);
			}
			m_shapeInfosList2.Clear();
		}

		public static void ConnectShapesWithChildren(HkdBreakableShape parent, HkdBreakableShape shapeA, HkdBreakableShape shapeB)
		{
			lock (m_sharedParentLock)
			{
				HkdConnection hkdConnection = CreateConnection(shapeA, shapeB, shapeA.CoM, shapeB.CoM);
				hkdConnection.AddToCommonParent();
				hkdConnection.RemoveReference();
				shapeB.GetChildren(m_shapeInfosList3);
				foreach (HkdShapeInstanceInfo item in m_shapeInfosList3)
				{
					HkdConnection hkdConnection2 = CreateConnection(shapeA, item.Shape, shapeA.CoM, shapeB.CoM);
					hkdConnection2.AddToCommonParent();
					hkdConnection2.RemoveReference();
				}
				m_shapeInfosList3.Clear();
			}
		}

		private static HkdConnection CreateConnection(HkdBreakableShape aShape, HkdBreakableShape bShape, Vector3 pivotA, Vector3 pivotB)
		{
			Vector3 normal = bShape.CoM - aShape.CoM;
			return new HkdConnection(aShape, bShape, pivotA, pivotB, normal, 6.25f);
		}

		private void UpdateMass(HkRigidBody rigidBody, bool setMass = true)
		{
			if (rigidBody.GetMotionType() != HkMotionType.Keyframed)
			{
				if (!MyPerGameSettings.Destruction)
				{
					UpdateMassProperties();
				}
				if (setMass)
				{
					SetMass(rigidBody);
				}
			}
		}

		public void SetMass(HkRigidBody rigidBody)
		{
			if (m_grid.Physics.IsWelded || m_grid.GetPhysicsBody().WeldInfo.Children.Count != 0)
			{
				m_grid.GetPhysicsBody().WeldedRigidBody.SetMassProperties(ref m_massProperties);
				m_grid.GetPhysicsBody().WeldInfo.SetMassProps(m_massProperties);
				m_grid.Physics.UpdateMassProps();
			}
			else
			{
				rigidBody.Mass = m_massProperties.Mass;
				rigidBody.SetMassProperties(ref m_massProperties);
			}
			m_grid.NotifyMassPropertiesChanged();
			MySharedTensorsGroups.MarkGroupDirty(m_grid);
			MyGridPhysicalGroupData.InvalidateSharedMassPropertiesCache(m_grid);
		}

		public void MarkBreakable(HkWorld world, HkRigidBody rigidBody)
		{
			if (!MyPerGameSettings.Destruction)
			{
				world.BreakOffPartsUtil.MarkEntityBreakable(rigidBody, BreakImpulse);
			}
		}

		public void UnmarkBreakable(HkWorld world, HkRigidBody rigidBody)
		{
			if (!MyPerGameSettings.Destruction)
			{
				world.BreakOffPartsUtil.UnmarkEntityBreakable(rigidBody);
			}
		}

		public void RefreshMass()
		{
			m_blockCollector.CollectMassElements(m_grid, m_massElements);
			UpdateMass(m_grid.Physics.RigidBody);
			m_grid.SetInventoryMassDirty();
		}

		public void UpdateMassFromInventories(HashSet<MyCubeBlock> blocks, MyPhysicsBody rb)
		{
			if (!(rb.RigidBody == null) && (rb.RigidBody.IsFixed || !rb.RigidBody.IsFixedOrKeyframed))
			{
				float cargoMassMultiplier = 1f / MySession.Static.BlocksInventorySizeMultiplier;
				if (MyFakes.ENABLE_STATIC_INVENTORY_MASS)
				{
					cargoMassMultiplier = 0f;
				}
				HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(blocks.Count + 1);
				int length = CollectBlockInventories(blocks, cargoMassMultiplier, array);
				HkMassElement hkMassElement;
				if (MyPerGameSettings.Destruction)
				{
					HkMassProperties massProperties = default(HkMassProperties);
					BreakableShape.BuildMassProperties(ref massProperties);
					int num = length++;
					hkMassElement = new HkMassElement
					{
						Properties = massProperties,
						Tranform = Matrix.Identity
					};
					array[num] = hkMassElement;
				}
				else
				{
					int num2 = length++;
					hkMassElement = new HkMassElement
					{
						Properties = m_originalMassProperties,
						Tranform = Matrix.Identity
					};
					array[num2] = hkMassElement;
				}
				SetMassProperties(rb, new Span<HkMassElement>(array, 0, length));
				ArrayPool<HkMassElement>.Shared.Return(array);
			}
		}

		private static int CollectBlockInventories(HashSet<MyCubeBlock> blocks, float cargoMassMultiplier, HkMassElement[] massElementsOut)
		{
			int result = 0;
			foreach (MyCubeBlock block in blocks)
			{
				float num = 0f;
				if (block is MyCockpit)
				{
					MyCockpit myCockpit = block as MyCockpit;
					if (myCockpit.Pilot != null)
					{
						num += myCockpit.Pilot.BaseMass;
					}
				}
				if (block.HasInventory)
				{
					for (int i = 0; i < block.InventoryCount; i++)
					{
						MyInventory inventory = block.GetInventory(i);
						if (inventory != null)
						{
							num += (float)inventory.CurrentMass * cargoMassMultiplier;
						}
					}
				}
				if (num > 0f)
				{
					Vector3 value = (block.Max - block.Min + Vector3I.One) * block.CubeGrid.GridSize;
					Vector3 position = (block.Min + block.Max) * 0.5f * block.CubeGrid.GridSize;
					HkMassProperties hkMassProperties = default(HkMassProperties);
					hkMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(value / 2f, MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(num) : num);
					massElementsOut[result++] = new HkMassElement
					{
						Properties = hkMassProperties,
						Tranform = Matrix.CreateTranslation(position)
					};
				}
			}
			return result;
		}

		private void SetMassProperties(MyPhysicsBody rb, Span<HkMassElement> massElements)
		{
			HkInertiaTensorComputer.CombineMassProperties(massElements, out HkMassProperties massProperties);
			if (Math.Abs(massProperties.Mass - m_massProperties.Mass) / m_massProperties.Mass < m_massElements.UpdateThreshold)
			{
				return;
			}
			m_massProperties = massProperties;
			if (rb.IsWelded || rb.WeldInfo.Children.Count != 0)
			{
				rb.WeldedRigidBody.SetMassProperties(ref m_massProperties);
				rb.WeldInfo.SetMassProps(m_massProperties);
				rb.UpdateMassProps();
			}
			else
			{
				HkRigidBody rigidBody = rb.RigidBody;
				if (!rigidBody.IsFixedOrKeyframed || Vector3.Distance(rigidBody.CenterOfMassLocal, m_massProperties.CenterOfMass) > 1f)
				{
					rigidBody.SetMassProperties(ref m_massProperties);
				}
				MySharedTensorsGroups.MarkGroupDirty(m_grid);
				MyGridPhysicalGroupData.InvalidateSharedMassPropertiesCache(m_grid);
				rb.ActivateIfNeeded();
			}
			m_grid.NotifyMassPropertiesChanged();
		}

		public void MarkSharedTensorDirty()
		{
			m_isSharedTensorDirty = true;
		}

		public void RecomputeSharedTensorIfNeeded()
		{
			if (m_isSharedTensorDirty)
			{
				m_isSharedTensorDirty = false;
				HashSetReader<MyGroups<MyCubeGrid, MySharedTensorData>.Node> gridsInSameGroup = MySharedTensorsGroups.GetGridsInSameGroup(m_grid);
				int num = (!gridsInSameGroup.IsValid) ? 1 : gridsInSameGroup.Count;
				HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(num);
				Span<HkMassElement> elements = new Span<HkMassElement>(array, 0, num);
				int length = 0;
				HkMassElement hkMassElement;
				if (gridsInSameGroup.IsValid)
				{
					MatrixD worldMatrixNormalizedInv = m_grid.PositionComp.WorldMatrixNormalizedInv;
					foreach (MyGroups<MyCubeGrid, MySharedTensorData>.Node item in gridsInSameGroup)
					{
						MyCubeGrid nodeData = item.NodeData;
						if (nodeData != m_grid)
						{
							MyGridPhysics physics = nodeData.Physics;
							if (physics != null)
							{
								HkMassProperties massProperties = physics.Shape.m_massProperties;
								massProperties.Mass /= 10f;
								ref HkMassElement reference = ref elements[length++];
								hkMassElement = new HkMassElement
								{
									Properties = massProperties,
									Tranform = nodeData.PositionComp.WorldMatrix * worldMatrixNormalizedInv
								};
								reference = hkMassElement;
							}
						}
					}
				}
				hkMassElement = (elements[length++] = new HkMassElement
				{
					Tranform = Matrix.Identity,
					Properties = m_massProperties
				});
				elements = elements.Slice(0, length);
				HkInertiaTensorComputer.CombineMassProperties(elements, out HkMassProperties massProperties2);
				ArrayPool<HkMassElement>.Shared.Return(array);
				HkMassProperties properties = m_massProperties;
				properties.InertiaTensor = massProperties2.InertiaTensor;
				m_grid.Physics.RigidBody.SetMassProperties(ref properties);
				m_grid.NotifyMassPropertiesChanged();
			}
		}

		public static implicit operator HkShape(MyGridShape shape)
		{
			return shape.m_root;
		}

		public float GetBlockMass(Vector3I position)
		{
			if (m_blocksShapes.ContainsKey(position))
			{
				return MyDestructionHelper.MassFromHavok(m_blocksShapes[position].Shape.GetMass());
			}
			if (m_grid.CubeExists(position))
			{
				return m_grid.GetCubeBlock(position).GetMass();
			}
			return 1f;
		}

		internal void DebugDraw()
		{
			if (MyDebugDrawSettings.BREAKABLE_SHAPE_CHILD_COUNT)
			{
				foreach (KeyValuePair<Vector3I, HkdShapeInstanceInfo> blocksShape in m_blocksShapes)
				{
					Vector3D vector3D = m_grid.GridIntegerToWorld((blocksShape.Value.GetTransform().Translation + blocksShape.Value.CoM) / m_grid.GridSize);
					if (!((vector3D - MySector.MainCamera.Position).Length() > 20.0))
					{
						MyRenderProxy.DebugDrawText3D(vector3D, MyValueFormatter.GetFormatedInt(blocksShape.Value.Shape.GetChildrenCount()), Color.White, 0.65f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					}
				}
			}
			if (MyHonzaInputComponent.DefaultComponent.ShowRealBlockMass != MyHonzaInputComponent.DefaultComponent.ShownMassEnum.None && !((m_grid.PositionComp.GetPosition() - MySector.MainCamera.Position).Length() > 20.0 + m_grid.PositionComp.WorldVolume.Radius))
			{
				foreach (KeyValuePair<Vector3I, HkdShapeInstanceInfo> blocksShape2 in m_blocksShapes)
				{
					Vector3D vector3D2 = m_grid.GridIntegerToWorld((blocksShape2.Value.GetTransform().Translation + blocksShape2.Value.CoM) / m_grid.GridSize);
					if (!((vector3D2 - MySector.MainCamera.Position).Length() > 20.0))
					{
						MySlimBlock cubeBlock = m_grid.GetCubeBlock(blocksShape2.Key);
						if (cubeBlock != null)
						{
							float num = cubeBlock.GetMass();
							if (cubeBlock.FatBlock is MyFracturedBlock)
							{
								num = m_blocksShapes[cubeBlock.Position].Shape.GetMass();
							}
							switch (MyHonzaInputComponent.DefaultComponent.ShowRealBlockMass)
							{
							case MyHonzaInputComponent.DefaultComponent.ShownMassEnum.Real:
								num = MyDestructionHelper.MassFromHavok(num);
								break;
							case MyHonzaInputComponent.DefaultComponent.ShownMassEnum.SI:
								num = MyDestructionHelper.MassFromHavok(num);
								break;
							}
							MyRenderProxy.DebugDrawText3D(vector3D2, MyValueFormatter.GetFormatedFloat(num, (num < 10f) ? 2 : 0), Color.White, 0.6f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
						}
					}
				}
			}
		}

		internal void RemoveBlock(MySlimBlock block)
		{
			m_tmpRemovedCubes.Add(block.Min);
			m_root.RemoveShapes(m_tmpRemovedCubes, null, null, null);
			m_tmpRemovedCubes.Clear();
		}

		public void GetShapeBounds(uint shapeKey, out Vector3I min, out Vector3I max)
		{
			m_root.GetShapeBounds(shapeKey, out min, out max);
		}

		public MySlimBlock GetBlockFromShapeKey(uint shapeKey)
		{
			m_root.GetShapeMin(shapeKey, out Vector3S min);
			return m_grid.GetCubeBlock(min);
		}
	}
}
