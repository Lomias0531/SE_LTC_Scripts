using System;
using VRage.Library.Collections;

namespace VRage
{
	public abstract class MyPacketDataBitStreamBase : IPacketData
	{
		private BitStream m_stream = new BitStream();

		protected bool m_returned;

		public BitStream Stream => m_stream;

		public abstract byte[] Data
		{
			get;
		}

		public abstract IntPtr Ptr
		{
			get;
		}

		public abstract int Size
		{
			get;
		}

		public abstract int Offset
		{
			get;
		}

		protected MyPacketDataBitStreamBase()
		{
			m_stream.ResetWrite();
		}

		public abstract void Return();

		public void Dispose()
		{
			m_stream.Dispose();
			m_stream = null;
		}
	}
}
