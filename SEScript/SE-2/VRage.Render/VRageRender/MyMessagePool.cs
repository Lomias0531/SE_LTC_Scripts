using System;
using System.Collections.Generic;
using VRage.Collections;
using VRageRender.Messages;

namespace VRageRender
{
	/// <summary>
	/// TODO: This should use some better sync, it could introduce delays with current state
	/// 1) Use spin lock
	/// 2) Lock only queue, not whole dictionary
	/// 3) Test count first and when it's insufficient, create new message, both should be safe to do out of any lock
	/// 4) Custom consumer/producer non-locking (except resize) queue could be better (maybe overkill)
	/// </summary>
	public class MyMessagePool : Dictionary<int, MyConcurrentQueue<MyRenderMessageBase>>
	{
		public MyMessagePool()
		{
			foreach (MyRenderMessageEnum value in Enum.GetValues(typeof(MyRenderMessageEnum)))
			{
				Add((int)value, new MyConcurrentQueue<MyRenderMessageBase>());
			}
		}

		public void Clear(MyRenderMessageEnum message)
		{
			base[(int)message].Clear();
		}

		public T Get<T>(MyRenderMessageEnum renderMessageEnum) where T : MyRenderMessageBase, new()
		{
			if (!base[(int)renderMessageEnum].TryDequeue(out MyRenderMessageBase instance))
			{
				instance = new T();
			}
			instance.Init();
			return (T)instance;
		}

		public void Return(MyRenderMessageBase message)
		{
			if (!message.IsPersistent)
			{
				MyConcurrentQueue<MyRenderMessageBase> myConcurrentQueue = base[(int)message.MessageType];
				message.Close();
				myConcurrentQueue.Enqueue(message);
			}
		}
	}
}
