using ParallelTasks;
using System;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Voxels
{
	[GenerateActivator]
	public abstract class MyPrecalcJob
	{
		public readonly Action OnCompleteDelegate;

		/// <summary>
		/// Invalid tasks finishes normally and restarts afterwards. Even if results
		/// are not valid, they may still be useful.
		/// </summary>
		public bool IsValid;

		public volatile bool Started;

		public virtual bool IsCanceled => false;

		public WorkOptions Options => Parallel.DefaultOptions;

		public virtual int Priority => 0;

		protected MyPrecalcJob(bool enableCompletionCallback)
		{
			if (enableCompletionCallback)
			{
				OnCompleteDelegate = OnComplete;
			}
		}

		public void DoWorkInternal()
		{
			Started = true;
			DoWork();
		}

		public abstract void DoWork();

		public abstract void Cancel();

		protected virtual void OnComplete()
		{
		}

		public virtual void DebugDraw(Color c)
		{
		}
	}
}
