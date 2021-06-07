using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyLoadAISchoolListResult : MyLoadListResult
	{
		protected override List<Tuple<string, MyWorldInfo>> GetAvailableSaves()
		{
			return MyLocalCache.GetAvailableAISchoolInfos();
		}
	}
}
