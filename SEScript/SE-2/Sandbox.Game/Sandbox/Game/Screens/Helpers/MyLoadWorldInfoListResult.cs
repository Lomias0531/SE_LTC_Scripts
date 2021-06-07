using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyLoadWorldInfoListResult : MyLoadListResult
	{
		public MyLoadWorldInfoListResult(string customPath = null)
			: base(customPath)
		{
		}

		protected override List<Tuple<string, MyWorldInfo>> GetAvailableSaves()
		{
			return MyLocalCache.GetAvailableWorldInfos(CustomPath);
		}
	}
}
