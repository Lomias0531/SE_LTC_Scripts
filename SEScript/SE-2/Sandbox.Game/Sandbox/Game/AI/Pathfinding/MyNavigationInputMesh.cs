using Havok;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Groups;
using VRage.Library.Collections;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	internal class MyNavigationInputMesh
	{
		public class CubeInfo
		{
			public int ID
			{
				get;
				set;
			}

			public BoundingBoxD BoundingBox
			{
				get;
				set;
			}

			public List<Vector3D> TriangleVertices
			{
				get;
				set;
			}
		}

		public struct GridInfo
		{
			public long ID
			{
				get;
				set;
			}

			public List<CubeInfo> Cubes
			{
				get;
				set;
			}
		}

		public class WorldVerticesInfo
		{
			public MyList<Vector3> Vertices = new MyList<Vector3>();

			public int VerticesMaxValue;

			public MyList<int> Triangles = new MyList<int>();
		}

		public struct CacheInterval
		{
			public Vector3I Min;

			public Vector3I Max;
		}

		public class IcoSphereMesh
		{
			private struct TriangleIndices
			{
				public int v1;

				public int v2;

				public int v3;

				public TriangleIndices(int v1, int v2, int v3)
				{
					this.v1 = v1;
					this.v2 = v2;
					this.v3 = v3;
				}
			}

			private const int RECURSION_LEVEL = 1;

			private int index;

			private Dictionary<long, int> middlePointIndexCache;

			private List<int> triangleIndices;

			private List<Vector3> positions;

			public IcoSphereMesh()
			{
				create();
			}

			private int addVertex(Vector3 p)
			{
				double num = Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
				positions.Add(new Vector3((double)p.X / num, (double)p.Y / num, (double)p.Z / num));
				return index++;
			}

			private int getMiddlePoint(int p1, int p2)
			{
				bool num = p1 < p2;
				long num2 = num ? p1 : p2;
				long num3 = num ? p2 : p1;
				long key = (num2 << 32) + num3;
				if (middlePointIndexCache.TryGetValue(key, out int value))
				{
					return value;
				}
				Vector3 vector = positions[p1];
				Vector3 vector2 = positions[p2];
				Vector3 p3 = new Vector3((double)(vector.X + vector2.X) / 2.0, (double)(vector.Y + vector2.Y) / 2.0, (double)(vector.Z + vector2.Z) / 2.0);
				int num4 = addVertex(p3);
				middlePointIndexCache.Add(key, num4);
				return num4;
			}

			private void create()
			{
				middlePointIndexCache = new Dictionary<long, int>();
				triangleIndices = new List<int>();
				positions = new List<Vector3>();
				index = 0;
				double num = (1.0 + Math.Sqrt(5.0)) / 2.0;
				addVertex(new Vector3(-1.0, num, 0.0));
				addVertex(new Vector3(1.0, num, 0.0));
				addVertex(new Vector3(-1.0, 0.0 - num, 0.0));
				addVertex(new Vector3(1.0, 0.0 - num, 0.0));
				addVertex(new Vector3(0.0, -1.0, num));
				addVertex(new Vector3(0.0, 1.0, num));
				addVertex(new Vector3(0.0, -1.0, 0.0 - num));
				addVertex(new Vector3(0.0, 1.0, 0.0 - num));
				addVertex(new Vector3(num, 0.0, -1.0));
				addVertex(new Vector3(num, 0.0, 1.0));
				addVertex(new Vector3(0.0 - num, 0.0, -1.0));
				addVertex(new Vector3(0.0 - num, 0.0, 1.0));
				List<TriangleIndices> list = new List<TriangleIndices>();
				list.Add(new TriangleIndices(0, 11, 5));
				list.Add(new TriangleIndices(0, 5, 1));
				list.Add(new TriangleIndices(0, 1, 7));
				list.Add(new TriangleIndices(0, 7, 10));
				list.Add(new TriangleIndices(0, 10, 11));
				list.Add(new TriangleIndices(1, 5, 9));
				list.Add(new TriangleIndices(5, 11, 4));
				list.Add(new TriangleIndices(11, 10, 2));
				list.Add(new TriangleIndices(10, 7, 6));
				list.Add(new TriangleIndices(7, 1, 8));
				list.Add(new TriangleIndices(3, 9, 4));
				list.Add(new TriangleIndices(3, 4, 2));
				list.Add(new TriangleIndices(3, 2, 6));
				list.Add(new TriangleIndices(3, 6, 8));
				list.Add(new TriangleIndices(3, 8, 9));
				list.Add(new TriangleIndices(4, 9, 5));
				list.Add(new TriangleIndices(2, 4, 11));
				list.Add(new TriangleIndices(6, 2, 10));
				list.Add(new TriangleIndices(8, 6, 7));
				list.Add(new TriangleIndices(9, 8, 1));
				for (int i = 0; i < 1; i++)
				{
					List<TriangleIndices> list2 = new List<TriangleIndices>();
					foreach (TriangleIndices item in list)
					{
						int middlePoint = getMiddlePoint(item.v1, item.v2);
						int middlePoint2 = getMiddlePoint(item.v2, item.v3);
						int middlePoint3 = getMiddlePoint(item.v3, item.v1);
						list2.Add(new TriangleIndices(item.v1, middlePoint, middlePoint3));
						list2.Add(new TriangleIndices(item.v2, middlePoint2, middlePoint));
						list2.Add(new TriangleIndices(item.v3, middlePoint3, middlePoint2));
						list2.Add(new TriangleIndices(middlePoint, middlePoint2, middlePoint3));
					}
					list = list2;
				}
				foreach (TriangleIndices item2 in list)
				{
					triangleIndices.Add(item2.v1);
					triangleIndices.Add(item2.v2);
					triangleIndices.Add(item2.v3);
				}
			}

			public void AddTrianglesToWorldVertices(Vector3 center, float radius)
			{
				foreach (int triangleIndex in triangleIndices)
				{
					m_worldVertices.Triangles.Add(m_worldVertices.VerticesMaxValue + triangleIndex);
				}
				foreach (Vector3 position in positions)
				{
					m_worldVertices.Vertices.Add(center + position * radius);
				}
				m_worldVertices.VerticesMaxValue += positions.Count;
			}
		}

		public class CapsuleMesh
		{
			private const double PId2 = Math.PI / 2.0;

			private const double PIm2 = Math.PI * 2.0;

			private List<Vector3> m_verticeList = new List<Vector3>();

			private List<int> m_triangleList = new List<int>();

			private int N = 8;

			private float radius = 1f;

			private float height;

			public CapsuleMesh()
			{
				Create();
			}

			private void Create()
			{
				for (int i = 0; i <= N / 4; i++)
				{
					for (int j = 0; j <= N; j++)
					{
						Vector3 item = default(Vector3);
						double num = (double)j * (Math.PI * 2.0) / (double)N;
						double num2 = -Math.PI / 2.0 + Math.PI * (double)i / (double)(N / 2);
						item.X = radius * (float)(Math.Cos(num2) * Math.Cos(num));
						item.Y = radius * (float)(Math.Cos(num2) * Math.Sin(num));
						item.Z = radius * (float)Math.Sin(num2) - height / 2f;
						m_verticeList.Add(item);
					}
				}
				for (int i = N / 4; i <= N / 2; i++)
				{
					for (int j = 0; j <= N; j++)
					{
						Vector3 item2 = default(Vector3);
						double num = (double)j * (Math.PI * 2.0) / (double)N;
						double num2 = -Math.PI / 2.0 + Math.PI * (double)i / (double)(N / 2);
						item2.X = radius * (float)(Math.Cos(num2) * Math.Cos(num));
						item2.Y = radius * (float)(Math.Cos(num2) * Math.Sin(num));
						item2.Z = radius * (float)Math.Sin(num2) + height / 2f;
						m_verticeList.Add(item2);
					}
				}
				for (int i = 0; i <= N / 2; i++)
				{
					for (int j = 0; j < N; j++)
					{
						int item3 = i * (N + 1) + j;
						int item4 = i * (N + 1) + (j + 1);
						int item5 = (i + 1) * (N + 1) + (j + 1);
						int item6 = (i + 1) * (N + 1) + j;
						m_triangleList.Add(item3);
						m_triangleList.Add(item4);
						m_triangleList.Add(item5);
						m_triangleList.Add(item3);
						m_triangleList.Add(item5);
						m_triangleList.Add(item6);
					}
				}
			}

			public void AddTrianglesToWorldVertices(Matrix transformMatrix, float radius, Line axisLine)
			{
				Matrix matrix = Matrix.CreateFromDir(axisLine.Direction);
				Vector3 translation = transformMatrix.Translation;
				transformMatrix.Translation = Vector3.Zero;
				int num = m_verticeList.Count / 2;
				Vector3 value = new Vector3(0f, 0f, axisLine.Length * 0.5f);
				for (int i = 0; i < num; i++)
				{
					m_worldVertices.Vertices.Add(Vector3.Transform(translation + m_verticeList[i] * radius - value, matrix));
				}
				for (int j = num; j < m_verticeList.Count; j++)
				{
					m_worldVertices.Vertices.Add(Vector3.Transform(translation + m_verticeList[j] * radius + value, matrix));
				}
				foreach (int triangle in m_triangleList)
				{
					m_worldVertices.Triangles.Add(m_worldVertices.VerticesMaxValue + triangle);
				}
				m_worldVertices.VerticesMaxValue += m_verticeList.Count;
			}
		}

		private static IcoSphereMesh m_icosphereMesh = new IcoSphereMesh();

		private static CapsuleMesh m_capsuleMesh = new CapsuleMesh();

		[ThreadStatic]
		private static WorldVerticesInfo m_worldVerticesInfoPerThread;

		private static Dictionary<string, BoundingBoxD> m_cachedBoxes = new Dictionary<string, BoundingBoxD>();

		[ThreadStatic]
		private static List<HkShape> m_tmpShapes;

		private const int NAVMESH_LOD = 0;

		private Dictionary<Vector3I, MyIsoMesh> m_meshCache = new Dictionary<Vector3I, MyIsoMesh>(1024, new Vector3I.EqualityComparer());

		private List<CacheInterval> m_invalidateMeshCacheCoord = new List<CacheInterval>();

		private List<CacheInterval> m_tmpInvalidCache = new List<CacheInterval>();

		private MyPlanet m_planet;

		private Vector3D m_center;

		private Quaternion rdWorldQuaternion;

		private MyRDPathfinding m_rdPathfinding;

		private List<GridInfo> m_lastGridsInfo = new List<GridInfo>();

		private List<CubeInfo> m_lastIntersectedGridsInfoCubes = new List<CubeInfo>();

		private static WorldVerticesInfo m_worldVertices
		{
			get
			{
				if (m_worldVerticesInfoPerThread == null)
				{
					m_worldVerticesInfoPerThread = new WorldVerticesInfo();
				}
				return m_worldVerticesInfoPerThread;
			}
		}

		public MyNavigationInputMesh(MyRDPathfinding rdPathfinding, MyPlanet planet, Vector3D center)
		{
			m_rdPathfinding = rdPathfinding;
			m_planet = planet;
			m_center = center;
			Vector3 vector = -(Vector3)Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(m_center));
			Vector3 forward = Vector3.CalculatePerpendicularVector(vector);
			rdWorldQuaternion = Quaternion.Inverse(Quaternion.CreateFromForwardUp(forward, vector));
		}

		public WorldVerticesInfo GetWorldVertices(float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> boundingBoxes, List<MyVoxelMap> trackedEntities)
		{
			ClearWorldVertices();
			AddEntities(border, originPosition, obb, boundingBoxes, trackedEntities);
			AddGround(border, originPosition, obb, boundingBoxes);
			return m_worldVertices;
		}

		public void DebugDraw()
		{
			foreach (GridInfo item in m_lastGridsInfo)
			{
				foreach (CubeInfo cube in item.Cubes)
				{
					if (m_lastIntersectedGridsInfoCubes.Contains(cube))
					{
						MyRenderProxy.DebugDrawAABB(cube.BoundingBox, Color.White);
					}
					else
					{
						MyRenderProxy.DebugDrawAABB(cube.BoundingBox, Color.Yellow);
					}
				}
			}
		}

		public void InvalidateCache(BoundingBoxD box)
		{
			Vector3D xyz = Vector3D.Transform(box.Min, m_planet.PositionComp.WorldMatrixInvScaled);
			Vector3D xyz2 = Vector3D.Transform(box.Max, m_planet.PositionComp.WorldMatrixInvScaled);
			xyz += m_planet.SizeInMetresHalf;
			xyz2 += m_planet.SizeInMetresHalf;
			Vector3I voxelCoord = new Vector3I(xyz);
			Vector3I voxelCoord2 = new Vector3I(xyz2);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord, out Vector3I geometryCellCoord);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord2, out Vector3I geometryCellCoord2);
			m_invalidateMeshCacheCoord.Add(new CacheInterval
			{
				Min = geometryCellCoord,
				Max = geometryCellCoord2
			});
		}

		public void RefreshCache()
		{
			m_meshCache.Clear();
		}

		public void Clear()
		{
			m_meshCache.Clear();
		}

		private void ClearWorldVertices()
		{
			m_worldVertices.Vertices.Clear();
			m_worldVertices.VerticesMaxValue = 0;
			m_worldVertices.Triangles.Clear();
		}

		private void BoundingBoxToTranslatedTriangles(BoundingBoxD bbox, Matrix worldMatrix)
		{
			Vector3 position = new Vector3(bbox.Min.X, bbox.Max.Y, bbox.Max.Z);
			Vector3 position2 = new Vector3(bbox.Max.X, bbox.Max.Y, bbox.Max.Z);
			Vector3 position3 = new Vector3(bbox.Min.X, bbox.Max.Y, bbox.Min.Z);
			Vector3 position4 = new Vector3(bbox.Max.X, bbox.Max.Y, bbox.Min.Z);
			Vector3 position5 = new Vector3(bbox.Min.X, bbox.Min.Y, bbox.Max.Z);
			Vector3 position6 = new Vector3(bbox.Max.X, bbox.Min.Y, bbox.Max.Z);
			Vector3 position7 = new Vector3(bbox.Min.X, bbox.Min.Y, bbox.Min.Z);
			Vector3 position8 = new Vector3(bbox.Max.X, bbox.Min.Y, bbox.Min.Z);
			Vector3.Transform(ref position, ref worldMatrix, out position);
			Vector3.Transform(ref position2, ref worldMatrix, out position2);
			Vector3.Transform(ref position3, ref worldMatrix, out position3);
			Vector3.Transform(ref position4, ref worldMatrix, out position4);
			Vector3.Transform(ref position5, ref worldMatrix, out position5);
			Vector3.Transform(ref position6, ref worldMatrix, out position6);
			Vector3.Transform(ref position7, ref worldMatrix, out position7);
			Vector3.Transform(ref position8, ref worldMatrix, out position8);
			m_worldVertices.Vertices.Add(position);
			m_worldVertices.Vertices.Add(position2);
			m_worldVertices.Vertices.Add(position3);
			m_worldVertices.Vertices.Add(position4);
			m_worldVertices.Vertices.Add(position5);
			m_worldVertices.Vertices.Add(position6);
			m_worldVertices.Vertices.Add(position7);
			m_worldVertices.Vertices.Add(position8);
			int verticesMaxValue = m_worldVertices.VerticesMaxValue;
			int item = m_worldVertices.VerticesMaxValue + 1;
			int item2 = m_worldVertices.VerticesMaxValue + 2;
			int item3 = m_worldVertices.VerticesMaxValue + 3;
			int item4 = m_worldVertices.VerticesMaxValue + 4;
			int item5 = m_worldVertices.VerticesMaxValue + 5;
			int item6 = m_worldVertices.VerticesMaxValue + 6;
			int item7 = m_worldVertices.VerticesMaxValue + 7;
			m_worldVertices.Triangles.Add(item3);
			m_worldVertices.Triangles.Add(item2);
			m_worldVertices.Triangles.Add(verticesMaxValue);
			m_worldVertices.Triangles.Add(verticesMaxValue);
			m_worldVertices.Triangles.Add(item);
			m_worldVertices.Triangles.Add(item3);
			m_worldVertices.Triangles.Add(item4);
			m_worldVertices.Triangles.Add(item6);
			m_worldVertices.Triangles.Add(item7);
			m_worldVertices.Triangles.Add(item7);
			m_worldVertices.Triangles.Add(item5);
			m_worldVertices.Triangles.Add(item4);
			m_worldVertices.Triangles.Add(item2);
			m_worldVertices.Triangles.Add(item7);
			m_worldVertices.Triangles.Add(item6);
			m_worldVertices.Triangles.Add(item2);
			m_worldVertices.Triangles.Add(item3);
			m_worldVertices.Triangles.Add(item7);
			m_worldVertices.Triangles.Add(verticesMaxValue);
			m_worldVertices.Triangles.Add(item4);
			m_worldVertices.Triangles.Add(item5);
			m_worldVertices.Triangles.Add(item5);
			m_worldVertices.Triangles.Add(item);
			m_worldVertices.Triangles.Add(verticesMaxValue);
			m_worldVertices.Triangles.Add(item6);
			m_worldVertices.Triangles.Add(item4);
			m_worldVertices.Triangles.Add(verticesMaxValue);
			m_worldVertices.Triangles.Add(verticesMaxValue);
			m_worldVertices.Triangles.Add(item2);
			m_worldVertices.Triangles.Add(item6);
			m_worldVertices.Triangles.Add(item);
			m_worldVertices.Triangles.Add(item5);
			m_worldVertices.Triangles.Add(item7);
			m_worldVertices.Triangles.Add(item7);
			m_worldVertices.Triangles.Add(item3);
			m_worldVertices.Triangles.Add(item);
			m_worldVertices.VerticesMaxValue += 8;
		}

		private void AddPhysicalShape(HkShape shape, Matrix rdWorldMatrix)
		{
			switch (shape.ShapeType)
			{
			case HkShapeType.Cylinder:
			case HkShapeType.Triangle:
			case HkShapeType.TriSampledHeightFieldCollection:
			case HkShapeType.TriSampledHeightFieldBvTree:
				break;
			case HkShapeType.Capsule:
				break;
			case HkShapeType.Box:
			{
				HkBoxShape hkBoxShape = (HkBoxShape)shape;
				Vector3D min = new Vector3D(0f - hkBoxShape.HalfExtents.X, 0f - hkBoxShape.HalfExtents.Y, 0f - hkBoxShape.HalfExtents.Z);
				Vector3D max = new Vector3D(hkBoxShape.HalfExtents.X, hkBoxShape.HalfExtents.Y, hkBoxShape.HalfExtents.Z);
				BoundingBoxD bbox = new BoundingBoxD(min, max);
				BoundingBoxToTranslatedTriangles(bbox, rdWorldMatrix);
				break;
			}
			case HkShapeType.List:
			{
				HkShapeContainerIterator iterator = ((HkListShape)shape).GetIterator();
				while (iterator.IsValid)
				{
					AddPhysicalShape(iterator.CurrentValue, rdWorldMatrix);
					iterator.Next();
				}
				break;
			}
			case HkShapeType.Mopp:
				AddPhysicalShape(((HkMoppBvTreeShape)shape).ShapeCollection, rdWorldMatrix);
				break;
			case HkShapeType.ConvexTransform:
			{
				HkConvexTransformShape hkConvexTransformShape = (HkConvexTransformShape)shape;
				AddPhysicalShape(hkConvexTransformShape.ChildShape, hkConvexTransformShape.Transform * rdWorldMatrix);
				break;
			}
			case HkShapeType.ConvexTranslate:
			{
				HkConvexTranslateShape hkConvexTranslateShape = (HkConvexTranslateShape)shape;
				Matrix matrix = Matrix.CreateTranslation(hkConvexTranslateShape.Translation);
				AddPhysicalShape(hkConvexTranslateShape.ChildShape, matrix * rdWorldMatrix);
				break;
			}
			case HkShapeType.Sphere:
			{
				HkSphereShape hkSphereShape = (HkSphereShape)shape;
				m_icosphereMesh.AddTrianglesToWorldVertices(rdWorldMatrix.Translation, hkSphereShape.Radius);
				break;
			}
			case HkShapeType.ConvexVertices:
			{
				HkConvexVerticesShape hkConvexVerticesShape = (HkConvexVerticesShape)shape;
				HkGeometry hkGeometry = new HkGeometry();
				hkConvexVerticesShape.GetGeometry(hkGeometry, out Vector3 _);
				for (int i = 0; i < hkGeometry.TriangleCount; i++)
				{
					hkGeometry.GetTriangle(i, out int i2, out int i3, out int i4, out int _);
					m_worldVertices.Triangles.Add(m_worldVertices.VerticesMaxValue + i2);
					m_worldVertices.Triangles.Add(m_worldVertices.VerticesMaxValue + i3);
					m_worldVertices.Triangles.Add(m_worldVertices.VerticesMaxValue + i4);
				}
				for (int j = 0; j < hkGeometry.VertexCount; j++)
				{
					Vector3 position = hkGeometry.GetVertex(j);
					Vector3.Transform(ref position, ref rdWorldMatrix, out position);
					m_worldVertices.Vertices.Add(position);
				}
				m_worldVertices.VerticesMaxValue += hkGeometry.VertexCount;
				break;
			}
			}
		}

		private void AddEntities(float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> boundingBoxes, List<MyVoxelMap> trackedEntities)
		{
			obb.HalfExtent += new Vector3D(border, 0.0, border);
			BoundingBoxD box = obb.GetAABB();
			List<MyEntity> list = new List<MyEntity>();
			MyGamePruningStructure.GetAllEntitiesInBox(ref box, list);
			if (list.Count((MyEntity e) => e is MyCubeGrid) > 0)
			{
				m_lastGridsInfo.Clear();
				m_lastIntersectedGridsInfoCubes.Clear();
			}
			foreach (MyEntity item in list)
			{
				using (item.Pin())
				{
					if (!item.MarkedForClose)
					{
						_ = (item as MyCubeGrid)?.IsStatic;
						MyVoxelMap myVoxelMap = item as MyVoxelMap;
						if (myVoxelMap != null)
						{
							trackedEntities.Add(myVoxelMap);
							AddVoxelVertices(myVoxelMap, border, originPosition, obb, boundingBoxes);
						}
					}
				}
			}
		}

		private void AddGridVerticesInsideOBB(MyCubeGrid grid, MyOrientedBoundingBoxD obb)
		{
			BoundingBoxD aABB = obb.GetAABB();
			foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(grid).Nodes)
			{
				MyCubeGrid nodeData = node.NodeData;
				m_rdPathfinding.AddToTrackedGrids(nodeData);
				MatrixD worldMatrix = nodeData.WorldMatrix;
				worldMatrix.Translation -= m_center;
				MatrixD m = MatrixD.Transform(worldMatrix, rdWorldQuaternion);
				if (MyPerGameSettings.Game == GameEnum.SE_GAME)
				{
					BoundingBoxD boundingBoxD = aABB.TransformFast(nodeData.PositionComp.WorldMatrixNormalizedInv);
					Vector3I value = new Vector3I((int)Math.Round(boundingBoxD.Min.X), (int)Math.Round(boundingBoxD.Min.Y), (int)Math.Round(boundingBoxD.Min.Z));
					Vector3I value2 = new Vector3I((int)Math.Round(boundingBoxD.Max.X), (int)Math.Round(boundingBoxD.Max.Y), (int)Math.Round(boundingBoxD.Max.Z));
					value = Vector3I.Min(value, value2);
					value2 = Vector3I.Max(value, value2);
					if (nodeData.Physics != null)
					{
						using (MyUtils.ReuseCollection(ref m_tmpShapes))
						{
							MyGridShape shape = nodeData.Physics.Shape;
							using (MyGridShape.NativeShapeLock.AcquireSharedUsing())
							{
								shape.GetShapesInInterval(value, value2, m_tmpShapes);
								foreach (HkShape tmpShape in m_tmpShapes)
								{
									AddPhysicalShape(tmpShape, m);
								}
							}
						}
					}
				}
			}
		}

		private void AddVoxelVertices(MyVoxelMap voxelMap, float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> bbList)
		{
			AddVoxelMesh(voxelMap, voxelMap.Storage, null, border, originPosition, obb, bbList);
		}

		private void AddMeshTriangles(MyIsoMesh mesh, Vector3 offset, Matrix rotation, Matrix ownRotation)
		{
			for (int i = 0; i < mesh.TrianglesCount; i++)
			{
				ushort v = mesh.Triangles[i].V0;
				ushort v2 = mesh.Triangles[i].V1;
				ushort v3 = mesh.Triangles[i].V2;
				m_worldVertices.Triangles.Add(m_worldVertices.VerticesMaxValue + v3);
				m_worldVertices.Triangles.Add(m_worldVertices.VerticesMaxValue + v2);
				m_worldVertices.Triangles.Add(m_worldVertices.VerticesMaxValue + v);
			}
			for (int j = 0; j < mesh.VerticesCount; j++)
			{
				mesh.GetUnpackedPosition(j, out Vector3 position);
				Vector3.Transform(ref position, ref ownRotation, out position);
				position -= offset;
				Vector3.Transform(ref position, ref rotation, out position);
				m_worldVertices.Vertices.Add(position);
			}
			m_worldVertices.VerticesMaxValue += mesh.VerticesCount;
		}

		private unsafe Vector3* GetMiddleOBBLocalPoints(MyOrientedBoundingBoxD obb, ref Vector3* points)
		{
			Vector3 value = obb.Orientation.Right * (float)obb.HalfExtent.X;
			Vector3 value2 = obb.Orientation.Forward * (float)obb.HalfExtent.Z;
			Vector3 value3 = obb.Center - m_planet.PositionComp.GetPosition();
			*points = value3 - value - value2;
			points[1] = value3 + value - value2;
			points[2] = value3 + value + value2;
			points[3] = value3 - value + value2;
			return points;
		}

		private unsafe bool SetTerrainLimits(ref MyOrientedBoundingBoxD obb)
		{
			int pointCount = 4;
			Vector3* points = stackalloc Vector3[4];
			GetMiddleOBBLocalPoints(obb, ref points);
			m_planet.Provider.Shape.GetBounds(points, pointCount, out float minHeight, out float maxHeight);
			if (minHeight.IsValid() && maxHeight.IsValid())
			{
				Vector3D value = obb.Orientation.Up * minHeight + m_planet.PositionComp.GetPosition();
				Vector3D value2 = obb.Orientation.Up * maxHeight + m_planet.PositionComp.GetPosition();
				obb.Center = (value + value2) * 0.5;
				float num = Math.Max(maxHeight - minHeight, 1f);
				obb.HalfExtent.Y = num * 0.5f;
				return true;
			}
			return false;
		}

		private void AddGround(float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> bbList)
		{
			if (SetTerrainLimits(ref obb))
			{
				AddVoxelMesh(m_planet, m_planet.Storage, m_meshCache, border, originPosition, obb, bbList);
			}
		}

		private void CheckCacheValidity()
		{
			if (m_invalidateMeshCacheCoord.Count > 0)
			{
				m_tmpInvalidCache.AddRange(m_invalidateMeshCacheCoord);
				m_invalidateMeshCacheCoord.Clear();
				foreach (CacheInterval item in m_tmpInvalidCache)
				{
					for (int i = 0; i < m_meshCache.Count; i++)
					{
						Vector3I key = m_meshCache.ElementAt(i).Key;
						if (key.X >= item.Min.X && key.Y >= item.Min.Y && key.Z >= item.Min.Z && key.X <= item.Max.X && key.Y <= item.Max.Y && key.Z <= item.Max.Z)
						{
							m_meshCache.Remove(key);
							break;
						}
					}
				}
				m_tmpInvalidCache.Clear();
			}
		}

		private void AddVoxelMesh(MyVoxelBase voxelBase, IMyStorage storage, Dictionary<Vector3I, MyIsoMesh> cache, float border, Vector3D originPosition, MyOrientedBoundingBoxD obb, List<BoundingBoxD> bbList)
		{
			bool flag = cache != null;
			if (flag)
			{
				CheckCacheValidity();
			}
			obb.HalfExtent += new Vector3D(border, 0.0, border);
			BoundingBoxD aABB = obb.GetAABB();
			int num = (int)Math.Round(aABB.HalfExtents.Max() * 2.0);
			aABB = new BoundingBoxD(aABB.Min, aABB.Min + num);
			aABB.Translate(obb.Center - aABB.Center);
			bbList.Add(new BoundingBoxD(aABB.Min, aABB.Max));
			aABB = aABB.TransformFast(voxelBase.PositionComp.WorldMatrixInvScaled);
			aABB.Translate(voxelBase.SizeInMetresHalf);
			Vector3I voxelCoord = Vector3I.Round(aABB.Min);
			Vector3I voxelCoord2 = voxelCoord + num;
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord, out Vector3I geometryCellCoord);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord2, out Vector3I geometryCellCoord2);
			MyOrientedBoundingBoxD myOrientedBoundingBoxD = obb;
			myOrientedBoundingBoxD.Transform(voxelBase.PositionComp.WorldMatrixInvScaled);
			myOrientedBoundingBoxD.Center += voxelBase.SizeInMetresHalf;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref geometryCellCoord, ref geometryCellCoord2);
			MyCellCoord myCellCoord = default(MyCellCoord);
			myCellCoord.Lod = 0;
			int num2 = 0;
			Vector3 offset = originPosition - voxelBase.PositionLeftBottomCorner;
			Vector3 vector = -Vector3.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(originPosition));
			Matrix rotation = Matrix.CreateFromQuaternion(Quaternion.Inverse(Quaternion.CreateFromForwardUp(Vector3.CalculatePerpendicularVector(vector), vector)));
			Matrix ownRotation = voxelBase.PositionComp.WorldMatrix.GetOrientation();
			while (vector3I_RangeIterator.IsValid())
			{
				if (flag && cache.TryGetValue(vector3I_RangeIterator.Current, out MyIsoMesh value))
				{
					if (value != null)
					{
						AddMeshTriangles(value, offset, rotation, ownRotation);
					}
					vector3I_RangeIterator.MoveNext();
					continue;
				}
				myCellCoord.CoordInLod = vector3I_RangeIterator.Current;
				MyVoxelCoordSystems.GeometryCellCoordToLocalAABB(ref myCellCoord.CoordInLod, out BoundingBox localAABB);
				if (!myOrientedBoundingBoxD.Intersects(ref localAABB))
				{
					num2++;
					vector3I_RangeIterator.MoveNext();
					continue;
				}
				BoundingBoxD item = new BoundingBoxD(localAABB.Min, localAABB.Max).Translate(-voxelBase.SizeInMetresHalf);
				bbList.Add(item);
				Vector3I vector3I = myCellCoord.CoordInLod * 8 - 1;
				Vector3I lodVoxelMax = vector3I + 8 + 1 + 1;
				MyIsoMesh myIsoMesh = MyPrecalcComponent.IsoMesher.Precalc(storage, 0, vector3I, lodVoxelMax, MyStorageDataTypeFlags.Content);
				if (flag)
				{
					cache[vector3I_RangeIterator.Current] = myIsoMesh;
				}
				if (myIsoMesh != null)
				{
					AddMeshTriangles(myIsoMesh, offset, rotation, ownRotation);
				}
				vector3I_RangeIterator.MoveNext();
			}
		}
	}
}
