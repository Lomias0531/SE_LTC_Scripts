using System.Runtime.InteropServices;

namespace VRage.Library.Utils
{
	public static class BlittableHelper<T>
	{
		public static readonly bool IsBlittable;

		static BlittableHelper()
		{
			try
			{
				if (default(T) != null)
				{
					GCHandle.Alloc(default(T), GCHandleType.Pinned).Free();
					IsBlittable = true;
				}
			}
			catch
			{
			}
		}
	}
}
