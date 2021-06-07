using VRage.Network;

namespace VRageRender.Messages
{
	[GenerateActivator]
	public abstract class MyRenderMessageBase
	{
		private int m_ref;

		/// <summary>
		/// Get message class
		/// </summary>
		public abstract MyRenderMessageType MessageClass
		{
			get;
		}

		/// <summary>
		/// Gets message type
		/// </summary>
		public abstract MyRenderMessageEnum MessageType
		{
			get;
		}

		public virtual bool IsPersistent => false;

		public virtual void Close()
		{
			m_ref = 0;
		}

		public virtual void Init()
		{
		}

		public void AddRef()
		{
			m_ref++;
		}

		public void Dispose()
		{
			m_ref--;
			if (m_ref == -1)
			{
				MyRenderProxy.MessagePool.Return(this);
			}
		}
	}
}
