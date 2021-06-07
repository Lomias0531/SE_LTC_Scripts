namespace System
{
	public struct BoolBlit
	{
		private byte m_value;

		internal BoolBlit(byte value)
		{
			m_value = value;
		}

		public static implicit operator bool(BoolBlit b)
		{
			return b.m_value != 0;
		}

		public static implicit operator BoolBlit(bool b)
		{
			return new BoolBlit((byte)(b ? byte.MaxValue : 0));
		}

		public override string ToString()
		{
			return ((bool)this).ToString();
		}
	}
}
