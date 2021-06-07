using Havok;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.Generics;
using VRage.Network;
using VRage.Voxels;

namespace Sandbox.Engine.Physics
{
	public class MyBreakableShapeCloneJob : MyPrecalcJob
	{
		public struct Args
		{
			public MyWorkTracker<MyDefinitionId, MyBreakableShapeCloneJob> Tracker;

			public string Model;

			public MyDefinitionId DefId;

			public HkdBreakableShape ShapeToClone;

			public int Count;
		}

		private class Sandbox_Engine_Physics_MyBreakableShapeCloneJob_003C_003EActor : IActivator, IActivator<MyBreakableShapeCloneJob>
		{
			private sealed override object CreateInstance()
			{
				return new MyBreakableShapeCloneJob();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBreakableShapeCloneJob CreateInstance()
			{
				return new MyBreakableShapeCloneJob();
			}

			MyBreakableShapeCloneJob IActivator<MyBreakableShapeCloneJob>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyDynamicObjectPool<MyBreakableShapeCloneJob> m_instancePool = new MyDynamicObjectPool<MyBreakableShapeCloneJob>(16);

		private Args m_args;

		private List<HkdBreakableShape> m_clonedShapes = new List<HkdBreakableShape>();

		private bool m_isCanceled;

		public static void Start(Args args)
		{
			MyBreakableShapeCloneJob myBreakableShapeCloneJob = m_instancePool.Allocate();
			myBreakableShapeCloneJob.m_args = args;
			args.Tracker.Add(args.DefId, myBreakableShapeCloneJob);
			MyPrecalcComponent.EnqueueBack(myBreakableShapeCloneJob);
		}

		public MyBreakableShapeCloneJob()
			: base(enableCompletionCallback: true)
		{
		}

		public override void DoWork()
		{
			for (int i = 0; i < m_args.Count && (!m_isCanceled || i <= 0); i++)
			{
				m_clonedShapes.Add(m_args.ShapeToClone.Clone());
			}
		}

		public override void Cancel()
		{
			m_isCanceled = true;
		}

		protected override void OnComplete()
		{
			base.OnComplete();
			if (MyDestructionData.Static != null && MyDestructionData.Static.BlockShapePool != null)
			{
				MyDestructionData.Static.BlockShapePool.EnqueShapes(m_args.Model, m_args.DefId, m_clonedShapes);
			}
			m_clonedShapes.Clear();
			m_args.Tracker.Complete(m_args.DefId);
			m_args = default(Args);
			m_isCanceled = false;
			m_instancePool.Deallocate(this);
		}
	}
}
