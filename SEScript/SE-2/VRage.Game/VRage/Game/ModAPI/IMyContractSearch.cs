namespace VRage.Game.ModAPI
{
	public interface IMyContractSearch : IMyContract
	{
		long TargetGridId
		{
			get;
		}

		double SearchRadius
		{
			get;
		}
	}
}
