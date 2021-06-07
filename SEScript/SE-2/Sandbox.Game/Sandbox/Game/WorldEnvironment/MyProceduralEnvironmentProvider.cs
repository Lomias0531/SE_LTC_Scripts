using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.WorldEnvironment
{
	public class MyProceduralEnvironmentProvider : IMyEnvironmentDataProvider
	{
		private readonly FastResourceLock m_sectorsLock = new FastResourceLock();

		private readonly Dictionary<long, MyProceduralLogicalSector> m_sectors = new Dictionary<long, MyProceduralLogicalSector>();

		private readonly Dictionary<long, MyObjectBuilder_ProceduralEnvironmentSector> m_savedSectors = new Dictionary<long, MyObjectBuilder_ProceduralEnvironmentSector>();

		private volatile bool m_sectorsQueued;

		private readonly MyConcurrentQueue<MyProceduralLogicalSector> m_sectorsToRaise = new MyConcurrentQueue<MyProceduralLogicalSector>();

		private readonly MyConcurrentQueue<MyProceduralLogicalSector> m_sectorsToDestroy = new MyConcurrentQueue<MyProceduralLogicalSector>();

		private readonly MyConcurrentHashSet<MyProceduralLogicalSector> m_sectorsForReplication = new MyConcurrentHashSet<MyProceduralLogicalSector>();

		public int LodFactor = 3;

		private Vector3D m_origin;

		private Vector3D m_basisX;

		private Vector3D m_basisY;

		private double m_sectorSize;

		private readonly Action m_raiseCallback;

		public IMyEnvironmentOwner Owner
		{
			get;
			private set;
		}

		public int ProviderId
		{
			get;
			set;
		}

		internal int SyncLod => Owner.EnvironmentDefinition.SyncLod;

		public IEnumerable<MyLogicalEnvironmentSectorBase> LogicalSectors => m_sectorsForReplication;

		public MyProceduralEnvironmentProvider()
		{
			m_raiseCallback = RaiseLogicalSectors;
		}

		public void Init(IMyEnvironmentOwner owner, ref Vector3D origin, ref Vector3D basisA, ref Vector3D basisB, double sectorSize, MyObjectBuilder_Base ob)
		{
			Owner = owner;
			m_sectorSize = sectorSize;
			m_origin = origin;
			m_basisX = basisA;
			m_basisY = basisB;
			MyObjectBuilder_ProceduralEnvironmentProvider myObjectBuilder_ProceduralEnvironmentProvider = ob as MyObjectBuilder_ProceduralEnvironmentProvider;
			if (myObjectBuilder_ProceduralEnvironmentProvider != null)
			{
				for (int i = 0; i < myObjectBuilder_ProceduralEnvironmentProvider.Sectors.Count; i++)
				{
					MyObjectBuilder_ProceduralEnvironmentSector myObjectBuilder_ProceduralEnvironmentSector = myObjectBuilder_ProceduralEnvironmentProvider.Sectors[i];
					m_savedSectors.Add(myObjectBuilder_ProceduralEnvironmentSector.SectorId, myObjectBuilder_ProceduralEnvironmentSector);
				}
			}
		}

		private void RaiseLogicalSectors()
		{
			MyMultiplayerServerBase myMultiplayerServerBase = (MyMultiplayerServerBase)MyMultiplayer.Static;
			m_sectorsQueued = false;
			MyProceduralLogicalSector instance;
			while (m_sectorsToDestroy.TryDequeue(out instance))
			{
				instance.Close();
			}
			while (m_sectorsToRaise.TryDequeue(out instance))
			{
				myMultiplayerServerBase?.RaiseReplicableCreated(instance);
			}
		}

		private void QueueRaiseLogicalSector(MyProceduralLogicalSector sector)
		{
			if (Sync.IsServer && Sync.MultiplayerActive)
			{
				m_sectorsToRaise.Enqueue(sector);
			}
		}

		private void QueueDestroyLogicalSector(MyProceduralLogicalSector sector)
		{
			if (Sync.IsServer && Sync.MultiplayerActive)
			{
				m_sectorsToDestroy.Enqueue(sector);
			}
			else
			{
				sector.Close();
			}
		}

		public MyEnvironmentDataView GetItemView(int lod, ref Vector2I start, ref Vector2I end, ref Vector3D localOrigin)
		{
			int num = lod / LodFactor;
			int logicalLod = lod % LodFactor;
			start >>= num * LodFactor;
			end >>= num * LodFactor;
			MyProceduralDataView myProceduralDataView = new MyProceduralDataView(this, lod, ref start, ref end);
			for (int i = start.Y; i <= end.Y; i++)
			{
				for (int j = start.X; j <= end.X; j++)
				{
					GetLogicalSector(j, i, num).AddView(myProceduralDataView, localOrigin, logicalLod);
				}
			}
			if ((m_sectorsToRaise.Count > 0 || m_sectorsToRaise.Count > 0) && !m_sectorsQueued)
			{
				m_sectorsQueued = true;
				MySandboxGame.Static.Invoke(m_raiseCallback, "RaiseLogicalSectors");
			}
			return myProceduralDataView;
		}

		private MyProceduralLogicalSector GetLogicalSector(int x, int y, int localLod)
		{
			long num = MyPlanetSectorId.MakeSectorId(x, y, ProviderId, localLod);
			bool flag;
			MyProceduralLogicalSector value;
			using (m_sectorsLock.AcquireSharedUsing())
			{
				flag = m_sectors.TryGetValue(num, out value);
			}
			if (!flag)
			{
				using (m_sectorsLock.AcquireExclusiveUsing())
				{
					if (m_sectors.TryGetValue(num, out value))
					{
						return value;
					}
					m_savedSectors.TryGetValue(num, out MyObjectBuilder_ProceduralEnvironmentSector value2);
					value = new MyProceduralLogicalSector(this, x, y, localLod, value2)
					{
						Id = num
					};
					value.OnViewerEmpty += CloseSector;
					m_sectors[num] = value;
					return value;
				}
			}
			return value;
		}

		public MyObjectBuilder_EnvironmentDataProvider GetObjectBuilder()
		{
			MyObjectBuilder_ProceduralEnvironmentProvider myObjectBuilder_ProceduralEnvironmentProvider = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ProceduralEnvironmentProvider>();
			using (m_sectorsLock.AcquireSharedUsing())
			{
				foreach (KeyValuePair<long, MyProceduralLogicalSector> sector in m_sectors)
				{
					MyObjectBuilder_EnvironmentSector objectBuilder = sector.Value.GetObjectBuilder();
					if (objectBuilder != null)
					{
						myObjectBuilder_ProceduralEnvironmentProvider.Sectors.Add((MyObjectBuilder_ProceduralEnvironmentSector)objectBuilder);
					}
				}
				foreach (KeyValuePair<long, MyObjectBuilder_ProceduralEnvironmentSector> savedSector in m_savedSectors)
				{
					if (!m_sectors.ContainsKey(savedSector.Key))
					{
						myObjectBuilder_ProceduralEnvironmentProvider.Sectors.Add(savedSector.Value);
					}
				}
				return myObjectBuilder_ProceduralEnvironmentProvider;
			}
		}

		public void DebugDraw()
		{
			float num = MyPlanetEnvironmentSessionComponent.DebugDrawDistance * MyPlanetEnvironmentSessionComponent.DebugDrawDistance;
			using (m_sectorsLock.AcquireSharedUsing())
			{
				foreach (MyProceduralLogicalSector value3 in m_sectors.Values)
				{
					MyRenderProxy.DebugDraw6FaceConvex(value3.Bounds, Color.Violet, 0.5f, depthRead: true, fill: false);
					Vector3D value = (value3.Bounds[4] + value3.Bounds[7]) / 2.0;
					if (Vector3D.DistanceSquared(value, MySector.MainCamera.Position) < (double)num)
					{
						Vector3 value2 = -MySector.MainCamera.UpVector * 3f;
						MyRenderProxy.DebugDrawText3D(value + value2, value3.ToString(), Color.Violet, 1f, depthRead: true);
					}
				}
			}
		}

		public MyLogicalEnvironmentSectorBase GetLogicalSector(long sectorId)
		{
			using (m_sectorsLock.AcquireSharedUsing())
			{
				m_sectors.TryGetValue(sectorId, out MyProceduralLogicalSector value);
				return value;
			}
		}

		public void CloseView(MyProceduralDataView view)
		{
			int lod = view.Lod / LodFactor;
			for (int i = view.Start.Y; i <= view.End.Y; i++)
			{
				for (int j = view.Start.X; j <= view.End.X; j++)
				{
					long key = MyPlanetSectorId.MakeSectorId(j, i, ProviderId, lod);
					MyProceduralLogicalSector myProceduralLogicalSector;
					using (m_sectorsLock.AcquireSharedUsing())
					{
						myProceduralLogicalSector = m_sectors[key];
					}
					myProceduralLogicalSector.RemoveView(view);
				}
			}
		}

		private void CloseSector(MyProceduralLogicalSector sector)
		{
			if (!sector.ServerOwned)
			{
				sector.OnViewerEmpty -= CloseSector;
				SaveLogicalSector(sector);
				using (m_sectorsLock.AcquireExclusiveUsing())
				{
					m_sectors.Remove(sector.Id);
				}
				if (sector.Replicable)
				{
					UnmarkReplicable(sector);
				}
				QueueDestroyLogicalSector(sector);
			}
		}

		private void SaveLogicalSector(MyProceduralLogicalSector sector)
		{
			MyObjectBuilder_EnvironmentSector objectBuilder = sector.GetObjectBuilder();
			if (objectBuilder == null)
			{
				m_savedSectors.Remove(sector.Id);
			}
			else
			{
				m_savedSectors[sector.Id] = (MyObjectBuilder_ProceduralEnvironmentSector)objectBuilder;
			}
		}

		public MyProceduralLogicalSector TryGetLogicalSector(int lod, int logicalx, int logicaly)
		{
			using (m_sectorsLock.AcquireSharedUsing())
			{
				m_sectors.TryGetValue(MyPlanetSectorId.MakeSectorId(logicalx, logicaly, ProviderId, lod), out MyProceduralLogicalSector value);
				return value;
			}
		}

		public void GeSectorWorldParameters(int x, int y, int localLod, out Vector3D worldPos, out Vector3 scanBasisA, out Vector3 scanBasisB)
		{
			double num = (double)(1 << localLod) * m_sectorSize;
			worldPos = m_origin + m_basisX * (((double)x + 0.5) * num) + m_basisY * (((double)y + 0.5) * num);
			scanBasisA = m_basisX * (num * 0.5);
			scanBasisB = m_basisY * (num * 0.5);
		}

		public int GetSeed()
		{
			return Owner.GetSeed();
		}

		internal void MarkReplicable(MyProceduralLogicalSector sector)
		{
			m_sectorsForReplication.Add(sector);
			QueueRaiseLogicalSector(sector);
			sector.Replicable = true;
			if (!Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyClientState.AddKnownSector, Owner.Entity.EntityId, sector.Id);
			}
		}

		internal void UnmarkReplicable(MyProceduralLogicalSector sector)
		{
			m_sectorsForReplication.Remove(sector);
			sector.Replicable = false;
			if (!Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyClientState.RemoveKnownSector, Owner.Entity.EntityId, sector.Id);
			}
		}
	}
}
