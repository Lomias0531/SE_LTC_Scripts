using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[]
	{
		true,
		false
	})]
	public delegate void PrefabSpawnedEvent(long entityId, string prefabName);
}
