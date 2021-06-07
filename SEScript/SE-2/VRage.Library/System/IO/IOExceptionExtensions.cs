using System.Runtime.InteropServices;

namespace System.IO
{
	public static class IOExceptionExtensions
	{
		public static bool IsFileLocked(this IOException e)
		{
			int num = Marshal.GetHRForException(e) & 0xFFFF;
			if (num != 32)
			{
				return num == 33;
			}
			return true;
		}
	}
}
