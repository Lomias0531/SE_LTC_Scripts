using Havok;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyCubeBlockCollector : IDisposable
	{
		public struct ShapeInfo
		{
			public int Count;

			public Vector3I Min;

			public Vector3I Max;
		}

		public const bool SHRINK_CONVEX_SHAPE = false;

		public const float BOX_SHRINK = 0f;

		private const bool ADD_INNER_BONES_TO_CONVEX = true;

		private const float MAX_BOX_EXTENT = 40f;

		public List<ShapeInfo> ShapeInfos = new List<ShapeInfo>();

		public List<HkShape> Shapes = new List<HkShape>();

		private HashSet<MySlimBlock> m_tmpRefreshSet = new HashSet<MySlimBlock>();

		private MyList<Vector3> m_tmpHelperVerts = new MyList<Vector3>();

		private List<Vector3I> m_tmpCubes = new List<Vector3I>();

		private HashSet<Vector3I> m_tmpCheck;

		public void Dispose()
		{
			Clear();
		}

		public void Clear()
		{
			ShapeInfos.Clear();
			foreach (HkShape shape in Shapes)
			{
				shape.RemoveReference();
			}
			Shapes.Clear();
		}

		private bool IsValid()
		{
			if (m_tmpCheck == null)
			{
				m_tmpCheck = new HashSet<Vector3I>();
			}
			try
			{
				foreach (ShapeInfo shapeInfo in ShapeInfos)
				{
					Vector3I item = default(Vector3I);
					item.X = shapeInfo.Min.X;
					while (item.X <= shapeInfo.Max.X)
					{
						item.Y = shapeInfo.Min.Y;
						while (item.Y <= shapeInfo.Max.Y)
						{
							item.Z = shapeInfo.Min.Z;
							while (item.Z <= shapeInfo.Max.Z)
							{
								if (!m_tmpCheck.Add(item))
								{
									return false;
								}
								item.Z++;
							}
							item.Y++;
						}
						item.X++;
					}
				}
				return true;
			}
			finally
			{
				m_tmpCheck.Clear();
			}
		}

		public void Collect(MyCubeGrid grid, MyVoxelSegmentation segmenter, MyVoxelSegmentationType segmentationType, IDictionary<Vector3I, HkMassElement> massResults)
		{
			foreach (MySlimBlock block in grid.GetBlocks())
			{
				if (block.FatBlock is MyCompoundCubeBlock)
				{
					CollectCompoundBlock((MyCompoundCubeBlock)block.FatBlock, massResults);
				}
				else
				{
					CollectBlock(block, block.BlockDefinition.PhysicsOption, massResults);
				}
			}
			AddSegmentedParts(grid.GridSize, segmenter, segmentationType);
			m_tmpCubes.Clear();
		}

		public void CollectArea(MyCubeGrid grid, HashSet<Vector3I> dirtyBlocks, MyVoxelSegmentation segmenter, MyVoxelSegmentationType segmentationType, IDictionary<Vector3I, HkMassElement> massResults)
		{
			using (MyUtils.ReuseCollection(ref m_tmpRefreshSet))
			{
				foreach (Vector3I dirtyBlock in dirtyBlocks)
				{
					massResults?.Remove(dirtyBlock);
					MySlimBlock cubeBlock = grid.GetCubeBlock(dirtyBlock);
					if (cubeBlock != null)
					{
						m_tmpRefreshSet.Add(cubeBlock);
					}
				}
				foreach (MySlimBlock item in m_tmpRefreshSet)
				{
					CollectBlock(item, item.BlockDefinition.PhysicsOption, massResults);
				}
				AddSegmentedParts(grid.GridSize, segmenter, segmentationType);
				m_tmpCubes.Clear();
			}
		}

		public void CollectMassElements(MyCubeGrid grid, IDictionary<Vector3I, HkMassElement> massResults)
		{
			if (massResults != null)
			{
				foreach (MySlimBlock block in grid.GetBlocks())
				{
					if (block.FatBlock is MyCompoundCubeBlock)
					{
						foreach (MySlimBlock block2 in ((MyCompoundCubeBlock)block.FatBlock).GetBlocks())
						{
							if (block2.BlockDefinition.BlockTopology == MyBlockTopology.TriangleMesh)
							{
								AddMass(block2, massResults);
							}
						}
					}
					else
					{
						AddMass(block, massResults);
					}
				}
			}
		}

		private void CollectCompoundBlock(MyCompoundCubeBlock compoundBlock, IDictionary<Vector3I, HkMassElement> massResults)
		{
			int count = ShapeInfos.Count;
			foreach (MySlimBlock block in compoundBlock.GetBlocks())
			{
				if (block.BlockDefinition.BlockTopology == MyBlockTopology.TriangleMesh)
				{
					CollectBlock(block, block.BlockDefinition.PhysicsOption, massResults, allowSegmentation: false);
				}
			}
			if (ShapeInfos.Count > count + 1)
			{
				ShapeInfo value = ShapeInfos[count];
				while (ShapeInfos.Count > count + 1)
				{
					int index = ShapeInfos.Count - 1;
					value.Count += ShapeInfos[index].Count;
					ShapeInfos.RemoveAt(index);
				}
				ShapeInfos[count] = value;
			}
		}

		private void AddSegmentedParts(float gridSize, MyVoxelSegmentation segmenter, MyVoxelSegmentationType segmentationType)
		{
			int num = (int)Math.Floor(40f / gridSize);
			Vector3 value = new Vector3(gridSize * 0.5f);
			if (segmenter != null)
			{
				int mergeIterations = (segmentationType == MyVoxelSegmentationType.Optimized) ? 1 : 0;
				segmenter.ClearInput();
				foreach (Vector3I tmpCube in m_tmpCubes)
				{
					segmenter.AddInput(tmpCube);
				}
				foreach (MyVoxelSegmentation.Segment item in segmenter.FindSegments(segmentationType, mergeIterations))
				{
					Vector3I vector3I = default(Vector3I);
					vector3I.X = item.Min.X;
					while (vector3I.X <= item.Max.X)
					{
						vector3I.Y = item.Min.Y;
						while (vector3I.Y <= item.Max.Y)
						{
							vector3I.Z = item.Min.Z;
							while (vector3I.Z <= item.Max.Z)
							{
								Vector3I vector3I2 = Vector3I.Min(vector3I + num - 1, item.Max);
								Vector3 min = vector3I * gridSize - value;
								Vector3 max = vector3I2 * gridSize + value;
								AddBox(vector3I, vector3I2, ref min, ref max);
								vector3I.Z += num;
							}
							vector3I.Y += num;
						}
						vector3I.X += num;
					}
				}
			}
			else
			{
				foreach (Vector3I tmpCube2 in m_tmpCubes)
				{
					Vector3 min2 = tmpCube2 * gridSize - value;
					Vector3 max2 = tmpCube2 * gridSize + value;
					AddBox(tmpCube2, tmpCube2, ref min2, ref max2);
				}
			}
		}

		private void AddBox(Vector3I minPos, Vector3I maxPos, ref Vector3 min, ref Vector3 max)
		{
			Vector3 vector = (min + max) * 0.5f;
			Vector3 halfExtents = max - vector;
			halfExtents -= 0f;
			HkBoxShape shape = new HkBoxShape(halfExtents, MyPerGameSettings.PhysicsConvexRadius);
			HkConvexTranslateShape shape2 = new HkConvexTranslateShape(shape, vector, HkReferencePolicy.TakeOwnership);
			Shapes.Add(shape2);
			ShapeInfos.Add(new ShapeInfo
			{
				Count = 1,
				Min = minPos,
				Max = maxPos
			});
		}

		private void CollectBlock(MySlimBlock block, MyPhysicsOption physicsOption, IDictionary<Vector3I, HkMassElement> massResults, bool allowSegmentation = true)
		{
			if (!block.BlockDefinition.HasPhysics || block.CubeGrid == null)
			{
				return;
			}
			if (massResults != null)
			{
				AddMass(block, massResults);
			}
			if (block.BlockDefinition.BlockTopology == MyBlockTopology.Cube)
			{
				MyCubeTopology myCubeTopology = (block.BlockDefinition.CubeDefinition != null) ? block.BlockDefinition.CubeDefinition.CubeTopology : MyCubeTopology.Box;
				if (MyFakes.ENABLE_SIMPLE_GRID_PHYSICS)
				{
					physicsOption = MyPhysicsOption.Box;
				}
				else if (myCubeTopology == MyCubeTopology.Box)
				{
					if (!block.ShowParts)
					{
						physicsOption = MyPhysicsOption.Box;
					}
					else if (block.BlockDefinition.CubeDefinition != null && block.CubeGrid.Skeleton.IsDeformed(block.Min, 0.05f, block.CubeGrid, checkBlockDefinition: false))
					{
						physicsOption = MyPhysicsOption.Convex;
					}
				}
				switch (physicsOption)
				{
				case MyPhysicsOption.Box:
					AddBoxes(block);
					break;
				case MyPhysicsOption.Convex:
					AddConvexShape(block, block.ShowParts);
					break;
				}
			}
			else
			{
				if (physicsOption == MyPhysicsOption.None)
				{
					return;
				}
				HkShape[] array = null;
				if (block.FatBlock != null)
				{
					array = block.FatBlock.ModelCollision.HavokCollisionShapes;
				}
				if (array != null && array.Length != 0 && !MyFakes.ENABLE_SIMPLE_GRID_PHYSICS)
				{
					Vector3 translation = (!block.FatBlock.ModelCollision.ExportedWrong) ? block.FatBlock.PositionComp.LocalMatrix.Translation : (block.Position * block.CubeGrid.GridSize);
					HkShape[] havokCollisionShapes = block.FatBlock.ModelCollision.HavokCollisionShapes;
					block.Orientation.GetQuaternion(out Quaternion result);
					Vector3 scale = Vector3.One * block.FatBlock.ModelCollision.ScaleFactor;
					if (havokCollisionShapes.Length == 1 && havokCollisionShapes[0].ShapeType == HkShapeType.List)
					{
						HkListShape hkListShape = (HkListShape)havokCollisionShapes[0];
						for (int i = 0; i < hkListShape.TotalChildrenCount; i++)
						{
							HkShape childByIndex = hkListShape.GetChildByIndex(i);
							Shapes.Add(new HkConvexTransformShape((HkConvexShape)childByIndex, ref translation, ref result, ref scale, HkReferencePolicy.None));
						}
					}
					else if (havokCollisionShapes.Length == 1 && havokCollisionShapes[0].ShapeType == HkShapeType.Mopp)
					{
						HkMoppBvTreeShape hkMoppBvTreeShape = (HkMoppBvTreeShape)havokCollisionShapes[0];
						for (int j = 0; j < hkMoppBvTreeShape.ShapeCollection.ShapeCount; j++)
						{
							HkShape shape = hkMoppBvTreeShape.ShapeCollection.GetShape((uint)j, null);
							Shapes.Add(new HkConvexTransformShape((HkConvexShape)shape, ref translation, ref result, ref scale, HkReferencePolicy.None));
						}
					}
					else
					{
						for (int k = 0; k < havokCollisionShapes.Length; k++)
						{
							Shapes.Add(new HkConvexTransformShape((HkConvexShape)havokCollisionShapes[k], ref translation, ref result, ref scale, HkReferencePolicy.None));
						}
					}
					ShapeInfos.Add(new ShapeInfo
					{
						Count = havokCollisionShapes.Length,
						Min = block.Min,
						Max = block.Max
					});
					return;
				}
				for (int l = block.Min.X; l <= block.Max.X; l++)
				{
					for (int m = block.Min.Y; m <= block.Max.Y; m++)
					{
						for (int n = block.Min.Z; n <= block.Max.Z; n++)
						{
							Vector3I vector3I = new Vector3I(l, m, n);
							if (allowSegmentation)
							{
								m_tmpCubes.Add(vector3I);
								continue;
							}
							Vector3 min = vector3I * block.CubeGrid.GridSize - new Vector3(block.CubeGrid.GridSize / 2f);
							Vector3 max = vector3I * block.CubeGrid.GridSize + new Vector3(block.CubeGrid.GridSize / 2f);
							AddBox(vector3I, vector3I, ref min, ref max);
						}
					}
				}
			}
		}

		private void AddMass(MySlimBlock block, IDictionary<Vector3I, HkMassElement> massResults)
		{
			float num = block.BlockDefinition.Mass;
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && block.FatBlock is MyCompoundCubeBlock)
			{
				num = 0f;
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					num += block2.GetMass();
				}
			}
			Vector3 value = (block.Max - block.Min + Vector3I.One) * block.CubeGrid.GridSize;
			Vector3 position = (block.Min + block.Max) * 0.5f * block.CubeGrid.GridSize;
			HkMassProperties hkMassProperties = default(HkMassProperties);
			hkMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(value / 2f, num);
			massResults[block.Position] = new HkMassElement
			{
				Properties = hkMassProperties,
				Tranform = Matrix.CreateTranslation(position)
			};
		}

		private void AddConvexShape(MySlimBlock block, bool applySkeleton)
		{
			m_tmpHelperVerts.Clear();
			Vector3 value = block.Min * block.CubeGrid.GridSize;
			Vector3I a = block.Min * 2 + 1;
			MyGridSkeleton skeleton = block.CubeGrid.Skeleton;
			foreach (Vector3 blockVertex in MyBlockVerticesCache.GetBlockVertices(block.BlockDefinition.CubeDefinition.CubeTopology, block.Orientation))
			{
				Vector3I pos = a + Vector3I.Round(blockVertex);
				Vector3 value2 = blockVertex * block.CubeGrid.GridSizeHalf;
				if (applySkeleton && skeleton.TryGetBone(ref pos, out Vector3 bone))
				{
					value2.Add(bone);
				}
				m_tmpHelperVerts.Add(value2 + value);
			}
			Shapes.Add(new HkConvexVerticesShape(m_tmpHelperVerts.GetInternalArray(), m_tmpHelperVerts.Count, shrink: false, MyPerGameSettings.PhysicsConvexRadius));
			ShapeInfos.Add(new ShapeInfo
			{
				Count = 1,
				Min = block.Min,
				Max = block.Max
			});
		}

		private void AddBoxes(MySlimBlock block)
		{
			for (int i = block.Min.X; i <= block.Max.X; i++)
			{
				for (int j = block.Min.Y; j <= block.Max.Y; j++)
				{
					for (int k = block.Min.Z; k <= block.Max.Z; k++)
					{
						Vector3I item = new Vector3I(i, j, k);
						m_tmpCubes.Add(item);
					}
				}
			}
		}
	}
}
