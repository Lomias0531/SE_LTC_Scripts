namespace VRageMath
{
	public struct MyShort4
	{
		public short X;

		public short Y;

		public short Z;

		public short W;

		public unsafe static explicit operator ulong(MyShort4 val)
		{
			return (ulong)(*(long*)(&val));
		}

		public unsafe static explicit operator MyShort4(ulong val)
		{
			return *(MyShort4*)(&val);
		}
	}
}
