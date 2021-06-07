using System;

namespace VRage.Game
{
	public enum MyRelationsBetweenFactions
	{
		Neutral,
		Enemies,
		[Obsolete("Not used in our code, it's here for backwards compatibility")]
		Allies,
		Friends
	}
}
