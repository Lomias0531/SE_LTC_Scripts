using System;
using VRage.Collections;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels.DualContouring;
using VRage.Voxels.Sewing;
using VRageMath;

namespace VRage.Voxels.Clipmap
{
	public sealed class MyClipmapMeshJob : MyPrecalcJob
	{
		private class VRage_Voxels_Clipmap_MyClipmapMeshJob_003C_003EActor : IActivator, IActivator<MyClipmapMeshJob>
		{
			private sealed override object CreateInstance()
			{
				return new MyClipmapMeshJob();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyClipmapMeshJob CreateInstance()
			{
				return new MyClipmapMeshJob();
			}

			MyClipmapMeshJob IActivator<MyClipmapMeshJob>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyConcurrentPool<MyClipmapMeshJob> m_instancePool = new MyConcurrentPool<MyClipmapMeshJob>(16);

		/// <summary>
		/// Resulting mesh.
		/// </summary>
		private VrSewGuide m_meshAndGuide;

		private MyVoxelContentConstitution m_resultConstitution;

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
		public MyWorkTracker<MyCellCoord, MyClipmapMeshJob> WorkTracker
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

		public bool IsReusingGuide => m_meshAndGuide != null;

		public MyClipmapMeshJob()
			: base(enableCompletionCallback: true)
		{
		}

		public static bool Start(MyWorkTracker<MyCellCoord, MyClipmapMeshJob> tracker, MyVoxelClipmap clipmap, MyCellCoord cell, VrSewGuide existingGuide = null)
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
			MyClipmapMeshJob myClipmapMeshJob = m_instancePool.Get();
			myClipmapMeshJob.m_isCancelled = false;
			myClipmapMeshJob.Clipmap = clipmap;
			myClipmapMeshJob.Cell = cell;
			myClipmapMeshJob.WorkTracker = tracker;
			myClipmapMeshJob.m_meshAndGuide = existingGuide;
			return myClipmapMeshJob.Enqueue();
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
				BoundingBoxI cellBounds = Clipmap.GetCellBounds(Cell);
				VrSewGuide vrSewGuide = m_meshAndGuide;
				MyMesherResult myMesherResult = Clipmap.Mesher.CalculateMesh(Cell.Lod, cellBounds.Min, cellBounds.Max, MyStorageDataTypeFlags.ContentAndMaterial, (MyVoxelRequestFlags)0, vrSewGuide?.Mesh);
				m_resultConstitution = myMesherResult.Constitution;
				if (myMesherResult.Constitution == MyVoxelContentConstitution.Mixed)
				{
					MyStorageData storageData = MyDualContouringMesher.Static.StorageData;
					VrShellDataCache dataCache = VrShellDataCache.FromDataCube(storageData.Size3D, storageData[MyStorageDataTypeEnum.Content], storageData[MyStorageDataTypeEnum.Material]);
					if (vrSewGuide != null)
					{
						vrSewGuide.SetMesh(myMesherResult.Mesh, dataCache);
					}
					else
					{
						vrSewGuide = new VrSewGuide(myMesherResult.Mesh, dataCache);
					}
				}
				else if (vrSewGuide != null)
				{
					MyStorageData storageData2 = MyDualContouringMesher.Static.StorageData;
					VrShellDataCache dataCache2 = VrShellDataCache.FromDataCube(storageData2.Size3D, storageData2[MyStorageDataTypeEnum.Content], storageData2[MyStorageDataTypeEnum.Material]);
					vrSewGuide.SetMesh(vrSewGuide.Mesh, dataCache2);
				}
				Clipmap.UpdateCellData(this, Cell, vrSewGuide, m_resultConstitution);
			}
			finally
			{
				m_meshAndGuide = null;
			}
		}

		protected override void OnComplete()
		{
			base.OnComplete();
			if (IsReusingGuide)
			{
				m_meshAndGuide.RemoveReference();
				m_meshAndGuide = null;
			}
			m_instancePool.Return(this);
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
