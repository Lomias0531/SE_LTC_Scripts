using Sandbox.Game.Contracts;
using VRage.Game.Definitions.SessionComponents;
using VRage.Library.Utils;

namespace Sandbox.Game.World.Generator
{
	public abstract class MyContractTypeBaseStrategy
	{
		public static readonly int TICKS_TO_LIVE = 1;

		protected MySessionComponentEconomyDefinition m_economyDefinition;

		public MyContractTypeBaseStrategy(MySessionComponentEconomyDefinition economyDefinition)
		{
			m_economyDefinition = economyDefinition;
		}

		public abstract MyContractCreationResults GenerateContract(out MyContract contract, long factionId, long stationId, MyMinimalPriceCalculator calculator, MyTimeSpan now);

		public abstract bool CanBeGenerated(MyStation station, MyFaction faction);
	}
}
