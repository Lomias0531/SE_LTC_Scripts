using System;

namespace VRage
{
	public interface IPacketData
	{
		byte[] Data
		{
			get;
		}

		IntPtr Ptr
		{
			get;
		}

		int Size
		{
			get;
		}

		int Offset
		{
			get;
		}

		void Return();
	}
}
