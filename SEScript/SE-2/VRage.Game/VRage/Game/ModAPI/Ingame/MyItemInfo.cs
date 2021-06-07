using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
	/// <summary>
	/// Accessible via `GetItemInfo` extension method on `MyItemType`
	/// </summary>
	public struct MyItemInfo
	{
		/// <summary>
		/// Mass in Kg
		/// </summary>
		public float Mass;

		/// <summary>
		/// Size in meters
		/// </summary>
		public Vector3 Size;

		/// <summary>
		/// Volume in m^3
		/// </summary>
		public float Volume;

		/// <summary>
		/// Max possible stacks of item in single inventory slot
		/// </summary>
		public MyFixedPoint MaxStackAmount;

		/// <summary>
		/// Ores and ingots are fractional. Ordinary items are not
		/// </summary>
		public bool UsesFractions;

		public bool IsOre;

		public bool IsIngot;

		public bool IsComponent;

		public bool IsTool;

		public bool IsAmmo;
	}
}
