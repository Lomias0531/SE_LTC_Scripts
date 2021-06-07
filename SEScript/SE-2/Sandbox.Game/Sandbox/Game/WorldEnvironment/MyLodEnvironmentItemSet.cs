using System.Collections.Generic;

namespace Sandbox.Game.WorldEnvironment
{
	public struct MyLodEnvironmentItemSet
	{
		public List<int> Items;

		public unsafe fixed int LodOffsets[16];
	}
}
