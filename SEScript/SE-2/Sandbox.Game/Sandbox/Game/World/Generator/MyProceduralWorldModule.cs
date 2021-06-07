using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Noise;
using VRage.Noise.Combiners;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public abstract class MyProceduralWorldModule
	{
		protected MyProceduralWorldModule m_parent;

		protected List<MyProceduralWorldModule> m_children = new List<MyProceduralWorldModule>();

		protected int m_seed;

		protected double m_objectDensity;

		protected MyDynamicAABBTreeD m_cellsTree = new MyDynamicAABBTreeD(Vector3D.Zero);

		protected Dictionary<Vector3I, MyProceduralCell> m_cells = new Dictionary<Vector3I, MyProceduralCell>();

		protected CachingHashSet<MyProceduralCell> m_dirtyCells = new CachingHashSet<MyProceduralCell>();

		protected static List<MyObjectSeed> m_tempObjectSeedList = new List<MyObjectSeed>();

		protected static List<MyProceduralCell> m_tempProceduralCellsList = new List<MyProceduralCell>();

		protected List<IMyAsteroidFieldDensityFunction> m_densityFunctionsFilled = new List<IMyAsteroidFieldDensityFunction>();

		protected List<IMyAsteroidFieldDensityFunction> m_densityFunctionsRemoved = new List<IMyAsteroidFieldDensityFunction>();

		public readonly double CELL_SIZE;

		public readonly int SCALE;

		protected const int BIG_PRIME1 = 16785407;

		protected const int BIG_PRIME2 = 39916801;

		protected const int BIG_PRIME3 = 479001599;

		protected const int TWIN_PRIME_MIDDLE1 = 240;

		protected const int TWIN_PRIME_MIDDLE2 = 312;

		protected const int TWIN_PRIME_MIDDLE3 = 462;

		private List<IMyModule> tmpDensityFunctions = new List<IMyModule>();

		private static List<MyObjectSeed> m_asteroidsCache;

		protected MyProceduralWorldModule(double cellSize, int radiusMultiplier, int seed, double density, MyProceduralWorldModule parent = null)
		{
			CELL_SIZE = cellSize;
			SCALE = radiusMultiplier;
			m_seed = seed;
			m_objectDensity = density;
			m_parent = parent;
			parent?.m_children.Add(this);
		}

		protected void ChildrenAddDensityFunctionFilled(IMyAsteroidFieldDensityFunction func)
		{
			foreach (MyProceduralWorldModule child in m_children)
			{
				child.AddDensityFunctionFilled(func);
				child.ChildrenAddDensityFunctionFilled(func);
			}
		}

		protected void ChildrenRemoveDensityFunctionFilled(IMyAsteroidFieldDensityFunction func)
		{
			foreach (MyProceduralWorldModule child in m_children)
			{
				child.ChildrenRemoveDensityFunctionFilled(func);
				child.RemoveDensityFunctionFilled(func);
			}
		}

		protected void ChildrenAddDensityFunctionRemoved(IMyAsteroidFieldDensityFunction func)
		{
			foreach (MyProceduralWorldModule child in m_children)
			{
				child.AddDensityFunctionRemoved(func);
				child.ChildrenAddDensityFunctionRemoved(func);
			}
		}

		protected void ChildrenRemoveDensityFunctionRemoved(IMyAsteroidFieldDensityFunction func)
		{
			foreach (MyProceduralWorldModule child in m_children)
			{
				child.ChildrenRemoveDensityFunctionRemoved(func);
				child.RemoveDensityFunctionRemoved(func);
			}
		}

		protected void AddDensityFunctionFilled(IMyAsteroidFieldDensityFunction func)
		{
			m_densityFunctionsFilled.Add(func);
		}

		protected void RemoveDensityFunctionFilled(IMyAsteroidFieldDensityFunction func)
		{
			m_densityFunctionsFilled.Remove(func);
		}

		public void AddDensityFunctionRemoved(IMyAsteroidFieldDensityFunction func)
		{
			lock (m_densityFunctionsRemoved)
			{
				m_densityFunctionsRemoved.Add(func);
			}
		}

		protected void RemoveDensityFunctionRemoved(IMyAsteroidFieldDensityFunction func)
		{
			lock (m_densityFunctionsRemoved)
			{
				m_densityFunctionsRemoved.Remove(func);
			}
		}

		public void GetObjectSeeds(BoundingSphereD sphere, List<MyObjectSeed> list, bool scale = true)
		{
			BoundingSphereD sphere2 = sphere;
			if (scale)
			{
				sphere2.Radius *= SCALE;
			}
			GenerateObjectSeeds(ref sphere2);
			OverlapAllBoundingSphere(ref sphere2, list);
		}

		protected abstract MyProceduralCell GenerateProceduralCell(ref Vector3I cellId);

		protected int GetCellSeed(ref Vector3I cell)
		{
			return m_seed + cell.X * 16785407 + cell.Y * 39916801 + cell.Z * 479001599;
		}

		protected int GetObjectIdSeed(MyObjectSeed objectSeed)
		{
			return (((((objectSeed.CellId.GetHashCode() * 397) ^ m_seed) * 397) ^ objectSeed.Params.Index) * 397) ^ objectSeed.Params.Seed;
		}

		public abstract void GenerateObjects(List<MyObjectSeed> list, HashSet<MyObjectSeedParams> existingObjectsSeeds);

		protected void GenerateObjectSeeds(ref BoundingSphereD sphere)
		{
			Vector3I_RangeIterator cellsIterator = GetCellsIterator(sphere);
			while (cellsIterator.IsValid())
			{
				Vector3I cellId = cellsIterator.Current;
				if (!m_cells.ContainsKey(cellId))
				{
					BoundingBoxD box = new BoundingBoxD(cellId * CELL_SIZE, (cellId + 1) * CELL_SIZE);
					if (sphere.Contains(box) != 0)
					{
						MyProceduralCell myProceduralCell = GenerateProceduralCell(ref cellId);
						if (myProceduralCell != null)
						{
							m_cells.Add(cellId, myProceduralCell);
							BoundingBoxD aabb = myProceduralCell.BoundingVolume;
							myProceduralCell.proxyId = m_cellsTree.AddProxy(ref aabb, myProceduralCell, 0u);
						}
					}
				}
				cellsIterator.MoveNext();
			}
		}

		protected IMyModule GetCellDensityFunctionFilled(BoundingBoxD bbox)
		{
			foreach (IMyAsteroidFieldDensityFunction item in m_densityFunctionsFilled)
			{
				if (item.ExistsInCell(ref bbox))
				{
					tmpDensityFunctions.Add(item);
				}
			}
			if (tmpDensityFunctions.Count == 0)
			{
				return null;
			}
			for (int num = tmpDensityFunctions.Count; num > 1; num = num / 2 + num % 2)
			{
				for (int i = 0; i < num / 2; i++)
				{
					tmpDensityFunctions[i] = new MyMax(tmpDensityFunctions[i * 2], tmpDensityFunctions[i * 2 + 1]);
				}
				if (num % 2 == 1)
				{
					tmpDensityFunctions[num - 1] = tmpDensityFunctions[num / 2];
				}
			}
			IMyModule result = tmpDensityFunctions[0];
			tmpDensityFunctions.Clear();
			return result;
		}

		protected IMyModule GetCellDensityFunctionRemoved(BoundingBoxD bbox)
		{
			foreach (IMyAsteroidFieldDensityFunction item in m_densityFunctionsRemoved)
			{
				if (item != null && item.ExistsInCell(ref bbox))
				{
					tmpDensityFunctions.Add(item);
				}
			}
			if (tmpDensityFunctions.Count == 0)
			{
				return null;
			}
			for (int num = tmpDensityFunctions.Count; num > 1; num = num / 2 + num % 2)
			{
				for (int i = 0; i < num / 2; i++)
				{
					tmpDensityFunctions[i] = new MyMin(tmpDensityFunctions[i * 2], tmpDensityFunctions[i * 2 + 1]);
				}
				if (num % 2 == 1)
				{
					tmpDensityFunctions[num - 1] = tmpDensityFunctions[num / 2];
				}
			}
			IMyModule result = tmpDensityFunctions[0];
			tmpDensityFunctions.Clear();
			return result;
		}

		public void MarkCellsDirty(BoundingSphereD toMark, BoundingSphereD? toExclude = null, bool scale = true)
		{
			BoundingSphereD sphere = new BoundingSphereD(toMark.Center, toMark.Radius * (double)((!scale) ? 1 : SCALE));
			BoundingSphereD boundingSphereD = default(BoundingSphereD);
			if (toExclude.HasValue)
			{
				boundingSphereD = toExclude.Value;
				if (scale)
				{
					boundingSphereD.Radius *= SCALE;
				}
			}
			Vector3I_RangeIterator cellsIterator = GetCellsIterator(sphere);
			while (cellsIterator.IsValid())
			{
				Vector3I current = cellsIterator.Current;
				if (m_cells.TryGetValue(current, out MyProceduralCell value) && (!toExclude.HasValue || boundingSphereD.Contains(value.BoundingVolume) == ContainmentType.Disjoint))
				{
					m_dirtyCells.Add(value);
				}
				cellsIterator.MoveNext();
			}
		}

		public void ProcessDirtyCells(Dictionary<MyEntity, MyEntityTracker> trackedEntities)
		{
			m_dirtyCells.ApplyAdditions();
			if (m_dirtyCells.Count != 0)
			{
				foreach (MyProceduralCell dirtyCell in m_dirtyCells)
				{
					foreach (MyEntityTracker value in trackedEntities.Values)
					{
						BoundingSphereD boundingVolume = value.BoundingVolume;
						boundingVolume.Radius *= SCALE;
						if (boundingVolume.Contains(dirtyCell.BoundingVolume) != 0)
						{
							m_dirtyCells.Remove(dirtyCell);
							break;
						}
					}
				}
				m_dirtyCells.ApplyRemovals();
				foreach (MyProceduralCell dirtyCell2 in m_dirtyCells)
				{
					dirtyCell2.GetAll(m_tempObjectSeedList);
					foreach (MyObjectSeed tempObjectSeed in m_tempObjectSeedList)
					{
						if (tempObjectSeed.Params.Generated)
						{
							CloseObjectSeed(tempObjectSeed);
						}
					}
					m_tempObjectSeedList.Clear();
				}
				foreach (MyProceduralCell dirtyCell3 in m_dirtyCells)
				{
					m_cells.Remove(dirtyCell3.CellId);
					m_cellsTree.RemoveProxy(dirtyCell3.proxyId);
				}
				m_dirtyCells.Clear();
			}
		}

		protected abstract void CloseObjectSeed(MyObjectSeed objectSeed);

		protected Vector3I_RangeIterator GetCellsIterator(BoundingSphereD sphere)
		{
			return GetCellsIterator(BoundingBoxD.CreateFromSphere(sphere));
		}

		protected Vector3I_RangeIterator GetCellsIterator(BoundingBoxD bbox)
		{
			Vector3I start = Vector3I.Floor(bbox.Min / CELL_SIZE);
			Vector3I end = Vector3I.Floor(bbox.Max / CELL_SIZE);
			return new Vector3I_RangeIterator(ref start, ref end);
		}

		protected void OverlapAllBoundingSphere(ref BoundingSphereD sphere, List<MyObjectSeed> list)
		{
			m_cellsTree.OverlapAllBoundingSphere(ref sphere, m_tempProceduralCellsList);
			foreach (MyProceduralCell tempProceduralCells in m_tempProceduralCellsList)
			{
				tempProceduralCells.OverlapAllBoundingSphere(ref sphere, list);
			}
			m_tempProceduralCellsList.Clear();
		}

		public static Vector3D? FindFreeLocationCloseToAsteroid(BoundingSphereD searchArea, BoundingSphereD? suppressedArea, bool takeOccupiedPositions, bool sortByDistance, float collisionRadius, float minFreeRange, out Vector3 forward, out Vector3 up)
		{
			Vector3D spawnPosition;
			Vector3D? freePosition;
			for (int num = (!sortByDistance) ? 1 : 3; num > 0; num--)
			{
				bool flag = num == 1;
				BoundingSphereD boundingSphereD = new BoundingSphereD(searchArea.Center, searchArea.Radius / (double)num);
				if (flag || !suppressedArea.HasValue || suppressedArea.Value.Contains(boundingSphereD) != ContainmentType.Contains)
				{
					using (MyUtils.ReuseCollection(ref m_asteroidsCache))
					{
						List<MyObjectSeed> asteroidsCache = m_asteroidsCache;
						MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(boundingSphereD, asteroidsCache);
						asteroidsCache.RemoveAll(delegate(MyObjectSeed x)
						{
							MyObjectSeedType type = x.Params.Type;
							return type != MyObjectSeedType.Asteroid && type != MyObjectSeedType.AsteroidCluster;
						});
						if (asteroidsCache.Count != 0)
						{
							if (sortByDistance)
							{
								spawnPosition = searchArea.Center;
								asteroidsCache.Sort(delegate(MyObjectSeed a, MyObjectSeed b)
								{
									double num3 = Vector3D.DistanceSquared(spawnPosition, a.BoundingVolume.Center);
									double value2 = Vector3D.DistanceSquared(spawnPosition, b.BoundingVolume.Center);
									return num3.CompareTo(value2);
								});
							}
							else
							{
								asteroidsCache.ShuffleList();
							}
							bool flag2 = false;
							for (int num2 = asteroidsCache.Count - 1; num2 >= 0; num2--)
							{
								bool flag3 = false;
								BoundingBoxD boundingVolume = asteroidsCache[num2].BoundingVolume;
								if (suppressedArea.HasValue && suppressedArea.Value.Contains(boundingVolume) != 0)
								{
									flag3 = true;
								}
								else
								{
									double radius = Math.Max(boundingVolume.HalfExtents.AbsMax() * 2.0, minFreeRange);
									flag3 = !IsZoneFree(new BoundingSphereD(boundingVolume.Center, radius));
								}
								if (flag3)
								{
									if (takeOccupiedPositions)
									{
										asteroidsCache.Add(asteroidsCache[num2]);
									}
									asteroidsCache.RemoveAt(num2);
								}
								else
								{
									flag2 = true;
								}
							}
							if (flag2 || flag)
							{
								foreach (MyObjectSeed item in asteroidsCache)
								{
									BoundingBoxD boundingVolume2 = item.BoundingVolume;
									Vector3D value = MyUtils.GetRandomVector3Normalized();
									Vector3D basePos = Vector3D.Clamp(boundingVolume2.Center + value * boundingVolume2.HalfExtents.AbsMax() * 10.0, boundingVolume2.Min, boundingVolume2.Max) + (boundingVolume2.HalfExtents.AbsMax() / 2.0 + (double)collisionRadius) * value;
									freePosition = MyEntities.FindFreePlace(basePos, collisionRadius);
									if (freePosition.HasValue && !MyPlanets.Static.GetPlanetAABBs().Any((BoundingBoxD x) => x.Contains(freePosition.Value) != ContainmentType.Disjoint) && !asteroidsCache.Any((MyObjectSeed x) => x.BoundingVolume.Contains(freePosition.Value) != ContainmentType.Disjoint))
									{
										forward = Vector3D.Normalize(boundingVolume2.Center - freePosition.Value);
										up = MyUtils.GetRandomPerpendicularVector(in forward);
										return freePosition.Value;
									}
								}
							}
						}
					}
				}
			}
			up = default(Vector3D);
			forward = default(Vector3D);
			return null;
		}

		public static bool IsZoneFree(BoundingSphereD safeZone)
		{
			ClearToken<MyEntity> clearToken = MyEntities.GetTopMostEntitiesInSphere(ref safeZone).GetClearToken();
			try
			{
				foreach (MyEntity item in clearToken.List)
				{
					if (item is MyCubeGrid)
					{
						return false;
					}
				}
			}
			finally
			{
				((IDisposable)clearToken).Dispose();
			}
			return true;
		}

		internal void OverlapAllBoundingBox(ref BoundingBoxD box, List<MyObjectSeed> list)
		{
			m_cellsTree.OverlapAllBoundingBox(ref box, m_tempProceduralCellsList);
			foreach (MyProceduralCell tempProceduralCells in m_tempProceduralCellsList)
			{
				tempProceduralCells.OverlapAllBoundingBox(ref box, list);
			}
			m_tempProceduralCellsList.Clear();
		}

		internal void GetAllCells(List<MyProceduralCell> list)
		{
			m_cellsTree.GetAll(list, clear: false);
		}
	}
}
