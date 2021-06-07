using System;
using VRage;
using VRage.Collections;

namespace ParallelTasks
{
	public class DependencyResolver : IDisposable
	{
		public struct JobToken
		{
			private readonly int m_jobId;

			private readonly DependencyResolver m_solver;

			public JobToken(int jobId, DependencyResolver solver)
			{
				m_jobId = jobId;
				m_solver = solver;
			}

			public JobToken Starts(JobToken child)
			{
				child.DependsOn(this);
				return this;
			}

			public JobToken DependsOn(JobToken parent)
			{
				int jobId = m_jobId;
				int jobId2 = parent.m_jobId;
				if (jobId2 == jobId)
				{
					throw new Exception("Cannot start/depend on itself");
				}
				m_solver.AddDependency(jobId2, jobId);
				return this;
			}
		}

		private static readonly MyConcurrentArrayBufferPool<int> m_pool = new MyConcurrentArrayBufferPool<int>("DependencySolver");

		private readonly DependencyBatch m_batch;

		private readonly MyTuple<int[], int>[] m_dependencies;

		public DependencyResolver(DependencyBatch batch)
		{
			m_batch = batch;
			m_dependencies = new MyTuple<int[], int>[500];
		}

		public JobToken Add(Action job)
		{
			int jobId = m_batch.Add(job);
			return new JobToken(jobId, this);
		}

		private void AddDependency(int parent, int child)
		{
			int num = m_dependencies[parent].Item2++;
			int[] arr = m_dependencies[parent].Item1;
			if (arr == null || arr.Length == num)
			{
				Resize(ref arr);
				m_dependencies[parent].Item1 = arr;
			}
			arr[num] = child;
		}

		private static void Resize(ref int[] arr)
		{
			bool flag = arr == null;
			int bucketId = flag ? 8 : (arr.Length + 1);
			int[] array = m_pool.Get(bucketId);
			if (!flag)
			{
				Array.Copy(arr, array, arr.Length);
				m_pool.Return(arr);
			}
			arr = array;
		}

		public void Dispose()
		{
			for (int i = 0; i < m_dependencies.Length; i++)
			{
				MyTuple<int[], int> myTuple = m_dependencies[i];
				int item = myTuple.Item2;
				if (item > 0)
				{
					using (DependencyBatch.StartToken startToken = m_batch.Job(i))
					{
						int[] item2 = myTuple.Item1;
						for (int j = 0; j < item; j++)
						{
							startToken.Starts(item2[j]);
						}
						m_pool.Return(item2);
					}
					m_dependencies[i] = default(MyTuple<int[], int>);
				}
			}
		}
	}
}
