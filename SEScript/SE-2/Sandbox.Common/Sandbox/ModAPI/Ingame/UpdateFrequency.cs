using System;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Flags set how often the script will run itself.
	/// </summary>
	[Flags]
	public enum UpdateFrequency : byte
	{
		/// <summary>
		/// Does not run autonomously.
		/// </summary>
		None = 0x0,
		/// <summary>
		/// Run every game tick.
		/// </summary>
		Update1 = 0x1,
		/// <summary>
		/// Run every 10th game tick.
		/// </summary>
		Update10 = 0x2,
		/// <summary>
		/// Run every 100th game tick.
		/// </summary>
		Update100 = 0x4,
		/// <summary>
		/// Run once before the next tick. Flag is un-set automatically after the update
		/// </summary>
		Once = 0x8
	}
}
