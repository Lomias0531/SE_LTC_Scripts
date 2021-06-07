using VRage.Game;
using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		true,
		false,
		false,
		false,
		false,
		false
	})]
	public delegate void ContractAbandonedEvent(long contractId, MyDefinitionId contractDefinitionId, long acceptingPlayerId, bool isPlayerMade, long startingBlockId, long startingFactionId, long startingStationId);
}
