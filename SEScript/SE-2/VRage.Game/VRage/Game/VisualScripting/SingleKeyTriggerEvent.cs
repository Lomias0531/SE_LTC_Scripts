namespace VRage.Game.VisualScripting
{
	[VisualScriptingEvent(new bool[]
	{
		true
	})]
	public delegate void SingleKeyTriggerEvent(string triggerName, long playerId);
}
