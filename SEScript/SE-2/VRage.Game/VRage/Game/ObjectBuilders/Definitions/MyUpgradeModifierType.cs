namespace VRage.Game.ObjectBuilders.Definitions
{
	/// <summary>
	/// Upgrade modifier type
	/// </summary>
	public enum MyUpgradeModifierType
	{
		/// <summary>
		/// Multiplies base value of block - to increase value by 50% set <see cref="P:VRage.Game.ObjectBuilders.Definitions.MyUpgradeModuleInfo.Modifier" /> to 1.5
		/// </summary>
		Multiplicative,
		/// <summary>
		/// Adds to base value of block - to increase value by 50% set <see cref="P:VRage.Game.ObjectBuilders.Definitions.MyUpgradeModuleInfo.Modifier" /> to 0.5
		/// </summary>
		Additive
	}
}
