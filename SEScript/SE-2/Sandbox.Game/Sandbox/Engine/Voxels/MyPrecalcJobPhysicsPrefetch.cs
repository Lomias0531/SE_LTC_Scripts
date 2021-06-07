using Havok;
using VRage.Game.Voxels;
using VRage.Generics;
using VRage.Network;
using VRage.Voxels;
using VRage.Voxels.Mesh;

namespace Sandbox.Engine.Voxels
{
	internal sealed class MyPrecalcJobPhysicsPrefetch : MyPrecalcJob
	{
		public struct Args
		{
			public MyWorkTracker<MyCellCoord, MyPrecalcJobPhysicsPrefetch> Tracker;

			public IMyStorage Storage;

			public MyCellCoord GeometryCell;

			public MyVoxelPhysicsBody TargetPhysics;
		}

		private class Sandbox_Engine_Voxels_MyPrecalcJobPhysicsPrefetch_003C_003EActor : IActivator, IActivator<MyPrecalcJobPhysicsPrefetch>
		{
			private sealed override object CreateInstance()
			{
				return new MyPrecalcJobPhysicsPrefetch();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPrecalcJobPhysicsPrefetch CreateInstance()
			{
				return new MyPrecalcJobPhysicsPrefetch();
			}

			MyPrecalcJobPhysicsPrefetch IActivator<MyPrecalcJobPhysicsPrefetch>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyDynamicObjectPool<MyPrecalcJobPhysicsPrefetch> m_instancePool = new MyDynamicObjectPool<MyPrecalcJobPhysicsPrefetch>(16);

		private Args m_args;

		private volatile bool m_isCancelled;

		public int Taken;

		public volatile bool ResultComplete;

		public HkBvCompressedMeshShape Result;

		public override bool IsCanceled => m_isCancelled;

		public MyPrecalcJobPhysicsPrefetch()
			: base(enableCompletionCallback: true)
		{
		}

		public static void Start(Args args)
		{
			MyPrecalcJobPhysicsPrefetch myPrecalcJobPhysicsPrefetch = m_instancePool.Allocate();
			myPrecalcJobPhysicsPrefetch.m_args = args;
			args.Tracker.Add(args.GeometryCell, myPrecalcJobPhysicsPrefetch);
			MyPrecalcComponent.EnqueueBack(myPrecalcJobPhysicsPrefetch);
		}

		public override void DoWork()
		{
			try
			{
				if (!m_isCancelled)
				{
					VrVoxelMesh vrVoxelMesh = m_args.TargetPhysics.CreateMesh(m_args.GeometryCell);
					try
					{
						if (!vrVoxelMesh.IsEmpty())
						{
							if (m_isCancelled)
							{
								return;
							}
							Result = m_args.TargetPhysics.CreateShape(vrVoxelMesh, m_args.GeometryCell.Lod);
						}
					}
					finally
					{
						vrVoxelMesh?.Dispose();
					}
					ResultComplete = true;
				}
			}
			finally
			{
			}
		}

		protected override void OnComplete()
		{
			base.OnComplete();
			if (!m_isCancelled && m_args.TargetPhysics.Entity != null)
			{
				m_args.TargetPhysics.OnTaskComplete(m_args.GeometryCell, Result);
			}
			if (!m_isCancelled)
			{
				m_args.Tracker.Complete(m_args.GeometryCell);
			}
			if (!Result.Base.IsZero && Taken == 0)
			{
				Result.Base.RemoveReference();
			}
			Taken = 0;
			m_args = default(Args);
			m_isCancelled = false;
			ResultComplete = false;
			Result = (HkBvCompressedMeshShape)HkShape.Empty;
			m_instancePool.Deallocate(this);
		}

		public override void Cancel()
		{
			m_isCancelled = true;
		}
	}
}
