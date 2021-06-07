namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Defines different link types for grid groups.
	/// </summary>
	public enum GridLinkTypeEnum
	{
		/// <summary>
		/// Terminal connections. i.e. rotors, pistons, wheels. Includes Mechanical connections.
		/// </summary>
		Logical,
		/// <summary>
		/// Connections which lock physics. i.e. connectors. Includes Logical and Mechanical connnections.
		/// </summary>
		Physical,
		/// <summary>
		/// Connections which lock physics, but do not connect terminals. Landing gear only.
		/// </summary>
		NoContactDamage,
		/// <summary>
		/// Connections by rotor, piston, suspension.
		/// </summary>
		Mechanical
	}
}
