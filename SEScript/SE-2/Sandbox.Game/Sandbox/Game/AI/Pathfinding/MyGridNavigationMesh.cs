using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyGridNavigationMesh : MyNavigationMesh
	{
		private struct EdgeIndex : IEquatable<EdgeIndex>
		{
			public Vector3I A;

			public Vector3I B;

			public EdgeIndex(Vector3I PointA, Vector3I PointB)
			{
				A = PointA;
				B = PointB;
			}

			public EdgeIndex(ref Vector3I PointA, ref Vector3I PointB)
			{
				A = PointA;
				B = PointB;
			}

			public override int GetHashCode()
			{
				return A.GetHashCode() * 1610612741 + B.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (!(obj is EdgeIndex))
				{
					return false;
				}
				return Equals((EdgeIndex)obj);
			}

			public override string ToString()
			{
				return "(" + A.ToString() + ", " + B.ToString() + ")";
			}

			public bool Equals(EdgeIndex other)
			{
				if (A == other.A)
				{
					return B == other.B;
				}
				return false;
			}
		}

		public class Component : IMyHighLevelComponent
		{
			private MyGridNavigationMesh m_parent;

			private int m_componentIndex;

			public bool FullyExplored => true;

			public Component(MyGridNavigationMesh parent, int componentIndex)
			{
				m_parent = parent;
				m_componentIndex = componentIndex;
			}

			public bool Contains(MyNavigationPrimitive primitive)
			{
				if (primitive.Group != m_parent)
				{
					return false;
				}
				MyNavigationTriangle myNavigationTriangle = primitive as MyNavigationTriangle;
				if (myNavigationTriangle == null)
				{
					return false;
				}
				return myNavigationTriangle.ComponentIndex == m_componentIndex;
			}
		}

		private MyCubeGrid m_grid;

		private Dictionary<Vector3I, List<int>> m_smallTriangleRegistry;

		private MyVector3ISet m_cubeSet;

		private Dictionary<EdgeIndex, int> m_connectionHelper;

		private MyNavmeshCoordinator m_coordinator;

		private MyHighLevelGroup m_higherLevel;

		private MyGridHighLevelHelper m_higherLevelHelper;

		private Component m_component;

		private static HashSet<Vector3I> m_mergeHelper;

		private static List<KeyValuePair<MyNavigationTriangle, Vector3I>> m_tmpTriangleList;

		private bool m_static;

		public bool HighLevelDirty => m_higherLevelHelper.IsDirty;

		public override MyHighLevelGroup HighLevelGroup => m_higherLevel;

		static MyGridNavigationMesh()
		{
			m_mergeHelper = new HashSet<Vector3I>();
			m_tmpTriangleList = new List<KeyValuePair<MyNavigationTriangle, Vector3I>>();
		}

		public MyGridNavigationMesh(MyCubeGrid grid, MyNavmeshCoordinator coordinator, int triPrealloc = 32, Func<long> timestampFunction = null)
			: base(coordinator?.Links, triPrealloc, timestampFunction)
		{
			m_connectionHelper = new Dictionary<EdgeIndex, int>();
			m_smallTriangleRegistry = new Dictionary<Vector3I, List<int>>();
			m_cubeSet = new MyVector3ISet();
			m_coordinator = coordinator;
			m_static = false;
			if (grid != null)
			{
				m_higherLevel = new MyHighLevelGroup(this, coordinator.HighLevelLinks, timestampFunction);
				m_higherLevelHelper = new MyGridHighLevelHelper(this, m_smallTriangleRegistry, new Vector3I(8, 8, 8));
				m_grid = grid;
				grid.OnBlockAdded += grid_OnBlockAdded;
				grid.OnBlockRemoved += grid_OnBlockRemoved;
				float scaleFactor = 1f / (float)grid.CubeBlocks.Count;
				Vector3 zero = Vector3.Zero;
				foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
				{
					OnBlockAddedInternal(cubeBlock);
					zero += cubeBlock.Position * grid.GridSize * scaleFactor;
				}
			}
		}

		public override string ToString()
		{
			return "Grid NavMesh: " + m_grid.DisplayName;
		}

		public void UpdateHighLevel()
		{
			m_higherLevelHelper.ProcessChangedCellComponents();
		}

		public MyNavigationTriangle AddTriangle(ref Vector3 a, ref Vector3 b, ref Vector3 c)
		{
			if (m_grid != null)
			{
				return null;
			}
			return AddTriangleInternal(a, b, c);
		}

		private MyNavigationTriangle AddTriangleInternal(Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3I PointB = Vector3I.Round(a * 256f);
			Vector3I PointA = Vector3I.Round(b * 256f);
			Vector3I PointA2 = Vector3I.Round(c * 256f);
			Vector3 A = PointB;
			A /= 256f;
			Vector3 C = PointA;
			C /= 256f;
			Vector3 B = PointA2;
			B /= 256f;
			if (!m_connectionHelper.TryGetValue(new EdgeIndex(ref PointA, ref PointB), out int value))
			{
				value = -1;
			}
			if (!m_connectionHelper.TryGetValue(new EdgeIndex(ref PointA2, ref PointA), out int value2))
			{
				value2 = -1;
			}
			if (!m_connectionHelper.TryGetValue(new EdgeIndex(ref PointB, ref PointA2), out int value3))
			{
				value3 = -1;
			}
			int num = value;
			int num2 = value2;
			int num3 = value3;
			MyNavigationTriangle result = AddTriangle(ref A, ref B, ref C, ref value3, ref value2, ref value);
			if (num == -1)
			{
				m_connectionHelper.Add(new EdgeIndex(ref PointB, ref PointA), value);
			}
			else
			{
				m_connectionHelper.Remove(new EdgeIndex(ref PointA, ref PointB));
			}
			if (num2 == -1)
			{
				m_connectionHelper.Add(new EdgeIndex(ref PointA, ref PointA2), value2);
			}
			else
			{
				m_connectionHelper.Remove(new EdgeIndex(ref PointA2, ref PointA));
			}
			if (num3 == -1)
			{
				m_connectionHelper.Add(new EdgeIndex(ref PointA2, ref PointB), value3);
			}
			else
			{
				m_connectionHelper.Remove(new EdgeIndex(ref PointB, ref PointA2));
			}
			return result;
		}

		public void RegisterTriangle(MyNavigationTriangle tri, ref Vector3I gridPos)
		{
			if (m_grid == null)
			{
				RegisterTriangleInternal(tri, ref gridPos);
			}
		}

		private void RegisterTriangleInternal(MyNavigationTriangle tri, ref Vector3I gridPos)
		{
			List<int> value = null;
			if (!m_smallTriangleRegistry.TryGetValue(gridPos, out value))
			{
				value = new List<int>();
				m_smallTriangleRegistry.Add(gridPos, value);
			}
			value.Add(tri.Index);
			tri.Registered = true;
		}

		public MyVector3ISet.Enumerator GetCubes()
		{
			return m_cubeSet.GetEnumerator();
		}

		public void GetCubeTriangles(Vector3I gridPos, List<MyNavigationTriangle> trianglesOut)
		{
			List<int> value = null;
			if (m_smallTriangleRegistry.TryGetValue(gridPos, out value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					trianglesOut.Add(GetTriangle(value[i]));
				}
			}
		}

		private void MergeFromAnotherMesh(MyGridNavigationMesh otherMesh, ref MatrixI transform)
		{
			m_mergeHelper.Clear();
			foreach (Vector3I key2 in otherMesh.m_smallTriangleRegistry.Keys)
			{
				bool flag = false;
				Vector3I[] intDirections = Base6Directions.IntDirections;
				foreach (Vector3I b in intDirections)
				{
					Vector3I position = Vector3I.Transform(key2 + b, transform);
					if (m_cubeSet.Contains(ref position))
					{
						m_mergeHelper.Add(key2 + b);
						flag = true;
					}
				}
				if (flag)
				{
					m_mergeHelper.Add(key2);
				}
			}
			foreach (KeyValuePair<Vector3I, List<int>> item in otherMesh.m_smallTriangleRegistry)
			{
				Vector3I value = item.Key;
				Vector3I.Transform(ref value, ref transform, out Vector3I result);
				if (m_mergeHelper.Contains(value))
				{
					m_tmpTriangleList.Clear();
					Base6Directions.Direction[] enumDirections = Base6Directions.EnumDirections;
					foreach (Base6Directions.Direction direction in enumDirections)
					{
						Vector3I intVector = Base6Directions.GetIntVector((int)direction);
						Vector3I intVector2 = Base6Directions.GetIntVector((int)Base6Directions.GetFlippedDirection(transform.GetDirection(direction)));
						if (m_mergeHelper.Contains(value + intVector))
						{
							List<int> value2 = null;
							if (m_smallTriangleRegistry.TryGetValue(result - intVector2, out value2))
							{
								foreach (int item2 in value2)
								{
									MyNavigationTriangle triangle = GetTriangle(item2);
									if (IsFaceTriangle(triangle, result - intVector2, intVector2))
									{
										m_tmpTriangleList.Add(new KeyValuePair<MyNavigationTriangle, Vector3I>(triangle, result - intVector2));
									}
								}
							}
						}
					}
					foreach (KeyValuePair<MyNavigationTriangle, Vector3I> tmpTriangle in m_tmpTriangleList)
					{
						RemoveTriangle(tmpTriangle.Key, tmpTriangle.Value);
					}
					m_tmpTriangleList.Clear();
					int num = 0;
					foreach (int item3 in item.Value)
					{
						MyNavigationTriangle triangle2 = otherMesh.GetTriangle(item3);
						Vector3I key = item.Key;
						bool flag2 = true;
						enumDirections = Base6Directions.EnumDirections;
						for (int i = 0; i < enumDirections.Length; i++)
						{
							Vector3I intVector3 = Base6Directions.GetIntVector((int)enumDirections[i]);
							if (m_mergeHelper.Contains(key + intVector3) && IsFaceTriangle(triangle2, key, intVector3))
							{
								flag2 = false;
								break;
							}
						}
						if (flag2)
						{
							_ = 5;
							CopyTriangle(triangle2, key, ref transform);
							num++;
						}
					}
				}
				else
				{
					foreach (int item4 in item.Value)
					{
						MyNavigationTriangle triangle3 = otherMesh.GetTriangle(item4);
						CopyTriangle(triangle3, item.Key, ref transform);
					}
				}
			}
			m_mergeHelper.Clear();
		}

		private bool IsFaceTriangle(MyNavigationTriangle triangle, Vector3I cubePosition, Vector3I direction)
		{
			MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = triangle.GetVertexEnumerator();
			vertexEnumerator.MoveNext();
			Vector3I a = Vector3I.Round(vertexEnumerator.Current * 256f);
			vertexEnumerator.MoveNext();
			Vector3I a2 = Vector3I.Round(vertexEnumerator.Current * 256f);
			vertexEnumerator.MoveNext();
			Vector3I a3 = Vector3I.Round(vertexEnumerator.Current * 256f);
			cubePosition *= 256;
			Vector3I vector3I = cubePosition + direction * 128;
			a -= vector3I;
			a2 -= vector3I;
			a3 -= vector3I;
			if (a * direction != Vector3I.Zero)
			{
				return false;
			}
			if (a2 * direction != Vector3I.Zero)
			{
				return false;
			}
			if (a3 * direction != Vector3I.Zero)
			{
				return false;
			}
			if (a.AbsMax() <= 128 && a2.AbsMax() <= 128)
			{
				return a3.AbsMax() <= 128;
			}
			return false;
		}

		private void RemoveTriangle(MyNavigationTriangle triangle, Vector3I cube)
		{
			MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = triangle.GetVertexEnumerator();
			vertexEnumerator.MoveNext();
			Vector3I PointA = Vector3I.Round(vertexEnumerator.Current * 256f);
			vertexEnumerator.MoveNext();
			Vector3I PointB = Vector3I.Round(vertexEnumerator.Current * 256f);
			vertexEnumerator.MoveNext();
			Vector3I PointB2 = Vector3I.Round(vertexEnumerator.Current * 256f);
			int edgeIndex = triangle.GetEdgeIndex(0);
			int edgeIndex2 = triangle.GetEdgeIndex(1);
			int edgeIndex3 = triangle.GetEdgeIndex(2);
			if (!m_connectionHelper.TryGetValue(new EdgeIndex(ref PointA, ref PointB2), out int value))
			{
				value = -1;
			}
			if (!m_connectionHelper.TryGetValue(new EdgeIndex(ref PointB2, ref PointB), out int value2))
			{
				value2 = -1;
			}
			if (!m_connectionHelper.TryGetValue(new EdgeIndex(ref PointB, ref PointA), out int value3))
			{
				value3 = -1;
			}
			if (value != -1 && edgeIndex3 == value)
			{
				m_connectionHelper.Remove(new EdgeIndex(ref PointA, ref PointB2));
			}
			else
			{
				m_connectionHelper.Add(new EdgeIndex(PointB2, PointA), edgeIndex3);
			}
			if (value2 != -1 && edgeIndex2 == value2)
			{
				m_connectionHelper.Remove(new EdgeIndex(ref PointB2, ref PointB));
			}
			else
			{
				m_connectionHelper.Add(new EdgeIndex(PointB, PointB2), edgeIndex2);
			}
			if (value3 != -1 && edgeIndex == value3)
			{
				m_connectionHelper.Remove(new EdgeIndex(ref PointB, ref PointA));
			}
			else
			{
				m_connectionHelper.Add(new EdgeIndex(PointA, PointB), edgeIndex);
			}
			List<int> value4 = null;
			m_smallTriangleRegistry.TryGetValue(cube, out value4);
			for (int i = 0; i < value4.Count; i++)
			{
				if (value4[i] == triangle.Index)
				{
					value4.RemoveAtFast(i);
					break;
				}
			}
			if (value4.Count == 0)
			{
				m_smallTriangleRegistry.Remove(cube);
			}
			RemoveTriangle(triangle);
			if (value != -1 && edgeIndex3 != value)
			{
				RemoveAndAddTriangle(ref PointA, ref PointB2, value);
			}
			if (value2 != -1 && edgeIndex2 != value2)
			{
				RemoveAndAddTriangle(ref PointB2, ref PointB, value2);
			}
			if (value3 != -1 && edgeIndex != value3)
			{
				RemoveAndAddTriangle(ref PointB, ref PointA, value3);
			}
		}

		private void RemoveAndAddTriangle(ref Vector3I positionA, ref Vector3I positionB, int registeredEdgeIndex)
		{
			MyNavigationTriangle edgeTriangle = GetEdgeTriangle(registeredEdgeIndex);
			MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = edgeTriangle.GetVertexEnumerator();
			vertexEnumerator.MoveNext();
			Vector3 current = vertexEnumerator.Current;
			vertexEnumerator.MoveNext();
			Vector3 current2 = vertexEnumerator.Current;
			vertexEnumerator.MoveNext();
			Vector3 current3 = vertexEnumerator.Current;
			Vector3I gridPos = FindTriangleCube(edgeTriangle.Index, ref positionA, ref positionB);
			RemoveTriangle(edgeTriangle, gridPos);
			MyNavigationTriangle tri = AddTriangleInternal(current, current3, current2);
			RegisterTriangleInternal(tri, ref gridPos);
		}

		private Vector3I FindTriangleCube(int triIndex, ref Vector3I edgePositionA, ref Vector3I edgePositionB)
		{
			Vector3I.Min(ref edgePositionA, ref edgePositionB, out Vector3I result);
			Vector3I.Max(ref edgePositionA, ref edgePositionB, out Vector3I result2);
			result = Vector3I.Round(new Vector3(result) / 256f - Vector3.Half);
			result2 = Vector3I.Round(new Vector3(result2) / 256f + Vector3.Half);
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref result, ref result2);
			while (vector3I_RangeIterator.IsValid())
			{
				m_smallTriangleRegistry.TryGetValue(result, out List<int> value);
				if (value != null && value.Contains(triIndex))
				{
					return result;
				}
				vector3I_RangeIterator.GetNext(out result);
			}
			return Vector3I.Zero;
		}

		private void CopyTriangle(MyNavigationTriangle otherTri, Vector3I triPosition, ref MatrixI transform)
		{
			otherTri.GetTransformed(ref transform, out Vector3 newA, out Vector3 newB, out Vector3 newC);
			if (MyPerGameSettings.NavmeshPresumesDownwardGravity)
			{
				Vector3 vector = Vector3.Cross(newC - newA, newB - newA);
				vector.Normalize();
				if (Vector3.Dot(vector, Base6Directions.GetVector(Base6Directions.Direction.Up)) < 0.7f)
				{
					return;
				}
			}
			Vector3I.Transform(ref triPosition, ref transform, out triPosition);
			MyNavigationTriangle tri = AddTriangleInternal(newA, newC, newB);
			RegisterTriangleInternal(tri, ref triPosition);
		}

		public void MakeStatic()
		{
			if (!m_static)
			{
				m_static = true;
				m_connectionHelper = null;
				m_cubeSet = null;
			}
		}

		public List<Vector4D> FindPath(Vector3 start, Vector3 end)
		{
			start /= m_grid.GridSize;
			end /= m_grid.GridSize;
			float closestDistSq = float.PositiveInfinity;
			MyNavigationTriangle closestNavigationTriangle = GetClosestNavigationTriangle(ref start, ref closestDistSq);
			if (closestNavigationTriangle == null)
			{
				return null;
			}
			closestDistSq = float.PositiveInfinity;
			MyNavigationTriangle closestNavigationTriangle2 = GetClosestNavigationTriangle(ref end, ref closestDistSq);
			if (closestNavigationTriangle2 == null)
			{
				return null;
			}
			List<Vector4D> list = FindRefinedPath(closestNavigationTriangle, closestNavigationTriangle2, ref start, ref end);
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Vector4D value = list[i];
					value = (list[i] = value * m_grid.GridSize);
				}
			}
			return list;
		}

		private MyNavigationTriangle GetClosestNavigationTriangle(ref Vector3 point, ref float closestDistSq)
		{
			Vector3I.Round(ref point, out Vector3I r);
			MyNavigationTriangle result = null;
			Vector3I start = r - new Vector3I(4, 4, 4);
			Vector3I end = r + new Vector3I(4, 4, 4);
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
			while (vector3I_RangeIterator.IsValid())
			{
				m_smallTriangleRegistry.TryGetValue(start, out List<int> value);
				if (value != null)
				{
					foreach (int item in value)
					{
						MyNavigationTriangle triangle = GetTriangle(item);
						MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = triangle.GetVertexEnumerator();
						vertexEnumerator.MoveNext();
						Vector3 current2 = vertexEnumerator.Current;
						vertexEnumerator.MoveNext();
						Vector3 current3 = vertexEnumerator.Current;
						vertexEnumerator.MoveNext();
						Vector3 current4 = vertexEnumerator.Current;
						Vector3 vector = (current2 + current3 + current4) / 3f;
						Vector3 vector2 = current3 - current2;
						Vector3 vector3 = current4 - current3;
						float num = Vector3.DistanceSquared(vector, point);
						if (num < vector2.LengthSquared() + vector3.LengthSquared())
						{
							Vector3 vector4 = current2 - current4;
							Vector3 vector5 = Vector3.Cross(vector2, vector3);
							vector5.Normalize();
							vector2 = Vector3.Cross(vector2, vector5);
							vector3 = Vector3.Cross(vector3, vector5);
							Vector3 vector6 = Vector3.Cross(vector4, vector5);
							float num2 = 0f - Vector3.Dot(vector2, current2);
							float num3 = 0f - Vector3.Dot(vector3, current3);
							float num4 = 0f - Vector3.Dot(vector6, current4);
							float num5 = Vector3.Dot(vector2, point) + num2;
							float num6 = Vector3.Dot(vector3, point) + num3;
							float num7 = Vector3.Dot(vector6, point) + num4;
							num = Vector3.Dot(vector5, point) - Vector3.Dot(vector5, vector);
							num *= num;
							if (num5 > 0f)
							{
								if (!(num6 > 0f))
								{
									num = ((!(num7 > 0f)) ? (num + Vector3.DistanceSquared(current4, point)) : (num + num6 * num6));
								}
								else if (num7 < 0f)
								{
									num += num7 * num7;
								}
							}
							else if (num6 > 0f)
							{
								num = ((!(num7 > 0f)) ? (num + Vector3.DistanceSquared(current2, point)) : (num + num5 * num5));
							}
							else if (num7 > 0f)
							{
								num += Vector3.DistanceSquared(current3, point);
							}
						}
						if (num < closestDistSq)
						{
							result = triangle;
							closestDistSq = num;
						}
					}
				}
				vector3I_RangeIterator.GetNext(out start);
			}
			return result;
		}

		public override MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, ref double closestDistanceSq)
		{
			if (highLevel)
			{
				return null;
			}
			Vector3 point2 = Vector3D.Transform(point, m_grid.PositionComp.WorldMatrixNormalizedInv);
			point2 /= m_grid.GridSize;
			float closestDistSq = (float)closestDistanceSq / m_grid.GridSize;
			MyNavigationTriangle closestNavigationTriangle = GetClosestNavigationTriangle(ref point2, ref closestDistSq);
			if (closestNavigationTriangle != null)
			{
				closestDistanceSq = closestDistSq * m_grid.GridSize;
			}
			return closestNavigationTriangle;
		}

		private void grid_OnBlockAdded(MySlimBlock block)
		{
			OnBlockAddedInternal(block);
		}

		private void OnBlockAddedInternal(MySlimBlock block)
		{
			MyCompoundCubeBlock myCompoundCubeBlock = m_grid.GetCubeBlock(block.Position).FatBlock as MyCompoundCubeBlock;
			if (!(block.FatBlock is MyCompoundCubeBlock) && block.BlockDefinition.NavigationDefinition == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (myCompoundCubeBlock != null)
			{
				ListReader<MySlimBlock> blocks = myCompoundCubeBlock.GetBlocks();
				if (blocks.Count == 0)
				{
					return;
				}
				foreach (MySlimBlock item in blocks)
				{
					if (item.BlockDefinition.NavigationDefinition != null)
					{
						if (item.BlockDefinition.NavigationDefinition.NoEntry || flag2)
						{
							flag2 = false;
							flag = true;
							break;
						}
						block = item;
						flag2 = true;
					}
				}
			}
			else if (block.BlockDefinition.NavigationDefinition != null)
			{
				if (block.BlockDefinition.NavigationDefinition.NoEntry)
				{
					flag2 = false;
					flag = true;
				}
				else
				{
					flag2 = true;
				}
			}
			if (!flag && !flag2)
			{
				return;
			}
			if (flag)
			{
				if (m_cubeSet.Contains(block.Position))
				{
					RemoveBlock(block.Min, block.Max, eraseCubeSet: true);
				}
				Vector3I position = default(Vector3I);
				position.X = block.Min.X;
				while (position.X <= block.Max.X)
				{
					position.Y = block.Min.Y;
					while (position.Y <= block.Max.Y)
					{
						position.Z = block.Min.Z - 1;
						if (m_cubeSet.Contains(ref position))
						{
							EraseFaceTriangles(position, Base6Directions.Direction.Backward);
						}
						position.Z = block.Max.Z + 1;
						if (m_cubeSet.Contains(ref position))
						{
							EraseFaceTriangles(position, Base6Directions.Direction.Forward);
						}
						position.Y++;
					}
					position.Z = block.Min.Z;
					while (position.Z <= block.Max.Z)
					{
						position.Y = block.Min.Y - 1;
						if (m_cubeSet.Contains(ref position))
						{
							EraseFaceTriangles(position, Base6Directions.Direction.Up);
						}
						position.Y = block.Max.Y + 1;
						if (m_cubeSet.Contains(ref position))
						{
							EraseFaceTriangles(position, Base6Directions.Direction.Down);
						}
						position.Z++;
					}
					position.X++;
				}
				position.Y = block.Min.Y;
				while (position.Y <= block.Max.Y)
				{
					position.Z = block.Min.Z;
					while (position.Z <= block.Max.Z)
					{
						position.X = block.Min.X - 1;
						if (m_cubeSet.Contains(ref position))
						{
							EraseFaceTriangles(position, Base6Directions.Direction.Right);
						}
						position.X = block.Max.X + 1;
						if (m_cubeSet.Contains(ref position))
						{
							EraseFaceTriangles(position, Base6Directions.Direction.Left);
						}
						position.Z++;
					}
					position.Y++;
				}
				position = block.Min;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref position, ref block.Max);
				while (vector3I_RangeIterator.IsValid())
				{
					m_cubeSet.Add(position);
					vector3I_RangeIterator.GetNext(out position);
				}
			}
			else
			{
				if (m_cubeSet.Contains(block.Position))
				{
					RemoveBlock(block.Min, block.Max, eraseCubeSet: true);
				}
				AddBlock(block);
			}
			block.GetWorldBoundingBox(out BoundingBoxD aabb);
			aabb.Inflate(5.0999999046325684);
			m_coordinator.InvalidateVoxelsBBox(ref aabb);
			MarkBlockChanged(block);
		}

		private void grid_OnBlockRemoved(MySlimBlock block)
		{
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			MySlimBlock cubeBlock = m_grid.GetCubeBlock(block.Position);
			MyCompoundCubeBlock myCompoundCubeBlock = (cubeBlock == null) ? null : (cubeBlock.FatBlock as MyCompoundCubeBlock);
			if (!(block.FatBlock is MyCompoundCubeBlock) && block.BlockDefinition.NavigationDefinition == null)
			{
				return;
			}
			if (myCompoundCubeBlock == null)
			{
				flag = false;
				if (cubeBlock != null)
				{
					if (block.BlockDefinition.NavigationDefinition.NoEntry)
					{
						flag2 = true;
					}
					else
					{
						flag3 = true;
					}
				}
			}
			else
			{
				ListReader<MySlimBlock> blocks = myCompoundCubeBlock.GetBlocks();
				if (blocks.Count != 0)
				{
					foreach (MySlimBlock item in blocks)
					{
						if (item.BlockDefinition.NavigationDefinition != null)
						{
							if (item.BlockDefinition.NavigationDefinition.NoEntry || flag3)
							{
								flag = false;
								flag2 = true;
								break;
							}
							flag = false;
							flag3 = true;
							block = item;
						}
					}
				}
			}
			block.GetWorldBoundingBox(out BoundingBoxD aabb);
			aabb.Inflate(5.0999999046325684);
			m_coordinator.InvalidateVoxelsBBox(ref aabb);
			MarkBlockChanged(block);
			MyCestmirPathfindingShorts.Pathfinding.GridPathfinding.MarkHighLevelDirty();
			if (flag)
			{
				RemoveBlock(block.Min, block.Max, eraseCubeSet: true);
				FixBlockFaces(block);
			}
			else if (flag2)
			{
				RemoveBlock(block.Min, block.Max, eraseCubeSet: false);
			}
			else if (flag3)
			{
				RemoveBlock(block.Min, block.Max, eraseCubeSet: true);
				AddBlock(block);
			}
			else if (m_cubeSet.Contains(block.Position))
			{
				RemoveBlock(block.Min, block.Max, eraseCubeSet: true);
				FixBlockFaces(block);
			}
		}

		private void MarkBlockChanged(MySlimBlock block)
		{
			m_higherLevelHelper.MarkBlockChanged(block);
			MyCestmirPathfindingShorts.Pathfinding.GridPathfinding.MarkHighLevelDirty();
		}

		private void AddBlock(MySlimBlock block)
		{
			Vector3I start = block.Min;
			Vector3I end = block.Max;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
			while (vector3I_RangeIterator.IsValid())
			{
				m_cubeSet.Add(ref start);
				vector3I_RangeIterator.GetNext(out start);
			}
			MatrixI transform = new MatrixI(block.Position, block.Orientation.Forward, block.Orientation.Up);
			MergeFromAnotherMesh(block.BlockDefinition.NavigationDefinition.Mesh, ref transform);
		}

		private void RemoveBlock(Vector3I min, Vector3I max, bool eraseCubeSet)
		{
			Vector3I start = min;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref max);
			while (vector3I_RangeIterator.IsValid())
			{
				if (eraseCubeSet)
				{
					m_cubeSet.Remove(ref start);
				}
				EraseCubeTriangles(start);
				vector3I_RangeIterator.GetNext(out start);
			}
		}

		private void EraseCubeTriangles(Vector3I pos)
		{
			if (m_smallTriangleRegistry.TryGetValue(pos, out List<int> value))
			{
				m_tmpTriangleList.Clear();
				foreach (int item in value)
				{
					MyNavigationTriangle triangle = GetTriangle(item);
					m_tmpTriangleList.Add(new KeyValuePair<MyNavigationTriangle, Vector3I>(triangle, pos));
				}
				foreach (KeyValuePair<MyNavigationTriangle, Vector3I> tmpTriangle in m_tmpTriangleList)
				{
					RemoveTriangle(tmpTriangle.Key, tmpTriangle.Value);
				}
				m_tmpTriangleList.Clear();
				m_smallTriangleRegistry.Remove(pos);
			}
		}

		private void EraseFaceTriangles(Vector3I pos, Base6Directions.Direction direction)
		{
			m_tmpTriangleList.Clear();
			Vector3I intVector = Base6Directions.GetIntVector((int)direction);
			List<int> value = null;
			if (m_smallTriangleRegistry.TryGetValue(pos, out value))
			{
				foreach (int item in value)
				{
					MyNavigationTriangle triangle = GetTriangle(item);
					if (IsFaceTriangle(triangle, pos, intVector))
					{
						m_tmpTriangleList.Add(new KeyValuePair<MyNavigationTriangle, Vector3I>(triangle, pos));
					}
				}
			}
			foreach (KeyValuePair<MyNavigationTriangle, Vector3I> tmpTriangle in m_tmpTriangleList)
			{
				RemoveTriangle(tmpTriangle.Key, tmpTriangle.Value);
			}
			m_tmpTriangleList.Clear();
		}

		private void FixBlockFaces(MySlimBlock block)
		{
			Vector3I pos = default(Vector3I);
			pos.X = block.Min.X;
			while (pos.X <= block.Max.X)
			{
				pos.Y = block.Min.Y;
				while (pos.Y <= block.Max.Y)
				{
					Vector3I dir = Vector3I.Backward;
					pos.Z = block.Min.Z - 1;
					FixCubeFace(ref pos, ref dir);
					dir = Vector3I.Forward;
					pos.Z = block.Max.Z + 1;
					FixCubeFace(ref pos, ref dir);
					pos.Y++;
				}
				pos.X++;
			}
			pos.X = block.Min.X;
			while (pos.X <= block.Max.X)
			{
				pos.Z = block.Min.Z;
				while (pos.Z <= block.Max.Z)
				{
					Vector3I dir = Vector3I.Up;
					pos.Y = block.Min.Y - 1;
					FixCubeFace(ref pos, ref dir);
					dir = Vector3I.Down;
					pos.Y = block.Max.Y + 1;
					FixCubeFace(ref pos, ref dir);
					pos.Z++;
				}
				pos.X++;
			}
			pos.Y = block.Min.Y;
			while (pos.Y <= block.Max.Y)
			{
				pos.Z = block.Min.Z;
				while (pos.Z <= block.Max.Z)
				{
					Vector3I dir = Vector3I.Right;
					pos.X = block.Min.X - 1;
					FixCubeFace(ref pos, ref dir);
					dir = Vector3I.Left;
					pos.X = block.Max.X + 1;
					FixCubeFace(ref pos, ref dir);
					pos.Z++;
				}
				pos.Y++;
			}
		}

		private void FixCubeFace(ref Vector3I pos, ref Vector3I dir)
		{
			if (!m_cubeSet.Contains(ref pos))
			{
				return;
			}
			MySlimBlock mySlimBlock = m_grid.GetCubeBlock(pos);
			MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
			if (myCompoundCubeBlock != null)
			{
				ListReader<MySlimBlock> blocks = myCompoundCubeBlock.GetBlocks();
				MySlimBlock mySlimBlock2 = null;
				foreach (MySlimBlock item in blocks)
				{
					if (item.BlockDefinition.NavigationDefinition != null)
					{
						mySlimBlock2 = item;
						break;
					}
				}
				if (mySlimBlock2 != null)
				{
					mySlimBlock = mySlimBlock2;
				}
			}
			if (mySlimBlock.BlockDefinition.NavigationDefinition != null)
			{
				MatrixI matrix = new MatrixI(mySlimBlock.Position, mySlimBlock.Orientation.Forward, mySlimBlock.Orientation.Up);
				MatrixI.Invert(ref matrix, out MatrixI result);
				Vector3I.Transform(ref pos, ref result, out Vector3I result2);
				Vector3I.TransformNormal(ref dir, ref result, out Vector3I result3);
				MyGridNavigationMesh mesh = mySlimBlock.BlockDefinition.NavigationDefinition.Mesh;
				if (mesh != null && mesh.m_smallTriangleRegistry.TryGetValue(result2, out List<int> value))
				{
					foreach (int item2 in value)
					{
						MyNavigationTriangle triangle = mesh.GetTriangle(item2);
						if (IsFaceTriangle(triangle, result2, result3))
						{
							CopyTriangle(triangle, result2, ref matrix);
						}
					}
				}
			}
		}

		public override MatrixD GetWorldMatrix()
		{
			MatrixD matrix = m_grid.WorldMatrix;
			MatrixD.Rescale(ref matrix, m_grid.GridSize);
			return matrix;
		}

		public override Vector3 GlobalToLocal(Vector3D globalPos)
		{
			return (Vector3)Vector3D.Transform(globalPos, m_grid.PositionComp.WorldMatrixNormalizedInv) / m_grid.GridSize;
		}

		public override Vector3D LocalToGlobal(Vector3 localPos)
		{
			localPos *= m_grid.GridSize;
			return Vector3D.Transform(localPos, m_grid.WorldMatrix);
		}

		public override MyHighLevelPrimitive GetHighLevelPrimitive(MyNavigationPrimitive myNavigationTriangle)
		{
			return m_higherLevelHelper.GetHighLevelNavigationPrimitive(myNavigationTriangle as MyNavigationTriangle);
		}

		public override IMyHighLevelComponent GetComponent(MyHighLevelPrimitive highLevelPrimitive)
		{
			return new Component(this, highLevelPrimitive.Index);
		}

		public override void DebugDraw(ref Matrix drawMatrix)
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				if ((MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.EDGES) != 0 && m_connectionHelper != null)
				{
					foreach (KeyValuePair<EdgeIndex, int> item in m_connectionHelper)
					{
						Vector3 v = Vector3.Transform(item.Key.A / 256f, drawMatrix);
						MyRenderProxy.DebugDrawLine3D(pointTo: Vector3.Transform(item.Key.B / 256f, drawMatrix), pointFrom: v, colorFrom: Color.Red, colorTo: Color.Yellow, depthRead: false);
					}
				}
				if ((MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.NORMALS) != 0)
				{
					foreach (KeyValuePair<Vector3I, List<int>> item2 in m_smallTriangleRegistry)
					{
						foreach (int item3 in item2.Value)
						{
							MyNavigationTriangle triangle = GetTriangle(item3);
							Vector3 v2 = Vector3.Transform(triangle.Center + triangle.Normal * 0.2f, drawMatrix);
							MyRenderProxy.DebugDrawLine3D(Vector3.Transform(triangle.Center, drawMatrix), v2, Color.Blue, Color.Blue, depthRead: true);
						}
					}
				}
				if (MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY && m_higherLevel != null)
				{
					m_higherLevel.DebugDraw(MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY_LITE);
				}
			}
		}
	}
}
