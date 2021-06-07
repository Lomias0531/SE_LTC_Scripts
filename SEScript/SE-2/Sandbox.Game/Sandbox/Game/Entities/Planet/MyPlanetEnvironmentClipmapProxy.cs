using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Planet
{
	internal class MyPlanetEnvironmentClipmapProxy : IMy2DClipmapNodeHandler
	{
		public long Id;

		public int Face;

		public int Lod;

		public Vector2I Coords;

		private int m_lodSet = -1;

		public MyEnvironmentSector EnvironmentSector;

		private MyPlanetEnvironmentComponent m_manager;

		private bool m_split;

		private bool m_closed;

		private bool m_stateCommited;

		private MyPlanetEnvironmentClipmapProxy m_parent;

		private readonly MyPlanetEnvironmentClipmapProxy[] m_children = new MyPlanetEnvironmentClipmapProxy[4];

		private readonly HashSet<MyPlanetEnvironmentClipmapProxy> m_dependencies = new HashSet<MyPlanetEnvironmentClipmapProxy>();

		private readonly HashSet<MyPlanetEnvironmentClipmapProxy> m_dependants = new HashSet<MyPlanetEnvironmentClipmapProxy>();

		public int LodSet
		{
			get
			{
				return m_lodSet;
			}
			protected set
			{
				m_lodSet = value;
				m_stateCommited = false;
			}
		}

		public void Init(IMy2DClipmapManager parent, int x, int y, int lod, ref BoundingBox2D bounds)
		{
			m_manager = (MyPlanetEnvironmentComponent)parent;
			BoundingBoxD boundingBoxD = new BoundingBoxD(new Vector3D(bounds.Min, 0.0), new Vector3D(bounds.Max, 0.0));
			Lod = lod;
			Face = m_manager.ActiveFace;
			MatrixD worldMatrix = m_manager.ActiveClipmap.WorldMatrix;
			boundingBoxD = boundingBoxD.TransformFast(worldMatrix);
			Coords = new Vector2I(x, y);
			Id = MyPlanetSectorId.MakeSectorId(x, y, m_manager.ActiveFace, lod);
			m_manager.RegisterProxy(this);
			worldMatrix.Translation = Vector3D.Zero;
			MyEnvironmentSectorParameters parameters = default(MyEnvironmentSectorParameters);
			parameters.SurfaceBasisX = Vector3.Transform(new Vector3(bounds.Width / 2.0, 0.0, 0.0), worldMatrix);
			parameters.SurfaceBasisY = Vector3.Transform(new Vector3(0.0, bounds.Height / 2.0, 0.0), worldMatrix);
			parameters.Center = boundingBoxD.Center;
			if (lod <= m_manager.MaxLod)
			{
				if (!m_manager.TryGetSector(Id, out EnvironmentSector))
				{
					parameters.SectorId = Id;
					parameters.EntityId = MyPlanetSectorId.MakeSectorId(x, y, m_manager.ActiveFace, lod);
					parameters.Bounds = m_manager.GetBoundingShape(ref parameters.Center, ref parameters.SurfaceBasisX, ref parameters.SurfaceBasisY);
					parameters.Environment = m_manager.EnvironmentDefinition;
					parameters.DataRange = new BoundingBox2I(Coords << lod, (Coords + 1 << lod) - 1);
					parameters.Provider = m_manager.Providers[m_manager.ActiveFace];
					EnvironmentSector = m_manager.EnvironmentDefinition.CreateSector();
					EnvironmentSector.Init(m_manager, ref parameters);
					m_manager.Planet.AddChildEntity(EnvironmentSector);
				}
				m_manager.EnqueueOperation(this, lod);
				LodSet = lod;
				EnvironmentSector.OnLodCommit += sector_OnMyLodCommit;
			}
		}

		public void Close()
		{
			if (m_closed)
			{
				return;
			}
			m_closed = true;
			if (EnvironmentSector != null)
			{
				m_manager.MarkProxyOutgoingProxy(this);
				NotifyDependants(clipmapUpdate: true);
				if (m_split)
				{
					for (int i = 0; i < 4; i++)
					{
						WaitFor(m_children[i]);
					}
				}
				else if (m_parent != null)
				{
					WaitFor(m_parent);
				}
				if (m_manager.IsQueued(this) || m_dependencies.Count == 0)
				{
					EnqueueClose(clipmapUpdate: true);
				}
				if (m_dependencies.Count == 0)
				{
					CloseCommit(clipmapUpdate: true);
				}
			}
			else
			{
				m_manager.UnregisterProxy(this);
			}
		}

		private void EnqueueClose(bool clipmapUpdate)
		{
			if (EnvironmentSector.IsClosed)
			{
				return;
			}
			if (clipmapUpdate)
			{
				m_manager.EnqueueOperation(this, -1, !m_split);
				LodSet = -1;
				return;
			}
			EnvironmentSector.SetLod(-1);
			LodSet = -1;
			if (!m_split)
			{
				m_manager.CheckOnGraphicsClose(EnvironmentSector);
			}
		}

		public void InitJoin(IMy2DClipmapNodeHandler[] children)
		{
			m_split = false;
			m_closed = false;
			if (EnvironmentSector != null)
			{
				m_manager.UnmarkProxyOutgoingProxy(this);
				m_manager.EnqueueOperation(this, Lod);
				LodSet = Lod;
				for (int i = 0; i < 4; i++)
				{
					m_children[i] = null;
				}
			}
			else
			{
				m_manager.RegisterProxy(this);
			}
		}

		public unsafe void Split(BoundingBox2D* childBoxes, ref IMy2DClipmapNodeHandler[] children)
		{
			m_split = true;
			for (int i = 0; i < 4; i++)
			{
				children[i].Init(m_manager, (Coords.X << 1) + (i & 1), (Coords.Y << 1) + ((i >> 1) & 1), Lod - 1, ref childBoxes[i]);
			}
			if (EnvironmentSector != null)
			{
				for (int j = 0; j < 4; j++)
				{
					m_children[j] = (MyPlanetEnvironmentClipmapProxy)children[j];
					m_children[j].m_parent = this;
				}
			}
		}

		private void WaitFor(MyPlanetEnvironmentClipmapProxy proxy)
		{
			if (proxy.LodSet != -1)
			{
				m_dependencies.Add(proxy);
				proxy.m_dependants.Add(this);
			}
		}

		private void sector_OnMyLodCommit(MyEnvironmentSector sector, int lod)
		{
			if (lod != LodSet)
			{
				return;
			}
			m_stateCommited = true;
			if (m_dependencies.Count == 0)
			{
				if (lod == -1 && m_closed)
				{
					CloseCommit(clipmapUpdate: false);
				}
				else
				{
					NotifyDependants(clipmapUpdate: false);
				}
			}
		}

		private void CloseCommit(bool clipmapUpdate)
		{
			if (!m_split)
			{
				m_manager.UnregisterOutgoingProxy(this);
				EnvironmentSector.OnLodCommit -= sector_OnMyLodCommit;
			}
			NotifyDependants(clipmapUpdate);
		}

		private void NotifyDependants(bool clipmapUpdate)
		{
			foreach (MyPlanetEnvironmentClipmapProxy dependant in m_dependants)
			{
				dependant.Notify(this, clipmapUpdate);
			}
			m_dependants.Clear();
		}

		private void ClearDependencies()
		{
			foreach (MyPlanetEnvironmentClipmapProxy dependency in m_dependencies)
			{
				dependency.m_dependants.Remove(this);
			}
			m_dependencies.Clear();
		}

		private void Notify(MyPlanetEnvironmentClipmapProxy proxy, bool clipmapUpdate)
		{
			if (m_dependencies.Count == 0)
			{
				return;
			}
			m_dependencies.Remove(proxy);
			if (m_dependencies.Count == 0 && m_closed)
			{
				EnqueueClose(clipmapUpdate);
				if (EnvironmentSector.IsClosed || EnvironmentSector.LodLevel == -1)
				{
					CloseCommit(clipmapUpdate);
				}
			}
		}

		internal void DebugDraw(bool outgoing = false)
		{
			if (EnvironmentSector != null)
			{
				Vector3D vector3D = (EnvironmentSector.Bounds[4] + EnvironmentSector.Bounds[7]) / 2.0;
				Vector3 vector = MySector.MainCamera.UpVector * 2f * (1 << Lod);
				string text = string.Format("Lod: {4}; Dependants: {0}; Dependencies: {1}\nSplit: {2}; Closed:{3}", m_dependants.Count, m_dependencies.Count, m_split, m_closed, Lod);
				MyRenderProxy.DebugDrawText3D(vector3D += vector, text, outgoing ? Color.Yellow : Color.White, 1f, depthRead: true);
				((MyEntity)EnvironmentSector).DebugDraw();
			}
		}
	}
}
