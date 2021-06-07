using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VRage.Collections;
using VRage.Entities.Components;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels.Mesh;
using VRage.Voxels.Sewing;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
	/// <summary>
	/// Lod controller for voxel meshes implemented in the fashion of the classic clipmap paper.
	/// </summary>
	/// The original paper:
	/// https://web.archive.org/web/20160428150805/http://www.cs.virginia.edu/~gfx/Courses/2002/BigData/papers/Texturing/Clipmap.pdf
	///
	/// This implementation extends the ideas presented in the paper to 3 dimensions
	/// and manages a mesh 'sewing' process that joins the boundaries between meshes.
	public class MyVoxelClipmap : IMyLodController
	{
		private struct CellRenderUpdate
		{
			public readonly MyCellCoord Cell;

			public readonly StitchOperation Operation;

			public MyVoxelRenderCellData Data;

			public CellRenderUpdate(MyCellCoord cell, StitchOperation operation, ref MyVoxelRenderCellData data)
			{
				Cell = cell;
				Data = data;
				Operation = operation;
			}
		}

		/// <summary>
		/// Description of a mesh sewing operation.
		/// </summary>
		[GenerateActivator]
		internal class StitchOperation
		{
			public enum OpState
			{
				Pooled,
				Pending,
				Queued,
				Working,
				Ready,
				Returned
			}

			private class VRage_Voxels_Clipmap_MyVoxelClipmap_003C_003EStitchOperation_003C_003EActor : IActivator, IActivator<StitchOperation>
			{
				private sealed override object CreateInstance()
				{
					return new StitchOperation();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override StitchOperation CreateInstance()
				{
					return new StitchOperation();
				}

				StitchOperation IActivator<StitchOperation>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			/// <summary>
			/// Neighbors.
			/// </summary>
			public MyCellCoord[] Dependencies = new MyCellCoord[8];

			/// <summary>
			/// Meshes to be stitched.
			/// </summary>
			public VrSewGuide[] Guides = new VrSewGuide[8];

			/// <summary>
			/// Operations to be carried in the mesh.
			/// </summary>
			public VrSewOperation Operation;

			/// <summary>
			/// Range over which to stitch.
			/// </summary>
			public BoundingBoxI? Range;

			/// <summary>
			/// Count of pending dependencies.
			/// </summary>
			public short Pending;

			/// <summary>
			/// Recalculate this operation once done.
			///
			/// Used when the operation is invalidated before completed.
			/// </summary>
			public bool Recalculate;

			/// <summary>
			/// Whether this operation contain cells of multiple LODs (in a border).
			/// </summary>
			public bool BorderOperation;

			private OpState m_state;

			/// <summary>
			/// Cell waiting for stitch.
			/// </summary>
			public MyCellCoord Cell
			{
				get;
				private set;
			}

			public OpState State => m_state;

			public virtual void Init(MyCellCoord coord)
			{
				Cell = coord;
			}

			public void Start()
			{
				for (int i = 0; i < Guides.Length; i++)
				{
					if (Guides[i] != null)
					{
						Guides[i].AddReference();
					}
				}
			}

			public virtual void Clear(bool dereference = true)
			{
				for (int i = 0; i < Guides.Length; i++)
				{
					if (Guides[i] != null && dereference)
					{
						Guides[i].RemoveReference();
					}
					Guides[i] = null;
				}
				Recalculate = false;
				if (Range.HasValue)
				{
					Range = null;
				}
				BorderOperation = false;
			}

			public virtual CompoundStitchOperation GetCompound()
			{
				return null;
			}

			[Conditional("DEBUG")]
			public virtual void SetState(OpState value)
			{
				m_state = value;
			}
		}

		/// <summary>
		/// A stitch operation with child operations, used when a forward neighbor of a cell is inside an inner lod,
		/// resulting in many meshes to be sewn with.
		/// </summary>
		internal class CompoundStitchOperation : StitchOperation
		{
			private class VRage_Voxels_Clipmap_MyVoxelClipmap_003C_003ECompoundStitchOperation_003C_003EActor : IActivator, IActivator<CompoundStitchOperation>
			{
				private sealed override object CreateInstance()
				{
					return new CompoundStitchOperation();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CompoundStitchOperation CreateInstance()
				{
					return new CompoundStitchOperation();
				}

				CompoundStitchOperation IActivator<CompoundStitchOperation>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public List<StitchOperation> Children = new List<StitchOperation>();

			public override void Init(MyCellCoord coord)
			{
				base.Init(coord);
			}

			public override void Clear(bool dereference = true)
			{
				base.Clear(dereference);
				Children.Clear();
			}

			public override CompoundStitchOperation GetCompound()
			{
				return this;
			}

			/// <inheritdoc />
			public override void SetState(OpState value)
			{
				for (int i = 0; i < Children.Count; i++)
				{
				}
			}
		}

		private enum UpdateState
		{
			NotReady,
			Idle,
			Calculate,
			Unloaded
		}

		/// <summary>
		/// Enum representing the various stitch modes.
		/// </summary>
		public enum StitchMode
		{
			/// <summary>
			/// Leave the gap between meshes
			/// </summary>
			None,
			/// <summary>
			/// Generate meshes exactly up to their neighbors.
			/// </summary>
			BlindMeet,
			/// <summary>
			/// Overlap meshes by one voxel.
			/// </summary>
			Overlap,
			/// <summary>
			/// Sew meshes to their neighbors.
			/// </summary>
			Stitch
		}

		/// <summary>
		/// Log base two of the size of a cell.
		/// </summary>
		internal int CellBits;

		/// <summary>
		/// The size of a cell (always a power of two).
		/// </summary>
		internal int CellSize;

		/// <summary>
		/// Distance at which we proceed to update a cell.
		/// </summary>
		private int m_updateDistance;

		/// <summary>
		/// Cell cache for this clipmap instance.
		/// </summary>
		private MyVoxelClipmapCache m_cache;

		/// <summary>
		/// Size of each ring cells
		/// </summary>
		internal readonly int[] Ranges = (int[])(object)new int[16];

		private readonly Vector3I m_voxelSize;

		private MatrixD m_localToWorld;

		private MatrixD m_worldToLocal;

		internal readonly List<MyVoxelClipmapRing> Rings = new List<MyVoxelClipmapRing>(16);

		private Ref<int> m_lockOwner = Ref.Create<int>(-1);

		private FastResourceLock m_lock = new FastResourceLock();

		private float? m_spherizeRadius;

		private Vector3D m_spherizePosition;

		private const int LOADED_WAIT_TIME = 5;

		private int m_loadedCounter;

		/// <summary>
		/// Current settings.
		/// </summary>
		private MyVoxelClipmapSettings m_settings;

		/// <summary>
		/// Flag that the clipmap settings have changed and need to be reloaded.
		/// </summary>
		private bool m_settingsChanged;

		private static readonly MyConcurrentPool<StitchOperation> m_stitchDependencyPool = new MyConcurrentPool<StitchOperation>(100, null, 1000000);

		private static readonly MyConcurrentPool<CompoundStitchOperation> m_compoundStitchDependencyPool = new MyConcurrentPool<CompoundStitchOperation>(50, null, 1000000);

		private readonly MyWorkTracker<MyCellCoord, MyClipmapMeshJob> m_dataWorkTracker = new MyWorkTracker<MyCellCoord, MyClipmapMeshJob>();

		private readonly MyWorkTracker<MyCellCoord, MyClipmapFullMeshJob> m_fullWorkTracker = new MyWorkTracker<MyCellCoord, MyClipmapFullMeshJob>();

		private List<MyTuple<MyCellCoord, VrSewGuide>> m_cachedCellRequests = new List<MyTuple<MyCellCoord, VrSewGuide>>();

		private readonly MyConcurrentQueue<CellRenderUpdate> m_cellRenderUpdates = new MyConcurrentQueue<CellRenderUpdate>(128);

		private readonly MyListDictionary<MyCellCoord, StitchOperation> m_stitchDependencies = new MyListDictionary<MyCellCoord, StitchOperation>(MyCellCoord.Comparer);

		/// <summary>
		/// Work tracker for sewing.
		/// </summary>
		private readonly MyWorkTracker<MyCellCoord, MyClipmapSewJob> m_stitchWorkTracker = new MyWorkTracker<MyCellCoord, MyClipmapSewJob>(MyCellCoord.Comparer);

		/// <summary>
		/// Offsets to the neighbor cells for stitching.
		/// </summary>
		private static readonly Vector3I[] m_neighbourOffsets = new Vector3I[8]
		{
			new Vector3I(0, 0, 0),
			new Vector3I(1, 0, 0),
			new Vector3I(0, 1, 0),
			new Vector3I(1, 1, 0),
			new Vector3I(0, 0, 1),
			new Vector3I(1, 0, 1),
			new Vector3I(0, 1, 1),
			new Vector3I(1, 1, 1)
		};

		/// <summary>
		/// List of operations to be disabled when a given cell is not available.
		/// </summary>
		private static readonly VrSewOperation[] m_compromizes = new VrSewOperation[8]
		{
			(VrSewOperation)0,
			VrSewOperation.XFace,
			VrSewOperation.YFace,
			VrSewOperation.XY | VrSewOperation.XYZ,
			VrSewOperation.ZFace,
			VrSewOperation.XZ | VrSewOperation.XYZ,
			VrSewOperation.YZ | VrSewOperation.XYZ,
			(VrSewOperation)0
		};

		private UpdateState m_updateState;

		private Vector3L m_lastPosition;

		private BoundingBoxI? m_invalidateRange;

		/// <summary>
		/// Whether to draw the mesh dependency graphs when debug drawing clipmaps.
		/// </summary>
		public static bool DebugDrawDependencies = false;

		/// <summary>
		/// Whether clipmaps should do visibility updates.
		/// </summary>
		public static bool UpdateVisibility = true;

		/// <summary>
		/// Global stitch mode for all clipmaps.
		/// </summary>
		public static StitchMode ActiveStitchMode = StitchMode.Stitch;

		internal MyVoxelMesherComponent Mesher
		{
			get;
			private set;
		}

		/// <summary>
		/// Cell cached used by this clipmap. Can be null.
		/// </summary>
		///
		/// The clipmap uses this cache both for storing recently used meshes in case they may be needed again and also when pre-fetching.
		/// If no cache is provided pre-fetching will be disabled.
		public MyVoxelClipmapCache Cache
		{
			get
			{
				return m_cache;
			}
			set
			{
				if (m_cache != null && Actor != null)
				{
					m_cache.Unregister(Actor.Id);
				}
				m_cache = value;
				BindToCache();
			}
		}

		public IMyVoxelRenderDataProcessorProvider VoxelRenderDataProcessorProvider
		{
			get;
			set;
		}

		/// <summary>
		/// The settings group this clipmap take it's settings from.
		/// </summary>
		/// <seealso cref="T:VRage.Voxels.Clipmap.MyVoxelClipmapSettings" />
		public string SettingsGroup
		{
			get;
			private set;
		}

		/// <summary>
		/// Whether we have been unloaded in the renderer.
		/// </summary>
		public bool IsUnloaded => m_updateState == UpdateState.Unloaded;

		public StitchMode InstanceStitchMode
		{
			get;
			private set;
		}

		public IEnumerable<IMyVoxelActorCell> Cells => from x in Rings.SelectMany((MyVoxelClipmapRing x) => x.Cells.Values)
			select x.Cell into x
			where x != null
			select x;

		public IMyVoxelActor Actor
		{
			get;
			private set;
		}

		public Vector3I Size => m_voxelSize;

		public MatrixD LocalToWorld
		{
			get
			{
				return m_localToWorld;
			}
			set
			{
				m_localToWorld = value;
			}
		}

		private bool WorkPending
		{
			get
			{
				if (m_stitchDependencies.KeyCount == 0 && !m_stitchWorkTracker.HasAny && !m_dataWorkTracker.HasAny && !m_fullWorkTracker.HasAny)
				{
					return m_cellRenderUpdates.Count != 0;
				}
				return true;
			}
		}

		public float? SpherizeRadius => m_spherizeRadius;

		public Vector3D SpherizePosition => m_spherizePosition;

		private event Action<IMyLodController> m_loaded;

		/// <summary>
		/// Event fired when this clipmap is ready.
		/// </summary>
		public event Action<IMyLodController> Loaded
		{
			add
			{
				m_loaded += value;
				Interlocked.Exchange(ref m_loadedCounter, 5);
			}
			remove
			{
				m_loaded -= value;
			}
		}

		public MyVoxelClipmap(Vector3I voxelSize, MatrixD worldMatrix, MyVoxelMesherComponent mesher, float? spherizeRadius, Vector3D spherizePosition, string settingsGroup = null)
		{
			m_voxelSize = voxelSize;
			m_spherizeRadius = spherizeRadius;
			m_spherizePosition = spherizePosition;
			for (int i = 0; i < 16; i++)
			{
				Rings.Add(new MyVoxelClipmapRing(this, i));
			}
			Mesher = mesher;
			UpdateWorldMatrix(ref worldMatrix);
			SettingsGroup = settingsGroup;
			MyVoxelClipmapSettings settings = MyVoxelClipmapSettings.GetSettings(SettingsGroup);
			UpdateSettings(settings);
			ApplySettings(invalidateCells: false);
		}

		private void UpdateWorldMatrix(ref MatrixD matrix)
		{
			LocalToWorld = matrix;
			MatrixD.Invert(ref m_localToWorld, out m_worldToLocal);
		}

		/// <summary>
		/// Update the clipmap settings.
		/// </summary>
		/// <param name="settings"></param>
		public bool UpdateSettings(MyVoxelClipmapSettings settings)
		{
			if (!settings.IsValid || settings.Equals(m_settings))
			{
				return false;
			}
			using (m_lock.AcquireExclusiveUsing())
			{
				m_settings = settings;
				m_settingsChanged = true;
			}
			return true;
		}

		private void ApplySettings(bool invalidateCells)
		{
			CellBits = m_settings.CellSizeLg2;
			CellSize = 1 << CellBits;
			m_updateDistance = CellSize * CellSize / 4;
			Vector3I vector3I = m_voxelSize + CellSize - 1 >> CellBits;
			for (int i = 0; i < 16; i++)
			{
				Rings[i].UpdateSize(vector3I);
				vector3I = vector3I + 1 >> 1;
				Ranges[i] = m_settings.LodRanges[i];
			}
			m_settingsChanged = false;
			if (invalidateCells)
			{
				m_invalidateRange = BoundingBoxI.CreateInvalid();
			}
		}

		/// <summary>
		/// Verify if the given cell will contain a surface.
		/// </summary>
		/// <param name="cell">The coordinates of the cell.</param>
		/// <param name="lod">The lod of the cell.</param>
		/// <returns></returns>
		internal MyVoxelContentConstitution ApproximateCellConstitution(Vector3I cell, int lod)
		{
			BoundingBoxI box = GetCellBounds(new MyCellCoord(lod, cell), inLod: false);
			box.Min -= 1;
			IMyStorage storage = Mesher.Storage;
			if (storage == null)
			{
				return MyVoxelContentConstitution.Empty;
			}
			switch (storage.Intersect(ref box, lod))
			{
			case ContainmentType.Disjoint:
				return MyVoxelContentConstitution.Empty;
			case ContainmentType.Contains:
				return MyVoxelContentConstitution.Full;
			case ContainmentType.Intersects:
				return MyVoxelContentConstitution.Mixed;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		internal void RequestCell(Vector3I cell, int lod, VrSewGuide existingGuide = null)
		{
			MyCellCoord myCellCoord = new MyCellCoord(lod, cell);
			if (InstanceStitchMode == StitchMode.Stitch)
			{
				if (m_cache != null && m_cache.TryRead(Actor.Id, myCellCoord, out VrSewGuide data))
				{
					data.AddReference();
					m_cachedCellRequests.Add(MyTuple.Create(myCellCoord, data));
				}
				else if (m_dataWorkTracker.Exists(myCellCoord))
				{
					m_dataWorkTracker.Invalidate(myCellCoord);
				}
				else
				{
					existingGuide?.AddReference();
					MyClipmapMeshJob.Start(m_dataWorkTracker, this, myCellCoord, existingGuide);
				}
			}
			else if (m_fullWorkTracker.Exists(myCellCoord))
			{
				m_fullWorkTracker.Invalidate(myCellCoord);
			}
			else
			{
				MyClipmapFullMeshJob.Start(m_fullWorkTracker, this, myCellCoord);
			}
		}

		private void BindToCache()
		{
			if (m_cache != null && Actor != null)
			{
				m_cache.Register(Actor.Id, this);
			}
		}

		/// <summary>
		/// Process all mesh requests that hit the cache.
		///
		/// This has to be postponed until after the visibility update since feeding the mesh has many side effects.
		/// </summary>
		private void ProcessCacheHits()
		{
			int count = m_cachedCellRequests.Count;
			for (int i = 0; i < count; i++)
			{
				MyTuple<MyCellCoord, VrSewGuide> myTuple = m_cachedCellRequests[i];
				myTuple.Item2.AddReference();
				FeedMeshResult(myTuple.Item1, myTuple.Item2, MyVoxelContentConstitution.Mixed);
				myTuple.Item2.RemoveReference();
			}
			m_cachedCellRequests.Clear();
		}

		internal void HandleCacheEviction(MyCellCoord coord, VrSewGuide guide)
		{
		}

		/// <summary>
		/// Process all queued cell updates.
		/// </summary>
		///
		/// Cell render updates are queued because they deal with render data. As a result we must process them on the render thread.
		private void ProcessUpdates()
		{
			CellRenderUpdate instance;
			while (m_cellRenderUpdates.TryDequeue(out instance))
			{
				if (IsUnloaded)
				{
					if (instance.Operation != null)
					{
						CommitStitchOperation(instance.Operation);
					}
					instance.Data.Dispose();
					continue;
				}
				MyVoxelClipmapRing myVoxelClipmapRing = Rings[instance.Cell.Lod];
				myVoxelClipmapRing.UpdateCellRender(instance.Cell.CoordInLod, ref instance.Data);
				if (instance.Operation != null)
				{
					bool recalculate = instance.Operation.Recalculate;
					CommitStitchOperation(instance.Operation);
					if (recalculate)
					{
						Stitch(instance.Cell);
					}
				}
				myVoxelClipmapRing.FinishAdd(instance.Cell.CoordInLod);
			}
		}

		/// <summary>
		/// Update mesh data for a cell.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="guide"></param>
		/// <param name="constitution">The constitution of the range where the mesh was calculated.</param>
		internal void UpdateCellData(MyClipmapMeshJob job, MyCellCoord cell, VrSewGuide guide, MyVoxelContentConstitution constitution)
		{
			using (m_lock.AcquireExclusiveRecursiveUsing(m_lockOwner))
			{
				if (job.IsReusingGuide)
				{
					guide.RemoveReference();
				}
				if ((IsUnloaded || job.IsCanceled) && !job.IsReusingGuide)
				{
					guide?.RemoveReference();
				}
				else
				{
					FeedMeshResult(cell, guide, constitution);
					if (m_cache != null && guide?.Mesh != null)
					{
						m_cache.Write(Actor.Id, cell, guide);
					}
					m_dataWorkTracker.Complete(cell);
				}
			}
		}

		/// <summary>
		/// Provide the calculated mesh to it's cell.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="mesh"></param>
		/// <param name="constitution"></param>
		private void FeedMeshResult(MyCellCoord cell, VrSewGuide mesh, MyVoxelContentConstitution constitution)
		{
			MyVoxelClipmapRing myVoxelClipmapRing = Rings[cell.Lod];
			if (mesh == null && myVoxelClipmapRing.IsForwardEdge(cell.CoordInLod))
			{
				BoundingBoxI cellBounds = GetCellBounds(cell);
				mesh = new VrSewGuide(cell.Lod, cellBounds.Min, cellBounds.Max, GetShellCacheForConstitution(constitution));
			}
			myVoxelClipmapRing.UpdateCellData(cell.CoordInLod, mesh, constitution);
			ReadyAllStitchDependencies(cell);
			if (mesh != null)
			{
				Stitch(cell);
			}
		}

		/// <summary>
		/// Update render data for a cell.
		/// </summary>
		/// <param name="coord"></param>
		/// <param name="stitch"></param>
		/// <param name="cellData"></param>
		internal void UpdateCellRender(MyCellCoord coord, StitchOperation stitch, ref MyVoxelRenderCellData cellData)
		{
			if (IsUnloaded)
			{
				using (m_lock.AcquireExclusiveRecursiveUsing(m_lockOwner))
				{
					if (stitch != null)
					{
						CommitStitchOperation(stitch);
					}
				}
			}
			else
			{
				m_cellRenderUpdates.Enqueue(new CellRenderUpdate(coord, stitch, ref cellData));
			}
		}

		/// <summary>
		/// Schedule stitching for a given cell.
		///
		/// If the cell is a cell in the back of it's lod then additional jobs are scheduled to stitch it to the LODs above.
		/// Because these jobs all modify the same cell, they are all scheduled as a group in a compound stitch job.
		/// </summary>
		/// <param name="cell">The coordinate of the cell to be stitched.</param>
		internal bool Stitch(MyCellCoord cell)
		{
			if (InstanceStitchMode != StitchMode.Stitch)
			{
				return false;
			}
			MyVoxelClipmapRing myVoxelClipmapRing = Rings[cell.Lod];
			if (!myVoxelClipmapRing.TryGetCell(cell.CoordInLod, out MyVoxelClipmapRing.CellData data))
			{
				return false;
			}
			if (m_stitchWorkTracker.Exists(cell))
			{
				m_stitchWorkTracker.Invalidate(cell);
				return true;
			}
			StitchOperation stitchOperation;
			if (myVoxelClipmapRing.IsInnerLodEdge(cell.CoordInLod, out int innerCornerIndex))
			{
				CompoundStitchOperation compoundStitchOperation;
				using (m_lock.AcquireExclusiveRecursiveUsing(m_lockOwner))
				{
					compoundStitchOperation = m_compoundStitchDependencyPool.Get();
				}
				if (compoundStitchOperation == null)
				{
					return false;
				}
				compoundStitchOperation.Init(cell);
				PrepareStitch(data, cell, null, compoundStitchOperation);
				Vector3I cell2 = cell.CoordInLod + m_neighbourOffsets[innerCornerIndex];
				if (myVoxelClipmapRing.IsInsideInnerLod(cell2))
				{
					CollectChildStitch(compoundStitchOperation, data, cell, m_neighbourOffsets[innerCornerIndex]);
				}
				stitchOperation = compoundStitchOperation;
			}
			else
			{
				stitchOperation = PrepareStitch(data, cell);
			}
			if (stitchOperation != null && stitchOperation.Pending == 0)
			{
				DispatchStitch(stitchOperation);
			}
			return true;
		}

		/// <summary>
		/// Collect meshes of inner lod for child sewing operations.
		/// </summary>
		/// <param name="compound">Parent compound stitch operation.</param>
		/// <param name="parentData">Cell data for the main cell.</param>
		/// <param name="cell">Cell coordinates.</param>
		/// <param name="neighbourOffset">Offset of the neighbor to sew with.</param>
		private void CollectChildStitch(CompoundStitchOperation compound, MyVoxelClipmapRing.CellData parentData, MyCellCoord cell, Vector3I neighbourOffset)
		{
			Vector3I v = cell.CoordInLod + neighbourOffset;
			Vector3I b = Vector3I.One - neighbourOffset;
			int num = cell.Lod - 1;
			while (num >= 0)
			{
				int num2 = cell.Lod - num;
				Vector3I vector3I = v << num2;
				Vector3I vector3I2 = vector3I + ((1 << num2) - 1) * b + 1;
				if (Rings[num] != null && (Rings[num].IsInBounds(vector3I) || Rings[num].IsInBounds(vector3I2)) && Rings[num].Cells.Count() != 0)
				{
					foreach (Vector3I item in Vector3I.EnumerateRange(vector3I, vector3I2))
					{
						StitchOperation stitchOperation = m_stitchDependencyPool.Get();
						if (stitchOperation == null)
						{
							return;
						}
						stitchOperation.Init(cell);
						PrepareStitch(parentData, new MyCellCoord(num, item - neighbourOffset), compound, stitchOperation);
						compound.Children.Add(stitchOperation);
						Vector3I vector3I3 = item - vector3I << CellBits >> num2;
						Vector3I max = vector3I3 + (1 << CellBits >> num2);
						if (neighbourOffset.X == 1)
						{
							max.X = CellSize;
						}
						if (neighbourOffset.Y == 1)
						{
							max.Y = CellSize;
						}
						if (neighbourOffset.Z == 1)
						{
							max.Z = CellSize;
						}
						stitchOperation.Range = new BoundingBoxI(vector3I3, max);
					}
					num--;
					continue;
				}
				break;
			}
		}

		/// <summary>
		/// Enqueue a stitch operation based on cell status.
		/// </summary>
		/// <param name="parentData"></param>
		/// <param name="cell"></param>
		/// <param name="parent">Optional parent compound stitch operation.</param>
		/// <param name="preallocatedOperation">Optional preallocated operation. Used when setting up compound operations.</param>
		private StitchOperation PrepareStitch(MyVoxelClipmapRing.CellData parentData, MyCellCoord cell, CompoundStitchOperation parent = null, StitchOperation preallocatedOperation = null)
		{
			StitchOperation stitchOperation = preallocatedOperation;
			if (preallocatedOperation == null)
			{
				using (m_lock.AcquireExclusiveRecursiveUsing(m_lockOwner))
				{
					stitchOperation = m_stitchDependencyPool.Get();
				}
				stitchOperation.Init(cell);
			}
			StitchOperation stitchOperation2 = parent ?? stitchOperation;
			int pending = stitchOperation2.Pending;
			if (parentData.Status == MyVoxelClipmapRing.CellStatus.Pending)
			{
				stitchOperation.Dependencies[0] = stitchOperation.Cell;
				stitchOperation2.Pending++;
			}
			else
			{
				stitchOperation.Dependencies[0] = MakeFulfilled(stitchOperation.Cell);
				stitchOperation.Guides[0] = parentData.Guide;
			}
			VrSewOperation vrSewOperation = VrSewOperation.All;
			if (parent != null)
			{
				vrSewOperation = (VrSewOperation)0;
			}
			for (int i = 1; i < m_neighbourOffsets.Length; i++)
			{
				MyCellCoord cell2 = new MyCellCoord(cell.Lod, cell.CoordInLod + m_neighbourOffsets[i]);
				if (cell2.Lod != stitchOperation.Cell.Lod)
				{
					stitchOperation.BorderOperation = true;
				}
				if (TryGetCellAt(ref cell2, out MyVoxelClipmapRing.CellData data))
				{
					if (data.Status == MyVoxelClipmapRing.CellStatus.Pending)
					{
						stitchOperation.Dependencies[i] = cell2;
						stitchOperation2.Pending++;
					}
					else
					{
						stitchOperation.Guides[i] = CollectMeshForOperation(stitchOperation, cell2, data);
						stitchOperation.Dependencies[i] = MakeFulfilled(cell2);
					}
				}
				else
				{
					vrSewOperation = vrSewOperation.Without(m_compromizes[i]);
					stitchOperation.Dependencies[i] = MakeFulfilled(cell2);
				}
				if (parent != null && cell2.Lod < stitchOperation.Cell.Lod)
				{
					vrSewOperation = vrSewOperation.With(m_compromizes[i]);
				}
			}
			stitchOperation.Operation = vrSewOperation;
			int num = 0;
			if (stitchOperation2.Pending != pending)
			{
				for (int j = 0; j < stitchOperation.Dependencies.Length; j++)
				{
					MyCellCoord key = stitchOperation.Dependencies[j];
					if (key.Lod >= 0)
					{
						m_stitchDependencies.Add(key, stitchOperation2);
						num++;
					}
				}
			}
			return stitchOperation;
		}

		/// <summary>
		/// Try to find a cell with valid mesh at or above the provided position.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		private bool TryGetCellAt(ref MyCellCoord cell, out MyVoxelClipmapRing.CellData data)
		{
			do
			{
				if (Rings[cell.Lod].TryGetCell(cell.CoordInLod, out data))
				{
					return true;
				}
				cell.Lod++;
				cell.CoordInLod >>= 1;
			}
			while (cell.Lod < 16);
			data = null;
			return false;
		}

		/// <summary>
		/// Dispatch the stitch job associated with a given stitch operation.
		/// </summary>
		/// <param name="stitch"></param>
		/// <returns>Whether the stitch was dispatched.</returns>
		private void DispatchStitch(StitchOperation stitch)
		{
			CompoundStitchOperation compound = stitch.GetCompound();
			if (compound != null)
			{
				CollectMeshes(stitch);
				for (int num = compound.Children.Count - 1; num >= 0; num--)
				{
					if (!CollectMeshes(compound.Children[num], child: true))
					{
						compound.Children[num].Start();
						CommitStitchOperation(compound.Children[num]);
						compound.Children.RemoveAtFast(num);
					}
				}
			}
			else if (!CollectMeshes(stitch))
			{
				stitch.Start();
				CommitStitchOperation(stitch);
				return;
			}
			stitch.Start();
			stitch.GetCompound()?.Children.ForEach(delegate(StitchOperation x)
			{
				x.Start();
			});
			if (!m_stitchWorkTracker.Exists(stitch.Cell))
			{
				if (!MyClipmapSewJob.Start(m_stitchWorkTracker, this, stitch))
				{
					CommitStitchOperation(stitch);
				}
			}
			else
			{
				CommitStitchOperation(stitch);
			}
		}

		/// <summary>
		/// Collect mesh for an operation.
		///
		/// If the operation is a border operation a guide with constant cache is created for a cell that has no mesh.
		/// </summary>
		/// <param name="op">Operation</param>
		/// <param name="cell">Coordinates of the cell.</param>
		/// <param name="cellData">Cell to fetch the guide from.</param>
		/// <returns>Whether the cell existed and was ready.</returns>
		private VrSewGuide CollectMeshForOperation(StitchOperation op, MyCellCoord cell, MyVoxelClipmapRing.CellData cellData)
		{
			if (op.BorderOperation && cellData.Guide == null)
			{
				BoundingBoxI cellBounds = GetCellBounds(cell);
				cellData.Guide = new VrSewGuide(cell.Lod, cellBounds.Min, cellBounds.Max, GetShellCacheForConstitution(cellData.Constitution));
			}
			return cellData.Guide;
		}

		private bool CollectMeshes(StitchOperation stitch, bool child = false)
		{
			bool flag = false;
			MyVoxelClipmapRing.CellData data;
			for (int i = 0; i < stitch.Dependencies.Length; i++)
			{
				MyCellCoord cell = stitch.Dependencies[i];
				if (cell.Lod >= 0)
				{
					stitch.Dependencies[i] = MakeFulfilled(stitch.Dependencies[i]);
					if (Rings[cell.Lod].TryGetCell(cell.CoordInLod, out data))
					{
						stitch.Guides[i] = CollectMeshForOperation(stitch, cell, data);
					}
				}
				if (stitch.Guides[i] != null && stitch.Guides[i].Mesh != null)
				{
					flag = true;
				}
			}
			if (stitch.Guides[0] != null)
			{
				if (!flag)
				{
					return false;
				}
				if (!child)
				{
					MyVoxelClipmapRing.Vicinity vicinity = new MyVoxelClipmapRing.Vicinity(stitch.Guides, stitch.Dependencies);
					MyCellCoord myCellCoord = MakeFulfilled(stitch.Dependencies[0]);
					Rings[myCellCoord.Lod].TryGetCell(myCellCoord.CoordInLod, out data);
					if (data.Vicinity == vicinity)
					{
						return false;
					}
					data.Vicinity = vicinity;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// State that a dependent cell is ready.
		/// </summary>
		/// <param name="stitch"></param>
		private void ReadyStitchDependency(StitchOperation stitch)
		{
			stitch.Pending--;
			if (stitch.Pending == 0)
			{
				DispatchStitch(stitch);
			}
		}

		/// <summary>
		/// Signal a stitch operation is complete and the job data can return to the pool.
		/// </summary>
		/// <param name="stitch"></param>
		internal void CommitStitchOperation(StitchOperation stitch, bool dereference = true)
		{
			CompoundStitchOperation compound = stitch.GetCompound();
			if (compound != null)
			{
				foreach (StitchOperation child in compound.Children)
				{
					child.Clear(dereference);
					m_stitchDependencyPool.Return(child);
				}
				stitch.Clear(dereference);
				m_compoundStitchDependencyPool.Return(compound);
			}
			else
			{
				stitch.Clear(dereference);
				m_stitchDependencyPool.Return(stitch);
			}
		}

		/// <summary>
		/// Fire all visibility or normal stitch dependencies for a cell.
		/// </summary>
		/// <param name="cell"></param>
		private void ReadyAllStitchDependencies(MyCellCoord cell)
		{
			if (m_stitchDependencies.TryGet(cell, out List<StitchOperation> list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					ReadyStitchDependency(list[i]);
				}
				m_stitchDependencies.Remove(cell);
			}
		}

		/// <summary>
		/// Take a cell coordinate from a stitch operation and toggle it's fulfilled status.
		/// </summary>
		/// <param name="fullfiled"></param>
		/// <returns></returns>
		internal static MyCellCoord MakeFulfilled(MyCellCoord fullfiled)
		{
			return new MyCellCoord(~fullfiled.Lod, fullfiled.CoordInLod);
		}

		/// <summary>
		/// Given a cell, find whether it stands a the forward boundary of it's lod.
		/// </summary>
		/// <param name="constitution">The constitution of the cell.</param>
		/// <returns></returns>
		private VrShellDataCache GetShellCacheForConstitution(MyVoxelContentConstitution constitution)
		{
			if (constitution != 0)
			{
				return VrShellDataCache.Full;
			}
			return VrShellDataCache.Empty;
		}

		public void Update(ref MatrixD view, BoundingFrustumD viewFrustum, float farClipping)
		{
			if (Mesher.Storage != null && m_updateState != 0)
			{
				using (m_lock.AcquireExclusiveRecursiveUsing(m_lockOwner))
				{
					ProcessUpdates();
					if (!WorkPending)
					{
						InstanceStitchMode = ActiveStitchMode;
						switch (m_updateState)
						{
						case UpdateState.Idle:
							if (m_settingsChanged)
							{
								ApplySettings(invalidateCells: true);
							}
							else if (UpdateVisibility && !MyRenderProxy.Settings.FreezeTerrainQueries && MoveUpdate(ref view, viewFrustum, farClipping))
							{
								m_updateState = UpdateState.Calculate;
							}
							else if (m_invalidateRange.HasValue)
							{
								InvalidateInternal(m_invalidateRange.Value);
								m_invalidateRange = null;
							}
							else if (!object.Equals((int)m_loadedCounter, 0) && this.m_loaded != null)
							{
								Interlocked.Decrement(ref m_loadedCounter);
								if (object.Equals((int)m_loadedCounter, 0))
								{
									MyRenderProxy.EnqueueMainThreadCallback(delegate
									{
										this.m_loaded?.Invoke(this);
									});
								}
							}
							break;
						case UpdateState.Calculate:
							if (!WorkPending)
							{
								m_updateState = UpdateState.Idle;
								bool flag = this.m_loaded != null;
								if (flag)
								{
									MyRenderProxy.EnqueueMainThreadCallback(delegate
									{
										this.m_loaded?.Invoke(this);
									});
								}
								_ = m_cache;
								Actor.EndBatch(flag);
							}
							break;
						}
					}
				}
			}
		}

		private bool MoveUpdate(ref MatrixD view, BoundingFrustumD viewFrustum, float farClipping)
		{
			Vector3D vector3D = Vector3D.Transform(view.Translation, m_worldToLocal) / 1.0;
			if (Vector3D.DistanceSquared(vector3D, m_lastPosition) >= (double)m_updateDistance)
			{
				Vector3L relativePosition = m_lastPosition = new Vector3L(vector3D);
				if (!Actor.IsBatching)
				{
					Actor.BeginBatch(MyVoxelActorTransitionMode.Fade);
				}
				foreach (MyVoxelClipmapRing ring in Rings)
				{
					ring.Update(relativePosition);
				}
				foreach (MyVoxelClipmapRing ring2 in Rings)
				{
					ring2.ProcessChanges();
				}
				foreach (MyVoxelClipmapRing ring3 in Rings)
				{
					ring3.DispatchStitchingRefreshes();
				}
				ProcessCacheHits();
				return true;
			}
			return false;
		}

		public void BindToActor(IMyVoxelActor actor)
		{
			if (Actor != null)
			{
				throw new InvalidOperationException("Lod Controller is already bound to actor.");
			}
			Actor = actor;
			Actor.TransitionMode = MyVoxelActorTransitionMode.Fade;
			Actor.Move += UpdateWorldMatrix;
			BindToCache();
			m_updateState = UpdateState.Idle;
		}

		public void Unload()
		{
			m_dataWorkTracker.CancelAll();
			m_fullWorkTracker.CancelAll();
			m_stitchWorkTracker.CancelAll();
			using (m_lock.AcquireExclusiveRecursiveUsing(m_lockOwner))
			{
				m_updateState = UpdateState.Unloaded;
				Actor.Move -= UpdateWorldMatrix;
				ProcessUpdates();
				foreach (StitchOperation item in from x in m_stitchDependencies.Values.SelectMany((List<StitchOperation> x) => x)
					group x by x into x
					select x.Key)
				{
					item.Pending = 0;
					CommitStitchOperation(item, dereference: false);
				}
				foreach (MyVoxelClipmapRing ring in Rings)
				{
					ring.InvalidateAll();
				}
				m_stitchDependencies.Clear();
				if (m_cache != null)
				{
					m_cache.Unregister(Actor.Id);
				}
				if (this.m_loaded != null)
				{
					MyRenderProxy.EnqueueMainThreadCallback(delegate
					{
						this.m_loaded?.Invoke(this);
					});
				}
			}
		}

		public void InvalidateRange(Vector3I min, Vector3I max)
		{
			BoundingBoxI boundingBoxI = new BoundingBoxI(min - 1, max + 1);
			using (m_lock.AcquireExclusiveRecursiveUsing(m_lockOwner))
			{
				if (m_updateState != 0)
				{
					if (m_invalidateRange.HasValue)
					{
						m_invalidateRange = m_invalidateRange.Value.Include(boundingBoxI);
					}
					else
					{
						m_invalidateRange = boundingBoxI;
					}
				}
			}
		}

		private void InvalidateInternal(BoundingBoxI bounds)
		{
			if (bounds == BoundingBoxI.CreateInvalid())
			{
				if (m_cache != null)
				{
					m_cache.EvictAll(Actor.Id);
				}
				Actor.BeginBatch(MyVoxelActorTransitionMode.Fade);
				foreach (MyVoxelClipmapRing ring in Rings)
				{
					ring.InvalidateAll();
				}
				m_lastPosition = Vector3I.MinValue;
			}
			else
			{
				if (m_cache != null)
				{
					m_cache.EvictAll(Actor.Id, new BoundingBoxI(bounds.Min >> CellBits, bounds.Max + (CellSize - 1) >> CellBits));
				}
				Actor.BeginBatch(MyVoxelActorTransitionMode.Immediate);
				foreach (MyVoxelClipmapRing ring2 in Rings)
				{
					ring2.InvalidateRange(bounds);
				}
				m_updateState = UpdateState.Calculate;
			}
		}

		public void InvalidateAll()
		{
			using (m_lock.AcquireExclusiveRecursiveUsing(m_lockOwner))
			{
				if (m_updateState != 0)
				{
					m_invalidateRange = BoundingBoxI.CreateInvalid();
				}
			}
		}

		~MyVoxelClipmap()
		{
		}

		/// <param name="cameraMatrix"></param>
		/// <inheritdoc />
		public void DebugDraw(ref MatrixD cameraMatrix)
		{
			using (m_lock.AcquireExclusiveRecursiveUsing(m_lockOwner))
			{
				foreach (MyVoxelClipmapRing ring in Rings)
				{
					ring.DebugDraw();
				}
				if (DebugDrawDependencies)
				{
					DebugDrawDependenciesInternal();
				}
			}
		}

		/// <summary>
		/// Debug draw mesh dependencies.
		/// </summary>
		private void DebugDrawDependenciesInternal()
		{
			_ = m_lastPosition;
			foreach (KeyValuePair<MyCellCoord, List<StitchOperation>> stitchDependency in m_stitchDependencies)
			{
				Vector3D cellCenter = GetCellCenter(stitchDependency.Key);
				float num = 1f;
				if (!(num < 0f))
				{
					Color color = new Color(Color.Orange, num);
					MyCellCoord key = stitchDependency.Key;
					if (Rings[key.Lod].TryGetCell(key.CoordInLod, out MyVoxelClipmapRing.CellData data))
					{
						MyRenderProxy.DebugDrawText3D(cellCenter, $"Status: {data.Status}, Constitution: {data.Constitution}", color, 0.7f, depthRead: true);
					}
					foreach (StitchOperation item in stitchDependency.Value)
					{
						Vector3D cellCenter2 = GetCellCenter(item.Cell);
						color = new Color(Color.Orange, num);
						MyRenderProxy.DebugDrawArrow3D(cellCenter2, cellCenter, color, color, depthRead: true);
						MyRenderProxy.DebugDrawText3D(cellCenter2, $"Stitch dependency \npending: {item.Pending}", color, 0.7f, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
					}
				}
			}
		}

		private Vector3D GetCellCenter(MyCellCoord cell)
		{
			return Vector3D.Transform((Vector3D)(cell.CoordInLod * CellSize + (CellSize >> 1) << cell.Lod), LocalToWorld);
		}

		/// <summary>
		/// Get a cached mesh based on a position contained in it and it's desired lod.
		/// You are not becoming owner of the mesh and you are not allowed to keep it!
		/// </summary>
		/// <param name="coord"></param>
		/// <param name="lod"></param>
		/// <returns></returns>
		public VrVoxelMesh GetCachedMesh(Vector3I coord, int lod)
		{
			using (m_lock.AcquireSharedUsing())
			{
				MyVoxelClipmapRing myVoxelClipmapRing = Rings[lod];
				coord >>= lod + CellBits;
				if (myVoxelClipmapRing.TryGetCell(coord, out MyVoxelClipmapRing.CellData data) && data.Guide != null)
				{
					return data.Guide.Mesh;
				}
			}
			return null;
		}

		/// <summary>
		/// Look for any loaded mesh containing the provided position.
		/// You are not becoming owner of the mesh and you are not allowed to keep it!
		/// </summary>
		/// <param name="coord"></param>
		/// <returns></returns>
		public VrVoxelMesh GetCachedMesh(Vector3I coord)
		{
			using (m_lock.AcquireSharedUsing())
			{
				for (int i = 0; i < 16; i++)
				{
					MyVoxelClipmapRing myVoxelClipmapRing = Rings[i];
					Vector3I cell = coord >> i + CellBits;
					if (myVoxelClipmapRing.TryGetCell(cell, out MyVoxelClipmapRing.CellData data) && data.Guide != null)
					{
						return data.Guide.Mesh;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Calculate the boundaries of a render cell based on it's coordinates.
		///
		/// The resulting bounding box is an _inclusive_ range.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="inLod">When set to true the resulting bounds will be relative to the lod of the cell. Otherwise they will be an absolute range relative to lod0</param>
		/// <returns></returns>
		public BoundingBoxI GetCellBounds(MyCellCoord cell, bool inLod = true)
		{
			int cellSize = CellSize;
			Vector3I vector3I = cell.CoordInLod * cellSize;
			Vector3I max = vector3I + cellSize;
			switch (InstanceStitchMode)
			{
			case StitchMode.BlindMeet:
				max += 1;
				break;
			case StitchMode.Overlap:
				max += 2;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case StitchMode.None:
			case StitchMode.Stitch:
				break;
			}
			if (!inLod)
			{
				vector3I <<= cell.Lod;
				max <<= cell.Lod;
			}
			return new BoundingBoxI(vector3I, max);
		}
	}
}
