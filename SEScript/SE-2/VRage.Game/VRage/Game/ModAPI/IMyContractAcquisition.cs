namespace VRage.Game.ModAPI
{
	public interface IMyContractAcquisition : IMyContract
	{
		long EndBlockId
		{
			get;
		}

		MyDefinitionId ItemTypeId
		{
			get;
		}

		int ItemAmount
		{
			get;
		}
	}
}
