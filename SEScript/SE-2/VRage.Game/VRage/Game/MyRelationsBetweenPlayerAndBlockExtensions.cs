namespace VRage.Game
{
	public static class MyRelationsBetweenPlayerAndBlockExtensions
	{
		public static bool IsFriendly(this MyRelationsBetweenPlayerAndBlock relations)
		{
			if (relations != 0 && relations != MyRelationsBetweenPlayerAndBlock.Owner)
			{
				return relations == MyRelationsBetweenPlayerAndBlock.FactionShare;
			}
			return true;
		}
	}
}
