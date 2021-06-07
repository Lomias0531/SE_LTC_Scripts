namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Types of block integrity change that can occur
	/// </summary>
	public enum MyIntegrityChangeEnum
	{
		Damage,
		ConstructionBegin,
		ConstructionEnd,
		ConstructionProcess,
		DeconstructionBegin,
		DeconstructionEnd,
		DeconstructionProcess,
		Repair
	}
}
