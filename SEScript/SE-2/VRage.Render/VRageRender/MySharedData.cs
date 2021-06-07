using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Library.Threading;
using VRage.Library.Utils;

namespace VRageRender
{
	/// <summary>
	/// Data shared between render and update
	/// </summary>
	public class MySharedData
	{
		private readonly SpinLockRef m_lock = new SpinLockRef();

		private readonly MySwapQueue<HashSet<uint>> m_outputVisibleObjects = MySwapQueue.Create<HashSet<uint>>();

		private readonly MyMessageQueue m_outputRenderMessages = new MyMessageQueue();

		private readonly MyUpdateData m_inputRenderMessages = new MyUpdateData();

		private readonly MySwapQueue<MyBillboardBatch<MyBillboard>> m_inputBillboards = MySwapQueue.Create<MyBillboardBatch<MyBillboard>>();

		private readonly MySwapQueue<MyBillboardBatch<MyTriangleBillboard>> m_inputTriangleBillboards = MySwapQueue.Create<MyBillboardBatch<MyTriangleBillboard>>();

		private readonly ConcurrentCachingList<MyBillboard> m_persistentBillboards = new ConcurrentCachingList<MyBillboard>();

		public MyUpdateFrame MessagesForNextFrame = new MyUpdateFrame();

		public MySwapQueue<MyBillboardBatch<MyBillboard>> Billboards => m_inputBillboards;

		public MySwapQueue<MyBillboardBatch<MyTriangleBillboard>> TriangleBillboards => m_inputTriangleBillboards;

		public MySwapQueue<HashSet<uint>> VisibleObjects => m_outputVisibleObjects;

		public MyUpdateFrame CurrentUpdateFrame => m_inputRenderMessages.CurrentUpdateFrame;

		public MyMessageQueue RenderOutputMessageQueue => m_outputRenderMessages;

		public int PersistentBillboardsCount => m_persistentBillboards.Count;

		/// <summary>
		/// Refresh data from render (visible objects, render messages)
		/// </summary>
		public void BeforeUpdate()
		{
			using (m_lock.Acquire())
			{
				m_outputVisibleObjects.RefreshRead();
				m_outputRenderMessages.Commit();
			}
		}

		public void AfterUpdate(MyTimeSpan? updateTimestamp)
		{
			using (m_lock.Acquire())
			{
				if (updateTimestamp.HasValue)
				{
					m_inputRenderMessages.CurrentUpdateFrame.UpdateTimestamp = updateTimestamp.Value;
				}
				m_inputRenderMessages.CommitUpdateFrame(m_lock);
				m_inputBillboards.CommitWrite();
				m_inputBillboards.Write.Clear();
				m_inputTriangleBillboards.CommitWrite();
				m_inputTriangleBillboards.Write.Clear();
			}
		}

		public void BeforeRender(MyTimeSpan? currentDrawTime)
		{
			using (m_lock.Acquire())
			{
				m_persistentBillboards.ApplyChanges();
				if (currentDrawTime.HasValue)
				{
					MyRenderProxy.CurrentDrawTime = currentDrawTime.Value;
				}
			}
		}

		public MyUpdateFrame GetRenderFrame(out bool isPreFrame)
		{
			using (m_lock.Acquire())
			{
				MyUpdateFrame renderFrame = m_inputRenderMessages.GetRenderFrame(out isPreFrame);
				if (!isPreFrame)
				{
					m_inputBillboards.RefreshRead();
					m_inputTriangleBillboards.RefreshRead();
				}
				return renderFrame;
			}
		}

		public void ReturnPreFrame(MyUpdateFrame frame)
		{
			m_inputRenderMessages.ReturnPreFrame(frame);
		}

		public void AfterRender()
		{
			using (m_lock.Acquire())
			{
				m_outputVisibleObjects.CommitWrite();
				m_outputVisibleObjects.Write.Clear();
			}
		}

		public MyBillboard AddPersistentBillboard()
		{
			MyBillboard myBillboard = new MyBillboard();
			m_persistentBillboards.Add(myBillboard);
			return myBillboard;
		}

		public void RemovePersistentBillboard(MyBillboard billboard)
		{
			m_persistentBillboards.Remove(billboard);
		}

		public void ApplyActionOnPersistentBillboards(Action<MyBillboard> action)
		{
			foreach (MyBillboard persistentBillboard in m_persistentBillboards)
			{
				action(persistentBillboard);
			}
		}
	}
}
