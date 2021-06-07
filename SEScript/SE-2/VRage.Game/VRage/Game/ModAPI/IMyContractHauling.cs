namespace VRage.Game.ModAPI
{
	public interface IMyContractHauling : IMyContract
	{
		long EndBlockId
		{
			get;
		}
	}
}
