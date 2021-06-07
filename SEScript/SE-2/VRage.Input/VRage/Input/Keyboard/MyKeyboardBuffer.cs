namespace VRage.Input.Keyboard
{
	public struct MyKeyboardBuffer
	{
		public unsafe fixed byte Data[32];

		public unsafe void SetBit(byte bit, bool value)
		{
			if (bit != 0)
			{
				int num = (int)bit % 8;
				byte b = (byte)(1 << num);
				fixed (byte* ptr = Data)
				{
					if (value)
					{
						byte* intPtr = ptr + (int)bit / 8;
						*intPtr = (byte)(*intPtr | b);
					}
					else
					{
						byte* intPtr2 = ptr + (int)bit / 8;
						*intPtr2 = (byte)(*intPtr2 & (byte)(~b));
					}
				}
			}
		}

		public unsafe bool AnyBitSet()
		{
			fixed (byte* ptr = Data)
			{
				long* ptr2 = (long*)ptr;
				return *ptr2 + ptr2[1] + ptr2[2] + ptr2[3] != 0;
			}
		}

		public unsafe bool GetBit(byte bit)
		{
			int num = (int)bit % 8;
			byte b = (byte)(1 << num);
			fixed (byte* ptr = Data)
			{
				return (ptr[(int)bit / 8] & b) != 0;
			}
		}
	}
}
