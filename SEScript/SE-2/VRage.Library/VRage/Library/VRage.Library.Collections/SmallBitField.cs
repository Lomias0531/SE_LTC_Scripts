namespace VRage.Library.Collections
{
	/// <summary>
	/// Bit field with up to 64 bits.
	/// </summary>
	public struct SmallBitField
	{
		public const int BitCount = 64;

		public const ulong BitsEmpty = 0uL;

		public const ulong BitsFull = ulong.MaxValue;

		public static readonly SmallBitField Empty = new SmallBitField(value: false);

		public static readonly SmallBitField Full = new SmallBitField(value: true);

		public ulong Bits;

		public bool this[int index]
		{
			get
			{
				return ((Bits >> index) & 1) != 0;
			}
			set
			{
				if (value)
				{
					Bits |= (uint)(1 << index);
				}
				else
				{
					Bits &= (uint)(~(1 << index));
				}
			}
		}

		public SmallBitField(bool value)
		{
			Bits = (ulong)(value ? (-1) : 0);
		}

		public void Reset(bool value)
		{
			Bits = (ulong)(value ? (-1) : 0);
		}
	}
}
