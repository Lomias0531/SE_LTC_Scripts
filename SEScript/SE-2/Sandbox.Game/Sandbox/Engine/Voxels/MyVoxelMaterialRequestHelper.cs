using System;

namespace Sandbox.Engine.Voxels
{
	internal class MyVoxelMaterialRequestHelper
	{
		public struct ContouringFlagsProxy : IDisposable
		{
			private bool oldState;

			public void Dispose()
			{
				WantsOcclusion = false;
				IsContouring = false;
			}
		}

		[ThreadStatic]
		public static bool WantsOcclusion;

		[ThreadStatic]
		public static bool IsContouring;

		public static ContouringFlagsProxy StartContouring()
		{
			WantsOcclusion = true;
			IsContouring = true;
			return default(ContouringFlagsProxy);
		}
	}
}
