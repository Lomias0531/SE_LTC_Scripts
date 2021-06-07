using VRage.Game.VisualScripting;
using VRageMath;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		true,
		false,
		false,
		false
	})]
	public delegate void ItemSpawnedEvent(string itemTypeName, string itemSubTypeName, long itemId, int amount, Vector3D position);
}
