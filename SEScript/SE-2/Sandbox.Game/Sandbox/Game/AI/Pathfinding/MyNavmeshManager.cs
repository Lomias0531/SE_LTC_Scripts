using ParallelTasks;
using RecastDetour;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Library.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyNavmeshManager
	{
		public class CoordComparer : IEqualityComparer<Vector2I>
		{
			public bool Equals(Vector2I a, Vector2I b)
			{
				if (a.X == b.X)
				{
					return a.Y == b.Y;
				}
				return false;
			}

			public int GetHashCode(Vector2I point)
			{
				return (point.X + point.Y).GetHashCode();
			}
		}

		public class OBBCoordComparer : IEqualityComparer<MyNavmeshOBBs.OBBCoords>
		{
			public bool Equals(MyNavmeshOBBs.OBBCoords a, MyNavmeshOBBs.OBBCoords b)
			{
				if (a.Coords.X == b.Coords.X)
				{
					return a.Coords.Y == b.Coords.Y;
				}
				return false;
			}

			public int GetHashCode(MyNavmeshOBBs.OBBCoords point)
			{
				return (point.Coords.X + point.Coords.Y).GetHashCode();
			}
		}

		private struct Vertex
		{
			public Vector3D pos;

			public Color color;
		}

		private static MyRandom ran = new MyRandom(0);

		public Color m_debugColor;

		private const float RECAST_CELL_SIZE = 0.2f;

		private const int MAX_TILES_TO_GENERATE = 7;

		private const int MAX_TICKS_WITHOUT_HEARTBEAT = 5000;

		private int m_ticksAfterLastPathRequest;

		private int m_tileSize;

		private int m_tileHeight;

		private int m_tileLineCount;

		private float m_border;

		private float m_heightCoordTransformationIncrease;

		private bool m_allTilesGenerated;

		private bool m_isManagerAlive = true;

		private MyNavmeshOBBs m_navmeshOBBs;

		private MyRecastOptions m_recastOptions;

		private MyNavigationInputMesh m_navInputMesh;

		private HashSet<MyNavmeshOBBs.OBBCoords> m_obbCoordsToUpdate = new HashSet<MyNavmeshOBBs.OBBCoords>(new OBBCoordComparer());

		private HashSet<Vector2I> m_coordsAlreadyGenerated = new HashSet<Vector2I>(new CoordComparer());

		private Dictionary<Vector2I, List<MyFormatPositionColor>> m_obbCoordsPolygons = new Dictionary<Vector2I, List<MyFormatPositionColor>>();

		private Dictionary<Vector2I, List<MyFormatPositionColor>> m_newObbCoordsPolygons = new Dictionary<Vector2I, List<MyFormatPositionColor>>();

		private bool m_navmeshTileGenerationRunning;

		private MyRDWrapper m_rdWrapper;

		private MyOrientedBoundingBoxD m_extendedBaseOBB;

		private List<MyVoxelMap> m_tmpTrackedVoxelMaps = new List<MyVoxelMap>();

		private Dictionary<long, MyVoxelMap> m_trackedVoxelMaps = new Dictionary<long, MyVoxelMap>();

		private int?[][] m_debugTileSize;

		private bool m_drawMesh;

		private bool m_updateDrawMesh;

		private List<MyRecastDetourPolygon> m_polygons = new List<MyRecastDetourPolygon>();

		private List<BoundingBoxD> m_groundCaptureAABBs = new List<BoundingBoxD>();

		private uint m_drawNavmeshID = uint.MaxValue;

		public Vector3D Center => m_navmeshOBBs.CenterOBB.Center;

		public MyOrientedBoundingBoxD CenterOBB => m_navmeshOBBs.CenterOBB;

		public MyPlanet Planet
		{
			get;
			private set;
		}

		public bool TilesAreWaitingGeneration => m_obbCoordsToUpdate.Count > 0;

		public bool DrawNavmesh
		{
			get
			{
				return m_drawMesh;
			}
			set
			{
				m_drawMesh = value;
				if (m_drawMesh)
				{
					DrawPersistentDebugNavmesh();
				}
				else
				{
					HidePersistentDebugNavmesh();
				}
			}
		}

		public MyNavmeshManager(MyRDPathfinding rdPathfinding, Vector3D center, Vector3D forwardDirection, int tileSize, int tileHeight, int tileLineCount, MyRecastOptions recastOptions)
		{
			Vector3 vector = new Vector3(ran.NextFloat(), ran.NextFloat(), ran.NextFloat());
			vector -= Math.Min(vector.X, Math.Min(vector.Y, vector.Z));
			vector /= Math.Max(vector.X, Math.Max(vector.Y, vector.Z));
			m_debugColor = new Color(vector);
			m_tileSize = tileSize;
			m_tileHeight = tileHeight;
			m_tileLineCount = tileLineCount;
			Planet = GetPlanet(center);
			m_heightCoordTransformationIncrease = 0.5f;
			float num = 0.2f;
			m_recastOptions = recastOptions;
			float num2 = (float)m_tileSize * 0.5f + (float)m_tileSize * (float)Math.Floor((float)m_tileLineCount * 0.5f);
			float num3 = (float)m_tileHeight * 0.5f;
			m_border = m_recastOptions.agentRadius + 3f * num;
			float[] bMin = new float[3]
			{
				0f - num2,
				0f - num3,
				0f - num2
			};
			float[] bMax = new float[3]
			{
				num2,
				num3,
				num2
			};
			m_rdWrapper = new MyRDWrapper();
			m_rdWrapper.Init(num, m_tileSize, bMin, bMax);
			Vector3D forwardDirection2 = Vector3D.CalculatePerpendicularVector(-Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(center)));
			m_navmeshOBBs = new MyNavmeshOBBs(Planet, center, forwardDirection2, m_tileLineCount, m_tileSize, m_tileHeight);
			m_debugTileSize = new int?[m_tileLineCount][];
			for (int i = 0; i < m_tileLineCount; i++)
			{
				m_debugTileSize[i] = new int?[m_tileLineCount];
			}
			m_extendedBaseOBB = new MyOrientedBoundingBoxD(m_navmeshOBBs.BaseOBB.Center, new Vector3D(m_navmeshOBBs.BaseOBB.HalfExtent.X, m_tileHeight, m_navmeshOBBs.BaseOBB.HalfExtent.Z), m_navmeshOBBs.BaseOBB.Orientation);
			m_navInputMesh = new MyNavigationInputMesh(rdPathfinding, Planet, center);
		}

		public bool InvalidateArea(BoundingBoxD areaAABB)
		{
			bool flag = false;
			if (!Intersects(areaAABB))
			{
				return flag;
			}
			bool flag2 = false;
			for (int i = 0; i < m_tileLineCount; i++)
			{
				bool flag3 = false;
				bool flag4 = false;
				for (int j = 0; j < m_tileLineCount; j++)
				{
					if (m_navmeshOBBs.GetOBB(i, j).Value.Intersects(ref areaAABB))
					{
						Vector2I vector2I = new Vector2I(i, j);
						flag3 = (flag4 = true);
						if (m_coordsAlreadyGenerated.Remove(vector2I))
						{
							flag = true;
							m_allTilesGenerated = false;
							m_newObbCoordsPolygons[vector2I] = null;
							m_navInputMesh.InvalidateCache(areaAABB);
						}
					}
					else if (flag4)
					{
						break;
					}
				}
				if (flag3)
				{
					flag2 = true;
				}
				else if (flag2)
				{
					break;
				}
			}
			if (flag)
			{
				m_updateDrawMesh = true;
			}
			return flag;
		}

		public bool ContainsPosition(Vector3D position)
		{
			LineD line = new LineD(Planet.PositionComp.WorldAABB.Center, position);
			return m_navmeshOBBs.BaseOBB.Intersects(ref line).HasValue;
		}

		public void TilesToGenerate(Vector3D initialPosition, Vector3D targetPosition)
		{
			TilesToGenerateInternal(initialPosition, targetPosition, out int _);
		}

		public bool GetPathPoints(Vector3D initialPosition, Vector3D targetPosition, out List<Vector3D> path, out bool noTilesToGenerate)
		{
			Heartbeat();
			bool result = false;
			noTilesToGenerate = true;
			path = new List<Vector3D>();
			if (!m_allTilesGenerated)
			{
				TilesToGenerateInternal(initialPosition, targetPosition, out int tilesAddedToGeneration);
				noTilesToGenerate = (tilesAddedToGeneration == 0);
			}
			Vector3D v = WorldPositionToLocalNavmeshPosition(initialPosition, m_heightCoordTransformationIncrease);
			Vector3D vector3D = targetPosition;
			bool flag = !ContainsPosition(targetPosition);
			if (flag)
			{
				vector3D = GetBorderPoint(initialPosition, targetPosition);
				vector3D = GetPositionAtDistanceFromPlanetCenter(vector3D, (initialPosition - Planet.PositionComp.WorldAABB.Center).Length());
			}
			Vector3D v2 = WorldPositionToLocalNavmeshPosition(vector3D, m_heightCoordTransformationIncrease);
			List<Vector3> path2 = m_rdWrapper.GetPath(v, v2);
			if (path2.Count > 0)
			{
				foreach (Vector3 item in path2)
				{
					path.Add(LocalPositionToWorldPosition(item));
				}
				Vector3D vector3D2 = path.Last();
				double num = (vector3D - vector3D2).Length();
				double num2 = 0.25;
				bool num3 = num <= num2;
				result = (num3 && !flag);
				if (num3)
				{
					if (flag)
					{
						path.RemoveAt(path.Count - 1);
						path.Add(targetPosition);
					}
					else if (noTilesToGenerate)
					{
						double pathDistance = GetPathDistance(path);
						double num4 = Vector3D.Distance(initialPosition, targetPosition);
						if (pathDistance > 3.0 * num4)
						{
							noTilesToGenerate = !TryGenerateTilesAroundPosition(initialPosition);
						}
					}
				}
				if ((!num3 && !m_allTilesGenerated) & noTilesToGenerate)
				{
					noTilesToGenerate = !TryGenerateTilesAroundPosition(vector3D2);
				}
			}
			return result;
		}

		public bool Update()
		{
			if (!CheckManagerHeartbeat())
			{
				return false;
			}
			GenerateNextQueuedTile();
			if (m_updateDrawMesh)
			{
				m_updateDrawMesh = false;
				UpdatePersistentDebugNavmesh();
			}
			return true;
		}

		public void UnloadData()
		{
			m_isManagerAlive = false;
			foreach (KeyValuePair<long, MyVoxelMap> trackedVoxelMap in m_trackedVoxelMaps)
			{
				trackedVoxelMap.Value.RangeChanged -= VoxelMapRangeChanged;
			}
			m_trackedVoxelMaps.Clear();
			m_rdWrapper.Clear();
			m_rdWrapper = null;
			m_navInputMesh.Clear();
			m_navInputMesh = null;
			m_navmeshOBBs.Clear();
			m_navmeshOBBs = null;
			m_obbCoordsToUpdate.Clear();
			m_obbCoordsToUpdate = null;
			m_coordsAlreadyGenerated.Clear();
			m_coordsAlreadyGenerated = null;
			m_obbCoordsPolygons.Clear();
			m_obbCoordsPolygons = null;
			m_newObbCoordsPolygons.Clear();
			m_newObbCoordsPolygons = null;
			m_polygons.Clear();
			m_polygons = null;
		}

		public void DebugDraw()
		{
			m_navmeshOBBs.DebugDraw();
			m_navInputMesh.DebugDraw();
			MyRenderProxy.DebugDrawOBB(m_extendedBaseOBB, Color.White, 0f, depthRead: true, smooth: false);
			foreach (BoundingBoxD groundCaptureAABB in m_groundCaptureAABBs)
			{
				MyRenderProxy.DebugDrawAABB(groundCaptureAABB, Color.Yellow);
			}
		}

		private Vector3D LocalPositionToWorldPosition(Vector3D position)
		{
			Vector3D vector3D = position;
			if (m_navmeshOBBs != null)
			{
				vector3D = Center;
			}
			Vector3D value = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(vector3D));
			return LocalNavmeshPositionToWorldPosition(m_navmeshOBBs.CenterOBB, position, vector3D, (0f - m_heightCoordTransformationIncrease) * value);
		}

		private MatrixD LocalNavmeshPositionToWorldPositionTransform(MyOrientedBoundingBoxD obb, Vector3D center)
		{
			Vector3D v = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(center));
			return MatrixD.CreateFromQuaternion(Quaternion.CreateFromForwardUp(Vector3D.CalculatePerpendicularVector(v), v));
		}

		private Vector3D LocalNavmeshPositionToWorldPosition(MyOrientedBoundingBoxD obb, Vector3D position, Vector3D center, Vector3D heightIncrease)
		{
			MatrixD matrix = LocalNavmeshPositionToWorldPositionTransform(obb, center);
			return Vector3D.Transform(position, matrix) + Center + heightIncrease;
		}

		private Vector3D WorldPositionToLocalNavmeshPosition(Vector3D position, float heightIncrease)
		{
			Vector3D vector3D = -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(Center));
			MatrixD matrix = MatrixD.CreateFromQuaternion(Quaternion.Inverse(Quaternion.CreateFromForwardUp(Vector3D.CalculatePerpendicularVector(vector3D), vector3D)));
			return Vector3D.Transform(position - Center + heightIncrease * vector3D, matrix);
		}

		private Vector3D GetBorderPoint(Vector3D startingPoint, Vector3D outsidePoint)
		{
			LineD line = new LineD(startingPoint, outsidePoint);
			double? num = m_extendedBaseOBB.Intersects(ref line);
			if (!num.HasValue)
			{
				return outsidePoint;
			}
			line.Length = num.Value - 1.0;
			line.To = startingPoint + line.Direction * num.Value;
			return line.To;
		}

		private void Heartbeat()
		{
			m_ticksAfterLastPathRequest = 0;
		}

		private bool CheckManagerHeartbeat()
		{
			if (!m_isManagerAlive)
			{
				return false;
			}
			m_ticksAfterLastPathRequest++;
			m_isManagerAlive = (m_ticksAfterLastPathRequest < 5000);
			if (!m_isManagerAlive)
			{
				UnloadData();
			}
			return m_isManagerAlive;
		}

		private double GetPathDistance(List<Vector3D> path)
		{
			double num = 0.0;
			for (int i = 0; i < path.Count - 1; i++)
			{
				num += Vector3D.Distance(path[i], path[i + 1]);
			}
			return num;
		}

		private bool Intersects(BoundingBoxD obb)
		{
			return m_extendedBaseOBB.Intersects(ref obb);
		}

		private bool TryGenerateTilesAroundPosition(Vector3D position)
		{
			MyNavmeshOBBs.OBBCoords? oBBCoord = m_navmeshOBBs.GetOBBCoord(position);
			if (oBBCoord.HasValue)
			{
				return TryGenerateNeighbourTiles(oBBCoord.Value);
			}
			return false;
		}

		private bool TryGenerateNeighbourTiles(MyNavmeshOBBs.OBBCoords obbCoord, int radius = 1)
		{
			int num = 0;
			bool flag = false;
			Vector2I vector2I = default(Vector2I);
			for (int i = -radius; i <= radius; i++)
			{
				int num2 = (i == -radius || i == radius) ? 1 : (2 * radius);
				for (int j = -radius; j <= radius; j += num2)
				{
					vector2I.X = obbCoord.Coords.X + j;
					vector2I.Y = obbCoord.Coords.Y + i;
					MyNavmeshOBBs.OBBCoords? oBBCoord = m_navmeshOBBs.GetOBBCoord(vector2I.X, vector2I.Y);
					if (!oBBCoord.HasValue)
					{
						continue;
					}
					flag = true;
					if (AddTileToGeneration(oBBCoord.Value))
					{
						num++;
						if (num >= 7)
						{
							return true;
						}
					}
				}
			}
			if (num > 0)
			{
				return true;
			}
			m_allTilesGenerated = !flag;
			if (m_allTilesGenerated)
			{
				return false;
			}
			return TryGenerateNeighbourTiles(obbCoord, radius + 1);
		}

		private List<MyNavmeshOBBs.OBBCoords> TilesToGenerateInternal(Vector3D initialPosition, Vector3D targetPosition, out int tilesAddedToGeneration)
		{
			tilesAddedToGeneration = 0;
			List<MyNavmeshOBBs.OBBCoords> intersectedOBB = m_navmeshOBBs.GetIntersectedOBB(new LineD(initialPosition, targetPosition));
			foreach (MyNavmeshOBBs.OBBCoords item in intersectedOBB)
			{
				if (AddTileToGeneration(item))
				{
					tilesAddedToGeneration++;
					if (tilesAddedToGeneration == 7)
					{
						return intersectedOBB;
					}
				}
			}
			return intersectedOBB;
		}

		private bool AddTileToGeneration(MyNavmeshOBBs.OBBCoords obbCoord)
		{
			if (!m_coordsAlreadyGenerated.Contains(obbCoord.Coords))
			{
				return m_obbCoordsToUpdate.Add(obbCoord);
			}
			return false;
		}

		private Vector3D GetPositionAtDistanceFromPlanetCenter(Vector3D position, double distance)
		{
			(position - Planet.PositionComp.WorldAABB.Center).Length();
			return -Vector3D.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(position)) * distance + Planet.PositionComp.WorldAABB.Center;
		}

		private MyPlanet GetPlanet(Vector3D position)
		{
			int num = 200;
			BoundingBoxD box = new BoundingBoxD(position - (float)num * 0.5f, position + (float)num * 0.5f);
			return MyGamePruningStructure.GetClosestPlanet(ref box);
		}

		private void GenerateNextQueuedTile()
		{
			if (!m_navmeshTileGenerationRunning && TilesAreWaitingGeneration)
			{
				m_navmeshTileGenerationRunning = true;
				MyNavmeshOBBs.OBBCoords obb = m_obbCoordsToUpdate.First();
				m_obbCoordsToUpdate.Remove(obb);
				m_coordsAlreadyGenerated.Add(obb.Coords);
				Parallel.Start(delegate
				{
					GenerateTile(obb);
				});
			}
		}

		private unsafe void GenerateTile(MyNavmeshOBBs.OBBCoords obbCoord)
		{
			MyOrientedBoundingBoxD oBB = obbCoord.OBB;
			Vector3 vector = WorldPositionToLocalNavmeshPosition(oBB.Center, 0f);
			List<BoundingBoxD> list = new List<BoundingBoxD>();
			MyNavigationInputMesh.WorldVerticesInfo worldVertices = m_navInputMesh.GetWorldVertices(m_border, Center, oBB, list, m_tmpTrackedVoxelMaps);
			m_groundCaptureAABBs = list;
			foreach (MyVoxelMap tmpTrackedVoxelMap in m_tmpTrackedVoxelMaps)
			{
				if (!m_trackedVoxelMaps.ContainsKey(tmpTrackedVoxelMap.EntityId))
				{
					tmpTrackedVoxelMap.RangeChanged += VoxelMapRangeChanged;
					m_trackedVoxelMaps.Add(tmpTrackedVoxelMap.EntityId, tmpTrackedVoxelMap);
				}
			}
			m_tmpTrackedVoxelMaps.Clear();
			if (worldVertices.Triangles.Count > 0)
			{
				fixed (Vector3* ptr = worldVertices.Vertices.GetInternalArray())
				{
					float* vertices = (float*)ptr;
					fixed (int* triangles = worldVertices.Triangles.GetInternalArray())
					{
						m_rdWrapper.CreateNavmeshTile(vector, ref m_recastOptions, ref m_polygons, obbCoord.Coords.X, obbCoord.Coords.Y, 0, vertices, worldVertices.Vertices.Count, triangles, worldVertices.Triangles.Count / 3);
					}
				}
				GenerateDebugDrawPolygonNavmesh(Planet, m_polygons, m_navmeshOBBs.CenterOBB, obbCoord.Coords);
				GenerateDebugTileDataSize(vector, obbCoord.Coords.X, obbCoord.Coords.Y);
				if (m_polygons != null)
				{
					m_polygons.Clear();
					m_updateDrawMesh = true;
				}
			}
			else
			{
				m_newObbCoordsPolygons[obbCoord.Coords] = null;
			}
			m_navmeshTileGenerationRunning = false;
		}

		private void VoxelMapRangeChanged(MyVoxelBase storage, Vector3I minVoxelChanged, Vector3I maxVoxelChanged, MyStorageDataTypeFlags changedData)
		{
			BoundingBoxD voxelAreaAABB = MyRDPathfinding.GetVoxelAreaAABB(storage, minVoxelChanged, maxVoxelChanged);
			InvalidateArea(voxelAreaAABB);
		}

		private void GenerateDebugTileDataSize(Vector3 center, int xCoord, int yCoord)
		{
			int tileDataSize = m_rdWrapper.GetTileDataSize(center, 0);
			m_debugTileSize[xCoord][yCoord] = tileDataSize;
		}

		private void GenerateDebugDrawPolygonNavmesh(MyPlanet planet, List<MyRecastDetourPolygon> polygons, MyOrientedBoundingBoxD centerOBB, Vector2I coords)
		{
			if (polygons != null)
			{
				List<MyFormatPositionColor> list = new List<MyFormatPositionColor>();
				int num = 10;
				int num2 = 0;
				int num3 = 95;
				int num4 = 10;
				foreach (MyRecastDetourPolygon polygon in polygons)
				{
					Vector3[] vertices = polygon.Vertices;
					foreach (Vector3 v in vertices)
					{
						MyFormatPositionColor myFormatPositionColor = default(MyFormatPositionColor);
						myFormatPositionColor.Position = LocalNavmeshPositionToWorldPosition(centerOBB, v, Center, Vector3D.Zero);
						myFormatPositionColor.Color = new Color(0, num + num2, 0);
						MyFormatPositionColor item = myFormatPositionColor;
						list.Add(item);
					}
					num2 += num4;
					num2 %= num3;
				}
				if (list.Count > 0)
				{
					m_newObbCoordsPolygons[coords] = list;
				}
			}
		}

		private void DrawPersistentDebugNavmesh()
		{
			foreach (KeyValuePair<Vector2I, List<MyFormatPositionColor>> newObbCoordsPolygon in m_newObbCoordsPolygons)
			{
				if (m_newObbCoordsPolygons[newObbCoordsPolygon.Key] == null)
				{
					m_obbCoordsPolygons.Remove(newObbCoordsPolygon.Key);
				}
				else
				{
					m_obbCoordsPolygons[newObbCoordsPolygon.Key] = newObbCoordsPolygon.Value;
				}
			}
			m_newObbCoordsPolygons.Clear();
			if (m_obbCoordsPolygons.Count > 0)
			{
				List<MyFormatPositionColor> list = new List<MyFormatPositionColor>();
				foreach (List<MyFormatPositionColor> value in m_obbCoordsPolygons.Values)
				{
					for (int i = 0; i < value.Count; i++)
					{
						list.Add(value[i]);
					}
				}
				if (m_drawNavmeshID == uint.MaxValue)
				{
					m_drawNavmeshID = MyRenderProxy.DebugDrawMesh(list, MatrixD.Identity, depthRead: true, shaded: true);
				}
				else
				{
					MyRenderProxy.DebugDrawUpdateMesh(m_drawNavmeshID, list, MatrixD.Identity, depthRead: true, shaded: true);
				}
			}
		}

		private void HidePersistentDebugNavmesh()
		{
			if (m_drawNavmeshID != uint.MaxValue)
			{
				MyRenderProxy.RemoveRenderObject(m_drawNavmeshID, MyRenderProxy.ObjectType.DebugDrawMesh);
				m_drawNavmeshID = uint.MaxValue;
			}
		}

		private void UpdatePersistentDebugNavmesh()
		{
			DrawNavmesh = DrawNavmesh;
		}
	}
}
