using System;

namespace VRage
{
	public interface IAfterMath
	{
		int Init(IntPtr device);

		void Shutdown();

		string GetInfo(IntPtr context);

		void SetEventMarker(IntPtr nativePointer, string tag);
	}
}
