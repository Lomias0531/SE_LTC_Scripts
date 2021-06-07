using ParallelTasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Utils;
using VRage.Render.Scene.Components;
using VRage.Utils;

namespace VRage.Render.Scene
{
	public class MyActorUpdater
	{
		private struct MyDelayedCall
		{
			public MyTimeSpan CallTime;

			public Action Call;
		}

		private class StateChangeCollector<T>
		{
			private bool m_stateChanged;

			private readonly HashSet<T> m_targetSet;

			private readonly ConcurrentDictionary<T, bool> m_changeLog = new ConcurrentDictionary<T, bool>();

			public StateChangeCollector(HashSet<T> mTargetSet)
			{
				m_targetSet = mTargetSet;
			}

			public void StateChanged(T instance, bool add)
			{
				if (m_targetSet.Contains(instance) != add)
				{
					m_stateChanged = true;
					m_changeLog[instance] = add;
				}
			}

			public void Commit()
			{
				if (m_stateChanged)
				{
					m_stateChanged = false;
					foreach (KeyValuePair<T, bool> item in m_changeLog)
					{
						m_changeLog.Remove(item.Key);
						if (item.Value)
						{
							m_targetSet.Add(item.Key);
						}
						else
						{
							m_targetSet.Remove(item.Key);
						}
					}
				}
			}
		}

		private HashSet<MyActor> m_pendingUpdate = new HashSet<MyActor>();

		private HashSet<MyActor> m_pendingUpdateProcessed = new HashSet<MyActor>();

		private readonly HashSet<MyActor> m_alwaysUpdateActors = new HashSet<MyActor>();

		private readonly HashSet<MyActorComponent> m_alwaysUpdateComponents = new HashSet<MyActorComponent>();

		private readonly HashSet<MyActorComponent> m_alwaysUpdateComponentsParallel = new HashSet<MyActorComponent>();

		private readonly StateChangeCollector<MyActorComponent> m_alwaysUpdateComponentsParallelCollector;

		private bool m_isUpdatingParallel;

		[ThreadStatic]
		private static HashSet<MyActor> m_pendingUpdateCache;

		private readonly List<HashSet<MyActor>> m_pendingCaches = new List<HashSet<MyActor>>();

		private readonly List<MyDelayedCall> m_delayedCalls = new List<MyDelayedCall>();

		public MyActorUpdater()
		{
			m_alwaysUpdateComponentsParallelCollector = new StateChangeCollector<MyActorComponent>(m_alwaysUpdateComponentsParallel);
		}

		public void CallIn(Action what, MyTimeSpan delay)
		{
			MyTimeSpan callTime = new MyTimeSpan(Stopwatch.GetTimestamp()) + delay;
			lock (m_delayedCalls)
			{
				m_delayedCalls.Add(new MyDelayedCall
				{
					Call = what,
					CallTime = callTime
				});
			}
		}

		public void DestroyNextFrame(MyActor actor)
		{
			DestroyIn(actor, MyTimeSpan.Zero);
		}

		public void DestroyIn(MyActor actor, MyTimeSpan delay)
		{
			CallIn(delegate
			{
				if (!actor.IsDestroyed)
				{
					actor.Destruct();
				}
			}, delay);
		}

		public void ForceDelayedCalls()
		{
			UpdateDelayedCalls(MyTimeSpan.MaxValue);
		}

		public void UpdateDelayedCalls(MyTimeSpan currentTime)
		{
			lock (m_delayedCalls)
			{
				int num = 0;
				while (num < m_delayedCalls.Count)
				{
					MyDelayedCall myDelayedCall = m_delayedCalls[num];
					if (currentTime >= myDelayedCall.CallTime)
					{
						myDelayedCall.Call();
						m_delayedCalls.RemoveAtFast(num);
					}
					else
					{
						num++;
					}
				}
			}
		}

		public void Update()
		{
			MyTimeSpan currentTime = new MyTimeSpan(Stopwatch.GetTimestamp());
			UpdateDelayedCalls(currentTime);
			m_isUpdatingParallel = true;
			Parallel.ForEach(m_alwaysUpdateComponentsParallel, delegate(MyActorComponent c)
			{
				c.OnUpdateBeforeDraw();
			}, WorkPriority.VeryHigh, null, blocking: true);
			m_isUpdatingParallel = false;
			foreach (MyActorComponent alwaysUpdateComponent in m_alwaysUpdateComponents)
			{
				alwaysUpdateComponent.OnUpdateBeforeDraw();
			}
			m_alwaysUpdateComponentsParallelCollector.Commit();
			foreach (HashSet<MyActor> pendingCach in m_pendingCaches)
			{
				foreach (MyActor item in pendingCach)
				{
					AddToNextUpdate(item);
				}
				pendingCach.Clear();
			}
			foreach (MyActor alwaysUpdateActor in m_alwaysUpdateActors)
			{
				alwaysUpdateActor.UpdateBeforeDraw();
			}
			while (m_pendingUpdate.Count > 0)
			{
				MyUtils.Swap(ref m_pendingUpdateProcessed, ref m_pendingUpdate);
				foreach (MyActor item2 in m_pendingUpdateProcessed)
				{
					item2.UpdateBeforeDraw();
				}
				m_pendingUpdateProcessed.Clear();
			}
		}

		public void AddForParallelUpdate(MyActorComponent component)
		{
			if (m_isUpdatingParallel)
			{
				m_alwaysUpdateComponentsParallelCollector.StateChanged(component, add: true);
			}
			else
			{
				m_alwaysUpdateComponentsParallel.Add(component);
			}
		}

		public void RemoveFromParallelUpdate(MyActorComponent component)
		{
			if (m_isUpdatingParallel)
			{
				m_alwaysUpdateComponentsParallelCollector.StateChanged(component, add: false);
			}
			else
			{
				m_alwaysUpdateComponentsParallel.Remove(component);
			}
		}

		public void AddToNextUpdate(MyActor actor)
		{
			if (actor.AlwaysUpdate)
			{
				return;
			}
			if (m_isUpdatingParallel)
			{
				HashSet<MyActor> hashSet = m_pendingUpdateCache;
				if (hashSet == null)
				{
					hashSet = (m_pendingUpdateCache = new HashSet<MyActor>());
					lock (m_pendingCaches)
					{
						m_pendingCaches.Add(hashSet);
					}
				}
				hashSet.Add(actor);
			}
			else
			{
				m_pendingUpdate.Add(actor);
			}
		}

		public void AddToAlwaysUpdate(MyActorComponent component)
		{
			m_alwaysUpdateComponents.Add(component);
		}

		public void RemoveFromAlwaysUpdate(MyActorComponent component)
		{
			m_alwaysUpdateComponents.Remove(component);
		}

		public void AddToAlwaysUpdate(MyActor actor)
		{
			m_alwaysUpdateActors.Add(actor);
		}

		public void RemoveFromAlwaysUpdate(MyActor actor)
		{
			m_alwaysUpdateActors.Remove(actor);
		}

		public void RemoveFromUpdates(MyActor actor)
		{
			m_alwaysUpdateActors.Remove(actor);
			m_pendingUpdate.Remove(actor);
		}
	}
}
