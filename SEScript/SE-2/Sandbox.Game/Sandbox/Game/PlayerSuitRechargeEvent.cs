using Sandbox.Game.GameSystems;
using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		false,
		false
	})]
	public delegate void PlayerSuitRechargeEvent(long playerId, MyLifeSupportingBlockType blockType);
}
