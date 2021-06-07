using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Noise;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyProceduralPlanetCellGenerator : MyProceduralWorldModule
	{
		public const int MOON_SIZE_MIN_LIMIT = 4000;

		public const int MOON_SIZE_MAX_LIMIT = 30000;

		public const int PLANET_SIZE_MIN_LIMIT = 8000;

		public const int PLANET_SIZE_MAX_LIMIT = 120000;

		internal readonly float PLANET_SIZE_MIN;

		internal readonly float PLANET_SIZE_MAX;

		internal const int MOONS_MAX = 3;

		internal readonly float MOON_SIZE_MIN;

		internal readonly float MOON_SIZE_MAX;

		internal const int MOON_DISTANCE_MIN = 4000;

		internal const int MOON_DISTANCE_MAX = 32000;

		internal const double MOON_DENSITY = 0.0;

		internal const int FALLOFF = 16000;

		internal const double GRAVITY_SIZE_MULTIPLIER = 1.1;

		internal readonly double OBJECT_SEED_RADIUS;

		private List<BoundingBoxD> m_tmpClusterBoxes = new List<BoundingBoxD>(4);

		private List<MyVoxelBase> m_tmpVoxelMapsList = new List<MyVoxelBase>();

		public MyProceduralPlanetCellGenerator(int seed, double density, float planetSizeMax, float planetSizeMin, float moonSizeMax, float moonSizeMin, MyProceduralWorldModule parent = null)
			: base(2048000.0, 250, seed, (density + 1.0) / 2.0 - 1.0, parent)
		{
			if (planetSizeMax < planetSizeMin)
			{
				float num = planetSizeMax;
				planetSizeMax = planetSizeMin;
				planetSizeMin = num;
			}
			PLANET_SIZE_MAX = MathHelper.Clamp(planetSizeMax, 8000f, 120000f);
			PLANET_SIZE_MIN = MathHelper.Clamp(planetSizeMin, 8000f, planetSizeMax);
			if (moonSizeMax < moonSizeMin)
			{
				float num2 = moonSizeMax;
				moonSizeMax = moonSizeMin;
				moonSizeMin = num2;
			}
			MOON_SIZE_MAX = MathHelper.Clamp(moonSizeMax, 4000f, 30000f);
			MOON_SIZE_MIN = MathHelper.Clamp(moonSizeMin, 4000f, moonSizeMax);
			OBJECT_SEED_RADIUS = (double)PLANET_SIZE_MAX / 2.0 * 1.1 + 2.0 * ((double)MOON_SIZE_MAX / 2.0 * 1.1 + 64000.0);
			AddDensityFunctionFilled(new MyInfiniteDensityFunction(MyRandom.Instance, 0.001));
		}

		protected override MyProceduralCell GenerateProceduralCell(ref Vector3I cellId)
		{
			MyProceduralCell myProceduralCell = new MyProceduralCell(cellId, CELL_SIZE);
			IMyModule cellDensityFunctionFilled = GetCellDensityFunctionFilled(myProceduralCell.BoundingVolume);
			if (cellDensityFunctionFilled == null)
			{
				return null;
			}
			IMyModule cellDensityFunctionRemoved = GetCellDensityFunctionRemoved(myProceduralCell.BoundingVolume);
			int cellSeed = GetCellSeed(ref cellId);
			MyRandom instance = MyRandom.Instance;
			using (instance.PushSeed(cellSeed))
			{
				Vector3D vector3D = new Vector3D(instance.NextDouble(), instance.NextDouble(), instance.NextDouble());
				vector3D *= (CELL_SIZE - 2.0 * OBJECT_SEED_RADIUS) / CELL_SIZE;
				vector3D += OBJECT_SEED_RADIUS / CELL_SIZE;
				vector3D += (Vector3D)cellId;
				vector3D *= CELL_SIZE;
				if (!MyEntities.IsInsideWorld(vector3D))
				{
					return myProceduralCell;
				}
				if (!(cellDensityFunctionFilled.GetValue(vector3D.X, vector3D.Y, vector3D.Z) < m_objectDensity))
				{
					return myProceduralCell;
				}
				double size = MathHelper.Lerp(PLANET_SIZE_MIN, PLANET_SIZE_MAX, instance.NextDouble());
				MyObjectSeed myObjectSeed = new MyObjectSeed(myProceduralCell, vector3D, size);
				myObjectSeed.Params.Type = MyObjectSeedType.Planet;
				myObjectSeed.Params.Seed = instance.Next();
				myObjectSeed.Params.Index = 0;
				myObjectSeed.UserData = new MySphereDensityFunction(vector3D, (double)PLANET_SIZE_MAX / 2.0 * 1.1 + 16000.0, 16000.0);
				int index = 1;
				GenerateObject(myProceduralCell, myObjectSeed, ref index, instance, cellDensityFunctionFilled, cellDensityFunctionRemoved);
				return myProceduralCell;
			}
		}

		private void GenerateObject(MyProceduralCell cell, MyObjectSeed objectSeed, ref int index, MyRandom random, IMyModule densityFunctionFilled, IMyModule densityFunctionRemoved)
		{
			cell.AddObject(objectSeed);
			IMyAsteroidFieldDensityFunction myAsteroidFieldDensityFunction = objectSeed.UserData as IMyAsteroidFieldDensityFunction;
			if (myAsteroidFieldDensityFunction != null)
			{
				ChildrenAddDensityFunctionRemoved(myAsteroidFieldDensityFunction);
			}
			switch (objectSeed.Params.Type)
			{
			case MyObjectSeedType.Empty:
			case MyObjectSeedType.Moon:
				break;
			case MyObjectSeedType.Planet:
			{
				m_tmpClusterBoxes.Add(objectSeed.BoundingVolume);
				for (int i = 0; i < 3; i++)
				{
					Vector3D randomDirection = MyProceduralWorldGenerator.GetRandomDirection(random);
					double num = MathHelper.Lerp(MOON_SIZE_MIN, MOON_SIZE_MAX, random.NextDouble());
					double num2 = MathHelper.Lerp(4000.0, 32000.0, random.NextDouble());
					Vector3D vector3D = objectSeed.BoundingVolume.Center + randomDirection * (num + objectSeed.BoundingVolume.HalfExtents.Length() * 2.0 + num2);
					if (densityFunctionFilled.GetValue(vector3D.X, vector3D.Y, vector3D.Z) < 0.0)
					{
						MyObjectSeed myObjectSeed = new MyObjectSeed(cell, vector3D, num);
						myObjectSeed.Params.Seed = random.Next();
						myObjectSeed.Params.Type = MyObjectSeedType.Moon;
						myObjectSeed.Params.Index = index++;
						myObjectSeed.UserData = new MySphereDensityFunction(vector3D, (double)MOON_SIZE_MAX / 2.0 * 1.1 + 16000.0, 16000.0);
						bool flag = false;
						foreach (BoundingBoxD tmpClusterBox in m_tmpClusterBoxes)
						{
							if (flag |= myObjectSeed.BoundingVolume.Intersects(tmpClusterBox))
							{
								break;
							}
						}
						if (!flag)
						{
							m_tmpClusterBoxes.Add(myObjectSeed.BoundingVolume);
							GenerateObject(cell, myObjectSeed, ref index, random, densityFunctionFilled, densityFunctionRemoved);
						}
					}
				}
				m_tmpClusterBoxes.Clear();
				break;
			}
			default:
				throw new InvalidBranchException();
			}
		}

		public override void GenerateObjects(List<MyObjectSeed> objectsList, HashSet<MyObjectSeedParams> existingObjectsSeeds)
		{
			foreach (MyObjectSeed objects in objectsList)
			{
				if (!objects.Params.Generated)
				{
					objects.Params.Generated = true;
					using (MyRandom.Instance.PushSeed(GetObjectIdSeed(objects)))
					{
						BoundingBoxD box = objects.BoundingVolume;
						MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, m_tmpVoxelMapsList);
						string b = $"{objects.Params.Type}_{objects.CellId.X}_{objects.CellId.Y}_{objects.CellId.Z}_{objects.Params.Index}_{objects.Params.Seed}";
						bool flag = false;
						foreach (MyVoxelBase tmpVoxelMaps in m_tmpVoxelMapsList)
						{
							if (tmpVoxelMaps.StorageName == b)
							{
								existingObjectsSeeds.Add(objects.Params);
								flag = true;
								break;
							}
						}
						m_tmpVoxelMapsList.Clear();
					}
				}
			}
		}

		private long GetPlanetEntityId(MyObjectSeed objectSeed)
		{
			Vector3I cellId = objectSeed.CellId;
			return ((((((((((((((long)Math.Abs(cellId.X) * 397L) ^ Math.Abs(cellId.Y)) * 397) ^ Math.Abs(cellId.Z)) * 397) ^ (Math.Sign(cellId.X) + 240)) * 397) ^ (Math.Sign(cellId.Y) + 312)) * 397) ^ (Math.Sign(cellId.Z) + 462)) * 397) ^ ((long)objectSeed.Params.Index * 16785407L)) & 0xFFFFFFFFFFFFFF) | 0x700000000000000;
		}

		private static Vector3I GetPlanetVoxelSize(double size)
		{
			return new Vector3I(Math.Max(64, (int)Math.Ceiling(size)));
		}

		protected override void CloseObjectSeed(MyObjectSeed objectSeed)
		{
			IMyAsteroidFieldDensityFunction myAsteroidFieldDensityFunction = objectSeed.UserData as IMyAsteroidFieldDensityFunction;
			if (myAsteroidFieldDensityFunction != null)
			{
				ChildrenRemoveDensityFunctionRemoved(myAsteroidFieldDensityFunction);
			}
			BoundingBoxD box = objectSeed.BoundingVolume;
			MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, m_tmpVoxelMapsList);
			string b = $"{objectSeed.Params.Type}_{objectSeed.CellId.X}_{objectSeed.CellId.Y}_{objectSeed.CellId.Z}_{objectSeed.Params.Index}_{objectSeed.Params.Seed}";
			foreach (MyVoxelBase tmpVoxelMaps in m_tmpVoxelMapsList)
			{
				if (tmpVoxelMaps.StorageName == b)
				{
					if (!tmpVoxelMaps.Save)
					{
						tmpVoxelMaps.Close();
					}
					break;
				}
			}
			m_tmpVoxelMapsList.Clear();
		}
	}
}
