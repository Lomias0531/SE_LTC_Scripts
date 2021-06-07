using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Planet
{
	[MyComponentBuilder(typeof(MyObjectBuilder_PlanetEnvironmentComponent), true)]
	public class MyPlanetEnvironmentComponent : MyEntityComponentBase, IMy2DClipmapManager, IMyEnvironmentOwner
	{
		private struct Operation
		{
			public MyPlanetEnvironmentClipmapProxy Proxy;

			public int LodToSet;

			public bool ShouldClose;
		}

		private class Sandbox_Game_Entities_Planet_MyPlanetEnvironmentComponent_003C_003EActor : IActivator, IActivator<MyPlanetEnvironmentComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetEnvironmentComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetEnvironmentComponent CreateInstance()
			{
				return new MyPlanetEnvironmentComponent();
			}

			MyPlanetEnvironmentComponent IActivator<MyPlanetEnvironmentComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private List<BoundingBoxD> m_sectorBoxes = new List<BoundingBoxD>();

		private readonly My2DClipmap<MyPlanetEnvironmentClipmapProxy>[] m_clipmaps = new My2DClipmap<MyPlanetEnvironmentClipmapProxy>[6];

		internal My2DClipmap<MyPlanetEnvironmentClipmapProxy> ActiveClipmap;

		internal Dictionary<long, MyEnvironmentSector> PhysicsSectors = new Dictionary<long, MyEnvironmentSector>();

		internal Dictionary<long, MyEnvironmentSector> HeldSectors = new Dictionary<long, MyEnvironmentSector>();

		internal Dictionary<long, MyPlanetEnvironmentClipmapProxy> Proxies = new Dictionary<long, MyPlanetEnvironmentClipmapProxy>();

		internal Dictionary<long, MyPlanetEnvironmentClipmapProxy> OutgoingProxies = new Dictionary<long, MyPlanetEnvironmentClipmapProxy>();

		internal readonly IMyEnvironmentDataProvider[] Providers = new IMyEnvironmentDataProvider[6];

		private MyObjectBuilder_EnvironmentDataProvider[] m_providerData = new MyObjectBuilder_EnvironmentDataProvider[6];

		private float m_cachedVegetationDrawDistance;

		private readonly ManualResetEvent m_parallelSyncPoint = new ManualResetEvent(initialState: true);

		private const long ParallelWorkTimeMilliseconds = 100L;

		private const int SequentialWorkCount = 10;

		private bool m_parallelInProgress;

		private readonly HashSet<MyEnvironmentSector> m_sectorsClosing = new HashSet<MyEnvironmentSector>();

		private readonly List<MyEnvironmentSector> m_sectorsClosed = new List<MyEnvironmentSector>();

		private readonly MyIterableComplementSet<MyEnvironmentSector> m_sectorsWithPhysics = new MyIterableComplementSet<MyEnvironmentSector>();

		private readonly MyConcurrentQueue<MyEnvironmentSector> m_sectorsToWorkParallel = new MyConcurrentQueue<MyEnvironmentSector>(10);

		private readonly MyConcurrentQueue<MyEnvironmentSector> m_sectorsToWorkSerial = new MyConcurrentQueue<MyEnvironmentSector>(10);

		private readonly Action m_parallelWorkDelegate;

		private readonly Action m_serialWorkDelegate;

		private readonly Dictionary<long, Operation> m_sectorOperations = new Dictionary<long, Operation>();

		private readonly List<MyPhysicalModelDefinition> m_physicalModels = new List<MyPhysicalModelDefinition>();

		private readonly Dictionary<MyPhysicalModelDefinition, short> m_physicalModelToKey = new Dictionary<MyPhysicalModelDefinition, short>();

		private Dictionary<long, List<MyOrientedBoundingBoxD>> m_obstructorsPerSector;

		private int m_InstanceHash;

		internal int ActiveFace
		{
			get;
			private set;
		}

		internal MyPlanet Planet => (MyPlanet)base.Entity;

		internal Vector3D PlanetTranslation
		{
			get;
			private set;
		}

		public int MaxLod
		{
			get;
			private set;
		}

		public override string ComponentTypeDebugString => "Planet Environment Component";

		public MyWorldEnvironmentDefinition EnvironmentDefinition
		{
			get;
			private set;
		}

		MyEntity IMyEnvironmentOwner.Entity => Planet;

		public IMyEnvironmentDataProvider DataProvider => null;

		public bool CollisionCheckEnabled
		{
			get;
			private set;
		}

		public MyPlanetEnvironmentComponent()
		{
			m_parallelWorkDelegate = ParallelWorkCallback;
			m_serialWorkDelegate = SerialWorkCallback;
		}

		public void InitEnvironment()
		{
			EnvironmentDefinition = Planet.Generator.EnvironmentDefinition;
			PlanetTranslation = Planet.WorldMatrix.Translation;
			m_InstanceHash = Planet.GetInstanceHash();
			double num = (double)Planet.AverageRadius * Math.Sqrt(2.0);
			double num2 = num / 2.0;
			double sectorSize = EnvironmentDefinition.SectorSize;
			for (int i = 0; i < 6; i++)
			{
				MyPlanetCubemapHelper.GetForwardUp((Base6Directions.Direction)i, out Vector3D forward, out Vector3D up);
				Vector3D position = forward * num2 + PlanetTranslation;
				forward = -forward;
				MatrixD.CreateWorld(ref position, ref forward, ref up, out MatrixD result);
				Vector3D position2 = new Vector3D(0.0 - num2, 0.0 - num2, 0.0);
				Vector3D.Transform(ref position2, ref result, out position2);
				Vector3D vector = new Vector3D(1.0, 0.0, 0.0);
				Vector3D vector2 = new Vector3D(0.0, 1.0, 0.0);
				Vector3D.RotateAndScale(ref vector, ref result, out vector);
				Vector3D.RotateAndScale(ref vector2, ref result, out vector2);
				m_clipmaps[i] = new My2DClipmap<MyPlanetEnvironmentClipmapProxy>();
				ActiveClipmap = m_clipmaps[i];
				ActiveFace = i;
				m_clipmaps[i].Init(this, ref result, sectorSize, num);
				ActiveFace = -1;
				MyProceduralEnvironmentProvider myProceduralEnvironmentProvider = new MyProceduralEnvironmentProvider
				{
					ProviderId = i
				};
				myProceduralEnvironmentProvider.Init(this, ref position2, ref vector, ref vector2, ActiveClipmap.LeafSize, m_providerData[i]);
				Providers[i] = myProceduralEnvironmentProvider;
			}
		}

		public void Update(bool doLazyUpdates = true, bool forceUpdate = false)
		{
			int maxLod = MaxLod;
			float num = (!MySandboxGame.Config.VegetationDrawDistance.HasValue) ? 100f : MySandboxGame.Config.VegetationDrawDistance.Value;
			if (m_cachedVegetationDrawDistance != num)
			{
				m_cachedVegetationDrawDistance = num;
				MaxLod = MathHelper.Log2Floor((int)((double)num / EnvironmentDefinition.SectorSize + 0.5));
				if (MaxLod != maxLod)
				{
					for (int i = 0; i < m_clipmaps.Length; i++)
					{
						ActiveFace = i;
						ActiveClipmap = m_clipmaps[i];
						ActiveClipmap.Clear();
						ActiveClipmap.LastPosition = Vector3D.PositiveInfinity;
						EvaluateOperations();
					}
					ActiveFace = -1;
					ActiveClipmap = null;
				}
			}
			UpdateClipmaps();
			UpdatePhysics();
			if (doLazyUpdates)
			{
				LazyUpdate();
			}
			if (m_parallelInProgress)
			{
				return;
			}
			if (m_sectorsToWorkParallel.Count > 0)
			{
				if (forceUpdate)
				{
					MyEnvironmentSector instance;
					while (m_sectorsToWorkParallel.TryDequeue(out instance))
					{
						instance.DoParallelWork();
					}
					while (m_sectorsToWorkSerial.TryDequeue(out instance))
					{
						instance.DoSerialWork();
					}
				}
				else
				{
					m_parallelInProgress = true;
					Parallel.Start(m_parallelWorkDelegate, m_serialWorkDelegate);
				}
			}
			else if (m_sectorsToWorkSerial.Count > 0)
			{
				SerialWorkCallback();
			}
		}

		private void UpdateClipmaps()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated || m_sectorsToWorkParallel.Count > 0)
			{
				return;
			}
			Vector3D localPos = MySector.MainCamera.Position - PlanetTranslation;
			double num = localPos.Length();
			if (num > (double)Planet.AverageRadius + m_clipmaps[0].FaceHalf && Proxies.Count == 0)
			{
				return;
			}
			num = Math.Abs(Planet.Provider.Shape.GetDistanceToSurfaceCacheless(localPos));
			MyPlanetCubemapHelper.ProjectToCube(ref localPos, out int direction, out Vector2D texcoords);
			Vector3D localPosition = default(Vector3D);
			for (int i = 0; i < 6; i++)
			{
				ActiveFace = i;
				ActiveClipmap = m_clipmaps[i];
				MyPlanetCubemapHelper.TranslateTexcoordsToFace(ref texcoords, direction, i, out Vector2D newCoords);
				localPosition.X = newCoords.X * ActiveClipmap.FaceHalf;
				localPosition.Y = newCoords.Y * ActiveClipmap.FaceHalf;
				if ((i ^ direction) == 1)
				{
					localPosition.Z = num + (double)(Planet.AverageRadius * 2f);
				}
				else
				{
					localPosition.Z = num;
				}
				ActiveClipmap.Update(localPosition);
				EvaluateOperations();
			}
			ActiveFace = -1;
		}

		private void LazyUpdate()
		{
			foreach (MyEnvironmentSector item in m_sectorsWithPhysics.Set())
			{
				item.EnablePhysics(physics: false);
				PhysicsSectors.Remove(item.SectorId);
				if (!Proxies.ContainsKey(item.SectorId) && !OutgoingProxies.ContainsKey(item.SectorId) && !item.IsPinned)
				{
					m_sectorsClosing.Add(item);
				}
			}
			m_sectorsWithPhysics.ClearSet();
			m_sectorsWithPhysics.AllToSet();
			foreach (MyEnvironmentSector item2 in m_sectorsClosing)
			{
				if (!item2.HasWorkPending())
				{
					item2.Close();
					Planet.RemoveChildEntity(item2);
					m_sectorsClosed.Add(item2);
				}
				else
				{
					item2.CancelParallel();
					if (item2.HasSerialWorkPending)
					{
						item2.DoSerialWork();
					}
				}
			}
			foreach (MyEnvironmentSector item3 in m_sectorsClosed)
			{
				m_sectorsClosing.Remove(item3);
			}
			m_sectorsClosed.Clear();
		}

		public void DebugDraw()
		{
			if (MyPlanetEnvironmentSessionComponent.DebugDrawSectors && MyPlanetEnvironmentSessionComponent.DebugDrawDynamicObjectClusters)
			{
				using (IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(MatrixD.Identity, new Color(Color.Green, 0.2f)))
				{
					foreach (BoundingBoxD sectorBox in m_sectorBoxes)
					{
						BoundingBoxD aabb = sectorBox;
						myDebugDrawBatchAabb.Add(ref aabb);
					}
				}
			}
			if (MyPlanetEnvironmentSessionComponent.DebugDrawProxies)
			{
				foreach (MyPlanetEnvironmentClipmapProxy value in Proxies.Values)
				{
					value.DebugDraw();
				}
				foreach (MyPlanetEnvironmentClipmapProxy value2 in OutgoingProxies.Values)
				{
					value2.DebugDraw(outgoing: true);
				}
			}
			if (MyPlanetEnvironmentSessionComponent.DebugDrawCollisionCheckers && m_obstructorsPerSector != null)
			{
				foreach (List<MyOrientedBoundingBoxD> value3 in m_obstructorsPerSector.Values)
				{
					foreach (MyOrientedBoundingBoxD item in value3)
					{
						MyRenderProxy.DebugDrawOBB(item, Color.Red, 0.1f, depthRead: true, smooth: true);
					}
				}
			}
		}

		private void UpdatePhysics()
		{
			BoundingBoxD container = Planet.PositionComp.WorldAABB;
			container.Min -= 1024.0;
			container.Max += 1024f;
			m_sectorBoxes.Clear();
			MyGamePruningStructure.GetAproximateDynamicClustersForSize(ref container, EnvironmentDefinition.SectorSize / 2.0, m_sectorBoxes);
			foreach (BoundingBoxD sectorBox in m_sectorBoxes)
			{
				sectorBox.Translate(-PlanetTranslation);
				sectorBox.Inflate(EnvironmentDefinition.SectorSize / 2.0);
				double num = sectorBox.Center.Length();
				double num2 = sectorBox.Size.Length() / 2.0;
				if (num >= (double)Planet.MinimumRadius - num2 && num <= (double)Planet.MaximumRadius + num2)
				{
					RasterSectorsForPhysics(sectorBox);
				}
			}
		}

		private unsafe void RasterSectorsForPhysics(BoundingBoxD range)
		{
			range.InflateToMinimum(EnvironmentDefinition.SectorSize);
			Vector2I v = new Vector2I(1 << m_clipmaps[0].Depth) - 1;
			Vector3D* ptr = stackalloc Vector3D[8];
			range.GetCornersUnsafe(ptr);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 8; i++)
			{
				Vector3D localPos = ptr[i];
				int num3 = MyPlanetCubemapHelper.FindCubeFace(ref localPos);
				num2 = num3;
				num3 = 1 << num3;
				if ((num & ~num3) != 0)
				{
					num |= 0x40;
				}
				num |= num3;
			}
			int num4 = 0;
			int num5 = 5;
			if ((num & 0x40) == 0)
			{
				num4 = (num5 = num2);
			}
			for (int j = num4; j <= num5; j++)
			{
				if (((1 << j) & num) == 0)
				{
					continue;
				}
				_ = m_clipmaps[j].LeafSize;
				int num6 = 1 << m_clipmaps[j].Depth - 1;
				BoundingBox2D boundingBox2D = BoundingBox2D.CreateInvalid();
				for (int k = 0; k < 8; k++)
				{
					Vector3D localPos2 = ptr[k];
					MyPlanetCubemapHelper.ProjectForFace(ref localPos2, j, out Vector2D normalCoord);
					boundingBox2D.Include(normalCoord);
				}
				boundingBox2D.Min += 1.0;
				boundingBox2D.Min *= (double)num6;
				boundingBox2D.Max += 1.0;
				boundingBox2D.Max *= (double)num6;
				Vector2I v2 = new Vector2I((int)boundingBox2D.Min.X, (int)boundingBox2D.Min.Y);
				Vector2I v3 = new Vector2I((int)boundingBox2D.Max.X, (int)boundingBox2D.Max.Y);
				Vector2I.Max(ref v2, ref Vector2I.Zero, out v2);
				Vector2I.Min(ref v3, ref v, out v3);
				for (int l = v2.X; l <= v3.X; l++)
				{
					for (int m = v2.Y; m <= v3.Y; m++)
					{
						EnsurePhysicsSector(l, m, j);
					}
				}
			}
		}

		private void EnsurePhysicsSector(int x, int y, int face)
		{
			long key = MyPlanetSectorId.MakeSectorId(x, y, face);
			if (!PhysicsSectors.TryGetValue(key, out MyEnvironmentSector value))
			{
				if (Proxies.TryGetValue(key, out MyPlanetEnvironmentClipmapProxy value2))
				{
					value = value2.EnvironmentSector;
					value.EnablePhysics(physics: true);
				}
				else if (!HeldSectors.TryGetValue(key, out value))
				{
					value = EnvironmentDefinition.CreateSector();
					MyEnvironmentSectorParameters parameters = default(MyEnvironmentSectorParameters);
					double leafSize = m_clipmaps[face].LeafSize;
					double num = m_clipmaps[face].LeafSize / 2.0;
					int num2 = 1 << m_clipmaps[face].Depth - 1;
					Matrix matrix = m_clipmaps[face].WorldMatrix;
					parameters.SurfaceBasisX = new Vector3(num, 0.0, 0.0);
					Vector3.RotateAndScale(ref parameters.SurfaceBasisX, ref matrix, out parameters.SurfaceBasisX);
					parameters.SurfaceBasisY = new Vector3(0.0, num, 0.0);
					Vector3.RotateAndScale(ref parameters.SurfaceBasisY, ref matrix, out parameters.SurfaceBasisY);
					parameters.Environment = EnvironmentDefinition;
					parameters.Center = Vector3D.Transform(new Vector3D(((double)(x - num2) + 0.5) * leafSize, ((double)(y - num2) + 0.5) * leafSize, 0.0), m_clipmaps[face].WorldMatrix);
					parameters.DataRange = new BoundingBox2I(new Vector2I(x, y), new Vector2I(x, y));
					parameters.Provider = Providers[face];
					parameters.EntityId = MyPlanetSectorId.MakeSectorEntityId(x, y, 0, face, Planet.EntityId);
					parameters.SectorId = MyPlanetSectorId.MakeSectorId(x, y, face);
					parameters.Bounds = GetBoundingShape(ref parameters.Center, ref parameters.SurfaceBasisX, ref parameters.SurfaceBasisY);
					value.Init(this, ref parameters);
					value.EnablePhysics(physics: true);
					Planet.AddChildEntity(value);
				}
				PhysicsSectors.Add(key, value);
			}
			m_sectorsWithPhysics.AddOrEnsureOnComplement(value);
		}

		public override void OnAddedToScene()
		{
			MySession.Static.GetComponent<MyPlanetEnvironmentSessionComponent>().RegisterPlanetEnvironment(this);
		}

		public override void OnRemovedFromScene()
		{
			MySession.Static.GetComponent<MyPlanetEnvironmentSessionComponent>().UnregisterPlanetEnvironment(this);
			CloseAll();
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_PlanetEnvironmentComponent myObjectBuilder_PlanetEnvironmentComponent = new MyObjectBuilder_PlanetEnvironmentComponent();
			myObjectBuilder_PlanetEnvironmentComponent.DataProviders = new MyObjectBuilder_EnvironmentDataProvider[Providers.Length];
			for (int i = 0; i < Providers.Length; i++)
			{
				myObjectBuilder_PlanetEnvironmentComponent.DataProviders[i] = Providers[i].GetObjectBuilder();
				myObjectBuilder_PlanetEnvironmentComponent.DataProviders[i].Face = (Base6Directions.Direction)i;
			}
			if (CollisionCheckEnabled && m_obstructorsPerSector.Count > 0)
			{
				myObjectBuilder_PlanetEnvironmentComponent.SectorObstructions = new List<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox>();
				{
					foreach (KeyValuePair<long, List<MyOrientedBoundingBoxD>> item2 in m_obstructorsPerSector)
					{
						MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox obstructingBox = default(MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox);
						obstructingBox.SectorId = item2.Key;
						MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox item = obstructingBox;
						item.ObstructingBoxes = new List<SerializableOrientedBoundingBoxD>();
						if (item2.Value != null)
						{
							foreach (MyOrientedBoundingBoxD item3 in item2.Value)
							{
								item.ObstructingBoxes.Add(item3);
							}
						}
						myObjectBuilder_PlanetEnvironmentComponent.SectorObstructions.Add(item);
					}
					return myObjectBuilder_PlanetEnvironmentComponent;
				}
			}
			return myObjectBuilder_PlanetEnvironmentComponent;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			MyObjectBuilder_PlanetEnvironmentComponent myObjectBuilder_PlanetEnvironmentComponent = builder as MyObjectBuilder_PlanetEnvironmentComponent;
			if (myObjectBuilder_PlanetEnvironmentComponent != null)
			{
				m_providerData = myObjectBuilder_PlanetEnvironmentComponent.DataProviders;
				if (myObjectBuilder_PlanetEnvironmentComponent.SectorObstructions != null)
				{
					CollisionCheckEnabled = true;
					m_obstructorsPerSector = new Dictionary<long, List<MyOrientedBoundingBoxD>>();
					foreach (MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox sectorObstruction in myObjectBuilder_PlanetEnvironmentComponent.SectorObstructions)
					{
						m_obstructorsPerSector[sectorObstruction.SectorId] = new List<MyOrientedBoundingBoxD>();
						if (sectorObstruction.ObstructingBoxes != null)
						{
							foreach (SerializableOrientedBoundingBoxD obstructingBox in sectorObstruction.ObstructingBoxes)
							{
								m_obstructorsPerSector[sectorObstruction.SectorId].Add(obstructingBox);
							}
						}
					}
				}
			}
		}

		private void ParallelWorkCallback()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			m_parallelSyncPoint.Reset();
			MyPlanet planet = Planet;
			if (planet != null)
			{
				using (planet.Pin())
				{
					if (!planet.MarkedForClose)
					{
						MyEnvironmentSector instance;
						while (stopwatch.ElapsedMilliseconds < 100 && m_sectorsToWorkParallel.TryDequeue(out instance))
						{
							using (instance.Pin())
							{
								if (!instance.MarkedForClose)
								{
									instance.DoParallelWork();
								}
							}
						}
					}
				}
			}
			m_parallelSyncPoint.Set();
		}

		private void SerialWorkCallback()
		{
			int num = m_sectorsToWorkSerial.Count;
			while (num > 0 && m_sectorsToWorkSerial.Count > 0)
			{
				MyEnvironmentSector myEnvironmentSector = m_sectorsToWorkSerial.Dequeue();
				if (!myEnvironmentSector.HasParallelWorkPending)
				{
					myEnvironmentSector.DoSerialWork();
				}
				else
				{
					m_sectorsToWorkSerial.Enqueue(myEnvironmentSector);
				}
				num--;
			}
			m_parallelInProgress = false;
		}

		internal void EnqueueClosing(MyEnvironmentSector sector)
		{
			m_sectorsClosing.Add(sector);
		}

		internal bool IsQueued(MyPlanetEnvironmentClipmapProxy sector)
		{
			return m_sectorOperations.ContainsKey(sector.Id);
		}

		internal int QueuedLod(MyPlanetEnvironmentClipmapProxy sector)
		{
			if (m_sectorOperations.TryGetValue(sector.Id, out Operation value))
			{
				return value.LodToSet;
			}
			return sector.Lod;
		}

		internal void EnqueueOperation(MyPlanetEnvironmentClipmapProxy proxy, int lod, bool close = false)
		{
			long id = proxy.Id;
			if (m_sectorOperations.TryGetValue(id, out Operation value))
			{
				value.LodToSet = lod;
				value.ShouldClose = close;
				m_sectorOperations[id] = value;
			}
			else
			{
				value.LodToSet = lod;
				value.Proxy = proxy;
				value.ShouldClose = close;
				m_sectorOperations.Add(id, value);
			}
		}

		private void EvaluateOperations()
		{
			foreach (Operation value in m_sectorOperations.Values)
			{
				MyPlanetEnvironmentClipmapProxy proxy = value.Proxy;
				proxy.EnvironmentSector.SetLod(value.LodToSet);
				if (value.ShouldClose && value.LodToSet == -1)
				{
					CheckOnGraphicsClose(proxy.EnvironmentSector);
				}
			}
			m_sectorOperations.Clear();
		}

		internal bool CheckOnGraphicsClose(MyEnvironmentSector sector)
		{
			if (sector.HasPhysics == sector.IsPendingPhysicsToggle && !sector.IsPinned)
			{
				EnqueueClosing(sector);
				return true;
			}
			return false;
		}

		internal void RegisterProxy(MyPlanetEnvironmentClipmapProxy proxy)
		{
			Proxies.Add(proxy.Id, proxy);
		}

		internal void MarkProxyOutgoingProxy(MyPlanetEnvironmentClipmapProxy proxy)
		{
			Proxies.Remove(proxy.Id);
			OutgoingProxies[proxy.Id] = proxy;
		}

		internal void UnmarkProxyOutgoingProxy(MyPlanetEnvironmentClipmapProxy proxy)
		{
			OutgoingProxies.Remove(proxy.Id);
			Proxies.Add(proxy.Id, proxy);
		}

		internal void UnregisterProxy(MyPlanetEnvironmentClipmapProxy proxy)
		{
			Proxies.Remove(proxy.Id);
		}

		internal void UnregisterOutgoingProxy(MyPlanetEnvironmentClipmapProxy proxy)
		{
			OutgoingProxies.Remove(proxy.Id);
		}

		public void QuerySurfaceParameters(Vector3D localOrigin, ref BoundingBoxD queryBounds, List<Vector3> queries, MyList<MySurfaceParams> results)
		{
			localOrigin -= Planet.PositionLeftBottomCorner;
			using (Planet.Storage.Pin())
			{
				BoundingBox request = (BoundingBox)queryBounds.Translate(-Planet.PositionLeftBottomCorner);
				Planet.Provider.Shape.PrepareCache();
				Planet.Provider.Material.PrepareRulesForBox(ref request);
				if (results.Capacity != queries.Count)
				{
					results.Capacity = queries.Count;
				}
				MySurfaceParams[] internalArray = results.GetInternalArray();
				for (int i = 0; i < queries.Count; i++)
				{
					Planet.Provider.ComputeCombinedMaterialAndSurface(queries[i] + localOrigin, useCache: true, out internalArray[i]);
					ref Vector3 position = ref internalArray[i].Position;
					position -= localOrigin;
				}
				results.SetSize(queries.Count);
			}
		}

		public MyEnvironmentSector GetSectorForPosition(Vector3D positionWorld)
		{
			Vector3D localPos = positionWorld - PlanetTranslation;
			MyPlanetCubemapHelper.ProjectToCube(ref localPos, out int direction, out Vector2D texcoords);
			texcoords *= m_clipmaps[direction].FaceHalf;
			return m_clipmaps[direction].GetHandler(texcoords)?.EnvironmentSector;
		}

		public MyEnvironmentSector GetSectorById(long packedSectorId)
		{
			if (!PhysicsSectors.TryGetValue(packedSectorId, out MyEnvironmentSector value))
			{
				if (!Proxies.TryGetValue(packedSectorId, out MyPlanetEnvironmentClipmapProxy value2))
				{
					return null;
				}
				return value2.EnvironmentSector;
			}
			return value;
		}

		public void SetSectorPinned(MyEnvironmentSector sector, bool pinned)
		{
			if (pinned != sector.IsPinned)
			{
				if (pinned)
				{
					sector.IsPinned = true;
					HeldSectors.Add(sector.SectorId, sector);
				}
				else
				{
					sector.IsPinned = false;
					HeldSectors.Remove(sector.SectorId);
				}
			}
		}

		public void GetSectorsInRange(ref BoundingBoxD bb, List<MyEntity> outSectors)
		{
			(base.Container.Get<MyHierarchyComponentBase>() as MyHierarchyComponent<MyEntity>).QueryAABB(ref bb, outSectors);
		}

		public int GetSeed()
		{
			return m_InstanceHash;
		}

		public MyPhysicalModelDefinition GetModelForId(short id)
		{
			if (id < m_physicalModels.Count)
			{
				return m_physicalModels[id];
			}
			return null;
		}

		public void GetDefinition(ushort index, out MyRuntimeEnvironmentItemInfo def)
		{
			def = EnvironmentDefinition.Items[index];
		}

		public void ProjectPointToSurface(ref Vector3D center)
		{
			center = Planet.GetClosestSurfacePointGlobal(ref center);
		}

		public void GetSurfaceNormalForPoint(ref Vector3D point, out Vector3D normal)
		{
			normal = point - PlanetTranslation;
			normal.Normalize();
		}

		public Vector3D[] GetBoundingShape(ref Vector3D worldPos, ref Vector3 basisX, ref Vector3 basisY)
		{
			BoundingBox box = BoundingBox.CreateInvalid();
			box.Include(-basisX - basisY);
			box.Include(basisX + basisY);
			box.Translate(worldPos - Planet.WorldMatrix.Translation);
			Planet.Provider.Shape.GetBounds(ref box);
			box.Min.Z -= 1f;
			box.Max.Z += 1f;
			Vector3D[] array = new Vector3D[8]
			{
				worldPos - basisX - basisY,
				worldPos + basisX - basisY,
				worldPos - basisX + basisY,
				worldPos + basisX + basisY,
				default(Vector3D),
				default(Vector3D),
				default(Vector3D),
				default(Vector3D)
			};
			for (int i = 0; i < 4; i++)
			{
				array[i] -= Planet.WorldMatrix.Translation;
				array[i].Normalize();
				array[i + 4] = array[i] * box.Max.Z;
				array[i] *= (double)box.Min.Z;
				array[i] += Planet.WorldMatrix.Translation;
				array[i + 4] += Planet.WorldMatrix.Translation;
			}
			return array;
		}

		public short GetModelId(MyPhysicalModelDefinition def)
		{
			if (!m_physicalModelToKey.TryGetValue(def, out short value))
			{
				value = (short)m_physicalModels.Count;
				m_physicalModelToKey.Add(def, value);
				m_physicalModels.Add(def);
			}
			return value;
		}

		public void ScheduleWork(MyEnvironmentSector sector, bool parallel)
		{
			if (parallel)
			{
				m_sectorsToWorkParallel.Enqueue(sector);
			}
			else
			{
				m_sectorsToWorkSerial.Enqueue(sector);
			}
		}

		public List<MyOrientedBoundingBoxD> GetCollidedBoxes(long sectorId)
		{
			if (m_obstructorsPerSector.TryGetValue(sectorId, out List<MyOrientedBoundingBoxD> value))
			{
				m_obstructorsPerSector.Remove(sectorId);
			}
			return value;
		}

		public void InitClearAreasManagement()
		{
			m_obstructorsPerSector = new Dictionary<long, List<MyOrientedBoundingBoxD>>();
			BoundingBoxD box = Planet.PositionComp.WorldAABB;
			List<MyEntity> list = new List<MyEntity>();
			MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, list);
			foreach (MyEntity item in list)
			{
				RasterSectorsForCollision(item);
			}
			CollisionCheckEnabled = true;
		}

		private unsafe void RasterSectorsForCollision(MyEntity entity)
		{
			if (!(entity is MyCubeGrid))
			{
				return;
			}
			BoundingBoxD worldAABB = entity.PositionComp.WorldAABB;
			worldAABB.Inflate(8.0);
			worldAABB.Translate(-PlanetTranslation);
			Vector2I v = new Vector2I(1 << m_clipmaps[0].Depth) - 1;
			Vector3D* ptr = stackalloc Vector3D[8];
			worldAABB.GetCornersUnsafe(ptr);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 8; i++)
			{
				Vector3D localPos = ptr[i];
				int num3 = MyPlanetCubemapHelper.FindCubeFace(ref localPos);
				num2 = num3;
				num3 = 1 << num3;
				if ((num & ~num3) != 0)
				{
					num |= 0x40;
				}
				num |= num3;
			}
			int num4 = 0;
			int num5 = 5;
			if ((num & 0x40) == 0)
			{
				num4 = (num5 = num2);
			}
			for (int j = num4; j <= num5; j++)
			{
				if (((1 << j) & num) == 0)
				{
					continue;
				}
				int num6 = 1 << m_clipmaps[j].Depth - 1;
				BoundingBox2D boundingBox2D = BoundingBox2D.CreateInvalid();
				for (int k = 0; k < 8; k++)
				{
					Vector3D localPos2 = ptr[k];
					MyPlanetCubemapHelper.ProjectForFace(ref localPos2, j, out Vector2D normalCoord);
					boundingBox2D.Include(normalCoord);
				}
				boundingBox2D.Min += 1.0;
				boundingBox2D.Min *= (double)num6;
				boundingBox2D.Max += 1.0;
				boundingBox2D.Max *= (double)num6;
				Vector2I v2 = new Vector2I((int)boundingBox2D.Min.X, (int)boundingBox2D.Min.Y);
				Vector2I v3 = new Vector2I((int)boundingBox2D.Max.X, (int)boundingBox2D.Max.Y);
				Vector2I.Max(ref v2, ref Vector2I.Zero, out v2);
				Vector2I.Min(ref v3, ref v, out v3);
				for (int l = v2.X; l <= v3.X; l++)
				{
					for (int m = v2.Y; m <= v3.Y; m++)
					{
						long key = MyPlanetSectorId.MakeSectorId(l, m, j);
						if (!m_obstructorsPerSector.TryGetValue(key, out List<MyOrientedBoundingBoxD> value))
						{
							value = new List<MyOrientedBoundingBoxD>();
							m_obstructorsPerSector.Add(key, value);
						}
						BoundingBox localAABB = entity.PositionComp.LocalAABB;
						localAABB.Inflate(8f);
						value.Add(new MyOrientedBoundingBoxD(localAABB, entity.PositionComp.WorldMatrix));
					}
				}
			}
		}

		public MyLogicalEnvironmentSectorBase GetLogicalSector(long packedSectorId)
		{
			int face = MyPlanetSectorId.GetFace(packedSectorId);
			return Providers[face].GetLogicalSector(packedSectorId);
		}

		public void CloseAll()
		{
			m_parallelSyncPoint.Reset();
			foreach (MyEnvironmentSector value in PhysicsSectors.Values)
			{
				value.EnablePhysics(physics: false);
				if (value.LodLevel == -1 && !value.IsPendingLodSwitch)
				{
					m_sectorsClosing.Add(value);
				}
			}
			m_sectorsWithPhysics.Clear();
			PhysicsSectors.Clear();
			for (int i = 0; i < m_clipmaps.Length; i++)
			{
				ActiveFace = i;
				(ActiveClipmap = m_clipmaps[i]).Clear();
				EvaluateOperations();
			}
			ActiveFace = -1;
			ActiveClipmap = null;
			foreach (MyEnvironmentSector item in m_sectorsClosing)
			{
				if (item.HasParallelWorkPending)
				{
					item.DoParallelWork();
				}
				if (item.HasSerialWorkPending)
				{
					item.DoSerialWork();
				}
				item.Close();
			}
			m_sectorsClosing.Clear();
			m_sectorsToWorkParallel.Clear();
			m_sectorsToWorkSerial.Clear();
			m_parallelSyncPoint.Set();
		}

		public bool TryGetSector(long id, out MyEnvironmentSector environmentSector)
		{
			if (!PhysicsSectors.TryGetValue(id, out environmentSector))
			{
				return HeldSectors.TryGetValue(id, out environmentSector);
			}
			return false;
		}
	}
}
