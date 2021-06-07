using System.Collections.Generic;

namespace VRage.Render.Scene
{
	public struct CullData
	{
		public int IterationOffset;

		public int ActiveActorsLastFrame;

		public MyCullResultsBase ActiveResults;

		public List<MyBruteCullData> CulledActors;

		public List<MyBruteCullData> ActiveActors;

		public static CullData Create()
		{
			CullData result = default(CullData);
			result.IterationOffset = 0;
			result.ActiveResults = null;
			result.ActiveActorsLastFrame = 0;
			result.CulledActors = new List<MyBruteCullData>();
			result.ActiveActors = new List<MyBruteCullData>();
			return result;
		}
	}
}
