using System;
using VRage.Collections;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels.DualContouring;
using VRageMath;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
	public class MyClipmapFullMeshJob : MyPrecalcJob
	{
		private class VRage_Voxels_Clipmap_MyClipmapFullMeshJob_003C_003EActor : IActivator, IActivator<MyClipmapFullMeshJob>
		{
			private sealed override object CreateInstance()
			{
				return new MyClipmapFullMeshJob();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyClipmapFullMeshJob CreateInstance()
			{
				return new MyClipmapFullMeshJob();
			}

			MyClipmapFullMeshJob IActivator<MyClipmapFullMeshJob>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyConcurrentPool<MyClipmapFullMeshJob> m_instancePool = new MyConcurrentPool<MyClipmapFullMeshJob>(16);

		/// <summary>
		/// Resulting mesh.
		/// </summary>
		private MyVoxelRenderCellData m_cellData;

		private volatile bool m_isCancelled;

		/// <summary>
		/// Target clipmap.
		/// </summary>
		public MyVoxelClipmap Clipmap
		{
			get;
			private set;
		}

		/// <summary>
		/// Clipmap cell.
		/// </summary>
		public MyCellCoord Cell
		{
			get;
			private set;
		}

		/// <summary>
		/// Work tracker.
		/// </summary>
		public MyWorkTracker<MyCellCoord, MyClipmapFullMeshJob> WorkTracker
		{
			get;
			private set;
		}

		public override bool IsCanceled => m_isCancelled;

		public override int Priority
		{
			get
			{
				if (!m_isCancelled)
				{
					return Cell.Lod;
				}
				return int.MaxValue;
			}
		}

		public MyClipmapFullMeshJob()
			: base(enableCompletionCallback: true)
		{
		}

		public static bool Start(MyWorkTracker<MyCellCoord, MyClipmapFullMeshJob> tracker, MyVoxelClipmap clipmap, MyCellCoord cell)
		{
			if (tracker == null)
			{
				throw new ArgumentNullException("tracker");
			}
			if (clipmap == null)
			{
				throw new ArgumentNullException("clipmap");
			}
			if (tracker.Exists(cell))
			{
				MyLog.Default.Error("A Stitch job for cell {0} is already scheduled.", cell);
				return false;
			}
			MyClipmapFullMeshJob myClipmapFullMeshJob = m_instancePool.Get();
			myClipmapFullMeshJob.m_isCancelled = false;
			myClipmapFullMeshJob.Clipmap = clipmap;
			myClipmapFullMeshJob.Cell = cell;
			myClipmapFullMeshJob.WorkTracker = tracker;
			return myClipmapFullMeshJob.Enqueue();
		}

		private bool Enqueue()
		{
			WorkTracker.Add(Cell, this);
			if (!MyPrecalcComponent.EnqueueBack(this))
			{
				WorkTracker.Complete(Cell);
				return false;
			}
			return true;
		}

		public override void DoWork()
		{
			try
			{
				if (!m_isCancelled)
				{
					BoundingBoxI cellBounds = Clipmap.GetCellBounds(Cell);
					MyMesherResult myMesherResult = Clipmap.Mesher.CalculateMesh(Cell.Lod, cellBounds.Min, cellBounds.Max);
					if (m_isCancelled || !myMesherResult.MeshProduced)
					{
						m_cellData = default(MyVoxelRenderCellData);
					}
					else
					{
						MyRenderDataBuilder.Instance.Build(myMesherResult.Mesh, out m_cellData, Clipmap.VoxelRenderDataProcessorProvider);
					}
					if (myMesherResult.MeshProduced)
					{
						myMesherResult.Mesh.Dispose();
					}
				}
			}
			finally
			{
			}
		}

		protected override void OnComplete()
		{
			base.OnComplete();
			bool flag = false;
			if (!m_isCancelled && Clipmap.Mesher != null)
			{
				Clipmap.UpdateCellRender(Cell, null, ref m_cellData);
				if (!IsValid)
				{
					flag = true;
				}
			}
			if (!m_isCancelled)
			{
				WorkTracker.Complete(Cell);
			}
			m_cellData = default(MyVoxelRenderCellData);
			if (flag)
			{
				Enqueue();
			}
			else
			{
				m_instancePool.Return(this);
			}
		}

		public override void Cancel()
		{
			m_isCancelled = true;
		}

		public override void DebugDraw(Color c)
		{
		}
	}
}
