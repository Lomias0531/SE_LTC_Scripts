using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Voxels;
using VRage.Library.Memory;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_Planet), true)]
	public class MyPlanet : MyVoxelBase, IMyOxygenProvider
	{
		private class Sandbox_Game_Entities_MyPlanet_003C_003EActor : IActivator, IActivator<MyPlanet>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanet();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanet CreateInstance()
			{
				return new MyPlanet();
			}

			MyPlanet IActivator<MyPlanet>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static MyMemorySystem MemoryTracker = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("Planets");

		public const int PHYSICS_SECTOR_SIZE_METERS = 1024;

		private const double INTRASECTOR_OBJECT_CLUSTER_SIZE = 512.0;

		public static bool RUN_SECTORS = false;

		private List<BoundingBoxD> m_clustersIntersection = new List<BoundingBoxD>();

		private MyConcurrentDictionary<Vector3I, MyVoxelPhysics> m_physicsShapes;

		private HashSet<Vector3I> m_sectorsPhysicsToRemove = new HashSet<Vector3I>();

		private Vector3I m_numCells;

		private bool m_canSpawnSectors = true;

		private MyPlanetInitArguments m_planetInitValues;

		private List<MyEntity> m_entities = new List<MyEntity>();

		private MyDynamicAABBTreeD m_children;

		public float AtmosphereAltitude
		{
			get;
			private set;
		}

		public MyPlanetStorageProvider Provider
		{
			get;
			private set;
		}

		public override MyVoxelBase RootVoxel => this;

		public MyPlanetGeneratorDefinition Generator
		{
			get;
			private set;
		}

		public new VRage.Game.Voxels.IMyStorage Storage
		{
			get
			{
				return base.m_storage;
			}
			set
			{
				bool flag = false;
				if (base.m_storage != null)
				{
					base.m_storage.RangeChanged -= storage_RangeChangedPlanet;
					flag = true;
				}
				base.m_storage = value;
				base.m_storage.RangeChanged += storage_RangeChangedPlanet;
				m_storageMax = base.m_storage.Size;
				if (flag)
				{
					ClearPhysicsShapes();
					(base.Render as MyRenderComponentVoxelMap).Clipmap.InvalidateAll();
				}
			}
		}

		public override Vector3D PositionLeftBottomCorner
		{
			get
			{
				return base.PositionLeftBottomCorner;
			}
			set
			{
				if (value != base.PositionLeftBottomCorner)
				{
					base.PositionLeftBottomCorner = value;
					if (m_physicsShapes != null)
					{
						foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in m_physicsShapes)
						{
							if (physicsShape.Value != null)
							{
								Vector3D vector3D = PositionLeftBottomCorner + physicsShape.Key * 1024 * 1f;
								physicsShape.Value.PositionLeftBottomCorner = vector3D;
								physicsShape.Value.PositionComp.SetPosition(vector3D + physicsShape.Value.Size * 0.5f);
							}
						}
					}
				}
			}
		}

		public MyPlanetInitArguments GetInitArguments => m_planetInitValues;

		public Vector3 AtmosphereWavelengths => m_planetInitValues.AtmosphereWavelengths;

		public MyAtmosphereSettings AtmosphereSettings
		{
			get
			{
				return m_planetInitValues.AtmosphereSettings;
			}
			set
			{
				m_planetInitValues.AtmosphereSettings = value;
				(base.Render as MyRenderComponentPlanet).UpdateAtmosphereSettings(value);
			}
		}

		public float MinimumRadius
		{
			get
			{
				if (Provider == null)
				{
					return 0f;
				}
				return Provider.Shape.InnerRadius;
			}
		}

		public float AverageRadius
		{
			get
			{
				if (Provider == null)
				{
					return 0f;
				}
				return Provider.Shape.Radius;
			}
		}

		public float MaximumRadius
		{
			get
			{
				if (Provider == null)
				{
					return 0f;
				}
				return Provider.Shape.OuterRadius;
			}
		}

		public float AtmosphereRadius => m_planetInitValues.AtmosphereRadius;

		public bool HasAtmosphere => m_planetInitValues.HasAtmosphere;

		public bool SpherizeWithDistance => m_planetInitValues.SpherizeWithDistance;

		public override MyClipmapScaleEnum ScaleGroup => MyClipmapScaleEnum.Massive;

		bool IMyOxygenProvider.IsPositionInRange(Vector3D worldPoint)
		{
			if (Generator == null || !Generator.HasAtmosphere || !Generator.Atmosphere.Breathable)
			{
				return false;
			}
			return (base.WorldMatrix.Translation - worldPoint).Length() < (double)(AtmosphereAltitude + AverageRadius);
		}

		public float GetOxygenForPosition(Vector3D worldPoint)
		{
			if (Generator == null)
			{
				return 0f;
			}
			if (Generator.Atmosphere.Breathable)
			{
				return GetAirDensity(worldPoint) * Generator.Atmosphere.OxygenDensity;
			}
			return 0f;
		}

		public float GetAirDensity(Vector3D worldPosition)
		{
			if (Generator == null)
			{
				return 0f;
			}
			if (Generator.HasAtmosphere)
			{
				double num = (worldPosition - base.WorldMatrix.Translation).Length();
				return (float)MathHelper.Clamp(1.0 - (num - (double)AverageRadius) / (double)AtmosphereAltitude, 0.0, 1.0) * Generator.Atmosphere.Density;
			}
			return 0f;
		}

		public float GetWindSpeed(Vector3D worldPosition)
		{
			if (Generator == null)
			{
				return 0f;
			}
			float airDensity = GetAirDensity(worldPosition);
			return Generator.Atmosphere.MaxWindSpeed * airDensity;
		}

		public void AddToStationOreBlockTree(ref MyDynamicAABBTree stationOreBlockTree, Vector3D position, float radius)
		{
			MyVoxelBase rootVoxel = RootVoxel;
			Vector3 value = new Vector3(radius);
			Vector3D worldPosition = position - value;
			Vector3D worldPosition2 = position + value;
			MyVoxelCoordSystems.WorldPositionToLocalPosition(rootVoxel.PositionLeftBottomCorner, ref worldPosition, out Vector3 localPosition);
			MyVoxelCoordSystems.WorldPositionToLocalPosition(rootVoxel.PositionLeftBottomCorner, ref worldPosition2, out Vector3 localPosition2);
			BoundingBox aabb = new BoundingBox(localPosition, localPosition2);
			stationOreBlockTree.AddProxy(ref aabb, null, 0u);
		}

		public void SetStationOreBlockTree(MyDynamicAABBTree tree)
		{
			Provider.Material.SetStationOreBlockTree(tree);
			(Storage.DataProvider as MyPlanetStorageProvider)?.Material.SetStationOreBlockTree(tree);
		}

		internal void VoxelStorageUpdated()
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			SetStationOreBlockTree(component.GetStationBlockTree(base.EntityId));
		}

		public MyPlanet()
		{
			(base.PositionComp as MyPositionComponent).WorldPositionChanged = base.WorldPositionChanged;
			base.Render = new MyRenderComponentPlanet();
			AddDebugRenderComponent(new MyDebugRenderComponentPlanet(this));
			base.Render.DrawOutsideViewDistance = true;
		}

		public override void Init(MyObjectBuilder_EntityBase builder)
		{
			Init(builder, null);
		}

		public override void Init(MyObjectBuilder_EntityBase builder, VRage.Game.Voxels.IMyStorage storage)
		{
			base.SyncFlag = true;
			base.Init(builder);
			MyObjectBuilder_Planet myObjectBuilder_Planet = (MyObjectBuilder_Planet)builder;
			if (myObjectBuilder_Planet == null)
			{
				return;
			}
			MyLog.Default.WriteLine("Planet init info - MutableStorage:" + myObjectBuilder_Planet.MutableStorage + " StorageName:" + myObjectBuilder_Planet.StorageName + " storage?:" + (storage != null));
			if (myObjectBuilder_Planet.MutableStorage)
			{
				base.StorageName = myObjectBuilder_Planet.StorageName;
			}
			else
			{
				base.StorageName = $"{myObjectBuilder_Planet.StorageName}";
			}
			m_planetInitValues.Seed = myObjectBuilder_Planet.Seed;
			m_planetInitValues.StorageName = base.StorageName;
			m_planetInitValues.PositionMinCorner = myObjectBuilder_Planet.PositionAndOrientation.Value.Position;
			m_planetInitValues.HasAtmosphere = myObjectBuilder_Planet.HasAtmosphere;
			m_planetInitValues.AtmosphereRadius = myObjectBuilder_Planet.AtmosphereRadius;
			m_planetInitValues.AtmosphereWavelengths = myObjectBuilder_Planet.AtmosphereWavelengths;
			m_planetInitValues.GravityFalloff = myObjectBuilder_Planet.GravityFalloff;
			m_planetInitValues.MarkAreaEmpty = myObjectBuilder_Planet.MarkAreaEmpty;
			m_planetInitValues.SurfaceGravity = myObjectBuilder_Planet.SurfaceGravity;
			m_planetInitValues.AddGps = myObjectBuilder_Planet.ShowGPS;
			m_planetInitValues.SpherizeWithDistance = myObjectBuilder_Planet.SpherizeWithDistance;
			m_planetInitValues.Generator = ((myObjectBuilder_Planet.PlanetGenerator == "") ? null : MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(myObjectBuilder_Planet.PlanetGenerator)));
			if (m_planetInitValues.Generator == null)
			{
				string text = "No definition found for planet generator " + myObjectBuilder_Planet.PlanetGenerator + ".";
				MyLog.Default.WriteLine(text);
				throw new MyIncompatibleDataException(text);
			}
			if (!MyPlanets.Static.CanSpawnPlanet(m_planetInitValues.Generator, out string errorMessage))
			{
				throw new Exception(errorMessage);
			}
			m_planetInitValues.AtmosphereSettings = (m_planetInitValues.Generator.AtmosphereSettings ?? MyAtmosphereSettings.Defaults());
			m_planetInitValues.UserCreated = false;
			if (storage != null)
			{
				m_planetInitValues.Storage = storage;
			}
			else
			{
				m_planetInitValues.Storage = MyStorageBase.Load(myObjectBuilder_Planet.StorageName, cache: false);
				if (m_planetInitValues.Storage == null)
				{
					string text2 = "No storage loaded for planet " + myObjectBuilder_Planet.StorageName + ".";
					MyLog.Default.WriteLine(text2);
					throw new MyIncompatibleDataException(text2);
				}
			}
			m_planetInitValues.InitializeComponents = false;
			MyLog.Default.Log(MyLogSeverity.Info, "Planet generator name: {0}", myObjectBuilder_Planet.PlanetGenerator ?? "<null>");
			Init(m_planetInitValues);
		}

		public void Init(MyPlanetInitArguments arguments)
		{
			base.SyncFlag = true;
			m_planetInitValues = arguments;
			MyLog.Default.Log(MyLogSeverity.Info, "Planet init values: {0}", m_planetInitValues.ToString());
			if (m_planetInitValues.Storage == null)
			{
				MyLog.Default.Log(MyLogSeverity.Error, "MyPlanet.Init: Planet storage is null! Init of the planet was cancelled.");
				return;
			}
			Provider = (m_planetInitValues.Storage.DataProvider as MyPlanetStorageProvider);
			if (Provider == null)
			{
				MyLog.Default.Error("MyPlanet.Init: Planet storage provider is null! Init of the planet was cancelled.");
				return;
			}
			if (arguments.Generator == null)
			{
				MyLog.Default.Error("MyPlanet.Init: Planet generator is null! Init of the planet was cancelled.");
				return;
			}
			if (!MyPlanets.Static.CanSpawnPlanet(arguments.Generator, out string errorMessage))
			{
				throw new Exception(errorMessage);
			}
			m_planetInitValues.Radius = Provider.Radius;
			m_planetInitValues.MaxRadius = Provider.Shape.OuterRadius;
			m_planetInitValues.MinRadius = Provider.Shape.InnerRadius;
			Generator = arguments.Generator;
			AtmosphereAltitude = Provider.Shape.MaxHillHeight * ((Generator != null) ? Generator.Atmosphere.LimitAltitude : 1f);
			Init(m_planetInitValues.StorageName, m_planetInitValues.Storage, m_planetInitValues.PositionMinCorner);
			((MyStorageBase)Storage).InitWriteCache();
			base.NeedsUpdate |= (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
			base.m_storage.RangeChanged += storage_RangeChangedPlanet;
			if (m_planetInitValues.MarkAreaEmpty && MyProceduralWorldGenerator.Static != null)
			{
				MyProceduralWorldGenerator.Static.MarkEmptyArea(base.PositionComp.GetPosition(), m_planetInitValues.MaxRadius);
			}
			if (base.Physics != null)
			{
				base.Physics.Enabled = false;
				base.Physics.Close();
				base.Physics = null;
			}
			if (Name == null)
			{
				Name = base.StorageName + "-" + base.EntityId;
			}
			Vector3I size = m_planetInitValues.Storage.Size;
			m_numCells = new Vector3I(size.X / 1024, size.Y / 1024, size.Z / 1024);
			m_numCells -= 1;
			m_numCells = Vector3I.Max(Vector3I.Zero, m_numCells);
			base.StorageName = m_planetInitValues.StorageName;
			m_storageMax = m_planetInitValues.Storage.Size;
			PrepareSectors();
			if (Generator != null && Generator.EnvironmentDefinition != null)
			{
				if (!base.Components.Contains(typeof(MyPlanetEnvironmentComponent)))
				{
					base.Components.Add(new MyPlanetEnvironmentComponent());
				}
				base.Components.Get<MyPlanetEnvironmentComponent>().InitEnvironment();
			}
			base.Components.Add((MyGravityProviderComponent)new MySphericalNaturalGravityComponent(m_planetInitValues.MinRadius, m_planetInitValues.MaxRadius, m_planetInitValues.GravityFalloff, m_planetInitValues.SurfaceGravity));
			base.CreatedByUser = m_planetInitValues.UserCreated;
			base.Render.FadeIn = m_planetInitValues.FadeIn;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_Planet obj = (MyObjectBuilder_Planet)base.GetObjectBuilder(copy);
			obj.Radius = m_planetInitValues.Radius;
			obj.Seed = m_planetInitValues.Seed;
			obj.HasAtmosphere = m_planetInitValues.HasAtmosphere;
			obj.AtmosphereRadius = m_planetInitValues.AtmosphereRadius;
			obj.MinimumSurfaceRadius = m_planetInitValues.MinRadius;
			obj.MaximumHillRadius = m_planetInitValues.MaxRadius;
			obj.AtmosphereWavelengths = m_planetInitValues.AtmosphereWavelengths;
			obj.GravityFalloff = m_planetInitValues.GravityFalloff;
			obj.MarkAreaEmpty = m_planetInitValues.MarkAreaEmpty;
			obj.AtmosphereSettings = m_planetInitValues.AtmosphereSettings;
			obj.SurfaceGravity = m_planetInitValues.SurfaceGravity;
			obj.ShowGPS = m_planetInitValues.AddGps;
			obj.SpherizeWithDistance = m_planetInitValues.SpherizeWithDistance;
			MyPlanetGeneratorDefinition generator = Generator;
			object planetGenerator;
			if (generator == null)
			{
				planetGenerator = null;
			}
			else
			{
				MyStringHash subtypeId = generator.Id.SubtypeId;
				planetGenerator = subtypeId.ToString();
			}
			obj.PlanetGenerator = (string)planetGenerator;
			return obj;
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_physicsShapes != null)
			{
				foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in m_physicsShapes)
				{
					if (physicsShape.Value != null)
					{
						physicsShape.Value.Close();
					}
				}
			}
		}

		protected override void BeforeDelete()
		{
			base.BeforeDelete();
			if (m_physicsShapes != null)
			{
				foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in m_physicsShapes)
				{
					if (physicsShape.Value != null)
					{
						MySession.Static.VoxelMaps.RemoveVoxelMap(physicsShape.Value);
						physicsShape.Value.RemoveFromGamePruningStructure();
					}
				}
			}
			MySession.Static.VoxelMaps.RemoveVoxelMap(this);
			if (base.m_storage != null)
			{
				base.m_storage.RangeChanged -= storage_RangeChangedPlanet;
				base.m_storage.Close();
				base.m_storage = null;
			}
			Provider = null;
			m_planetInitValues = default(MyPlanetInitArguments);
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			MyPlanets.Register(this);
			MyGravityProviderSystem.AddNaturalGravityProvider(base.Components.Get<MyGravityProviderComponent>());
			MyOxygenProviderSystem.AddOxygenGenerator(this);
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			MyPlanets.UnRegister(this);
			MyGravityProviderSystem.RemoveNaturalGravityProvider(base.Components.Get<MyGravityProviderComponent>());
			MyOxygenProviderSystem.RemoveOxygenGenerator(this);
		}

		private void storage_RangeChangedPlanet(Vector3I minChanged, Vector3I maxChanged, MyStorageDataTypeFlags dataChanged)
		{
			Vector3I start = minChanged / 1024;
			Vector3I end = maxChanged / 1024;
			if (m_physicsShapes != null)
			{
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				while (vector3I_RangeIterator.IsValid())
				{
					if (m_physicsShapes.TryGetValue(vector3I_RangeIterator.Current, out MyVoxelPhysics value))
					{
						value?.OnStorageChanged(minChanged, maxChanged, dataChanged);
					}
					vector3I_RangeIterator.MoveNext();
				}
			}
			if (base.Render is MyRenderComponentVoxelMap)
			{
				(base.Render as MyRenderComponentVoxelMap).InvalidateRange(minChanged, maxChanged);
			}
			OnRangeChanged(minChanged, maxChanged, dataChanged);
			base.ContentChanged = true;
		}

		private MyVoxelPhysics CreateVoxelPhysics(ref Vector3I increment, ref Vector3I_RangeIterator it)
		{
			if (m_physicsShapes == null)
			{
				m_physicsShapes = new MyConcurrentDictionary<Vector3I, MyVoxelPhysics>();
			}
			MyVoxelPhysics value = null;
			if (!m_physicsShapes.TryGetValue(it.Current, out value) || value == null)
			{
				Vector3I vector3I = it.Current * increment;
				Vector3I vector3I2 = vector3I + increment;
				BoundingBox box = new BoundingBox(vector3I, vector3I2);
				if (Storage.Intersect(ref box, lazy: false) == ContainmentType.Intersects)
				{
					value = new MyVoxelPhysics();
					value.Init(base.m_storage, PositionLeftBottomCorner + vector3I * 1f, vector3I, vector3I2, this);
					value.Save = false;
					MyEntities.Add(value);
				}
				m_physicsShapes[it.Current] = value;
			}
			else if (value != null && !value.Valid)
			{
				value.RefreshPhysics(base.m_storage);
			}
			return value;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateFloraAndPhysics(serial: true);
			if (m_planetInitValues.AddGps)
			{
				MyGps gps = new MyGps
				{
					Name = base.StorageName,
					Coords = base.PositionComp.GetPosition(),
					ShowOnHud = true
				};
				gps.UpdateHash();
				MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps, 0L);
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			UpdateFloraAndPhysics();
		}

		public override void BeforePaste()
		{
		}

		public override void AfterPaste()
		{
		}

		private void UpdateFloraAndPhysics(bool serial = false)
		{
			BoundingBoxD box = base.PositionComp.WorldAABB;
			box.Min -= 1024.0;
			box.Max += 1024f;
			UpdatePlanetPhysics(ref box);
		}

		private void UpdatePlanetPhysics(ref BoundingBoxD box)
		{
			Vector3I increment = base.m_storage.Size / (m_numCells + 1);
			MyGamePruningStructure.GetAproximateDynamicClustersForSize(ref box, 512.0, m_clustersIntersection);
			foreach (BoundingBoxD item in m_clustersIntersection)
			{
				BoundingBoxD shapeBox = item;
				shapeBox.Inflate(32.0);
				GeneratePhysicalShapeForBox(ref increment, ref shapeBox);
			}
			if (MySession.Static.ControlledEntity != null)
			{
				BoundingBoxD shapeBox2 = MySession.Static.ControlledEntity.Entity.PositionComp.WorldAABB;
				shapeBox2.Inflate(32.0);
				GeneratePhysicalShapeForBox(ref increment, ref shapeBox2);
			}
			m_clustersIntersection.Clear();
		}

		private void GeneratePhysicalShapeForBox(ref Vector3I increment, ref BoundingBoxD shapeBox)
		{
			if (shapeBox.Intersects(base.PositionComp.WorldAABB))
			{
				if (!shapeBox.Valid || !shapeBox.Min.IsValid() || !shapeBox.Max.IsValid())
				{
					string message = "Invalid shapeBox: " + shapeBox;
					throw new ArgumentOutOfRangeException("shapeBox", message);
				}
				MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref shapeBox.Min, out Vector3I voxelCoord);
				MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref shapeBox.Max, out Vector3I voxelCoord2);
				voxelCoord /= 1024;
				voxelCoord2 /= 1024;
				Vector3I_RangeIterator it = new Vector3I_RangeIterator(ref voxelCoord, ref voxelCoord2);
				while (it.IsValid())
				{
					CreateVoxelPhysics(ref increment, ref it);
					it.MoveNext();
				}
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (m_physicsShapes != null)
			{
				foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in m_physicsShapes)
				{
					BoundingBoxD box;
					if (physicsShape.Value != null)
					{
						box = physicsShape.Value.PositionComp.WorldAABB;
						box.Min -= box.HalfExtents;
						box.Max += box.HalfExtents;
					}
					else
					{
						Vector3 vector = (Vector3)physicsShape.Key * 1024f + PositionLeftBottomCorner;
						box = new BoundingBoxD(vector, vector + 1024f);
					}
					bool flag = false;
					using (MyUtils.ReuseCollection(ref m_entities))
					{
						MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, m_entities);
						foreach (MyEntity entity in m_entities)
						{
							if (entity.Physics != null)
							{
								if (entity.Physics.IsStatic)
								{
									MyCubeGrid myCubeGrid = entity as MyCubeGrid;
									if (myCubeGrid != null && !myCubeGrid.IsStatic)
									{
										flag = true;
									}
								}
								else
								{
									flag = true;
								}
							}
						}
					}
					if (!flag)
					{
						m_sectorsPhysicsToRemove.Add(physicsShape.Key);
					}
				}
				foreach (Vector3I item in m_sectorsPhysicsToRemove)
				{
					if (m_physicsShapes.TryGetValue(item, out MyVoxelPhysics value))
					{
						value?.Close();
					}
					m_physicsShapes.Remove(item);
				}
				m_sectorsPhysicsToRemove.Clear();
			}
		}

		public void ClearPhysicsShapes()
		{
			if (m_physicsShapes != null)
			{
				foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in m_physicsShapes)
				{
					if (physicsShape.Value != null)
					{
						physicsShape.Value.Valid = false;
					}
				}
			}
		}

		public bool CorrectSpawnLocation2(ref Vector3D position, double radius, bool resumeSearch = false)
		{
			Vector3D value = position - base.WorldMatrix.Translation;
			value.Normalize();
			Vector3D value2 = new Vector3D(radius, radius, radius);
			Vector3D worldPosition;
			Vector3 localPosition;
			if (resumeSearch)
			{
				worldPosition = position;
			}
			else
			{
				MyVoxelCoordSystems.WorldPositionToLocalPosition(PositionLeftBottomCorner, ref position, out localPosition);
				BoundingBox box = new BoundingBox(localPosition - value2, localPosition + value2);
				if (Storage.Intersect(ref box) == ContainmentType.Disjoint)
				{
					return true;
				}
				worldPosition = GetClosestSurfacePointGlobal(ref position);
			}
			for (int i = 0; i < 10; i++)
			{
				worldPosition += value * radius;
				MyVoxelCoordSystems.WorldPositionToLocalPosition(PositionLeftBottomCorner, ref worldPosition, out localPosition);
				BoundingBox box = new BoundingBox(localPosition - value2, localPosition + value2);
				if (Storage.Intersect(ref box) == ContainmentType.Disjoint)
				{
					position = worldPosition;
					return true;
				}
			}
			return false;
		}

		public void CorrectSpawnLocation(ref Vector3D position, double radius)
		{
			Vector3D value = position - base.WorldMatrix.Translation;
			value.Normalize();
			MyVoxelCoordSystems.WorldPositionToLocalPosition(PositionLeftBottomCorner, ref position, out Vector3 localPosition);
			Vector3D value2 = new Vector3D(radius, radius, radius);
			BoundingBox box = new BoundingBox(localPosition - value2, localPosition + value2);
			ContainmentType containmentType = Storage.Intersect(ref box);
			for (int i = 0; i < 10; i++)
			{
				if (containmentType != ContainmentType.Intersects && containmentType != ContainmentType.Contains)
				{
					break;
				}
				Vector3D closestSurfacePointGlobal = GetClosestSurfacePointGlobal(ref position);
				position = closestSurfacePointGlobal + value * radius;
				MyVoxelCoordSystems.WorldPositionToLocalPosition(PositionLeftBottomCorner, ref position, out localPosition);
				box = new BoundingBox(localPosition - value2, localPosition + value2);
				containmentType = Storage.Intersect(ref box);
			}
		}

		public Vector3D GetClosestSurfacePointGlobal(Vector3D globalPos)
		{
			return GetClosestSurfacePointGlobal(ref globalPos);
		}

		public Vector3D GetClosestSurfacePointGlobal(ref Vector3D globalPos)
		{
			Vector3D translation = base.WorldMatrix.Translation;
			Vector3 localPos = globalPos - translation;
			return GetClosestSurfacePointLocal(ref localPos) + translation;
		}

		public Vector3D GetClosestSurfacePointLocal(ref Vector3 localPos)
		{
			if (!localPos.IsValid())
			{
				return Vector3D.Zero;
			}
			Provider.Shape.ProjectToSurface(localPos, out Vector3 surface);
			return surface;
		}

		public override void DebugDrawPhysics()
		{
			if (m_physicsShapes != null)
			{
				foreach (KeyValuePair<Vector3I, MyVoxelPhysics> physicsShape in m_physicsShapes)
				{
					Vector3 vector = (Vector3)physicsShape.Key * 1024f + PositionLeftBottomCorner;
					BoundingBoxD aabb = new BoundingBoxD(vector, vector + 1024f);
					if (physicsShape.Value != null && !physicsShape.Value.Closed)
					{
						physicsShape.Value.Physics.DebugDraw();
						MyRenderProxy.DebugDrawAABB(aabb, Color.Cyan);
					}
					else
					{
						MyRenderProxy.DebugDrawAABB(aabb, Color.DarkGreen);
					}
				}
			}
		}

		public override int GetOrePriority()
		{
			return -1;
		}

		public int GetInstanceHash()
		{
			return Name.GetHashCode();
		}

		public void PrefetchShapeOnRay(ref LineD ray)
		{
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref ray.From, out Vector3I voxelCoord);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref ray.To, out Vector3I voxelCoord2);
			voxelCoord /= 1024;
			voxelCoord2 /= 1024;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref voxelCoord, ref voxelCoord2);
			while (vector3I_RangeIterator.IsValid())
			{
				if (m_physicsShapes.ContainsKey(vector3I_RangeIterator.Current))
				{
					m_physicsShapes[vector3I_RangeIterator.Current].PrefetchShapeOnRay(ref ray);
				}
				vector3I_RangeIterator.MoveNext();
			}
		}

		public bool IntersectsWithGravityFast(ref BoundingBoxD boundingBox)
		{
			Vector3D position = base.PositionComp.GetPosition();
			float gravityLimit = ((MySphericalNaturalGravityComponent)base.Components.Get<MyGravityProviderComponent>()).GravityLimit;
			new BoundingSphereD(position, gravityLimit).Contains(ref boundingBox, out ContainmentType result);
			return result != ContainmentType.Disjoint;
		}

		private void PrepareSectors()
		{
			m_children = new MyDynamicAABBTreeD(Vector3D.Zero);
			base.Hierarchy.QueryAABBImpl = Hierarchy_QueryAABB;
			base.Hierarchy.QueryLineImpl = Hierarchy_QueryLine;
			base.Hierarchy.QuerySphereImpl = Hierarchy_QuerySphere;
		}

		private void Hierarchy_QueryAABB(BoundingBoxD query, List<MyEntity> results)
		{
			m_children.OverlapAllBoundingBox(ref query, results, 0u, clear: false);
		}

		private void Hierarchy_QuerySphere(BoundingSphereD query, List<MyEntity> results)
		{
			m_children.OverlapAllBoundingSphere(ref query, results, clear: false);
		}

		private void Hierarchy_QueryLine(LineD query, List<MyLineSegmentOverlapResult<MyEntity>> results)
		{
			m_children.OverlapAllLineSegment(ref query, results, clear: false);
		}

		public void AddChildEntity(MyEntity child)
		{
			if (MyFakes.ENABLE_PLANET_HIERARCHY)
			{
				BoundingBoxD aabb = child.PositionComp.WorldAABB;
				int num = m_children.AddProxy(ref aabb, child, 0u);
				base.Hierarchy.AddChild(child, preserveWorldPos: true);
				child.Components.Get<MyHierarchyComponentBase>().ChildId = num;
			}
			else
			{
				MyEntities.Add(child);
			}
		}

		public void RemoveChildEntity(MyEntity child)
		{
			if (MyFakes.ENABLE_PLANET_HIERARCHY && child.Parent == this)
			{
				MyHierarchyComponentBase myHierarchyComponentBase = child.Components.Get<MyHierarchyComponentBase>();
				m_children.RemoveProxy((int)myHierarchyComponentBase.ChildId);
				base.Hierarchy.RemoveChild(child, preserveWorldPos: true);
			}
		}

		internal void CloseChildEntity(MyEntity child)
		{
			RemoveChildEntity(child);
			child.Close();
		}
	}
}
