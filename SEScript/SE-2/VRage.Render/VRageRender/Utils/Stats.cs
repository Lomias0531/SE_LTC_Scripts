using VRage.Stats;

namespace VRageRender.Utils
{
	public static class Stats
	{
		public struct MyPerAppLifetime
		{
			public int MyModelsCount;

			public int MyModelsMeshesCount;

			public int MyModelsVertexesCount;

			public int MyModelsTrianglesCount;
		}

		public static readonly MyStats Timing;

		public static readonly MyStats Generic;

		public static readonly MyStats Network;

		public static MyPerAppLifetime PerAppLifetime;

		static Stats()
		{
			Timing = new MyStats();
			Generic = MyRenderStats.Generic;
			Network = new MyStats();
			MyRenderStats.SetColumn(MyRenderStats.ColumnEnum.Left, Timing, Generic, Network);
		}
	}
}
